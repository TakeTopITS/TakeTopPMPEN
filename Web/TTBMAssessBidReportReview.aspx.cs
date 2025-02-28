using System; using System.Resources;
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

public partial class TTBMAssessBidReportReview : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_AssessReportDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_AssessSpeaker.Text = ShareClass.GetUserName(strUserCode);
            DLC_ReviewDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadBMAssessBidRecordName();

            LoadBMAssessBidReportList();
        }
    }

    /// <summary>
    /// 获取人员所在部门
    /// </summary>
    /// <param name="strUserCode"></param>
    /// <returns></returns>
    protected string GetUserDepartName(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = '" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];

            strHQL = "From Department as department Where department.DepartCode = '" + projectMember.DepartCode.Trim() + "'";
            DepartmentBLL departmentBLL = new DepartmentBLL();
            lst = departmentBLL.GetAllDepartments(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                Department department = (Department)lst[0];
                return department.DepartName.Trim();
            }
            else
                return "";
        }
        else
            return "";
    }

    protected void LoadBMAssessBidRecordName()
    {
        string strHQL;
        //绑定评标记录名称T_BMAssessBidRecord
     //   strHQL = "select * From T_BMAssessBidRecord where ID in (select AssessBidRecordID from T_BMAssessBidReport) Order By ID Desc";
        strHQL = "select * From T_BMAssessBidRecord Order By ID Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidRecord");
        DL_AssessBidRecordID.DataSource = ds;
        DL_AssessBidRecordID.DataBind();
        DL_AssessBidRecordID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadBMAssessBidReportList()
    {
        string strHQL;

        strHQL = "Select * From T_BMAssessBidReport Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (AssessSpeaker like '%" + TextBox1.Text.Trim() + "%' or AssessReportContent like '%" + TextBox1.Text.Trim() + "%' or Reviewer like '%" + TextBox1.Text.Trim() + "%' " +
            "or ReviewResult like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and AssessBidRecordName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-AssessReportDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-AssessReportDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidReport");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    /// <summary>
    /// 新增时，获取表T_BMAssessBidReport中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMAssessBidReportID(BMAssessBidReport bmbp)
    {
        string strHQL = "Select ID From T_BMAssessBidReport where AssessSpeaker='" + bmbp.AssessSpeaker.Trim() + "' and AssessBidRecordID='" + bmbp.AssessBidRecordID.ToString().Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidReport").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetBMAssessBidRecordName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定评标记录名称
        strHQL = "From BMAssessBidRecord as bMAssessBidRecord Where bMAssessBidRecord.ID='" + strID + "' ";
        BMAssessBidRecordBLL bMAssessBidRecordBLL = new BMAssessBidRecordBLL();
        lst = bMAssessBidRecordBLL.GetAllBMAssessBidRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAssessBidRecord bMAssessBidRecord = (BMAssessBidRecord)lst[0];
            return bMAssessBidRecord.Name.Trim();
        }
        else
            return "";
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL = "From BMAssessBidReport as bMAssessBidReport where bMAssessBidReport.ID = '" + TB_ID.Text.Trim() + "'";
        BMAssessBidReportBLL bMAssessBidReportBLL = new BMAssessBidReportBLL();
        IList lst = bMAssessBidReportBLL.GetAllBMAssessBidReports(strHQL);
        BMAssessBidReport bMAssessBidReport = (BMAssessBidReport)lst[0];

        bMAssessBidReport.AssessSpeaker = TB_AssessSpeaker.Text.Trim();
        bMAssessBidReport.Reviewer = TB_Reviewer.Text.Trim();
        bMAssessBidReport.ReviewResult = TB_ReviewResult.Text.Trim();
        bMAssessBidReport.AssessReportContent = TB_AssessReportContent.Text.Trim();
        bMAssessBidReport.AssessBidRecordID = int.Parse(DL_AssessBidRecordID.SelectedValue.Trim());
        bMAssessBidReport.AssessBidRecordName = GetBMAssessBidRecordName(bMAssessBidReport.AssessBidRecordID.ToString().Trim());
        bMAssessBidReport.AssessReportDate = DateTime.Parse(DLC_AssessReportDate.Text.Trim());
        bMAssessBidReport.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());

        try
        {
            bMAssessBidReportBLL.UpdateBMAssessBidReport(bMAssessBidReport, bMAssessBidReport.ID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSHCG").ToString().Trim()+"')", true);

            LoadBMAssessBidReportList();
            LoadBMSupplierBidList(bMAssessBidReport.AssessBidRecordID.ToString().Trim());
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSHSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From BMAssessBidReport as bMAssessBidReport where bMAssessBidReport.ID = '" + strId + "'";
            BMAssessBidReportBLL bMAssessBidReportBLL = new BMAssessBidReportBLL();
            lst = bMAssessBidReportBLL.GetAllBMAssessBidReports(strHQL);
            BMAssessBidReport bMAssessBidReport = (BMAssessBidReport)lst[0];

            TB_ID.Text = bMAssessBidReport.ID.ToString().Trim();
            DL_AssessBidRecordID.SelectedValue = bMAssessBidReport.AssessBidRecordID.ToString().Trim();
            DLC_AssessReportDate.Text = bMAssessBidReport.AssessReportDate.ToString("yyyy-MM-dd");
            TB_AssessSpeaker.Text = bMAssessBidReport.AssessSpeaker.Trim();
            TB_AssessReportContent.Text = bMAssessBidReport.AssessReportContent.Trim();
            TB_Reviewer.Text = string.IsNullOrEmpty(bMAssessBidReport.Reviewer.Trim()) ? ShareClass.GetUserName(strUserCode) : bMAssessBidReport.Reviewer.Trim();
            TB_ReviewResult.Text = bMAssessBidReport.ReviewResult.Trim();
            DLC_ReviewDate.Text = bMAssessBidReport.ReviewDate.ToString("yyyy-MM-dd");

            LoadBMSupplierBidList(bMAssessBidReport.AssessBidRecordID.ToString().Trim());

            BT_Update.Enabled = true;
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidReport");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMAssessBidReportList();
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

            strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID <> '" + strId + "' and bMSupplierBid.AnnInvitationID='" + lbl_AnnInvitationID.Text.Trim() + "' " +
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
        }
        LoadBMSupplierBidList(DL_AssessBidRecordID.SelectedValue.Trim());
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql1.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void LoadBMSupplierBidList(string strAssessBidRecordID)
    {
        string strHQL = "From BMAssessBidRecord as bMAssessBidRecord where bMAssessBidRecord.ID = '" + strAssessBidRecordID + "'";
        BMAssessBidRecordBLL bMAssessBidRecordBLL = new BMAssessBidRecordBLL();
        IList lst = bMAssessBidRecordBLL.GetAllBMAssessBidRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAssessBidRecord bMAssessBidRecord = (BMAssessBidRecord)lst[0];

            strHQL = "From BMOpenBidRecord as bMOpenBidRecord where bMOpenBidRecord.ID = '" + bMAssessBidRecord.OpenBidRecordID.ToString() + "'";
            BMOpenBidRecordBLL bMOpenBidRecordBLL = new BMOpenBidRecordBLL();
            lst = bMOpenBidRecordBLL.GetAllBMOpenBidRecords(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                BMOpenBidRecord bMOpenBidRecord = (BMOpenBidRecord)lst[0];

                strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.BidPlanID = '" + bMOpenBidRecord.BidPlanID.ToString() + "'";
                BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
                lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
                if (lst.Count > 0 && lst != null)
                {
                    BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];

                    strHQL = "Select * From T_BMSupplierBid Where AnnInvitationID='" + bMAnnInvitation.ID.ToString() + "' and SupplierCode in (" + bMAnnInvitation.BidObjects.Trim() + ") order by ID Desc";

                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");

                    DataGrid1.CurrentPageIndex = 0;
                    DataGrid1.DataSource = ds;
                    DataGrid1.DataBind();
                    lbl_AnnInvitationID.Text = bMAnnInvitation.ID.ToString();
                    lbl_sql1.Text = strHQL;
                }
            }
        }
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
            return "Unopened Bid";   
        }
    }
}
