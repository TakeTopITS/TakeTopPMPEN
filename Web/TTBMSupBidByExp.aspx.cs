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

public partial class TTBMSupBidByExp : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "评标小组确认", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadBMAnnInvitationName();
            string strexpertId = GetBMExpertID(strUserCode.Trim());
            LoadBMSupplierBidList(strexpertId);
            LoadBMSupBidByExpList(string.Empty, strUserCode.Trim());
        }
    }

    protected void LoadBMAnnInvitationName()
    {
        string strHQL;
        strHQL = "select * From T_BMAnnInvitation Order By ID Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation");
        DL_AnnInvitationID.DataSource = ds;
        DL_AnnInvitationID.DataBind();
        DL_AnnInvitationID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    /// <summary>
    /// 获取专家编号
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetBMExpertID(string strCode)
    {
        string strHQL = "Select * From T_WZExpert Where ExpertCode='" + strCode.Trim() + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");
        if (ds.Tables[0].Rows.Count > 0 && ds != null)
        {
            return ds.Tables[0].Rows[0]["ID"].ToString().Trim();
        }
        else
            return "0";
    }

    protected void LoadBMSupplierBidList(string strExpertId)
    {
        string strHQL;
        string strBeforeId = strExpertId + ",";
        string strBackId = "," + strExpertId;
        string strMidId = "," + strExpertId + ",";
        int i = strExpertId.Length + 1;
        strHQL = "Select A.*,B.Name,B.BidPlanID From T_BMSupplierBid A,T_BMAnnInvitation B Where A.AnnInvitationID=B.ID and B.BidPlanID in (Select ID From T_BMBidPlan C Where " +
            "LEFT(C.UserCodeList," + i + ") = '" + strBeforeId + "' or RIGHT(C.UserCodeList," + i + ") = '" + strBackId + "'or C.UserCodeList = '" + strExpertId + "' or C.UserCodeList like '%" + strMidId + "%')";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (B.Name like '%" + TextBox1.Text.Trim() + "%' or B.Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and A.BiddingContent like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-A.BiddingDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-A.BiddingDate::date>=0 ";
        }
        strHQL += " Order By A.ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    /// <summary>
    /// 新增时，获取表T_BMSupBidByExp中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMSupBidByExpID(BMSupBidByExp bmbp)
    {
        string strHQL = "Select ID From T_BMSupBidByExp where SupplierBidID='" + bmbp.SupplierBidID.ToString().Trim() + "' and ExportCode='" + bmbp.ExportCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupBidByExp").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 根据投标ID及专家编码，查询专家评标意见
    /// </summary>
    /// <param name="strSupplierBidID"></param>
    /// <param name="strExpertCode"></param>
    /// <returns></returns>
    protected BMSupBidByExp GetBMSupBidByExpModel(string strSupplierBidID, string strExpertCode)
    {
        string strHQL;
        IList lst;
        strHQL = "From BMSupBidByExp as bMSupBidByExp Where bMSupBidByExp.SupplierBidID='" + strSupplierBidID + "' and bMSupBidByExp.ExportCode='" + strExpertCode + "' ";
        BMSupBidByExpBLL bMSupBidByExpBLL = new BMSupBidByExpBLL();
        lst = bMSupBidByExpBLL.GetAllBMSupBidByExps(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupBidByExp bMSupBidByExp = (BMSupBidByExp)lst[0];
            return bMSupBidByExp;
        }
        else
            return null;
    }

    /// <summary>
    /// 根据投标ID，获取投标实体
    /// </summary>
    /// <param name="strSupplierBidID"></param>
    /// <returns></returns>
    protected BMSupplierBid GetBMSupplierBidModel(string strSupplierBidID)
    {
        string strHQL;
        IList lst;
        strHQL = "From BMSupplierBid as bMSupplierBid Where bMSupplierBid.ID='" + strSupplierBidID + "' ";
        BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
        lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[0];
            return bMSupplierBid;
        }
        else
            return null;
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        BMSupBidByExpBLL bMSupBidByExpBLL = new BMSupBidByExpBLL();
        if (TB_BidStatus.Text.Trim() != LanguageHandle.GetWord("WeiKaiBiao").ToString().Trim() || string.IsNullOrEmpty(TB_BidStatus.Text) || TB_BidStatus.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGTBYKBWXZCR").ToString().Trim() + "')", true);
            return;
        }

        if (string.IsNullOrEmpty(LB_ID.Text) || LB_ID.Text.Trim() == "")
        {
            BMSupBidByExp bMSupBidByExp = new BMSupBidByExp();
            bMSupBidByExp.BiddingContent = TB_BiddingContent.Text.Trim();
            bMSupBidByExp.ExportCode = strUserCode.Trim();
            bMSupBidByExp.ExportResult = TB_ExportResult.Text.Trim();
            bMSupBidByExp.SupplierBidID = int.Parse(string.IsNullOrEmpty(lbl_BidID.Text) || lbl_BidID.Text.Trim() == "" ? "0" : lbl_BidID.Text.Trim());

            try
            {
                bMSupBidByExpBLL.AddBMSupBidByExp(bMSupBidByExp);
                LB_ID.Text = GetBMSupBidByExpModel(bMSupBidByExp.SupplierBidID.ToString(), strUserCode.Trim()).ID.ToString();

                LoadBMSupplierBidList(GetBMExpertID(strUserCode.Trim()));
                LoadBMSupBidByExpList(bMSupBidByExp.SupplierBidID.ToString(), strUserCode.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            }
        }
        else
        {
            strHQL = "From BMSupBidByExp as bMSupBidByExp where bMSupBidByExp.ID = '" + LB_ID.Text.Trim() + "'";
            lst = bMSupBidByExpBLL.GetAllBMSupBidByExps(strHQL);
            BMSupBidByExp bMSupBidByExp = (BMSupBidByExp)lst[0];
            bMSupBidByExp.ExportResult = TB_ExportResult.Text.Trim();

            try
            {
                bMSupBidByExpBLL.UpdateBMSupBidByExp(bMSupBidByExp, bMSupBidByExp.ID);

                LoadBMSupplierBidList(GetBMExpertID(strUserCode.Trim()));
                LoadBMSupBidByExpList(bMSupBidByExp.SupplierBidID.ToString(), strUserCode.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZRSBJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            }
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

            strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID = '" + strId + "'";
            BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
            lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
            BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[0];

            lbl_BidID.Text = bMSupplierBid.ID.ToString();
            DL_AnnInvitationID.SelectedValue = bMSupplierBid.AnnInvitationID.ToString().Trim();
            TB_SupplierName.Text = GetBMSupplierInfoName(bMSupplierBid.SupplierCode.ToString());
            LB_SupplierCode.Text = GetBMSupplierInfoCode(bMSupplierBid.SupplierCode.ToString());
            TB_BidStatus.Text = GetBMSupplierBidStatus(bMSupplierBid.BidStatus.Trim());
            TB_BiddingContent.Text = bMSupplierBid.BiddingContent.Trim();
            lbl_AnnInvitationContent.Text = GetBMAnnInvitationRemark(bMSupplierBid.AnnInvitationID.ToString());
            BMSupBidByExp bMSupBidByExp = GetBMSupBidByExpModel(bMSupplierBid.ID.ToString(), strUserCode.Trim());
            if (bMSupBidByExp == null)
            {
                TB_ExportResult.Text = "";
                LB_ID.Text = "";
            }
            else
            {
                LB_ID.Text = bMSupBidByExp.ID.ToString();
                TB_ExportResult.Text = bMSupBidByExp.ExportResult.Trim();
            }

            LoadBMSupBidByExpList(bMSupplierBid.ID.ToString(), strUserCode.Trim());

            BT_Update.Enabled = true;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        string strexpertId = GetBMExpertID(strUserCode.Trim());
        LoadBMSupplierBidList(strexpertId);
        LoadBMSupBidByExpList(string.Empty, strUserCode.Trim());
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From BMSupBidByExp as bMSupBidByExp where bMSupBidByExp.ID = '" + strId + "'";
            BMSupBidByExpBLL bMSupBidByExpBLL = new BMSupBidByExpBLL();
            lst = bMSupBidByExpBLL.GetAllBMSupBidByExps(strHQL);
            BMSupBidByExp bMSupBidByExp = (BMSupBidByExp)lst[0];

            LB_ID.Text = bMSupBidByExp.ID.ToString().Trim();
            TB_SupplierName.Text = GetBMSupplierNameByID(bMSupBidByExp.SupplierBidID.ToString());
            LB_SupplierCode.Text = GetBMSupplierCodeByID(bMSupBidByExp.SupplierBidID.ToString());
            TB_BiddingContent.Text = bMSupBidByExp.BiddingContent.Trim();
            TB_ExportResult.Text = bMSupBidByExp.ExportResult.Trim();

            BMSupplierBid bMSupplierBid = GetBMSupplierBidModel(bMSupBidByExp.SupplierBidID.ToString());
            if (bMSupplierBid == null)
            {
                DL_AnnInvitationID.SelectedValue = "0";
                TB_BidStatus.Text = "";
                lbl_BidID.Text = "";
                lbl_AnnInvitationContent.Text = "";
            }
            else
            {
                DL_AnnInvitationID.SelectedValue = bMSupplierBid.AnnInvitationID.ToString().Trim();
                TB_BidStatus.Text = GetBMSupplierBidStatus(bMSupplierBid.BidStatus.Trim());
                lbl_BidID.Text = bMSupplierBid.ID.ToString();
                lbl_AnnInvitationContent.Text = GetBMAnnInvitationRemark(bMSupplierBid.AnnInvitationID.ToString());
            }

            BT_Update.Enabled = true;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql1.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupBidByExp");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadBMSupBidByExpList(string strSupplierBidID, string strExportCode)
    {
        string strHQL;
        if (string.IsNullOrEmpty(strSupplierBidID))
            strHQL = "From BMSupBidByExp as bMSupBidByExp where bMSupBidByExp.ExportCode = '" + strExportCode + "'";
        else
            strHQL = "From BMSupBidByExp as bMSupBidByExp where bMSupBidByExp.ExportCode = '" + strExportCode + "' and bMSupBidByExp.SupplierBidID='" + strSupplierBidID + "' ";

        BMSupBidByExpBLL bMSupBidByExpBLL = new BMSupBidByExpBLL();
        IList lst = bMSupBidByExpBLL.GetAllBMSupBidByExps(strHQL);
        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
        lbl_sql1.Text = strHQL;
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

    protected string GetBMAnnInvitationRemark(string strId)
    {
        string strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.ID = '" + strId.Trim() + "'";
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
            return bMAnnInvitation.Remark.Trim();
        }
        else
        {
            return "";
        }
    }

    protected string GetBMSupplierNameByID(string strId)
    {
        string strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID = '" + strId + "'";
        BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
        IList lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[0];
            strHQL = "From BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + bMSupplierBid.SupplierCode.ToString() + "'";
            BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
            lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
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
        else
            return "";
    }

    protected string GetBMSupplierCodeByID(string strId)
    {
        string strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.ID = '" + strId + "'";
        BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
        IList lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[0];
            strHQL = "From BMSupplierInfo as bMSupplierInfo where bMSupplierInfo.ID = '" + bMSupplierBid.SupplierCode.ToString() + "'";
            BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
            lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                return bMSupplierInfo.Code;
            }
            else
            {
                return "";
            }
        }
        else
            return "";
    }

    protected string GetBMSupplierBidStatus(string strBidStatus)
    {
        if (strBidStatus.Trim().Equals("Y"))
        {
            return "中标"; 
        }
        else if (strBidStatus.Trim().Equals("N"))
        {
            return "Unsuccessful Bid";
        }
        else
        {
            return LanguageHandle.GetWord("WeiKaiBiao").ToString().Trim();
        }
    }
}
