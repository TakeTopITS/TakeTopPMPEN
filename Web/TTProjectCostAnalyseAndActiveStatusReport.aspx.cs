using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.SqlClient;


using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProjectCostAnalyseAndActiveStatusReport : System.Web.UI.Page
{
    string strLangCode, strUserCode;
    string strProjectID;

    DateTime dtStartDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        strLangCode = Session["LangCode"].ToString().Trim();
        strUserCode = Session["UserCode"].ToString().Trim();

        strProjectID = Request.QueryString["ProjectID"];

        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            Project project = ShareClass.GetProject(strProjectID);
            LB_ProjectCode.Text = project.ProjectCode.Trim();
            LB_ProjectName.Text = project.ProjectName.Trim();
            LB_ReportTime.Text = DateTime.Now.ToString("yyyy-MM-dd");


            //����ͬ����ֵ
            //��ͬ���һ�Ρ�
            LB_InitialConstractAmount.Text = GetInitialConstractAmountBeforTax(strProjectID);
            LB_InitialConstractTaxRate.Text = GetInitialConstractTaxRate(strProjectID);
            LB_InitialConstractTaxAmount.Text = GetInitialConstractTaxAmount(strProjectID);
            LB_InitialConstractAfterTaxAmount.Text = (decimal.Parse(LB_InitialConstractAmount.Text) + decimal.Parse(LB_InitialConstractTaxAmount.Text)).ToString("f6");
            LB_InitialConstractMainContent.Text = GetInitialConstractMainContent(strProjectID);
            LB_InitialConstractException.Text = GetInitialConstractException(strProjectID);

            //��ͬ������һ��
            LB_SupplementConstractAmount.Text = (decimal.Parse(GetSupplementConstractAmountBeforTax(strProjectID)) + decimal.Parse(GetConstractAmountAfterChange(strProjectID))).ToString("f6");
            LB_SupplementConstractTaxAmount.Text = (decimal.Parse(GetSupplementConstractTaxAmount(strProjectID)) + decimal.Parse(GetSupplementConstractChangeTaxAmount(strProjectID))).ToString("f6");
            LB_SupplementConstractAfterTaxAmount.Text = (decimal.Parse(LB_SupplementConstractAmount.Text) + decimal.Parse(LB_SupplementConstractTaxAmount.Text)).ToString("f6");
            try
            {
                LB_SupplementConstractTaxRate.Text = GetInitialConstractTaxRate(strProjectID);
                //LB_SupplementConstractTaxRate.Text = (decimal.Parse(LB_SupplementConstractTaxAmount.Text) / decimal.Parse(LB_SupplementConstractAfterTaxAmount.Text)).ToString("f6");
            }
            catch
            {
                LB_SupplementConstractTaxRate.Text = "0";
            }

            //��ͬ�����Ρ�
            LB_InitialSecondConstractAmount.Text = LB_InitialConstractAmount.Text;
            LB_InitialSecondConstractTaxRate.Text = GetInitialConstractTaxRate(strProjectID);
            LB_InitialSecondConstractTaxAmount.Text = GetInitialConstractTaxAmount(strProjectID);
            LB_InitialSecondConstractAfterTaxAmount.Text = (decimal.Parse(LB_InitialConstractAmount.Text) + decimal.Parse(LB_InitialConstractTaxAmount.Text)).ToString("f6");

            //��ͬ���������
            LB_ConstractSecondSupplementAmount.Text = LB_SupplementConstractAmount.Text;
            LB_ConstractSecondSupplementTaxRate.Text = LB_SupplementConstractTaxRate.Text;
            LB_ConstractSecondSupplementTaxAmount.Text = LB_SupplementConstractTaxAmount.Text;
            LB_ConstractSecondSupplementAfterTaxAmount.Text = LB_SupplementConstractAfterTaxAmount.Text;

            if (Request.QueryString["StartDate"] != null)
            {
                dtStartDate = DateTime.Parse(Request.QueryString["StartDate"]);
                MonthPicker1.StartDate = dtStartDate;
                LB_YearMonth.Text = DateTime.Parse(MonthPicker1.StartDate.ToString()).ToString("yyyyMM");

                SetReport(LB_YearMonth.Text);
            }
            else
            {
                MonthPicker1.StartDate = DateTime.Now;
                LB_YearMonth.Text = DateTime.Parse(MonthPicker1.StartDate.ToString()).ToString("yyyyMM");
            }

            string strYearNumber = LB_YearMonth.Text.Substring(0, 4);
            string strMonthNumber = LB_YearMonth.Text.Substring(4, 2);
            LB_ReportYearMonth.Text = strYearNumber + LanguageHandle.GetWord("Nian").ToString().Trim() + strMonthNumber + LanguageHandle.GetWord("Yue").ToString().Trim();
        }
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        Response.Redirect("TTProjectCostAnalyseAndActiveStatusReport.aspx?ProjectID=" + strProjectID + "&StartDate=" + MonthPicker1.StartDate.ToString());
    }

    protected void SetReport(string strYearMonth)
    {
        try
        {
            //"����Ԥ���ѱ��������"���С�����������ϼơ���˰ǰ��
            LB_RGSYGeXiangJiangLi.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sq", "my:riqi", strYearMonth);

            //"����Ԥ���ѱ��������"���С�����������ϼơ���˰�ʡ�
            LB_gexiangjiangli_slv.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-slv", "my:riqi", strYearMonth);

            //"����Ԥ���ѱ��������"���С�����������ϼơ���˰��
            LB_gexiangjiangli_sj.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sj", "my:riqi", strYearMonth);

            //"����Ԥ���ѱ��������"���С�����������ϼơ���˰���
            LB_gexiangjiangli_sh.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sh", "my:riqi", strYearMonth);



            ////������Ԥ���ѱ�����������С�����Ԥ���ѡ���������˰������Ԫ������ݶ�ȡ
            //LB_BuKeYuJianFei.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:bukeyujianfei-sq", "my:riqi", strYearMonth);

            ////������Ԥ���ѱ�����������С�����Ԥ���ѡ���˰�ʡ���Ԫ������ݶ�ȡ
            //LB_BuKeYuJianFeiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:bukeyujianfei-slv", "my:riqi", strYearMonth);

            ////������Ԥ���ѱ�����������С�����Ԥ���ѡ���˰�𡱵�Ԫ������ݶ�ȡ
            //LB_BuKeYuJianFeiTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:bukeyujianfei-sj", "my:riqi", strYearMonth);

            ////������Ԥ���ѱ�����������С�����Ԥ���ѡ���˰�����Ԫ������ݶ�ȡ
            //LB_BuKeYuJianFeiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:bukeyujianfei-sh", "my:riqi", strYearMonth);



            //����Ŀһ�α�۷�����С�����Ԥ���ѡ���������˰������Ԫ������ݶ�ȡ
            LB_BuKeYuJianFei.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:B8", "my:riqi", strYearMonth);

            //����Ŀһ�α�۷�����С�����Ԥ���ѡ���˰�ʡ���Ԫ������ݶ�ȡ
            LB_BuKeYuJianFeiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field37", "my:riqi", strYearMonth);

            //����Ŀһ�α�۷�����С�����Ԥ���ѡ���˰�𡱵�Ԫ������ݶ�ȡ
            LB_BuKeYuJianFeiTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field38", "my:riqi", strYearMonth);

            //����Ŀһ�α�۷�����С�����Ԥ���ѡ���˰�����Ԫ������ݶ�ȡ
            LB_BuKeYuJianFeiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field39", "my:riqi", strYearMonth);



            //Ĭ�ϴӱ�������Ŀһ�α�۷�����С�Ԥ��������Ԫ������ݣ��������������ݣ��ӡ���Ŀ���ģ���ȡ
            LB_FirstYiJiShenJianE.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:C", "my:riqi", strYearMonth);

            //Ĭ�ϴӱ�������Ŀһ�α�۷�����С�Ԥ������-��˰�ʡ���Ԫ�������
            LB_FirstYiJiShenJianETaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field45", "my:riqi", strYearMonth);

            //Ĭ�ϴӱ�������Ŀһ�α�۷�����С�Ԥ������-��˰�𡱵�Ԫ�������
            LB_FirstYiJiShenJianETaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field46", "my:riqi", strYearMonth);

            //Ĭ�ϴӱ�������Ŀһ�α�۷�����С�Ԥ������-��˰�����Ԫ�������
            LB_FirstYiJiShenJianEAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field47", "my:riqi", strYearMonth);



            //����ʣ����������ڴ˸���ֵ=Ԥ������һ�Σ�/��ͬ��˰ǰ���� C11/C7��
            try
            {
                LB_FirstYiJiShenJianRate.Text = (decimal.Parse(LB_FirstYiJiShenJianE.Text) / decimal.Parse(LB_InitialConstractAmount.Text)).ToString("f6");
            }
            catch
            {
                LB_FirstYiJiShenJianRate.Text = "0";
            }
            //��������ϼ�C7+C8+C9+C10-C11������1-5��ϼ��Զ���������
            decimal deYiJiZongShouRuHeJi = decimal.Parse(LB_InitialConstractAmount.Text) + decimal.Parse(LB_SupplementConstractAmount.Text) + decimal.Parse(LB_RGSYGeXiangJiangLi.Text) + decimal.Parse(LB_BuKeYuJianFei.Text);
            deYiJiZongShouRuHeJi -= decimal.Parse(LB_FirstYiJiShenJianE.Text);
            LB_YiJiHeTongYiShuanJiaZongShouRuHeJi.Text = deYiJiZongShouRuHeJi.ToString("f6");

            //Ԥ��������ϼ�E7+E8+E9+E10-E11
            decimal deYiJiTaxAmountZongShouRuHeJi = decimal.Parse(LB_InitialConstractTaxAmount.Text) + decimal.Parse(LB_SupplementConstractTaxAmount.Text) + decimal.Parse(LB_gexiangjiangli_sj.Text) + decimal.Parse(LB_BuKeYuJianFeiTaxAmount.Text);
            deYiJiTaxAmountZongShouRuHeJi -= decimal.Parse(LB_FirstYiJiShenJianETaxAmount.Text);
            LB_YiJiTaxAmountZongShouRuHeJi.Text = deYiJiTaxAmountZongShouRuHeJi.ToString("f6");

            //Ԥ��������ϼ�E12/C12
            try
            {
                LB_YiJiTaxRateZongShouRuHeJi.Text = (decimal.Parse(LB_YiJiTaxAmountZongShouRuHeJi.Text) / decimal.Parse(LB_YiJiHeTongYiShuanJiaZongShouRuHeJi.Text)).ToString("f6");
            }
            catch
            {
                LB_YiJiTaxRateZongShouRuHeJi.Text = "0";
            }

            //Ԥ��������ϼ�F7+F8+F9+F10-F11
            decimal deYiJiHeTongYiShuanJiaAfterTaxZongShouRuHeJi = decimal.Parse(LB_InitialConstractAfterTaxAmount.Text) + decimal.Parse(LB_SupplementConstractAfterTaxAmount.Text) + decimal.Parse(LB_gexiangjiangli_sh.Text) + decimal.Parse(LB_BuKeYuJianFeiAfterTaxAmount.Text);
            deYiJiHeTongYiShuanJiaAfterTaxZongShouRuHeJi -= decimal.Parse(LB_FirstYiJiShenJianEAfterTaxAmount.Text);
            LB_YiJiHeTongYiShuanJiaAfterTaxZongShouRuHeJi.Text = deYiJiHeTongYiShuanJiaAfterTaxZongShouRuHeJi.ToString("f6");

            //��������Ŀһ�α�۷�����С�������˰�����˹���
            LB_XiangMuFirstCiFengLiBiaoRenGongFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D1", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����˹���˰��
            LB_XiangMuFirstCiFengLiBiaoRenGongFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field52", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����˹���˰��
            LB_XiangMuFirstCiFengLiBiaoRenGongFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field53", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����˹���˰����
            LB_XiangMuFirstCiFengLiBiaoRenGongFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field54", "my:riqi", strYearMonth);


            //��������Ŀһ�α�۷�����С�������˰�����豸��
            LB_XiangMuFirstCiFengLiBiaoSheBeiFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D2", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����豸��˰��
            LB_XiangMuFirstCiFengLiBiaoSheBeiFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field56", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����豸��˰��
            LB_XiangMuFirstCiFengLiBiaoSheBeiFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field57", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����豸��˰����
            LB_XiangMuFirstCiFengLiBiaoSheBeiFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field58", "my:riqi", strYearMonth);


            //��������Ŀһ�α�۷�����С�������˰�������Ϸ�
            LB_XiangMuFirstCiFengLiBiaoCaiLiaoFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D3", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�������Ϸ�˰��
            LB_XiangMuFirstCiFengLiBiaoCaiLiaoFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field60", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�������Ϸ�˰��
            LB_XiangMuFirstCiFengLiBiaoCaiLiaoFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field61", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�������Ϸ�˰����
            LB_XiangMuFirstCiFengLiBiaoCaiLiaoFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field62", "my:riqi", strYearMonth);


            //��������Ŀһ�α�۷�����С�������˰������еʹ�÷�
            LB_XiangMuFirstCiFengLiBiaoJiJieShiYongFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D4", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������еʹ�÷�˰��
            LB_XiangMuFirstCiFengLiBiaoJiJieShiYongFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field64", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������еʹ�÷�˰��
            LB_XiangMuFirstCiFengLiBiaoJiJieShiYongFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field65", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������еʹ�÷�˰����
            LB_XiangMuFirstCiFengLiBiaoJiJieShiYongFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field66", "my:riqi", strYearMonth);



            //��������Ŀһ�α�۷�����С�������˰�����ְ��ɱ�
            LB_XiangMuFirstCiFengLiBiaoFenBaoChengBen.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D5D51D52", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����ְ��ɱ�˰��
            LB_XiangMuFirstCiFengLiBiaoFenBaoChengBenTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field68", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����ְ��ɱ�˰��
            LB_XiangMuFirstCiFengLiBiaoFenBaoChengBenTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field69", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����ְ��ɱ�˰����
            LB_XiangMuFirstCiFengLiBiaoFenBaoChengBenAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field70", "my:riqi", strYearMonth);



            //��������Ŀһ�α�۷�����С�������˰�����������̷�
            LB_XiangMuFirstCiFengLiBiaoJianZuoGongChengFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D51", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����������̷�˰��
            LB_XiangMuFirstCiFengLiBiaoJianZuoGongChengFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field72", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����������̷�˰��
            LB_XiangMuFirstCiFengLiBiaoJianZuoGongChengFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field73", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰�����������̷�˰����
            LB_XiangMuFirstCiFengLiBiaoJianZuoGongChengFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field74", "my:riqi", strYearMonth);


            //��������Ŀһ�α�۷�����С�������˰������װ���̷�
            LB_XiangMuFirstCiFengLiBiaoAnZhangGongChengFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D52", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������װ���̷�˰��
            LB_XiangMuFirstCiFengLiBiaoAnZhangGongChengFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field76", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������װ���̷�˰��
            LB_XiangMuFirstCiFengLiBiaoAnZhangGongChengFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field77", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������װ���̷�˰����
            LB_XiangMuFirstCiFengLiBiaoAnZhangGongChengFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field78", "my:riqi", strYearMonth);


            //��������Ŀһ�α�۷�����С�������˰������������
            LB_XiangMuFirstCiFengLiBiaoQiTaFeiYong.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D6", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������������˰��
            LB_XiangMuFirstCiFengLiBiaoQiTaFeiYongTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field80", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������������˰��
            LB_XiangMuFirstCiFengLiBiaoQiTaFeiYongTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field81", "my:riqi", strYearMonth);

            //��������Ŀһ�α�۷�����С�������˰������������˰����
            LB_XiangMuFirstCiFengLiBiaoQiTaFeiYongAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field82", "my:riqi", strYearMonth);


            //��������Ŀһ�α�۷�����С�������˰������ֵ˰����
            LB_XiangMuFirstCiFengLiBiaoZengChiShuiFuJia.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D7F12", "my:riqi", strYearMonth);


            //"����Ԥ���ѱ��������"���С�����������ϼơ�����˰ǰ����
            LB_BuKeYiJianBiaoGeXiangJiangLiHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sq", "my:riqi", strYearMonth); 
            LB_BuKeYiJianBiaoGeXiangJiangLiHeJiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID ,LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-slv", "my:riqi", strYearMonth);
            LB_BuKeYiJianBiaoGeXiangJiangLiHeJiTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sj", "my:riqi", strYearMonth);
            LB_BuKeYiJianBiaoGeXiangJiangLiHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sh", "my:riqi", strYearMonth);

            //����ġ���Ŀ���α�۷�����С�����Ԥ���ѡ���������˰������Ԫ������ݶ�ȡ
            LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field159", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field166", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field193", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field173", "my:riqi", strYearMonth);

            //Ĭ�ϴӱ�������Ŀ���α�۷�����С�Ԥ��������Ԫ������ݣ��������������ݣ��ӡ���Ŀ���ģ���ȡ��
            LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianE.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field160", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianETaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field167", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianETaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field194", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianEAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field174", "my:riqi", strYearMonth);
            try
            {
                LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianLu.Text = (decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianE.Text) / decimal.Parse(LB_InitialSecondConstractAmount.Text)).ToString("f6");
            }
            catch
            {
                LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianLu.Text = "0";
            }

            //����Ԥ��������ϼ�H7+H8+H9+H10-H11������1-5��ϼ��Զ���������
            decimal deSecondYiJiHeTongYiShuanJiaZongShouRuHeJi = decimal.Parse(LB_InitialSecondConstractAmount.Text) + decimal.Parse(LB_ConstractSecondSupplementAmount.Text) + decimal.Parse(LB_BuKeYiJianBiaoGeXiangJiangLiHeJiAmount.Text) + decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiAmount.Text);
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJi -= decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianE.Text);
            LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text = deSecondYiJiHeTongYiShuanJiaZongShouRuHeJi.ToString();

            //����Ԥ��������ϼ�J7+J8+J9+J10-J11
            decimal deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount;
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount = decimal.Parse(LB_InitialSecondConstractTaxAmount.Text) + decimal.Parse(LB_ConstractSecondSupplementTaxAmount.Text) + decimal.Parse(LB_BuKeYiJianBiaoGeXiangJiangLiHeJiTaxAmount.Text) + decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiTaxAmount.Text);
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount -= decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianETaxAmount.Text);
            LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount.Text = deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount.ToString();

            //����Ԥ��������ϼ�E12/C12
            try
            {
                LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxRate.Text = (decimal.Parse(LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount.Text) / decimal.Parse(LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text)).ToString("f6");
            }
            catch
            {
                LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxRate.Text = "0";
            }

            //����Ԥ��������ϼ�K7+K8+K9+K10-K11
            decimal deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount;
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount = decimal.Parse(LB_InitialSecondConstractAfterTaxAmount.Text) + decimal.Parse(LB_ConstractSecondSupplementAfterTaxAmount.Text) + decimal.Parse(LB_BuKeYiJianBiaoGeXiangJiangLiHeJiAfterTaxAmount.Text) + decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiAfterTaxAmount.Text);
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount -= decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianEAfterTaxAmount.Text);
            LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount.Text = deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount.ToString();

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֱ���˹��ѵ�����,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieRenGongFeiYongAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:rengongfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieRenGongFeiYongTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field2", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieRenGongFeiYongTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:rengongfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieRenGongFeiYongAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field3", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��빤�ʼ�����յ�Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiGeXiangBaoXianAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:gongzibaoxian", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiGeXiangBaoXianTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field6", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiGeXiangBaoXianTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field197", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiGeXiangBaoXianAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field7", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��뽱��Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiJiangJinAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jiangjin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiJiangJinTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field10", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiJiangJinTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field198", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiJiangJinAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field11", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е�������������Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiQiTaBuZhouAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitabuzhujintie", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiQiTaBuZhouTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field14", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiQiTaBuZhouTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field199", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiQiTaBuZhouAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field15", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е�������������Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiWaiChuJinTieAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:waichubuzhu", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiWaiChuJinTieTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field18", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiWaiChuJinTieTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field200", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiWaiChuJinTieAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field19", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ְ����̷ѵ�Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenBaoGongZhengAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fenbaogognchengfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenBaoGongZhengTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field22", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenBaoGongZhengTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fenbaofeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenBaoGongZhengAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field23", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ְ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBaoAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:laowufenbao", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBaoTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field26", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBaoTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field202", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBaoAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field27", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ְ�1��Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao1Amount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:laowufenbao1", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao1TaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field30", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao1TaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field203", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao1AfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field31", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ְ�2��Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao2Amount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:laowufenbao2", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao2TaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field34", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao2TaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field204", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao2AfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field35", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���רҵ�ְ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiZhuanYeFenBaoAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zhuanyefenbao", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiZhuanYeFenBaoTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field38", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiZhuanYeFenBaoTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field205", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiZhuanYeFenBaoAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field39", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾���굥Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenGongShiZiWanAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fengongsiziwan", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenGongShiZiWanTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field42", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenGongShiZiWanTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field206", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenGongShiZiWanAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field43", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾���ǵ�Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiDianYuAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:dianyi", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiDianYuTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field139", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiDianYuTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field207", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiDianYuAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field140", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾��װ��Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieDiaoZhuangAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:diaozhuang", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieDiaoZhuangTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field143", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieDiaoZhuangTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field208", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieDiaoZhuangAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field144", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾ֱ���Ϸѵ�Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhiJieLiaoFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zhijiecailiaofei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhiJieLiaoFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:cailiaofeishuilv", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhiJieLiaoFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:cailiaofeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhiJieLiaoFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field47", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾���ĵ�Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuCaiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zhucai", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuCaiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field50", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuCaiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field210", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuCaiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field51", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾���ĵ�Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFuCaiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fucai", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFuCaiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field54", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFuCaiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field211", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFuCaiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field55", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾ƽ�ⵥԪ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieCaiLiaoPingKuAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field306", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieCaiLiaoPingKuTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field307", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieCaiLiaoPingKuTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field308", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieCaiLiaoPingKuAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field309", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾���ᵥԪ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieDiQiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field310", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieDiQiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field311", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieDiQiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field312", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieDiQiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field313", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾��е�ѵ�Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jixiefei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field58", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jixiefeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field59", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е���ֹ�˾��еʹ�÷ѵ�Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieShiYongFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jixieshiyonfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieShiYongFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field62", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieShiYongFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field213", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieShiYongFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field63", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾�ֹ�˾����ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFenGongShiZiWanAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:feigongsiziwan", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFenGongShiZiWanTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field66", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFenGongShiZiWanTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field214", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFenGongShiZiWanAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field67", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾��������Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qita", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field70", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field215", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field71", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾��ʱ��ʩ�ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLingShiSheSiFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:linshisheshifei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLingShiSheSiFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field74", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLingShiSheSiFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:linshefeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLingShiSheSiFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field75", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾��ȫ��ʩ�ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieAnQianChuShiFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:anquancuoshifei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieAnQianChuShiFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field78", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieAnQianChuShiFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:anquanfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieAnQianChuShiFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field79", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾ˮ��ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieShuiDianFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:shuidianfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieShuiDianFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field126", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieShuiDianFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:shuidianfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieShuiDianFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field127", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾�������̷ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaGongChengFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitagongchengfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaGongChengFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field130", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaGongChengFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitagongchengfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaGongChengFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field131", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾�����ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitafei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field86", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitafeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field87", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾���ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJianCheFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jiancefei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJianCheFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field90", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJianCheFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field221", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJianCheFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field91", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾��Э�ӹ��ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieWaiXieJiaGongFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:waixiejiagongfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieWaiXieJiaGongFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field94", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieWaiXieJiaGongFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field222", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieWaiXieJiaGongFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field95", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾���޷ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuLingFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zulinfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuLingFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field98", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuLingFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field223", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuLingFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field99", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾�Ͷ������ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLaoDongBaoHuFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:laodongbaohufei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLaoDongBaoHuFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field102", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLaoDongBaoHuFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field224", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLaoDongBaoHuFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field103", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾����ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieXiuLiFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:xiulifei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieXiuLiFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field106", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieXiuLiFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field225", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieXiuLiFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field107", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾����Ԥ���ɱ�����Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieBuKeYiJianChenBenAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:bukeyujian", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieBuKeYiJianChenBenTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field134", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieBuKeYiJianChenBenTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field226", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieBuKeYiJianChenBenAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field135", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾�����Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFaKuanAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fakuan", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFaKuanTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field147", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFaKuanTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field227", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFaKuanAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field148", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾�칫��Ʒ����Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJiePanGongYongPingAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:bangongyongping", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJiePanGongYongPingTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field151", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJiePanGongYongPingTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field228", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJiePanGongYongPingAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field152", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾����2����Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTa2Amount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitafeiqita", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTa2TaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field110", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTa2TaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field229", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTa2AfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field111", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾��Ǩ�ѡ���Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieTongQianFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:dongqianfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieTongQianFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field114", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieTongQianFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:dongqianfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieTongQianFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field115", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾�����������Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaLiangLiXiangAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field302", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaLiangLiXiangTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field303", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaLiangLiXiangTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field304", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaLiangLiXiangAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field305", "my:riqi", strYearMonth);

            //���������α�۷���Ŀ��ɱ���ɱ����¸����������£��뵱ǰ�����������ġ�������˰�����е��롾�ܳɱ�����Ԫ�������,��������Ч����
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field176", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jinxiangshuijin", "my:riqi", strYearMonth);

            try
            {
                LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenTaxRate.Text = (decimal.Parse(LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenTaxAmount.Text) / decimal.Parse(LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenAmount.Text)).ToString("f6");
            }
            catch
            {
                LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenTaxRate.Text = "0";
            }
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenAfterTaxAmount.Text = (decimal.Parse(LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenTaxAmount.Text) + decimal.Parse(LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenAmount.Text)).ToString("f6");

            //Ԥ������������-�ܳɱ���,��H12-I52
            LB_RiRen.Text = (decimal.Parse(LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text) - decimal.Parse(LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenAmount.Text)).ToString();
            LB_XiaoXiangShuiE.Text = LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount.Text;
            LB_JinXiangShuiE.Text = LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenTaxAmount.Text;
            LB_ShuiJin.Text = (decimal.Parse(LB_XiaoXiangShuiE.Text) - decimal.Parse(LB_JinXiangShuiE.Text)).ToString();
            LB_ShuiJinFuJia.Text = ((decimal.Parse(LB_ShuiJin.Text) * 12) / 100).ToString();

            LB_XiangMuZongYuSuan.Text = ShareClass.GetProject(strProjectID).Budget.ToString();

            try
            {
                LB_XiangMuTaxRate.Text = GetWZProject(strProjectID).TaxRate.ToString();
            }
            catch
            {
                LB_XiangMuTaxRate.Text = "0";
            }
            try
            {
                LB_XiangMuTaxAmount.Text = GetWZProject(strProjectID).TaxAmount.ToString();
            }
            catch
            {
                LB_XiangMuTaxAmount.Text = "0";
            }
            LB_XiangMuZongShouRu.Text = (decimal.Parse(LB_XiangMuZongYuSuan.Text) - decimal.Parse(LB_XiangMuTaxAmount.Text)).ToString();

            try
            {
                LB_XiangMuYuSuanZongHeShuiFu.Text = ((decimal.Parse(LB_XiaoXiangShuiE.Text) - decimal.Parse(LB_JinXiangShuiE.Text)) / decimal.Parse(LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text)).ToString("f6");
            }
            catch
            {
                LB_XiangMuYuSuanZongHeShuiFu.Text = "0";
            }


            //******���ƶ���ͬ��ģ�飬����ͬ�������ǩ�¡����ʱ�䡱Ϊ���µ� ������=��������˰�󣩡�����-����ͬ��˰�󣩡�
            LB_XiangMuHeTongBianGenHouAmount.Text = (decimal.Parse(GetConstractCurrentMonthAmountAfterChange(strProjectID)) - decimal.Parse(GetInitialConstractAmountAfterTax(strProjectID))).ToString("f6");
            LB_XiangMuHeTongBianGenHouTaxRate.Text = LB_InitialSecondConstractTaxRate.Text;
            LB_XiangMuHeTongBianGenHouTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBianGenHouAmount.Text) * decimal.Parse(LB_XiangMuHeTongBianGenHouTaxRate.Text)).ToString();

            //******���µĺ�ͬ���б��������������ͬ��� - ���º�ͬ���б������
            decimal deXiangMuHeTongBenYueBianGenJianQiShangYueHeTongAmount = decimal.Parse(GetConstractCurrentMonthAmountAfterChange(strProjectID)) + decimal.Parse(GetConstractCurrentMonthSupplementAmountAfterTax(strProjectID)) - decimal.Parse(GetConstractPirorMonthAmountAfterChange(strProjectID)) - decimal.Parse(GetConstractPriorMonthSupplementAmountAfterTax(strProjectID));
            LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongAmount.Text = deXiangMuHeTongBenYueBianGenJianQiShangYueHeTongAmount.ToString();
            LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongTaxRate.Text = LB_ConstractSecondSupplementTaxRate.Text;
            LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongTaxRate.Text)).ToString();


            //���µ�H9������������ϼơ�����˰ǰ����-���µ�H9������������ϼơ�����˰ǰ����
            LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiAmount.Text = (decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sq")) - decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sq"))).ToString();
            LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiTaxRate.Text = LB_BuKeYiJianBiaoGeXiangJiangLiHeJiTaxRate.Text;
            LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiTaxRate.Text)).ToString();

            //ֵ=���µġ�����Ԥ���ѡ�-���µġ�����Ԥ���ѡ��� ���� H10-���� H10
            LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field159", "my:riqi", strYearMonth)) - decimal.Parse(GetWorkFlowColumnPriorMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field159", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiTaxRate.Text = LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiTaxRate.Text;
            LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiTaxRate.Text)).ToString();

            //ֵ=���µġ�Ԥ������-���µġ�Ԥ������
            LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianEAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field160", "my:riqi", strYearMonth)) - decimal.Parse(GetWorkFlowColumnPriorMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field160", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianETaxRate.Text = LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianETaxRate.Text;
            LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianETaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianEAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianETaxRate.Text)).ToString();

            //ֵ=���µġ�Ԥ��������ϼơ�-���µġ�Ԥ��������ϼơ�
            LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zongshouru", "my:riqi", strYearMonth)) - decimal.Parse(GetWorkFlowColumnPriorMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zongshouru", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuTaxRate.Text = LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxRate.Text;
            LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuTaxRate.Text)).ToString();

            //����ʵ�ʷ����ɱ���˰ǰ���˹��ѱ�-��rengongfeiheji��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenRenGongFeiHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:rengongfeiheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenRenGongFeiHeJiSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:rengongfeiheji");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenRenGongFeiHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBenRenGongFeiHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBenRenGongFeiHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:rengongfeiheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenRenGongFeiHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:rengongfeiheji");
            LB_XiangMuMiYueShiJiFaShengChengBenRenGongFeiHeJiTaxRate.Text = "0";

            //����ʵ�ʷ����ɱ���˰ǰ���˹��ѱ�-��gongzibaoxian��
            LB_XiangMuCurrentMonthShiJiFaShengChengBengongzibaoxianHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:gongzibaoxian", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBengongzibaoxianHeJiAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:gongzibaoxian");
            LB_XiangMuCurrentMonthShiJiFaShengChengBengongzibaoxianHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBengongzibaoxianHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBengongzibaoxianHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:gongzibaoxian", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBengongzibaoxianHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:gongzibaoxian");
            LB_XiangMuMiYueShiJiFaShengChengBengongzibaoxianHeJiTaxRate.Text = "0";

            //����ʵ�ʷ����ɱ���˰ǰ���˹��ѱ�-��gexiangjiangli ��
            LB_XiangMuCurrentMonthShiJiFaShengChengBengexiangjiangliHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:gexiangjiangli", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBengexiangjiangliHeJiAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:gexiangjiangli");
            LB_XiangMuCurrentMonthShiJiFaShengChengBengexiangjiangliHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBengexiangjiangliHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBengexiangjiangliHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:gexiangjiangli", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBengexiangjiangliHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:gexiangjiangli");
            LB_XiangMuMiYueShiJiFaShengChengBengexiangjiangliHeJiTaxRate.Text = "0";

            //����ʵ�ʷ����ɱ���˰ǰ���˹��ѱ�-��qitabuzhu ��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitabuzhuHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:qitabuzhu", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitabuzhuHeJiAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:qitabuzhu");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitabuzhuHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBenqitabuzhuHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitabuzhuHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:qitabuzhu", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitabuzhuHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:qitabuzhu");
            LB_XiangMuMiYueShiJiFaShengChengBenqitabuzhuHeJiTaxRate.Text = "0";


            //����ʵ�ʷ����ɱ���˰ǰ���˹��ѱ�-��waichubuzhu ��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaichubuzhuHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:waichubuzhu", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaichubuzhuHeJiAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:waichubuzhu");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaichubuzhuHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBenwaichubuzhuHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaichubuzhuHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:waichubuzhu", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaichubuzhuHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:waichubuzhu");
            LB_XiangMuMiYueShiJiFaShengChengBenwaichubuzhuHeJiTaxRate.Text = "0";


            //����ʵ�ʷ����ɱ���˰ǰ���ְ��÷ѱ�-laowufenbao1-sq+laowufenbao2-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sq", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqAmount.Text = (decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sq"))).ToString();
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqTaxAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sj", "my:riqi", strYearMonth))).ToString("f6");
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqSumTaxAmount.Text = (decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sj")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sj"))).ToString("f6");

            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqAfterTaxAmount.Text = (decimal.Parse(GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sh", "my:riqi", strYearMonth))).ToString("f6");
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqSumAfterTaxAmount.Text = (decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sh"))).ToString("f6");
            try
            {
                LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqTaxRate.Text = (decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqTaxAmount.Text) / decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqSumAfterTaxAmount.Text)).ToString("f6");
            }
            catch
            {
                LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqlaowufenbao2sqTaxRate.Text = "0";
            }

            //����ʵ�ʷ����ɱ���˰ǰ���ְ��÷ѱ�-fenbaofeiheji��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fenbaofeiheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfenbaofeihejiSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fenbaofeiheji");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fenbaoshuihouheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfenbaofeihejiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fenbaoshuihouheji");
            LB_XiangMuMiYueShiJiFaShengChengBenfenbaofeihejiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fenbaofei-slv", "my:riqi", strYearMonth);

            decimal deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount = decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sj", "my:riqi", strYearMonth));
            deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhaunyefenbao-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fengongsiziwan-sj", "my:riqi", strYearMonth));
            deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-sj", "my:riqi", strYearMonth));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount.Text = deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount.ToString("f6");

            decimal deXiangMuMiYueShiJiFaShengChengBenfenbaofeihejiSumTaxAmount = decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sj"))  + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sj"));
            deXiangMuMiYueShiJiFaShengChengBenfenbaofeihejiSumTaxAmount += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:zhaunyefenbao-sj")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fengongsiziwan-sj"));
            deXiangMuMiYueShiJiFaShengChengBenfenbaofeihejiSumTaxAmount += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:dianyi-sj")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:diaozhuang-sj")) ;
            LB_XiangMuMiYueShiJiFaShengChengBenfenbaofeihejiSumTaxAmount.Text = deXiangMuMiYueShiJiFaShengChengBenfenbaofeihejiSumTaxAmount.ToString("f6");
  

            //����ʵ�ʷ����ɱ���˰ǰ���ְ��÷ѱ�-laowufenbao1-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ���ְ��÷ѱ�-laowufenbao2-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao2sqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao2sqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao2sqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao2sqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao2sqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao2sqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao2sqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ���ְ��÷ѱ�-zhuanyefenbao-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzhuanyefenbaosqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhuanyefenbao-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzhuanyefenbaosqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:zhuanyefenbao-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzhuanyefenbaosqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhuanyefenbao-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzhuanyefenbaosqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:zhuanyefenbao-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzhuanyefenbaosqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhaunyefenbao-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzhuanyefenbaosqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:zhaunyefenbao-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenzhuanyefenbaosqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhuanyefenbao-slv", "my:riqi", strYearMonth);


            //����ʵ�ʷ����ɱ���˰ǰ���ְ��÷ѱ�-fbfengongsiziwan-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfbfengongsiziwansqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fbfengongsiziwan-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfbfengongsiziwansqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fbfengongsiziwan-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfbfengongsiziwansqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fengongsiziwan-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfbfengongsiziwansqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fengongsiziwan-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfbfengongsiziwansqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fengongsiziwan-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfbfengongsiziwansqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fengongsiziwan-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenfbfengongsiziwansqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fengongsiziwan-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ���ְ��÷ѱ�-dianyi-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBendianyisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendianyisqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:dianyi-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendianyisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendianyisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:dianyi-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendianyisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendianyisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:dianyi-sh");
            LB_XiangMuMiYueShiJiFaShengChengBendianyisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ���ְ��÷ѱ�-diaozhuang-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBendiaozhuangsqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendiaozhuangsqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:diaozhuang-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendiaozhuangsqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendiaozhuangsqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:diaozhuang-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendiaozhuangsqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendiaozhuangsqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:diaozhuang-sh");
            LB_XiangMuMiYueShiJiFaShengChengBendiaozhuangsqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-slv", "my:riqi", strYearMonth);


            //����ʵ�ʷ����ɱ���˰ǰ�����Ϸѱ�-cailiaofei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-slv", "my:riqi", strYearMonth);


            //����ʵ�ʷ����ɱ���˰ǰ�����Ϸѱ�-zhucai-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeizhucaisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:zhucai-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeizhucaisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:zhucai-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeizhucaisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:zhucai-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeizhucaisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:zhucai-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeizhucaisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:zhucai-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeizhucaisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:zhucai-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeizhucaisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:zhucai-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�����Ϸѱ�-fucai-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeifucaisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:fucai-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeifucaisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:fucai-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeifucaisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:fucai-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeifucaisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:fucai-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeifucaisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:fucai-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeifucaisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:fucai-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeifucaisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:fucai-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�����Ϸѱ�-pingku-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeipingkusqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:pingku-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeipingkusqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:pingku-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeipingkusqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:pingku-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeipingkusqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:pingku-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeipingkusqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:pingku-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeipingkusqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:pingku-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeipingkusqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:pingku-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�����Ϸѱ�-diqi-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeidiqisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:diqi-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeidiqisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:diqi-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeidiqisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:diqi-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeidiqisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:diqi-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeidiqisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:diqi-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeidiqisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:diqi-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeidiqisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:diqi-slv", "my:riqi", strYearMonth);


            //����ʵ�ʷ����ɱ���˰ǰ����е�豸���޷ѱ�-jixieshiyongfei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixieshiyongfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixieshiyongfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixieshiyongfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ����е�豸���޷ѱ�-jx-fengongsiziwan-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxfengongsiziwansqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxfengongsiziwansqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxfengongsiziwansqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:fengongsiziwan-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ����е�豸���޷ѱ�-jx-qita-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxqitasqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-qita-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxqitasqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-qita-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxqitasqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-qita-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ����е�豸���޷ѱ�-jixiezulinshuiqianheji��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixiezulinshuiqianhejiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulinshuiqianheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixiezulinshuiqianheji");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixiezulinshuiqianhejiTaxAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sj", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiSumTaxAmount.Text = (decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqSumTaxAmount.Text) + decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqSumTaxAmount.Text) + decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqSumTaxAmount.Text)).ToString();
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixiezulinshuiqianhejiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulinshuihouheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixiezulinshuihouheji");
            LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulin-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ����ʱ��ʩ�ѱ�-linshisheshi-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlinshisheshisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlinshisheshisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlinshisheshisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlinshisheshisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlinshisheshisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlinshisheshisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenlinshisheshisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ����ȫ��ʩ�ѱ�-anquancuoshi-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenanquancuoshisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenanquancuoshisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenanquancuoshisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenanquancuoshisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenanquancuoshisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenanquancuoshisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenanquancuoshisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ��ˮ��ѱ�-shuidianfei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenshuidianfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenshuidianfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenshuidianfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenshuidianfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenshuidianfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenshuidianfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenshuidianfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ���������̷��ñ�-qitagongchengfei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitagongchengfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitagongchengfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagongchengfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitagongchengfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitagongchengfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagongchengfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitagongchengfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagognchengfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitagongchengfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagognchengfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenqitagongchengfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-qitafeiheji-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitafeihejisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafeiheji-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitafeihejisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitafeiheji-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitafeihejisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitafeihejisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitaqita-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitafeihejisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafeiheji-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitafeihejisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitafeiheji-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenqitafeihejisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-jiancefei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjiancefeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:jiancefei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjiancefeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:jiancefei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjiancefeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:jiancefei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjiancefeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:jiancefei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjiancefeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:jiancefei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjiancefeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:jiancefei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenjiancefeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:jiancefei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-waixiejiagongfei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaixiejiagongfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:waixiejiagongfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaixiejiagongfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:waixiejiagongfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaixiejiagongfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:waixiejiagongfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaixiejiagongfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:waixiejiagongfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaixiejiagongfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:waixiejiagongfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaixiejiagongfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:waixiejiagongfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenwaixiejiagongfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:waixiejiagongfei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-zulinfei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzulinfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:zulinfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzulinfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:zulinfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzulinfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:zulinfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzulinfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:zulinfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzulinfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:zulinfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzulinfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:zulinfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenzulinfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:zulinfei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-laodongbaohufei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaodongbaohufeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:laodongbaohufei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaodongbaohufeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:laodongbaohufei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaodongbaohufeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:laodongbaohufei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaodongbaohufeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:laodongbaohufei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaodongbaohufeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:laodongbaohufei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaodongbaohufeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:laodongbaohufei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenlaodongbaohufeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:laodongbaohufei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-xiulifei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenxiulifeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:xiulifei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenxiulifeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:xiulifei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenxiulifeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:xiulifei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenxiulifeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:xiulifei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenxiulifeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:xiulifei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenxiulifeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:xiulifei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenxiulifeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:xiulifei-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-bukeyujian-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbukeyujiansqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bukeyujian-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbukeyujiansqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bukeyujian-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbukeyujiansqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bukeyujian-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbukeyujiansqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bukeyujian-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbukeyujiansqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bukeyujian-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbukeyujiansqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bukeyujian-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenbukeyujiansqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bukeyujian-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-fakuan-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfakuansqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:fakuan-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfakuansqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:fakuan-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfakuansqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:fakuan-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfakuansqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:fakuan-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfakuansqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:fakuan-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfakuansqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:fakuan-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenfakuansqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:fakuan-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-bangongyongping-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbangongyongpingsqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bangongyongping-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbangongyongpingsqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bangongyongping-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbangongyongpingsqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bangongyongping-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbangongyongpingsqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bangongyongping-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbangongyongpingsqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bangongyongping-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbangongyongpingsqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bangongyongping-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenbangongyongpingsqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bangongyongping-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�������ѱ�-qitaqita-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitaqitasqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitaqitasqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitaqita-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitaqitasqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitaqitasqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitaqita-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitaqitasqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitaqitasqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitaqita-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenqitaqitasqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ�� ��Ǩ�ѱ�-dongqianfei-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBendongqianfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendongqianfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendongqianfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendongqianfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendongqianfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendongqianfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBendongqianfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-slv", "my:riqi", strYearMonth);


            //����ʵ�ʷ����ɱ���˰ǰ�� �����ɱ����-ranglichengben-sq��
            LB_XiangMuCurrentMonthShiJiFaShengChengBenranglichengbensqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenranglichengbensqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenranglichengbensqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenranglichengbensqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenranglichengbensqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenranglichengbensqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenranglichengbensqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-slv", "my:riqi", strYearMonth);

            //����ʵ�ʷ����ɱ���˰ǰ��M15+M16+M17+M18+M21+M22+M23+M24+M25+M26+M28+M29+M30+M31+M33+M34+M35+M36+M37+M38+M39+M41+M42+M43+M44+M45+M46+M47+M48+M49+M50+M51   ����  M14+M19+M27+M32+M36+M37+M38+M39+M40+M50+M51
            decimal deSQSJCB = decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:rengongfeiheji", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fenbaofeiheji", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulinshuiqianheji", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sq", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-sq", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafeiheji-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sq", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse( GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sq", "my:riqi", strYearMonth));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenAmount.Text = deSQSJCB.ToString();

          
            //������ʵ�ʷ����ɱ���˰ǰ�������� M52 ��ÿ���ۼ�
            decimal deSQSJCBHJ = decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:rengongfeiheji")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fenbaofeiheji"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixiezulinshuiqianheji"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sq"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagongchengfei-sq"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitafeiheji-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sq"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sq"));
            LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text = deSQSJCBHJ.ToString();

            //������ʵ�ʷ����ɱ���˰�𣩡�O21+O22+O23+O24+O25+O26+O28+O29+O30+O31+O33+O34+O35+O36+O37+O38+O39+O41+O42+O43+O44+O45+O46+O47+O48+O49+O50+O51   ����  O14+O19+O27+O32+O36+O37+O38+O39+O40+O50+O51
            decimal deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount2 = decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sj", "my:riqi", strYearMonth));
            deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount2 += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhaunyefenbao-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fengongsiziwan-sj", "my:riqi", strYearMonth));
            deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount2 += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-sj", "my:riqi", strYearMonth));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount.Text = deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount2.ToString("f2");

            decimal deSQSJSJ = decimal.Parse("0") + deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount2;
            deSQSJSJ += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sj", "my:riqi", strYearMonth)) + decimal.Parse((decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sj", "my:riqi", strYearMonth))).ToString());
            deSQSJSJ += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sj", "my:riqi", strYearMonth));
            deSQSJSJ += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-sj", "my:riqi", strYearMonth));
            deSQSJSJ += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sj", "my:riqi", strYearMonth));
            deSQSJSJ += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sj", "my:riqi", strYearMonth));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenTaxAmount.Text = deSQSJSJ.ToString();

            //��ʵ��˰�𣨵��£������� O52 ��ÿ���ۼ�
            decimal deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount3 = decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sj")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sj"));
            deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount3 += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:zhaunyefenbao-sj")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fengongsiziwan-sj"));
            deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount3 += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:dianyi-sj")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:diaohzuang-sj"));

            decimal deSQSJSJHJ = decimal.Parse("0") + deXiangMuCurrentMonthShiJiFaShengChengBenfenbaofeihejiTaxAmount3;
            deSQSJSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sj")) + decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiSumTaxAmount.Text);
            deSQSJSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sj")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sj"));
            deSQSJSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sj")) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagognchengfei-sh", "my:riqi", strYearMonth));
            deSQSJSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagongchengfei-sj")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sj"));
            deSQSJSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sj"));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenSumTaxAmount.Text = deSQSJSJHJ.ToString();

            //������ʵ�ʷ����ɱ���˰�󣩡�Q15+Q16+Q17+Q18+Q21+Q22+Q23+Q24+Q25+Q26+Q28+Q29+Q30+Q31+Q33+Q34+Q35+Q36+Q37+Q38+Q39+Q41+Q42+Q43+Q44+Q45+Q46+Q47+Q48+Q49+Q50+Q51   ����  Q14+Q19+Q27+Q32+Q36+Q37+Q38+Q39+Q40+Q50+Q51
            decimal deSQSJSH = decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:rengongfeiheji", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fenbaoshuihouheji", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulinshuihouheji", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sh", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagognchengfei-sh", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafeiheji-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sh", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sh", "my:riqi", strYearMonth));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenAfterTaxAmount.Text = deSQSJSH.ToString();

            //����ʵ�ʷ����ɱ���˰�󣩼��� Q52 ��ÿ���ۼ�
            decimal deSQCBSJHJ = decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:rengongfeiheji")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fenbaoshuihouheji"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixiezulinshuihouheji"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sh"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagognchengfei-sh"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitafeiheji-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sh"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sh"));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenSumAfterTaxAmount.Text = deSQCBSJHJ.ToString();

            //˰��/˰ǰ��� ����O52/M52
            try
            {
                LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenTaxRate.Text = (decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengChengBenTaxAmount.Text) / decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenAmount.Text)).ToString("f2");
            }
            catch
            {
                LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenTaxRate.Text = "0";
            }

            //����������-�ۼ�ʵ�ʷ����ɱ���˰ǰ ����,��H12-N52
            LB_XiangMuCurrentMonthShiJiFaShengProfitAmount.Text = (decimal.Parse(LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text) - decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text)).ToString();

            //����˰��=��ʵ�ʽ����*˰��,�������Ա���   ���ȿ��������˰�𡱡��ϼơ���ʵ����˰��,����: �ؼ���xiaoxiangshuijinheji��        
            LB_XiangMuCurrentMonthShiJiFaShengProfitXiaoXiangTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProgressPaymentSettlementReport", "my:xiaoxiangshuijinheji", "my:riqi", strYearMonth);

            //����˰��������гɱ������д��ʵ����˰��,��:P52
            LB_XiangMuCurrentMonthShiJiFaShengProfitJingXiangTaxAmount.Text = LB_XiangMuCurrentMonthShiJiFaShengChengBenSumTaxAmount.Text;

            //˰�𸽼�=˰��*12%   ���� M56 * 12 %
            LB_XiangMuCurrentMonthShiJiFaShengProfitFuJiaTaxAmount.Text = ((decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengProfitJingXiangTaxAmount.Text) * 12) / 100).ToString();

            //��������Ŀһ�α�۷�����С�������˰������Ӧ����ͬ���е�Ԫ�������
            LB_XiangMuYiCiFenLiBiaoMaoLiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:EAD", "my:riqi", strYearMonth);

            LB_XiangMuYiCiFenLiBiaoShuiJingAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field91", "my:riqi", strYearMonth);
            LB_XiangMuYiCiFenLiBiaoChengBenJiaShuiJingAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:GDF", "my:riqi", strYearMonth);
            LB_XiangMuYiCiFenLiBiaoYiJiProfitAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:HAG", "my:riqi", strYearMonth);

            //����Ŀ����ģ�顰��Ŀ������桱����Ŀ��Ԥ�㣺
            LB_XiangMuTotalBudget.Text = ShareClass.GetProject(strProjectID).Budget.ToString();

            try
            {
                LB_XiangMuTotalTaxRate.Text = GetWZProject(strProjectID).TaxRate.ToString();
            }
            catch
            {
                LB_XiangMuTotalTaxRate.Text = "0";
            }
            try
            {
                LB_XiangMuTotalTaxAmount.Text = GetWZProject(strProjectID).TaxAmount.ToString();
            }
            catch
            {
                LB_XiangMuTotalTaxAmount.Text = "0";
            }

            LB_XiangMuTotalAfterTaxAmount.Text = (decimal.Parse(LB_XiangMuTotalBudget.Text) - decimal.Parse(LB_XiangMuTotalTaxAmount.Text)).ToString();

            //����˰��=��ʵ�ʽ����*˰��,�������Ա��� ���ȿ��������˰�𡱡��ϼơ���ʵ����˰��,����: �ؼ���xiaoxiangshuijinheji��
            LB_XiangMuCurrentMonthShiJiFaShengProfitSaleTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProgressPaymentSettlementReport", "my:xiaoxiangshuijinheji", "my:riqi", strYearMonth);

            try
            {
                //�ۺ�˰��=(����������˰�ܶ�?֧�������˰�ܶ�)/������˰ʵ�����룩  ����(M54-M55)/�������ȿ����������Ʊ�����ϼơ������ؼ���kpjeheji�������ݣ�
                LB_XiangMuTotalSummaryTaxAmount.Text = ((decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengProfitSaleTaxAmount.Text) - decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengProfitXiaoXiangTaxAmount.Text)) / decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProgressPaymentSettlementReport", "my:kpjeheji", "my:riqi", strYearMonth))).ToString();
            }
            catch
            {
                LB_XiangMuTotalSummaryTaxAmount.Text = "0";
            }


            //----------------------------�ڶ��ݱ���------------------------------------
            LB_ProjectCode2.Text = LB_ProjectCode.Text;
            LB_ProjectName2.Text = LB_ProjectName.Text;

            //����Ŀ�ɱ���������̬�������Ԥ��������ϼơ��У�����ͬԤ��ۣ����Σ����еĵ�Ԫ�����ݣ�����H12
            LB_XiangMuHeTongYuShuanJiaErCiAmount.Text = LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text;


            //����Ŀ�ɱ���������̬�������Ŀ��ɱ������Σ����С��ܳɱ����м���I52
            LB_XiangMuMuBiaoChengBenErCiAmount.Text = LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenAmount.Text;

            //����Ŀ�ɱ���������̬��������ۼ�ʵ�ʷ����ɱ���˰ǰ �����У����ܳɱ����еĵ�Ԫ�����ݣ�����N52
            LB_XiangMuLieJiShiJiFaShengChengBenAmount.Text = LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text;

            //(����Ŀ�ɱ���������̬��������ۼ�ʵ�ʷ����ɱ���˰ǰ �����У����ܳɱ����еĵ�Ԫ�����ݣ�����N52)+(�������롱������B4-��"���ȿ������"��������С��ϼơ�����ֵ)*(1-����ʣ����Σ�������Ŀ�ɱ���������̬�����L11)
            LB_XiangMuYiJiFaShengZongChengBenAmount.Text = ((decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text) + decimal.Parse(LB_XiangMuHeTongYuShuanJiaErCiAmount.Text)) * (1 - decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianLu.Text))).ToString("f6");


            try
            {
                //�������ȡ�*�������루Ԥ�㣩���������� E5/B5*B4
                LB_XiangMuHeTongJingDuShengYuZongShouRuAmount.Text = ((decimal.Parse(LB_XiangMuLieJiShiJiFaShengChengBenAmount.Text) / decimal.Parse(LB_XiangMuMuBiaoChengBenErCiAmount.Text)) * (decimal.Parse(LB_XiangMuHeTongYuShuanJiaErCiAmount.Text))).ToString("f6");
            }
            catch
            {
                LB_XiangMuHeTongJingDuShengYuZongShouRuAmount.Text = "0";
            }

            //����Ŀ����ģ�鱾��Ŀ����Ŀ�����ҳ��ġ������ܶ��ȡ��
            try
            {
                LB_XiangMuYiJiFaShengBaoGuanZongEAmount.Text = GetWZProject(strProjectID).BaoGuanZongE.ToString("f6");
            }
            catch
            {
                LB_XiangMuYiJiFaShengBaoGuanZongEAmount.Text = "0";
            }

            //�ɱ�ʵ��ֵ/��Ŀ�ܳɱ�����ʾΪ�ٷֱ���ʽ������ʵ�ʣ��ɱ���/��Ԥ�㣺�ɱ���  ������E5/B5*100%
            try
            {
                LB_XiangMuYiJiFaShengJingDuAmount.Text = ((decimal.Parse(LB_XiangMuYiJiFaShengZongChengBenAmount.Text) / decimal.Parse(LB_XiangMuMuBiaoChengBenErCiAmount.Text))).ToString("f6");
            }
            catch
            {
                LB_XiangMuYiJiFaShengJingDuAmount.Text = "0";
            }

            try
            {
                //����ʵ������-����ʵ�ʳɱ������������ȡ�*�������루Ԥ�㣩����-��ʵ�ʣ��ɱ����� ������B9*B4-E5
                LB_XiangMuYiJiFaShengLiRenMaoLiAmount.Text = ((decimal.Parse(LB_XiangMuYiJiFaShengJingDuAmount.Text) / decimal.Parse(LB_XiangMuMuBiaoChengBenErCiAmount.Text)) - decimal.Parse(LB_XiangMuLieJiShiJiFaShengChengBenAmount.Text)).ToString("f6");
            }
            catch
            {
                LB_XiangMuYiJiFaShengLiRenMaoLiAmount.Text = "0";
            }


            //��������ܶ��¶Ƚ������ϸ��
            SaveProjectDetailedListOfMonthlyBonusAmount(strProjectID, strYearMonth);

            //��Ŀ�ɱ����������ͼ
            SaveProjectCostIncomeAnalysisGeneralChart(strProjectID, strYearMonth);
        }
        catch (Exception err)
        {
            LB_ErrorText.Text = "Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace;
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //��������ܶ��¶Ƚ������ϸ��
    protected void SaveProjectDetailedListOfMonthlyBonusAmount(string strProjectID, string strYearMonth)
    {
        string strHQL;

        string strYearNumber = strYearMonth.Substring(0, 4);
        string strMonthNumber = strYearMonth.Substring(4, 2);

        strHQL = "Delete From T_ProjectDetailedListOfMonthlyBonusAmount Where ProjectID = " + strProjectID + " and YearNumber = " + strYearNumber + " and MonthNumber = " + strMonthNumber;
        ShareClass.RunSqlCommand(strHQL);

        string strProfit = LB_XiangMuYiJiFaShengLiRenMaoLiAmount.Text;

        string strClearing;
        string strdinganbiaoshangchuan = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SettlementRecoveryReport", "my:dinganbiaoshangchuan", "my:riqi", strYearMonth);
        if (strdinganbiaoshangchuan.Trim() == "0")
        {
            strClearing = "10%";
        }
        else
        {
            strClearing = "0%";
        }

        string strReturnMoney = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProgressPaymentSettlementReport", "my:shoukuanleijidefenzhi", "my:riqi", strYearMonth);
        string strQHSE = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProfessionalManagementAssessmentApprovalForm", "my:jiajianfenzhi", "my:riqi", strYearMonth);
        string strProgress = (decimal.Parse(LB_XiangMuYiJiFaShengJingDuAmount.Text) * 80 / 100).ToString();

        strHQL = "Insert Into T_ProjectDetailedListOfMonthlyBonusAmount(ProjectID,YearNumber,MonthNumber,Profit,Clearing,ReturnMoney,QHSE,Progress)";
        strHQL += " values(" + strProjectID + "," + strYearNumber + "," + strMonthNumber + "," + strProfit + ",'" + strClearing + "'," + strReturnMoney + "," + strQHSE + "," + strProgress + ")";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Select * From T_ProjectDetailedListOfMonthlyBonusAmount Where ProjectID = " + strProjectID + " and YearNumber = " + strYearNumber + " and MonthNumber = " + strMonthNumber;
        strHQL += " Order By MonthNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectDetailedListOfMonthlyBonusAmount");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        strHQL = "Select COALESCE(Sum(COALESCE(Profit,0)),0) as SumProfit,COALESCE(Sum(COALESCE(cast(replace(Clearing,'%','') as integer),0)),0) as SumClearing,COALESCE(Sum(COALESCE(ReturnMoney,0)),0) as SumReturnMoney,COALESCE(Sum(COALESCE(QHSE,0)),0) as SumQHSE,COALESCE(Sum(COALESCE(Progress,0)),0) AS SumProgress From T_ProjectDetailedListOfMonthlyBonusAmount Where ProjectID = " + strProjectID + " and YearNumber = " + strYearNumber + " and MonthNumber = " + strMonthNumber;
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectDetailedListOfMonthlyBonusAmount");
        if (ds.Tables[0].Rows.Count > 0)
        {
            LB_XiangMuBaoGuanZongEProfitHeJi.Text = ds.Tables[0].Rows[0]["SumProfit"].ToString();
            LB_XiangMuBaoGuanZongEClearingHeJi.Text = ds.Tables[0].Rows[0]["SumClearing"].ToString() + "%";
            LB_XiangMuBaoGuanZongEReturnMoneyHeJi.Text = ds.Tables[0].Rows[0]["SumReturnMoney"].ToString();
            LB_XiangMuBaoGuanZongEQHSEHeJi.Text = ds.Tables[0].Rows[0]["SumQHSE"].ToString();
            LB_XiangMuBaoGuanZongEProgressHeJi.Text = ds.Tables[0].Rows[0]["SumProgress"].ToString();
        }
    }

    //��Ŀ�ɱ����������ͼ
    protected void SaveProjectCostIncomeAnalysisGeneralChart(string strProjectID, string strYearMonth)
    {
        string strHQL;

        string strYearNumber = strYearMonth.Substring(0, 4);
        string strMonthNumber = strYearMonth.Substring(4, 2);

        strHQL = "Delete From T_ProjectCostIncomeAnalysisGeneralChart Where ProjectID = " + strProjectID + " and YearNumber = " + strYearNumber + " and MonthNumber = " + strMonthNumber;
        ShareClass.RunSqlCommand(strHQL);

        string strCurrentMonthTotalCost = LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenAfterTaxAmount.Text;
        string strCumulativeActualTaxCost = LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text;
        string strCumulativeActualAfterTaxCost = LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenSumAfterTaxAmount.Text;
        string strKPJEHeJi = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProgressPaymentSettlementReport", "my:kpjeheji");

        strHQL = "Insert Into T_ProjectCostIncomeAnalysisGeneralChart(ProjectID,YearNumber,MonthNumber,CurrentMonthTotalCost,CumulativeActualTaxCost,CumulativeActualAfterTaxCost,AccumulationSettlement)";
        strHQL += " values(" + strProjectID + "," + strYearNumber + "," + strMonthNumber + "," + strCurrentMonthTotalCost + ",'" + strCumulativeActualTaxCost + "'," + strCumulativeActualAfterTaxCost + "," + strKPJEHeJi + ")";
       ShareClass.RunSqlCommand(strHQL);

        strHQL = "Update T_ProjectCostIncomeAnalysisGeneralChart Set CurrentMonthTotalCost = 0 ";
        strHQL += "  Where ProjectID = " + strProjectID + " and YearNumber = " + strYearNumber + " and MonthNumber <> '01'";
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Select * From T_ProjectCostIncomeAnalysisGeneralChart Where ProjectID = " + strProjectID + " and YearNumber = " + strYearNumber + " and MonthNumber = " + strMonthNumber;
        strHQL += " Order By MonthNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectCostIncomeAnalysisGeneralChart");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        strHQL = @"Select ProjectID, MonthNumber as XName,CurrentMonthTotalCost as YNumber,CumulativeActualTaxCost as ZNumber,CumulativeActualTaxCost as HNumber,CumulativeActualAfterTaxCost,AccumulationSettlement as INumber
                  From T_ProjectCostIncomeAnalysisGeneralChart  Where ProjectID = " + strProjectID + " and YearNumber = " + strYearNumber + " and MonthNumber = " + strMonthNumber;
        strHQL += " Order By MonthNumber ASC";
        string strChartTitle = "ReportView";
        IFrame_Chart1.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Column4&ChartType=Column&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strHQL);

        //ShareClass.CreateAnalystFourColumnChart(strHQL, Chart1, SeriesChartType.Column, strChartTitle, "MonthNumber", "CurrentMonthTotalCost", "CumulativeActualTaxCost", "CumulativeActualAfterTaxCost", "AccumulationSettlement", "Default", "���¶���Ŀ��ɱ���˰��", "�ۼ�ʵ�ʷ����ɱ���˰ǰ��", "����ʵ�ʳɱ���˰���ۼ�", "�ۼƽ���");
        //Chart1.Visible = true;
    }


    //ȡ���̱���Ӧ�����������ڵķ���
    private string GetWorkFlowColumnDataByMaxFieldValue(string strProjectID, string strWorkFlowTemplateName, string strFieldName, string strSortFieldName, string strYearMonth)
    {
        string strHQL;

        strHQL = string.Format(@"Select FieldValue From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{1}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2} and WLID In (Select WLID From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' and RelatedID = {2})
                 and FieldName = '{3}' and FieldValue in (Select Max(FieldValue) From T_WorkFlowFormData 
                 Where TemplateName = '{0}' and FieldName = '{3}' and cast(substring(to_char(FieldValue::timestamp,'yyyymmdd'),1,6) as int) <= cast('{4}' as int)) 
                 and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2})))", strWorkFlowTemplateName, strFieldName, strProjectID, strSortFieldName, strYearMonth);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowFormData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString())/10000).ToString("f6");
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }


    //ȡ���̱���Ӧ���ӵķ��õĻ���
    private string GetWorkFlowColumnSumData(string strProjectID, string strYearMonth, string strSortFieldName, string strWorkFlowTemplateName, string strFieldName)
    {
        string strHQL;

        strHQL = string.Format(@"Select COALESCE(sum(cast(FieldValue as decimal)),0) From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{1}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2} and WLID In (Select WLID From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{3}' and cast(substring(to_char(FieldValue::timestamp,'yyyymmdd'),1,6) as int) <= cast('{4}' as int)))", strWorkFlowTemplateName, strFieldName, strProjectID, strSortFieldName, strYearMonth);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowFormData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }
   

    //���ֶ�����ȡ���¸����������£��뵱ǰ��������������̱���Ӧ���ӵķ���
    private string GetWorkFlowColumnLastestMonthDataByMaxFieldValue(string strProjectID, string strWorkFlowTemplateName, string strFieldName, string strSortFieldName, string strYearMonth)
    {
        string strHQL;

        strHQL = string.Format(@"Select FieldValue From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{1}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2} and WLID In (Select WLID From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{3}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' and RelatedID = {2})
                 and FieldValue in (Select Max(FieldValue) From T_WorkFlowFormData 
                 Where TemplateName = '{0}' and FieldName = '{3}' 
                 and cast(substring(to_char(FieldValue::timestamp,'yyyymmdd'),1,6) as int) <= cast('{4}' as int)) 
                 and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2})))", strWorkFlowTemplateName, strFieldName, strProjectID, strSortFieldName, strYearMonth);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowFormData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }

    //ȡ���µ����̱���Ӧ���ӵķ���
    private string GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(string strProjectID, string strWorkFlowTemplateName, string strFieldName, string strSortFieldName, string strYearMonth)
    {
        string strHQL;

        strHQL = string.Format(@"Select FieldValue From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{1}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2} and WLID In (Select WLID From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{3}' and substring(to_char(FieldValue::timestamp,'yyyymmdd'),1,6) = '{4}'))", strWorkFlowTemplateName, strFieldName, strProjectID, strSortFieldName, strYearMonth);


        //LB_ProjectName.Text = strHQL;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowFormData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }

    //ȡ���µ����̱���Ӧ���ӵķ���
    private string GetWorkFlowColumnPriorMonthDataByMaxFieldValue(string strProjectID, string strWorkFlowTemplateName, string strFieldName, string strSortFieldName, string strYearMonth)
    {
        string strHQL;

        strHQL = string.Format(@"Select FieldValue From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{1}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2} and WLID In (Select WLID From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{3}' and substring(to_char(FieldValue::timestamp,'yyyymmdd'),1,6) = substring(to_char('{4}'::timestamp  - '1 month'::interval,'yyyymmdd'),1,6)))", strWorkFlowTemplateName, strFieldName, strProjectID, strSortFieldName, strYearMonth + "01");

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowFormData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }

    //ȡ���̱���Ӧ�����������ڵ�˰������
    private string GetWorkFlowColumnTaxRateDataByMaxFieldValue(string strProjectID, string strWorkFlowTemplateName, string strFieldName, string strSortFieldName, string strYearMonth)
    {
        string strHQL;

        strHQL = string.Format(@"Select FieldValue From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{1}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2} and WLID In (Select WLID From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' and RelatedID = {2})
                 and FieldName = '{3}' and FieldValue in (Select Max(FieldValue) From T_WorkFlowFormData 
                 Where TemplateName = '{0}' and FieldName = '{3}' and cast(substring(to_char(FieldValue::timestamp,'yyyymmdd'),1,6) as int) <= cast('{4}' as int)) 
                 and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2})))", strWorkFlowTemplateName, strFieldName, strProjectID, strSortFieldName, strYearMonth);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowFormData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString())).ToString("f2");
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }

    //���ֶ�����ȡ���¸����������£��뵱ǰ��������������̱���Ӧ���ӵ�˰������
    private string GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(string strProjectID, string strWorkFlowTemplateName, string strFieldName, string strSortFieldName, string strYearMonth)
    {
        string strHQL;

        strHQL = string.Format(@"Select FieldValue From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{1}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2} and WLID In (Select WLID From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{3}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' and RelatedID = {2})
                 and FieldValue in (Select Max(FieldValue) From T_WorkFlowFormData 
                 Where TemplateName = '{0}' and FieldName = '{3}' 
                 and cast(substring(to_char(FieldValue::timestamp,'yyyymmdd'),1,6) as int) <= cast('{4}' as int)) 
                 and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2})))", strWorkFlowTemplateName, strFieldName, strProjectID, strSortFieldName, strYearMonth);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowFormData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString())).ToString("f2");
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }

    //ȡ���µ����̱���Ӧ���ӵ�˰������
    private string GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(string strProjectID, string strWorkFlowTemplateName, string strFieldName, string strSortFieldName, string strYearMonth)
    {
        string strHQL;

        strHQL = string.Format(@"Select FieldValue From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{1}' and WLID In (Select WLID From T_WorkFlow Where RelatedType = 'Project' 
                 and RelatedID = {2} and WLID In (Select WLID From T_WorkFlowFormData Where TemplateName = '{0}' 
                 and FieldName = '{3}' and substring(to_char(FieldValue::timestamp,'yyyymmdd'),1,6) = '{4}'))", strWorkFlowTemplateName, strFieldName, strProjectID, strSortFieldName, strYearMonth);

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowFormData");
        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString())).ToString("f2");
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }


    //ȡ����Ŀ��ͬ��Ҫ����
    private string GetInitialConstractMainContent(string strProjectID)
    {
        string strHQL;

        strHQL = "Select MainContent From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    //ȡ����Ŀ��ͬ�쳣����
    private string GetInitialConstractException(string strProjectID)
    {
        string strHQL;

        strHQL = "Select Exception From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    //ȡ����Ŀ˰ǰ��ͬ��
    private string GetInitialConstractAmountBeforTax(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(Amount,0)),0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and SignDate in (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse( ds.Tables[0].Rows[0][0].ToString())/10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ˰���ͬ��
    private string GetInitialConstractAmountAfterTax(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(AfterTaxTotalAmount,0)),0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and SignDate in (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ��ͬ��˰��
    private string GetInitialConstractTaxRate(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(TaxRate,0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and SignDate in (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return decimal.Parse(ds.Tables[0].Rows[0][0].ToString()).ToString("f2"); 
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ��ͬ��˰��
    private string GetInitialConstractTaxAmount(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(TaxAmount,0)),0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and SignDate in (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ������ͬ˰ǰ��ͬ��
    private string GetSupplementConstractAmountBeforTax(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(Amount,0)),0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and SignDate not in (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ����˰���ͬ��
    private string GetSupplementConstractAmountAfterTax(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(AfterTaxTotalAmount,0)),0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and SignDate not in (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ������ͬ��˰��
    private string GetSupplementConstractTaxAmount(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(TaxAmount,0)),0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and SignDate not in (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ������ͬ����ĵ�˰��
    private string GetSupplementConstractChangeTaxAmount(string strProjectID)
    {
        string strHQL;
        decimal deTotalChangeAmount = 0;

        strHQL = "Select COALESCE(AfterChangeAmount,0) * (Select TaxRate From T_Constract Where ConstractCode = A.ConstractCode) From T_ConstractChangeRecord A Where A.ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractChangeRecord");

        try
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                deTotalChangeAmount += decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            }

            return (decimal.Parse(deTotalChangeAmount.ToString()) / 10000).ToString("f6");
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ��ͬ�����¼�ĺ�ͬ��
    private string GetConstractAmountAfterChange(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(AfterChangeAmount,0) From T_ConstractChangeRecord Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " Order By ID Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractChangeRecord");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }


    //ȡ����Ŀ��ͬ���±��������
    private string GetConstractCurrentMonthAmountAfterChange(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(AfterChangeAmount,0)),0) From T_ConstractChangeRecord Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and substring(to_char(ChangeTime,'yyyymmdd'),0,6) = substring(to_char(now(),'yyyymmdd'),0,6)";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractChangeRecord");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ��ͬ���±��������
    private string GetConstractPirorMonthAmountAfterChange(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(AfterChangeAmount,0)),0) From T_ConstractChangeRecord Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and substring(to_char(ChangeTime,'yyyymmdd'),0,6) = substring(to_char(now() - '1 month'::interval,'yyyymmdd'),0,6)";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractChangeRecord");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ����������ͬ���ܶ�
    private string GetConstractCurrentMonthSupplementAmountAfterTax(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(AfterTaxTotalAmount,0)),0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and substring(to_char(SignDate,'yyyymmdd'),0,6) = substring(to_char(now(),'yyyymmdd'),0,6)";
        strHQL += " and SignDate Not In (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    //ȡ����Ŀ����������ͬ���ܶ�
    private string GetConstractPriorMonthSupplementAmountAfterTax(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(sum(COALESCE(AfterTaxTotalAmount,0)),0) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + ")";
        strHQL += " and substring(to_char(SignDate,'yyyymmdd'),0,6) = substring(to_char(now() - '1 month'::interval,'yyyymmdd'),0,6)";
        strHQL += " and SignDate Not In (Select Min(SignDate) From T_Constract Where ConstractCode In (Select ConstractCode From T_ConstractRelatedProject Where ProjectID = " + strProjectID + "))";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Constract");
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                return (decimal.Parse(ds.Tables[0].Rows[0][0].ToString()) / 10000).ToString("f6");
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    protected WZProject GetWZProject(string strProjectID)
    {
        string strWZProjectSql = "from WZProject as wZProject where RelatedProjectID = " + strProjectID;
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        IList lst = wZProjectBLL.GetAllWZProjects(strWZProjectSql);
        if (lst != null && lst.Count > 0)
        {
            return (WZProject)lst[0];
        }
        else
        {
            return null;
        }
    }

}