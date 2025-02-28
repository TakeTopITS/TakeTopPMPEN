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

public partial class TTBMAnnInvitation : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "招标邀请函", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_EnterDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_EnterPer.Text = ShareClass.GetUserName(strUserCode);
            TB_EnterUnit.Text = GetUserDepartName(strUserCode);

            HL_MakeProject.Visible = false;

            LoadBMBidPlanName();

            LoadBMAnnInvitationList();
            LoadBMSupplierInfoList();
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
    protected bool IsBMAnnInvitationBidPlanID(string strBMBidPlanId, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMAnnInvitation Where BidPlanID='" + strBMBidPlanId + "' ";
        }
        else
            strHQL = "Select ID From T_BMAnnInvitation Where BidPlanID='" + strBMBidPlanId + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation").Tables[0];
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

    protected void LoadBMAnnInvitationList()
    {
        string strHQL;

        strHQL = "Select * From T_BMAnnInvitation Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (EnterUnit like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or EnterPer like '%" + TextBox1.Text.Trim() + "%' " +
            "or BidWay like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and BidPlanName like '%" + TextBox2.Text.Trim() + "%' ";
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
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsBMAnnInvitationName(TB_Name.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_BidPlanID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFABJC").ToString().Trim() + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        if (cbSend.Checked)
        {
            if (TB_PhoneRemark.Text.Trim() == "" || string.IsNullOrEmpty(TB_PhoneRemark.Text))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSDXTZNRBTJC").ToString().Trim() + "')", true);
                TB_PhoneRemark.Focus();
                return;
            }
        }
        if (cbSend_Email.Checked)
        {
            if (TB_EmailRemark.Text.Trim() == "" || string.IsNullOrEmpty(TB_EmailRemark.Text))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSYJTZNRBTJC").ToString().Trim() + "')", true);
                TB_EmailRemark.Focus();
                return;
            }
        }
        BMBidPlan bMBidPlan = GetBMBidPlanModel(DL_BidPlanID.SelectedValue.Trim());
        //if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < bMBidPlan.BidStartDate || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > bMBidPlan.BidEndDate)
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDRBZZBFAYXNJC").ToString().Trim()+"')", true);
        //    DL_BidPlanID.Focus();
        //    return;
        //}
        if (IsBMAnnInvitationBidPlanID(DL_BidPlanID.SelectedValue.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFAYFBZBGGJC").ToString().Trim() + "')", true);
            DL_BidPlanID.Focus();
            return;
        }

        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        BMAnnInvitation bMAnnInvitation = new BMAnnInvitation();

        bMAnnInvitation.EnterUnit = TB_EnterUnit.Text.Trim();
        bMAnnInvitation.EnterPer = TB_EnterPer.Text.Trim();
        bMAnnInvitation.BidWay = DL_BidWay.SelectedValue.Trim();
        bMAnnInvitation.Name = TB_Name.Text.Trim();
        bMAnnInvitation.Remark = TB_Remark.Text.Trim();
        bMAnnInvitation.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMAnnInvitation.BidPlanName = GetBMBidPlanName(bMAnnInvitation.BidPlanID.ToString().Trim());
        bMAnnInvitation.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        bMAnnInvitation.BidObjects = lbl_SupplierId.Text.Trim(); //TB_BidObjects.Text.Trim();
        bMAnnInvitation.EnterCode = strUserCode.Trim();
        bMAnnInvitation.PhoneList = TB_LSPhoneList.Text.Trim();
        bMAnnInvitation.PhoneRemark = TB_PhoneRemark.Text.Trim();
        bMAnnInvitation.EmailRemark = TB_EmailRemark.Text.Trim();
        bMAnnInvitation.ResRemark = TB_ResRemark.Text.Trim();

        if (lbl_SupplierId.Text.Trim() == "" || string.IsNullOrEmpty(lbl_SupplierId.Text.Trim()) || lbl_SupplierId.Text.Trim() == "0")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBDXBJC").ToString().Trim() + "')", true);
            TB_BidObjects.Focus();
            return;
        }
        try
        {
            bMAnnInvitationBLL.AddBMAnnInvitation(bMAnnInvitation);
            LB_ID.Text = GetMaxBMAnnInvitationID(bMAnnInvitation).ToString();

            HL_MakeProject.Visible = true;
            HL_MakeProject.NavigateUrl = "TTMakeProjectFromOther.aspx?RelatedType=tender&RelatedID=" + LB_ID.Text.Trim();

            //BT_Delete.Visible = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            LoadBMAnnInvitationList();
            LoadBMSupplierInfoList();

            //选择是否发送短信通知
            if (cbSend.Checked)
            {
                SendSupplierMsg(bMAnnInvitation.BidObjects.Trim(), bMAnnInvitation.PhoneRemark.Trim());
                SendPhoneMsg(bMAnnInvitation.PhoneList.Trim(), bMAnnInvitation.PhoneRemark.Trim());
            }
            if (cbSend_Email.Checked)
            {
                SendSupplierEmailMsg(bMAnnInvitation.BidObjects.Trim(), bMAnnInvitation.EmailRemark.Trim());
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + ex.Message.ToString() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 新增或更新时，招标公告/邀请函名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMAnnInvitationName(string strName, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMAnnInvitation Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select ID From T_BMAnnInvitation Where Name='" + strName + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation").Tables[0];
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

    /// <summary>
    /// 新增时，获取表T_BMAnnInvitation中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMAnnInvitationID(BMAnnInvitation bmbp)
    {
        string strHQL = "Select ID From T_BMAnnInvitation where Name='" + bmbp.Name.Trim() + "' and EnterPer='" + bmbp.EnterPer.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation").Tables[0];
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
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsBMAnnInvitationName(TB_Name.Text.Trim(), LB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_BidPlanID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFABJC").ToString().Trim() + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        if (cbSend.Checked)
        {
            if (TB_PhoneRemark.Text.Trim() == "" || string.IsNullOrEmpty(TB_PhoneRemark.Text))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSDXTZNRBTJC").ToString().Trim() + "')", true);
                TB_PhoneRemark.Focus();
                return;
            }
        }
        if (cbSend_Email.Checked)
        {
            if (TB_EmailRemark.Text.Trim() == "" || string.IsNullOrEmpty(TB_EmailRemark.Text))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSYJTZNRBTJC").ToString().Trim() + "')", true);
                TB_EmailRemark.Focus();
                return;
            }
        }
        BMBidPlan bMBidPlan = GetBMBidPlanModel(DL_BidPlanID.SelectedValue.Trim());
        //if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < bMBidPlan.BidStartDate || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > bMBidPlan.BidEndDate)
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDRBZZBFAYXNJC").ToString().Trim()+"')", true);
        //    DL_BidPlanID.Focus();
        //    return;
        //}
        if (IsBMAnnInvitationBidPlanID(DL_BidPlanID.SelectedValue.Trim(), LB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFAYFBZBGGJC").ToString().Trim() + "')", true);
            DL_BidPlanID.Focus();
            return;
        }

        string strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.ID = '" + LB_ID.Text.Trim() + "'";
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];

        bMAnnInvitation.EnterUnit = TB_EnterUnit.Text.Trim();
        bMAnnInvitation.EnterPer = TB_EnterPer.Text.Trim();
        bMAnnInvitation.BidWay = DL_BidWay.SelectedValue.Trim();
        bMAnnInvitation.Name = TB_Name.Text.Trim();
        bMAnnInvitation.Remark = TB_Remark.Text.Trim();
        bMAnnInvitation.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMAnnInvitation.BidPlanName = GetBMBidPlanName(bMAnnInvitation.BidPlanID.ToString().Trim());
        bMAnnInvitation.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        bMAnnInvitation.BidObjects = lbl_SupplierId.Text.Trim(); //TB_BidObjects.Text.Trim();
        bMAnnInvitation.PhoneList = TB_LSPhoneList.Text.Trim();
        bMAnnInvitation.PhoneRemark = TB_PhoneRemark.Text.Trim();
        bMAnnInvitation.EmailRemark = TB_EmailRemark.Text.Trim();
        bMAnnInvitation.ResRemark = TB_ResRemark.Text.Trim();

        if (lbl_SupplierId.Text.Trim() == "" || string.IsNullOrEmpty(lbl_SupplierId.Text.Trim()) || lbl_SupplierId.Text.Trim() == "0")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBDXBJC").ToString().Trim() + "')", true);
            TB_BidObjects.Focus();
            return;
        }
        try
        {
            bMAnnInvitationBLL.UpdateBMAnnInvitation(bMAnnInvitation, bMAnnInvitation.ID);

            //选择是否发送短信通知
            if (cbSend.Checked)
            {
                //SendSupplierMsg(bMAnnInvitation.BidObjects.Trim(), bMAnnInvitation.Remark.Trim());
                SendSupplierMsg(bMAnnInvitation.BidObjects.Trim(), bMAnnInvitation.PhoneRemark.Trim());
                SendPhoneMsg(bMAnnInvitation.PhoneList.Trim(), bMAnnInvitation.PhoneRemark.Trim());
            }
            if (cbSend_Email.Checked)
            {
                SendSupplierEmailMsg(bMAnnInvitation.BidObjects.Trim(), bMAnnInvitation.EmailRemark.Trim());
            }

            HL_MakeProject.Visible = true;
            HL_MakeProject.NavigateUrl = "TTMakeProjectFromOther.aspx?RelatedType=tender&RelatedID=" + LB_ID.Text.Trim();

            //BT_Delete.Visible = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            LoadBMAnnInvitationList();
            LoadBMSupplierInfoList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = LB_ID.Text.Trim();
        if (IsBMAnnInvitation(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBYHYBDYSCSB").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Delete From T_BMAnnInvitation Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            HL_MakeProject.Visible = false;

            //BT_Delete.Visible = false;
            //BT_Update.Visible = false;

            LoadBMAnnInvitationList();
            LoadBMSupplierInfoList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    /// <summary>
    /// 删除时，判断招标公告/邀请函是否已被调用，已调用返回true；否则返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMAnnInvitation(string strID)
    {
        string strHQL;
        bool flag = true;
        bool flag1 = true;
        bool flag2 = true;

        strHQL = "Select ID From T_BMSupplierBid Where AnnInvitationID='" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBid").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }

        strHQL = "Select ID From T_BMSupplierBidRecord Where AnnInvitationID='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierBidRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag1 = true;
        }
        else
        {
            flag1 = false;
        }

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = 'BidRecommendation' and workFlow.RelatedType='Other' and workFlow.RelatedID = '" + strID + "' ";   //ChineseWord
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        IList lst = workFlowBLL.GetAllWorkFlows(strHQL);
        if (lst.Count > 0 && lst != null)
            flag2 = true;
        else
            flag2 = false;

        if (flag || flag1 || flag2)
            return true;
        else
            return false;
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



                strHQL = "From BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.ID = '" + strID + "'";
                BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
                lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
                BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];

                LB_ID.Text = bMAnnInvitation.ID.ToString().Trim();
                DL_BidPlanID.SelectedValue = bMAnnInvitation.BidPlanID.ToString().Trim();
                DLC_EnterDate.Text = bMAnnInvitation.EnterDate.ToString("yyyy-MM-dd");
                TB_EnterPer.Text = bMAnnInvitation.EnterPer.Trim();
                TB_Remark.Text = bMAnnInvitation.Remark.Trim();
                TB_Name.Text = bMAnnInvitation.Name.Trim();
                TB_EnterUnit.Text = bMAnnInvitation.EnterUnit.Trim();
                DL_BidWay.SelectedValue = bMAnnInvitation.BidWay.Trim();
                lbl_SupplierId.Text = bMAnnInvitation.BidObjects.Trim();
                TB_BidObjects.Text = GetSupplierNameList(bMAnnInvitation.BidObjects.Trim());
                TB_LSPhoneList.Text = bMAnnInvitation.PhoneList.Trim();
                TB_PhoneRemark.Text = bMAnnInvitation.PhoneRemark.Trim();
                TB_ResRemark.Text = string.IsNullOrEmpty(bMAnnInvitation.ResRemark) ? "" : bMAnnInvitation.ResRemark.Trim();

                //if (bMAnnInvitation.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Delete.Visible = true;
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;

                //    HL_MakeProject.Visible = true;
                //    HL_MakeProject.NavigateUrl = "TTMakeProjectFromOther.aspx?RelatedType=tender&RelatedID=" + LB_ID.Text.Trim();
                //}
                //else
                //{
                //    BT_Delete.Visible = false;
                //    BT_Update.Visible = false;

                //    HL_MakeProject.Visible = false;
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMAnnInvitationList();
    }

    protected void DL_BidPlanID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        //绑定招标方式
        strHQL = "From BMBidPlan as bMBidPlan Where bMBidPlan.ID='" + DL_BidPlanID.SelectedValue.Trim() + "' ";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
            DL_BidWay.SelectedValue = bMBidPlan.BidWay.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        StringBuilder sbName = new StringBuilder();
        StringBuilder sbId = new StringBuilder();
        StringBuilder sbPhoneNumber = new StringBuilder();
        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            CheckBox cbSelect = (CheckBox)DataGrid1.Items[i].FindControl("cbSelect");
            HiddenField hfID = (HiddenField)DataGrid1.Items[i].FindControl("hfID");
            HiddenField hfName = (HiddenField)DataGrid1.Items[i].FindControl("hfName");
            HiddenField hfPhoneNumber = (HiddenField)DataGrid1.Items[i].FindControl("hfPhoneNumber");
            if (cbSelect != null && hfID != null)
            {
                if (cbSelect.Checked)
                {
                    string subName = hfName.Value;
                    if (subName != null)
                    {
                        sbName.AppendFormat("{0}", subName);
                        sbId.AppendFormat("{0}", hfID.Value);
                        sbPhoneNumber.AppendFormat("{0}", hfPhoneNumber.Value);
                    }
                    sbName.Append(",");
                    sbId.Append(",");
                    sbPhoneNumber.Append(",");
                }
            }
        }
        if (sbName.Length > 0 && sbId.Length > 0 && sbPhoneNumber.Length > 0)
        {
            sbName.Remove(sbName.Length - 1, 1);
            sbId.Remove(sbId.Length - 1, 1);
            sbPhoneNumber.Remove(sbPhoneNumber.Length - 1, 1);
        }
        TB_BidObjects.Text = sbName.ToString().Trim();
        lbl_SupplierId.Text = sbId.ToString().Trim();
        TB_LSPhoneList.Text = sbPhoneNumber.ToString().Trim();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    /// <summary>
    /// 根据承包商ID，邀请函内容，给承包商发送短信通知
    /// </summary>
    /// <param name="strSupplierIdList"></param>
    /// <param name="strContent"></param>
    protected void SendSupplierMsg(string strSupplierIdList, string strContent)
    {
        Msg msg = new Msg();
        if (!string.IsNullOrEmpty(strSupplierIdList) && strSupplierIdList.Trim().Length > 0)
        {
            if (strSupplierIdList.Trim().Contains(","))//多个
            {
                string[] tempId = strSupplierIdList.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tempId.Length; i++)
                {
                    string strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + tempId[i] + "' ";
                    BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                    IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                    if (lst.Count > 0 && lst != null)
                    {
                        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                        //msg.SendMSM("Message",bMSupplierInfo.Code.Trim(), "招标邀请函内容如下：" + strContent + "请及时登录系统，进行投标！", strUserCode);
                        msg.SendMSM("Message", bMSupplierInfo.Code.Trim(), strContent, strUserCode);
                        continue;
                    }
                }
            }
            else//一个
            {
                string strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + strSupplierIdList.Trim() + "' ";
                BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                if (lst.Count > 0 && lst != null)
                {
                    BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                    msg.SendMSM("Message", bMSupplierInfo.Code.Trim(), strContent, strUserCode);
                }
            }
        }
    }

    /// <summary>
    /// 发送Email邮件
    /// </summary>
    /// <param name="strSupplierIdList"></param>
    /// <param name="strContent"></param>
    protected void SendSupplierEmailMsg(string strSupplierIdList, string strContent)
    {
        Msg msg = new Msg();
        if (!string.IsNullOrEmpty(strSupplierIdList) && strSupplierIdList.Trim().Length > 0)
        {
            if (strSupplierIdList.Trim().Contains(","))//多个
            {
                string[] tempId = strSupplierIdList.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tempId.Length; i++)
                {
                    string strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + tempId[i] + "' ";
                    BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                    IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                    if (lst.Count > 0 && lst != null)
                    {
                        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                        msg.SendMail(bMSupplierInfo.Code.Trim(), LanguageHandle.GetWord("QiaoBiaoYaoQingHanTongZhi").ToString().Trim(), strContent, strUserCode);
                        continue;
                    }
                }
            }
            else//一个
            {
                string strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + strSupplierIdList.Trim() + "' ";
                BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                IList lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                if (lst.Count > 0 && lst != null)
                {
                    BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                    msg.SendMail(bMSupplierInfo.Code.Trim(), LanguageHandle.GetWord("QiaoBiaoYaoQingHanTongZhi").ToString().Trim(), strContent, strUserCode);
                }
            }
        }
    }

    /// <summary>
    /// 根据输入的手机号码发送短信
    /// </summary>
    /// <param name="strPhoneList"></param>
    /// <param name="strContent"></param>
    protected void SendPhoneMsg(string strPhoneList, string strContent)
    {
        Msg msg = new Msg();
        if (!string.IsNullOrEmpty(strPhoneList) && strPhoneList.Trim().Length > 0)
        {
            if (strPhoneList.Trim().Contains("/"))//多个
            {
                string[] tempId = strPhoneList.Trim().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tempId.Length; i++)
                {
                    msg.SendPhoneMSMBySP(tempId[i].ToString().Trim(), strContent, strUserCode);
                    continue;
                }
            }
            else//一个
            {
                msg.SendPhoneMSMBySP(strPhoneList.Trim(), strContent, strUserCode);
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        LoadBMSupplierInfoList();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadBMSupplierInfoList()
    {
        string strHQL = "Select * From T_BMSupplierInfo Where Status='Qualified' ";

        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " and (Code like '%" + txt_SupplierInfo.Text.Trim() + "%' or Name like '%" + txt_SupplierInfo.Text.Trim() + "%' or CompanyFor like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
            "or CompanyType like '%" + txt_SupplierInfo.Text.Trim() + "%' or Address like '%" + txt_SupplierInfo.Text.Trim() + "%' or SupplyScope like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
            "or Qualification like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected string GetSupplierNameList(string strSupplierIdList)
    {
        StringBuilder sbName = new StringBuilder();
        string strHQL = "Select * From T_BMSupplierInfo Where ID in (" + strSupplierIdList + ") ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMSupplierInfo");
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
        return sbName.ToString().Trim();
    }
}
