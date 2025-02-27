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

public partial class TTBMOpenBidRecord : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_OpenBidDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_OpenBidder.Text = ShareClass.GetUserName(strUserCode);

            LoadBMBidPlanName();

            LoadBMOpenBidRecordList();
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
   //     strHQL = "select * From T_BMBidPlan where ID not in (select BidPlanID from T_BMOpenBidRecord) and Datediff(day,BidStartDate,'" + DateTime.Now.ToString() + "')>=0 and " +
   //         "Datediff(day,BidEndDate,'" + DateTime.Now.ToString() + "')<=0 Order By ID Desc";
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
    protected bool IsBMOpenBidRecordBidPlanID(string strBMBidPlanId, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMOpenBidRecord Where BidPlanID='" + strBMBidPlanId + "' ";
        }
        else
            strHQL = "Select ID From T_BMOpenBidRecord Where BidPlanID='" + strBMBidPlanId + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMOpenBidRecord").Tables[0];
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

    protected void LoadBMOpenBidRecordList()
    {
        string strHQL;

        strHQL = "Select * From T_BMOpenBidRecord Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Name like '%" + TextBox1.Text.Trim() + "%' or OpenBidder like '%" + TextBox1.Text.Trim() + "%' or OpenBidRemark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and BidPlanName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-OpenBidDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-OpenBidDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMOpenBidRecord");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim()+"')", true);
            TB_Name.Focus();
            return;
        }
        if (IsBMOpenBidRecordName(TB_Name.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim()+"')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_BidPlanID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZBJHBJC").ToString().Trim()+"')", true);
            DL_BidPlanID.Focus();
            return;
        }
        //BMBidPlan bMBidPlan = GetBMBidPlanModel(DL_BidPlanID.SelectedValue.Trim());
        //if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < bMBidPlan.BidStartDate || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > bMBidPlan.BidEndDate)
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDRBZZBJHYXNJC").ToString().Trim()+"')", true);
        //    DL_BidPlanID.Focus();
        //    return;
        //}
        if (IsBMOpenBidRecordBidPlanID(DL_BidPlanID.SelectedValue.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZBJHYKBJC").ToString().Trim()+"')", true);
            DL_BidPlanID.Focus();
            return;
        }

        BMOpenBidRecordBLL bMOpenBidRecordBLL = new BMOpenBidRecordBLL();
        BMOpenBidRecord bMOpenBidRecord = new BMOpenBidRecord();

        bMOpenBidRecord.OpenBidder = TB_OpenBidder.Text.Trim();
        bMOpenBidRecord.Name = TB_Name.Text.Trim();
        bMOpenBidRecord.OpenBidRemark = TB_OpenBidRemark.Text.Trim();
        bMOpenBidRecord.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMOpenBidRecord.BidPlanName = GetBMBidPlanName(bMOpenBidRecord.BidPlanID.ToString().Trim());
        bMOpenBidRecord.OpenBidDate = DateTime.Parse(DLC_OpenBidDate.Text.Trim());

        try
        {
            bMOpenBidRecordBLL.AddBMOpenBidRecord(bMOpenBidRecord);
            TB_ID.Text = GetMaxBMOpenBidRecordID(bMOpenBidRecord).ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);

            LoadBMOpenBidRecordList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    /// <summary>
    /// 新增或更新时，开标记录名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMOpenBidRecordName(string strName, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMOpenBidRecord Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select ID From T_BMOpenBidRecord Where Name='" + strName + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMOpenBidRecord").Tables[0];
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
    /// 新增时，获取表T_BMOpenBidRecord中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMOpenBidRecordID(BMOpenBidRecord bmbp)
    {
        string strHQL = "Select ID From T_BMOpenBidRecord where Name='" + bmbp.Name.Trim() + "' and OpenBidder='" + bmbp.OpenBidder.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMOpenBidRecord").Tables[0];
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

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim()+"')", true);
            TB_Name.Focus();
            return;
        }
        if (IsBMOpenBidRecordName(TB_Name.Text.Trim(), TB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim()+"')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_BidPlanID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZBJHBJC").ToString().Trim()+"')", true);
            DL_BidPlanID.Focus();
            return;
        }
        //BMBidPlan bMBidPlan = GetBMBidPlanModel(DL_BidPlanID.SelectedValue.Trim());
        //if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < bMBidPlan.BidStartDate || DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) > bMBidPlan.BidEndDate)
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGDRBZZBJHYXNJC").ToString().Trim()+"')", true);
        //    DL_BidPlanID.Focus();
        //    return;
        //}
        if (IsBMOpenBidRecordBidPlanID(DL_BidPlanID.SelectedValue.Trim(), TB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGZBJHYKBJC").ToString().Trim()+"')", true);
            DL_BidPlanID.Focus();
            return;
        }

        string strHQL = "From BMOpenBidRecord as bMOpenBidRecord where bMOpenBidRecord.ID = '" + TB_ID.Text.Trim() + "'";
        BMOpenBidRecordBLL bMOpenBidRecordBLL = new BMOpenBidRecordBLL();
        IList lst = bMOpenBidRecordBLL.GetAllBMOpenBidRecords(strHQL);
        BMOpenBidRecord bMOpenBidRecord = (BMOpenBidRecord)lst[0];

        bMOpenBidRecord.OpenBidder = TB_OpenBidder.Text.Trim();
        bMOpenBidRecord.Name = TB_Name.Text.Trim();
        bMOpenBidRecord.OpenBidRemark = TB_OpenBidRemark.Text.Trim();
        bMOpenBidRecord.BidPlanID = int.Parse(DL_BidPlanID.SelectedValue.Trim());
        bMOpenBidRecord.BidPlanName = GetBMBidPlanName(bMOpenBidRecord.BidPlanID.ToString().Trim());
        bMOpenBidRecord.OpenBidDate = DateTime.Parse(DLC_OpenBidDate.Text.Trim());

        try
        {
            bMOpenBidRecordBLL.UpdateBMOpenBidRecord(bMOpenBidRecord, bMOpenBidRecord.ID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

            LoadBMOpenBidRecordList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCode = TB_ID.Text.Trim();
        if (IsBMOpenBidRecord(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGKBJLYBDYSCSB").ToString().Trim()+"')", true);
            return;
        }

        strHQL = "Delete From T_BMOpenBidRecord Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);

            LoadBMOpenBidRecordList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        }
    }

    /// <summary>
    /// 删除时，判断开标记录是否已被调用，已调用返回true；否则返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMOpenBidRecord(string strID)
    {
        string strHQL;
        bool flag = true;

        strHQL = "Select ID From T_BMAssessBidRecord Where OpenBidRecordID='" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidRecord").Tables[0];
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

            strHQL = "From BMOpenBidRecord as bMOpenBidRecord where bMOpenBidRecord.ID = '" + strId + "'";
            BMOpenBidRecordBLL bMOpenBidRecordBLL = new BMOpenBidRecordBLL();
            lst = bMOpenBidRecordBLL.GetAllBMOpenBidRecords(strHQL);
            BMOpenBidRecord bMOpenBidRecord = (BMOpenBidRecord)lst[0];

            TB_ID.Text = bMOpenBidRecord.ID.ToString().Trim();
            DL_BidPlanID.SelectedValue = bMOpenBidRecord.BidPlanID.ToString().Trim();
            DLC_OpenBidDate.Text = bMOpenBidRecord.OpenBidDate.ToString("yyyy-MM-dd");
            TB_OpenBidder.Text = bMOpenBidRecord.OpenBidder.Trim();
            TB_OpenBidRemark.Text = bMOpenBidRecord.OpenBidRemark.Trim();
            TB_Name.Text = bMOpenBidRecord.Name.Trim();

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMOpenBidRecord");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMOpenBidRecordList();
    }
}