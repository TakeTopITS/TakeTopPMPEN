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

public partial class TTBMAnnInvitationSupplier : System.Web.UI.Page
{
    string strUserCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadBMAnnInvitationList(GetBMSupplierInfoID(strUserCode.Trim()));
        }
    }

    protected void LoadBMAnnInvitationList(string strSupplierId)
    {
        string strHQL;
        string strMidId = "," + strSupplierId + ",";
        string strBeforeId = strSupplierId + ",";
        string strBackId = "," + strSupplierId;
        int i = strSupplierId.Length + 1;
        strHQL = "Select * From T_BMAnnInvitation Where (BidObjects like '%" + strMidId + "%' or LEFT(BidObjects," + i + ") = '" + strBeforeId + "' or RIGHT(BidObjects," + i + ") = '" + strBackId + "' or BidObjects = '" + strSupplierId + "') ";

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

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (!GetBMSupplierInfoStatus(strUserCode.Trim()).Trim().Equals("Qualified"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGYSXXWSHHZSHBTGBNJXTBCZJC") + "')", true);
            return;
        }
        if (!string.IsNullOrEmpty(lbl_BMSupplierBidID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGTBXXYCZZNXGHSCJC") + "')", true);
            return;
        }
        if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < DateTime.Parse(lbl_StartTime.Text.Trim()) || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > DateTime.Parse(lbl_EndTime.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDRBZZBYXNJC") + "')", true);
            return;
        }

        BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
        BMSupplierBidRecordBLL bMSupplierBidRecordBLL = new BMSupplierBidRecordBLL();
        BMSupplierBid bMSupplierBid = new BMSupplierBid();
        BMSupplierBidRecord bMSupplierBidRecord = new BMSupplierBidRecord();

        bMSupplierBid.AnnInvitationID = int.Parse(LB_ID.Text.Trim());
        bMSupplierBid.AnnInvitationName = TB_Name.Text.Trim();
        bMSupplierBid.SupplierCode = int.Parse(GetBMSupplierInfoID(strUserCode.Trim()));
        bMSupplierBid.BiddingContent = TB_BiddingContent.Text.Trim();
        bMSupplierBid.BiddingDate = DateTime.Parse(DateTime.Now.ToString());
        bMSupplierBid.BidStatus = "W";
        bMSupplierBid.NoticeContent = "";
        bMSupplierBid.ExportResult = "";

        try
        {
            bMSupplierBidBLL.AddBMSupplierBid(bMSupplierBid);

            lbl_BMSupplierBidID.Text = GetMaxBMSupplierBidID(bMSupplierBid).ToString();

            bMSupplierBidRecord.SupplierBidID = int.Parse(lbl_BMSupplierBidID.Text.Trim());
            bMSupplierBidRecord.AnnInvitationID = int.Parse(LB_ID.Text.Trim());
            bMSupplierBidRecord.SupplierCode = bMSupplierBid.SupplierCode;
            bMSupplierBidRecord.BiddingContent = bMSupplierBid.BiddingContent;
            bMSupplierBidRecord.OperationType = "Add";   
            bMSupplierBidRecord.OperationDate = DateTime.Parse(DateTime.Now.ToString());
            bMSupplierBidRecordBLL.AddBMSupplierBidRecord(bMSupplierBidRecord);

            LoadBMAnnInvitationList(bMSupplierBid.SupplierCode.ToString());

            LoadBMSupplierBidRecordList(GetBMSupplierInfoID(strUserCode.Trim()), bMSupplierBid.AnnInvitationID.ToString());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);
        }
    }

    /// <summary>
    /// 新增时，获取表T_BMSupplierBid中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMSupplierBidID(BMSupplierBid bmbp)
    {
        string strHQL = "Select ID From T_BMSupplierBid where SupplierCode='" + bmbp.SupplierCode.ToString().Trim() + "' and AnnInvitationID='" + bmbp.AnnInvitationID.ToString().Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (!GetBMSupplierInfoStatus(strUserCode.Trim()).Trim().Equals("Qualified"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGYSXXWSHHZSHBTGBNJXTBCZJC") + "')", true);
            return;
        }
        if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < DateTime.Parse(lbl_StartTime.Text.Trim()) || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > DateTime.Parse(lbl_EndTime.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDRBZZBYXNJC") + "')", true);
            return;
        }

        BMSupplierBidRecordBLL bMSupplierBidRecordBLL = new BMSupplierBidRecordBLL();
        BMSupplierBidRecord bMSupplierBidRecord = new BMSupplierBidRecord();
        string strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID = '" + lbl_BMSupplierBidID.Text.Trim() + "'";
        BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
        IList lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
        BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[0];
        if (!bMSupplierBid.BidStatus.Trim().Equals("W"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGTBXXYKBWFXGTBXXJC") + "')", true);
            return;
        }
        bMSupplierBid.AnnInvitationName = TB_Name.Text.Trim();
        bMSupplierBid.BiddingContent = TB_BiddingContent.Text.Trim();

        try
        {
            bMSupplierBidBLL.UpdateBMSupplierBid(bMSupplierBid, bMSupplierBid.ID);

            bMSupplierBidRecord.SupplierBidID = bMSupplierBid.ID;
            bMSupplierBidRecord.AnnInvitationID = int.Parse(LB_ID.Text.Trim());
            bMSupplierBidRecord.SupplierCode = bMSupplierBid.SupplierCode;
            bMSupplierBidRecord.BiddingContent = bMSupplierBid.BiddingContent;
            bMSupplierBidRecord.OperationType = "Modify";   
            bMSupplierBidRecord.OperationDate = DateTime.Parse(DateTime.Now.ToString());
            bMSupplierBidRecordBLL.AddBMSupplierBidRecord(bMSupplierBidRecord);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

            LoadBMAnnInvitationList(bMSupplierBid.SupplierCode.ToString());
            LoadBMSupplierBidRecordList(GetBMSupplierInfoID(strUserCode.Trim()), bMSupplierBid.AnnInvitationID.ToString());
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        if (!GetBMSupplierInfoStatus(strUserCode.Trim()).Trim().Equals("Qualified"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGYSXXWSHHZSHBTGBNJXTBCZJC") + "')", true);
            return;
        }
        if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < DateTime.Parse(lbl_StartTime.Text.Trim()) || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > DateTime.Parse(lbl_EndTime.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDRBZZBYXNJC") + "')", true);
            return;
        }

        BMSupplierBidRecordBLL bMSupplierBidRecordBLL = new BMSupplierBidRecordBLL();
        BMSupplierBidRecord bMSupplierBidRecord = new BMSupplierBidRecord();
        string strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID = '" + lbl_BMSupplierBidID.Text.Trim() + "'";
        BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
        IList lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
        BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[0];
        if (!bMSupplierBid.BidStatus.Trim().Equals("W"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGTBXXYKBWFSCTBXXJC") + "')", true);
            return;
        }

        strHQL = "Delete from T_BMSupplierBid where ID='" + lbl_BMSupplierBidID.Text.Trim() + "'";

        try
        {
            bMSupplierBidRecord.SupplierBidID = bMSupplierBid.ID;
            bMSupplierBidRecord.AnnInvitationID = int.Parse(LB_ID.Text.Trim());
            bMSupplierBidRecord.SupplierCode = bMSupplierBid.SupplierCode;
            bMSupplierBidRecord.BiddingContent = bMSupplierBid.BiddingContent.Trim();
            bMSupplierBidRecord.OperationType = "Deleted";
            bMSupplierBidRecord.OperationDate = DateTime.Parse(DateTime.Now.ToString());
            bMSupplierBidRecordBLL.AddBMSupplierBidRecord(bMSupplierBidRecord);

            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);

            LoadBMAnnInvitationList(bMSupplierBid.SupplierCode.ToString());
            LoadBMSupplierBidRecordList(GetBMSupplierInfoID(strUserCode.Trim()), bMSupplierBid.AnnInvitationID.ToString());
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    /// <summary>
    /// 获取招标计划实体
    /// </summary>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected BMBidPlan GetBMBidPlanModel(string strID)
    {
        string strHQL = " from BMBidFile as bMBidFile where bMBidFile.BidPlanID = '" + strID.Trim() + "'";
        BMBidFileBLL bMBidFileBLL = new BMBidFileBLL();
        IList lst = bMBidFileBLL.GetAllBMBidFiles(strHQL);
        RP_BMBidFiles.DataSource = lst;
        RP_BMBidFiles.DataBind();

        strHQL = " from BMBidPlan as bMBidPlan where bMBidPlan.ID = '" + strID.Trim() + "'";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            return bMBidPlan;
        }
        else
            return null;
    }

    protected void GetBMBidTemplateFile(string strID)
    {
        string strHQL = "Select * From T_BMBidTemplateFile Where BidPlanID = '" + strID.Trim() + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidTemplateFile");

        RP_BMBidTemplateFile.DataSource = ds;
        RP_BMBidTemplateFile.DataBind();
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

            strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.ID = '" + strId + "'";
            BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
            lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];

            BMBidPlan bMBidPlan = GetBMBidPlanModel(bMAnnInvitation.BidPlanID.ToString().Trim());
            lbl_StartTime.Text = bMBidPlan.BidStartDate.ToString("yyyy-MM-dd");
            lbl_EndTime.Text = bMBidPlan.BidEndDate.ToString("yyyy-MM-dd");

            LB_ID.Text = bMAnnInvitation.ID.ToString().Trim();
            DLC_EnterDate.Text = bMAnnInvitation.EnterDate.ToString("yyyy-MM-dd");
            TB_Remark.Text = bMAnnInvitation.Remark.Trim();
            TB_Name.Text = bMAnnInvitation.Name.Trim();
            TB_EnterUnit.Text = bMAnnInvitation.EnterUnit.Trim();
            TB_BidWay.Text = bMAnnInvitation.BidWay.Trim();

            GetBMSupplierBidIDAndContent(GetBMSupplierInfoID(strUserCode.Trim()), bMAnnInvitation.ID.ToString());
            LoadBMSupplierBidRecordList(GetBMSupplierInfoID(strUserCode.Trim()), bMAnnInvitation.ID.ToString());

            //取得投标模板文件
            GetBMBidTemplateFile(bMAnnInvitation.BidPlanID.ToString());

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
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
        LoadBMAnnInvitationList(GetBMSupplierInfoID(strUserCode.Trim()));
    }

    protected string GetBMSupplierInfoID(string strCode)
    {
        if (strCode.Trim().Contains("-"))
        {
            strCode = strCode.Trim().Substring(0, strCode.Trim().IndexOf("-"));
        }
        string strHQL = "Select * From T_BMSupplierInfo Where Code='" + strCode.Trim() + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            return ds.Tables[0].Rows[0]["ID"].ToString().Trim();
        }
        else
            return "0";
    }

    /// <summary>
    /// 获得供应商的审核状态
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetBMSupplierInfoStatus(string strCode)
    {
        string strHQL = "Select * From T_BMSupplierInfo Where Code='" + strCode.Trim() + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            return ds.Tables[0].Rows[0]["Status"].ToString().Trim();
        }
        else
            return "New";
    }

    protected void GetBMSupplierBidIDAndContent(string strSupplierId, string strBMAnnInvitationId)
    {
        string strHQL = "Select * From T_BMSupplierBid Where AnnInvitationID='" + strBMAnnInvitationId.Trim() + "' and SupplierCode='" + strSupplierId + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            lbl_BMSupplierBidID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
            TB_BiddingContent.Text = ds.Tables[0].Rows[0]["BiddingContent"].ToString();
        }
        else
        {
            lbl_BMSupplierBidID.Text = "";
            TB_BiddingContent.Text = "";
        }
    }

    protected void LoadBMSupplierBidRecordList(string strSupplierId, string strBMAnnInvitationId)
    {
        string strHQL = "Select * From T_BMSupplierBidRecord Where AnnInvitationID='" + strBMAnnInvitationId.Trim() + "' and SupplierCode='" + strSupplierId + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }
}
