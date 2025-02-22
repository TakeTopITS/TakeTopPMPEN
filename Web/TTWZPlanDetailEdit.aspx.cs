using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Drawing;


public partial class TTWZPlanDetailEdit : System.Web.UI.Page
{
    public string strUserCode
    {
        get;
        set;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            string strID = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                strID = Request.QueryString["id"].ToString();
            }
            else
            {
                strID = "";
            }

            int intID = 0;
            int.TryParse(strID, out intID);

            DataPlanDetailBind(intID);

            TXT_PlanNumber.BackColor = Color.CornflowerBlue;
            TXT_ConvertPlanNumber.BackColor = Color.CornflowerBlue;
            TXT_Remark.BackColor = Color.CornflowerBlue;
        }
    }

    private void DataPlanDetailBind(int intID)
    {
        string strPlanDetailSQL = string.Format(@"select p.*,o.ObjectName,o.Criterion,o.Model,o.Grade,o.Market,o.ConvertRatio,(Select UnitName From T_WZSpan Where ID = o.Unit) as Unit,(Select UnitName From T_WZSpan Where ID = o.ConvertUnit) as ConvertUnit from T_WZPickingPlanDetail p
                    left join T_WZObject o on p.ObjectCode = o.ObjectCode
                 
                    where p.ID = {0}", intID);
        DataTable dtPlanDetail = ShareClass.GetDataSetFromSql(strPlanDetailSQL, "PlanDetail").Tables[0];
        if (dtPlanDetail != null && dtPlanDetail.Rows.Count > 0)
        {
            DataRow drPlanDetail = dtPlanDetail.Rows[0];

            TXT_ObjectName.Text = ShareClass.ObjectToString(drPlanDetail["ObjectName"]);
            TXT_Criterion.Text = ShareClass.ObjectToString(drPlanDetail["Criterion"]);
            TXT_Model.Text = ShareClass.ObjectToString(drPlanDetail["Model"]);
            TXT_Grade.Text = ShareClass.ObjectToString(drPlanDetail["Grade"]);

            HF_ConvertRatio.Value = ShareClass.ObjectToString(drPlanDetail["ConvertRatio"]);//换算系数
            HF_Market.Value = ShareClass.ObjectToString(drPlanDetail["Market"]);//市场行情
            HF_PickingPlanDetailID.Value = ShareClass.ObjectToString(drPlanDetail["ID"]);

            TXT_PlanNumber.Text = ShareClass.ObjectToString(drPlanDetail["PlanNumber"]);

            TXT_ConvertPlanNumber.Text = ShareClass.ObjectToString(drPlanDetail["ConvertNumber"]);
            TXT_Remark.Text = ShareClass.ObjectToString(drPlanDetail["Remark"]);

            LB_PlanUnit.Text = ShareClass.ObjectToString(drPlanDetail["Unit"]);
            LB_ConvertUnit.Text = ShareClass.ObjectToString(drPlanDetail["ConvertUnit"]);
            LB_ConvertRadio.Text = ShareClass.ObjectToString(drPlanDetail["ConvertRatio"]);
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(HF_PickingPlanDetailID.Value))
            {
                string strPlanNumber = TXT_PlanNumber.Text.Trim();
                string strConvertPlanNumber = TXT_ConvertPlanNumber.Text.Trim();
                string strRemark = TXT_Remark.Text.Trim();

                if (!ShareClass.CheckIsNumber(strPlanNumber) )
                {
                    if (Decimal.Parse(strPlanNumber) == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHSLBXSXSHZZSBBNWL + "')", true);
                        return;
                    }

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHSSLBXSXSHZZS + "')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strConvertPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHSSLBXSXSHZZS + "')", true);
                    return;
                }
                if (!ShareClass.CheckStringRight(strRemark))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBZBNWFFZFC + "')", true);
                    return;
                }

                int intPickingPlanDetailID = 0;
                int.TryParse(HF_PickingPlanDetailID.Value, out intPickingPlanDetailID);

                WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                string strWZPickingPlanDetailHQL = "from WZPickingPlanDetail as wZPickingPlanDetail where ID = " + intPickingPlanDetailID;
                IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                {
                    WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];
                    decimal decimalPlanNumber = 0;
                    decimal.TryParse(strPlanNumber, out decimalPlanNumber);
                    decimal decimalConvertNumber = 0;
                    decimal.TryParse(strConvertPlanNumber, out decimalConvertNumber);
                    decimal decimalMarket = 0;
                    decimal.TryParse(HF_Market.Value, out decimalMarket);
                    decimal decimalConvertRadio = 0;
                    decimal.TryParse(HF_ConvertRatio.Value, out decimalConvertRadio);

                    wZPickingPlanDetail.PlanNumber = decimalPlanNumber;
                    wZPickingPlanDetail.ConvertNumber = decimalConvertNumber;
                    wZPickingPlanDetail.Remark = strRemark;
                    wZPickingPlanDetail.PlanCost = decimalPlanNumber * decimalMarket;
                    wZPickingPlanDetail.ShortNumber = decimalPlanNumber - wZPickingPlanDetail.ReceivedNumber;
                    wZPickingPlanDetail.ShortConver = decimalPlanNumber / decimalConvertRadio;

                    wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, intPickingPlanDetailID);

                    //计划数量、预计费用写入计划里面
                    string strTotalPickingPlanDetailHQL = string.Format(@"select COUNT(1) as RowNumber,SUM(PlanCost) as TotalPlanCost from T_WZPickingPlanDetail
                                    where PlanCode = '{0}'
                                    group by PlanCode", wZPickingPlanDetail.PlanCode);
                    DataTable dtTotalPlan = ShareClass.GetDataSetFromSql(strTotalPickingPlanDetailHQL, "PickingPlanDetail").Tables[0];
                    int intDetailCount = 0;
                    decimal decimalPlanCost = 0;
                    if (dtTotalPlan != null && dtTotalPlan.Rows.Count > 0)
                    {
                        int.TryParse(ShareClass.ObjectToString(dtTotalPlan.Rows[0]["RowNumber"]), out intDetailCount);
                        decimal.TryParse(ShareClass.ObjectToString(dtTotalPlan.Rows[0]["TotalPlanCost"]), out decimalPlanCost);
                    }

                    int intIsMark = 0;
                    if (intDetailCount > 0)
                    {
                        intIsMark = -1;
                    }
                    string strUpdatePickPlanHQL = string.Format(@"update T_WZPickingPlan 
                                        set DetailCount = {0},
                                            PlanCost={1},
                                            IsMark = {3}
                                        where PlanCode='{2}'", intDetailCount, decimalPlanCost, wZPickingPlanDetail.PlanCode, intIsMark);
                    ShareClass.RunSqlCommand(strUpdatePickPlanHQL);



                    //重新加载计划明细列表
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
                }
            }
        }
        catch (Exception ex) {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCSB + "')", true);
        }
    }

    protected void btnPlan_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(HF_PickingPlanDetailID.Value))
            {
                string strPlanNumber = TXT_PlanNumber.Text.Trim();
                if (string.IsNullOrEmpty(strPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXTXJHSL + "')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHSLBXWXSHZZS + "')", true);
                    return;
                }

                decimal deciamlPlanNumber = 0;
                decimal.TryParse(strPlanNumber, out deciamlPlanNumber);

                if(deciamlPlanNumber == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHSLBNWLQJC + "')", true);
                    return;
                }

                decimal decimalConvertRatio = 0;
                decimal.TryParse(HF_ConvertRatio.Value, out decimalConvertRatio);

                decimal decimalResult = 0;
                if (decimalConvertRatio != 0)
                {
                    decimalResult = deciamlPlanNumber / decimalConvertRatio;
                }
                TXT_ConvertPlanNumber.Text = decimalResult.ToString("#0.000");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZJHMX + "')", true);
                return;
            }
        }
        catch (Exception ex)
        { }
    }


    protected void btnConvert_Click(object sender, EventArgs e)
    {
        try
        {
            //①〈换算数量〉＿赋值												
            //则〈计划数量〉＝〈换算数量〉×物资代码〈换算系数〉    例.  t＝m×系数												

            if (!string.IsNullOrEmpty(HF_PickingPlanDetailID.Value))
            {
                string strConvertPlanNumber = TXT_ConvertPlanNumber.Text.Trim();
                if (string.IsNullOrEmpty(strConvertPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXTXJHSL + "')", true);
                    return;
                }
                if (!ShareClass.CheckIsNumber(strConvertPlanNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJHSLBXWXSHZZS + "')", true);
                    return;
                }

                decimal deciamlConvertPlanNumber = 0;
                decimal.TryParse(strConvertPlanNumber, out deciamlConvertPlanNumber);

                if (deciamlConvertPlanNumber == 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZHSSLBNWLQJC + "')", true);
                    return;
                }

                decimal decimalConvertRatio = 0;
                decimal.TryParse(HF_ConvertRatio.Value, out decimalConvertRatio);

                decimal decimalResult = deciamlConvertPlanNumber * decimalConvertRatio;
                TXT_PlanNumber.Text = decimalResult.ToString("#0.000");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZJHMX + "')", true);
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }
}