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


            //给合同相关项赋值
            //合同金额一次”
            LB_InitialConstractAmount.Text = GetInitialConstractAmountBeforTax(strProjectID);
            LB_InitialConstractTaxRate.Text = GetInitialConstractTaxRate(strProjectID);
            LB_InitialConstractTaxAmount.Text = GetInitialConstractTaxAmount(strProjectID);
            LB_InitialConstractAfterTaxAmount.Text = (decimal.Parse(LB_InitialConstractAmount.Text) + decimal.Parse(LB_InitialConstractTaxAmount.Text)).ToString("f6");
            LB_InitialConstractMainContent.Text = GetInitialConstractMainContent(strProjectID);
            LB_InitialConstractException.Text = GetInitialConstractException(strProjectID);

            //合同增补额一次
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

            //合同金额二次”
            LB_InitialSecondConstractAmount.Text = LB_InitialConstractAmount.Text;
            LB_InitialSecondConstractTaxRate.Text = GetInitialConstractTaxRate(strProjectID);
            LB_InitialSecondConstractTaxAmount.Text = GetInitialConstractTaxAmount(strProjectID);
            LB_InitialSecondConstractAfterTaxAmount.Text = (decimal.Parse(LB_InitialConstractAmount.Text) + decimal.Parse(LB_InitialConstractTaxAmount.Text)).ToString("f6");

            //合同增补额二次
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
            //"不可预见费表单及各项奖励"表单中“各项奖励”“合计”“税前金额”
            LB_RGSYGeXiangJiangLi.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sq", "my:riqi", strYearMonth);

            //"不可预见费表单及各项奖励"表单中“各项奖励”“合计”“税率”
            LB_gexiangjiangli_slv.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-slv", "my:riqi", strYearMonth);

            //"不可预见费表单及各项奖励"表单中“各项奖励”“合计”“税金”
            LB_gexiangjiangli_sj.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sj", "my:riqi", strYearMonth);

            //"不可预见费表单及各项奖励"表单中“各项奖励”“合计”“税后金额”
            LB_gexiangjiangli_sh.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sh", "my:riqi", strYearMonth);



            ////“不可预见费表单及各项奖励”中“不可预见费”“金额（不含税）”单元格的数据读取
            //LB_BuKeYuJianFei.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:bukeyujianfei-sq", "my:riqi", strYearMonth);

            ////“不可预见费表单及各项奖励”中“不可预见费”“税率”单元格的数据读取
            //LB_BuKeYuJianFeiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:bukeyujianfei-slv", "my:riqi", strYearMonth);

            ////“不可预见费表单及各项奖励”中“不可预见费”“税金”单元格的数据读取
            //LB_BuKeYuJianFeiTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:bukeyujianfei-sj", "my:riqi", strYearMonth);

            ////“不可预见费表单及各项奖励”中“不可预见费”“税后金额”单元格的数据读取
            //LB_BuKeYuJianFeiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:bukeyujianfei-sh", "my:riqi", strYearMonth);



            //“项目一次标价分离表”中“不可预见费”“金额（不含税）”单元格的数据读取
            LB_BuKeYuJianFei.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:B8", "my:riqi", strYearMonth);

            //“项目一次标价分离表”中“不可预见费”“税率”单元格的数据读取
            LB_BuKeYuJianFeiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field37", "my:riqi", strYearMonth);

            //“项目一次标价分离表”中“不可预见费”“税金”单元格的数据读取
            LB_BuKeYuJianFeiTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field38", "my:riqi", strYearMonth);

            //“项目一次标价分离表”中“不可预见费”“税后金额”单元格的数据读取
            LB_BuKeYuJianFeiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field39", "my:riqi", strYearMonth);



            //默认从表单：“项目一次标价分离表”中“预计审减额”单元格的数据，当表单读不到数据，从“项目立项”模块读取
            LB_FirstYiJiShenJianE.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:C", "my:riqi", strYearMonth);

            //默认从表单：“项目一次标价分离表”中“预计审减额”-“税率”单元格的数据
            LB_FirstYiJiShenJianETaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field45", "my:riqi", strYearMonth);

            //默认从表单：“项目一次标价分离表”中“预计审减额”-“税金”单元格的数据
            LB_FirstYiJiShenJianETaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field46", "my:riqi", strYearMonth);

            //默认从表单：“项目一次标价分离表”中“预计审减额”-“税后金额”单元格的数据
            LB_FirstYiJiShenJianEAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field47", "my:riqi", strYearMonth);



            //审减率（将数据填于此格，其值=预计审减额（一次）/合同金额（税前）即 C11/C7）
            try
            {
                LB_FirstYiJiShenJianRate.Text = (decimal.Parse(LB_FirstYiJiShenJianE.Text) / decimal.Parse(LB_InitialConstractAmount.Text)).ToString("f6");
            }
            catch
            {
                LB_FirstYiJiShenJianRate.Text = "0";
            }
            //计总收入合计C7+C8+C9+C10-C11（以上1-5项合计自动计算结果）
            decimal deYiJiZongShouRuHeJi = decimal.Parse(LB_InitialConstractAmount.Text) + decimal.Parse(LB_SupplementConstractAmount.Text) + decimal.Parse(LB_RGSYGeXiangJiangLi.Text) + decimal.Parse(LB_BuKeYuJianFei.Text);
            deYiJiZongShouRuHeJi -= decimal.Parse(LB_FirstYiJiShenJianE.Text);
            LB_YiJiHeTongYiShuanJiaZongShouRuHeJi.Text = deYiJiZongShouRuHeJi.ToString("f6");

            //预计总收入合计E7+E8+E9+E10-E11
            decimal deYiJiTaxAmountZongShouRuHeJi = decimal.Parse(LB_InitialConstractTaxAmount.Text) + decimal.Parse(LB_SupplementConstractTaxAmount.Text) + decimal.Parse(LB_gexiangjiangli_sj.Text) + decimal.Parse(LB_BuKeYuJianFeiTaxAmount.Text);
            deYiJiTaxAmountZongShouRuHeJi -= decimal.Parse(LB_FirstYiJiShenJianETaxAmount.Text);
            LB_YiJiTaxAmountZongShouRuHeJi.Text = deYiJiTaxAmountZongShouRuHeJi.ToString("f6");

            //预计总收入合计E12/C12
            try
            {
                LB_YiJiTaxRateZongShouRuHeJi.Text = (decimal.Parse(LB_YiJiTaxAmountZongShouRuHeJi.Text) / decimal.Parse(LB_YiJiHeTongYiShuanJiaZongShouRuHeJi.Text)).ToString("f6");
            }
            catch
            {
                LB_YiJiTaxRateZongShouRuHeJi.Text = "0";
            }

            //预计总收入合计F7+F8+F9+F10-F11
            decimal deYiJiHeTongYiShuanJiaAfterTaxZongShouRuHeJi = decimal.Parse(LB_InitialConstractAfterTaxAmount.Text) + decimal.Parse(LB_SupplementConstractAfterTaxAmount.Text) + decimal.Parse(LB_gexiangjiangli_sh.Text) + decimal.Parse(LB_BuKeYuJianFeiAfterTaxAmount.Text);
            deYiJiHeTongYiShuanJiaAfterTaxZongShouRuHeJi -= decimal.Parse(LB_FirstYiJiShenJianEAfterTaxAmount.Text);
            LB_YiJiHeTongYiShuanJiaAfterTaxZongShouRuHeJi.Text = deYiJiHeTongYiShuanJiaAfterTaxZongShouRuHeJi.ToString("f6");

            //表单：“项目一次标价分离表”中“金额（不含税）”人工费
            LB_XiangMuFirstCiFengLiBiaoRenGongFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D1", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”人工费税率
            LB_XiangMuFirstCiFengLiBiaoRenGongFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field52", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”人工费税金
            LB_XiangMuFirstCiFengLiBiaoRenGongFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field53", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”人工费税后金额
            LB_XiangMuFirstCiFengLiBiaoRenGongFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field54", "my:riqi", strYearMonth);


            //表单：“项目一次标价分离表”中“金额（不含税）”设备费
            LB_XiangMuFirstCiFengLiBiaoSheBeiFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D2", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”设备费税率
            LB_XiangMuFirstCiFengLiBiaoSheBeiFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field56", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”设备费税金
            LB_XiangMuFirstCiFengLiBiaoSheBeiFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field57", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”设备费税后金额
            LB_XiangMuFirstCiFengLiBiaoSheBeiFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field58", "my:riqi", strYearMonth);


            //表单：“项目一次标价分离表”中“金额（不含税）”材料费
            LB_XiangMuFirstCiFengLiBiaoCaiLiaoFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D3", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”材料费税率
            LB_XiangMuFirstCiFengLiBiaoCaiLiaoFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field60", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”材料费税金
            LB_XiangMuFirstCiFengLiBiaoCaiLiaoFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field61", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”材料费税后金额
            LB_XiangMuFirstCiFengLiBiaoCaiLiaoFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field62", "my:riqi", strYearMonth);


            //表单：“项目一次标价分离表”中“金额（不含税）”机械使用费
            LB_XiangMuFirstCiFengLiBiaoJiJieShiYongFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D4", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”机械使用费税率
            LB_XiangMuFirstCiFengLiBiaoJiJieShiYongFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field64", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”机械使用费税金
            LB_XiangMuFirstCiFengLiBiaoJiJieShiYongFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field65", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”机械使用费税后金额
            LB_XiangMuFirstCiFengLiBiaoJiJieShiYongFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field66", "my:riqi", strYearMonth);



            //表单：“项目一次标价分离表”中“金额（不含税）”分包成本
            LB_XiangMuFirstCiFengLiBiaoFenBaoChengBen.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D5D51D52", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”分包成本税率
            LB_XiangMuFirstCiFengLiBiaoFenBaoChengBenTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field68", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”分包成本税金
            LB_XiangMuFirstCiFengLiBiaoFenBaoChengBenTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field69", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”分包成本税后金额
            LB_XiangMuFirstCiFengLiBiaoFenBaoChengBenAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field70", "my:riqi", strYearMonth);



            //表单：“项目一次标价分离表”中“金额（不含税）”建筑工程费
            LB_XiangMuFirstCiFengLiBiaoJianZuoGongChengFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D51", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”建筑工程费税率
            LB_XiangMuFirstCiFengLiBiaoJianZuoGongChengFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field72", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”建筑工程费税金
            LB_XiangMuFirstCiFengLiBiaoJianZuoGongChengFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field73", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”建筑工程费税后金额
            LB_XiangMuFirstCiFengLiBiaoJianZuoGongChengFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field74", "my:riqi", strYearMonth);


            //表单：“项目一次标价分离表”中“金额（不含税）”安装工程费
            LB_XiangMuFirstCiFengLiBiaoAnZhangGongChengFei.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D52", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”安装工程费税率
            LB_XiangMuFirstCiFengLiBiaoAnZhangGongChengFeiTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field76", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”安装工程费税金
            LB_XiangMuFirstCiFengLiBiaoAnZhangGongChengFeiTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field77", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”安装工程费税后金额
            LB_XiangMuFirstCiFengLiBiaoAnZhangGongChengFeiAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field78", "my:riqi", strYearMonth);


            //表单：“项目一次标价分离表”中“金额（不含税）”其它费用
            LB_XiangMuFirstCiFengLiBiaoQiTaFeiYong.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D6", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”其它费用税率
            LB_XiangMuFirstCiFengLiBiaoQiTaFeiYongTaxRate.Text = GetWorkFlowColumnTaxRateDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field80", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”其它费用税金
            LB_XiangMuFirstCiFengLiBiaoQiTaFeiYongTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field81", "my:riqi", strYearMonth);

            //表单：“项目一次标价分离表”中“金额（不含税）”其它费用税后金额
            LB_XiangMuFirstCiFengLiBiaoQiTaFeiYongAfterTaxAmount.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field82", "my:riqi", strYearMonth);


            //表单：“项目一次标价分离表”中“金额（不含税）”增值税附加
            LB_XiangMuFirstCiFengLiBiaoZengChiShuiFuJia.Text = GetWorkFlowColumnDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:D7F12", "my:riqi", strYearMonth);


            //"不可预见费表单及各项奖励"表单中“各项奖励”“合计”“金额（税前）”
            LB_BuKeYiJianBiaoGeXiangJiangLiHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sq", "my:riqi", strYearMonth); 
            LB_BuKeYiJianBiaoGeXiangJiangLiHeJiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID ,LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-slv", "my:riqi", strYearMonth);
            LB_BuKeYiJianBiaoGeXiangJiangLiHeJiTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sj", "my:riqi", strYearMonth);
            LB_BuKeYiJianBiaoGeXiangJiangLiHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sh", "my:riqi", strYearMonth);

            //最近的“项目二次标价分离表”中“不可预见费”“金额（不含税）”单元格的数据读取
            LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field159", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field166", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field193", "my:riqi", strYearMonth);
            LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field173", "my:riqi", strYearMonth);

            //默认从表单：“项目二次标价分离表”中“预计审减额”单元格的数据，当表单读不到数据，从“项目立项”模块读取。
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

            //二次预计总收入合计H7+H8+H9+H10-H11（以上1-5项合计自动计算结果）
            decimal deSecondYiJiHeTongYiShuanJiaZongShouRuHeJi = decimal.Parse(LB_InitialSecondConstractAmount.Text) + decimal.Parse(LB_ConstractSecondSupplementAmount.Text) + decimal.Parse(LB_BuKeYiJianBiaoGeXiangJiangLiHeJiAmount.Text) + decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiAmount.Text);
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJi -= decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianE.Text);
            LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text = deSecondYiJiHeTongYiShuanJiaZongShouRuHeJi.ToString();

            //二次预计总收入合计J7+J8+J9+J10-J11
            decimal deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount;
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount = decimal.Parse(LB_InitialSecondConstractTaxAmount.Text) + decimal.Parse(LB_ConstractSecondSupplementTaxAmount.Text) + decimal.Parse(LB_BuKeYiJianBiaoGeXiangJiangLiHeJiTaxAmount.Text) + decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiTaxAmount.Text);
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount -= decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianETaxAmount.Text);
            LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount.Text = deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount.ToString();

            //二次预计总收入合计E12/C12
            try
            {
                LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxRate.Text = (decimal.Parse(LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxAmount.Text) / decimal.Parse(LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text)).ToString("f6");
            }
            catch
            {
                LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxRate.Text = "0";
            }

            //二次预计总收入合计K7+K8+K9+K10-K11
            decimal deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount;
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount = decimal.Parse(LB_InitialSecondConstractAfterTaxAmount.Text) + decimal.Parse(LB_ConstractSecondSupplementAfterTaxAmount.Text) + decimal.Parse(LB_BuKeYiJianBiaoGeXiangJiangLiHeJiAfterTaxAmount.Text) + decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiAfterTaxAmount.Text);
            deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount -= decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianEAfterTaxAmount.Text);
            LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount.Text = deSecondYiJiHeTongYiShuanJiaZongShouRuHeJiAfterTaxAmount.ToString();

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与直接人工费的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieRenGongFeiYongAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:rengongfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieRenGongFeiYongTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field2", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieRenGongFeiYongTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:rengongfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieRenGongFeiYongAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field3", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与工资及各项保险单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiGeXiangBaoXianAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:gongzibaoxian", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiGeXiangBaoXianTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field6", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiGeXiangBaoXianTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field197", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiGeXiangBaoXianAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field7", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与奖金单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiJiangJinAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jiangjin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiJiangJinTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field10", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiJiangJinTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field198", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiJiangJinAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field11", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与其它补助单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiQiTaBuZhouAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitabuzhujintie", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiQiTaBuZhouTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field14", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiQiTaBuZhouTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field199", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiQiTaBuZhouAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field15", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与其它补助单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiWaiChuJinTieAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:waichubuzhu", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiWaiChuJinTieTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field18", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiWaiChuJinTieTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field200", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiWaiChuJinTieAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field19", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分包工程费单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenBaoGongZhengAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fenbaogognchengfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenBaoGongZhengTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field22", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenBaoGongZhengTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fenbaofeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenBaoGongZhengAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field23", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分包单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBaoAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:laowufenbao", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBaoTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field26", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBaoTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field202", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBaoAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field27", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分包1单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao1Amount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:laowufenbao1", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao1TaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field30", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao1TaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field203", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao1AfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field31", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分包2单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao2Amount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:laowufenbao2", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao2TaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field34", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao2TaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field204", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiLaoWuFenBao2AfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field35", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与专业分包单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiZhuanYeFenBaoAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zhuanyefenbao", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiZhuanYeFenBaoTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field38", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiZhuanYeFenBaoTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field205", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiZhuanYeFenBaoAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field39", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司自完单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenGongShiZiWanAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fengongsiziwan", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenGongShiZiWanTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field42", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenGongShiZiWanTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field206", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiFenGongShiZiWanAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field43", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司电仪单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiDianYuAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:dianyi", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiDianYuTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field139", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiDianYuTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field207", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieGongZhiJiDianYuAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field140", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司调装单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieDiaoZhuangAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:diaozhuang", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieDiaoZhuangTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field143", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieDiaoZhuangTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field208", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZiJieDiaoZhuangAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field144", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司直接料费单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhiJieLiaoFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zhijiecailiaofei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhiJieLiaoFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:cailiaofeishuilv", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhiJieLiaoFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:cailiaofeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhiJieLiaoFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field47", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司主材单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuCaiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zhucai", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuCaiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field50", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuCaiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field210", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuCaiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field51", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司辅材单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFuCaiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fucai", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFuCaiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field54", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFuCaiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field211", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFuCaiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field55", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司平库单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieCaiLiaoPingKuAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field306", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieCaiLiaoPingKuTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field307", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieCaiLiaoPingKuTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field308", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieCaiLiaoPingKuAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field309", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司底漆单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieDiQiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field310", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieDiQiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field311", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieDiQiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field312", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieDiQiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field313", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司机械费单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jixiefei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field58", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jixiefeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field59", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与分公司机械使用费单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieShiYongFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jixieshiyonfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieShiYongFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field62", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieShiYongFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field213", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJiJieShiYongFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field63", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【分公司自完费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFenGongShiZiWanAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:feigongsiziwan", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFenGongShiZiWanTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field66", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFenGongShiZiWanTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field214", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFenGongShiZiWanAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field67", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【其他】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qita", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field70", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field215", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field71", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【临时设施费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLingShiSheSiFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:linshisheshifei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLingShiSheSiFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field74", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLingShiSheSiFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:linshefeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLingShiSheSiFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field75", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【安全措施费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieAnQianChuShiFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:anquancuoshifei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieAnQianChuShiFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field78", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieAnQianChuShiFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:anquanfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieAnQianChuShiFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field79", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【水电费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieShuiDianFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:shuidianfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieShuiDianFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field126", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieShuiDianFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:shuidianfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieShuiDianFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field127", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【其它工程费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaGongChengFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitagongchengfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaGongChengFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field130", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaGongChengFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitagongchengfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaGongChengFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field131", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【其它费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitafei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field86", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitafeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field87", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【检测费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJianCheFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:jiancefei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJianCheFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field90", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJianCheFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field221", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieJianCheFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field91", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【外协加工费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieWaiXieJiaGongFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:waixiejiagongfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieWaiXieJiaGongFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field94", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieWaiXieJiaGongFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field222", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieWaiXieJiaGongFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field95", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【租赁费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuLingFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zulinfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuLingFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field98", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuLingFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field223", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZhuLingFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field99", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【劳动保护费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLaoDongBaoHuFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:laodongbaohufei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLaoDongBaoHuFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field102", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLaoDongBaoHuFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field224", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieLaoDongBaoHuFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field103", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【修理费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieXiuLiFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:xiulifei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieXiuLiFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field106", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieXiuLiFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field225", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieXiuLiFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field107", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【不可预见成本】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieBuKeYiJianChenBenAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:bukeyujian", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieBuKeYiJianChenBenTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field134", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieBuKeYiJianChenBenTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field226", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieBuKeYiJianChenBenAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field135", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【罚款】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFaKuanAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:fakuan", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFaKuanTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field147", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFaKuanTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field227", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieFaKuanAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field148", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【办公用品】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJiePanGongYongPingAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:bangongyongping", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJiePanGongYongPingTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field151", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJiePanGongYongPingTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field228", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJiePanGongYongPingAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field152", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【其它2】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTa2Amount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:qitafeiqita", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTa2TaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field110", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTa2TaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field229", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTa2AfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field111", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【动迁费】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieTongQianFeiAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:dongqianfei", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieTongQianFeiTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field114", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieTongQianFeiTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:dongqianfeishuijin", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieTongQianFeiAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field115", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【其它让利项】单元格的数据,即最新有效数据
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaLiangLiXiangAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field302", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaLiangLiXiangTaxRate.Text = GetWorkFlowColumnTaxRateLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field303", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaLiangLiXiangTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field304", "my:riqi", strYearMonth);
            LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieQiTaLiangLiXiangAfterTaxAmount.Text = GetWorkFlowColumnLastestMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field305", "my:riqi", strYearMonth);

            //表单：“二次标价分离目标成本组成表”逐月各表中最晚月（离当前日期最近）表的“金额（不含税）”列的与【总成本】单元格的数据,即最新有效数据
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

            //预计利润（总收入-总成本）,即H12-I52
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


            //******“制定合同”模块，“合同变更”标签下“变更时间”为本月的 变更金额=“变更后金额（税后）”汇总-“合同金额（税后）”
            LB_XiangMuHeTongBianGenHouAmount.Text = (decimal.Parse(GetConstractCurrentMonthAmountAfterChange(strProjectID)) - decimal.Parse(GetInitialConstractAmountAfterTax(strProjectID))).ToString("f6");
            LB_XiangMuHeTongBianGenHouTaxRate.Text = LB_InitialSecondConstractTaxRate.Text;
            LB_XiangMuHeTongBianGenHouTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBianGenHouAmount.Text) * decimal.Parse(LB_XiangMuHeTongBianGenHouTaxRate.Text)).ToString();

            //******本月的合同所有变更金额及所有增补合同金额 - 上月合同所有变更金额及所
            decimal deXiangMuHeTongBenYueBianGenJianQiShangYueHeTongAmount = decimal.Parse(GetConstractCurrentMonthAmountAfterChange(strProjectID)) + decimal.Parse(GetConstractCurrentMonthSupplementAmountAfterTax(strProjectID)) - decimal.Parse(GetConstractPirorMonthAmountAfterChange(strProjectID)) - decimal.Parse(GetConstractPriorMonthSupplementAmountAfterTax(strProjectID));
            LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongAmount.Text = deXiangMuHeTongBenYueBianGenJianQiShangYueHeTongAmount.ToString();
            LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongTaxRate.Text = LB_ConstractSecondSupplementTaxRate.Text;
            LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueBianGenJianQiShangYueHeTongTaxRate.Text)).ToString();


            //本月的H9“各项奖励”“合计”“金额（税前）”-上月的H9“各项奖励”“合计”“金额（税前）”
            LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiAmount.Text = (decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sq")) - decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", LanguageHandle.GetWord("BuKeYuJianFeiJiGeXiangJiangLi").ToString().Trim(), "my:gexiangjiangli-sq"))).ToString();
            LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiTaxRate.Text = LB_BuKeYiJianBiaoGeXiangJiangLiHeJiTaxRate.Text;
            LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueGeXiangJiangLiQianQiShangYueGeXiangJiangLiHeJiTaxRate.Text)).ToString();

            //值=本月的“不可预见费”-上月的“不可预见费”即 本月 H10-上月 H10
            LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field159", "my:riqi", strYearMonth)) - decimal.Parse(GetWorkFlowColumnPriorMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field159", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiTaxRate.Text = LB_XiangMuECiBiaoJiaFenLiBiaoBuKeYiJianFeiTaxRate.Text;
            LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueBuKeYuJianFeiQianQiShangYueBuKeYuJianFeiTaxRate.Text)).ToString();

            //值=本月的“预计审减额”-上月的“预计审减额”
            LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianEAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field160", "my:riqi", strYearMonth)) - decimal.Parse(GetWorkFlowColumnPriorMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:field160", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianETaxRate.Text = LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianETaxRate.Text;
            LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianETaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianEAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueYiJiShengJianEQianQiShangYueYiJiShengJianETaxRate.Text)).ToString();

            //值=本月的“预计总收入合计”-上月的“预计总收入合计”
            LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zongshouru", "my:riqi", strYearMonth)) - decimal.Parse(GetWorkFlowColumnPriorMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("ErCiBiaoJiaFenLiMuBiaoChengBen").ToString().Trim(), "my:zongshouru", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuTaxRate.Text = LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJiTaxRate.Text;
            LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuTaxAmount.Text = (decimal.Parse(LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuAmount.Text) * decimal.Parse(LB_XiangMuHeTongBenYueYiJiZongShouRuQianQiShangYueZongShouRuTaxRate.Text)).ToString();

            //当月实际发生成本（税前）人工费表-“rengongfeiheji”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenRenGongFeiHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:rengongfeiheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenRenGongFeiHeJiSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:rengongfeiheji");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenRenGongFeiHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBenRenGongFeiHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBenRenGongFeiHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:rengongfeiheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenRenGongFeiHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:rengongfeiheji");
            LB_XiangMuMiYueShiJiFaShengChengBenRenGongFeiHeJiTaxRate.Text = "0";

            //当月实际发生成本（税前）人工费表-“gongzibaoxian”
            LB_XiangMuCurrentMonthShiJiFaShengChengBengongzibaoxianHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:gongzibaoxian", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBengongzibaoxianHeJiAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:gongzibaoxian");
            LB_XiangMuCurrentMonthShiJiFaShengChengBengongzibaoxianHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBengongzibaoxianHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBengongzibaoxianHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:gongzibaoxian", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBengongzibaoxianHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:gongzibaoxian");
            LB_XiangMuMiYueShiJiFaShengChengBengongzibaoxianHeJiTaxRate.Text = "0";

            //当月实际发生成本（税前）人工费表-“gexiangjiangli ”
            LB_XiangMuCurrentMonthShiJiFaShengChengBengexiangjiangliHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:gexiangjiangli", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBengexiangjiangliHeJiAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:gexiangjiangli");
            LB_XiangMuCurrentMonthShiJiFaShengChengBengexiangjiangliHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBengexiangjiangliHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBengexiangjiangliHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:gexiangjiangli", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBengexiangjiangliHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:gexiangjiangli");
            LB_XiangMuMiYueShiJiFaShengChengBengexiangjiangliHeJiTaxRate.Text = "0";

            //当月实际发生成本（税前）人工费表-“qitabuzhu ”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitabuzhuHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:qitabuzhu", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitabuzhuHeJiAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:qitabuzhu");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitabuzhuHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBenqitabuzhuHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitabuzhuHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:qitabuzhu", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitabuzhuHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:qitabuzhu");
            LB_XiangMuMiYueShiJiFaShengChengBenqitabuzhuHeJiTaxRate.Text = "0";


            //当月实际发生成本（税前）人工费表-“waichubuzhu ”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaichubuzhuHeJiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:waichubuzhu", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaichubuzhuHeJiAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:waichubuzhu");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaichubuzhuHeJiTaxAmount.Text = "0";
            LB_XiangMuMiYueShiJiFaShengChengBenwaichubuzhuHeJiSumTaxAmount.Text = "0";
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaichubuzhuHeJiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:waichubuzhu", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaichubuzhuHeJiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:waichubuzhu");
            LB_XiangMuMiYueShiJiFaShengChengBenwaichubuzhuHeJiTaxRate.Text = "0";


            //当月实际发生成本（税前）分包用费表-laowufenbao1-sq+laowufenbao2-sq”
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

            //当月实际发生成本（税前）分包用费表-fenbaofeiheji”
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
  

            //当月实际发生成本（税前）分包用费表-laowufenbao1-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao1sqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao1-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao1sqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao1-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）分包用费表-laowufenbao2-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao2sqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao2sqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao2sqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao2sqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaowufenbao2sqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao2sqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:laowufenbao2-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenlaowufenbao2sqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:laowufenbao2-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）分包用费表-zhuanyefenbao-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzhuanyefenbaosqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhuanyefenbao-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzhuanyefenbaosqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:zhuanyefenbao-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzhuanyefenbaosqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhuanyefenbao-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzhuanyefenbaosqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:zhuanyefenbao-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzhuanyefenbaosqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhaunyefenbao-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzhuanyefenbaosqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:zhaunyefenbao-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenzhuanyefenbaosqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:zhuanyefenbao-slv", "my:riqi", strYearMonth);


            //当月实际发生成本（税前）分包用费表-fbfengongsiziwan-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfbfengongsiziwansqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fbfengongsiziwan-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfbfengongsiziwansqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fbfengongsiziwan-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfbfengongsiziwansqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fengongsiziwan-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfbfengongsiziwansqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fengongsiziwan-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfbfengongsiziwansqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fengongsiziwan-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfbfengongsiziwansqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fengongsiziwan-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenfbfengongsiziwansqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fengongsiziwan-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）分包用费表-dianyi-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBendianyisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendianyisqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:dianyi-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendianyisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendianyisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:dianyi-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendianyisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendianyisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:dianyi-sh");
            LB_XiangMuMiYueShiJiFaShengChengBendianyisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:dianyi-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）分包用费表-diaozhuang-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBendiaozhuangsqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendiaozhuangsqAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:diaozhuang-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendiaozhuangsqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendiaozhuangsqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:diaozhuang-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendiaozhuangsqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendiaozhuangsqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:diaozhuang-sh");
            LB_XiangMuMiYueShiJiFaShengChengBendiaozhuangsqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:diaozhuang-slv", "my:riqi", strYearMonth);


            //当月实际发生成本（税前）材料费表-cailiaofei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-slv", "my:riqi", strYearMonth);


            //当月实际发生成本（税前）材料费表-zhucai-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeizhucaisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:zhucai-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeizhucaisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:zhucai-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeizhucaisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:zhucai-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeizhucaisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:zhucai-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeizhucaisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:zhucai-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeizhucaisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:zhucai-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeizhucaisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:zhucai-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）材料费表-fucai-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeifucaisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:fucai-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeifucaisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:fucai-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeifucaisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:fucai-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeifucaisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:fucai-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeifucaisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:fucai-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeifucaisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:fucai-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeifucaisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:fucai-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）材料费表-pingku-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeipingkusqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:pingku-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeipingkusqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:pingku-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeipingkusqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:pingku-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeipingkusqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:pingku-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeipingkusqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:pingku-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeipingkusqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:pingku-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeipingkusqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:pingku-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）材料费表-diqi-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeidiqisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:diqi-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeidiqisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:diqi-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeidiqisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:diqi-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeidiqisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:diqi-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBencailiaofeidiqisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:diqi-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeidiqisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:diqi-sh");
            LB_XiangMuMiYueShiJiFaShengChengBencailiaofeidiqisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:diqi-slv", "my:riqi", strYearMonth);


            //当月实际发生成本（税前）机械设备租赁费表-jixieshiyongfei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixieshiyongfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixieshiyongfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixieshiyongfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）机械设备租赁费表-jx-fengongsiziwan-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxfengongsiziwansqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxfengongsiziwansqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxfengongsiziwansqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:fengongsiziwan-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）机械设备租赁费表-jx-qita-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxqitasqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-qita-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxqitasqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-qita-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjxqitasqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jx-qita-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）机械设备租赁费表-jixiezulinshuiqianheji”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixiezulinshuiqianhejiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulinshuiqianheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixiezulinshuiqianheji");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixiezulinshuiqianhejiTaxAmount.Text = (decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixieshiyongfei-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-fengongsiziwan-sj", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jx-qita-sj", "my:riqi", strYearMonth))).ToString();
            LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiSumTaxAmount.Text = (decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenjixieshiyongfeisqSumTaxAmount.Text) + decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenjxfengongsiziwansqSumTaxAmount.Text) + decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenjxqitasqSumTaxAmount.Text)).ToString();
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjixiezulinshuiqianhejiAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulinshuihouheji", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixiezulinshuihouheji");
            LB_XiangMuMiYueShiJiFaShengChengBenjixiezulinshuiqianhejiTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulin-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）临时设施费表-linshisheshi-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlinshisheshisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlinshisheshisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlinshisheshisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlinshisheshisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlinshisheshisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlinshisheshisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenlinshisheshisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）安全措施费表-anquancuoshi-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenanquancuoshisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenanquancuoshisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenanquancuoshisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenanquancuoshisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenanquancuoshisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenanquancuoshisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenanquancuoshisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）水电费表-shuidianfei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenshuidianfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenshuidianfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenshuidianfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenshuidianfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenshuidianfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenshuidianfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenshuidianfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他工程费用表-qitagongchengfei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitagongchengfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitagongchengfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagongchengfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitagongchengfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitagongchengfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagongchengfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitagongchengfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagognchengfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitagongchengfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagognchengfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenqitagongchengfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-qitafeiheji-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitafeihejisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafeiheji-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitafeihejisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitafeiheji-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitafeihejisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitafeihejisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitaqita-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitafeihejisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafeiheji-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitafeihejisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitafeiheji-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenqitafeihejisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-jiancefei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjiancefeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:jiancefei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjiancefeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:jiancefei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjiancefeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:jiancefei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjiancefeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:jiancefei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenjiancefeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:jiancefei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenjiancefeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:jiancefei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenjiancefeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:jiancefei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-waixiejiagongfei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaixiejiagongfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:waixiejiagongfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaixiejiagongfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:waixiejiagongfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaixiejiagongfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:waixiejiagongfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaixiejiagongfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:waixiejiagongfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenwaixiejiagongfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:waixiejiagongfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenwaixiejiagongfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:waixiejiagongfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenwaixiejiagongfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:waixiejiagongfei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-zulinfei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzulinfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:zulinfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzulinfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:zulinfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzulinfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:zulinfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzulinfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:zulinfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenzulinfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:zulinfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenzulinfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:zulinfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenzulinfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:zulinfei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-laodongbaohufei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaodongbaohufeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:laodongbaohufei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaodongbaohufeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:laodongbaohufei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaodongbaohufeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:laodongbaohufei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaodongbaohufeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:laodongbaohufei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenlaodongbaohufeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:laodongbaohufei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenlaodongbaohufeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:laodongbaohufei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenlaodongbaohufeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:laodongbaohufei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-xiulifei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenxiulifeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:xiulifei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenxiulifeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:xiulifei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenxiulifeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:xiulifei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenxiulifeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:xiulifei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenxiulifeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:xiulifei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenxiulifeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:xiulifei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenxiulifeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:xiulifei-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-bukeyujian-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbukeyujiansqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bukeyujian-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbukeyujiansqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bukeyujian-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbukeyujiansqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bukeyujian-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbukeyujiansqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bukeyujian-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbukeyujiansqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bukeyujian-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbukeyujiansqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bukeyujian-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenbukeyujiansqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bukeyujian-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-fakuan-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfakuansqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:fakuan-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfakuansqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:fakuan-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfakuansqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:fakuan-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfakuansqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:fakuan-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenfakuansqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:fakuan-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenfakuansqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:fakuan-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenfakuansqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:fakuan-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-bangongyongping-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbangongyongpingsqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bangongyongping-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbangongyongpingsqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bangongyongping-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbangongyongpingsqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bangongyongping-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbangongyongpingsqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bangongyongping-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenbangongyongpingsqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bangongyongping-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenbangongyongpingsqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:bangongyongping-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenbangongyongpingsqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:bangongyongping-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）其他费表-qitaqita-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitaqitasqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitaqitasqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitaqita-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitaqitasqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitaqitasqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitaqita-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenqitaqitasqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenqitaqitasqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitaqita-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenqitaqitasqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitaqita-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前） 动迁费表-dongqianfei-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBendongqianfeisqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendongqianfeisqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendongqianfeisqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendongqianfeisqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBendongqianfeisqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBendongqianfeisqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sh");
            LB_XiangMuMiYueShiJiFaShengChengBendongqianfeisqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-slv", "my:riqi", strYearMonth);


            //当月实际发生成本（税前） 让利成本项表-ranglichengben-sq”
            LB_XiangMuCurrentMonthShiJiFaShengChengBenranglichengbensqAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sq", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenranglichengbensqSumAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sq");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenranglichengbensqTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sj", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenranglichengbensqSumTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sj");
            LB_XiangMuCurrentMonthShiJiFaShengChengBenranglichengbensqAfterTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sh", "my:riqi", strYearMonth);
            LB_XiangMuMiYueShiJiFaShengChengBenranglichengbensqSumAfterTaxAmount.Text = GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sh");
            LB_XiangMuMiYueShiJiFaShengChengBenranglichengbensqTaxRate.Text = GetWorkFlowColumnTaxRateCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-slv", "my:riqi", strYearMonth);

            //当月实际发生成本（税前）M15+M16+M17+M18+M21+M22+M23+M24+M25+M26+M28+M29+M30+M31+M33+M34+M35+M36+M37+M38+M39+M41+M42+M43+M44+M45+M46+M47+M48+M49+M50+M51   或者  M14+M19+M27+M32+M36+M37+M38+M39+M40+M50+M51
            decimal deSQSJCB = decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:rengongfeiheji", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fenbaofeiheji", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulinshuiqianheji", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sq", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagongchengfei-sq", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafeiheji-sq", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sq", "my:riqi", strYearMonth));
            deSQSJCB += decimal.Parse( GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sq", "my:riqi", strYearMonth));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenAmount.Text = deSQSJCB.ToString();

          
            //“当月实际发生成本（税前）”即： M52 的每月累加
            decimal deSQSJCBHJ = decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:rengongfeiheji")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fenbaofeiheji"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixiezulinshuiqianheji"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sq"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagongchengfei-sq"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitafeiheji-sq")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sq"));
            deSQSJCBHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sq"));
            LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text = deSQSJCBHJ.ToString();

            //“当月实际发生成本（税金）”O21+O22+O23+O24+O25+O26+O28+O29+O30+O31+O33+O34+O35+O36+O37+O38+O39+O41+O42+O43+O44+O45+O46+O47+O48+O49+O50+O51   或者  O14+O19+O27+O32+O36+O37+O38+O39+O40+O50+O51
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

            //“实际税金（当月）”即： O52 的每月累加
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

            //“当月实际发生成本（税后）”Q15+Q16+Q17+Q18+Q21+Q22+Q23+Q24+Q25+Q26+Q28+Q29+Q30+Q31+Q33+Q34+Q35+Q36+Q37+Q38+Q39+Q41+Q42+Q43+Q44+Q45+Q46+Q47+Q48+Q49+Q50+Q51   或者  Q14+Q19+Q27+Q32+Q36+Q37+Q38+Q39+Q40+Q50+Q51
            decimal deSQSJSH = decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProjectLaborCostSheet", "my:rengongfeiheji", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SubcontractingFee", "my:fenbaoshuihouheji", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MaterialCost", "my:cailiaofei-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "MechanicalEquipmentLeaseFee", "my:jixiezulinshuihouheji", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "TemporaryFacilitiesFee", "my:linshisheshi-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "SafetyMeasuresFee", "my:anquancuoshi-sh", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "UtilitiesFee", "my:shuidianfei-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherEngineeringCosts", "my:qitagognchengfei-sh", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherCosts", "my:qitafeiheji-sh", "my:riqi", strYearMonth)) + decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "RelocationFee", "my:dongqianfei-sh", "my:riqi", strYearMonth));
            deSQSJSH += decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "OtherConcessions", "my:ranglichengben-sh", "my:riqi", strYearMonth));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenAfterTaxAmount.Text = deSQSJSH.ToString();

            //当月实际发生成本（税后）即： Q52 的每月累加
            decimal deSQCBSJHJ = decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "ProjectLaborCostSheet", "my:rengongfeiheji")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SubcontractingFee", "my:fenbaoshuihouheji"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MaterialCost", "my:cailiaofei-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "MechanicalEquipmentLeaseFee", "my:jixiezulinshuihouheji"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "TemporaryFacilitiesFee", "my:linshisheshi-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "SafetyMeasuresFee", "my:anquancuoshi-sh"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "UtilitiesFee", "my:shuidianfei-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherEngineeringCosts", "my:qitagognchengfei-sh"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherCosts", "my:qitafeiheji-sh")) + decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "RelocationFee", "my:dongqianfei-sh"));
            deSQCBSJHJ += decimal.Parse(GetWorkFlowColumnSumData(strProjectID, strYearMonth, "my:riqi", "OtherConcessions", "my:ranglichengben-sh"));
            LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenSumAfterTaxAmount.Text = deSQCBSJHJ.ToString();

            //税金/税前金额 即：O52/M52
            try
            {
                LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenTaxRate.Text = (decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengChengBenTaxAmount.Text) / decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenAmount.Text)).ToString("f2");
            }
            catch
            {
                LB_XiangMuCurrentMonthShiJiFaShengChengBenShiJiChengBenTaxRate.Text = "0";
            }

            //利润（总收入-累计实际发生成本（税前 ））,即H12-N52
            LB_XiangMuCurrentMonthShiJiFaShengProfitAmount.Text = (decimal.Parse(LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text) - decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text)).ToString();

            //销项税额=∑实际结算额*税率,即：来自表单“   进度款情况表”“税金”“合计”的实际总税额,即表单: 控件“xiaoxiangshuijinheji”        
            LB_XiangMuCurrentMonthShiJiFaShengProfitXiaoXiangTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProgressPaymentSettlementReport", "my:xiaoxiangshuijinheji", "my:riqi", strYearMonth);

            //进项税额：来自所有成本项表单填写的实际总税额,即:P52
            LB_XiangMuCurrentMonthShiJiFaShengProfitJingXiangTaxAmount.Text = LB_XiangMuCurrentMonthShiJiFaShengChengBenSumTaxAmount.Text;

            //税金附加=税金*12%   即： M56 * 12 %
            LB_XiangMuCurrentMonthShiJiFaShengProfitFuJiaTaxAmount.Text = ((decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengProfitJingXiangTaxAmount.Text) * 12) / 100).ToString();

            //表单：“项目一次标价分离表”中“金额（不含税）”对应本行同名行单元格的数据
            LB_XiangMuYiCiFenLiBiaoMaoLiAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:EAD", "my:riqi", strYearMonth);

            LB_XiangMuYiCiFenLiBiaoShuiJingAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:field91", "my:riqi", strYearMonth);
            LB_XiangMuYiCiFenLiBiaoChengBenJiaShuiJingAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:GDF", "my:riqi", strYearMonth);
            LB_XiangMuYiCiFenLiBiaoYiJiProfitAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, LanguageHandle.GetWord("YiCiBiaoJiaFenLiBiao").ToString().Trim(), "my:HAG", "my:riqi", strYearMonth);

            //“项目管理”模块“项目立项”界面”“项目总预算：
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

            //销项税额=∑实际结算额*税率,即：来自表单“ 进度款情况表”“税金”“合计”的实际总税额,即表单: 控件“xiaoxiangshuijinheji”
            LB_XiangMuCurrentMonthShiJiFaShengProfitSaleTaxAmount.Text = GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProgressPaymentSettlementReport", "my:xiaoxiangshuijinheji", "my:riqi", strYearMonth);

            try
            {
                //综合税负=(收入类销项税总额?支出类进项税总额)/（不含税实际收入）  即：(M54-M55)/（“进度款情况表”“开票金额”“合计”即：控件“kpjeheji”的数据）
                LB_XiangMuTotalSummaryTaxAmount.Text = ((decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengProfitSaleTaxAmount.Text) - decimal.Parse(LB_XiangMuCurrentMonthShiJiFaShengProfitXiaoXiangTaxAmount.Text)) / decimal.Parse(GetWorkFlowColumnCurrentMonthDataByMaxFieldValue(strProjectID, "ProgressPaymentSettlementReport", "my:kpjeheji", "my:riqi", strYearMonth))).ToString();
            }
            catch
            {
                LB_XiangMuTotalSummaryTaxAmount.Text = "0";
            }


            //----------------------------第二份报表------------------------------------
            LB_ProjectCode2.Text = LB_ProjectCode.Text;
            LB_ProjectName2.Text = LB_ProjectName.Text;

            //“项目成本分析及动态管理表”“预计总收入合计”行，“合同预算价（二次）”列的单元格数据，即：H12
            LB_XiangMuHeTongYuShuanJiaErCiAmount.Text = LB_SecondYiJiHeTongYiShuanJiaZongShouRuHeJi.Text;


            //“项目成本分析及动态管理表”“目标成本（二次）”列“总成本”行即：I52
            LB_XiangMuMuBiaoChengBenErCiAmount.Text = LB_XiangMuErChiBiaoJiaFenLiBiaoZhiJieZongChengBenAmount.Text;

            //“项目成本分析及动态管理表”“累计实际发生成本（税前 ）”列，“总成本”行的单元格数据，即：N52
            LB_XiangMuLieJiShiJiFaShengChengBenAmount.Text = LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text;

            //(“项目成本分析及动态管理表”“累计实际发生成本（税前 ）”列，“总成本”行的单元格数据，即：N52)+(“总收入”即本表B4-表单"进度款情况表"“申请金额”列“合计”行数值)*(1-审减率（二次）：“项目成本分析及动态管理表”L11)
            LB_XiangMuYiJiFaShengZongChengBenAmount.Text = ((decimal.Parse(LB_XiangMuMiYueShiJiFaShengChengBenShiJiChengBenSumAmount.Text) + decimal.Parse(LB_XiangMuHeTongYuShuanJiaErCiAmount.Text)) * (1 - decimal.Parse(LB_XiangMuECiBiaoJiaFenLiBiaoBYiJiShenJianLu.Text))).ToString("f6");


            try
            {
                //（“进度”*“总收入（预算）”）即本表 E5/B5*B4
                LB_XiangMuHeTongJingDuShengYuZongShouRuAmount.Text = ((decimal.Parse(LB_XiangMuLieJiShiJiFaShengChengBenAmount.Text) / decimal.Parse(LB_XiangMuMuBiaoChengBenErCiAmount.Text)) * (decimal.Parse(LB_XiangMuHeTongYuShuanJiaErCiAmount.Text))).ToString("f6");
            }
            catch
            {
                LB_XiangMuHeTongJingDuShengYuZongShouRuAmount.Text = "0";
            }

            //从项目管理模块本项目“项目立项”界页面的“包干总额：”取数
            try
            {
                LB_XiangMuYiJiFaShengBaoGuanZongEAmount.Text = GetWZProject(strProjectID).BaoGuanZongE.ToString("f6");
            }
            catch
            {
                LB_XiangMuYiJiFaShengBaoGuanZongEAmount.Text = "0";
            }

            //成本实际值/项目总成本（显示为百分比形式）：（实际：成本）/（预算：成本）  即本表：E5/B5*100%
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
                //当月实际收入-当月实际成本：（（“进度”*“总收入（预算）”）-（实际：成本）） 即本表：B9*B4-E5
                LB_XiangMuYiJiFaShengLiRenMaoLiAmount.Text = ((decimal.Parse(LB_XiangMuYiJiFaShengJingDuAmount.Text) / decimal.Parse(LB_XiangMuMuBiaoChengBenErCiAmount.Text)) - decimal.Parse(LB_XiangMuLieJiShiJiFaShengChengBenAmount.Text)).ToString("f6");
            }
            catch
            {
                LB_XiangMuYiJiFaShengLiRenMaoLiAmount.Text = "0";
            }


            //奖金包干总额月度奖金额明细表
            SaveProjectDetailedListOfMonthlyBonusAmount(strProjectID, strYearMonth);

            //项目成本收入分析总图
            SaveProjectCostIncomeAnalysisGeneralChart(strProjectID, strYearMonth);
        }
        catch (Exception err)
        {
            LB_ErrorText.Text = "Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace;
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }
    }

    //奖金包干总额月度奖金额明细表
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

    //项目成本收入分析总图
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

        //ShareClass.CreateAnalystFourColumnChart(strHQL, Chart1, SeriesChartType.Column, strChartTitle, "MonthNumber", "CurrentMonthTotalCost", "CumulativeActualTaxCost", "CumulativeActualAfterTaxCost", "AccumulationSettlement", "Default", "当月二次目标成本（税后）", "累计实际发生成本（税前）", "当月实际成本（税后）累计", "累计结算");
        //Chart1.Visible = true;
    }


    //取流程表单相应格子最新日期的费用
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


    //取流程表单相应格子的费用的汇总
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
   

    //依字段排序取逐月各表中最晚月（离当前日期最近）的流程表单相应格子的费用
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

    //取当月的流程表单相应格子的费用
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

    //取上月的流程表单相应格子的费用
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

    //取流程表单相应格子最新日期的税率数据
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

    //依字段排序取逐月各表中最晚月（离当前日期最近）的流程表单相应格子的税率数据
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

    //取当月的流程表单相应格子的税率数据
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


    //取得项目合同主要内容
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

    //取得项目合同异常描述
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

    //取得项目税前合同额
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

    //取得项目税后合同额
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

    //取得项目合同的税率
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

    //取得项目合同的税金
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

    //取得项目增补合同税前合同额
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

    //取得项目增补税后合同额
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

    //取得项目增补合同的税金
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

    //取得项目增补合同变更的的税金
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

    //取得项目合同变更记录的合同额
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


    //取得项目合同本月变更金额汇总
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

    //取得项目合同上月变更金额汇总
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

    //取得项目本月增补合同额总额
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

    //取得项目上月增补合同额总额
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