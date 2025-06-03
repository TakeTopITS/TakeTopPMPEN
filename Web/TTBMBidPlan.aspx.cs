using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTBMBidPlan : System.Web.UI.Page
{
    private string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "招标方案", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_EnterDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_BidStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_BidEndDate.Text = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd");

            TB_EnterPer.Text = ShareClass.GetUserName(strUserCode);
            TB_EnterDepart.Text = GetUserDepartName(strUserCode);

            LoadBMBidType();

            LoadBMPurchaseApplicationName();

            LoadBMBidPlanList();

            LoadWZExpertList();

            ShareClass.LoadWFTemplate(strUserCode, DL_WFType.SelectedValue.Trim(), DL_TemName);
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

    protected void LoadBMPurchaseApplicationName()
    {
        string strHQL;
        IList lst;
        //绑定采购申请名称Status = "Qualified"
        strHQL = "From BMPurchaseApplication as bMPurchaseApplication Order By bMPurchaseApplication.ID Desc";
        BMPurchaseApplicationBLL bMPurchaseApplicationBLL = new BMPurchaseApplicationBLL();
        lst = bMPurchaseApplicationBLL.GetAllBMPurchaseApplications(strHQL);
        DL_PurchaseAppID.DataSource = lst;
        DL_PurchaseAppID.DataBind();
        DL_PurchaseAppID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void LoadBMBidPlanList()
    {
        string strHQL;

        strHQL = "Select * From T_BMBidPlan Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (EnterDepart like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or EnterPer like '%" + TextBox1.Text.Trim() + "%' " +
            "or BidAddress like '%" + TextBox1.Text.Trim() + "%' or BidRemark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and PurchaseAppName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-BidEndDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-BidStartDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidPlan");

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
        if (IsBMBidPlanName(TB_Name.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DateTime.Parse(DLC_BidStartDate.Text.Trim()) > DateTime.Parse(DLC_BidEndDate.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBKSRBNDYJSRCZSBJC").ToString().Trim() + "')", true);
            DLC_BidStartDate.Focus();
            return;
        }

        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        BMBidPlan bMBidPlan = new BMBidPlan();

        bMBidPlan.BidStartDate = DateTime.Parse(DLC_BidStartDate.Text.Trim());
        bMBidPlan.EnterDepart = TB_EnterDepart.Text.Trim();
        bMBidPlan.EnterPer = TB_EnterPer.Text.Trim();
        bMBidPlan.BidWay = DL_BidWay.SelectedValue.Trim();
        bMBidPlan.Name = TB_Name.Text.Trim();
        bMBidPlan.BidRemark = TB_BidRemark.Text.Trim();
        bMBidPlan.PurchaseAppID = int.Parse(DL_PurchaseAppID.SelectedValue.Trim());
        bMBidPlan.PurchaseAppName = GetBMPurchaseApplicationName(bMBidPlan.PurchaseAppID.ToString().Trim());
        bMBidPlan.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        bMBidPlan.BidType = ddl_BidType.SelectedValue.Trim();
        bMBidPlan.BidAddress = TB_BidAddress.Text.Trim();
        bMBidPlan.BidEndDate = DateTime.Parse(DLC_BidEndDate.Text.Trim());
        bMBidPlan.UserCodeList = lbl_UserCodeList.Text.Trim();
        bMBidPlan.EnterCode = strUserCode.Trim();
        bMBidPlan.PurchaseAppCode = TB_BMPurchaseApplicationCode.Text.Trim();
        bMBidPlan.BidLimitedPrice = NB_BidLimitedPrice.Amount;

        if (CB_IsEnginerringSupplier.Checked == true)
        {
            bMBidPlan.EnginerringSupplier = "YES";
        }
        else
        {
            bMBidPlan.EnginerringSupplier = "NO";
        }
        if (CB_IsInternationSupplier.Checked == true)
        {
            bMBidPlan.InternationSupplier = "YES";
        }
        else
        {
            bMBidPlan.InternationSupplier = "NO";
        }
        if (CB_IsRetailSupplier.Checked == true)
        {
            bMBidPlan.RetailSupplier = "YES";
        }
        else
        {
            bMBidPlan.RetailSupplier = "NO";
        }
        if (CB_IsStoreSupplier.Checked == true)
        {
            bMBidPlan.StoreSupplier = "YES";
        }
        else
        {
            bMBidPlan.StoreSupplier = "NO";
        }

        try
        {
            bMBidPlanBLL.AddBMBidPlan(bMBidPlan);
            LB_ID.Text = GetMaxBMBidPlanID(bMBidPlan).ToString();


            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            UpdateWZExpertWorkingPoint(bMBidPlan.UserCodeList.Trim());

            //依招标类型添加关联的工作流模板和文档模板
            ShareClass.AddRelatedWorkFlowTemplateByBMBidType(ddl_BidType.SelectedValue.Trim(), LB_ID.Text);

            LoadBMBidPlanList();
            LoadWZExpertList();

            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;

            LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "Other", int.Parse(LB_ID.Text.Trim()));

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    /// <summary>
    /// 新增或更新时，招标计划名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMBidPlanName(string strName, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMBidPlan Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select ID From T_BMBidPlan Where Name='" + strName + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidPlan").Tables[0];
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
    /// 新增时，获取表T_BMBidPlan中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMBidPlanID(BMBidPlan bmbp)
    {
        string strHQL = "Select ID From T_BMBidPlan where Name='" + bmbp.Name.Trim() + "' and EnterPer='" + bmbp.EnterPer.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidPlan").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetBMPurchaseApplicationName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定采购申请名称
        strHQL = "From BMPurchaseApplication as bMPurchaseApplication Where bMPurchaseApplication.ID='" + strID + "' ";
        BMPurchaseApplicationBLL bMPurchaseApplicationBLL = new BMPurchaseApplicationBLL();
        lst = bMPurchaseApplicationBLL.GetAllBMPurchaseApplications(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMPurchaseApplication bMPurchaseApplication = (BMPurchaseApplication)lst[0];
            return bMPurchaseApplication.Name.Trim();
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
        if (IsBMBidPlanName(TB_Name.Text.Trim(), LB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DateTime.Parse(DLC_BidStartDate.Text.Trim()) > DateTime.Parse(DLC_BidEndDate.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBKSRBNDYJSRCZSBJC").ToString().Trim() + "')", true);
            DLC_BidStartDate.Focus();
            return;
        }

        string strHQL = "From BMBidPlan as bMBidPlan where bMBidPlan.ID = '" + LB_ID.Text.Trim() + "'";
        BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
        IList lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
        BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
        string usercodelistold = bMBidPlan.UserCodeList.Trim();

        bMBidPlan.BidStartDate = DateTime.Parse(DLC_BidStartDate.Text.Trim());
        bMBidPlan.EnterDepart = TB_EnterDepart.Text.Trim();
        bMBidPlan.EnterPer = TB_EnterPer.Text.Trim();
        bMBidPlan.BidWay = DL_BidWay.SelectedValue.Trim();
        bMBidPlan.Name = TB_Name.Text.Trim();
        bMBidPlan.BidRemark = TB_BidRemark.Text.Trim();
        bMBidPlan.PurchaseAppID = int.Parse(DL_PurchaseAppID.SelectedValue.Trim());
        bMBidPlan.PurchaseAppName = GetBMPurchaseApplicationName(bMBidPlan.PurchaseAppID.ToString().Trim());
        bMBidPlan.EnterDate = DateTime.Parse(DLC_EnterDate.Text.Trim());
        bMBidPlan.BidType = ddl_BidType.SelectedValue.Trim();
        bMBidPlan.BidAddress = TB_BidAddress.Text.Trim();
        bMBidPlan.BidEndDate = DateTime.Parse(DLC_BidEndDate.Text.Trim());
        bMBidPlan.UserCodeList = lbl_UserCodeList.Text.Trim();
        bMBidPlan.PurchaseAppCode = TB_BMPurchaseApplicationCode.Text.Trim();
        bMBidPlan.BidLimitedPrice = NB_BidLimitedPrice.Amount;

        if (CB_IsEnginerringSupplier.Checked == true)
        {
            bMBidPlan.EnginerringSupplier = "YES";
        }
        else
        {
            bMBidPlan.EnginerringSupplier = "NO";
        }
        if (CB_IsInternationSupplier.Checked == true)
        {
            bMBidPlan.InternationSupplier = "YES";
        }
        else
        {
            bMBidPlan.InternationSupplier = "NO";
        }
        if (CB_IsRetailSupplier.Checked == true)
        {
            bMBidPlan.RetailSupplier = "YES";
        }
        else
        {
            bMBidPlan.RetailSupplier = "NO";
        }
        if (CB_IsStoreSupplier.Checked == true)
        {
            bMBidPlan.StoreSupplier = "YES";
        }
        else
        {
            bMBidPlan.StoreSupplier = "NO";
        }

        try
        {
            bMBidPlanBLL.UpdateBMBidPlan(bMBidPlan, bMBidPlan.ID);

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            UpdateWZExpertWorkingPoint(bMBidPlan.UserCodeList.Trim(), usercodelistold);

            //依招标类型添加关联的工作流模板和文档模板
            ShareClass.AddRelatedWorkFlowTemplateByBMBidType(ddl_BidType.SelectedValue.Trim(), LB_ID.Text);

            LoadBMBidPlanList();
            LoadWZExpertList();

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
        if (IsBMBidPlan(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBFAYBDYSCSB").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Delete From T_BMBidPlan Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            //删除招标文件
            strHQL = "Delete From T_BMBidFile Where BidPlanID='" + strCode + "' ";
            ShareClass.RunSqlCommand(strHQL);

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            BT_SubmitApply.Visible = false;

            LoadBMBidPlanList();
            LoadWZExpertList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    /// <summary>
    /// 删除时，判断招标计划是否已被调用，已调用返回true；否则返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMBidPlan(string strID)
    {
        string strHQL;
        bool flag = true;
        bool flag1 = true;
        bool flag2 = true;
        bool flag3 = true;
        bool flag4 = true;
        bool flag5 = true;
        strHQL = "Select ID From T_BMAnnClaFile Where BidPlanID='" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnClaFile").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }

        strHQL = "Select ID From T_BMAnnInvitation Where BidPlanID='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAnnInvitation").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag1 = true;
        }
        else
        {
            flag1 = false;
        }
        strHQL = "Select ID From T_BMBidAddendum Where BidPlanID='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidAddendum").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag2 = true;
        }
        else
        {
            flag2 = false;
        }
        strHQL = "Select ID From T_BMBidNoticeContent Where BidPlanID='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidNoticeContent").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag3 = true;
        }
        else
        {
            flag3 = false;
        }
        strHQL = "Select ID From T_BMOpenBidRecord Where BidPlanID='" + strID + "' ";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMOpenBidRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag4 = true;
        }
        else
        {
            flag4 = false;
        }

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = '" + DL_WFType.SelectedValue.Trim() + "' and workFlow.RelatedType='Other' and workFlow.RelatedID = '" + strID + "' ";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        IList lst = workFlowBLL.GetAllWorkFlows(strHQL);
        if (lst.Count > 0 && lst != null)
            flag5 = true;
        else
            flag5 = false;

        if (flag || flag1 || flag2 || flag3 || flag4 || flag5)
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
            strID = e.Item.Cells[3].Text.Trim();
            LB_ID.Text = strID;

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "From BMBidPlan as bMBidPlan where bMBidPlan.ID = '" + strID + "'";
            BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
            lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);

            BMBidPlan bMBidPlan = (BMBidPlan)lst[0];

            if (e.CommandName == "Update")
            {
                LB_ID.Text = bMBidPlan.ID.ToString().Trim();
                ddl_BidType.SelectedValue = bMBidPlan.BidType;
                DL_PurchaseAppID.SelectedValue = bMBidPlan.PurchaseAppID.ToString().Trim();
                DLC_EnterDate.Text = bMBidPlan.EnterDate.ToString("yyyy-MM-dd");
                TB_EnterPer.Text = bMBidPlan.EnterPer.Trim();
                DLC_BidStartDate.Text = bMBidPlan.BidStartDate.ToString("yyyy-MM-dd");
                TB_BidRemark.Text = bMBidPlan.BidRemark.Trim();
                TB_Name.Text = bMBidPlan.Name.Trim();
                TB_EnterDepart.Text = bMBidPlan.EnterDepart.Trim();
                DL_BidWay.SelectedValue = bMBidPlan.BidWay.Trim();
                TB_BidAddress.Text = bMBidPlan.BidAddress.Trim();
                DLC_BidEndDate.Text = bMBidPlan.BidEndDate.ToString("yyyy-MM-dd");
                lbl_UserCodeList.Text = bMBidPlan.UserCodeList.Trim();
                TB_UserCodeList.Text = GetWZExpertNameList(bMBidPlan.UserCodeList.Trim());
                TB_BMPurchaseApplicationCode.Text = bMBidPlan.PurchaseAppCode.Trim();
                NB_BidLimitedPrice.Amount = bMBidPlan.BidLimitedPrice;

                if (bMBidPlan.EnginerringSupplier.Trim() == "YES")
                {
                    CB_IsEnginerringSupplier.Checked = true;
                }
                else
                {
                    CB_IsEnginerringSupplier.Checked = false;
                }
                if (bMBidPlan.InternationSupplier.Trim() == "YES")
                {
                    CB_IsInternationSupplier.Checked = true;
                }
                else
                {
                    CB_IsInternationSupplier.Checked = false;
                }
                if (bMBidPlan.RetailSupplier.Trim() == "YES")
                {
                    CB_IsRetailSupplier.Checked = true;
                }
                else
                {
                    CB_IsRetailSupplier.Checked = false;
                }
                if (bMBidPlan.StoreSupplier.Trim() == "YES")
                {
                    CB_IsStoreSupplier.Checked = true;
                }
                else
                {
                    CB_IsStoreSupplier.Checked = false;
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                Delete();
            }

            if (e.CommandName == "Workflow")
            {
                if (bMBidPlan.EnterCode.Trim() == strUserCode.Trim())
                {
                    //BT_Update.Visible = true;
                    //BT_Delete.Visible = true;
                    //BT_Update.Enabled = true;
                    //BT_Delete.Enabled = true;

                    BT_SubmitApply.Visible = true;
                    BT_SubmitApply.Enabled = true;

                    LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "Other", int.Parse(LB_ID.Text.Trim()));

                    int intWLNumber = GetRelatedWorkFlowNumber(DL_WFType.SelectedValue.Trim(), "Other", LB_ID.Text.Trim());

                    if (intWLNumber > 0)
                    {
                        BT_SubmitApply.Visible = false;
                    }
                }
                else
                {
                    //BT_Update.Visible = false;
                    //BT_Delete.Visible = false;

                    BT_SubmitApply.Visible = false;

                    LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "Other", int.Parse(LB_ID.Text.Trim()));
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWorkflow','false') ", true);

            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidPlan");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMBidPlanList();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        LoadWZExpertList();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        StringBuilder sbName = new StringBuilder();
        StringBuilder sbCode = new StringBuilder();
        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            CheckBox cbSelect = (CheckBox)DataGrid1.Items[i].FindControl("cbSelect");
            HiddenField hfID = (HiddenField)DataGrid1.Items[i].FindControl("hfID");
            HiddenField hfName = (HiddenField)DataGrid1.Items[i].FindControl("hfName");
            HiddenField hfExpertCode = (HiddenField)DataGrid1.Items[i].FindControl("hfExpertCode");
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
        TB_UserCodeList.Text = sbName.ToString().Trim();
        lbl_UserCodeList.Text = sbCode.ToString().Trim();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadWZExpertList()
    {
        string strType = ddl_BidType.SelectedValue.Trim();
        //if (strType == "工程招标")
        //    strType = "EngineeringBiddingExpert";
        //else if (strType == "物资招标")
        //    strType = "MaterialBiddingExpert";
        //else
        //    strType = "OtherBiddingExpert";
        string strHQL;

        strHQL = "Select * From T_WZExpert ";
        //strHQL += " Type ='" + strType + "' ";
        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " Where  (ExpertCode like '%" + txt_SupplierInfo.Text.Trim() + "%' or Name like '%" + txt_SupplierInfo.Text.Trim() + "%' or WorkUnit like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
            "or Job like '%" + txt_SupplierInfo.Text.Trim() + "%' or JobTitle like '%" + txt_SupplierInfo.Text.Trim() + "%' or Phone like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
            "or ExpertType like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected string GetWZExpertNameList(string strUserIdList)
    {
        if (strUserIdList.Trim() == "" || string.IsNullOrEmpty(strUserIdList))
        {
            return "";
        }
        else
        {
            StringBuilder sbName = new StringBuilder();
            string strHQL = "Select * From T_WZExpert Where ID in (" + strUserIdList + ") ";
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
            return sbName.ToString().Trim();
        }
    }

    /// <summary>
    /// 当选择专家，新增招标方案时，专家的工作点数累计加1
    /// </summary>
    /// <param name="struseridlist"></param>
    protected void UpdateWZExpertWorkingPoint(string struseridlist)
    {
        if (struseridlist.Trim() != "" && !string.IsNullOrEmpty(struseridlist))
        {
            string strHQL = "Select * From T_WZExpert Where ID in (" + struseridlist + ") Order By ID ";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");
            if (ds.Tables[0].Rows.Count > 0 && ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    WZExpertBLL wZExpertBLL = new WZExpertBLL();
                    strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + ds.Tables[0].Rows[i]["ID"].ToString() + "' ";
                    IList lst = wZExpertBLL.GetAllWZExperts(strHQL);
                    WZExpert wZExpert = (WZExpert)lst[0];
                    wZExpert.WorkingPoint = wZExpert.WorkingPoint + 1;
                    wZExpertBLL.UpdateWZExpert(wZExpert, wZExpert.ID);
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// 当选择专家，更新招标方案时，新增加的专家，其工作点数累计加1
    /// </summary>
    /// <param name="struseridlistNew"></param>
    /// <param name="struseridlistOld"></param>
    protected void UpdateWZExpertWorkingPoint(string struseridlistNew, string struseridlistOld)
    {
        if (struseridlistNew.Trim() == "" && string.IsNullOrEmpty(struseridlistNew))
        {
        }
        else
        {
            if (struseridlistOld.Trim() == "" || string.IsNullOrEmpty(struseridlistOld))
            {
                string strHQL = "Select * From T_WZExpert Where ID in (" + struseridlistNew + ") Order By ID ";
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");
                if (ds.Tables[0].Rows.Count > 0 && ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        WZExpertBLL wZExpertBLL = new WZExpertBLL();
                        strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + ds.Tables[0].Rows[i]["ID"].ToString() + "' ";
                        IList lst = wZExpertBLL.GetAllWZExperts(strHQL);
                        WZExpert wZExpert = (WZExpert)lst[0];
                        wZExpert.WorkingPoint = int.Parse(ds.Tables[0].Rows[i]["WorkingPoint"].ToString()) + 1;
                        wZExpertBLL.UpdateWZExpert(wZExpert, wZExpert.ID);
                        continue;
                    }
                }
            }
            else
            {
                string strHQL = "Select * From T_WZExpert Where ID in (" + struseridlistNew + ") and ID not in (" + struseridlistOld + ") Order By ID ";
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");
                if (ds.Tables[0].Rows.Count > 0 && ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        WZExpertBLL wZExpertBLL = new WZExpertBLL();
                        strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + ds.Tables[0].Rows[i]["ID"].ToString() + "' ";
                        IList lst = wZExpertBLL.GetAllWZExperts(strHQL);
                        WZExpert wZExpert = (WZExpert)lst[0];
                        wZExpert.WorkingPoint = int.Parse(ds.Tables[0].Rows[i]["WorkingPoint"].ToString()) + 1;
                        wZExpertBLL.UpdateWZExpert(wZExpert, wZExpert.ID);
                        continue;
                    }
                }
            }
        }
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

    protected string SubmitApply()
    {
        string strCmdText, strID, strWLID, strXMLFileName, strXMLFile2, strTemName;

        strID = LB_ID.Text.Trim();
        strWLID = "0";

        strTemName = DL_TemName.SelectedValue.Trim();
        if (strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWLCMBBNWKJC").ToString().Trim() + "')", true);
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
            workFlow.Description = TB_BidRemark.Text.Trim();
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

                strCmdText = "select * from T_BMBidPlan where ID = " + strID;

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_BMBidPlan", strXMLFile2);

                LoadRelatedWL(DL_WFType.SelectedValue.Trim(), "Other", int.Parse(strID));

                //DL_Status.SelectedValue = "InProgress";

                //BT_Update.Visible = false;
                //BT_Delete.Visible = false;

                BT_SubmitApply.Visible = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZCG").ToString().Trim() + "')", true);
            }
            catch
            {
                strWLID = "0";

                BT_SubmitApply.Visible = true;
                BT_SubmitApply.Enabled = true;
                //BT_Update.Visible = true;
                //BT_Update.Enabled = true;
                //BT_Delete.Visible = true;
                //BT_Delete.Enabled = true;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZQiaoBiaoFangAnShengChengLang").ToString().Trim() + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true); 
            }

            LoadBMBidPlanList();

            LoadWZExpertList();
        }
        catch
        {
            strWLID = "0";

            BT_SubmitApply.Visible = true;
            BT_SubmitApply.Enabled = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZQiaoBiaoFangAnShengChengLang").ToString().Trim() + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true); 
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWorkflow','false') ", true);
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowWorkflow','false') ", true);
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

    protected void ddl_BidType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWZExpertList();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strtype = DL_WFType.SelectedValue.Trim();

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = '" + strtype + "'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
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

        LoadRelatedWL(strtype, "Other", int.Parse(LB_ID.Text.Trim()));

        int intWLNumber = GetRelatedWorkFlowNumber(strtype, "Other", LB_ID.Text.Trim());

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

    protected void DL_PurchaseAppID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From BMPurchaseApplication as bMPurchaseApplication where bMPurchaseApplication.ID = '" + DL_PurchaseAppID.SelectedValue.Trim() + "'";
        BMPurchaseApplicationBLL bMPurchaseApplicationBLL = new BMPurchaseApplicationBLL();
        IList lst = bMPurchaseApplicationBLL.GetAllBMPurchaseApplications(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMPurchaseApplication bMPurchaseApplication = (BMPurchaseApplication)lst[0];
            TB_BMPurchaseApplicationCode.Text = bMPurchaseApplication.Code.Trim();
            TB_Name.Text = bMPurchaseApplication.Name.Trim();
        }
        else
        {
            TB_BMPurchaseApplicationCode.Text = "";
            TB_Name.Text = "";
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadBMBidType()
    {
        string strHQL;

        strHQL = "Select * From T_BMBidType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidType");

        ddl_BidType.DataSource = ds;
        ddl_BidType.DataBind();
    }

}
