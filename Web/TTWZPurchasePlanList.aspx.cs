using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWZPurchasePlanList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString() : "";

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "采购计划", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataBinder();

            TXT_SearchProjectCode.BackColor = Color.CornflowerBlue;
            TXT_SearchPurchaseName.BackColor = Color.CornflowerBlue;
        }
    }

    private void DataBinder()
    {
        DG_List.CurrentPageIndex = 0;

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                        e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName,
                        t.UserName as TenderCompetentName,
                        s.UserName as DecisionName,                    

                        e1.Name as ExpertCode1Name,
                        e2.Name as ExpertCode2Name,
                        e3.Name as ExpertCode3Name,

                        s1.SupplierName as SupplierCode1Name,
                        s2.SupplierName as SupplierCode2Name,
                        s3.SupplierName as SupplierCode3Name,
                        s4.SupplierName as SupplierCode4Name,
                        s5.SupplierName as SupplierCode5Name,
                        s6.SupplierName as SupplierCode6Name

                        from T_WZPurchase p


                        left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode

                        left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                        left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                        left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode

                        left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                        left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                        left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                        left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                        left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                        left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode

                        where (p.PurchaseEngineer = '{0}' or p.TenderCompetent = '{0}') ", strUserCode);
        //and p.Progress in ('提交','上报','Approved')", strUserCode);
        string strSearchProgress = DDL_SearchProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strSearchProgress))
        {
            strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        }
        string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchProjectCode))
        {
            strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        }
        string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchPurchaseName))
        {
            strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        }

        strPurchaseHQL += "order by p.MarkTime desc";
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;

        try
        {
            DG_List.DataBind();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }

    protected string ConvertPurchaseEndTimeToNull(string strPurchaseEndTime)
    {
        if (strPurchaseEndTime == "0001/1/1 0")
        {
            return "";
        }
        else
        {
            return strPurchaseEndTime;
        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        ControlStatusCloseChange();
    }

    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType != ListItemType.Pager)
            {
                string cmdName = e.CommandName;

                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string cmdArges = e.CommandArgument.ToString();
                WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
                string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + cmdArges + "'";
                IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
                if (listPurchase != null && listPurchase.Count == 1)
                {
                    WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

                    HL_NewScaling.NavigateUrl = "TTWZPurchaseScaling.aspx?PurchaseCode=" + cmdArges + "'";

                    if (cmdName == "click")
                    {
                        string strProgress = wZPurchase.Progress;
                        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ControlStatusChange('" + strProgress + "');", true);

                        ControlStatusChange((wZPurchase.TenderCompetent == null ? "" : wZPurchase.TenderCompetent.Trim()), (wZPurchase.PurchaseEngineer == null ? "" : wZPurchase.PurchaseEngineer.Trim()), strProgress, wZPurchase.IsMark.ToString(), wZPurchase.PlanMoney, wZPurchase.PurchaseEndTime);                      

                        HF_NewPurchaseCode.Value = wZPurchase.PurchaseCode;
                        HF_TenderCompetent.Value = wZPurchase.TenderCompetent;
                        HF_PurchaseEngineer.Value = wZPurchase.PurchaseEngineer;
                        HF_Progress.Value = strProgress;
                        HF_IsMark.Value = wZPurchase.IsMark.ToString();
                        HF_PlanMoney.Value = wZPurchase.PlanMoney.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYCYCYYSEXMESSAGE").ToString().Trim() + "')", true);
        }
    }

    protected void BT_NewSetVolume_Click(object sender, EventArgs e)
    {
        //组卷
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchasePlanListVolume.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
        //return;
    }


    protected void BT_NewReport_Click(object sender, EventArgs e)
    {
        //上报
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            //            ② 采购工程师上报												
            //              点击【上报】按钮，写记录：  采购文件〈进度〉＝“上报”												
            //            ③ 招标主管上报												
            //              点击【上报】按钮，弹出上报对话框，选择录入“上级领导”												
            //              点击“保存”后，写记录：  采购文件〈进度〉＝“上报”												
            //              点击“取消”后，关闭上报对话框，退出上报程序												
            if (wZPurchase.PurchaseEngineer.Trim() == strUserCode)
            {
                wZPurchase.Progress = LanguageHandle.GetWord("ShangBao").ToString().Trim();

                wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCG").ToString().Trim() + "');", true);
            }
            else if (wZPurchase.TenderCompetent.Trim() == strUserCode)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "ClickReport('" + wZPurchase.PurchaseCode + "')", true);

            }
        }
    }


    protected void BT_NewReportReturn_Click(object sender, EventArgs e)
    {
        //上报退回
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = LanguageHandle.GetWord("DiJiao").ToString().Trim();
            wZPurchase.UpLeader = "";

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            //重新加载列表
            DataBinder();

            ControlStatusCloseChange();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('上报退回成功！');", true); 
        }
    }


    protected void BT_NewEnquiry_Click(object sender, EventArgs e)
    {
        //询价
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        //① 询价检查：												
        //     采购文件〈报价开始〉≥“系统日期”												
        //     未通过检查，提示并弹出询价对话框，对〈报价开始〉〈报价截止〉字段进行修改												
        //         点击保存后再次检查，通过后进行下一步												
        //         点击取消，退出提交询价程序												
        //     通过检查，进行下一步												

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            DateTime dtPurchaseStartTime = DateTime.Now;
            DateTime.TryParse(wZPurchase.PurchaseStartTime, out dtPurchaseStartTime);
            if (dtPurchaseStartTime < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                //弹出对话框，去修改报价开始时间
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchasePlanListApplyTime.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
                return;
            }
            else
            {
                //通过检查，进行下一步

                //先删除
                string strDeletePurchaseOfferRecordSQL = string.Format(@"delete T_WZPurchaseOfferRecord where PurchaseCode = '{0}'", strEditPurchaseCode);
                ShareClass.RunSqlCommand(strDeletePurchaseOfferRecordSQL);

                string strSelectSupplierSQL = string.Format(@"select * from T_WZPurchaseSupplier
                            where PurchaseCode = '{0}'", strEditPurchaseCode);
                DataTable dtSelectSupplier = ShareClass.GetDataSetFromSql(strSelectSupplierSQL, "SelectSupplier").Tables[0];
                if (dtSelectSupplier != null && dtSelectSupplier.Rows.Count > 0)
                {
                    string strSelectPurchaseDetailSQL = string.Format(@"select d.*,o.ObjectName,o.Model,o.Criterion,o.Grade,o.Unit,s.UnitName from T_WZPurchaseDetail d
                            left join T_WZObject o on d.ObjectCode = o.ObjectCode
                            left join T_WZSpan s on o.Unit = s.ID
                            where PurchaseCode = '{0}'", strEditPurchaseCode);
                    DataTable dtSelectPurchaseDetail = ShareClass.GetDataSetFromSql(strSelectPurchaseDetailSQL, "strSelectPurchaseDetailSQL").Tables[0];
                    if (dtSelectPurchaseDetail != null && dtSelectPurchaseDetail.Rows.Count > 0)
                    {
                        string strInsertPurchaseOfferRecordSQL = string.Empty;
                        foreach (DataRow drSupplier in dtSelectSupplier.Rows)
                        {
                            foreach (DataRow drPurchaseDetail in dtSelectPurchaseDetail.Rows)
                            {
                                int intUnit = 0;
                                int.TryParse(ShareClass.ObjectToString(drPurchaseDetail["Unit"]), out intUnit);
                                decimal decimalPurchaseNumber = 0;
                                decimal.TryParse(ShareClass.ObjectToString(drPurchaseDetail["PurchaseNumber"]), out decimalPurchaseNumber);

                                strInsertPurchaseOfferRecordSQL += string.Format(@"INSERT INTO T_WZPurchaseOfferRecord
                                                   (PurchaseCode,PlanDetailID,PurchaseDetailID,SupplierCode
                                                   ,Tenders,SerialNumber,ObjectCode,ObjectName,Model
                                                   ,Criterion,Grade,Unit,PurchaseNumber,ApplyMoney
                                                   ,TotalMoney,ReplaceCode,ScalingResult,Progress)
                                             VALUES
                                                   ('{0}'
                                                   ,{1}
                                                   ,{2}
                                                   ,'{3}'
                                                   ,'{4}'
                                                   ,'{5}'
                                                   ,'{6}'
                                                   ,'{7}'
                                                   ,'{8}'
                                                   ,'{9}'
                                                   ,'{10}'
                                                   ,{11}
                                                   ,{12}
                                                   ,{13}
                                                   ,{14}
                                                   ,'{15}'
                                                   ,'{16}'
                                                   ,'{17}');", strEditPurchaseCode,
                                                             ShareClass.ObjectToString(drPurchaseDetail["PlanDetailID"]),
                                                            ShareClass.ObjectToString(drPurchaseDetail["ID"]),
                                                            ShareClass.ObjectToString(drSupplier["SupplierCode"]),
                                                            ShareClass.ObjectToString(drPurchaseDetail["Tenders"]),
                                                            ShareClass.ObjectToString(drPurchaseDetail["SerialNumber"]),
                                                            ShareClass.ObjectToString(drPurchaseDetail["SerialNumber"]),
                                                            ShareClass.ObjectToString(drPurchaseDetail["ObjectName"]),
                                                            ShareClass.ObjectToString(drPurchaseDetail["Model"]),
                                                            ShareClass.ObjectToString(drPurchaseDetail["Criterion"]),
                                                            ShareClass.ObjectToString(drPurchaseDetail["Grade"]),
                                                            intUnit,
                                                            decimalPurchaseNumber,
                                                            0, 0, "", "", LanguageHandle.GetWord("XunJia").ToString().Trim());
                            }
                        }


                        wZPurchase.Progress = LanguageHandle.GetWord("XunJia").ToString().Trim();
                        wZPurchaseBLL.UpdateWZPurchase(wZPurchase, strEditPurchaseCode);

                        string strUpdatePurchaseDetailSQL = string.Format(@"update T_WZPurchaseDetail
                            set Progress = '询价'
                            where PurchaseCode = '{0}'", strEditPurchaseCode); 
                        ShareClass.RunSqlCommand(strUpdatePurchaseDetailSQL);

                        string strUpdatePlanDetailSQL = string.Format(@"update T_WZPickingPlanDetail
                            set Progress = '询价'
                            where PurchaseCode = '{0}'", strEditPurchaseCode); 
                        ShareClass.RunSqlCommand(strUpdatePlanDetailSQL);

                        ShareClass.RunSqlCommand(strInsertPurchaseOfferRecordSQL);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('当前采购文件未选择采购清单！');", true); 
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('当前采购文件未选择供应商！');", true); 
                    return;
                }
                //                ② 写记录：												
                //    采购文件〈进度〉＝“询价”												
                //    采购清单〈进度〉＝“询价”												
                //    计划明细〈进度〉＝“询价”												
                //③ 生成报价单												
                //    写记录：												
                //       报价单〈采购编号〉＝采购文件〈采购编号〉												
                //       报价单〈计划编号〉＝采购清单〈计划编号〉												
                //       报价单〈供方编号〉＝采购文件〈供方编号 1 〉												
                //       报价单〈标段〉＝采购清单〈标段〉												
                //       报价单〈序号〉＝采购清单〈序号〉												
                //       报价单〈物资代码〉＝采购清单〈物资代码〉												
                //       报价单〈物资名称〉＝物资代码〈物资名称〉												
                //       报价单〈规格型号〉＝物资代码〈规格型号〉												
                //       报价单〈标准〉＝物资代码〈标准〉												
                //       报价单〈级别〉＝物资代码〈级别〉												
                //       报价单〈计量单位〉＝物资代码〈计量单位〉												
                //       报价单〈采购数量〉＝采购清单〈采购数量〉												
                //       报价单〈报价〉＝“空”												
                //       报价单〈报价合计〉＝“空”												
                //       报价单〈替代型号〉＝“空”												
                //       报价单〈定标结果〉＝“空”												
                //       报价单〈进度〉＝“询价”												
                //    循环操作至最后一个采购文件〈供方编号 N 〉结束												

            }

            //重新加载列表
            DataBinder();


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('询价成功！');", true); 

        }
    }


    protected void BT_NewEnquiryReturn_Click(object sender, EventArgs e)
    {
        //询价退回
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = "Approved";
            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, strEditPurchaseCode);

            string strUpdatePurchaseDetailSQL = string.Format(@"update T_WZPurchaseDetail
                            set Progress = '录入'
                            where PurchaseCode = '{0}'", strEditPurchaseCode); 
            ShareClass.RunSqlCommand(strUpdatePurchaseDetailSQL);

            string strUpdatePlanDetailSQL = string.Format(@"update T_WZPickingPlanDetail
                            set Progress = '录入'
                            where PurchaseCode = '{0}'", strEditPurchaseCode); 
            ShareClass.RunSqlCommand(strUpdatePlanDetailSQL);

            //先删除
            string strDeletePurchaseOfferRecordSQL = string.Format(@"delete T_WZPurchaseOfferRecord where PurchaseCode = '{0}'", strEditPurchaseCode);
            ShareClass.RunSqlCommand(strDeletePurchaseOfferRecordSQL);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('询价退回成功！');", true); 
        }
    }


    protected void BT_NewAssessment_Click(object sender, EventArgs e)
    {
        //评标
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        //采购文件〈进度〉＝“评标”												
        //报价单〈进度〉＝“评标”  （逐条写入）												
        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = LanguageHandle.GetWord("PingBiao").ToString().Trim();
            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, strEditPurchaseCode);

            string strUpdatePurchaseOfferRecordSQL = string.Format(@"update T_WZPurchaseOfferRecord
                            set Progress = '评标'
                            where PurchaseCode = '{0}'", strEditPurchaseCode); 
            ShareClass.RunSqlCommand(strUpdatePurchaseOfferRecordSQL);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZPBCG").ToString().Trim() + "')", true);
        }
    }



    protected void BT_NewDecisionRecord_Click(object sender, EventArgs e)
    {
        //决策记录
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchasePlanListDecisionRecord.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
    }


    protected void BT_NewApproval_Click(object sender, EventArgs e)
    {
        //报批
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }
        //② 点击【报批】按钮，写记录：												
        //采购文件〈进度〉＝“报批”												
        //报价单〈进度〉＝“报批”												
        //评标记录〈进度〉＝“报批”												
        //决策记录〈进度〉＝“报批”												

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = LanguageHandle.GetWord("BaoPi").ToString().Trim();
            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, strEditPurchaseCode);

            //报价单
            string strUpdatePurchaseOfferRecordSQL = string.Format(@"update T_WZPurchaseOfferRecord
                        set Progress = '报批'
                        where PurchaseCode = '{0}'", strEditPurchaseCode); 
            ShareClass.RunSqlCommand(strUpdatePurchaseOfferRecordSQL);


            //重新加载列表
            DataBinder();

            BT_NewApproval.Enabled = false;
            BT_NewApprovalReturn.Enabled = true;


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBPCG").ToString().Trim() + "')", true);
        }
    }

    protected void BT_NewApprovalReturn_Click(object sender, EventArgs e)
    {
        //报批退回
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        //② 点击【报批退回】按钮，写记录：												
        //采购文件〈进度〉＝“评标”												
        //报价单〈进度〉＝“评标”												
        //评标记录〈进度〉＝“评标”												
        //决策记录〈进度〉＝“录入”												
        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strEditPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = LanguageHandle.GetWord("PingBiao").ToString().Trim();
            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, strEditPurchaseCode);

            //报价单
            string strUpdatePurchaseOfferRecordSQL = string.Format(@"update T_WZPurchaseOfferRecord
                        set Progress = '评标'
                        where PurchaseCode = '{0}'", strEditPurchaseCode); 
            ShareClass.RunSqlCommand(strUpdatePurchaseOfferRecordSQL);

            //重新加载列表
            DataBinder();

            BT_NewApproval.Enabled = true;
            BT_NewApprovalReturn.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBPTHCG").ToString().Trim() + "')", true);
        }
    }

    protected void BT_NewScaling_Click(object sender, EventArgs e)
    {
        //定标
        string strEditPurchaseCode = HF_NewPurchaseCode.Value;
        if (string.IsNullOrEmpty(strEditPurchaseCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXDJYCZDCGLB").ToString().Trim() + "')", true);
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTWZPurchaseScaling.aspx?PurchaseCode=" + strEditPurchaseCode + "');", true);
    }

    protected void DDL_SearchProgress_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();
    }

    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();
    }

    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();

    }





    protected void BT_SortPurchaseCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        //string strPurchaseHQL = string.Format(@"select p.*,
        //                e.UserName as PurchaseEngineerName,
        //                u.UserName as UpLeaderName,
        //                m.UserName as PurchaseManagerName,
        //                d.UserName as DisciplinarySupervisionName,
        //                c.UserName as ControlMoneyName,
        //                t.UserName as TenderCompetentName,
        //                s.UserName as DecisionName
        //                from T_WZPurchase p
        //                left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
        //                left join T_ProjectMember u on p.UpLeader = u.UserCode
        //                left join T_ProjectMember m on p.PurchaseManager = m.UserCode
        //                left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
        //                left join T_ProjectMember c on p.ControlMoney = c.UserCode
        //                left join T_ProjectMember t on p.TenderCompetent = t.UserCode
        //                left join T_ProjectMember s on p.Decision = s.UserCode
        //                where (p.PurchaseEngineer = '{0}' or p.TenderCompetent = '{0}')
        //                and p.Progress in ('提交','上报','Approved')", strUserCode); 
        //string strSearchProgress = DDL_SearchProgress.SelectedValue;
        //if (!string.IsNullOrEmpty(strSearchProgress))
        //{
        //    strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        //}
        //string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchProjectCode))
        //{
        //    strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        //}
        //string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchPurchaseName))
        //{
        //    strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        //}

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                        e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName,
                        t.UserName as TenderCompetentName,
                        s.UserName as DecisionName,

                        e1.Name as ExpertCode1Name,
                        e2.Name as ExpertCode2Name,
                        e3.Name as ExpertCode3Name,

                        s1.SupplierName as SupplierCode1Name,
                        s2.SupplierName as SupplierCode2Name,
                        s3.SupplierName as SupplierCode3Name,
                        s4.SupplierName as SupplierCode4Name,
                        s5.SupplierName as SupplierCode5Name,
                        s6.SupplierName as SupplierCode6Name

                        from T_WZPurchase p


                        left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode

                        left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                        left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                        left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode

                        left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                        left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                        left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                        left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                        left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                        left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode

                        where (p.PurchaseEngineer = '{0}' or p.TenderCompetent = '{0}') ", strUserCode);
        //and p.Progress in ('提交','上报','Approved')", strUserCode);
        string strSearchProgress = DDL_SearchProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strSearchProgress))
        {
            strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        }
        string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchProjectCode))
        {
            strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        }
        string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchPurchaseName))
        {
            strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortPurchaseCode.Value))
        {
            strPurchaseHQL += " order by p.PurchaseCode desc";

            HF_SortPurchaseCode.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by p.PurchaseCode asc";

            HF_SortPurchaseCode.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }



    protected void BT_SortProjectCode_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        //string strPurchaseHQL = string.Format(@"select p.*,
        //                e.UserName as PurchaseEngineerName,
        //                u.UserName as UpLeaderName,
        //                m.UserName as PurchaseManagerName,
        //                d.UserName as DisciplinarySupervisionName,
        //                c.UserName as ControlMoneyName,
        //                t.UserName as TenderCompetentName,
        //                s.UserName as DecisionName
        //                from T_WZPurchase p
        //                left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
        //                left join T_ProjectMember u on p.UpLeader = u.UserCode
        //                left join T_ProjectMember m on p.PurchaseManager = m.UserCode
        //                left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
        //                left join T_ProjectMember c on p.ControlMoney = c.UserCode
        //                left join T_ProjectMember t on p.TenderCompetent = t.UserCode
        //                left join T_ProjectMember s on p.Decision = s.UserCode
        //                where (p.PurchaseEngineer = '{0}' or p.TenderCompetent = '{0}')
        //                and p.Progress in ('提交','上报','Approved')", strUserCode); 
        //string strSearchProgress = DDL_SearchProgress.SelectedValue;
        //if (!string.IsNullOrEmpty(strSearchProgress))
        //{
        //    strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        //}
        //string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchProjectCode))
        //{
        //    strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        //}
        //string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchPurchaseName))
        //{
        //    strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        //}

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                        e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName,
                        t.UserName as TenderCompetentName,
                        s.UserName as DecisionName,

                        e1.Name as ExpertCode1Name,
                        e2.Name as ExpertCode2Name,
                        e3.Name as ExpertCode3Name,

                        s1.SupplierName as SupplierCode1Name,
                        s2.SupplierName as SupplierCode2Name,
                        s3.SupplierName as SupplierCode3Name,
                        s4.SupplierName as SupplierCode4Name,
                        s5.SupplierName as SupplierCode5Name,
                        s6.SupplierName as SupplierCode6Name

                        from T_WZPurchase p


                        left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode

                        left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                        left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                        left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode

                        left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                        left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                        left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                        left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                        left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                        left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode

                        where (p.PurchaseEngineer = '{0}' or p.TenderCompetent = '{0}') ", strUserCode);
        //and p.Progress in ('提交','上报','Approved')", strUserCode);
        string strSearchProgress = DDL_SearchProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strSearchProgress))
        {
            strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        }
        string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchProjectCode))
        {
            strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        }
        string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchPurchaseName))
        {
            strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortProjectCode.Value))
        {
            strPurchaseHQL += " order by p.ProjectCode desc";

            HF_SortProjectCode.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by p.ProjectCode asc";

            HF_SortProjectCode.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }

    protected void BT_SortPurchaseStartTime_Click(object sender, EventArgs e)
    {
        DG_List.CurrentPageIndex = 0;

        //string strPurchaseHQL = string.Format(@"select p.*,
        //                e.UserName as PurchaseEngineerName,
        //                u.UserName as UpLeaderName,
        //                m.UserName as PurchaseManagerName,
        //                d.UserName as DisciplinarySupervisionName,
        //                c.UserName as ControlMoneyName,
        //                t.UserName as TenderCompetentName,
        //                s.UserName as DecisionName
        //                from T_WZPurchase p
        //                left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
        //                left join T_ProjectMember u on p.UpLeader = u.UserCode
        //                left join T_ProjectMember m on p.PurchaseManager = m.UserCode
        //                left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
        //                left join T_ProjectMember c on p.ControlMoney = c.UserCode
        //                left join T_ProjectMember t on p.TenderCompetent = t.UserCode
        //                left join T_ProjectMember s on p.Decision = s.UserCode
        //                where (p.PurchaseEngineer = '{0}' or p.TenderCompetent = '{0}')
        //                and p.Progress in ('提交','上报','Approved')", strUserCode); 
        //string strSearchProgress = DDL_SearchProgress.SelectedValue;
        //if (!string.IsNullOrEmpty(strSearchProgress))
        //{
        //    strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        //}
        //string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchProjectCode))
        //{
        //    strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        //}
        //string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        //if (!string.IsNullOrEmpty(strSearchPurchaseName))
        //{
        //    strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        //}

        string strPurchaseHQL = string.Format(@"select distinct p.*,
                        e.UserName as PurchaseEngineerName,
                        u.UserName as UpLeaderName,
                        m.UserName as PurchaseManagerName,
                        d.UserName as DisciplinarySupervisionName,
                        c.UserName as ControlMoneyName,
                        t.UserName as TenderCompetentName,
                        s.UserName as DecisionName,

                        e1.Name as ExpertCode1Name,
                        e2.Name as ExpertCode2Name,
                        e3.Name as ExpertCode3Name,

                        s1.SupplierName as SupplierCode1Name,
                        s2.SupplierName as SupplierCode2Name,
                        s3.SupplierName as SupplierCode3Name,
                        s4.SupplierName as SupplierCode4Name,
                        s5.SupplierName as SupplierCode5Name,
                        s6.SupplierName as SupplierCode6Name

                        from T_WZPurchase p


                        left join T_ProjectMember e on p.PurchaseEngineer = e.UserCode
                        left join T_ProjectMember u on p.UpLeader = u.UserCode
                        left join T_ProjectMember m on p.PurchaseManager = m.UserCode
                        left join T_ProjectMember d on p.DisciplinarySupervision = d.UserCode
                        left join T_ProjectMember c on p.ControlMoney = c.UserCode
                        left join T_ProjectMember t on p.TenderCompetent = t.UserCode
                        left join T_ProjectMember s on p.Decision = s.UserCode

                        left join T_WZExpert e1 on p.ExpertCode1 = e1.ExpertCode
                        left join T_WZExpert e2 on p.ExpertCode2 = e2.ExpertCode
                        left join T_WZExpert e3 on p.ExpertCode3 = e3.ExpertCode

                        left join T_WZSupplier s1 on p.SupplierCode1 = s1.SupplierCode
                        left join T_WZSupplier s2 on p.SupplierCode2 = s2.SupplierCode
                        left join T_WZSupplier s3 on p.SupplierCode3 = s3.SupplierCode
                        left join T_WZSupplier s4 on p.SupplierCode4 = s4.SupplierCode
                        left join T_WZSupplier s5 on p.SupplierCode5 = s5.SupplierCode
                        left join T_WZSupplier s6 on p.SupplierCode6 = s6.SupplierCode

                        where (p.PurchaseEngineer = '{0}' or p.TenderCompetent = '{0}') ", strUserCode);
        //and p.Progress in ('提交','上报','Approved')", strUserCode);
        string strSearchProgress = DDL_SearchProgress.SelectedValue;
        if (!string.IsNullOrEmpty(strSearchProgress))
        {
            strPurchaseHQL += " and p.Progress = '" + strSearchProgress + "'";
        }
        string strSearchProjectCode = TXT_SearchProjectCode.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchProjectCode))
        {
            strPurchaseHQL += " and p.ProjectCode like '%" + strSearchProjectCode + "%'";
        }
        string strSearchPurchaseName = TXT_SearchPurchaseName.Text.Trim();
        if (!string.IsNullOrEmpty(strSearchPurchaseName))
        {
            strPurchaseHQL += " and p.PurchaseName like '%" + strSearchPurchaseName + "%'";
        }

        if (!string.IsNullOrEmpty(HF_SortPurchaseStartTime.Value))
        {
            strPurchaseHQL += " order by p.PurchaseStartTime desc";

            HF_SortPurchaseStartTime.Value = "";
        }
        else
        {
            strPurchaseHQL += " order by p.PurchaseStartTime asc";

            HF_SortPurchaseStartTime.Value = "asc";
        }

        DataTable dtPurchase = ShareClass.GetDataSetFromSql(strPurchaseHQL, "Purchase").Tables[0];

        DG_List.DataSource = dtPurchase;
        DG_List.DataBind();

        LB_Sql.Text = strPurchaseHQL;

        LB_RecordCount.Text = dtPurchase.Rows.Count.ToString();

        ControlStatusCloseChange();
    }

    private void ControlStatusChange(string objTenderCompetent, string objPurchaseEngineer, string objProgress, string objIsMark, decimal objPlanMoney, string strPurchaseEndTime)
    {

        if ((objTenderCompetent == strUserCode && objProgress == LanguageHandle.GetWord("DiJiao").ToString().Trim() && objPlanMoney >= 300000) || (objPurchaseEngineer == strUserCode && objProgress == LanguageHandle.GetWord("DiJiao").ToString().Trim() && objPlanMoney < 300000))
        {
            BT_NewSetVolume.Enabled = true;                   //组卷
            BT_NewReport.Enabled = true;                   //上报

            BT_NewReportReturn.Enabled = false;                   //上报退回

        }
        else if ((objTenderCompetent == strUserCode && objProgress == LanguageHandle.GetWord("ShangBao").ToString().Trim() && objPlanMoney >= 300000) || (objPurchaseEngineer == strUserCode && objProgress == LanguageHandle.GetWord("ShangBao").ToString().Trim() && objPlanMoney < 300000))
        {
            BT_NewSetVolume.Enabled = false;                   //组卷
            BT_NewReport.Enabled = false;                    //上报
            BT_NewReportReturn.Enabled = true;                   //上报退回
        }
        else
        {
            BT_NewSetVolume.Enabled = false;                   //组卷
            BT_NewReport.Enabled = false;                 //上报
            BT_NewReportReturn.Enabled = false;                  //上报退回
        }

        if ((objTenderCompetent == strUserCode || objPurchaseEngineer == strUserCode) && objProgress == "Approved")
        {
            BT_NewEnquiry.Enabled = true;                    //询价
            BT_NewEnquiryReturn.Enabled = false;             //询价退回
        }
        else if ((objTenderCompetent == strUserCode || objPurchaseEngineer == strUserCode) && objProgress == LanguageHandle.GetWord("XunJia").ToString().Trim())
        {
            BT_NewEnquiry.Enabled = false;                    //询价
            BT_NewEnquiryReturn.Enabled = true;              //询价退回
        }
        else
        {
            BT_NewEnquiry.Enabled = false;                    //询价
            BT_NewEnquiryReturn.Enabled = false;             //询价退回
        }


        DateTime dtPurchaseEndTime = DateTime.Now;
        DateTime.TryParse(strPurchaseEndTime, out dtPurchaseEndTime);
        if ((objTenderCompetent == strUserCode || objPurchaseEngineer == strUserCode) && objProgress == LanguageHandle.GetWord("XunJia").ToString().Trim() && dtPurchaseEndTime <= DateTime.Now)
        {
            BT_NewAssessment.Enabled = true;                   //评标
        }
        else
        {
            BT_NewAssessment.Enabled = false;                    //评标
        }

        if ((objTenderCompetent == strUserCode || objPurchaseEngineer == strUserCode) && objProgress == LanguageHandle.GetWord("PingBiao").ToString().Trim())
        {
            BT_NewDecisionRecord.Enabled = true;                    //决策记录
            BT_NewApproval.Enabled = true;
        }
        else
        {
            BT_NewDecisionRecord.Enabled = false;                    //决策记录
            BT_NewApproval.Enabled = false;
        }

        if ((objTenderCompetent == strUserCode || objPurchaseEngineer == strUserCode) && objProgress == LanguageHandle.GetWord("BaoPi").ToString().Trim())
        {
            BT_NewApproval.Enabled = false;
            BT_NewApprovalReturn.Enabled = true;
            BT_NewScaling.Enabled = true;
        }

        if (objProgress == LanguageHandle.GetWord("JueCe").ToString().Trim())
        {
            BT_NewScaling.Enabled = true;
            HL_NewScaling.Enabled = true;
        }
    }



    private void ControlStatusCloseChange()
    {
        BT_NewSetVolume.Enabled = false;

        BT_NewReport.Enabled = false;
        BT_NewReportReturn.Enabled = false;
        BT_NewEnquiry.Enabled = false;
        BT_NewEnquiryReturn.Enabled = false;
        BT_NewAssessment.Enabled = false;
        BT_NewDecisionRecord.Enabled = false;
        BT_NewApproval.Enabled = false;
        BT_NewApprovalReturn.Enabled = false;
        BT_NewScaling.Enabled = false;
        //HL_NewScaling.Enabled = false;
    }



    protected void BT_Report_Click(object sender, EventArgs e)
    {
        string strPurchaseCode = HF_UpLoaderPurchaseCode.Value;
        string strUpLeader = HF_UpLoaderCodeName.Value;

        string[] arrUpLeader = strUpLeader.Split('|');

        WZPurchaseBLL wZPurchaseBLL = new WZPurchaseBLL();
        string strPurchaseSql = "from WZPurchase as wZPurchase where PurchaseCode = '" + strPurchaseCode + "'";
        IList listPurchase = wZPurchaseBLL.GetAllWZPurchases(strPurchaseSql);
        if (listPurchase != null && listPurchase.Count == 1)
        {
            WZPurchase wZPurchase = (WZPurchase)listPurchase[0];

            wZPurchase.Progress = LanguageHandle.GetWord("ShangBao").ToString().Trim();
            wZPurchase.PurchaseStartTime = "-";
            wZPurchase.UpLeader = arrUpLeader[0];

            wZPurchaseBLL.UpdateWZPurchase(wZPurchase, wZPurchase.PurchaseCode);

            //重新加载列表
            DataBinder();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCG").ToString().Trim() + "')", true);
        }
    }



}
