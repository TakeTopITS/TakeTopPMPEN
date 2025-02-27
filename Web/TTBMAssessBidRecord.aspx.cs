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

public partial class TTBMAssessBidRecord : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_AssessBidDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_AssessBidder.Text = ShareClass.GetUserName(strUserCode);

            LoadBMOpenBidRecordName();

            LoadBMAssessBidRecordList();
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

    protected void LoadBMOpenBidRecordName()
    {
        string strHQL;
        //绑定开标记录名称T_BMOpenBidRecord
      //  strHQL = "select * From T_BMOpenBidRecord where ID not in (select OpenBidRecordID from T_BMAssessBidRecord) Order By ID Desc";
        strHQL = "select * From T_BMOpenBidRecord Order By ID Desc";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMOpenBidRecord");
        DL_OpenBidRecordID.DataSource = ds;
        DL_OpenBidRecordID.DataBind();
        DL_OpenBidRecordID.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    /// <summary>
    /// 新增或更新时，开标记录ID是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMAssessBidRecordOpenBidRecordID(string strOpenBidRecordId, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMAssessBidRecord Where OpenBidRecordID='" + strOpenBidRecordId + "' ";
        }
        else
            strHQL = "Select ID From T_BMAssessBidRecord Where OpenBidRecordID='" + strOpenBidRecordId + "' and ID<>'" + strID + "' ";
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

    protected void LoadBMAssessBidRecordList()
    {
        string strHQL;

        strHQL = "Select * From T_BMAssessBidRecord Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (AssessBidder like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or AssessBidFactors like '%" + TextBox1.Text.Trim() + "%' " +
            "or BidWay like '%" + TextBox1.Text.Trim() + "%' or AssessBidContent like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and OpenBidRecordName like '%" + TextBox2.Text.Trim() + "%' ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-AssessBidDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-AssessBidDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidRecord");

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
        if (IsBMAssessBidRecordName(TB_Name.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim()+"')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_OpenBidRecordID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGKBJLBJC").ToString().Trim()+"')", true);
            DL_OpenBidRecordID.Focus();
            return;
        }
        if (IsBMAssessBidRecordOpenBidRecordID(DL_OpenBidRecordID.SelectedValue.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGKBJLYPBJC").ToString().Trim()+"')", true);
            DL_OpenBidRecordID.Focus();
            return;
        }

        BMAssessBidRecordBLL bMAssessBidRecordBLL = new BMAssessBidRecordBLL();
        BMAssessBidRecord bMAssessBidRecord = new BMAssessBidRecord();

        bMAssessBidRecord.BudgetPrice = NB_BudgetPrice.Amount;
        bMAssessBidRecord.ReservePrice = NB_ReservePrice.Amount;
        bMAssessBidRecord.AssessBidder = TB_AssessBidder.Text.Trim();
        bMAssessBidRecord.BidWay = DL_BidWay.SelectedValue.Trim();
        bMAssessBidRecord.Name = TB_Name.Text.Trim();
        bMAssessBidRecord.AssessBidContent = TB_AssessBidContent.Text.Trim();
        bMAssessBidRecord.OpenBidRecordID = int.Parse(DL_OpenBidRecordID.SelectedValue.Trim());
        bMAssessBidRecord.OpenBidRecordName = GetBMOpenBidRecordName(bMAssessBidRecord.OpenBidRecordID.ToString().Trim());
        bMAssessBidRecord.AssessBidDate = DateTime.Parse(DLC_AssessBidDate.Text.Trim());
        bMAssessBidRecord.AssessBidFactors = TB_AssessBidFactors.Text.Trim();

        try
        {
            bMAssessBidRecordBLL.AddBMAssessBidRecord(bMAssessBidRecord);
            TB_ID.Text = GetMaxBMAssessBidRecordID(bMAssessBidRecord).ToString();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);

            LoadBMAssessBidRecordList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    /// <summary>
    /// 新增或更新时，评标记录名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMAssessBidRecordName(string strName, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_BMAssessBidRecord Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select ID From T_BMAssessBidRecord Where Name='" + strName + "' and ID<>'" + strID + "' ";
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

    /// <summary>
    /// 新增时，获取表T_BMAssessBidRecord中最大编号。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected int GetMaxBMAssessBidRecordID(BMAssessBidRecord bmbp)
    {
        string strHQL = "Select ID From T_BMAssessBidRecord where Name='" + bmbp.Name.Trim() + "' and AssessBidder='" + bmbp.AssessBidder.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected string GetBMOpenBidRecordName(string strID)
    {
        string strHQL;
        IList lst;
        //绑定开标记录名称
        strHQL = "From BMOpenBidRecord as bMOpenBidRecord Where bMOpenBidRecord.ID='" + strID + "' ";
        BMOpenBidRecordBLL bMOpenBidRecordBLL = new BMOpenBidRecordBLL();
        lst = bMOpenBidRecordBLL.GetAllBMOpenBidRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMOpenBidRecord bMOpenBidRecord = (BMOpenBidRecord)lst[0];
            return bMOpenBidRecord.Name.Trim();
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
        if (IsBMAssessBidRecordName(TB_Name.Text.Trim(), TB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim()+"')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_OpenBidRecordID.SelectedValue.Trim().Equals("0"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGKBJLBJC").ToString().Trim()+"')", true);
            DL_OpenBidRecordID.Focus();
            return;
        }
        if (IsBMAssessBidRecordOpenBidRecordID(DL_OpenBidRecordID.SelectedValue.Trim(), TB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGKBJLYPBJC").ToString().Trim()+"')", true);
            DL_OpenBidRecordID.Focus();
            return;
        }

        string strHQL = "From BMAssessBidRecord as bMAssessBidRecord where bMAssessBidRecord.ID = '" + TB_ID.Text.Trim() + "'";
        BMAssessBidRecordBLL bMAssessBidRecordBLL = new BMAssessBidRecordBLL();
        IList lst = bMAssessBidRecordBLL.GetAllBMAssessBidRecords(strHQL);
        BMAssessBidRecord bMAssessBidRecord = (BMAssessBidRecord)lst[0];

        bMAssessBidRecord.BudgetPrice = NB_BudgetPrice.Amount;
        bMAssessBidRecord.ReservePrice = NB_ReservePrice.Amount;
        bMAssessBidRecord.AssessBidder = TB_AssessBidder.Text.Trim();
        bMAssessBidRecord.BidWay = DL_BidWay.SelectedValue.Trim();
        bMAssessBidRecord.Name = TB_Name.Text.Trim();
        bMAssessBidRecord.AssessBidContent = TB_AssessBidContent.Text.Trim();
        bMAssessBidRecord.OpenBidRecordID = int.Parse(DL_OpenBidRecordID.SelectedValue.Trim());
        bMAssessBidRecord.OpenBidRecordName = GetBMOpenBidRecordName(bMAssessBidRecord.OpenBidRecordID.ToString().Trim());
        bMAssessBidRecord.AssessBidDate = DateTime.Parse(DLC_AssessBidDate.Text.Trim());
        bMAssessBidRecord.AssessBidFactors = TB_AssessBidFactors.Text.Trim();

        try
        {
            bMAssessBidRecordBLL.UpdateBMAssessBidRecord(bMAssessBidRecord, bMAssessBidRecord.ID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);

            LoadBMAssessBidRecordList();
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
        if (IsBMAssessBidRecord(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGPBJLYBDYSCSB").ToString().Trim()+"')", true);
            return;
        }

        strHQL = "Delete From T_BMAssessBidRecord Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);

            LoadBMAssessBidRecordList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        }
    }

    /// <summary>
    /// 删除时，判断评标记录是否已被调用，已调用返回true；否则返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsBMAssessBidRecord(string strID)
    {
        string strHQL;
        bool flag = true;

        strHQL = "Select ID From T_BMAssessBidReport Where AssessBidRecordID='" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidReport").Tables[0];
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

            strHQL = "From BMAssessBidRecord as bMAssessBidRecord where bMAssessBidRecord.ID = '" + strId + "'";
            BMAssessBidRecordBLL bMAssessBidRecordBLL = new BMAssessBidRecordBLL();
            lst = bMAssessBidRecordBLL.GetAllBMAssessBidRecords(strHQL);
            BMAssessBidRecord bMAssessBidRecord = (BMAssessBidRecord)lst[0];

            TB_ID.Text = bMAssessBidRecord.ID.ToString().Trim();
            DL_OpenBidRecordID.SelectedValue = bMAssessBidRecord.OpenBidRecordID.ToString().Trim();
            DLC_AssessBidDate.Text = bMAssessBidRecord.AssessBidDate.ToString("yyyy-MM-dd");
            TB_AssessBidder.Text = bMAssessBidRecord.AssessBidder.Trim();
            TB_AssessBidContent.Text = bMAssessBidRecord.AssessBidContent.Trim();
            TB_Name.Text = bMAssessBidRecord.Name.Trim();
            NB_ReservePrice.Amount = bMAssessBidRecord.ReservePrice;
            NB_BudgetPrice.Amount = bMAssessBidRecord.BudgetPrice;
            DL_BidWay.SelectedValue = bMAssessBidRecord.BidWay.Trim();
            TB_AssessBidFactors.Text = bMAssessBidRecord.AssessBidFactors.Trim();

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMAssessBidRecord");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMAssessBidRecordList();
    }

    protected void DL_OpenBidRecordID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        //绑定招标方式
        strHQL = "From BMOpenBidRecord as bMOpenBidRecord Where bMOpenBidRecord.ID='" + DL_OpenBidRecordID.SelectedValue.Trim() + "' ";
        BMOpenBidRecordBLL bMOpenBidRecordBLL = new BMOpenBidRecordBLL();
        lst = bMOpenBidRecordBLL.GetAllBMOpenBidRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            BMOpenBidRecord bMOpenBidRecord = (BMOpenBidRecord)lst[0];
            strHQL = "From BMBidPlan as bMBidPlan Where bMBidPlan.ID='" + bMOpenBidRecord.BidPlanID.ToString() + "' ";
            BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
            lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
                DL_BidWay.SelectedValue = bMBidPlan.BidWay.Trim();
            }
        }
    }
}