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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTBaseDataOuter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "基础数据(外置)", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadBarType();

            LoadReqType();
            LoadDefectType();

            LoadAssetType();

            LoadProductProcess();
            LoadIndustryType();

            LoadTaskOperation();

            LoadMeetingType();
            LoadMTType();
            LoadChangeType();

            LoadUnit();
            LoadActorGroupType();

            LoadTaskType();
            LoadTaskRecordType();

            LoadConstractType();

            LoadDailyWorkUnitBonus();


            LoadAttendanceRule();

            LoadCarType();

            LoadPlanType();
            LoadReportType();

            LoadKPIType();

            LoadOilType();

            LoadCustomerQuestionType();

            LoadCurrencyType();

            LoadBookReaderType();//用工类型



            LoadStockCountMethod();//出入库存算法

            LoadConststactBigType();//合同大类

            LoadLeaveType();//请假类型

            LoadDayHourNum();//一天工作时间设置

            LoadReceivePayWay();

            LoadBank();

            LoadDepartPosition();

            LoadUserDuty();

            LoadKPICheckTypeWeight();

            LoadSiteCustomerServiceOperator();

            LoadGoodsShipmentType();

            LoadGoodsCheckInType();

            LoadCodeRule();

            LoadCustomerQuestionStage();
            LoadCustomerQuestionCustomerStage();

            LoadSupplierBigType();
            LoadSupplierSmallType();

            LoadBMBidType();

            LoadPackingType();
            LoadSaleType();

            LoadConstractRadio();

            LoadInvoiceType();
            LoadOvertimeType();
            LoadFestivalsType();

            LoadTenderContent();

            LoadFundingSource();



            NB_ScheduleLimitedDays.Amount = GetScheduleLimitedDays();

            NB_WeekendFirstDay.Amount = decimal.Parse(ShareClass.GetWeekendFirstDay());
            NB_WeekendSecondDay.Amount = decimal.Parse(ShareClass.GetWeekendSecondDay());
            DL_WeekendsAreWorkdays.SelectedValue = ShareClass.GetWeekendsAreWorkdays();
        }
    }

    protected void DL_BarType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strBarType;

        strBarType = DL_BarType.SelectedValue.Trim();

        try
        {
            strHQL = "Update T_BarType Set Type = '" + strBarType + "'";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJC") + "')", true);
        }

    }

    protected void DL_StockCountMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strMethod;

        strMethod = DL_StockCountMethod.SelectedValue.Trim();

        try
        {
            strHQL = "Update T_GoodsStockCountMethod Set Method = '" + strMethod + "'";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJC") + "')", true);
        }

    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strDuty = ((Button)e.Item.FindControl("BT_Duty")).Text;
        string strKeyWord = e.Item.Cells[1].Text.Trim();
        string strSortNumber = e.Item.Cells[2].Text.Trim();

        TB_Duty.Text = strDuty;
        LB_Duty_Backup.Text = strDuty;
        TB_DutyKeyWord.Text = strKeyWord;
        LB_DutyKeyWord_Backup.Text = strKeyWord;
        TB_DutySort.Text = strSortNumber;

        BT_UpdateDuty.Enabled = true;
        BT_DeleteDuty.Enabled = true;
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_ReqType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_ReqType.Text = strType;
        TB_ReqTypeSort.Text = strSortNumber;
    }

    protected void DataGrid47_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_DefectType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_DefectType.Text = strType;
        TB_DefectTypeSort.Text = strSortNumber;
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strProcessName = ((Button)e.Item.FindControl("BT_ProcessName")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_ProcessName.Text = strProcessName;
        TB_ProcessSort.Text = strSortNumber;
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_AssetType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_AssetType.Text = strType;
        TB_AssetTypeSort.Text = strSortNumber;
    }

    protected void DataGrid5_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_SupplierBigType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();
        TB_SupplierBigType.Text = strType;
        TB_SupplierBigTypeSort.Text = strSortNumber;
    }

    protected void DataGrid6_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_SupplierSmallType")).Text.Trim();
        string strBigType = e.Item.Cells[1].Text;
        string strSortNumber = e.Item.Cells[2].Text.Trim();

        Label71.Text = strBigType;

        TB_SupplierSmallType.Text = strType;
        TB_SupplierSmallTypeSort.Text = strSortNumber;
        DL_SupplierBigType.SelectedValue = strBigType;
    }

    protected void DataGrid7_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOperation = ((Button)e.Item.FindControl("BT_Operation")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_TaskOperation.Text = strOperation;
        TB_OperationSortNumber.Text = strSortNumber;
    }

    protected void DataGrid8_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_BMBidType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_BMBidType.Text = strType;
        TB_BMBidTypeSort.Text = strSortNumber;
    }

    protected void DataGrid9_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_MeetingType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_MeetingType.Text = strType;
        TB_MeetingTypeSort.Text = strSortNumber;
    }

    protected void DataGrid10_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_MTType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_MTType.Text = strType;
        TB_MTTypeSort.Text = strSortNumber;
    }

    protected void DataGrid12_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_SaleType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_SaleType.Text = strType;
        TB_SaleTypeSort.Text = strSortNumber;
    }


    protected void DataGrid11_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_ChangeType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_ChangeType.Text = strType;
        TB_ChangeTypeSort.Text = strSortNumber;
    }

    protected void DataGrid14_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUnitName = ((Button)e.Item.FindControl("BT_UnitName")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_UnitName.Text = strUnitName;
        TB_UnitSort.Text = strSortNumber;
    }

    protected void DataGrid15_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strActorGroupType = ((Button)e.Item.FindControl("BT_ActorGroupType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_ActorGroupType.Text = strActorGroupType;
        TB_ActorGroupTypeSort.Text = strSortNumber;
    }


    protected void DataGrid16_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOvertimeType = ((Button)e.Item.FindControl("BT_OvertimeType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_OvertimeType.Text = strOvertimeType;
        TB_OvertimeTypeSort.Text = strSortNumber;
    }

    protected void DataGrid18_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_TaskType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_TaskType.Text = strType;
        TB_TaskTypeSort.Text = strSortNumber;

    }

    protected void DataGrid19_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_TaskRecordType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_TaskRecordType.Text = strType;
        TB_TaskRecordTypeSort.Text = strSortNumber;
    }



    protected void DataGrid22_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_ConstractType")).Text.Trim();
        string strKeyWord = e.Item.Cells[1].Text.Trim();
        string strSortNumber = e.Item.Cells[2].Text.Trim();

        TB_ConstractType.Text = strType;
        TB_ConstractTypeKeyWord.Text = strKeyWord;
        TB_ConstractTypeSort.Text = strSortNumber;
    }

    protected void DataGrid23_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
        string strEveryCharPrice = e.Item.Cells[1].Text.Trim();
        string strCharUpper = e.Item.Cells[2].Text.Trim();
        string strEveryDocPrice = e.Item.Cells[3].Text.Trim();
        string strDocUpper = e.Item.Cells[4].Text.Trim();


        LB_DailyWorkUnitBonusID.Text = strID;
        NB_EveryCharPrice.Amount = decimal.Parse(strEveryCharPrice);
        NB_EveryDocPrice.Amount = decimal.Parse(strEveryDocPrice);
        NB_CharUpper.Amount = int.Parse(strCharUpper);
        NB_DocUpper.Amount = int.Parse(strDocUpper);
    }


    protected void DataGrid24_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

        string strHQL = " From AttendanceRule as attendanceRule where attendanceRule.ID = " + strID;
        AttendanceRuleBLL attendanceRuleBLL = new AttendanceRuleBLL();
        IList lst = attendanceRuleBLL.GetAllAttendanceRules(strHQL);

        AttendanceRule attendanceRule = (AttendanceRule)lst[0];

        LB_AttendanceRuleID.Text = strID;

        TB_MCheckInStart.Text = attendanceRule.MCheckInStart.Trim();
        TB_MCheckInEnd.Text = attendanceRule.MCheckInEnd.Trim();
        DDL_MCheckInIsMust.SelectedValue = attendanceRule.MCheckInIsMust.Trim();

        TB_MCheckOutStart.Text = attendanceRule.MCheckOutStart.Trim();
        TB_MCheckOutEnd.Text = attendanceRule.MCheckOutEnd.Trim();
        DDL_MCheckOutIsMust.SelectedValue = attendanceRule.MCheckOutIsMust.Trim();

        TB_ACheckInStart.Text = attendanceRule.ACheckInStart.Trim();
        TB_ACheckInEnd.Text = attendanceRule.ACheckInEnd.Trim();
        DDL_ACheckInIsMust.SelectedValue = attendanceRule.ACheckInIsMust.Trim();

        TB_ACheckOutStart.Text = attendanceRule.ACheckOutStart.Trim();
        TB_AChectOutEnd.Text = attendanceRule.ACheckOutEnd.Trim();
        DDL_ACheckOutIsMust.SelectedValue = attendanceRule.ACheckOutIsMust.Trim();

        TB_NCheckInStart.Text = attendanceRule.NCheckInStart.Trim();
        TB_NCheckInEnd.Text = attendanceRule.NCheckInEnd.Trim();
        DDL_NCheckInIsMust.SelectedValue = attendanceRule.NCheckInIsMust.Trim();

        TB_NCheckOutStart.Text = attendanceRule.NCheckOutStart.Trim();
        TB_NCheckOutEnd.Text = attendanceRule.NCheckOutEnd.Trim();
        DDL_NCheckOutIsMust.SelectedValue = attendanceRule.NCheckOutIsMust.Trim();

        TB_OCheckInStart.Text = attendanceRule.OCheckInStart.Trim();
        TB_OCheckInEnd.Text = attendanceRule.OCheckInEnd.Trim();
        DDL_OCheckInIsMust.SelectedValue = attendanceRule.OCheckInIsMust.Trim();

        TB_OCheckOutStart.Text = attendanceRule.OCheckOutStart.Trim();
        TB_OCheckOutEnd.Text = attendanceRule.OCheckOutEnd.Trim();
        DDL_OCheckOutIsMust.SelectedValue = attendanceRule.OCheckOutIsMust.Trim();

        NB_LargestDistance.Amount = attendanceRule.LargestDistance;

        TB_Longitude.Text = attendanceRule.OfficeLongitude.Trim();
        TB_Latitude.Text = attendanceRule.OfficeLatitude.Trim();

        TB_Address.Text = attendanceRule.Address;
    }


    protected void DataGrid26_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_IndustryType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();


        TB_IndustryType.Text = strType;
        TB_IndustryTypeSort.Text = strSortNumber;
    }

    protected void DataGrid27_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_CarType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();


        TB_CarType.Text = strType;
        TB_CarTypeSort.Text = strSortNumber;
    }

    protected void DataGrid28_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_PlanType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();


        TB_PlanType.Text = strType;
        TB_PlanTypeSort.Text = strSortNumber;
    }

    protected void DataGrid29_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_ReportType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();


        TB_ReportType.Text = strType;
        TB_ReportTypeSort.Text = strSortNumber;
    }

    protected void DataGrid30_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_KPIType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();


        TB_KPIType.Text = strType;
        TB_KPITypeSort.Text = strSortNumber;
    }

    protected void DataGrid32_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_CustomerQuestionType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_CustomerQuestionType.Text = strType;
        TB_CustomerQuestionTypeSort.Text = strSortNumber;
    }

    protected void DataGrid33_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId = ((Button)e.Item.FindControl("BT_ID")).Text;
        string strOilName = e.Item.Cells[1].Text.Trim();

        txt_OilName.Text = strOilName.Substring(0, strOilName.IndexOf("@"));
        txt_OilType.Text = strOilName.Substring(strOilName.IndexOf("@"));
        txt_ID.Text = strId;
    }
    protected void DataGrid37_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_ConstractBigType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();


        TB_ConstractBigType.Text = strType;
        TB_ConstractBigTypeSortNo.Text = strSortNumber;
    }

    protected void DataGrid38_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_ReceivePayType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_ReceivePayType.Text = strType;
        TB_ReceivePayTypeSort.Text = strSortNumber;
    }

    protected void DataGrid39_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strBankName = ((Button)e.Item.FindControl("BT_BankName")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_BankName.Text = strBankName;
        TB_BankSort.Text = strSortNumber;
    }

    protected void DataGrid40_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = ((Button)e.Item.FindControl("BT_PositionID")).Text;
        string strPosition = e.Item.Cells[1].Text.Trim();
        string strSortNumber = e.Item.Cells[2].Text.Trim();

        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid40.Items.Count; i++)
            {
                DataGrid40.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            LB_PositionID.Text = strID;
            TB_Position.Text = strPosition;
            TB_DepartPositionSort.Text = strSortNumber;

            BT_UpdatePosition.Enabled = true;
            BT_DeletePosition.Enabled = true;
        }
    }

    protected void DataGrid41_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strWebSite = ((Button)e.Item.FindControl("BT_WebSite")).Text;
        string strUserCode = e.Item.Cells[1].Text.Trim();
        string strUserName = e.Item.Cells[2].Text.Trim();
        string strSortNumber = e.Item.Cells[3].Text.Trim();

        if (e.CommandName != "Page")
        {
            for (int i = 0; i < DataGrid41.Items.Count; i++)
            {
                DataGrid41.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            TB_WebSite.Text = strWebSite;
            TB_SiteUserCode.Text = strUserCode;
            TB_SiteUserName.Text = strUserName;
            TB_WebSiteSort.Text = strSortNumber;
        }
    }

    protected void DataGrid42_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_GoodsShipmentType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();


        TB_GoodsShipmentType.Text = strType;
        TB_GoodsShipmentSort.Text = strSortNumber;
    }

    protected void DataGrid43_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_GoodsCheckInType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();


        TB_GoodsCheckInType.Text = strType;
        TB_GoodsCheckInSort.Text = strSortNumber;
    }

    protected void DataGrid44_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID = ((Button)e.Item.FindControl("BT_CodeRuleID")).Text.Trim();

        string strHQL;

        strHQL = "Select CodeType,HeadChar,FieldName,FlowIDWidth,IsStartup From T_CodeRule Where ID = " + strID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CodeRule");

        LB_CodeRuleID.Text = strID;
        DL_CodeType.SelectedValue = ds.Tables[0].Rows[0][0].ToString().Trim();
        TB_HeadChar.Text = ds.Tables[0].Rows[0][1].ToString();
        DL_FieldRule.SelectedValue = ds.Tables[0].Rows[0][2].ToString().Trim();
        TB_FlowIDWidth.Text = ds.Tables[0].Rows[0][3].ToString();
        DL_IsStartup.SelectedValue = ds.Tables[0].Rows[0][4].ToString().Trim();
    }

    protected void DataGrid45_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strStage = ((Button)e.Item.FindControl("BT_Stage")).Text.Trim();
        string strPossibility = e.Item.Cells[1].Text.Trim();

        TB_CustomerQuestionStage.Text = strStage;
        TB_CustomerQuestionPossibility.Text = strPossibility;
    }

    protected void DataGrid46_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strStage = ((Button)e.Item.FindControl("BT_Stage")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_CustomerQuestionCustomerStage.Text = strStage;
        TB_CustomerQuestionCustomerStageSort.Text = strSortNumber;
    }

    protected void DataGrid48_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_PackingType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_PackingType.Text = strType;
        TB_PackingTypeSort.Text = strSortNumber;
    }

    protected void DataGrid49_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_InvoiceType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_InvoiceType.Text = strType;
        TB_InvoiceTypeSort.Text = strSortNumber;
    }

    protected void DataGrid50_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_FestivalsType")).Text;
        string strSortNumber = e.Item.Cells[1].Text.Trim();

        TB_FestivalsType.Text = strType;
        TB_FestivalsTypeSort.Text = strSortNumber;
    }

    protected void BT_ReqTypeNew_Click(object sender, EventArgs e)
    {
        string strReqType = TB_ReqType.Text.Trim();
        string strSortNumber = TB_ReqTypeSort.Text.Trim();

        ReqTypeBLL reqTypeBLL = new ReqTypeBLL();
        ReqType reqType = new ReqType();

        try
        {
            reqType.Type = strReqType;
            reqType.SortNumber = int.Parse(strSortNumber);

            reqTypeBLL.AddReqType(reqType);

            LoadReqType();

        }
        catch
        {
        }

    }
    protected void BT_ReqTypeDelete_Click(object sender, EventArgs e)
    {
        string strReqType = TB_ReqType.Text.Trim();
        string strSortNumber = TB_ReqTypeSort.Text.Trim();

        ReqTypeBLL reqTypeBLL = new ReqTypeBLL();
        ReqType reqType = new ReqType();

        try
        {
            reqType.Type = strReqType;
            reqType.SortNumber = int.Parse(strSortNumber);

            reqTypeBLL.DeleteReqType(reqType);

            LoadReqType();
        }
        catch
        {
        }

    }


    protected void BT_DefectTypeNew_Click(object sender, EventArgs e)
    {
        string strDefectType = TB_DefectType.Text.Trim();
        string strSortNumber = TB_DefectTypeSort.Text.Trim();

        DefectTypeBLL defectTypeBLL = new DefectTypeBLL();
        DefectType defectType = new DefectType();

        try
        {
            defectType.Type = strDefectType;
            defectType.SortNumber = int.Parse(strSortNumber);

            defectTypeBLL.AddDefectType(defectType);

            LoadDefectType();
        }
        catch
        {
        }

    }
    protected void BT_DefectTypeDelete_Click(object sender, EventArgs e)
    {
        string strDefectType = TB_DefectType.Text.Trim();
        string strSortNumber = TB_DefectTypeSort.Text.Trim();

        DefectTypeBLL defectTypeBLL = new DefectTypeBLL();
        DefectType defectType = new DefectType();

        try
        {
            defectType.Type = strDefectType;
            defectType.SortNumber = int.Parse(strSortNumber);

            defectTypeBLL.DeleteDefectType(defectType);

            LoadDefectType();
        }
        catch
        {
        }
    }



    protected void BT_AssetTypeNew_Click(object sender, EventArgs e)
    {
        string strAssetType = TB_AssetType.Text.Trim();
        string strSortNumber = TB_AssetTypeSort.Text.Trim();

        AssetTypeBLL assetTypeBLL = new AssetTypeBLL();
        AssetType assetType = new AssetType();

        try
        {
            assetType.Type = strAssetType;
            assetType.SortNumber = int.Parse(strSortNumber);

            assetTypeBLL.AddAssetType(assetType);

            LoadAssetType();
        }
        catch
        {

        }
    }
    protected void BT_AssetTypeDelete_Click(object sender, EventArgs e)
    {
        string strAssetType = TB_AssetType.Text.Trim();
        string strSortNumber = TB_AssetTypeSort.Text.Trim();

        AssetTypeBLL assetTypeBLL = new AssetTypeBLL();
        AssetType assetType = new AssetType();

        try
        {
            assetType.Type = strAssetType;
            assetType.SortNumber = int.Parse(strSortNumber);

            assetTypeBLL.DeleteAssetType(assetType);

            LoadAssetType();
        }
        catch
        {

        }
    }

    protected void BT_PackingTypeNew_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strType, strSortNumber;

        strType = TB_PackingType.Text.Trim();
        strSortNumber = TB_PackingTypeSort.Text.Trim();

        strHQL = "Insert Into T_PackingType Values('" + strType + "'," + strSortNumber + ")";
        ShareClass.RunSqlCommand(strHQL);

        LoadPackingType();
    }

    protected void BT_PackingTypeDelete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strType, strSortNumber;

        strType = TB_PackingType.Text.Trim();
        strSortNumber = TB_PackingTypeSort.Text.Trim();

        strHQL = "Delete From  T_PackingType Where Type = '" + strType + "'";
        ShareClass.RunSqlCommand(strHQL);

        LoadPackingType();
    }

    protected void LoadBarType()
    {
        string strHQL = "Select Type From T_BarType";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BarType");

        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                DL_BarType.SelectedValue = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            catch
            {

            }
        }

    }

    protected void LoadStockCountMethod()
    {
        string strHQL = "Select Method From T_GoodsStockCountMethod";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsStockCountMethod");

        if (ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                DL_StockCountMethod.SelectedValue = ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            catch
            {

            }
        }

    }

    protected void LoadReqType()
    {
        string strHQL = "from ReqType as reqType order by reqType.SortNumber ASC";
        ReqTypeBLL reqTypeBLL = new ReqTypeBLL();
        IList lst = reqTypeBLL.GetAllReqTypes(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }


    protected void LoadDefectType()
    {
        string strHQL = "from DefectType as defectType order by defectType.SortNumber ASC";
        DefectTypeBLL defectTypeBLL = new DefectTypeBLL();
        IList lst = defectTypeBLL.GetAllDefectTypes(strHQL);

        DataGrid47.DataSource = lst;
        DataGrid47.DataBind();
    }


    protected void LoadProductProcess()
    {
        string strHQL = "From ProductProcess as productProcess Order By productProcess.SortNumber ASC";
        ProductProcessBLL productProcessBLL = new ProductProcessBLL();
        IList lst = productProcessBLL.GetAllProductProcesss(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void LoadIndustryType()
    {
        string strHQL = "From IndustryType as industryType Order By industryType.SortNumber ASC";
        IndustryTypeBLL industryTypeBLL = new IndustryTypeBLL();
        IList lst = industryTypeBLL.GetAllIndustryTypes(strHQL);

        DataGrid26.DataSource = lst;
        DataGrid26.DataBind();
    }

    protected void LoadAssetType()
    {
        string strHQL = "from AssetType as assetType order by assetType.SortNumber ASC";
        AssetTypeBLL assetTypeBLL = new AssetTypeBLL();
        IList lst = assetTypeBLL.GetAllAssetTypes(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }



    protected void LoadTaskOperation()
    {
        string strHQL = "from TaskOperation as taskOperation order by taskOperation.SortNumber ASC";
        TaskOperationBLL taskOperationBLL = new TaskOperationBLL();
        IList lst = taskOperationBLL.GetAllTaskOperations(strHQL);

        DataGrid7.DataSource = lst;
        DataGrid7.DataBind();
    }


    protected void LoadMeetingType()
    {
        string strHQL = "from MeetingType as meetingType Order by meetingType.SortNumber ASC";
        MeetingTypeBLL meetingTypeBLL = new MeetingTypeBLL();
        IList lst = meetingTypeBLL.GetAllMeetingTypes(strHQL);

        DataGrid9.DataSource = lst;
        DataGrid9.DataBind();
    }

    protected void LoadMTType()
    {
        string strHQL = "from MTType as mtType Order by mtType.SortNumber ASC";
        MTTypeBLL mtTypeBLL = new MTTypeBLL();
        IList lst = mtTypeBLL.GetAllMTTypes(strHQL);

        DataGrid10.DataSource = lst;
        DataGrid10.DataBind();
    }

    protected void LoadChangeType()
    {
        string strHQL = "from ChangeType as changeType Order by changeType.SortNumber ASC";
        ChangeTypeBLL changeTypeBLL = new ChangeTypeBLL();
        IList lst = changeTypeBLL.GetAllChangeTypes(strHQL);

        DataGrid11.DataSource = lst;
        DataGrid11.DataBind();
    }


    protected void LoadUnit()
    {
        string strHQL = "from JNUnit as jnUnit Order by jnUnit.SortNumber ASC";
        JNUnitBLL jnUnitBLl = new JNUnitBLL();
        IList lst = jnUnitBLl.GetAllJNUnits(strHQL);

        DataGrid14.DataSource = lst;
        DataGrid14.DataBind();
    }

    protected void LoadActorGroupType()
    {
        string strHQL = "from ActorGroupType as actorGroupType Order by actorGroupType.SortNumber ASC";
        ActorGroupTypeBLL actorGroupTypeBLL = new ActorGroupTypeBLL();
        IList lst = actorGroupTypeBLL.GetAllActorGroupTypes(strHQL);

        DataGrid15.DataSource = lst;
        DataGrid15.DataBind();
    }

    protected void LoadTaskType()
    {
        string strHQl = "from TaskType as taskType order by taskType.SortNumber ASC";
        TaskTypeBLL taskTypeBLL = new TaskTypeBLL();
        IList lst = taskTypeBLL.GetAllTaskTypes(strHQl);

        DataGrid18.DataSource = lst;
        DataGrid18.DataBind();
    }

    protected void LoadTaskRecordType()
    {
        string strHQL = "from TaskRecordType as taskRecordType order by taskRecordType.SortNumber ASC";
        TaskRecordTypeBLL taskRecordTypeBLL = new TaskRecordTypeBLL();
        IList lst = taskRecordTypeBLL.GetAllTaskRecordTypes(strHQL);

        DataGrid19.DataSource = lst;
        DataGrid19.DataBind();
    }

    protected void LoadConstractType()
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractType as constractType Order By constractType.SortNumber ASC";
        ConstractTypeBLL constractTypeBLL = new ConstractTypeBLL();
        lst = constractTypeBLL.GetAllConstractTypes(strHQL);

        DataGrid22.DataSource = lst;
        DataGrid22.DataBind();
    }

    protected void LoadDailyWorkUnitBonus()
    {
        string strHQL;
        IList lst;

        strHQL = "from DailyWorkUnitBonus as dailyWorkUnitBonus Order by dailyWorkUnitBonus.ID ASC";
        DailyWorkUnitBonusBLL dailyWorkUnitBonusBLL = new DailyWorkUnitBonusBLL();
        lst = dailyWorkUnitBonusBLL.GetAllDailyWorkUnitBonuss(strHQL);

        DataGrid23.DataSource = lst;
        DataGrid23.DataBind();
    }

    protected void LoadAttendanceRule()
    {
        string strHQL;
        IList lst;

        strHQL = "from AttendanceRule as attendanceRule Order by attendanceRule.ID ASC";
        AttendanceRuleBLL attendanceRuleBLL = new AttendanceRuleBLL();
        lst = attendanceRuleBLL.GetAllAttendanceRules(strHQL);

        DataGrid24.DataSource = lst;
        DataGrid24.DataBind();
    }

    protected void LoadCarType()
    {
        string strHQL;
        IList lst;

        strHQL = "from CarType as carType Order By carType.SortNumber ASC";
        CarTypeBLL carTypeBLL = new CarTypeBLL();
        lst = carTypeBLL.GetAllCarTypes(strHQL);

        DataGrid27.DataSource = lst;
        DataGrid27.DataBind();
    }

    protected void LoadPlanType()
    {
        string strHQL;
        IList lst;

        strHQL = "from PlanType as planType Order By planType.SortNumber ASC";
        PlanTypeBLL planTypeBLL = new PlanTypeBLL();
        lst = planTypeBLL.GetAllPlanTypes(strHQL);

        DataGrid28.DataSource = lst;
        DataGrid28.DataBind();
    }

    protected void LoadReportType()
    {
        string strHQL;
        IList lst;

        strHQL = "from ReportType as reportType Order By reportType.SortNumber ASC";
        ReportTypeBLL reportTypeBLL = new ReportTypeBLL();
        lst = reportTypeBLL.GetAllReportTypes(strHQL);

        DataGrid29.DataSource = lst;
        DataGrid29.DataBind();
    }

    protected void LoadKPIType()
    {
        string strHQL;
        IList lst;

        strHQL = "from KPIType as kpiType Order By kpiType.SortNumber ASC";
        KPITypeBLL kpiTypeBLL = new KPITypeBLL();
        lst = kpiTypeBLL.GetAllKPITypes(strHQL);

        DataGrid30.DataSource = lst;
        DataGrid30.DataBind();
    }

    protected void LoadCustomerQuestionType()
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestionType as customerQuestionType Order By customerQuestionType.SortNumber ASC";
        CustomerQuestionTypeBLL customerQuestionTypeBLL = new CustomerQuestionTypeBLL();
        lst = customerQuestionTypeBLL.GetAllCustomerQuestionTypes(strHQL);

        DataGrid32.DataSource = lst;
        DataGrid32.DataBind();
    }

    protected void LoadCurrencyType()
    {
        string strHQL;
        IList lst;

        strHQL = "from CurrencyType as currencyType Order By currencyType.SortNo ASC";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);

        DataGrid35.DataSource = lst;
        DataGrid35.DataBind();
    }

    protected void LoadGoodsShipmentType()
    {
        string strHQL;

        strHQL = "Select * from T_GoodsShipmentType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsShipmentType");

        DataGrid42.DataSource = ds;
        DataGrid42.DataBind();
    }

    protected void LoadGoodsCheckInType()
    {
        string strHQL;

        strHQL = "Select * from T_GoodsCheckInType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsCheckInType");

        DataGrid43.DataSource = ds;
        DataGrid43.DataBind();
    }

    protected void LoadSupplierBigType()
    {
        string strHQL;

        strHQL = "Select * From T_BMSupplierBigType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBigType");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();

        DL_SupplierBigType.DataSource = ds;
        DL_SupplierBigType.DataBind();
    }

    protected void LoadSupplierSmallType()
    {
        string strHQL;

        strHQL = "Select * From T_BMSupplierSmallType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierSmallType");

        DataGrid6.DataSource = ds;
        DataGrid6.DataBind();
    }

    protected void LoadBMBidType()
    {
        string strHQL;

        strHQL = "Select * From T_BMBidType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidType");

        DataGrid8.DataSource = ds;
        DataGrid8.DataBind();
    }

    protected void LoadPackingType()
    {
        string strHQL;

        strHQL = "Select * From T_PackingType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_PackingType");

        DataGrid48.DataSource = ds;
        DataGrid48.DataBind();
    }

    protected void LoadSaleType()
    {
        string strHQL;

        strHQL = "Select * From T_SaleType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SaleType");

        DataGrid12.DataSource = ds;
        DataGrid12.DataBind();
    }

    protected void LoadConstractRadio()
    {
        string strHQL;

        strHQL = "Select * From T_ConstractRadio ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ConstractRadio");

        DataGrid21.DataSource = ds;
        DataGrid21.DataBind();
    }

    protected void LoadInvoiceType()
    {
        string strHQL;

        strHQL = "Select * From T_InvoiceType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_InvoiceType");

        DataGrid49.DataSource = ds;
        DataGrid49.DataBind();
    }

    protected void LoadOvertimeType()
    {
        string strHQL;

        strHQL = "Select * From T_OvertimeType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_OvertimeType");

        DataGrid16.DataSource = ds;
        DataGrid16.DataBind();
    }

    protected void LoadFestivalsType()
    {
        string strHQL;

        strHQL = "Select * From T_FestivalsType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_FestivalsType");

        DataGrid50.DataSource = ds;
        DataGrid50.DataBind();
    }

    protected void BT_OperationNew_Click(object sender, EventArgs e)
    {
        string strOperation = TB_TaskOperation.Text.Trim();
        string strSortNumber = TB_OperationSortNumber.Text.Trim();

        TaskOperationBLL taskOperationBLL = new TaskOperationBLL();
        TaskOperation taskOperation = new TaskOperation();

        try
        {
            taskOperation.Operation = strOperation;
            taskOperation.SortNumber = int.Parse(strSortNumber);

            taskOperationBLL.AddTaskOperation(taskOperation);

            LoadTaskOperation();
        }
        catch
        {
        }

    }
    protected void BT_OperationDelete_Click(object sender, EventArgs e)
    {
        string strOperation = TB_TaskOperation.Text.Trim();
        string strSortNumber = TB_OperationSortNumber.Text.Trim();

        TaskOperationBLL taskOperationBLL = new TaskOperationBLL();
        TaskOperation taskOperation = new TaskOperation();

        try
        {
            taskOperation.Operation = strOperation;
            taskOperation.SortNumber = int.Parse(strSortNumber);

            taskOperationBLL.DeleteTaskOperation(taskOperation);

            LoadTaskOperation();
        }
        catch
        {
        }

    }

    protected void BT_MeetingTypeNew_Click(object sender, EventArgs e)
    {

        string strMeetingType = TB_MeetingType.Text.Trim();
        string strSortNumber = TB_MeetingTypeSort.Text.Trim();


        try
        {
            MeetingTypeBLL meetingTypeBLL = new MeetingTypeBLL();
            MeetingType meetingType = new MeetingType();

            meetingType.Type = strMeetingType;
            meetingType.SortNumber = int.Parse(strSortNumber);

            meetingTypeBLL.AddMeetingType(meetingType);

            LoadMeetingType();
        }
        catch
        {
        }


    }
    protected void BT_MeetingTypeDelete_Click(object sender, EventArgs e)
    {
        string strMeetingType = TB_MeetingType.Text.Trim();
        string strSortNumber = TB_MeetingTypeSort.Text.Trim();

        try
        {
            MeetingTypeBLL meetingTypeBLL = new MeetingTypeBLL();
            MeetingType meetingType = new MeetingType();

            meetingType.Type = strMeetingType;
            meetingType.SortNumber = int.Parse(strSortNumber);

            meetingTypeBLL.DeleteMeetingType(meetingType);

            LoadMeetingType();
        }
        catch
        {
        }
    }

    protected void BT_MTTypeNew_Click(object sender, EventArgs e)
    {
        string strMTType, strSortNumber;

        strMTType = TB_MTType.Text.Trim();
        strSortNumber = TB_MTTypeSort.Text.Trim();

        MTTypeBLL mtTypeBLL = new MTTypeBLL();
        MTType mtType = new MTType();

        mtType.Type = strMTType;
        mtType.SortNumber = int.Parse(strSortNumber);

        try
        {
            mtTypeBLL.AddMTType(mtType);
            LoadMTType();
        }
        catch
        {
        }

    }
    protected void BT_MTTypeDelete_Click(object sender, EventArgs e)
    {

        string strMTType, strSortNumber;

        strMTType = TB_MTType.Text.Trim();
        strSortNumber = TB_MTTypeSort.Text.Trim();

        MTTypeBLL mtTypeBLL = new MTTypeBLL();
        MTType mtType = new MTType();

        mtType.Type = strMTType;
        mtType.SortNumber = int.Parse(strSortNumber);

        try
        {
            mtTypeBLL.DeleteMTType(mtType);
            LoadMTType();
        }
        catch
        {
        }
    }




    protected void BT_AddCustomerQuestionStage_Click(object sender, EventArgs e)
    {
        string strStage, strPossibility;

        strStage = TB_CustomerQuestionStage.Text.Trim();
        strPossibility = TB_CustomerQuestionPossibility.Text.Trim();

        CustomerQuestionStageBLL customerQuestionStageBLL = new CustomerQuestionStageBLL();
        CustomerQuestionStage customerQuestionStage = new CustomerQuestionStage();

        customerQuestionStage.Stage = strStage;
        customerQuestionStage.Possibility = int.Parse(strPossibility);

        try
        {
            customerQuestionStageBLL.AddCustomerQuestionStage(customerQuestionStage);

            LoadCustomerQuestionStage();
        }
        catch
        {

        }
    }

    protected void BT_DeleteCustomerQuestionPossibility_Click(object sender, EventArgs e)
    {
        string strStage, strPossibility;

        strStage = TB_CustomerQuestionStage.Text.Trim();
        strPossibility = TB_CustomerQuestionPossibility.Text.Trim();

        CustomerQuestionStageBLL customerQuestionStageBLL = new CustomerQuestionStageBLL();
        CustomerQuestionStage customerQuestionStage = new CustomerQuestionStage();

        customerQuestionStage.Stage = strStage;
        customerQuestionStage.Possibility = int.Parse(strPossibility);

        try
        {
            customerQuestionStageBLL.DeleteCustomerQuestionStage(customerQuestionStage);

            LoadCustomerQuestionStage();
        }
        catch
        {

        }
    }


    protected void BT_AddCustomerQuestionCustomerStage_Click(object sender, EventArgs e)
    {
        string strStage, strSortNumber;

        strStage = TB_CustomerQuestionCustomerStage.Text.Trim();
        strSortNumber = TB_CustomerQuestionCustomerStageSort.Text.Trim();

        CustomerQuestionCustomerStageBLL customerQuestionCustomerStageBLL = new CustomerQuestionCustomerStageBLL();
        CustomerQuestionCustomerStage customerQuestionCustomerStage = new CustomerQuestionCustomerStage();

        customerQuestionCustomerStage.Stage = strStage;
        customerQuestionCustomerStage.SortNumber = int.Parse(strSortNumber);

        try
        {
            customerQuestionCustomerStageBLL.AddCustomerQuestionCustomerStage(customerQuestionCustomerStage);

            LoadCustomerQuestionCustomerStage();
        }
        catch
        {

        }
    }

    protected void BT_DeleteCustomerQuestionCustomerStage_Click(object sender, EventArgs e)
    {
        string strStage, strSortNumber;

        strStage = TB_CustomerQuestionCustomerStage.Text.Trim();
        strSortNumber = TB_CustomerQuestionCustomerStageSort.Text.Trim();

        CustomerQuestionCustomerStageBLL customerQuestionCustomerStageBLL = new CustomerQuestionCustomerStageBLL();
        CustomerQuestionCustomerStage customerQuestionCustomerStage = new CustomerQuestionCustomerStage();

        customerQuestionCustomerStage.Stage = strStage;
        customerQuestionCustomerStage.SortNumber = int.Parse(strSortNumber);

        try
        {
            customerQuestionCustomerStageBLL.DeleteCustomerQuestionCustomerStage(customerQuestionCustomerStage);

            LoadCustomerQuestionCustomerStage();
        }
        catch
        {

        }
    }


    protected void BT_ChangeTypeNew_Click(object sender, EventArgs e)
    {
        string strChangeType, strSortNumber;

        strChangeType = TB_ChangeType.Text.Trim();
        strSortNumber = TB_ChangeTypeSort.Text.Trim();

        ChangeTypeBLL changeTypeBLL = new ChangeTypeBLL();
        ChangeType changeType = new ChangeType();

        changeType.Type = strChangeType;
        changeType.SortNumber = int.Parse(strSortNumber);

        try
        {
            changeTypeBLL.AddChangeType(changeType);
            LoadChangeType();
        }
        catch
        {
        }


    }
    protected void BT_ChangeTypeDelete_Click(object sender, EventArgs e)
    {
        string strChangeType, strSortNumber;

        strChangeType = TB_ChangeType.Text.Trim();
        strSortNumber = TB_ChangeTypeSort.Text.Trim();

        ChangeTypeBLL changeTypeBLL = new ChangeTypeBLL();
        ChangeType changeType = new ChangeType();

        changeType.Type = strChangeType;
        changeType.SortNumber = int.Parse(strSortNumber);

        try
        {
            changeTypeBLL.DeleteChangeType(changeType);
            LoadChangeType();
        }
        catch
        {
        }

    }

    protected void BT_UnitNew_Click(object sender, EventArgs e)
    {
        string strUnitName, strSortNumber;

        strUnitName = TB_UnitName.Text.Trim();
        strSortNumber = TB_UnitSort.Text.Trim();

        JNUnitBLL jnUnitBLL = new JNUnitBLL();
        JNUnit jnUnit = new JNUnit();

        try
        {
            jnUnit.UnitName = strUnitName;
            jnUnit.SortNumber = int.Parse(strSortNumber);

            jnUnitBLL.AddJNUnit(jnUnit);

            LoadUnit();
        }
        catch
        {
        }
    }

    protected void BT_UnitDelete_Click(object sender, EventArgs e)
    {
        string strUnitName, strSortNumber;

        strUnitName = TB_UnitName.Text.Trim();
        strSortNumber = TB_UnitSort.Text.Trim();

        JNUnitBLL jnUnitBLL = new JNUnitBLL();
        JNUnit jnUnit = new JNUnit();

        try
        {
            jnUnit.UnitName = strUnitName;
            jnUnit.SortNumber = int.Parse(strSortNumber);

            jnUnitBLL.DeleteJNUnit(jnUnit);

            LoadUnit();
        }
        catch
        {
        }

    }
    protected void BT_ActorGroupNew_Click(object sender, EventArgs e)
    {

        string strType, strSortNumber;

        strType = TB_ActorGroupType.Text.Trim();
        strSortNumber = TB_ActorGroupTypeSort.Text.Trim();

        ActorGroupType actorGroupType = new ActorGroupType();
        actorGroupType.Type = strType;
        actorGroupType.SortNumber = int.Parse(strSortNumber);

        try
        {
            ActorGroupTypeBLL actorGroupTypeBLL = new ActorGroupTypeBLL();
            actorGroupTypeBLL.AddActorGroupType(actorGroupType);

            LoadActorGroupType();
        }
        catch
        {
        }
    }
    protected void BT_ActorGroupDelete_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_ActorGroupType.Text.Trim();
        strSortNumber = TB_ActorGroupTypeSort.Text.Trim();

        ActorGroupType actorGroupType = new ActorGroupType();
        actorGroupType.Type = strType;
        actorGroupType.SortNumber = int.Parse(strSortNumber);

        try
        {
            ActorGroupTypeBLL actorGroupTypeBLL = new ActorGroupTypeBLL();
            actorGroupTypeBLL.DeleteActorGroupType(actorGroupType);

            LoadActorGroupType();
        }
        catch
        {
        }
    }

    protected void BT_TaskTypeNew_Click(object sender, EventArgs e)
    {
        string strType = TB_TaskType.Text.Trim();
        string strSortNumber = TB_TaskTypeSort.Text.Trim();

        TaskTypeBLL taskTypeBLL = new TaskTypeBLL();
        TaskType taskType = new TaskType();

        taskType.Type = strType;
        taskType.SortNumber = int.Parse(strSortNumber);

        try
        {

            taskTypeBLL.AddTaskType(taskType);
            LoadTaskType();
        }
        catch
        {
        }
    }
    protected void BT_TaskTypeDelete_Click(object sender, EventArgs e)
    {
        string strType = TB_TaskType.Text.Trim();
        string strSortNumber = TB_TaskTypeSort.Text.Trim();

        TaskTypeBLL taskTypeBLL = new TaskTypeBLL();
        TaskType taskType = new TaskType();

        taskType.Type = strType;
        taskType.SortNumber = int.Parse(strSortNumber);

        try
        {
            taskTypeBLL.DeleteTaskType(taskType);
            LoadTaskType();
        }
        catch
        {
        }
    }
    protected void BT_TaskRecordNew_Click(object sender, EventArgs e)
    {
        string strType = TB_TaskRecordType.Text.Trim();
        string strSortNumber = TB_TaskRecordTypeSort.Text.Trim();

        TaskRecordTypeBLL taskRecordTypeBLL = new TaskRecordTypeBLL();
        TaskRecordType taskRecordType = new TaskRecordType();

        taskRecordType.Type = strType;
        taskRecordType.SortNumber = int.Parse(strSortNumber);

        try
        {
            taskRecordTypeBLL.AddTaskRecordType(taskRecordType);
            LoadTaskRecordType();
        }
        catch
        {
        }
    }

    protected void BT_TaskRecordDelete_Click(object sender, EventArgs e)
    {
        string strType = TB_TaskRecordType.Text.Trim();
        string strSortNumber = TB_TaskRecordTypeSort.Text.Trim();

        TaskRecordTypeBLL taskRecordTypeBLL = new TaskRecordTypeBLL();
        TaskRecordType taskRecordType = new TaskRecordType();

        taskRecordType.Type = strType;
        taskRecordType.SortNumber = int.Parse(strSortNumber);

        try
        {
            taskRecordTypeBLL.DeleteTaskRecordType(taskRecordType);
            LoadTaskRecordType();
        }
        catch
        {
        }
    }

    protected void BT_ConstractAdd_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber, strKeyWord;

        strType = TB_ConstractType.Text.Trim();
        strKeyWord = TB_ConstractTypeKeyWord.Text.Trim();
        strSortNumber = TB_ConstractTypeSort.Text.Trim();

        ConstractTypeBLL constractTypeBLL = new ConstractTypeBLL();
        ConstractType constractType = new ConstractType();

        constractType.Type = strType;
        constractType.KeyWord = strKeyWord;
        constractType.SortNumber = int.Parse(strSortNumber);

        try
        {
            constractTypeBLL.AddConstractType(constractType);
            LoadConstractType();
        }
        catch
        {
        }
    }

    protected void BT_ConstractDelete_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_ConstractType.Text.Trim();
        strSortNumber = TB_ConstractTypeSort.Text.Trim();

        ConstractTypeBLL constractTypeBLL = new ConstractTypeBLL();
        ConstractType constractType = new ConstractType();

        constractType.Type = strType;
        constractType.SortNumber = int.Parse(strSortNumber);

        try
        {
            constractTypeBLL.DeleteConstractType(constractType);
            LoadConstractType();
        }
        catch
        {
        }
    }

    protected void BT_AddEveryCharPrice_Click(object sender, EventArgs e)
    {
        decimal deEveryCharPrice = NB_EveryCharPrice.Amount;
        decimal deEveryDocPrice = NB_EveryDocPrice.Amount;
        int intCharUpper = int.Parse(NB_CharUpper.Amount.ToString());
        int intDocUpper = int.Parse(NB_DocUpper.Amount.ToString());

        DailyWorkUnitBonusBLL dailyWorkUnitBonusBLL = new DailyWorkUnitBonusBLL();
        DailyWorkUnitBonus dailyWorkUnitBonus = new DailyWorkUnitBonus();

        dailyWorkUnitBonus.EveryCharPrice = deEveryCharPrice;
        dailyWorkUnitBonus.EveryDocPrice = deEveryDocPrice;
        dailyWorkUnitBonus.CharUpper = intCharUpper;
        dailyWorkUnitBonus.DocUpper = intDocUpper;


        try
        {
            dailyWorkUnitBonusBLL.AddDailyWorkUnitBonus(dailyWorkUnitBonus);
            LoadDailyWorkUnitBonus();
        }
        catch
        {
        }
    }

    protected void BT_DeleteEveryDocPrice_Click(object sender, EventArgs e)
    {
        string strID = LB_DailyWorkUnitBonusID.Text.Trim();

        DailyWorkUnitBonusBLL dailyWorkUnitBonusBLL = new DailyWorkUnitBonusBLL();
        DailyWorkUnitBonus dailyWorkUnitBonus = new DailyWorkUnitBonus();
        dailyWorkUnitBonus.ID = int.Parse(strID);

        try
        {
            dailyWorkUnitBonusBLL.DeleteDailyWorkUnitBonus(dailyWorkUnitBonus);
            LoadDailyWorkUnitBonus();
        }
        catch
        {
        }
    }

    protected void BT_AddProductProcess_Click(object sender, EventArgs e)
    {
        string strProcessName = TB_ProcessName.Text.Trim();
        string strSortNumber = TB_ProcessSort.Text.Trim();

        ProductProcessBLL productProcessBLL = new ProductProcessBLL();
        ProductProcess productProcess = new ProductProcess();

        productProcess.ProcessName = strProcessName;
        productProcess.SortNumber = int.Parse(strSortNumber);

        try
        {
            productProcessBLL.AddProductProcess(productProcess);
            LoadProductProcess();
        }
        catch
        {
        }
    }

    protected void BT_DeleteProductProcess_Click(object sender, EventArgs e)
    {
        string strProcessName = TB_ProcessName.Text.Trim();
        string strSortNumber = TB_ProcessSort.Text.Trim();

        ProductProcessBLL productProcessBLL = new ProductProcessBLL();
        ProductProcess productProcess = new ProductProcess();

        productProcess.ProcessName = strProcessName;
        productProcess.SortNumber = int.Parse(strSortNumber);

        try
        {
            productProcessBLL.DeleteProductProcess(productProcess);

            LoadProductProcess();
        }
        catch
        {
        }
    }

    protected void BT_AddIndustryType_Click(object sender, EventArgs e)
    {
        string strType = TB_IndustryType.Text.Trim();
        string strSortNumber = TB_IndustryTypeSort.Text.Trim();

        IndustryTypeBLL industryTypeBLL = new IndustryTypeBLL();
        IndustryType industryType = new IndustryType();

        industryType.Type = strType;
        industryType.SortNumber = int.Parse(strSortNumber);

        try
        {
            industryTypeBLL.AddIndustryType(industryType);
            LoadIndustryType();
        }
        catch
        {
        }
    }

    protected void BT_DeleteIndustryType_Click(object sender, EventArgs e)
    {
        string strType = TB_IndustryType.Text.Trim();
        string strSortNumber = TB_IndustryTypeSort.Text.Trim();

        IndustryTypeBLL industryTypeBLL = new IndustryTypeBLL();
        IndustryType industryType = new IndustryType();

        industryType.Type = strType;
        industryType.SortNumber = int.Parse(strSortNumber);

        try
        {
            industryTypeBLL.DeleteIndustryType(industryType);
            LoadIndustryType();
        }
        catch
        {
        }
    }

    protected void BT_AddAttendanceRule_Click(object sender, EventArgs e)
    {
        string strMCheckInStart, strMCheckInEnd, strMCheckOutStart, strMCheckOutEnd;
        string strACheckInStart, strACheckInEnd, strACheckOutStart, strACheckOutEnd;
        string strNCheckInStart, strNCheckInEnd, strNCheckOutStart, strNCheckOutEnd;
        string strOCheckInStart, strOCheckInEnd, strOCheckOutStart, strOCheckOutEnd;

        strMCheckInStart = TB_MCheckInStart.Text.Trim();
        strMCheckInEnd = TB_MCheckInEnd.Text.Trim();
        strMCheckOutStart = TB_MCheckOutStart.Text.Trim();
        strMCheckOutEnd = TB_MCheckOutEnd.Text.Trim();

        strACheckInStart = TB_ACheckInStart.Text.Trim();
        strACheckInEnd = TB_ACheckInEnd.Text.Trim();
        strACheckOutStart = TB_ACheckOutStart.Text.Trim();
        strACheckOutEnd = TB_AChectOutEnd.Text.Trim();

        strNCheckInStart = TB_NCheckInStart.Text.Trim();
        strNCheckInEnd = TB_NCheckInEnd.Text.Trim();
        strNCheckOutStart = TB_NCheckOutStart.Text.Trim();
        strNCheckOutEnd = TB_NCheckOutEnd.Text.Trim();

        strOCheckInStart = TB_OCheckInStart.Text.Trim();
        strOCheckInEnd = TB_OCheckInEnd.Text.Trim();
        strOCheckOutStart = TB_OCheckOutStart.Text.Trim();
        strOCheckOutEnd = TB_OCheckOutEnd.Text.Trim();

        if (strMCheckInStart == "" || strMCheckInEnd == "" || strMCheckOutStart == "" || strMCheckOutEnd == ""
           || strACheckInStart == "" || strACheckInEnd == "" || strACheckOutStart == "" || strACheckOutEnd == ""
           || strNCheckInStart == "" || strNCheckInEnd == "" || strNCheckOutStart == "" || strNCheckOutEnd == ""
           || strOCheckInStart == "" || strOCheckInEnd == "" || strOCheckOutStart == "" || strOCheckOutEnd == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSGXDBNWKQJC") + "')", true);
            return;
        }


        AttendanceRuleBLL attendanceRuleBLL = new AttendanceRuleBLL();
        AttendanceRule attendanceRule = new AttendanceRule();

        attendanceRule.MCheckInStart = strMCheckInStart;
        attendanceRule.MCheckInEnd = strMCheckInEnd;
        attendanceRule.MCheckInIsMust = DDL_MCheckInIsMust.SelectedValue.Trim();

        attendanceRule.MCheckOutStart = strMCheckOutStart;
        attendanceRule.MCheckOutEnd = strMCheckOutEnd;
        attendanceRule.MCheckOutIsMust = DDL_MCheckOutIsMust.SelectedValue.Trim();

        attendanceRule.ACheckInStart = strACheckInStart;
        attendanceRule.ACheckInEnd = strACheckInEnd;
        attendanceRule.ACheckInIsMust = DDL_ACheckInIsMust.SelectedValue.Trim();

        attendanceRule.ACheckOutStart = strACheckOutStart;
        attendanceRule.ACheckOutEnd = strACheckOutEnd;
        attendanceRule.ACheckOutIsMust = DDL_ACheckOutIsMust.SelectedValue.Trim();

        attendanceRule.NCheckInStart = strNCheckInStart;
        attendanceRule.NCheckInEnd = strNCheckInEnd;
        attendanceRule.NCheckInIsMust = DDL_NCheckInIsMust.SelectedValue.Trim();

        attendanceRule.NCheckOutStart = strNCheckOutStart;
        attendanceRule.NCheckOutEnd = strNCheckOutEnd;
        attendanceRule.NCheckOutIsMust = DDL_NCheckOutIsMust.SelectedValue.Trim();

        attendanceRule.OCheckInStart = strOCheckInStart;
        attendanceRule.OCheckInEnd = strOCheckInEnd;
        attendanceRule.OCheckInIsMust = DDL_OCheckInIsMust.SelectedValue.Trim();

        attendanceRule.OCheckOutStart = strOCheckOutStart;
        attendanceRule.OCheckOutEnd = strOCheckOutEnd;
        attendanceRule.OCheckOutIsMust = DDL_OCheckOutIsMust.SelectedValue.Trim();

        attendanceRule.LargestDistance = NB_LargestDistance.Amount;

        attendanceRule.OfficeLatitude = TB_Latitude.Text.Trim();
        attendanceRule.OfficeLongitude = TB_Longitude.Text.Trim();

        attendanceRule.Address = TB_Address.Text.Trim();

        try
        {
            attendanceRuleBLL.AddAttendanceRule(attendanceRule);
            LoadAttendanceRule();
        }
        catch
        {
        }
    }

    protected void BT_DeleteAttendanceRule_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID;

        strID = LB_AttendanceRuleID.Text.Trim();

        strHQL = "Delete From T_AttendanceRule Where ID = " + strID;
        ShareClass.RunSqlCommand(strHQL);

        LoadAttendanceRule();
    }

    protected void BT_AddCarType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_CarType.Text.Trim();
        strSortNumber = TB_CarTypeSort.Text.Trim();

        CarTypeBLL carTypeBLL = new CarTypeBLL();
        CarType carType = new CarType();

        carType.Type = strType;
        carType.SortNumber = int.Parse(strSortNumber);

        try
        {
            carTypeBLL.AddCarType(carType);

            LoadCarType();
        }
        catch
        {
        }
    }

    protected void BT_DeleteCarType_Click(object sender, EventArgs e)
    {

        string strType, strSortNumber;

        strType = TB_CarType.Text.Trim();
        strSortNumber = TB_CarTypeSort.Text.Trim();

        CarTypeBLL carTypeBLL = new CarTypeBLL();
        CarType carType = new CarType();

        carType.Type = strType;
        carType.SortNumber = int.Parse(strSortNumber);

        try
        {
            carTypeBLL.DeleteCarType(carType);

            LoadCarType();
        }
        catch
        {
        }
    }

    protected void BT_AddPlanType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_PlanType.Text.Trim();
        strSortNumber = TB_PlanTypeSort.Text.Trim();

        PlanTypeBLL planTypeBLL = new PlanTypeBLL();
        PlanType planType = new PlanType();

        planType.Type = strType;
        planType.SortNumber = int.Parse(strSortNumber);

        try
        {
            planTypeBLL.AddPlanType(planType);

            LoadPlanType();
        }
        catch
        {
        }
    }
    protected void BT_DeletePlanType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_PlanType.Text.Trim();
        strSortNumber = TB_PlanTypeSort.Text.Trim();

        PlanTypeBLL planTypeBLL = new PlanTypeBLL();
        PlanType planType = new PlanType();

        planType.Type = strType;
        planType.SortNumber = int.Parse(strSortNumber);

        try
        {
            planTypeBLL.DeletePlanType(planType);

            LoadPlanType();
        }
        catch
        {
        }
    }

    protected void BT_AddReportType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_ReportType.Text.Trim();
        strSortNumber = TB_ReportTypeSort.Text.Trim();

        ReportTypeBLL reportTypeBLL = new ReportTypeBLL();
        ReportType reportType = new ReportType();

        reportType.Type = strType;
        reportType.SortNumber = int.Parse(strSortNumber);

        try
        {
            reportTypeBLL.AddReportType(reportType);

            LoadReportType();
        }
        catch
        {
        }
    }

    protected void BT_DeleteReportType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_ReportType.Text.Trim();
        strSortNumber = TB_ReportTypeSort.Text.Trim();

        ReportTypeBLL reportTypeBLL = new ReportTypeBLL();
        ReportType reportType = new ReportType();

        reportType.Type = strType;
        reportType.SortNumber = int.Parse(strSortNumber);

        try
        {
            reportTypeBLL.DeleteReportType(reportType);

            LoadReportType();
        }
        catch
        {
        }
    }

    protected void BT_AddKPIType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_KPIType.Text.Trim();
        strSortNumber = TB_KPITypeSort.Text.Trim();

        KPITypeBLL kpiTypeBLL = new KPITypeBLL();
        KPIType kpiType = new KPIType();

        kpiType.Type = strType;
        kpiType.SortNumber = int.Parse(strSortNumber);

        try
        {
            kpiTypeBLL.AddKPIType(kpiType);

            LoadKPIType();
        }
        catch
        {
        }
    }
    protected void BT_DeleteKPIType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_KPIType.Text.Trim();
        strSortNumber = TB_KPITypeSort.Text.Trim();

        KPITypeBLL kpiTypeBLL = new KPITypeBLL();
        KPIType kpiType = new KPIType();

        kpiType.Type = strType;
        kpiType.SortNumber = int.Parse(strSortNumber);

        try
        {
            kpiTypeBLL.DeleteKPIType(kpiType);

            LoadKPIType();
        }
        catch
        {
        }
    }

    protected void BT_ScheduleLimitedDaysUpdate_Click(object sender, EventArgs e)
    {
        string strHQL;
        int intLimitedDays;

        intLimitedDays = int.Parse(NB_ScheduleLimitedDays.Amount.ToString());


        try
        {
            strHQL = "Delete From T_ScheduleLimitedDays ";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Insert Into T_ScheduleLimitedDays(LimitedDays) Values(" + intLimitedDays.ToString() + ")";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }
    }

    protected void BT_WorkingDayRuleUpdate_Click(object sender, EventArgs e)
    {
        string strHQL;
        int intWeekendFirstDay, intWeekendSecondDay;
        string strWeekendsAreWorkdays;

        intWeekendFirstDay = int.Parse(NB_WeekendFirstDay.Amount.ToString());
        intWeekendSecondDay = int.Parse(NB_WeekendSecondDay.Amount.ToString());
        strWeekendsAreWorkdays = DL_WeekendsAreWorkdays.SelectedValue;

        if (intWeekendFirstDay < 0 | intWeekendFirstDay > 6 | intWeekendSecondDay < 0 | intWeekendSecondDay > 6)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBaoCunShiBaiZhouMoKaiShiRiJi") + "')", true);
            return;
        }

        try
        {
            strHQL = "Delete From T_WorkingDayRule ";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Insert Into T_WorkingDayRule(WeekendFirstDay,WeekendSecondDay,WeekendsAreWorkdays) Values(" + intWeekendFirstDay.ToString() + "," + intWeekendSecondDay + ",'" + strWeekendsAreWorkdays + "')";
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }
    }

    protected int GetScheduleLimitedDays()
    {
        string strHQL;


        strHQL = "Select LimitedDays From T_ScheduleLimitedDays";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ScheduleLimitedDays");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }

    }

    protected void BT_AddCustomerQuestionType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_CustomerQuestionType.Text.Trim();
        strSortNumber = TB_CustomerQuestionTypeSort.Text.Trim();

        CustomerQuestionTypeBLL customerQuestionTypeBLL = new CustomerQuestionTypeBLL();
        CustomerQuestionType customerQuestionType = new CustomerQuestionType();

        customerQuestionType.Type = strType;
        customerQuestionType.SortNumber = int.Parse(strSortNumber);

        try
        {
            customerQuestionTypeBLL.AddCustomerQuestionType(customerQuestionType);

            LoadCustomerQuestionType();
        }
        catch
        {
        }
    }
    protected void BT_DeleteCustomerQuestionType_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_CustomerQuestionType.Text.Trim();
        strSortNumber = TB_CustomerQuestionTypeSort.Text.Trim();

        CustomerQuestionTypeBLL customerQuestionTypeBLL = new CustomerQuestionTypeBLL();
        CustomerQuestionType customerQuestionType = new CustomerQuestionType();

        customerQuestionType.Type = strType;
        customerQuestionType.SortNumber = int.Parse(strSortNumber);

        try
        {
            customerQuestionTypeBLL.DeleteCustomerQuestionType(customerQuestionType);

            LoadCustomerQuestionType();
        }
        catch
        {
        }
    }

    protected void BT_AddReceivePayWay_Click(object sender, EventArgs e)
    {
        string strWay, strSortNumber;

        strWay = TB_ReceivePayType.Text.Trim();
        strSortNumber = TB_ReceivePayTypeSort.Text.Trim();

        ReceivePayWayBLL receivePayWayBLL = new ReceivePayWayBLL();
        ReceivePayWay receivePayWay = new ReceivePayWay();

        receivePayWay.Type = strWay;
        receivePayWay.SortNumber = int.Parse(strSortNumber);

        try
        {
            receivePayWayBLL.AddReceivePayWay(receivePayWay);
            LoadReceivePayWay();
        }
        catch
        {
        }
    }

    protected void BT_DeleteReceivePayWay_Click(object sender, EventArgs e)
    {
        string strWay, strSortNumber;

        strWay = TB_ReceivePayType.Text.Trim();
        strSortNumber = TB_ReceivePayTypeSort.Text.Trim();

        ReceivePayWayBLL receivePayWayBLL = new ReceivePayWayBLL();
        ReceivePayWay receivePayWay = new ReceivePayWay();

        receivePayWay.Type = strWay;
        receivePayWay.SortNumber = int.Parse(strSortNumber);

        try
        {
            receivePayWayBLL.DeleteReceivePayWay(receivePayWay);
            LoadReceivePayWay();
        }
        catch
        {
        }
    }


    protected void BT_AddBank_Click(object sender, EventArgs e)
    {
        string strBankName, strSortNumber;

        strBankName = TB_BankName.Text.Trim();
        strSortNumber = TB_BankSort.Text.Trim();

        BankBLL bankBLL = new BankBLL();
        Bank bank = new Bank();
        bank.BankName = strBankName;
        bank.SortNumber = int.Parse(strSortNumber);

        try
        {
            bankBLL.AddBank(bank);
            LoadBank();
        }
        catch
        {
        }
    }

    protected void BT_DeleteBank_Click(object sender, EventArgs e)
    {
        string strBankName, strSortNumber;

        strBankName = TB_BankName.Text.Trim();
        strSortNumber = TB_BankSort.Text.Trim();

        BankBLL bankBLL = new BankBLL();
        Bank bank = new Bank();
        bank.BankName = strBankName;
        bank.SortNumber = int.Parse(strSortNumber);

        try
        {
            bankBLL.DeleteBank(bank);
            LoadBank();
        }
        catch
        {
        }
    }

    protected void BT_NewPosition_Click(object sender, EventArgs e)
    {
        string strPositon;
        string strSortNumber;

        strPositon = TB_Position.Text.Trim();
        strSortNumber = TB_DepartPositionSort.Text.Trim();

        DepartPositionBLL departPositionBLL = new DepartPositionBLL();
        DepartPosition departPosition = new DepartPosition();


        departPosition.Position = strPositon;
        departPosition.SortNumber = int.Parse(strSortNumber);

        try
        {
            departPositionBLL.AddDepartPosition(departPosition);

            BT_UpdatePosition.Enabled = true;
            BT_DeletePosition.Enabled = true;

            LoadDepartPosition();
        }
        catch
        {
        }
    }

    protected void BT_UpdatePosition_Click(object sender, EventArgs e)
    {
        string strID, strPositon, strSortNumber;

        strID = LB_PositionID.Text.Trim();
        strPositon = TB_Position.Text.Trim();
        strSortNumber = TB_DepartPositionSort.Text.Trim();

        DepartPositionBLL departPositionBLL = new DepartPositionBLL();
        DepartPosition departPosition = new DepartPosition();

        departPosition.ID = int.Parse(strID);
        departPosition.Position = strPositon;
        departPosition.SortNumber = int.Parse(strSortNumber);


        try
        {
            departPositionBLL.UpdateDepartPosition(departPosition, int.Parse(strID));

            LoadDepartPosition();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void BT_DeletePosition_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strID = LB_PositionID.Text.Trim();


        strHQL = "Delete From T_DepartPosition Where ID = " + strID;
        ShareClass.RunSqlCommand(strHQL);


        BT_DeletePosition.Enabled = false;

        LoadDepartPosition();
    }

    protected void BT_KPICheckWeight_Click(object sender, EventArgs e)
    {
        string strHQL;

        decimal deSelfCheckWeight, deLeaderCheckWeight, deThirdPartCheckWeight, deSqlCheckWeight, deHRCheckWeight;
        decimal deSum;

        deSelfCheckWeight = NB_KPISelfCheckWeight.Amount;
        deLeaderCheckWeight = NB_KPILeaderCheckWeight.Amount;
        deThirdPartCheckWeight = NB_KPIThirdPartCheckWeight.Amount;
        deSqlCheckWeight = NB_KPISqlCheckWeight.Amount;
        deHRCheckWeight = NB_KPIHRCheckWeight.Amount;

        deSum = deSelfCheckWeight + deLeaderCheckWeight + deThirdPartCheckWeight + deSqlCheckWeight + deHRCheckWeight;

        if (deSum > 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZZHBNDY1JC") + "')", true);
            return;
        }

        strHQL = " Select * From T_KPICheckTypeWeight ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPICheckTypeWeight");
        if (ds.Tables[0].Rows.Count == 0)
        {
            strHQL = "Insert Into T_KPICheckTypeWeight(SelfCheckWeight,LeaderCheckWeight,ThirdPartCheckWeight,SqlCheckWeight,HRCheckWeight)";
            strHQL += " Values(" + deSelfCheckWeight.ToString() + "," + deLeaderCheckWeight.ToString() + "," + deThirdPartCheckWeight.ToString() + "," + deSqlCheckWeight.ToString() + "," + deHRCheckWeight.ToString() + ")";

            try
            {
                ShareClass.RunSqlCommand(strHQL);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
            }
        }
        else
        {
            strHQL = " Update T_KPICheckTypeWeight Set SelfCheckWeight = " + deSelfCheckWeight.ToString() + ",LeaderCheckWeight = " + deLeaderCheckWeight.ToString() + ",ThirdPartCheckWeight = " + deThirdPartCheckWeight.ToString() + ",SqlCheckWeight = " + deSelfCheckWeight.ToString() + ",HRCheckWeight = " + deHRCheckWeight.ToString();

            try
            {
                ShareClass.RunSqlCommand(strHQL);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
            }
        }
    }

    protected void BT_NewDuty_Click(object sender, EventArgs e)
    {
        string strDuty, strKeyWord;
        string strSortNumber;

        strDuty = TB_Duty.Text.Trim();
        strKeyWord = TB_DutyKeyWord.Text.Trim();
        strSortNumber = TB_DutySort.Text.Trim();

        UserDutyBLL userDutyBLL = new UserDutyBLL();
        UserDuty userDuty = new UserDuty();

        try
        {
            userDuty.Duty = strDuty;
            userDuty.KeyWord = strKeyWord;
            userDuty.SortNumber = int.Parse(strSortNumber);

            if (strKeyWord == "DRIVER" | strKeyWord == "GUARD")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSJHMWZWYJCZJC") + "')", true);
                return;
            }

            userDutyBLL.AddUserDuty(userDuty);
            LoadUserDuty();

            LB_Duty_Backup.Text = strDuty;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void BT_UpdateDuty_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strDuty, strDuty_Backup;
        string strSortNumber;
        string strKeyWord, strKeyWord_Backup;

        strDuty = TB_Duty.Text.Trim();
        strDuty_Backup = LB_Duty_Backup.Text.Trim();
        strKeyWord = TB_DutyKeyWord.Text.Trim();
        strKeyWord_Backup = LB_DutyKeyWord_Backup.Text.Trim();
        strSortNumber = TB_DutySort.Text.Trim();

        try
        {
            if (strKeyWord_Backup != "DRIVER" & strKeyWord_Backup != "GUARD")
            {
                strHQL = "Update T_UserDuty Set Duty = " + "'" + strDuty + "'" + ",KeyWord=" + "'" + strKeyWord + "'" + ",SortNumber = " + strSortNumber;
            }
            else
            {
                strHQL = "Update T_UserDuty Set Duty = " + "'" + strDuty + "'" + ",SortNumber = " + strSortNumber;
            }
            strHQL += " Where Duty = " + "'" + strDuty_Backup + "'";

            ShareClass.RunSqlCommand(strHQL);
            LoadUserDuty();

            LB_Duty_Backup.Text = strDuty;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void BT_DeleteDuty_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDuty;
        string strSortNumber;
        string strKeyWord;

        strDuty = TB_Duty.Text.Trim();
        strSortNumber = TB_DutySort.Text.Trim();

        strHQL = "From UserDuty as userDuty Where userDuty.Duty = " + "'" + strDuty + "'";
        UserDutyBLL userDutyBLL = new UserDutyBLL();
        lst = userDutyBLL.GetAllUserDutys(strHQL);

        try
        {
            UserDuty userDuty = (UserDuty)lst[0];

            strKeyWord = userDuty.KeyWord.Trim();
            if (strKeyWord == "DRIVER" | strKeyWord == "GUARD")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSJHMWZWBNSC") + "')", true);
                return;
            }
            userDutyBLL.DeleteUserDuty(userDuty);

            LoadUserDuty();

            LB_Duty_Backup.Text = "";

            BT_UpdateDuty.Enabled = false;
            BT_DeleteDuty.Enabled = false;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void btn_OilTypeAdd_Click(object sender, EventArgs e)
    {
        string strOilName = txt_OilName.Text.Trim();
        string strOilType = txt_OilType.Text.Trim();
        string strpa = strOilName + "@" + strOilType;
        if (!IsOilTypeExits(strpa))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGSYXHYCZZXSR") + "')", true);
            return;
        }

        OilTypeBLL OilTypeBLL = new OilTypeBLL();
        OilType OilType = new OilType();

        try
        {
            OilType.OilName = strpa;

            OilTypeBLL.AddOilType(OilType);

            LoadOilType();
        }
        catch
        {
        }
    }

    protected void btn_OilTypeDelete_Click(object sender, EventArgs e)
    {
        string strId = txt_ID.Text.Trim();

        OilTypeBLL OilTypeBLL = new OilTypeBLL();
        OilType OilType = new OilType();

        try
        {
            OilType.ID = int.Parse(strId);
            OilTypeBLL.DeleteOilType(OilType);
            LoadOilType();
        }
        catch
        {
        }
    }

    protected void LoadOilType()
    {
        string strHQL = " from OilType as OilType order by OilType.ID ASC";
        OilTypeBLL OilTypeBLL = new OilTypeBLL();
        IList lst = OilTypeBLL.GetAllOilType(strHQL);

        DataGrid33.DataSource = lst;
        DataGrid33.DataBind();
    }

    protected bool IsOilTypeExits(string strP)
    {
        bool flag = true;
        string strHQL = " from OilType as OilType where OilType.OilName = '" + strP + "' ";
        OilTypeBLL OilTypeBLL = new OilTypeBLL();
        IList lst = OilTypeBLL.GetAllOilType(strHQL);
        if (lst.Count > 0)
        {
            flag = false;
        }
        else
            flag = true;

        return flag;
    }

    protected void BT_AddWebSiteOperator_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strWebSite, strUserCode, strUserName, strSortNumber;

        strWebSite = TB_WebSite.Text.Trim();
        strUserCode = TB_SiteUserCode.Text.Trim();
        strUserName = TB_SiteUserName.Text.Trim();
        strSortNumber = TB_WebSiteSort.Text.Trim();

        strHQL = "Select * From T_ProjectMember Where UserCode = " + "'" + strUserCode + "'" + " and UserName = " + "'" + strUserName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");
        if (ds.Tables[0].Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCYHDMHXMBCZJC") + "')", true);
            return;
        }

        try
        {
            strHQL = "Insert Into T_SiteCustomerServiceOperator(WebSite,UserCode,UserName,SortNumber) Values(" + "'" + strWebSite + "'" + "," + "'" + strUserCode + "'" + "," + "'" + strUserName + "'" + "," + strSortNumber + ")";
            ShareClass.RunSqlCommand(strHQL);

            LoadSiteCustomerServiceOperator();
        }
        catch
        {
        }
    }


    protected void BT_DeleteWebSiteOperator_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strWebSite;

        strWebSite = TB_WebSite.Text.Trim();


        try
        {
            strHQL = "Delete From T_SiteCustomerServiceOperator Where WebSite = " + "'" + strWebSite + "'";
            ShareClass.RunSqlCommand(strHQL);

            LoadSiteCustomerServiceOperator();
        }
        catch
        {
        }
    }


    protected void BT_GoodsShipmentAdd_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strType = TB_GoodsShipmentType.Text.Trim();
        string strSort = TB_GoodsShipmentSort.Text.Trim();

        strHQL = "Insert Into T_GoodsShipmentType(TypeName,SortNumber) Values (" + "'" + strType + "'" + "," + strSort + ")";
        ShareClass.RunSqlCommand(strHQL);

        LoadGoodsShipmentType();
    }
    protected void BT_GoodsShipmentDelete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strType = TB_GoodsShipmentType.Text.Trim();
        string strSort = TB_GoodsShipmentSort.Text.Trim();

        strHQL = "Delete From T_GoodsShipmentType Where TypeName = " + "'" + strType + "'";
        ShareClass.RunSqlCommand(strHQL);

        LoadGoodsShipmentType();
    }


    protected void BT_GoodsCheckInAdd_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strType = TB_GoodsCheckInType.Text.Trim();
        string strSort = TB_GoodsCheckInSort.Text.Trim();

        strHQL = "Insert Into T_GoodsCheckInType(TypeName,SortNumber) Values (" + "'" + strType + "'" + "," + strSort + ")";
        ShareClass.RunSqlCommand(strHQL);

        LoadGoodsCheckInType();
    }
    protected void BT_GoodsCheckInDelete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strType = TB_GoodsCheckInType.Text.Trim();
        string strSort = TB_GoodsCheckInSort.Text.Trim();

        strHQL = "Delete From T_GoodsCheckInType Where TypeName = " + "'" + strType + "'";
        ShareClass.RunSqlCommand(strHQL);

        LoadGoodsCheckInType();
    }



    protected void BT_CodeRuleAdd_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strCodeType, strHeadChar, strFieldName, strIsStartup;
        int intFlowIDWidth;

        strCodeType = DL_CodeType.SelectedValue.Trim();
        strHeadChar = TB_HeadChar.Text.Trim();
        strFieldName = DL_FieldRule.SelectedValue.Trim();
        strIsStartup = DL_IsStartup.SelectedValue.Trim();

        try
        {
            intFlowIDWidth = int.Parse(TB_FlowIDWidth.Text.Trim());
        }
        catch
        {
            return;
        }

        strHQL = "Insert Into T_CodeRule(CodeType,HeadChar,FieldName,FlowIDWidth,IsStartup) Values(" + "'" + strCodeType + "'" + "," + "'" + strHeadChar + "'" + "," + "'" + strFieldName + "'" + "," + intFlowIDWidth.ToString() + "," + "'" + strIsStartup + "'" + ")";
        ShareClass.RunSqlCommand(strHQL);

        LoadCodeRule();

    }

    protected void BT_CodeRuleDelete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCodeRuleID;

        strCodeRuleID = LB_CodeRuleID.Text.Trim();

        strHQL = "Delete From T_CodeRule Where ID = " + strCodeRuleID;
        ShareClass.RunSqlCommand(strHQL);

        LoadCodeRule();
    }

    protected void LoadCodeRule()
    {
        string strHQL;

        strHQL = "Select * From T_CodeRule Order By ID ASc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CodeRule");
        DataGrid44.DataSource = ds;
        DataGrid44.DataBind();
    }


    protected void LoadCustomerQuestionStage()
    {
        string strHQL;

        strHQL = "Select * From T_CustomerQuestionStage Order By Possibility ASc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerQuestionStage");

        DataGrid45.DataSource = ds;
        DataGrid45.DataBind();
    }

    protected void LoadCustomerQuestionCustomerStage()
    {
        string strHQL;

        strHQL = "Select * From T_CustomerQuestionCustomerStage Order By SortNumber ASc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CustomerQuestionCustomerStage");

        DataGrid46.DataSource = ds;
        DataGrid46.DataBind();
    }


    protected void DataGrid34_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strTypeName = ((Button)e.Item.FindControl("BT_TypeName")).Text.Trim();
        string strSortNo = e.Item.Cells[1].Text.Trim();

        TB_TypeName.Text = strTypeName;
        TB_TypeSortNo.Text = strSortNo;
    }

    protected void DataGrid35_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strTypeName = ((Button)e.Item.FindControl("BT_CurrencyType")).Text.Trim();
        string strExchangeRate = e.Item.Cells[1].Text.Trim();
        string strSortNo = e.Item.Cells[2].Text.Trim();
        string strIsHome = e.Item.Cells[3].Text.Trim();

        TB_CurrencyType.Text = strTypeName;
        TB_ExchangeRate.Text = strExchangeRate;
        TB_CurrencyTypeSortNo.Text = strSortNo;
        DL_IsHomeCurrency.SelectedValue = strIsHome;
    }

    protected void BT_AddBookReaderType_Click(object sender, EventArgs e)
    {
        string strTypeName = TB_TypeName.Text.Trim();
        string strSortNo = string.IsNullOrEmpty(TB_TypeSortNo.Text.Trim()) ? "0" : TB_TypeSortNo.Text.Trim();
        if (!IsNumeric(strSortNo))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDSXYDY0DZSJC") + "')", true);
            TB_TypeSortNo.Focus();
            return;
        }
        if (strSortNo.Contains(".") || strSortNo.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDSXYDY0DZSJC") + "')", true);
            TB_TypeSortNo.Focus();
            return;
        }
        if (IsBookReaderType(strTypeName))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGLXMCZSJBZYCZJC") + "')", true);
            TB_TypeName.Focus();
            return;
        }

        WorkTypeBLL workTypeBLL = new WorkTypeBLL();
        WorkType workType = new WorkType();

        try
        {
            workType.TypeName = strTypeName;
            workType.SortNo = int.Parse(strSortNo);

            workTypeBLL.AddWorkType(workType);

            LoadBookReaderType();
        }
        catch
        {
        }
    }

    protected void BT_DeleteBookReaderType_Click(object sender, EventArgs e)
    {
        string strTypeName = TB_TypeName.Text.Trim();
        string strSortNo = string.IsNullOrEmpty(TB_TypeSortNo.Text.Trim()) ? "0" : TB_TypeSortNo.Text.Trim();

        WorkTypeBLL workTypeBLL = new WorkTypeBLL();
        WorkType workType = new WorkType();

        try
        {
            workType.TypeName = strTypeName;
            workType.SortNo = int.Parse(strSortNo);

            workTypeBLL.DeleteWorkType(workType);

            LoadBookReaderType();
        }
        catch
        {
        }
    }

    protected void BT_AddCurrencyType_Click(object sender, EventArgs e)
    {
        string strCurrencyType, strExchangeRate, strCurrencyTypeSortNo;

        strCurrencyType = TB_CurrencyType.Text.Trim();
        strExchangeRate = TB_ExchangeRate.Text.Trim();
        strCurrencyTypeSortNo = TB_CurrencyTypeSortNo.Text.Trim();

        if (!IsNumeric(strExchangeRate))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDHLLXBXSSZJC") + "')", true);
            TB_ExchangeRate.Focus();
            return;
        }
        if (!IsNumeric(strCurrencyTypeSortNo))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDSXYDY0DZSJC") + "')", true);
            TB_CurrencyTypeSortNo.Focus();
            return;
        }


        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        CurrencyType currencyType = new CurrencyType();

        try
        {
            currencyType.Type = strCurrencyType;
            currencyType.ExchangeRate = decimal.Parse(strExchangeRate);
            currencyType.SortNo = int.Parse(strCurrencyTypeSortNo);
            currencyType.IsHome = DL_IsHomeCurrency.SelectedValue.Trim();


            currencyTypeBLL.AddCurrencyType(currencyType);

            LoadCurrencyType();
        }
        catch
        {
        }
    }

    protected void BT_DeleteCurrencyType_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCurrencyType;

        strCurrencyType = TB_CurrencyType.Text.Trim();

        try
        {
            strHQL = "Delete From T_CurrencyType Where Type = " + "'" + strCurrencyType + "'";
            ShareClass.RunSqlCommand(strHQL);

            LoadCurrencyType();
        }
        catch
        {
        }
    }

    protected void BT_AddConstractBigType_Click(object sender, EventArgs e)
    {
        string strBigType = TB_ConstractBigType.Text.Trim();
        string strSortNo = TB_ConstractBigTypeSortNo.Text.Trim();

        string strHQL = "Insert Into T_ConstractBigType(BigType,SortNumber) VAlues (" + "'" + strBigType + "'" + "," + "'" + strSortNo + "'" + ")";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadConststactBigType();
        }
        catch
        {
        }
    }

    protected void BT_DeleteConstractType_Click(object sender, EventArgs e)
    {
        string strBigType = TB_ConstractBigType.Text.Trim();
        string strSortNo = TB_ConstractBigTypeSortNo.Text.Trim();

        string strHQL = "Delete From T_ConstractBigType Where BigType = " + "'" + strBigType + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadConststactBigType();
        }
        catch
        {
        }
    }

    protected void BT_SaleTypeNew_Click(object sender, EventArgs e)
    {
        string strType = TB_SaleType.Text.Trim();
        string strSortNo = TB_SaleTypeSort.Text.Trim();

        string strHQL = "Insert Into T_SaleType(Type,SortNumber) VAlues (" + "'" + strType + "'" + "," + "'" + strSortNo + "'" + ")";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadSaleType();
        }
        catch
        {
        }
    }

    protected void BT_SaleTypeDelete_Click(object sender, EventArgs e)
    {
        string strType = TB_SaleType.Text.Trim();
        string strSortNo = TB_SaleTypeSort.Text.Trim();

        string strHQL = "Delete From T_SaleType Where Type =  '" + strType + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadSaleType();
        }
        catch
        {
        }
    }

    protected void BT_ConstractRadioNew_Click(object sender, EventArgs e)
    {
        string strRadio = TB_ConstractRadio.Text.Trim();

        string strHQL = "Insert Into T_ConstractRadio(Radio) VAlues (" + "'" + strRadio + "')";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadConstractRadio();
        }
        catch
        {
        }
    }

    protected void BT_ConstractRadioDelete_Click(object sender, EventArgs e)
    {
        string strRadio = TB_ConstractRadio.Text.Trim();
        string strHQL = "Delete From T_ConstractRadio Where Radio =  '" + strRadio + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadConstractRadio();
        }
        catch
        {
        }
    }

    protected void BT_InvoiceTypeNew_Click(object sender, EventArgs e)
    {
        string strType = TB_InvoiceType.Text.Trim();
        string strSortNo = TB_InvoiceTypeSort.Text.Trim();

        string strHQL = "Insert Into T_InvoiceType(Type,SortNumber) VAlues (" + "'" + strType + "'" + "," + "'" + strSortNo + "'" + ")";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadInvoiceType();
        }
        catch
        {
        }
    }

    protected void BT_InvoiceTypeDelete_Click(object sender, EventArgs e)
    {
        string strType = TB_InvoiceType.Text.Trim();
        string strSortNo = TB_InvoiceTypeSort.Text.Trim();

        string strHQL = "Delete From T_InvoiceType Where Type =  '" + strType + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadInvoiceType();
        }
        catch
        {
        }
    }


    protected void BT_OvertimeTypeNew_Click(object sender, EventArgs e)
    {
        string strType = TB_OvertimeType.Text.Trim();
        string strSortNo = TB_OvertimeTypeSort.Text.Trim();

        string strHQL = "Insert Into T_OvertimeType(Type,SortNumber) VAlues (" + "'" + strType + "'" + "," + "'" + strSortNo + "'" + ")";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadOvertimeType();
        }
        catch
        {
        }
    }

    protected void BT_OvertimeTypeDelete_Click(object sender, EventArgs e)
    {
        string strType = TB_OvertimeType.Text.Trim();
        string strSortNo = TB_OvertimeTypeSort.Text.Trim();

        string strHQL = "Delete From T_OvertimeType Where Type =  '" + strType + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadOvertimeType();
        }
        catch
        {
        }
    }

    protected void BT_FestivalsTypeNew_Click(object sender, EventArgs e)
    {
        string strType = TB_FestivalsType.Text.Trim();
        string strSortNo = TB_FestivalsTypeSort.Text.Trim();

        string strHQL = "Insert Into T_FestivalsType(Type,SortNumber) VAlues (" + "'" + strType + "'" + "," + "'" + strSortNo + "'" + ")";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadFestivalsType();
        }
        catch
        {
        }
    }

    protected void BT_FestivalsTypeDelete_Click(object sender, EventArgs e)
    {
        string strType = TB_FestivalsType.Text.Trim();
        string strSortNo = TB_FestivalsTypeSort.Text.Trim();

        string strHQL = "Delete From T_FestivalsType Where Type =  '" + strType + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadFestivalsType();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 合同大类
    /// </summary>
    protected void LoadConststactBigType()
    {
        string strHQL;
        IList lst;

        strHQL = "from ConstractBigType as constractBigType Order by constractBigType.SortNumber ASC";
        ConstractBigTypeBLL constractBigTypeBLL = new ConstractBigTypeBLL();
        lst = constractBigTypeBLL.GetAllConstractBigTypes(strHQL);

        DataGrid37.DataSource = lst;
        DataGrid37.DataBind();
    }



    /// <summary>
    /// 读者类型数据
    /// </summary>
    protected void LoadBookReaderType()
    {
        string strHQL = "from WorkType as workType order by workType.SortNo ASC";
        WorkTypeBLL workTypeBLL = new WorkTypeBLL();
        IList lst = workTypeBLL.GetAllWorkType(strHQL);

        DataGrid34.DataSource = lst;
        DataGrid34.DataBind();
    }

    /// <summary>
    /// 判断读者类型是否存在  存在返回true；不存在则返回false
    /// </summary>
    protected bool IsBookReaderType(string strtypename)
    {
        bool flag = true;
        string strHQL = "from WorkType as workType Where workType.TypeName='" + strtypename + "' ";
        WorkTypeBLL workTypeBLL = new WorkTypeBLL();
        IList lst = workTypeBLL.GetAllWorkType(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;
        return flag;
    }



    //判断输入的字符是否是数字
    private bool IsNumeric(string str)
    {
        System.Text.RegularExpressions.Regex reg1
            = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
        return reg1.IsMatch(str);
    }

    protected void LoadLeaveType()
    {
        string strHQL;
        IList lst;

        strHQL = "from LeaveType as leaveType Order By leaveType.SortNumber ASC";
        LeaveTypeBLL leaveTypeBLL = new LeaveTypeBLL();
        lst = leaveTypeBLL.GetAllLeaveTypes(strHQL);

        DataGrid17.DataSource = lst;
        DataGrid17.DataBind();
    }

    protected void LoadReceivePayWay()
    {
        string strHQL;
        IList lst;

        strHQL = "From ReceivePayWay as receivePayWay Order By receivePayWay.SortNumber ASC";
        ReceivePayWayBLL receivePayWayBLL = new ReceivePayWayBLL();
        lst = receivePayWayBLL.GetAllReceivePayWays(strHQL);

        DataGrid38.DataSource = lst;
        DataGrid38.DataBind();
    }

    protected void LoadBank()
    {
        string strHQL;
        IList lst;

        strHQL = "From Bank as bank Order By bank.SortNumber ASC";
        BankBLL bankBLL = new BankBLL();
        lst = bankBLL.GetAllBanks(strHQL);

        DataGrid39.DataSource = lst;
        DataGrid39.DataBind();
    }


    protected void LoadDepartPosition()
    {
        string strHQL;
        IList lst;

        strHQL = "From DepartPosition as departPosition Order By departPosition.SortNumber ASC";
        DepartPositionBLL departPositionBLL = new DepartPositionBLL();
        lst = departPositionBLL.GetAllDepartPositions(strHQL);

        DataGrid40.DataSource = lst;
        DataGrid40.DataBind();
    }

    protected void LoadUserDuty()
    {
        string strHQL;
        IList lst;

        strHQL = "From UserDuty as userDuty Order By userDuty.SortNumber ASC";
        UserDutyBLL userDutyBLL = new UserDutyBLL();
        lst = userDutyBLL.GetAllUserDutys(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadKPICheckTypeWeight()
    {
        string strHQL;

        strHQL = "Select SelfCheckWeight,LeaderCheckWeight,ThirdPartCheckWeight,SqlCheckWeight,HRCheckWeight From T_KPICheckTypeWeight";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_KPICheckTypeWeight");

        if (ds.Tables[0].Rows.Count > 0)
        {
            NB_KPISelfCheckWeight.Amount = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
            NB_KPILeaderCheckWeight.Amount = decimal.Parse(ds.Tables[0].Rows[0][1].ToString());
            NB_KPIThirdPartCheckWeight.Amount = decimal.Parse(ds.Tables[0].Rows[0][2].ToString());
            NB_KPISqlCheckWeight.Amount = decimal.Parse(ds.Tables[0].Rows[0][3].ToString());
            NB_KPIHRCheckWeight.Amount = decimal.Parse(ds.Tables[0].Rows[0][4].ToString());
        }
    }


    protected void LoadSiteCustomerServiceOperator()
    {
        string strHQL;

        strHQL = "Select * From T_SiteCustomerServiceOperator Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SiteCustomerServiceOperator");

        DataGrid41.DataSource = ds;
        DataGrid41.DataBind();
    }

    protected void DataGrid17_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_LeaveType")).Text.Trim();
        string strSortNumber = e.Item.Cells[1].Text.Trim();
        TB_LeaveType.Text = strType;
        TB_LeaveSortNumber.Text = strSortNumber;
    }

    protected void BT_AddLeaveType_Click(object sender, EventArgs e)
    {
        if (TB_LeaveType.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJLXWBTSJJC") + "')", true);
            TB_LeaveType.Focus();
            return;
        }
        if (TB_LeaveSortNumber.Text.Trim() != "")
        {
            if (!IsNumeric(TB_LeaveSortNumber.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDSXYDY0DZSJC") + "')", true);
                TB_LeaveSortNumber.Focus();
                return;
            }
        }
        string strType, strSortNumber;

        strType = TB_LeaveType.Text.Trim();
        strSortNumber = TB_LeaveSortNumber.Text.Trim() == "" ? "0" : TB_LeaveSortNumber.Text.Trim();

        LeaveTypeBLL leaveTypeBLL = new LeaveTypeBLL();
        LeaveType leaveType = new LeaveType();

        leaveType.Type = strType;
        leaveType.SortNumber = int.Parse(strSortNumber);

        try
        {
            leaveTypeBLL.AddLeaveType(leaveType);

            LoadLeaveType();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJLXYCZJC") + "')", true);
        }
    }

    protected void BT_DeleteLeaveType_Click(object sender, EventArgs e)
    {
        if (TB_LeaveType.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJLXWBTSJJC") + "')", true);
            TB_LeaveType.Focus();
            return;
        }
        if (TB_LeaveSortNumber.Text.Trim() != "")
        {
            if (!IsNumeric(TB_LeaveSortNumber.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDSXYDY0DZSJC") + "')", true);
                TB_LeaveSortNumber.Focus();
                return;
            }
        }
        string strType, strSortNumber;

        strType = TB_LeaveType.Text.Trim();
        strSortNumber = TB_LeaveSortNumber.Text.Trim() == "" ? "0" : TB_LeaveSortNumber.Text.Trim();

        LeaveTypeBLL leaveTypeBLL = new LeaveTypeBLL();
        LeaveType leaveType = new LeaveType();

        leaveType.Type = strType;
        leaveType.SortNumber = int.Parse(strSortNumber);

        try
        {
            leaveTypeBLL.DeleteLeaveType(leaveType);

            LoadLeaveType();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCJLXSJBCZJC") + "')", true);
        }
    }

    protected void BT_SupplierBigTypeAdd_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_SupplierBigType.Text.Trim();
        strSortNumber = TB_SupplierBigTypeSort.Text.Trim();

        string strHQL = "Insert Into T_BMSupplierBigType(Type,SortNumber) Values('" + strType + "','" + strSortNumber + "')";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSupplierBigType();
        }
        catch
        {
        }
    }

    protected void BT_SupplierBigTypeDelete_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_SupplierBigType.Text.Trim();
        strSortNumber = TB_SupplierBigTypeSort.Text.Trim();

        string strHQL = "Delete From T_BMSupplierBigType Where Type = '" + strType + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSupplierBigType();
        }
        catch
        {
        }
    }

    protected void BT_SupplierSmallTypeAdd_Click(object sender, EventArgs e)
    {
        string strType, strBigType, strSortNumber;

        strType = TB_SupplierSmallType.Text.Trim();
        strBigType = DL_SupplierBigType.SelectedValue.Trim();
        strSortNumber = TB_SupplierSmallTypeSort.Text.Trim();

        string strHQL = "Insert Into T_BMSupplierSmallType(Type,BigType,SortNumber) Values('" + strType + "','" + strBigType + "','" + strSortNumber + "')";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSupplierSmallType();
        }
        catch
        {
        }
    }

    protected void BT_SupplierSmallTypeDelete_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_SupplierSmallType.Text.Trim();
        strSortNumber = TB_SupplierBigTypeSort.Text.Trim();

        string strHQL = "Delete From T_BMSupplierSmallType Where Type = '" + strType + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadSupplierSmallType();
        }
        catch
        {
        }
    }

    protected void BT_BMBidTypeAdd_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_BMBidType.Text.Trim();
        strSortNumber = TB_BMBidTypeSort.Text.Trim();

        string strHQL = "Insert Into T_BMBidType(Type,SortNumber) Values('" + strType + "','" + strSortNumber + "')";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadBMBidType();
        }
        catch
        {
        }
    }

    protected void BT_BMBidTypeDelete_Click(object sender, EventArgs e)
    {
        string strType, strSortNumber;

        strType = TB_BMBidType.Text.Trim();
        strSortNumber = TB_BMBidTypeSort.Text.Trim();

        string strHQL = "Delete From T_BMBidType Where Type = '" + strType + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadBMBidType();
        }
        catch
        {
        }
    }


    protected void LoadDayHourNum()
    {
        string strHQL = "Select * From T_DayHourNum Order By ID Asc ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DayHourNum");

        try
        {
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                lbl_DayHourNumID.Text = ds.Tables[0].Rows[0]["ID"].ToString().Trim();
                TB_HourNum.Text = ds.Tables[0].Rows[0]["HourNum"].ToString().Trim();
                DL_StartHour.SelectedValue = ds.Tables[0].Rows[0]["StartTime"].ToString().Trim().Substring(0, 2);
                DL_StartMin.SelectedValue = ds.Tables[0].Rows[0]["StartTime"].ToString().Trim().Substring(3, 2);
                DL_EndHour.SelectedValue = ds.Tables[0].Rows[0]["EndTime"].ToString().Trim().Substring(0, 2);
                DL_EndMin.SelectedValue = ds.Tables[0].Rows[0]["EndTime"].ToString().Trim().Substring(3, 2);

                DL_RestStartTimeHour.SelectedValue = ds.Tables[0].Rows[0]["RestStartTime"].ToString().Trim().Substring(0, 2);
                DL_RestStartTimeMin.SelectedValue = ds.Tables[0].Rows[0]["RestStartTime"].ToString().Trim().Substring(3, 2);
                DL_RestEndTimeHour.SelectedValue = ds.Tables[0].Rows[0]["RestEndTime"].ToString().Trim().Substring(0, 2);
                DL_RestEndTimeMin.SelectedValue = ds.Tables[0].Rows[0]["RestEndTime"].ToString().Trim().Substring(3, 2);
            }
            else
            {
                lbl_DayHourNumID.Text = "";
                TB_HourNum.Text = "8";
                DL_StartHour.SelectedValue = "08";
                DL_StartMin.SelectedValue = "30";
                DL_EndHour.SelectedValue = "18";
                DL_EndMin.SelectedValue = "30";

                DL_RestStartTimeHour.SelectedValue = "12";
                DL_RestStartTimeMin.SelectedValue = "00";
                DL_RestEndTimeHour.SelectedValue = "14";
                DL_RestEndTimeMin.SelectedValue = "00";
            }
        }
        catch
        {

        }
    }

    protected void BT_DayHourNum_Click(object sender, EventArgs e)
    {
        DayHourNumBLL dayHourNumBLL = new DayHourNumBLL();
        try
        {
            if (lbl_DayHourNumID.Text.Trim() == "")//增加
            {
                DayHourNum dayHourNum = new DayHourNum();

                dayHourNum.HourNum = decimal.Parse(TB_HourNum.Text.Trim() == "" ? "8" : TB_HourNum.Text.Trim());
                dayHourNum.EndTime = DL_EndHour.SelectedValue.Trim() + ":" + DL_EndMin.SelectedValue.Trim();
                dayHourNum.StartTime = DL_StartHour.SelectedValue.Trim() + ":" + DL_StartMin.SelectedValue.Trim();
                dayHourNum.RestStartTime = DL_RestStartTimeHour.SelectedValue.Trim() + ":" + DL_RestStartTimeMin.SelectedValue.Trim();
                dayHourNum.RestEndTime = DL_RestEndTimeHour.SelectedValue.Trim() + ":" + DL_RestEndTimeMin.SelectedValue.Trim();
                DateTime dt1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + dayHourNum.StartTime.Trim());
                DateTime dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + dayHourNum.EndTime.Trim());
                DateTime dt3 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + dayHourNum.RestStartTime.Trim());
                DateTime dt4 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + dayHourNum.RestEndTime.Trim());
                if (dt1 >= dt2)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKSSJYXYJSSJJC") + "')", true);
                    return;
                }
                if (dt3 >= dt4)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXSD1YXYXXSD2JC") + "')", true);
                    return;
                }
                if (dt3 < dt1)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXSD1BNXYKSSJJC") + "')", true);
                    return;
                }
                if (dt4 > dt2)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXSD2BNDYJSSJJC") + "')", true);
                    return;
                }
                TimeSpan ts1 = dt2.Subtract(dt1);
                TimeSpan ts2 = dt4.Subtract(dt3);
                if (double.Parse(dayHourNum.HourNum.ToString()) + ts2.TotalHours > ts1.TotalHours)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZSJXXSJCCSBSJJC") + "')", true);
                    return;
                }
                dayHourNumBLL.AddDayHourNum(dayHourNum);
                lbl_DayHourNumID.Text = GetDayHourNumID();
            }
            else//更新
            {
                string strHQL = "From DayHourNum as dayHourNum Where dayHourNum.ID='" + lbl_DayHourNumID.Text.Trim() + "' ";
                IList lst = dayHourNumBLL.GetAllDayHourNums(strHQL);
                if (lst.Count > 0 && lst != null)
                {
                    DayHourNum dayHourNum = (DayHourNum)lst[0];
                    dayHourNum.HourNum = decimal.Parse(TB_HourNum.Text.Trim() == "" ? "8" : TB_HourNum.Text.Trim());
                    dayHourNum.EndTime = DL_EndHour.SelectedValue.Trim() + ":" + DL_EndMin.SelectedValue.Trim();
                    dayHourNum.StartTime = DL_StartHour.SelectedValue.Trim() + ":" + DL_StartMin.SelectedValue.Trim();
                    dayHourNum.RestStartTime = DL_RestStartTimeHour.SelectedValue.Trim() + ":" + DL_RestStartTimeMin.SelectedValue.Trim();
                    dayHourNum.RestEndTime = DL_RestEndTimeHour.SelectedValue.Trim() + ":" + DL_RestEndTimeMin.SelectedValue.Trim();
                    DateTime dt1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + dayHourNum.StartTime.Trim());
                    DateTime dt2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + dayHourNum.EndTime.Trim());
                    DateTime dt3 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + dayHourNum.RestStartTime.Trim());
                    DateTime dt4 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + dayHourNum.RestEndTime.Trim());
                    if (dt1 >= dt2)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKSSJYXYJSSJJC") + "')", true);
                        return;
                    }
                    if (dt3 >= dt4)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXSD1YXYXXSD2JC") + "')", true);
                        return;
                    }
                    if (dt3 < dt1)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXSD1BNXYKSSJJC") + "')", true);
                        return;
                    }
                    if (dt4 > dt2)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXSD2BNDYJSSJJC") + "')", true);
                        return;
                    }
                    TimeSpan ts1 = dt2.Subtract(dt1);
                    TimeSpan ts2 = dt4.Subtract(dt3);
                    if (double.Parse(dayHourNum.HourNum.ToString()) + ts2.TotalHours > ts1.TotalHours)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZSJXXSJCCSBSJJC") + "')", true);
                        return;
                    }
                    dayHourNumBLL.UpdateDayHourNum(dayHourNum, dayHourNum.ID);
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }

    }

    protected string GetDayHourNumID()
    {
        string flag = "0";
        string strHQL = "From DayHourNum as dayHourNum Order By dayHourNum.ID Desc ";
        DayHourNumBLL dayHourNumBLL = new DayHourNumBLL();
        IList lst = dayHourNumBLL.GetAllDayHourNums(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            DayHourNum dayHourNum = (DayHourNum)lst[0];
            flag = dayHourNum.ID.ToString();
        }
        return flag;
    }

    protected void DataGrid13_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strTenderContent = ((Button)e.Item.FindControl("BT_TenderContent")).Text;
        TB_TenderContent.Text = strTenderContent;
    }

    protected void BT_TenderContentAdd_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strTenderContent;

        strTenderContent = TB_TenderContent.Text.Trim();

        strHQL = string.Format(@"Insert Into T_Tender_Content(TenderContent) values('{0}')", strTenderContent);
        ShareClass.RunSqlCommand(strHQL);

        LoadTenderContent();
    }

    protected void BT_TenderContentDelete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strTenderContent;

        strTenderContent = TB_TenderContent.Text.Trim();

        strHQL = string.Format(@"Delete From T_Tender_Content Where TenderContent= '{0}'", strTenderContent);
        ShareClass.RunSqlCommand(strHQL);

        LoadTenderContent();
    }


    protected void LoadTenderContent()
    {
        string strHQL;

        strHQL = "Select TenderContent from t_tender_content";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Tender_Content");

        DataGrid13.DataSource = ds;
        DataGrid13.DataBind();
    }

    protected void DataGrid20_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strFundingSource = ((Button)e.Item.FindControl("BT_FundingSource")).Text;
        TB_FundingSource.Text = strFundingSource;
    }

    protected void BT_FundingSourceAdd_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strFundingSource;

        strFundingSource = TB_FundingSource.Text.Trim();

        strHQL = string.Format(@"Insert Into T_FundingSource(FundingSource) values('{0}')", strFundingSource);
        ShareClass.RunSqlCommand(strHQL);

        LoadFundingSource();
    }

    protected void BT_FundingSourceDelete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strFundingSource;

        strFundingSource = TB_FundingSource.Text.Trim();

        strHQL = string.Format(@"Delete From T_FundingSource Where FundingSource= '{0}'", strFundingSource);
        ShareClass.RunSqlCommand(strHQL);

        LoadFundingSource();
    }

    protected void LoadFundingSource()
    {
        string strHQL;

        strHQL = "Select FundingSource from t_FundingSource";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_FundingSource");

        DataGrid20.DataSource = ds;
        DataGrid20.DataBind();
    }


    protected void DataGrid21_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strType = ((Button)e.Item.FindControl("BT_ConstractRadio")).Text;

        TB_ConstractRadio.Text = strType;
    }

}
