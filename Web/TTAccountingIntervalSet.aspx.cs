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

public partial class TTAccountingIntervalSet : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","财务区间设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadAccountFinancialSet();
            LoadAccountingIntervalSetList(txt_IntervalInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());
        }
    }

    protected void LoadAccountFinancialSet()
    {
        string strHQL = "From AccountFinancialSet as accountFinancialSet Order By accountFinancialSet.ID ASC ";
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        DL_FinancialID.DataSource = lst;
        DL_FinancialID.DataBind();
        DL_FinancialID.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadAccountingIntervalSetList(string strFinancialInfo, string strStatus)
    {
        string strHQL = "Select * From T_AccountingIntervalSet Where 1=1 ";
        if (!string.IsNullOrEmpty(strFinancialInfo))
        {
            strHQL += " and (IntervalName like '%" + strFinancialInfo + "%' or IntervalCode like '%" + strFinancialInfo + "%') ";
        }
        if (strStatus.Trim() != "PleaseSelect")
        {
            strHQL += " and Status = '" + strStatus + "' ";
        }
        strHQL += " Order By ID DESC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountingIntervalSet");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (DL_FinancialID.SelectedValue.Trim()=="")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZTWBXJC")+"')", true);
            DL_FinancialID.Focus();
            return;
        }
        string FinancialStatus = GetFinancialStatus(DL_FinancialID.SelectedValue.Trim());
        if (FinancialStatus != "OPEN")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGZTYGBJC")+"')", true);
            DL_FinancialID.Focus();
            return;
        }
        if (TB_IntervalCode.Text.Trim() == "" || string.IsNullOrEmpty(TB_IntervalCode.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJBMBNWKJC")+"')", true);
            TB_IntervalCode.Focus();
            return;
        }
        if (TB_IntervalName.Text.Trim() == "" || string.IsNullOrEmpty(TB_IntervalName.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJMCBNWKJC")+"')", true);
            TB_IntervalName.Focus();
            return;
        }
        if (DLC_StartTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSKSRBNWKJC")+"')", true);
            DLC_StartTime.Focus();
            return;
        }
        if (DLC_EndTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJSRBNWKJC")+"')", true);
            DLC_EndTime.Focus();
            return;
        }
        if (DateTime.Parse(DLC_StartTime.Text.Trim()) > DateTime.Parse(DLC_EndTime.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSKSRBNDYJSRJC")+"')", true);
            DLC_StartTime.Focus();
            DLC_EndTime.Focus();
            return;
        }
        if (IsAccountingIntervalSet(TB_IntervalCode.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGJBMYCZJC")+"')", true);
            TB_IntervalCode.Focus();
            return;
        }
        AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
        AccountingIntervalSet accountingIntervalSet = new AccountingIntervalSet();
        accountingIntervalSet.CreaterCode = strUserCode.Trim();
        accountingIntervalSet.EndTime = DateTime.Parse(DLC_EndTime.Text.Trim());
        accountingIntervalSet.StartTime = DateTime.Parse(DLC_StartTime.Text.Trim());
        accountingIntervalSet.Status = DL_Status.SelectedValue.Trim();
        accountingIntervalSet.FinancialCode = DL_FinancialID.SelectedValue.Trim();
        accountingIntervalSet.IntervalName = TB_IntervalName.Text.Trim();
        accountingIntervalSet.IntervalCode = TB_IntervalCode.Text.Trim();

        try
        {
            accountingIntervalSetBLL.AddAccountingIntervalSet(accountingIntervalSet);
            TB_ID.Text = GetAccountingIntervalSetID(accountingIntervalSet);
            UpDateIntervalCode(accountingIntervalSet.IntervalCode.Trim(), accountingIntervalSet.IntervalCode.Trim());

            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;

            LoadAccountingIntervalSetList(txt_IntervalInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJXZCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJXZSB")+"')", true);
        }
    }

    protected string GetAccountingIntervalSetID(AccountingIntervalSet bmp)
    {
        string flag = string.Empty;
        string strHQL = "Select ID From T_AccountingIntervalSet where CreaterCode='" + bmp.CreaterCode + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountingIntervalSet").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["ID"].ToString().Trim();
        }
        else
        {
            flag = "0";
        }
        return flag;
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (DL_FinancialID.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZTWBXJC")+"')", true);
            DL_FinancialID.Focus();
            return;
        }
        string FinancialStatus = GetFinancialStatus(DL_FinancialID.SelectedValue.Trim());
        if (FinancialStatus != "OPEN")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGZTYGBJC")+"')", true);
            DL_FinancialID.Focus();
            return;
        }
        if (TB_IntervalCode.Text.Trim() == "" || string.IsNullOrEmpty(TB_IntervalCode.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJBMBNWKJC")+"')", true);
            TB_IntervalCode.Focus();
            return;
        }
        if (TB_IntervalName.Text.Trim() == "" || string.IsNullOrEmpty(TB_IntervalName.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJMCBNWKJC")+"')", true);
            TB_IntervalName.Focus();
            return;
        }
        if (DLC_StartTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSKSRBNWKJC")+"')", true);
            DLC_StartTime.Focus();
            return;
        }
        if (DLC_EndTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJSRBNWKJC")+"')", true);
            DLC_EndTime.Focus();
            return;
        }
        if (DateTime.Parse(DLC_StartTime.Text.Trim()) > DateTime.Parse(DLC_EndTime.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSKSRBNDYJSRJC")+"')", true);
            DLC_StartTime.Focus();
            DLC_EndTime.Focus();
            return;
        }
        if (TB_ID.Text.Trim() == "" || string.IsNullOrEmpty(TB_ID.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGSJBCZJC")+"')", true);
            return;
        }
        if (IsAccountingIntervalSet(TB_IntervalCode.Text.Trim(), TB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGJBMYCZJC")+"')", true);
            TB_IntervalCode.Focus();
            return;
        }
        AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
        string strHQL = "from AccountingIntervalSet as accountingIntervalSet where accountingIntervalSet.ID = '" + TB_ID.Text.Trim() + "' ";
        IList lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
        if (lst != null && lst.Count > 0)
        {
            AccountingIntervalSet accountingIntervalSet = (AccountingIntervalSet)lst[0];
            accountingIntervalSet.EndTime = DateTime.Parse(DLC_EndTime.Text.Trim());
            accountingIntervalSet.StartTime = DateTime.Parse(DLC_StartTime.Text.Trim());
            accountingIntervalSet.Status = DL_Status.SelectedValue.Trim();
            accountingIntervalSet.FinancialCode = DL_FinancialID.SelectedValue.Trim();
            accountingIntervalSet.IntervalName = TB_IntervalName.Text.Trim();
            accountingIntervalSet.IntervalCode = TB_IntervalCode.Text.Trim();

            try
            {
                accountingIntervalSetBLL.UpdateAccountingIntervalSet(accountingIntervalSet, accountingIntervalSet.ID);
                UpDateIntervalCode(lbl_IntervalCode.Text.Trim(), accountingIntervalSet.IntervalCode.Trim());

                BT_Delete.Visible = true;
                BT_Delete.Enabled = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;

                LoadAccountingIntervalSetList(txt_IntervalInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGXCG")+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGXSB")+"')", true);
            }
        }
    }

    protected bool IsAccountingIntervalSet(string strCode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_AccountingIntervalSet Where IntervalCode='" + strCode + "' ";
        }
        else
            strHQL = "Select ID From T_AccountingIntervalSet Where IntervalCode='" + strCode + "' and ID<>'" + strID + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountingIntervalSet").Tables[0];
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

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCode = TB_ID.Text.Trim();
        if (TB_ID.Text.Trim() == "" || string.IsNullOrEmpty(TB_ID.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGSJBCZJC")+"')", true);
            return;
        }
        strHQL = "Delete From T_AccountingIntervalSet Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_Update.Visible = false;
            BT_Delete.Visible = false;
            BT_Delete.Enabled = false;
            BT_Update.Enabled = false;

            LoadAccountingIntervalSetList(txt_IntervalInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC")+"')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadAccountingIntervalSetList(txt_IntervalInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            Button strID = (Button)e.Item.FindControl("BT_Interval");

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from AccountingIntervalSet as accountingIntervalSet where accountingIntervalSet.ID = '" + strID.Text + "' ";
            AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
            lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
            if (lst != null && lst.Count > 0)
            {
                AccountingIntervalSet accountingIntervalSet = (AccountingIntervalSet)lst[0];
                TB_ID.Text = accountingIntervalSet.ID.ToString();
                DL_Status.SelectedValue = accountingIntervalSet.Status.Trim();
                DLC_EndTime.Text = accountingIntervalSet.EndTime.ToString("yyyy-MM-dd");
                DLC_StartTime.Text = accountingIntervalSet.StartTime.ToString("yyyy-MM-dd");
                TB_IntervalName.Text = accountingIntervalSet.IntervalName.Trim();
                DL_FinancialID.SelectedValue = accountingIntervalSet.FinancialCode.Trim();
                TB_IntervalCode.Text = string.IsNullOrEmpty(accountingIntervalSet.IntervalCode) ? "" : accountingIntervalSet.IntervalCode.Trim();
                lbl_IntervalCode.Text = string.IsNullOrEmpty(accountingIntervalSet.IntervalCode) ? "" : accountingIntervalSet.IntervalCode.Trim();

                if (accountingIntervalSet.CreaterCode.Trim() == strUserCode.Trim())
                {
                    BT_Update.Visible = true;
                    BT_Delete.Visible = true;
                    BT_Delete.Enabled = true;
                    BT_Update.Enabled = true;
                }
                else
                {
                    BT_Update.Visible = false;
                    BT_Delete.Visible = false;
                }
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountingIntervalSet");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected string GetFinancialName(string strFinancialCode)
    {
        string flag = string.Empty;
        string strHQL = "Select FinancialName From T_AccountFinancialSet where FinancialCode='" + strFinancialCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["FinancialName"].ToString().Trim();
        }
        else
        {
            flag = "";
        }
        return flag;
    }

    protected string GetFinancialStatus(string strFinancialCode)
    {
        string flag = string.Empty;
        string strHQL = "Select Status From T_AccountFinancialSet where FinancialCode='" + strFinancialCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["Status"].ToString().Trim();
        }
        else
        {
            flag = "";
        }
        return flag;
    }

    protected void UpDateIntervalCode(string strOldIntervalCode, string strNewIntervalCode)
    {
        //更新出入账的区间编码
        string strHQL = "From AccountGeneralLedger as accountGeneralLedger where accountGeneralLedger.IntervalCode = '" + strOldIntervalCode + "'";
        AccountGeneralLedgerBLL accountGeneralLedgerBLL = new AccountGeneralLedgerBLL();
        IList lst = accountGeneralLedgerBLL.GetAllAccountGeneralLedgers(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                AccountGeneralLedger accountGeneralLedger = (AccountGeneralLedger)lst[i];
                accountGeneralLedger.IntervalCode = strNewIntervalCode;
                accountGeneralLedgerBLL.UpdateAccountGeneralLedger(accountGeneralLedger, accountGeneralLedger.ID);
            }
        }
    }
}