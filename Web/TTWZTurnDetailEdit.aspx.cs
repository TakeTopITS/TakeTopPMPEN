
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZTurnDetailEdit : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataTurnBinder();

            DG_TurnDetail.DataSource = "";
            DG_TurnDetail.DataBind();
        }
    }

    private void DataTurnBinder()
    {
        WZTurnBLL wZTurnBLL = new WZTurnBLL();
        string strWZTurnHQL = "from WZTurn as wZTurn Where ((wZTurn.MaterialPerson = " + "'" + strUserCode + "'" + ") or (wZTurn.PurchaseEngineer = " + "'" + strUserCode + "'" + ") or( wZTurn.CheckPerson = " + "'" + strUserCode + "'" + " and wZTurn.Progress = 'Acceptance')) order by  wZTurn.TurnTime desc";

        IList listWZTurn = wZTurnBLL.GetAllWZTurns(strWZTurnHQL);
        LB_Turn.DataSource = listWZTurn;
        LB_Turn.DataBind();
    }

    private void DataTurnDetailBinder()
    {
        if (!string.IsNullOrEmpty(LB_Turn.SelectedValue))
        {
            string strWZTurnDetailHQL = string.Format(@"select t.*,p.PlanCode as PickingPlanCode,
                                m.UserName as MaterialPersonName,
                                o.ObjectName
                                from T_WZTurnDetail t
                                left join T_WZPickingPlanDetail p on t.PlanCode = p.ID 
                                left join T_ProjectMember m on t.MaterialPerson = m.UserCode
                                left join T_WZObject o on t.ObjectCode = o.ObjectCode
                                where t.TurnCode= '{0}'", LB_Turn.SelectedValue);
            DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strWZTurnDetailHQL, "TurnDetail").Tables[0];

            DG_TurnDetail.DataSource = dtTurnDetail;
            DG_TurnDetail.DataBind();

            LB_Sql.Text = strWZTurnDetailHQL;

            string strMaterialPerson, strCheckPerson, strPurchaseEngineer;

            strCheckPerson = GetCheckPersonFormWZTurn(LB_Turn.SelectedValue).Trim();
            strPurchaseEngineer = GetPurchaseEngineerFormWZTurn(LB_Turn.SelectedValue).Trim();

            string strProgress;
            for (int i = 0; i < DG_TurnDetail.Items.Count; i++)
            {
                strMaterialPerson = dtTurnDetail.Rows[i]["MaterialPerson"].ToString().Trim();

                strProgress = dtTurnDetail.Rows[i]["Progress"].ToString();
                if (strProgress == LanguageHandle.GetWord("LuRu").ToString().Trim())
                {
                    DG_TurnDetail.Items[i].Cells[16].Text = "";
                }

                if ((strProgress == LanguageHandle.GetWord("QianShou").ToString().Trim() | strProgress == LanguageHandle.GetWord("LuRu").ToString().Trim()) & (strMaterialPerson == strUserCode | strMaterialPerson == strUserName))
                {
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_Virturl")).Visible = true;
                }
                else
                {
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_Virturl")).Visible = false;
                }

                //当前用户为验收人且不是材料员
                if ((strCheckPerson == strUserCode | strCheckPerson == strUserName) & (strMaterialPerson != strUserCode & strMaterialPerson != strUserName) & (strPurchaseEngineer != strUserCode))
                {
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_Virturl")).Visible = false;

                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_Edit")).Visible = false;
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_CancelVirturl")).Visible = false;
                }


                if (strPurchaseEngineer == strUserCode & strPurchaseEngineer != strMaterialPerson & GetWZTurnProgress(LB_Turn.SelectedValue) == LanguageHandle.GetWord("QianShou").ToString().Trim())
                {
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_CancelVirturl")).Visible = false;
                }


                if (strProgress == LanguageHandle.GetWord("QianShou").ToString().Trim())
                {
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_Edit")).Visible = false;
                }

                if (strProgress == "Completed")
                {
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_Edit")).Visible = false;
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_CancelVirturl")).Visible = false;
                }

                if (strProgress == "Acceptance")
                {
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_Edit")).Visible = false;
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_CancelVirturl")).Visible = false;
                }

                if (strProgress == LanguageHandle.GetWord("HeXiao").ToString().Trim())
                {
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_Edit")).Visible = false;
                    ((LinkButton)DG_TurnDetail.Items[i].FindControl("LBT_CancelVirturl")).Visible = false;
                }


            }
        }
    }

    protected string GetWZTurnProgress(string strTurnCode)
    {
        string strHQL = "Select Progress From T_WZTurn  where TurnCode = '" + strTurnCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurn");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    //取得验收人代码或名称
    protected string GetCheckPersonFormWZTurn(string strTurnCode)
    {
        string strHQL;

        strHQL = "Select CheckPerson From T_WZTurn Where TurnCode = " + "'" + strTurnCode + "'";
        DataTable dtTurn = ShareClass.GetDataSetFromSql(strHQL, "Turn").Tables[0];

        return dtTurn.Rows[0][0].ToString();
    }

    //取得采购工程师的代码
    protected string GetPurchaseEngineerFormWZTurn(string strTurnCode)
    {
        string strHQL;

        strHQL = "Select PurchaseEngineer From T_WZTurn Where TurnCode = " + "'" + strTurnCode + "'";
        DataTable dtTurn = ShareClass.GetDataSetFromSql(strHQL, "Turn").Tables[0];

        return dtTurn.Rows[0][0].ToString();
    }

    protected void DG_TurnDetail_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            for (int i = 0; i < DG_TurnDetail.Items.Count; i++)
            {
                DG_TurnDetail.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string cmdName = e.CommandName;
            if (cmdName == "edit")
            {
                string cmdArges = e.CommandArgument.ToString();

                WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where ID= " + cmdArges;
                IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
                if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
                {
                    WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];
                    HF_ID.Value = wZTurnDetail.ID.ToString();
                    DDL_PickingMethod.SelectedValue = wZTurnDetail.PickingMethod;
                    TXT_NoCode.Text = wZTurnDetail.NoCode;
                    TXT_TicketNumber.Text = wZTurnDetail.TicketNumber.ToString();
                    TXT_TicketPrice.Text = wZTurnDetail.TicketPrice.ToString();
                    TXT_TicketMoney.Text = wZTurnDetail.TicketMoney.ToString();
                    TXT_ActualNumber.Text = wZTurnDetail.ActualNumber.ToString();

                    if (wZTurnDetail.PickingMethod == LanguageHandle.GetWord("GongPiao").ToString().Trim())
                    {
                        TXT_ActualNumber.BackColor = Color.Red;
                        TXT_TicketNumber.BackColor = Color.Red;
                    }
                    else //if (wZTurnDetail.PickingMethod == LanguageHandle.GetWord("LanPiao").ToString().Trim()) 
                    {
                        TXT_ActualNumber.BackColor = Color.CornflowerBlue;
                        TXT_TicketNumber.BackColor = Color.CornflowerBlue;
                    }

                    DDL_PickingMethod.BackColor = Color.CornflowerBlue;
                    TXT_NoCode.BackColor = Color.CornflowerBlue;
                    TXT_TicketPrice.BackColor = Color.CornflowerBlue;
                    TXT_TicketMoney.BackColor = Color.CornflowerBlue;

                    if (wZTurnDetail.Progress.Trim() == LanguageHandle.GetWord("DaShou").ToString().Trim())
                    {
                        DDL_PickingMethod.Enabled = false;
                        TXT_TicketNumber.Enabled = false;

                        DDL_PickingMethod.ForeColor = Color.Black;
                        TXT_TicketNumber.ForeColor = Color.Black;

                        DDL_PickingMethod.BackColor = Color.Gray;
                        TXT_TicketNumber.BackColor = Color.Gray;
                    }
                    else
                    {
                        DDL_PickingMethod.Enabled = true;
                        TXT_TicketNumber.Enabled = true;
                    }


                    string strMaterialPerson;
                    strMaterialPerson = wZTurnDetail.MaterialPerson.Trim();

                    if (strMaterialPerson == strUserCode)
                    {
                        DDL_PickingMethod.Enabled = false;
                        //TXT_NoCode.Enabled = false;
                        TXT_TicketNumber.Enabled = false;

                        DDL_PickingMethod.ForeColor = Color.Black;
                        //TXT_NoCode.ForeColor = Color.Black;
                        TXT_TicketNumber.ForeColor = Color.Black;


                        DDL_PickingMethod.BackColor = Color.Gray;
                        //TXT_NoCode.BackColor = Color.Gray;
                        TXT_TicketNumber.BackColor = Color.Gray;
                    }
                    else
                    {
                        DDL_PickingMethod.Enabled = true;
                        TXT_NoCode.Enabled = true;
                        TXT_TicketNumber.Enabled = true;
                        TXT_ActualNumber.Enabled = true;
                        TXT_TicketPrice.Enabled = true;
                        TXT_TicketMoney.Enabled = true;
                    }

                    //TXT_NoCode.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
            else if (cmdName == "virturl")
            {
                //假入库
                try
                {
                    string cmdArges = e.CommandArgument.ToString();

                    int intTurnDetailID = 0;
                    int.TryParse(cmdArges, out intTurnDetailID);
                    WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                    string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where ID= " + intTurnDetailID;
                    IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
                    if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
                    {
                        WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];

                        wZTurnDetail.Progress = LanguageHandle.GetWord("LingLiao").ToString().Trim();
                        wZTurnDetail.IsMark = -1;
                        wZTurnDetailBLL.UpdateWZTurnDetail(wZTurnDetail, intTurnDetailID);

                        //计划明细<已供数量>=移交明细<实领数量>
                        //计划明细<缺口数量>=计划明细<计划数量>-计划明细<已供数量>
                        //计划明细<缺口换算>=计划明细<缺口数量>/物资代码<换算系数>
                        //计划明细<进度> = '发料'
                        WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                        string strWZPickingPlanDetailHQL = string.Format(@"from WZPickingPlanDetail as wZPickingPlanDetail where wZPickingPlanDetail.ID = {0}", wZTurnDetail.PlanCode);
                        IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                        if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                        {
                            WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];

                            //wZPickingPlanDetail.ReceivedNumber = wZPickingPlanDetail.ReceivedNumber + wZTurnDetail.ActualNumber;
                            wZPickingPlanDetail.ReceivedNumber = wZTurnDetail.ActualNumber;
                            wZPickingPlanDetail.ShortNumber = wZPickingPlanDetail.PlanNumber - wZPickingPlanDetail.ReceivedNumber;

                            //根据物资代码查询换算系数
                            decimal decimalConvertRatio = 0;
                            string strObjectConvertRatioHQL = string.Format("select ConvertRatio from T_WZObject where ObjectCode = '{0}'", wZTurnDetail.ObjectCode);
                            DataTable dtObjectConverRatio = ShareClass.GetDataSetFromSql(strObjectConvertRatioHQL, "ObjectConvertRatio").Tables[0];
                            if (dtObjectConverRatio != null && dtObjectConverRatio.Rows.Count > 0)
                            {
                                decimal.TryParse(dtObjectConverRatio.Rows[0]["ConvertRatio"].ToString(), out decimalConvertRatio);
                                if (decimalConvertRatio > 0)
                                {
                                    wZPickingPlanDetail.ShortConver = wZPickingPlanDetail.ShortNumber / decimalConvertRatio;
                                }
                            }
                            wZPickingPlanDetail.Progress = LanguageHandle.GetWord("FaLiao").ToString().Trim();

                            wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, int.Parse(wZTurnDetail.PlanCode));

                            //string strHQL;
                            //strHQL = "Update T_WZPickingPlan Set Progress = '发料' Where PlanCode = " + "'" + wZPickingPlanDetail.PlanCode + "'";
                            //ShareClass.RunSqlCommand(strHQL);

                            //重新加载移交单明细列表
                            DataTurnDetailBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJRKCG").ToString().Trim() + "')", true);
                        }
                    }
                }
                catch (Exception ex) { }
            }
            else if (cmdName == "cancelVirturl")
            {
                //取消上帐
                try
                {
                    string cmdArges = e.CommandArgument.ToString();

                    int intTurnDetailID = 0;
                    int.TryParse(cmdArges, out intTurnDetailID);
                    WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
                    string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where ID= " + intTurnDetailID;
                    IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
                    if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
                    {
                        WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];

                        wZTurnDetail.Progress = LanguageHandle.GetWord("LuRu").ToString().Trim();
                        wZTurnDetail.IsMark = 0;
                        wZTurnDetailBLL.UpdateWZTurnDetail(wZTurnDetail, intTurnDetailID);

                        //计划明细<已供数量>=0;
                        //计划明细<缺口数量>=计划明细<计划数量>-计划明细<已供数量>
                        //计划明细<缺口换算>=计划明细<缺口数量>/物资代码<换算系数>
                        //计划明细<进度> = '移交'
                        WZPickingPlanDetailBLL wZPickingPlanDetailBLL = new WZPickingPlanDetailBLL();
                        string strWZPickingPlanDetailHQL = string.Format(@"from WZPickingPlanDetail as wZPickingPlanDetail where ID = {0}", wZTurnDetail.PlanCode);
                        IList listWZPickingPlanDetail = wZPickingPlanDetailBLL.GetAllWZPickingPlanDetails(strWZPickingPlanDetailHQL);
                        if (listWZPickingPlanDetail != null && listWZPickingPlanDetail.Count > 0)
                        {
                            WZPickingPlanDetail wZPickingPlanDetail = (WZPickingPlanDetail)listWZPickingPlanDetail[0];

                            wZPickingPlanDetail.ReceivedNumber = wZPickingPlanDetail.ReceivedNumber - wZTurnDetail.ActualNumber;
                            wZPickingPlanDetail.ShortNumber = wZPickingPlanDetail.PlanNumber - wZPickingPlanDetail.ReceivedNumber;

                            //根据物资代码查询换算系数
                            decimal decimalConvertRatio = 0;
                            string strObjectConvertRatioHQL = string.Format("select ConvertRatio from T_WZObject where ObjectCode = '{0}'", wZTurnDetail.ObjectCode);
                            DataTable dtObjectConverRatio = ShareClass.GetDataSetFromSql(strObjectConvertRatioHQL, "ObjectConvertRatio").Tables[0];
                            if (dtObjectConverRatio != null && dtObjectConverRatio.Rows.Count > 0)
                            {
                                decimal.TryParse(dtObjectConverRatio.Rows[0]["ConvertRatio"].ToString(), out decimalConvertRatio);
                                if (decimalConvertRatio > 0)
                                {
                                    wZPickingPlanDetail.ShortConver = wZPickingPlanDetail.ShortNumber / decimalConvertRatio;
                                }
                            }
                            wZPickingPlanDetail.Progress = LanguageHandle.GetWord("YiJiao").ToString().Trim();

                            wZPickingPlanDetailBLL.UpdateWZPickingPlanDetail(wZPickingPlanDetail, int.Parse(wZTurnDetail.PlanCode));

                            //重新加载移交单明细列表
                            DataTurnDetailBinder();

                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXSZCG").ToString().Trim() + "')", true);
                        }
                    }
                }
                catch (Exception ex) { }
            }


            //材料员没有对这3个字段编辑的权限
            DDL_PickingMethod.Enabled = false;
            TXT_NoCode.Enabled = false;
            TXT_TicketNumber.Enabled = false;

            DDL_PickingMethod.ForeColor = Color.Black;
            TXT_NoCode.ForeColor = Color.Black;
            TXT_TicketNumber.ForeColor = Color.Black;

            DDL_PickingMethod.BackColor = Color.Gray;
            TXT_NoCode.BackColor = Color.Gray;
            TXT_TicketNumber.BackColor = Color.Gray;
        }
    }


    protected void DG_TurnDetail_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_TurnDetail.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text.Trim();
        DataTable dtTurnDetail = ShareClass.GetDataSetFromSql(strHQL, "TurnDetail").Tables[0];

        DG_TurnDetail.DataSource = dtTurnDetail;
        DG_TurnDetail.DataBind();
    }

    protected void LB_Turn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LB_Turn.SelectedValue))
        {
            string strHQL;
            string strTurnCode, strPurchaseEngineer, strMaterialPerson, strCheckPerson, strProgress;

            strTurnCode = LB_Turn.SelectedValue.Trim();

            strHQL = "Select PurchaseEngineer,MaterialPerson,CheckPerson,Progress From T_WZTurn Where TurnCode = " + "'" + strTurnCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurn");
            strPurchaseEngineer = ds.Tables[0].Rows[0][0].ToString().Trim();
            strMaterialPerson = ds.Tables[0].Rows[0][1].ToString().Trim();
            strCheckPerson = ds.Tables[0].Rows[0][2].ToString().Trim();
            strProgress = ds.Tables[0].Rows[0][3].ToString().Trim();

            BT_NewDetailBrowse.Enabled = true;

            if (strPurchaseEngineer == strUserCode)
            {
                btnSave.Enabled = true;
                btnReset.Enabled = true;

                btnSign.Enabled = false;
                btnAccetance.Enabled = false;
                btnCheck.Enabled = false;
                btnCancelCheck.Enabled = false;
            }

            if (strCheckPerson == strUserCode)
            {
                btnFinish.Enabled = true;

                btnSave.Enabled = false;
                btnReset.Enabled = false;

                btnSign.Enabled = false;
                btnAccetance.Enabled = false;
                btnCheck.Enabled = false;
                btnCancelCheck.Enabled = false;
            }

            if (strMaterialPerson == strUserCode)
            {
                if (strProgress == LanguageHandle.GetWord("QianShou").ToString().Trim())
                {
                    btnAccetance.Enabled = true;

                    btnSave.Enabled = false;
                    btnReset.Enabled = false;
                    btnSign.Enabled = false;
                }

                if (strProgress == "Completed")
                {
                    btnCheck.Enabled = true;
                    btnCancelCheck.Enabled = true;
                }
            }

            if (strProgress == LanguageHandle.GetWord("QianShou").ToString().Trim())
            {
                btnSign.Enabled = false;

                btnSave.Enabled = false;
                btnReset.Enabled = false;
            }

            if (strProgress == "Acceptance")
            {
                btnAccetance.Enabled = false;

                btnSign.Enabled = false;
                btnSave.Enabled = false;
                btnReset.Enabled = false;

                if (strCheckPerson == strUserCode | strCheckPerson == strUserName)
                {
                    btnAccetance.Enabled = true;
                }
            }

            if (strProgress == "Completed")
            {
                btnFinish.Enabled = false;
                btnSave.Enabled = false;
                btnReset.Enabled = false;
                btnSign.Enabled = false;
                btnAccetance.Enabled = false;
            }

            if (strProgress == LanguageHandle.GetWord("HeXiao").ToString().Trim())
            {
                btnCheck.Enabled = false;

                btnFinish.Enabled = false;
                btnSave.Enabled = false;
                btnReset.Enabled = false;
                btnSign.Enabled = false;
                btnAccetance.Enabled = false;
            }

            if (strCheckPerson == strUserCode)
            {
                btnCheck.Enabled = false;
                btnCancelCheck.Enabled = false;
            }

            DataTurnDetailBinder();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strID = HF_ID.Value;
            string strPickingMethod = DDL_PickingMethod.SelectedValue;
            string strNoCode = TXT_NoCode.Text.Trim();
            string strTicketNumber = TXT_TicketNumber.Text.Trim();
            string strTicketPrice = TXT_TicketPrice.Text.Trim();
            string strTicketMoney = TXT_TicketMoney.Text.Trim();
            string strActualNumber = TXT_ActualNumber.Text.Trim();

            if (string.IsNullOrEmpty(strID) || strID == "0" || string.IsNullOrEmpty(strActualNumber) || string.IsNullOrEmpty(strTicketPrice) || string.IsNullOrEmpty(strTicketMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYBJDYJDMX").ToString().Trim() + "')", true);
                return;
            }

            int intTurnDetailID = 0;
            int.TryParse(strID, out intTurnDetailID);


            WZTurnDetailBLL wZTurnDetailBLL = new WZTurnDetailBLL();
            string strWZTurnDetailHQL = "from WZTurnDetail as wZTurnDetail where ID= " + intTurnDetailID;
            IList listWZTurnDetail = wZTurnDetailBLL.GetAllWZTurnDetails(strWZTurnDetailHQL);
            if (listWZTurnDetail != null && listWZTurnDetail.Count > 0)
            {
                WZTurnDetail wZTurnDetail = (WZTurnDetail)listWZTurnDetail[0];

                //if (wZTurnDetail.Progress != LanguageHandle.GetWord("LuRu").ToString().Trim())
                //{
                //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDYJDMXJDBWLRBYXBJ").ToString().Trim()+"')", true);
                //    return;
                //}

                if (string.IsNullOrEmpty(strNoCode))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZNOBHBNWKBC").ToString().Trim() + "')", true);
                    return;
                }

                if (!ShareClass.CheckIsNumber(strActualNumber) | string.IsNullOrEmpty(strActualNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShiLingShuLiangBuNengWeiKong").ToString().Trim()+"')", true);
                    return;
                }

                if (!ShareClass.CheckIsNumber(strTicketNumber))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPSLZNWXS").ToString().Trim() + "')", true);
                    return;
                }

                if (!ShareClass.CheckIsNumber(strTicketPrice) | string.IsNullOrEmpty(strTicketPrice))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPDJZNWXS").ToString().Trim() + "')", true);
                    return;
                }

                if (!ShareClass.CheckIsNumber(strTicketMoney) | string.IsNullOrEmpty(strTicketMoney))
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPJEZNWXS").ToString().Trim() + "')", true);
                    return;
                }


                decimal decimalTicketNumber = 0;
                decimal.TryParse(strTicketNumber, out decimalTicketNumber);
                decimal decimalTicketPrice = 0;
                decimal.TryParse(strTicketPrice, out decimalTicketPrice);
                decimal decimalTicketMoney = 0;
                decimal.TryParse(strTicketMoney, out decimalTicketMoney);
                decimal decimalActualNumber = 0;
                decimal.TryParse(strActualNumber, out decimalActualNumber);


                wZTurnDetail.PickingMethod = strPickingMethod;
                wZTurnDetail.NoCode = strNoCode;
                wZTurnDetail.TicketNumber = decimalTicketNumber;
                wZTurnDetail.TicketPrice = decimalTicketPrice;
                wZTurnDetail.TicketMoney = decimalTicketMoney;
                wZTurnDetail.ActualNumber = decimalActualNumber;

                wZTurnDetailBLL.UpdateWZTurnDetail(wZTurnDetail, intTurnDetailID);

                string strHQL;
                strHQL = "Update T_WZPickingPlanDetail Set ReceivedNumber = " + strActualNumber + " Where ID = " + wZTurnDetail.PlanCode;
                ShareClass.RunSqlCommand(strHQL);
                strHQL = "Update T_WZPickingPlanDetail Set TurnCode = " + "'" + wZTurnDetail.TurnCode + "'" + " Where ID = " + wZTurnDetail.PlanCode;
                ShareClass.RunSqlCommand(strHQL);
                strHQL = "Update T_WZPickingPlanDetail Set ShortNumber = PlanNumber - " + strActualNumber + " Where ID = " + wZTurnDetail.PlanCode;
                ShareClass.RunSqlCommand(strHQL);
                strHQL = "Update T_WZPickingPlanDetail Set ShortConver = ShortNumber / " + GetObjectConvertRatio(wZTurnDetail.ObjectCode).ToString() + " Where ID = " + wZTurnDetail.PlanCode;
                ShareClass.RunSqlCommand(strHQL);

                //重新加载移交单明细列表
                DataTurnDetailBinder();

                HF_ID.Value = "0";
                DDL_PickingMethod.SelectedIndex = 0;
                TXT_NoCode.Text = "";
                TXT_TicketNumber.Text = "0";
                TXT_TicketPrice.Text = "0";
                TXT_TicketMoney.Text = "0";
                TXT_ActualNumber.Text = "0";

                DDL_PickingMethod.BackColor = Color.White;
                TXT_NoCode.BackColor = Color.White;
                TXT_TicketNumber.BackColor = Color.White;
                TXT_TicketPrice.BackColor = Color.White;
                TXT_TicketMoney.BackColor = Color.White;
                TXT_ActualNumber.BackColor = Color.White;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
        }
        catch (Exception ex) { }
    }

    protected decimal GetObjectConvertRatio(string strObjectCode)
    {
        string strHQL;

        strHQL = "Select COALESCE(ConvertRatio,1) From T_WZObject Where ObjectCode = " + "'" + strObjectCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZObject");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 1;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_TurnDetail.Items.Count; i++)
        {
            DG_TurnDetail.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "0";
        DDL_PickingMethod.SelectedIndex = 0;
        TXT_NoCode.Text = "";
        TXT_TicketNumber.Text = "0";
        TXT_TicketPrice.Text = "0";
        TXT_TicketMoney.Text = "0";
        TXT_ActualNumber.Text = "0";

        DDL_PickingMethod.BackColor = Color.White;
        TXT_NoCode.BackColor = Color.White;
        TXT_TicketNumber.BackColor = Color.White;
        TXT_TicketPrice.BackColor = Color.White;
        TXT_TicketMoney.BackColor = Color.White;
        TXT_ActualNumber.BackColor = Color.White;
    }


    protected void btnSign_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;
            string strTurnCode, strID;

            strTurnCode = LB_Turn.SelectedValue.Trim();
            strID = HF_ID.Value;


            //strHQL = "Update T_WZTurnDetail Set Progress = '签收' Where TurnCode = " + "'" + strTurnCode + "'" + " and ID = " + strID;
            //ShareClass.RunSqlCommand(strHQL);

            //strHQL = "Select * From T_WZTurnDetail Where TurnCode = " + "'" + strTurnCode + "'" + " and Progress <> '签收'";
            //DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurnDetail");
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            strHQL = "Update T_WZTurn Set Progress = '签收' Where TurnCode = " + "'" + strTurnCode + "'"; 
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_WZTurn Set SingTime = " + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + " Where TurnCode = " + "'" + strTurnCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_WZTurnDetail Set PickingTime = now() Where TurnCode = " + "'" + strTurnCode + "'" + " and ID = " + strID;
            ShareClass.RunSqlCommand(strHQL);
            //}

            DataTurnDetailBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZQianShouChengGong").ToString().Trim()+"')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('签收失败，请检查！')" + ex.Message.ToString(), true); 
        }
    }

    protected void btnAccetance_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;
            string strTurnCode, strID;

            strTurnCode = LB_Turn.SelectedValue.Trim();
            strID = HF_ID.Value;

            strHQL = "Select * From T_WZTurnDetail Where TurnCode = " + "'" + strTurnCode + "'" + " and IsMark <> -1";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurnDetail");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZBuNengYanShouYinCunZaiJieSua").ToString().Trim()+"')", true);
                return;
            }

            //strHQL = "Update T_WZTurnDetail Set Progress = 'Acceptance' Where TurnCode = " + "'" + strTurnCode + "'" + " and ID = " + strID;
            //ShareClass.RunSqlCommand(strHQL);

            //strHQL = "Select * From T_WZTurnDetail Where TurnCode = " + "'" + strTurnCode + "'" + " and Progress <> 'Acceptance'";
            // ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurnDetail");
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            strHQL = "Update T_WZTurn Set Progress = 'Acceptance' Where TurnCode = " + "'" + strTurnCode + "'";
            ShareClass.RunSqlCommand(strHQL);
            //}

            btnAccetance.Enabled = false;

            DataTurnDetailBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZYanShouChengGong").ToString().Trim()+"')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZYanShouShiBaiQingJianCha").ToString().Trim()+"')", true);
        }
    }

    protected string GetTurnProgress(string strTurnCode)
    {
        string strHQL;

        strHQL = "Select Progress From T_WZTurn Where TurnCode = " + "'" + strTurnCode + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurn");
        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    protected void btnFinish_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;
            string strTurnCode, strID;

            strID = HF_ID.Value;
            strTurnCode = LB_Turn.SelectedValue.Trim();

            if (GetTurnProgress(strTurnCode) != "Acceptance")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCiYiJiaoChanJinDuBuWeiYanSho").ToString().Trim()+"')", true);
                return;
            }

            //strHQL = "Update T_WZTurnDetail Set Progress = 'Completed' Where TurnCode = " + "'" + strTurnCode + "'" + " and ID = " + strID;
            //ShareClass.RunSqlCommand(strHQL);

            //strHQL = "Select * From T_WZTurnDetail Where TurnCode = " + "'" + strTurnCode + "'" + " and Progress <> 'Completed'";
            //DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurnDetail");
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            strHQL = "Update T_WZTurn Set Progress = 'Completed',FinishTime = now() Where TurnCode = " + "'" + strTurnCode + "'";
            ShareClass.RunSqlCommand(strHQL);
            //}

            btnFinish.Enabled = false;
            btnCheck.Enabled = true;

            DataTurnDetailBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZWanChengChengGong").ToString().Trim()+"')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZWanChengShiBaiQingJianCha").ToString().Trim()+"')", true);
        }
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;
            string strTurnCode, strID;

            strID = HF_ID.Value;
            strTurnCode = LB_Turn.SelectedValue.Trim();

            if (GetTurnProgress(strTurnCode) != "Completed")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCiYiJiaoChanJinDuBuWeiWanChe").ToString().Trim()+"')", true);
                return;
            }

            //strHQL = "Update T_WZTurnDetail Set Progress = '核销' Where TurnCode = " + "'" + strTurnCode + "'" + " and ID = " + strID;
            //ShareClass.RunSqlCommand(strHQL);

            //strHQL = "Select * From T_WZTurnDetail Where TurnCode = " + "'" + strTurnCode + "'" + " and Progress <> '核销'";
            //DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurnDetail");
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            strHQL = "Update T_WZTurn Set Progress = '核销' Where TurnCode = " + "'" + strTurnCode + "'"; 
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_WZTurnDetail Set Progress = '核销' Where TurnCode = " + "'" + strTurnCode + "'"; 
            ShareClass.RunSqlCommand(strHQL);

            //}

            btnCheck.Enabled = false;
            btnCancelCheck.Enabled = true;

            DataTurnDetailBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZHeXiaoChengGong").ToString().Trim()+"')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZHeXiaoShiBaiQingJianCha").ToString().Trim()+"')", true);
        }
    }

    protected void btnCancelCheck_Click(object sender, EventArgs e)
    {
        try
        {
            string strHQL;
            string strTurnCode, strID;

            strID = HF_ID.Value;
            strTurnCode = LB_Turn.SelectedValue.Trim();

            if (GetTurnProgress(strTurnCode) != LanguageHandle.GetWord("HeXiao").ToString().Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZCiYiJiaoChanJinDuBuWeiHeXiao").ToString().Trim()+"')", true);
                return;
            }

            //strHQL = "Update T_WZTurnDetail Set Progress = 'Completed' Where TurnCode = " + "'" + strTurnCode + "'" + " and ID = " + strID;
            //ShareClass.RunSqlCommand(strHQL);

            //strHQL = "Select * From T_WZTurnDetail Where TurnCode = " + "'" + strTurnCode + "'" + " and Progress = '核销'";
            //DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZTurnDetail");
            //if (ds.Tables[0].Rows.Count  == 0)
            //{
            strHQL = "Update T_WZTurn Set Progress = 'Completed' Where TurnCode = " + "'" + strTurnCode + "'";
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Update T_WZTurnDetail Set Progress = '领料' Where TurnCode = " + "'" + strTurnCode + "'"; 
            ShareClass.RunSqlCommand(strHQL);

            //}

            btnCancelCheck.Enabled = false;
            btnCheck.Enabled = true;

            DataTurnDetailBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZQuXiaoLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZQuXiaoLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
        }
    }

    protected void BT_Money_Click(object sender, EventArgs e)
    {
        //开票单价
        try
        {
            string strID = HF_ID.Value;
            if (string.IsNullOrEmpty(strID) || strID == "0")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYBJDYJDMX").ToString().Trim() + "')", true);
                return;
            }

            string strActualNumber = TXT_ActualNumber.Text.Trim();
            string strTicketPrice = TXT_TicketPrice.Text.Trim();

            if (!ShareClass.CheckIsNumber(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLSLBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strTicketPrice))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPDJBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }

            decimal decimalActualNumber = 0;
            decimal.TryParse(strActualNumber, out decimalActualNumber);
            decimal decimalTicketPrice = 0;
            decimal.TryParse(strTicketPrice, out decimalTicketPrice);

            decimal decimalResult = decimalActualNumber * decimalTicketPrice;
            TXT_TicketMoney.Text = decimalResult.ToString("#0.00");
        }
        catch (Exception ex)
        { }
    }


    protected void BT_Price_Click(object sender, EventArgs e)
    {
        try
        {
            string strID = HF_ID.Value;
            if (string.IsNullOrEmpty(strID) || strID == "0")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYBJDYJDMX").ToString().Trim() + "')", true);
                return;
            }

            string strActualNumber = TXT_ActualNumber.Text.Trim();
            string strTicketMoney = TXT_TicketMoney.Text.Trim();

            if (!ShareClass.CheckIsNumber(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLSLBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strTicketMoney))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPJEBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }

            decimal decimalActualNumber = 0;
            decimal.TryParse(strActualNumber, out decimalActualNumber);
            decimal decimalTicketMoney = 0;
            decimal.TryParse(strTicketMoney, out decimalTicketMoney);

            decimal decimalResult = 0;
            if (decimalActualNumber != 0)
            {
                decimalResult = decimalTicketMoney / decimalActualNumber;
            }
            TXT_TicketPrice.Text = decimalResult.ToString("#0.00");
        }
        catch (Exception ex)
        { }
    }

    protected void BT_Actual_Click(object sender, EventArgs e)
    {
        try
        {
            string strID = HF_ID.Value;
            if (string.IsNullOrEmpty(strID) || strID == "0")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYBJDYJDMX").ToString().Trim() + "')", true);
                return;
            }

            string strActualNumber = TXT_ActualNumber.Text.Trim();
            string strTicketPrice = TXT_TicketPrice.Text.Trim();

            if (!ShareClass.CheckIsNumber(strActualNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLSLBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strTicketPrice))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZKPDJBXWXSHZZS").ToString().Trim() + "')", true);
                return;
            }

            decimal decimalActualNumber = 0;
            decimal.TryParse(strActualNumber, out decimalActualNumber);
            decimal decimalTicketPrice = 0;
            decimal.TryParse(strTicketPrice, out decimalTicketPrice);

            decimal decimalResult = decimalActualNumber * decimalTicketPrice;
            TXT_TicketMoney.Text = decimalResult.ToString("#0.00");
        }
        catch (Exception ex)
        { }
    }


    protected void BT_NewDetailBrowse_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LB_Turn.SelectedValue))
        {
            string strTurnCode;

            strTurnCode = LB_Turn.SelectedValue.Trim();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZTurnDetailBrowse.aspx?TurnCode=" + strTurnCode + "');", true);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('TTWZTurnDetailBrowse.aspx?TurnCode=" + strTurnCode + "');", true);
          
            return;
        }
    }

    protected void DDL_PickingMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strPickingMethodValue = DDL_PickingMethod.SelectedValue;
        if (!string.IsNullOrEmpty(strPickingMethodValue))
        {
            try
            {
                decimal decimalActualNumber = 0;
                decimal.TryParse(TXT_ActualNumber.Text.Trim(), out decimalActualNumber);

                decimal decimalTicketNumber = 0;
                decimal.TryParse(TXT_TicketNumber.Text.Trim(), out decimalTicketNumber);

                if (strPickingMethodValue == LanguageHandle.GetWord("GongPiao").ToString().Trim())
                {
                    decimalActualNumber = decimalActualNumber > 0 ? -1 * decimalActualNumber : decimalActualNumber;
                    decimalTicketNumber = decimalTicketNumber > 0 ? -1 * decimalTicketNumber : decimalTicketNumber;

                    if (decimalActualNumber == 0)
                    {
                        TXT_ActualNumber.Text = "-0.00";
                    }
                    else
                    {
                        TXT_ActualNumber.Text = decimalActualNumber.ToString("#0.00");
                    }
                    TXT_ActualNumber.BackColor = Color.Red;

                    if (decimalTicketNumber == 0)
                    {
                        TXT_TicketNumber.Text = "-0.00";
                    }
                    else
                    {
                        TXT_TicketNumber.Text = decimalTicketNumber.ToString("#0.00");
                    }
                    TXT_TicketNumber.BackColor = Color.Red;
                }
                else if (strPickingMethodValue == LanguageHandle.GetWord("LanPiao").ToString().Trim())
                {
                    decimalActualNumber = decimalActualNumber > 0 ? decimalActualNumber : -1 * decimalActualNumber;
                    decimalTicketNumber = decimalTicketNumber > 0 ? decimalTicketNumber : -1 * decimalTicketNumber;

                    TXT_ActualNumber.Text = decimalActualNumber.ToString("#0.00");
                    TXT_ActualNumber.BackColor = Color.White;

                    TXT_TicketNumber.Text = decimalTicketNumber.ToString("#0.00");
                    TXT_TicketNumber.BackColor = Color.White;
                }
            }
            catch (Exception ex) { }
        }
    }


}
