using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTBMWinRecommend : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "中标人推荐确认", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadBMAnnInvitationList();

            ShareClass.LoadWFTemplate(strUserCode, DL_WFType.SelectedValue.Trim(), DL_TemName);

            BT_SubmitApply.Visible = false;
        }
    }

    protected void LoadBMAnnInvitationList()
    {
        string strHQL;

        strHQL = "Select * From T_BMAnnInvitation Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (EnterUnit like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or EnterPer like '%" + TextBox1.Text.Trim() + "%' " +
            "or BidWay like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-EnterDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-EnterDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            lbl_ID.Text = strId;

            strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.ID = '" + strId + "'";
            BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
            lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            if (e.CommandName == "Update")
            {
              
                lbl_ID.Text = bMAnnInvitation.ID.ToString().Trim();
                lbl_Remark.Text = bMAnnInvitation.Remark.Trim();
                lbl_Name.Text = bMAnnInvitation.Name.Trim();
                lbl_EnterCode.Text = bMAnnInvitation.EnterCode.Trim();
                lbl_BidRemark.Text = GetBMBidPlanRemark(bMAnnInvitation.BidPlanID.ToString());
                lbl_BidObjects.Text = bMAnnInvitation.BidObjects.Trim();

                GetUserNameList(bMAnnInvitation.BidPlanID.ToString());

                lbl_BidPlanID.Text = bMAnnInvitation.BidPlanID.ToString();

                HL_BMBidFile.Enabled = true;
                HL_BMBidFile.NavigateUrl = "TTBMPerformanceEvaluation.aspx?BidPlanID=" + lbl_BidPlanID.Text.Trim();

                LoadWZExpertList(lbl_BidType.Text.Trim());
                LoadBMSupplierBidList(bMAnnInvitation.ID.ToString(), bMAnnInvitation.BidObjects.Trim());

               

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }

            if (e.CommandName == "Workflow")
            {
                lbl_count.Text = GetBMSupplierBidStatus(bMAnnInvitation.ID.ToString(), bMAnnInvitation.BidObjects.Trim()).ToString();
                if (lbl_count.Text.Trim() == "0")
                {
                    BT_SubmitApply.Visible = true;
                    BT_SubmitApply.Enabled = true;

                    int intWLNumber = GetRelatedWorkFlowNumber(DL_WFType.SelectedValue.Trim(), "Other", lbl_ID.Text.Trim());

                    if (intWLNumber > 0)
                    {
                        BT_SubmitApply.Visible = false;
                    }
                }
                else
                {
                    BT_SubmitApply.Visible = false;
                }

                LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "Other", int.Parse(lbl_ID.Text.Trim()));
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWorkflow','false') ", true);
            }
        }
    }

    protected void GetUserNameList(string strBidPlanID)
    {
        string strHQL = "From BMBidPlan as bMBidPlan where bMBidPlan.ID = '" + strBidPlanID + "'";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        IList lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            lbl_BidType.Text = bMBidPlan.BidType.Trim();
            lbl_ExperCodeList.Text = string.IsNullOrEmpty(bMBidPlan.AddUserCodeList) || bMBidPlan.AddUserCodeList.Trim() == "" ? "" : bMBidPlan.AddUserCodeList.Trim();

            if (string.IsNullOrEmpty(bMBidPlan.UserCodeList) || bMBidPlan.UserCodeList.Trim() == "")
            {
                TextBox5.Text = "";
            }
            else
            {
                StringBuilder sbName = new StringBuilder();
                strHQL = "Select * From T_WZExpert Where ID in (" + bMBidPlan.UserCodeList + ") ";
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");
                if (ds.Tables[0].Rows.Count > 0 && ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sbName.AppendFormat("{0}", ds.Tables[0].Rows[i]["Name"].ToString());
                        sbName.Append(",");
                    }
                    if (sbName.Length > 0)
                    {
                        sbName.Remove(sbName.Length - 1, 1);
                    }
                }
                TextBox5.Text = sbName.ToString().Trim();
            }

            if (string.IsNullOrEmpty(bMBidPlan.AddUserCodeList) || bMBidPlan.AddUserCodeList.Trim() == "")
            {
                TextBox6.Text = "";
            }
            else
            {
                StringBuilder sbName = new StringBuilder();
                strHQL = "Select * From T_WZExpert Where ID in (" + bMBidPlan.AddUserCodeList + ") ";
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");
                if (ds.Tables[0].Rows.Count > 0 && ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        sbName.AppendFormat("{0}", ds.Tables[0].Rows[i]["Name"].ToString());
                        sbName.Append(",");
                    }
                    if (sbName.Length > 0)
                    {
                        sbName.Remove(sbName.Length - 1, 1);
                    }
                }
                TextBox6.Text = sbName.ToString().Trim();
            }
        }
        else
        {
            lbl_BidType.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMAnnInvitationList();
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName == LanguageHandle.GetWord("ZhongBiao").ToString().Trim())
        {
            strId = e.Item.Cells[1].Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID = '" + strId + "'";
            BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
            lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
            BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[0];
            bMSupplierBid.BidStatus = "Y";
            bMSupplierBidBLL.UpdateBMSupplierBid(bMSupplierBid, bMSupplierBid.ID);

            strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID <> '" + strId + "' and bMSupplierBid.AnnInvitationID='" + lbl_ID.Text.Trim() + "' " +
                " and bMSupplierBid.BidStatus='W' ";
            lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    BMSupplierBid bMSupplierBid1 = (BMSupplierBid)lst[i];
                    bMSupplierBid1.BidStatus = "N";
                    bMSupplierBidBLL.UpdateBMSupplierBid(bMSupplierBid1, bMSupplierBid1.ID);
                }
            }
            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;

            int intWLNumber = GetRelatedWorkFlowNumber(DL_WFType.SelectedValue.Trim(), "Other", lbl_ID.Text.Trim());

            if (intWLNumber > 0)
            {
                BT_SubmitApply.Visible = false;
            }
        }
        else if (e.CommandName == "Unsuccessful Bid")
        {
            strId = e.Item.Cells[1].Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID = '" + strId + "'";
            BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
            lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
            BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[0];
            bMSupplierBid.BidStatus = "N";
            bMSupplierBidBLL.UpdateBMSupplierBid(bMSupplierBid, bMSupplierBid.ID);

            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;

            int intWLNumber = GetRelatedWorkFlowNumber(DL_WFType.SelectedValue.Trim(), "Other", lbl_ID.Text.Trim());
            if (intWLNumber > 0)
            {
                BT_SubmitApply.Visible = false;
            }
        }
        LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "Other", int.Parse(lbl_ID.Text.Trim()));
        LoadBMSupplierBidList(lbl_ID.Text.Trim(), lbl_BidObjects.Text.Trim());

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql1.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void LoadBMSupplierBidList(string strAnnInvitationID, string strBidObjects)
    {
        string strHQL = "Select * From T_BMSupplierBid Where AnnInvitationID='" + strAnnInvitationID + "' and SupplierCode in (" + strBidObjects + ") order by ID Desc";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql1.Text = strHQL;
    }

    protected string GetBMSupplierInfoCode(string strId)
    {
        string strHQL = "From BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strId.Trim() + "'";
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
            return bMSupplierInfo.Code.Trim();
        }
        else
        {
            return "";
        }
    }

    protected int GetBMSupplierBidStatus(string strAnnInvitationID, string strBidObjects)
    {
        int flag = 0;
        string strHQL = "Select * From T_BMSupplierBid Where AnnInvitationID='" + strAnnInvitationID + "' and SupplierCode in (" + strBidObjects + ") and BidStatus='W' order by ID Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            flag = ds.Tables[0].Rows.Count;
        }

        return flag;
    }


    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        Button BT_Bidding = (Button)e.Item.FindControl("BT_Bidding");
        Button BT_NoBidding = (Button)e.Item.FindControl("BT_NoBidding");
        HiddenField hfStatus = (HiddenField)e.Item.FindControl("hfStatus");
        if (hfStatus != null)
        {
            if (hfStatus.Value.Trim().Equals("W"))
            {
                if (BT_Bidding != null)
                {
                    BT_Bidding.Visible = true;
                }
                else
                {
                    BT_Bidding.Visible = false;
                }
                if (BT_NoBidding != null)
                {
                    BT_NoBidding.Visible = true;
                }
                else
                {
                    BT_NoBidding.Visible = false;
                }
            }
            else if (hfStatus.Value.Trim().Equals("Y"))
            {
                if (BT_Bidding != null)
                {
                    BT_Bidding.Visible = false;
                }
                else
                {
                    BT_Bidding.Visible = false;
                }
                if (BT_NoBidding != null)
                {
                    BT_NoBidding.Visible = true;
                }
                else
                {
                    BT_NoBidding.Visible = false;
                }
            }
            else if (hfStatus.Value.Trim().Equals("N"))
            {
                if (BT_Bidding != null)
                {
                    BT_Bidding.Visible = true;
                }
                else
                {
                    BT_Bidding.Visible = false;
                }
                if (BT_NoBidding != null)
                {
                    BT_NoBidding.Visible = false;
                }
                else
                {
                    BT_NoBidding.Visible = false;
                }
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected string GetBMSupplierInfoName(string strId)
    {
        string strHQL = "From BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + strId.Trim() + "'";
        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
        IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
            return bMSupplierInfo.Name.Trim();
        }
        else
        {
            return "";
        }
    }

    protected string GetBMSupplierBidStatus(string strBidStatus)
    {
        if (strBidStatus.Trim().Equals("Y"))
        {
            return LanguageHandle.GetWord("ZhongBiao").ToString().Trim();
        }
        else if (strBidStatus.Trim().Equals("N"))
        {
            return "Unsuccessful Bid";
        }
        else
        {
            return "未开标"; 
        }
    }

    protected string GetBMBidPlanRemark(string strID)
    {
        string strHQL;
        IList lst;
        //绑定招标计划
        strHQL = "From BMBidPlan as bMBidPlan Where bMBidPlan.ID='" + strID + "' ";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            return bMBidPlan.BidRemark.Trim();
        }
        else
            return "";
    }

    protected string GetBMSupBidByExpResult(string strID)
    {
        string strHQL;
        IList lst;
        string result = "";
        //绑定招标计划
        strHQL = "From BMSupBidByExp as bMSupBidByExp Where bMSupBidByExp.SupplierBidID='" + strID + "' Order By bMSupBidByExp.ID ";
        BMSupBidByExpBLL bMSupBidByExpBLL = new BMSupBidByExpBLL();
        lst = bMSupBidByExpBLL.GetAllBMSupBidByExps(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                BMSupBidByExp bMSupBidByExp = (BMSupBidByExp)lst[i];
                result += LanguageHandle.GetWord("ZhuanGu").ToString().Trim() + ShareClass.GetUserName(bMSupBidByExp.ExportCode.Trim()) + "：" + bMSupBidByExp.ExportResult.Trim() + "；";
            }
        }
        return result;
    }

    protected void BindTemNameData(string strType)
    {
        string strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = '" + strType + "' ";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        IList lst = workFlowBLL.GetAllWorkFlows(strHQL);
        DL_TemName.Items.Clear();
        if (lst.Count > 0 && lst != null)
        {
            DL_TemName.DataSource = lst;
            DL_TemName.DataBind();
        }
        else
        {
            DL_TemName.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }

    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strtype = DL_WFType.SelectedValue.Trim();
        BindTemNameData(strtype);

        LoadRelatedWL(strtype, "Other", int.Parse(lbl_ID.Text.Trim()));

        int intWLNumber = GetRelatedWorkFlowNumber(strtype, "Other", lbl_ID.Text.Trim());

        if (intWLNumber > 0)
        {
            BT_SubmitApply.Visible = false;
        }
        else
        {
            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWorkflow','false') ", true);
    }

    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWorkflow','false') ", true);

    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWorkflow','false') ", true);

    }

    protected string SubmitApply()
    {
        string strCmdText, strID, strWLID, strXMLFileName, strXMLFile2, strTemName;

        strID = lbl_ID.Text.Trim();
        strWLID = "0";

        strTemName = DL_TemName.SelectedValue.Trim();
        if (strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWLCMBMCBNWKJC").ToString().Trim() + "')", true);
            return strWLID;
        }

        XMLProcess xmlProcess = new XMLProcess();
        //strHQL = "Update T_CarApplyForm Set Status = 'InProgress' Where ID = " + strID;
        try
        {
            //ShareClass.RunSqlCommand(strHQL);

            strXMLFileName = DL_WFType.SelectedValue.Trim() + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
            strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = DL_WFType.SelectedValue.Trim();
            workFlow.WLType = DL_WFType.SelectedValue.Trim();
            workFlow.Status = "New";
            workFlow.TemName = DL_TemName.SelectedValue.Trim();
            workFlow.CreateTime = DateTime.Now;
            workFlow.CreatorCode = strUserCode;
            workFlow.CreatorName = ShareClass.GetUserName(strUserCode.Trim());
            workFlow.Description = lbl_BidRemark.Text.Trim();
            workFlow.XMLFile = strXMLFile2;
            workFlow.RelatedType = "Other";
            workFlow.RelatedID = int.Parse(strID);
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

                strCmdText = "select * from T_BMAnnInvitation where ID = " + strID;

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_BMAnnInvitation", strXMLFile2);

                LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "Other", int.Parse(strID));

                //DL_Status.SelectedValue = "InProgress";

                BT_SubmitApply.Visible = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZBTJSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                strWLID = "0";

                BT_SubmitApply.Visible = true;
                BT_SubmitApply.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZBTJSCSBJC").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            strWLID = "0";
            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZBTJSCSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWorkflow','false') ", true);


        return strWLID;
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        LoadWZExpertList(lbl_BidType.Text.Trim());

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        StringBuilder sbName = new StringBuilder();
        StringBuilder sbCode = new StringBuilder();
        for (int i = 0; i < DataGrid3.Items.Count; i++)
        {
            CheckBox cbSelect = (CheckBox)DataGrid3.Items[i].FindControl("cbSelect");
            HiddenField hfID = (HiddenField)DataGrid3.Items[i].FindControl("hfID");
            HiddenField hfName = (HiddenField)DataGrid3.Items[i].FindControl("hfName");
            HiddenField hfExpertCode = (HiddenField)DataGrid3.Items[i].FindControl("hfExpertCode");
            if (cbSelect != null && hfID != null)
            {
                if (cbSelect.Checked)
                {
                    if (hfExpertCode != null)
                    {
                        sbName.AppendFormat("{0}", hfName.Value);
                        sbCode.AppendFormat("{0}", hfID.Value);
                    }
                    sbName.Append(",");
                    sbCode.Append(",");
                }
            }
        }
        if (sbName.Length > 0 && sbCode.Length > 0)
        {
            sbName.Remove(sbName.Length - 1, 1);
            sbCode.Remove(sbCode.Length - 1, 1);
        }
        TextBox6.Text = sbName.ToString().Trim();
        lbl_ExperCodeList.Text = sbCode.ToString().Trim();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    protected void LoadWZExpertList(string strType)
    {
        //if (strType == "工程招标")
        //    strType = "EngineeringBiddingExpert";
        //else if (strType == "物资招标")
        //    strType = "MaterialBiddingExpert";
        //else if (strType == "分包招标")
        //    strType = "分包招标专家";
        //else
        //    strType = "OtherBiddingExpert";

        string strHQL = "Select * From T_WZExpert ";

        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += "Where (ExpertCode like '%" + txt_SupplierInfo.Text.Trim() + "%' or Name like '%" + txt_SupplierInfo.Text.Trim() + "%' or WorkUnit like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
            "or Job like '%" + txt_SupplierInfo.Text.Trim() + "%' or JobTitle like '%" + txt_SupplierInfo.Text.Trim() + "%' or Phone like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
            "or ExpertType like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string strHQL = "From BMBidPlan as bMBidPlan where bMBidPlan.ID = '" + lbl_BidPlanID.Text.Trim() + "'";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        IList lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            bMBidPlan.AddUserCodeList = lbl_ExperCodeList.Text.Trim();
            bMBidPlanBLL.UpdateBMBidPlan(bMBidPlan, bMBidPlan.ID);

            HL_BMBidFile.Enabled = true;
            HL_BMBidFile.NavigateUrl = "TTBMPerformanceEvaluation.aspx?BidPlanID=" + bMBidPlan.ID.ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCPRZJCG").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }
}
