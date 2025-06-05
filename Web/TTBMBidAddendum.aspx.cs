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

public partial class TTBMBidAddendum : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "招标补遗信息", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_AddendumDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_Addendumer.Text = ShareClass.GetUserName(strUserCode);

            LoadBMBidPlanName();

            LoadBMBidAddendumList();
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
        //   strHQL = "select * From T_BMBidPlan where Datediff(day,BidStartDate,'" + DateTime.Now.ToString() + "')>=0 and Datediff(day,BidEndDate,'" + DateTime.Now.ToString() + "')<=0 Order By ID Desc";
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

    protected void LoadBMBidAddendumList()
    {
        string strHQL;

        strHQL = "Select * From T_BMBidAddendum Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Addendumer like '%" + TextBox1.Text.Trim() + "%' or AddendumContent like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and BidPlanName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-AddendumDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-AddendumDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidAddendum");

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBJHBJC") + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        BMBidPlan bMBidPlan = GetBMBidPlanModel(DL_BidPlanID.SelectedValue.Trim());
        if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < bMBidPlan.BidStartDate || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > bMBidPlan.BidEndDate)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDRBZZBJHYXNJC") + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        BMBidAddendumBLL bMBidAddendumBLL = new BMBidAddendumBLL();
        BMBidAddendum bMBidAddendum = new BMBidAddendum();

        bMBidAddendum.Addendumer = TB_Addendumer.Text.Trim();
        bMBidAddendum.AddendumContent = TB_AddendumContent.Text.Trim();
        bMBidAddendum.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMBidAddendum.BidPlanName = GetBMBidPlanName(bMBidAddendum.BidPlanID.ToString().Trim());
        bMBidAddendum.AddendumDate = DateTime.Parse(DLC_AddendumDate.Text.Trim());
        bMBidAddendum.EnterCode = strUserCode.Trim();

        try
        {
            bMBidAddendumBLL.AddBMBidAddendum(bMBidAddendum);
            LB_ID.Text = GetMaxBMBidAddendumID(bMBidAddendum).ToString();

            LoadBMBidAddendumList();

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 新增时，获取表T_BMBidAddendum中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMBidAddendumID(BMBidAddendum bmbp)
    {
        string strHQL = "Select ID From T_BMBidAddendum where Addendumer='" + bmbp.Addendumer.Trim() + "' and BidPlanID='" + bmbp.BidPlanID.ToString().Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidAddendum").Tables[0];
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBJHBJC") + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        BMBidPlan bMBidPlan = GetBMBidPlanModel(DL_BidPlanID.SelectedValue.Trim());
        if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < bMBidPlan.BidStartDate || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > bMBidPlan.BidEndDate)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDRBZZBJHYXNJC") + "')", true);
            DL_BidPlanID.Focus();
            return;
        }
        string strHQL = "From BMBidAddendum as bMBidAddendum where bMBidAddendum.ID = '" + LB_ID.Text.Trim() + "'";
        BMBidAddendumBLL bMBidAddendumBLL = new BMBidAddendumBLL();
        IList lst = bMBidAddendumBLL.GetAllBMBidAddendums(strHQL);
        BMBidAddendum bMBidAddendum = (BMBidAddendum)lst[0];

        bMBidAddendum.Addendumer = TB_Addendumer.Text.Trim();
        bMBidAddendum.AddendumContent = TB_AddendumContent.Text.Trim();
        bMBidAddendum.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMBidAddendum.BidPlanName = GetBMBidPlanName(bMBidAddendum.BidPlanID.ToString().Trim());
        bMBidAddendum.AddendumDate = DateTime.Parse(DLC_AddendumDate.Text.Trim());

        try
        {
            bMBidAddendumBLL.UpdateBMBidAddendum(bMBidAddendum, bMBidAddendum.ID);

            LoadBMBidAddendumList();
            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

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

        strHQL = "Delete From T_BMBidAddendum Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadBMBidAddendumList();
            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

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

                strHQL = "From BMBidAddendum as bMBidAddendum where bMBidAddendum.ID = '" + strID + "'";
                BMBidAddendumBLL bMBidAddendumBLL = new BMBidAddendumBLL();
                lst = bMBidAddendumBLL.GetAllBMBidAddendums(strHQL);
                BMBidAddendum bMBidAddendum = (BMBidAddendum)lst[0];

                LB_ID.Text = bMBidAddendum.ID.ToString().Trim();
                DL_BidPlanID.SelectedValue = bMBidAddendum.BidPlanID.ToString().Trim();
                DLC_AddendumDate.Text = bMBidAddendum.AddendumDate.ToString("yyyy-MM-dd");
                TB_Addendumer.Text = bMBidAddendum.Addendumer.Trim();
                TB_AddendumContent.Text = bMBidAddendum.AddendumContent.Trim();
                //if (bMBidAddendum.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidAddendum");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMBidAddendumList();
    }
}