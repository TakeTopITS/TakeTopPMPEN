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

public partial class TTBMBidNoticeContent : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "预中标通知书", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            LoadBMBidPlanName();

            LoadBMBidNoticeContentList();
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

    protected void LoadBMBidPlanName()
    {
        string strHQL;
        //绑定招标计划名称T_BMBidPlan
        //    strHQL = "select * From T_BMBidPlan where ID not in (select BidPlanID from T_BMBidNoticeContent) and Datediff(day,BidStartDate,'" + DateTime.Now.ToString() + "')>=0 and Datediff(day,BidEndDate,'" + DateTime.Now.ToString() + "')<=0 Order By ID Desc";
        strHQL = "select * From T_BMBidPlan Order By ID Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidPlan");
        DL_BidPlanID.DataSource = ds;
        DL_BidPlanID.DataBind();
        DL_BidPlanID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    /// <summary>
    /// 获取招标计划实体
    /// </summary>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected BMBidPlan GetBMBidPlanModel(string strID)
    {
        string strHQL = " from BMBidPlan as bMBidPlan where bMBidPlan.ID = '" + strID.Trim() + "'";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        IList lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            return bMBidPlan;
        }
        else
            return null;
    }

    /// <summary>
    /// 新增或更新时，招标计划ID是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMBidNoticeContentBidPlanID(string strBMBidPlanId, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMBidNoticeContent Where BidPlanID='" + strBMBidPlanId + "' ";
        }
        else
            strHQL = "Select ID From T_BMBidNoticeContent Where BidPlanID='" + strBMBidPlanId + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidNoticeContent").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    protected void LoadBMBidNoticeContentList()
    {
        string strHQL;

        strHQL = "Select * From T_BMBidNoticeContent Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (BidWinContent like '%" + TextBox1.Text.Trim() + "%' or NoBidWinContent like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and BidPlanName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-NoticeDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-NoticeDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidNoticeContent");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }
    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text;

        if (strID == "")
        {
            Add();
        }
        else
        {
            Update();
        }
    }

    protected void Add()
    {
        if (DL_BidPlanID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFABJC") + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        if (IsBMBidNoticeContentBidPlanID(DL_BidPlanID.SelectedValue.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFAYFBZBWZBTZJC") + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        BMBidNoticeContentBLL bMBidNoticeContentBLL = new BMBidNoticeContentBLL();
        BMBidNoticeContent bMBidNoticeContent = new BMBidNoticeContent();

        bMBidNoticeContent.BidWinContent = TB_BidWinContent.Text.Trim();
        bMBidNoticeContent.NoBidWinContent = TB_NoBidWinContent.Text.Trim();
        bMBidNoticeContent.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMBidNoticeContent.BidPlanName = GetBMBidPlanName(bMBidNoticeContent.BidPlanID.ToString().Trim());
        bMBidNoticeContent.NoticeDate = DateTime.Parse(DateTime.Now.ToString());
        bMBidNoticeContent.EnterCode = strUserCode.Trim();

        try
        {
            bMBidNoticeContentBLL.AddBMBidNoticeContent(bMBidNoticeContent);
            LB_ID.Text = GetMaxBMBidNoticeContentID(bMBidNoticeContent).ToString();
            UpdateBMSupplierBidNoticeContent(bMBidNoticeContent);

            if (CBSend.Checked)
            {
                SendSupplierMsg(bMBidNoticeContent.BidPlanID.ToString());
            }
            if (cbSend_Email.Checked)
            {
                SendSupplierEmailMsg(bMBidNoticeContent.BidPlanID.ToString());
            }

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            LoadBMBidNoticeContentList();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void SendSupplierMsg(string strBidPlanID)
    {
        Msg msg = new Msg();
        string strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.BidPlanID = '" + strBidPlanID + "'";
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
            strHQL = "From BMSupplierBid as bMSupplierBid Where bMSupplierBid.AnnInvitationID='" + bMAnnInvitation.ID.ToString() + "' ";
            BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
            lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                for (int j = 0; j < lst.Count; j++)
                {
                    BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[j];
                    strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + bMSupplierBid.SupplierCode.ToString() + "' ";
                    BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                    lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                    if (lst.Count > 0 && lst != null)
                    {
                        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                        if ((bMSupplierBid.BidStatus.Trim().Equals("Y") || bMSupplierBid.BidStatus.Trim().Equals("N")) && !string.IsNullOrEmpty(bMSupplierBid.NoticeContent) && bMSupplierBid.NoticeContent.Trim() != "")
                        {
                            msg.SendMSM("Message", bMSupplierInfo.Code.Trim(), bMSupplierBid.NoticeContent.Trim(), strUserCode);
                        }
                        continue;
                    }
                }
            }
        }
    }

    protected void SendSupplierEmailMsg(string strBidPlanID)
    {
        Msg msg = new Msg();
        string strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.BidPlanID = '" + strBidPlanID + "'";
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
            strHQL = "From BMSupplierBid as bMSupplierBid Where bMSupplierBid.AnnInvitationID='" + bMAnnInvitation.ID.ToString() + "' ";
            BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
            lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                for (int j = 0; j < lst.Count; j++)
                {
                    BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[j];
                    strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + bMSupplierBid.SupplierCode.ToString() + "' ";
                    BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                    lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                    if (lst.Count > 0 && lst != null)
                    {
                        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                        if ((bMSupplierBid.BidStatus.Trim().Equals("Y") || bMSupplierBid.BidStatus.Trim().Equals("N")) && !string.IsNullOrEmpty(bMSupplierBid.NoticeContent) && bMSupplierBid.NoticeContent.Trim() != "")
                        {
                            msg.SendMail(bMSupplierInfo.Code.Trim(), LanguageHandle.GetWord("ZhongBiaoYuFouQingKuangTongZhi"), bMSupplierBid.NoticeContent.Trim(), strUserCode);
                        }
                        continue;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 新增时，获取表T_BMBidNoticeContent中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMBidNoticeContentID(BMBidNoticeContent bmbp)
    {
        string strHQL = "Select ID From T_BMBidNoticeContent where BidPlanID='" + bmbp.BidPlanID.ToString().Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidNoticeContent").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetBMBidPlanName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定招标计划名称
        strHQL = "From BMBidPlan as bMBidPlan Where bMBidPlan.ID='" + strID + "' ";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            return bMBidPlan.Name.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        if (DL_BidPlanID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFABJC") + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        if (IsBMBidNoticeContentBidPlanID(DL_BidPlanID.SelectedValue.Trim(), LB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFAYFBZBWZBTZJC") + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        string strHQL = "From BMBidNoticeContent as bMBidNoticeContent where bMBidNoticeContent.ID = '" + LB_ID.Text.Trim() + "'";
        BMBidNoticeContentBLL bMBidNoticeContentBLL = new BMBidNoticeContentBLL();
        IList lst = bMBidNoticeContentBLL.GetAllBMBidNoticeContents(strHQL);
        BMBidNoticeContent bMBidNoticeContent = (BMBidNoticeContent)lst[0];

        bMBidNoticeContent.BidWinContent = TB_BidWinContent.Text.Trim();
        bMBidNoticeContent.NoBidWinContent = TB_NoBidWinContent.Text.Trim();
        bMBidNoticeContent.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMBidNoticeContent.BidPlanName = GetBMBidPlanName(bMBidNoticeContent.BidPlanID.ToString().Trim());

        try
        {
            bMBidNoticeContentBLL.UpdateBMBidNoticeContent(bMBidNoticeContent, bMBidNoticeContent.ID);
            UpdateBMSupplierBidNoticeContent(bMBidNoticeContent);

            if (CBSend.Checked)
            {
                SendSupplierMsg(bMBidNoticeContent.BidPlanID.ToString());
            }
            if (cbSend_Email.Checked)
            {
                SendSupplierEmailMsg(bMBidNoticeContent.BidPlanID.ToString());
            }

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            LoadBMBidNoticeContentList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = LB_ID.Text.Trim();
        strHQL = "From BMBidNoticeContent as bMBidNoticeContent where bMBidNoticeContent.ID = '" + LB_ID.Text.Trim() + "'";
        BMBidNoticeContentBLL bMBidNoticeContentBLL = new BMBidNoticeContentBLL();
        IList lst = bMBidNoticeContentBLL.GetAllBMBidNoticeContents(strHQL);
        BMBidNoticeContent bMBidNoticeContent = (BMBidNoticeContent)lst[0];

        strHQL = "Delete From T_BMBidNoticeContent Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            DeleteBMSupplierBidNoticeContent(bMBidNoticeContent);

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            LoadBMBidNoticeContentList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;


                strHQL = "From BMBidNoticeContent as bMBidNoticeContent where bMBidNoticeContent.ID = '" + strID + "'";
                BMBidNoticeContentBLL bMBidNoticeContentBLL = new BMBidNoticeContentBLL();
                lst = bMBidNoticeContentBLL.GetAllBMBidNoticeContents(strHQL);
                BMBidNoticeContent bMBidNoticeContent = (BMBidNoticeContent)lst[0];

                LB_ID.Text = bMBidNoticeContent.ID.ToString().Trim();
                DL_BidPlanID.SelectedValue = bMBidNoticeContent.BidPlanID.ToString().Trim();
                TB_BidWinContent.Text = bMBidNoticeContent.BidWinContent.Trim();
                TB_NoBidWinContent.Text = bMBidNoticeContent.NoBidWinContent.Trim();

                //if (bMBidNoticeContent.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Delete.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Update.Visible = false;
                //    BT_Delete.Visible = false;
                //}
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }


            if (e.CommandName == "Delete")
            {
                Delete();

            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidNoticeContent");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMBidNoticeContentList();
    }

    protected void UpdateBMSupplierBidNoticeContent(BMBidNoticeContent bmbnc)
    {
        string strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.BidPlanID = '" + bmbnc.BidPlanID.ToString() + "'";
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
            strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.AnnInvitationID = '" + bMAnnInvitation.ID.ToString() + "'";
            BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
            lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[i];
                    if (bMSupplierBid.BidStatus.Trim().Equals("Y"))
                    {
                        bMSupplierBid.NoticeContent = bmbnc.BidWinContent.Trim();
                    }
                    else if (bMSupplierBid.BidStatus.Trim().Equals("N"))
                    {
                        bMSupplierBid.NoticeContent = bmbnc.NoBidWinContent.Trim();
                    }
                    else
                        bMSupplierBid.NoticeContent = "";
                    bMSupplierBidBLL.UpdateBMSupplierBid(bMSupplierBid, bMSupplierBid.ID);
                }
            }
        }
    }

    protected void DeleteBMSupplierBidNoticeContent(BMBidNoticeContent bmbnc)
    {
        string strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.BidPlanID = '" + bmbnc.BidPlanID.ToString() + "'";
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
            strHQL = "From BMSupplierBid as bMSupplierBid where bMSupplierBid.AnnInvitationID = '" + bMAnnInvitation.ID.ToString() + "'";
            BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
            lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[i];
                    bMSupplierBid.NoticeContent = "";
                    bMSupplierBidBLL.UpdateBMSupplierBid(bMSupplierBid, bMSupplierBid.ID);
                }
            }
        }
    }
}