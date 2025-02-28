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

using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

using TakeTopCore;

public partial class TTGoodsMRPExtend : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserName;

        strUserCode = Session["UserCode"].ToString();

        LB_UserCode.Text = strUserCode;
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "物控管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthorityAsset(strUserCode);
            LB_DepartString.Text = strDepartString;

            LoadItemMainPlan(strUserCode);

            ShareClass.LoadWFTemplate(strUserCode, "PlanApproval", DL_TemName);

            BT_MRPExtend.Attributes.Add("onclick", "if(!confirm('展开运算会覆盖前次运算的数据，您确定要进行吗？')) return false;");   //ChineseWord
        }
    }

    protected void DataGrid5_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strMRPVerType, strPlanMRPVerID;

            string strPlanVerID;

            strPlanVerID = ((Button)e.Item.FindControl("BT_PlanVerID")).Text.Trim();
            LB_PlanVerID.Text = strPlanVerID;
            LB_PlanVerName.Text = e.Item.Cells[1].Text;


            for (int i = 0; i < DataGrid5.Items.Count; i++)
            {
                DataGrid5.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;


            LoadItemMainPlanDetail(strPlanVerID);

            strHQL = "Select ID,PlanMRPVerID,Type,ExpendType,OnOrder,OnProduction,OnLine,relatedprojectid,relatedprojectplanverid From T_ItemMainPlanMRPVersion Where PlanVerID = " + strPlanVerID;
            strHQL += " Order By PlanVerID ASC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanMRPVersion");
            DL_PlanMRPVersionID.DataSource = ds;
            DL_PlanMRPVersionID.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text;
                DL_ExpendType.SelectedValue = ds.Tables[0].Rows[0][3].ToString().Trim();

                if (strPlanMRPVerID != "")
                {
                    LoadItemMainPlanRelatedItemPurchasePlan(strPlanVerID, strPlanMRPVerID);
                    LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID);
                    LoadItemMainPlanRelatedItemOutSourcingPlan(strPlanVerID, strPlanMRPVerID);

                    TB_WLName.Text = LanguageHandle.GetWord("JiHua").ToString().Trim() + LB_PlanVerName.Text.Trim() + LanguageHandle.GetWord("BanBen").ToString().Trim() + strPlanMRPVerID + LanguageHandle.GetWord("ShenQing").ToString().Trim();
                    ShareClass.LoadRelatedWL("PlanApproval", "Plan", int.Parse(strPlanMRPVerID), DataGrid8);
                    BT_SubmitApply.Enabled = true;

                    HL_PurchasePlanView.NavigateUrl = "TTGoodsMRPPurchaseRequirementPlanView.aspx?PlanVerID=" + strPlanVerID + "&PlanMRPVerID=" + strPlanMRPVerID;
                    HL_ProductionPlanView.NavigateUrl = "TTGoodsMRPProductionRequirementPlanView.aspx?PlanVerID=" + strPlanVerID + "&PlanMRPVerID=" + strPlanMRPVerID;
                    HL_OutsourcePlanView.NavigateUrl = "TTGoodsMRPOutsourceRequirementPlanView.aspx?PlanVerID=" + strPlanVerID + "&PlanMRPVerID=" + strPlanMRPVerID;
                }
                else
                {
                    LoadItemMainPlanRelatedItemPurchasePlan(strPlanVerID, "0");
                    LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, "0");
                    LoadItemMainPlanRelatedItemOutSourcingPlan(strPlanVerID, "0");

                    BT_SubmitApply.Enabled = false;
                }

                strMRPVerType = ds.Tables[0].Rows[0][2].ToString().Trim();
                DL_VersionType.SelectedValue = strMRPVerType;

                if (ds.Tables[0].Rows[0][4].ToString().Trim() == "YES")
                {
                    CB_OnOrder.Checked = true;
                }
                else
                {
                    CB_OnOrder.Checked = false;
                }
                if (ds.Tables[0].Rows[0][5].ToString().Trim() == "YES")
                {
                    CB_OnProduction.Checked = true;
                }
                else
                {
                    CB_OnProduction.Checked = false;
                }
                if (ds.Tables[0].Rows[0][6].ToString().Trim() == "YES")
                {
                    CB_OnLine.Checked = true;
                }
                else
                {
                    CB_OnLine.Checked = false;
                }

                HL_MainPlanMateriaDetailReport.NavigateUrl = "TTGoodsMainPlanMaterialDetailDataReport.aspx?PlanVerID=" + strPlanVerID + "&MRPVerID=" + strPlanMRPVerID;


                HL_TranferToProjectplan.Enabled = true;
                HL_TranferToProjectplan.NavigateUrl = "TTProjectMRPPlanToProjectPlan.aspx?MainPlanVerID=" + strPlanVerID + "&MRPPlanVerID=" + strPlanMRPVerID;

                string strRelatedProjectID, strRelatedProjectPlanVerID;
                strRelatedProjectID = ds.Tables[0].Rows[0]["RelatedProjectID"].ToString();
                strRelatedProjectPlanVerID = ds.Tables[0].Rows[0]["RelatedProjectPlanVerID"].ToString();

                if (strRelatedProjectID != "0" && strRelatedProjectPlanVerID != "0")
                {
                    HL_RelatedProjectPlanGantt.Enabled = true;
                    HL_RelatedProjectPlanGantt.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strRelatedProjectID + "&VerID=" + strRelatedProjectPlanVerID;
                }
                else
                {
                    HL_RelatedProjectPlanGantt.Enabled = false;
                    HL_RelatedProjectPlanGantt.NavigateUrl = "";
                }

            }
            else
            {
                LoadItemMainPlanRelatedItemPurchasePlan("0", "0");
                LoadItemMainPlanRelatedItemProductPlan("0", "0");
                LoadItemMainPlanRelatedItemOutSourcingPlan("0", "0");

                CB_OnOrder.Checked = false;
                CB_OnProduction.Checked = false;
                CB_OnLine.Checked = false;

                BT_SubmitApply.Enabled = false;
            }
        }

    }

    protected void DL_PlanMRPVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanVerID, strID, strPlanMRPVerID, strMRPVerType, strExpendType;
        string strRelatedProjectID = "0", strRelatedProjectPlanVerID = "0";

        strPlanVerID = LB_PlanVerID.Text.Trim();
        strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text;
        strID = DL_PlanMRPVersionID.SelectedValue;

        strHQL = "Select Type,ExpendType,OnOrder,OnProduction,OnLine,RelatedProjectID,RelatedProjectPlanVerID From T_ItemMainPlanMRPVersion Where ID = " + strID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanMRPVersion");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strMRPVerType = ds.Tables[0].Rows[0][0].ToString().Trim();
            DL_VersionType.SelectedValue = strMRPVerType;

            strExpendType = ds.Tables[0].Rows[0][1].ToString().Trim();
            DL_ExpendType.SelectedValue = strExpendType;

            strRelatedProjectID = ds.Tables[0].Rows[0]["RelatedProjectID"].ToString().Trim();
            strRelatedProjectPlanVerID = ds.Tables[0].Rows[0]["RelatedProjectPlanVerID"].ToString().Trim();

            if (ds.Tables[0].Rows[0][2].ToString().Trim() == "YES")
            {
                CB_OnOrder.Checked = true;
            }
            else
            {
                CB_OnOrder.Checked = false;
            }
            if (ds.Tables[0].Rows[0][3].ToString().Trim() == "YES")
            {
                CB_OnProduction.Checked = true;
            }
            else
            {
                CB_OnProduction.Checked = false;
            }
            if (ds.Tables[0].Rows[0][4].ToString().Trim() == "YES")
            {
                CB_OnLine.Checked = true;
            }
            else
            {
                CB_OnLine.Checked = false;
            }


            LoadItemMainPlanRelatedItemPurchasePlan(strPlanVerID, strPlanMRPVerID);
            LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID);
            LoadItemMainPlanRelatedItemOutSourcingPlan(strPlanVerID, strPlanMRPVerID);


            int intWLNumber = ShareClass.GetRelatedWorkFlowNumber("PlanApproval", "Plan", strPlanMRPVerID);
            if (intWLNumber == 0)
            {
                BT_MRPExtend.Enabled = true;
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_MRPExtend.Enabled = false;
                BT_Delete.Enabled = false;
            }

            HL_PurchasePlanView.NavigateUrl = "TTGoodsMRPPurchaseRequirementPlanView.aspx?PlanVerID=" + strPlanVerID + "&PlanMRPVerID=" + strPlanMRPVerID;
            HL_ProductionPlanView.NavigateUrl = "TTGoodsMRPProductionRequirementPlanView.aspx?PlanVerID=" + strPlanVerID + "&PlanMRPVerID=" + strPlanMRPVerID;
            HL_OutsourcePlanView.NavigateUrl = "TTGoodsMRPOutsourceRequirementPlanView.aspx?PlanVerID=" + strPlanVerID + "&PlanMRPVerID=" + strPlanMRPVerID;

            HL_MainPlanMateriaDetailReport.NavigateUrl = "TTGoodsMainPlanMaterialDetailDataReport.aspx?PlanVerID=" + strPlanVerID + "&MRPVerID=" + strPlanMRPVerID;

            HL_TranferToProjectplan.Enabled = true;
            HL_TranferToProjectplan.NavigateUrl = "TTProjectMRPPlanToProjectPlan.aspx?MainPlanVerID=" + strPlanVerID + "&MRPPlanVerID=" + strPlanMRPVerID;


            if (strRelatedProjectID != "0" && strRelatedProjectPlanVerID != "0")
            {
                HL_RelatedProjectPlanGantt.Enabled = true;
                HL_RelatedProjectPlanGantt.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strRelatedProjectID + "&VerID=" + strRelatedProjectPlanVerID;
            }
            else
            {
                HL_RelatedProjectPlanGantt.Enabled = false;
                HL_RelatedProjectPlanGantt.NavigateUrl = "";
            }
        }

        TB_WLName.Text = LanguageHandle.GetWord("JiHua").ToString().Trim() + LB_PlanVerName.Text.Trim() + LanguageHandle.GetWord("BanBen").ToString().Trim() + strPlanMRPVerID + LanguageHandle.GetWord("ShenQing").ToString().Trim();
        ShareClass.LoadRelatedWL("PlanApproval", "Plan", int.Parse(strPlanMRPVerID), DataGrid8);
        BT_SubmitApply.Enabled = true;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanMRPVerID, strPlanVerID;

        strPlanVerID = LB_PlanVerID.Text.Trim();
        strPlanMRPVerID = int.Parse(NB_NewVerID.Amount.ToString()).ToString();

        strHQL = "Select * From T_ItemMainPlanMRPVersion Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanMRPVersion");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBNCFZJJHQJC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Insert Into T_ItemMainPlanMRPVersion(PlanVerID,PlanMRPVerID,Type,ExpendType,CreatorCode,CreatorName,CreateTime,OnOrder,OnProduction,OnLine,RelatedProjectID,RelatedProjectPlanVerID)";
        strHQL += " Values(" + strPlanVerID + "," + strPlanMRPVerID + ",'BACKUP','PART'," + "'" + strUserCode + "'" + "," + "'" + ShareClass.GetUserName(strUserCode) + "'" + ",now(),'NO','NO','NO',0,0)";
        ShareClass.RunSqlCommand(strHQL);

        LoadPlanMRPVersion(strPlanVerID);

        LoadItemMainPlanRelatedItemPurchasePlan(strPlanVerID, strPlanMRPVerID);
        LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID);
        LoadItemMainPlanRelatedItemOutSourcingPlan(strPlanVerID, strPlanMRPVerID);

        DL_PlanMRPVersionID.SelectedValue = GetPlanMRPVersionValueID(strPlanVerID, strPlanMRPVerID);

        TB_WLName.Text = LanguageHandle.GetWord("JiHua").ToString().Trim() + LB_PlanVerName.Text.Trim() + LanguageHandle.GetWord("BanBen").ToString().Trim() + strPlanMRPVerID + LanguageHandle.GetWord("ShenQing").ToString().Trim();
        ShareClass.LoadRelatedWL("PlanApproval", "Plan", int.Parse(strPlanMRPVerID), DataGrid8);
        BT_SubmitApply.Enabled = true;
    }

    protected string GetPlanMRPVersionValueID(string strPlanVerID, string strPlanMRPVerID)
    {
        string strHQL;

        strHQL = "Select ID From T_ItemMainPlanMRPVersion Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanMRPVersion");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanMRPVerID, strPlanVerID;

        strPlanVerID = LB_PlanVerID.Text.Trim();
        strPlanMRPVerID = int.Parse(NB_NewVerID.Amount.ToString()).ToString();

        strHQL = "Delete From T_ItemMainPlanMRPVersion Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Delete From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        ShareClass.RunSqlCommand(strHQL);

        strHQL = "Delete From T_ItemMainPlanRelatedItemPurchasePlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        ShareClass.RunSqlCommand(strHQL);

        LoadPlanMRPVersion(strPlanVerID);

        LoadItemMainPlanRelatedItemPurchasePlan(strPlanVerID, "0");
        LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, "0");

        BT_SubmitApply.Enabled = false;
    }

    protected void DL_VersionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanVerID, strMRPID, strPlanMRPVerID, strMRPVerType;

        strMRPVerType = DL_VersionType.SelectedValue.Trim();
        strPlanVerID = LB_PlanVerID.Text.Trim();
        strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text.Trim();
        strMRPID = DL_PlanMRPVersionID.SelectedValue.Trim();

        try
        {
            strHQL = "Update T_ItemMainPlanMRPVersion Set Type = " + "'" + strMRPVerType + "'" + " Where ID = " + strMRPID;
            ShareClass.RunSqlCommand(strHQL);

            HL_MainPlanMateriaDetailReport.NavigateUrl = "TTGoodsMainPlanMaterialDetailDataReport.aspx?PlanVerID=" + strPlanVerID + "&MRPVerID=" + strPlanMRPVerID;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBBLXGGCG").ToString().Trim() + "')", true);
        }
        catch
        {
        }
    }

    protected void DL_ExpendType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanVerID, strMRPID, strPlanMRPVerID, strExpendType;

        strExpendType = DL_ExpendType.SelectedValue.Trim();
        strPlanVerID = LB_PlanVerID.Text.Trim();
        strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text.Trim();
        strMRPID = DL_PlanMRPVersionID.SelectedValue.Trim();

        try
        {
            strHQL = "Update T_ItemMainPlanMRPVersion Set ExpendType = " + "'" + strExpendType + "'" + " Where ID = " + strMRPID;
            ShareClass.RunSqlCommand(strHQL);

            HL_MainPlanMateriaDetailReport.NavigateUrl = "TTGoodsMainPlanMaterialDetailDataReport.aspx?PlanVerID=" + strPlanVerID + "&MRPVerID=" + strPlanMRPVerID;

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZKLXGGCG").ToString().Trim() + "')", true);
        }
        catch
        {
        }
    }

    protected void BT_MRPExtend_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strPlanVerID, strPlanMRPVerID, strPlanMRPID, strMRPExpendType;
        string strOnOrder, strOnProduction, strOnLine;


        strPlanVerID = LB_PlanVerID.Text.Trim();
        strPlanMRPID = DL_PlanMRPVersionID.SelectedValue.Trim();
        strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text;
        strMRPExpendType = DL_ExpendType.SelectedValue.Trim();

        if (CB_OnOrder.Checked == true)
        {
            strOnOrder = "YES";
        }
        else
        {
            strOnOrder = "NO";
        }
        if (CB_OnProduction.Checked == true)
        {
            strOnProduction = "YES";
        }
        else
        {
            strOnProduction = "NO";
        }
        if (CB_OnLine.Checked == true)
        {
            strOnLine = "YES";
        }
        else
        {
            strOnLine = "NO";
        }

        if (strPlanMRPVerID == null | strPlanMRPVerID == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZMRPJHBBBNWKQJC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Delete From T_ItemMainPlanRelatedItemPlanData Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        ShareClass.RunSqlCommand(strHQL);
        strHQL = "Delete From T_ItemMainPlanRelatedItemRemainingData Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        ShareClass.RunSqlCommand(strHQL);
        strHQL = "Delete From T_ItemMainPlanRelatedItemPurchasePlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        ShareClass.RunSqlCommand(strHQL);
        strHQL = "Delete From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        ShareClass.RunSqlCommand(strHQL);


        //MRP计划展开计算
        TakeTopPlan.GoodsMRPPlanExtend(strPlanVerID, strPlanMRPVerID, strMRPExpendType, strOnOrder, strOnProduction, strOnLine);

        LoadItemMainPlanRelatedItemPurchasePlan(strPlanVerID, strPlanMRPVerID);
        LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID);
        LoadItemMainPlanRelatedItemOutSourcingPlan(strPlanVerID, strPlanMRPVerID);

        strHQL = "Update T_ItemMainPlanMRPVersion Set OnOrder = " + "'" + strOnOrder + "',OnProduction = " + "'" + strOnProduction + "',OnLine = " + "'" + strOnLine + "'";
        strHQL += " Where PlanVerID = " + strPlanVerID + " and ID = " + strPlanMRPID;
        ShareClass.RunSqlCommand(strHQL);

        TabContainer1.ActiveTabIndex = 1;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "hideWaitingIcon", "document.getElementById('IMG_Waiting').style.display = 'none';", true);
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZMRPZKYSWC").ToString().Trim() + "')", true);
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strPlanVerID, strPlanMRPVerID;
        string strBusinessType, strBussnessID, strRecordID;

        DataSet ds;

        strPlanVerID = LB_PlanVerID.Text.Trim();
        strPlanMRPVerID = DL_PlanMRPVersionID.SelectedItem.Text.Trim();

        strBusinessType = DL_BusinessType.SelectedValue.Trim();
        strBussnessID = NB_BusinessID.Amount.ToString();
        strRecordID = NB_BusinessRecordID.Amount.ToString();


        if (strBusinessType == "SaleOrder")
        {
            if (strRecordID == "0")
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemPurchasePlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID in (Select ID From T_GoodsSaleRecord Where SOID in (Select SOID From T_GoodsSaleOrder Where SOID = " + strBussnessID + "))";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemPurchasePlan");

                DataGrid2.DataSource = ds;
                DataGrid2.DataBind();

                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID in (Select ID From T_GoodsSaleRecord Where  SOID in (Select SOID From T_GoodsSaleOrder Where SOID = " + strBussnessID + "))";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();

                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID in (Select ID From T_GoodsSaleRecord Where  SOID in (Select SOID From T_GoodsSaleOrder Where SOID = " + strBussnessID + "))";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid4.DataSource = ds;
                DataGrid4.DataBind();
            }
            else
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemPurchasePlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID = " + strRecordID;
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemPurchasePlan");

                DataGrid2.DataSource = ds;
                DataGrid2.DataBind();

                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID = " + strRecordID;
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();

                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                strHQL += " and SourceType = 'GoodsSORecord' and SourceRecordID = " + strRecordID;
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid4.DataSource = ds;
                DataGrid4.DataBind();
            }
        }

        if (strBusinessType == "Project")
        {
            if (strRecordID == "0")
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemPurchasePlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID in (Select ID From T_ProjectRelatedItem Where ProjectID = " + strBussnessID + ")";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemPurchasePlan");

                DataGrid2.DataSource = ds;
                DataGrid2.DataBind();

                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID in (Select ID From T_ProjectRelatedItem Where  ProjectID = " + strBussnessID + ")";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();

                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID in (Select ID From T_ProjectRelatedItem Where  ProjectID = " + strBussnessID + ")";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid4.DataSource = ds;
                DataGrid4.DataBind();
            }

            else
            {
                strHQL = "Select * From T_ItemMainPlanRelatedItemPurchasePlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID = " + strRecordID;
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemPurchasePlan");

                DataGrid2.DataSource = ds;
                DataGrid2.DataBind();

                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
                strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID = " + strRecordID;
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid3.DataSource = ds;
                DataGrid3.DataBind();

                strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
                strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
                strHQL += " and SourceType = 'GoodsPJRecord' and SourceRecordID = " + strRecordID;
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

                DataGrid4.DataSource = ds;
                DataGrid4.DataBind();
            }
        }

        if (strBusinessType == "ApplicationOrder")
        {
            strHQL = "Select * From T_ItemMainPlanRelatedItemPurchasePlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
            strHQL += " and SourceType = 'GoodsAORecord' and SourceRecordID in (Select ID From T_GoodsApplicationDetail Where AAID in (Select AAID From T_GoodsApplication Where AAID = " + "'" + strBussnessID + "'))";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemPurchasePlan");

            DataGrid2.DataSource = ds;
            DataGrid2.DataBind();

            strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
            strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
            strHQL += " and SourceType = 'GoodsAORecord' and SourceRecordID in (Select ID From T_GoodsApplicationDetail Where AAID in (Select AID From T_GoodsApplication Where AAID = " + "'" + strBussnessID + "'))";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

            DataGrid3.DataSource = ds;
            DataGrid3.DataBind();

            strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
            strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
            strHQL += " and SourceType = 'GoodsAORecord' and SourceRecordID in (Select ID From T_GoodsApplicationDetail Where AAID in (Select AID From T_GoodsApplication Where AAID = " + "'" + strBussnessID + "'))";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

            DataGrid4.DataSource = ds;
            DataGrid4.DataBind();
        }

        if (strBusinessType == "Other")
        {
            LoadItemMainPlanRelatedItemPurchasePlan(strPlanVerID, strPlanMRPVerID);
            LoadItemMainPlanRelatedItemProductPlan(strPlanVerID, strPlanMRPVerID);
            LoadItemMainPlanRelatedItemOutSourcingPlan(strPlanVerID, strPlanMRPVerID);
        }

        HL_MainPlanBusinessObjectMateriaDetailReport.NavigateUrl = "TTGoodsMainPlanBusinessObjectMaterialDetailDataReport.aspx?PlanVerID=" + strPlanVerID + "&PlanMRPVerID=" + strPlanMRPVerID + "&RelatedType=" + strBusinessType + "&RelatedID=" + strBussnessID;

    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strID, strPlanVerID, strPlanMRPID, strRequireNumber;
            DateTime dtOrdertime, dtRequireTime;
            decimal deRequireNumber;

            strPlanVerID = LB_PlanVerID.Text;
            strPlanMRPID = DL_PlanMRPVersionID.SelectedValue;


            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strID = e.Item.Cells[2].Text;
            dtOrdertime = DateTime.Parse(((TextBox)e.Item.FindControl("DLC_POOrderTime")).Text);
            dtRequireTime = DateTime.Parse(((TextBox)e.Item.FindControl("DLC_PORequireTime")).Text);

            try
            {
                strRequireNumber = ((TextBox)(e.Item.FindControl("NB_PORequireNumber"))).Text;
                deRequireNumber = decimal.Parse(strRequireNumber);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBXWSZX").ToString().Trim() + "')", true);
                return;
            }

            try
            {
                strHQL = "Update T_ItemMainPlanRelatedItemPurchasePlan Set RequireNumber = " + deRequireNumber.ToString() + ", OrderTime = '" + dtOrdertime.ToString("yyyy-MM-dd") + "' ,RequireTime = '" + dtRequireTime.ToString("yyyy-MM-dd") + "' Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
            }
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strID, strPlanVerID, strPlanMRPID;
            DateTime dtOrdertime, dtRequireTime;
            decimal deRequireNumber;

            strPlanVerID = LB_PlanVerID.Text;
            strPlanMRPID = DL_PlanMRPVersionID.SelectedValue;

            for (int i = 0; i < DataGrid3.Items.Count; i++)
            {
                DataGrid3.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strID = e.Item.Cells[2].Text;
            dtOrdertime = DateTime.Parse(((TextBox)e.Item.FindControl("DLC_PDOrderTime")).Text);
            dtRequireTime = DateTime.Parse(((TextBox)e.Item.FindControl("DLC_PDRequireTime")).Text);
            try
            {
                deRequireNumber = decimal.Parse(((TextBox)e.Item.FindControl("NB_PDRequireNumber")).Text);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBXWSZX").ToString().Trim() + "')", true);
                return;
            }

            try
            {
                strHQL = "Update T_ItemMainPlanRelatedItemProductPlan Set RequireNumber = " + deRequireNumber.ToString() + ", OrderTime = '" + dtOrdertime.ToString("yyyy-MM-dd") + "' ,RequireTime = '" + dtRequireTime.ToString("yyyy-MM-dd") + "' Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
            }
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strID, strPlanVerID, strPlanMRPID;
            DateTime dtOrdertime, dtRequireTime;
            decimal deRequireNumber;

            strPlanVerID = LB_PlanVerID.Text;
            strPlanMRPID = DL_PlanMRPVersionID.SelectedValue;

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                DataGrid4.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strID = e.Item.Cells[2].Text;
            dtOrdertime = DateTime.Parse(((TextBox)e.Item.FindControl("DLC_PDOrderTime")).Text);
            dtRequireTime = DateTime.Parse(((TextBox)e.Item.FindControl("DLC_PDRequireTime")).Text);
            try
            {
                deRequireNumber = decimal.Parse(((TextBox)e.Item.FindControl("NB_PDRequireNumber")).Text);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSLBXWSZX").ToString().Trim() + "')", true);
                return;
            }


            try
            {
                strHQL = "Update T_ItemMainPlanRelatedItemProductPlan Set RequireNumber = " + deRequireNumber.ToString() + ", OrderTime = '" + dtOrdertime.ToString("yyyy-MM-dd") + "' ,RequireTime = '" + dtRequireTime.ToString("yyyy-MM-dd") + "' Where ID = " + strID;
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
            }
        }
    }

    protected string SubmitApply()
    {
        string strWLName, strWLType, strTemName, strXMLFileName, strXMLFile2;
        string strDescription, strCreatorCode, strCreatorName;
        string strCmdText, strPlanMRPID;

        string strWLID, strUserCode;

        strWLID = "0";
        strUserCode = LB_UserCode.Text.Trim();

        strPlanMRPID = DL_PlanMRPVersionID.SelectedValue.Trim();

        XMLProcess xmlProcess = new XMLProcess();

        strWLName = TB_WLName.Text.Trim();
        strWLType = DL_WFType.SelectedValue.Trim();
        strTemName = DL_TemName.SelectedValue.Trim();
        strDescription = TB_Description.Text.Trim();
        strCreatorCode = LB_UserCode.Text.Trim();
        strCreatorName = ShareClass.GetUserName(strCreatorCode);

        if (strTemName == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSSCSBLCMBBNWKJC").ToString().Trim() + "');</script>");
            return "0";
        }

        strXMLFileName = strWLType + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
        strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        WorkFlow workFlow = new WorkFlow();

        workFlow.WLName = strWLName;
        workFlow.WLType = strWLType;
        workFlow.XMLFile = strXMLFile2;
        workFlow.TemName = strTemName;
        workFlow.Description = strDescription;
        workFlow.CreatorCode = strCreatorCode;
        workFlow.CreatorName = strCreatorName;
        workFlow.CreateTime = DateTime.Now;
        workFlow.RelatedType = "Plan";
        workFlow.Status = "New";
        workFlow.RelatedID = int.Parse(strPlanMRPID);
        workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

        if (CB_SMS.Checked == true)
        {
            workFlow.ReceiveSMS = "YES";
        }
        else
        {
            workFlow.ReceiveSMS = "NO";
        }

        if (CB_Mail.Checked == true)
        {
            workFlow.ReceiveEMail = "YES";
        }
        else
        {
            workFlow.ReceiveEMail = "NO";
        }

        try
        {
            workFlowBLL.AddWorkFlow(workFlow);

            strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

            ShareClass.LoadRelatedWL(strWLType, "Plan", int.Parse(strPlanMRPID), DataGrid8);

            strCmdText = "select A.*,B.ID from T_ItemMainPlan A,T_ItemMainPlanMRPVersion B where A.PlanVerID = B.PlanVerID and B.ID =  " + strPlanMRPID;
            strXMLFile2 = Server.MapPath(strXMLFile2);
            xmlProcess.DbToXML(strCmdText, "T_ItemMainPlan", strXMLFile2);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZMRPJHSPSSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZMRPJHSPSSBKNGZLMCGCZD25GHZJC").ToString().Trim() + "')", true);
            return "0";
        }

        return strWLID;
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }

    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'PlanApproval'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }

    protected void LoadItemMainPlanRelatedItemPurchasePlan(string strPlanVerID, string strPlanMRPVerID)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlanRelatedItemPurchasePlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemPurchasePlan");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void LoadItemMainPlanRelatedItemProductPlan(string strPlanVerID, string strPlanMRPVerID)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'MadeParts')";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void LoadItemMainPlanRelatedItemOutSourcingPlan(string strPlanVerID, string strPlanMRPVerID)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlanRelatedItemProductPlan Where PlanVerID = " + strPlanVerID + " and PlanMRPVerID = " + strPlanMRPVerID;
        strHQL += " and ItemCode in (Select ItemCode From T_Item Where Type = 'OutParts')";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanRelatedItemProductPlan");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql5.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlan");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();
    }

    protected void LoadPlanMRPVersion(string strPlanVerID)
    {
        string strHQL;

        strHQL = "Select ID,PlanMRPVerID,Type From T_ItemMainPlanMRPVersion Where PlanVerID = " + strPlanVerID;
        strHQL += " Order By PlanVerID ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanMRPVersion");
        DL_PlanMRPVersionID.DataSource = ds;
        DL_PlanMRPVersionID.DataBind();
    }

    protected void LoadItemMainPlan(string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlan Where CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " Or CreatorCode in (Select UserCode From T_ProjectMember Where DepartCode in  " + LB_DepartString.Text.Trim() + ")";
        strHQL += " Order By PlanVerID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlan");

        DataGrid5.DataSource = ds;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }

    protected void LoadItemMainPlanDetail(string strPlanVerID)
    {
        string strHQL;

        strHQL = "Select * From T_ItemMainPlanDetail Where PlanVerID = " + strPlanVerID;
        strHQL += " Order By ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ItemMainPlanDetail");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql1.Text = strHQL;
    }

    //取得当月结余库存量
    public decimal GetGoodsInventoryNumber(string strGoodsCode, string strModelNumber, string strSpecification, string strUnit)
    {
        string strHQL;
        decimal deInventoryNumber;


        strHQL = "Select COALESCE(Sum(Number),0) From T_Goods";
        strHQL += " Where GoodsCode = " + "'" + strGoodsCode.Trim() + "'";
        strHQL += " and ModelNumber = " + "'" + strModelNumber + "'";
        strHQL += " and Spec = " + "'" + strSpecification.Trim() + "'";
        strHQL += " and UnitName = " + "'" + strUnit.Trim() + "'";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Goods");

        if (ds.Tables[0].Rows.Count > 0)
        {
            deInventoryNumber = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            deInventoryNumber = 0;
        }


        return deInventoryNumber;
    }


}
