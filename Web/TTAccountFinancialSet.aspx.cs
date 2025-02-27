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

public partial class TTAccountFinancialSet : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","财务帐套设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TakeTopCore.CoreShareClass.InitialAllDepartmentTree( LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1);
            LoadCurrency();
            LoadAccountFinancialSetList(txt_FinancialInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());
        }
    }

    protected void LoadCurrency()
    {
        string strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC ";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        DL_CurrencyType.DataSource = lst;
        DL_CurrencyType.DataBind();
        DL_CurrencyType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadAccountFinancialSetList(string strFinancialInfo, string strStatus)
    {
        string strHQL = "Select * From T_AccountFinancialSet Where 1=1 ";
        if (!string.IsNullOrEmpty(strFinancialInfo))
        {
            strHQL += " and (FinancialName like '%" + strFinancialInfo + "%' or DepartName like '%" + strFinancialInfo + "%' or Industry like '%" + strFinancialInfo + "%' " +
                "or CurrencyType like '%" + strFinancialInfo + "%' or FinancialCode like '%" + strFinancialInfo + "%') ";
        }
        if (strStatus.Trim() != "PleaseSelect")
        {
            strHQL += " and Status = '" + strStatus + "' ";
        }
        strHQL += " Order By ID DESC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;
        if (treeNode.Target != "0")
        {
            lbl_DepartCode.Text = treeNode.Target.Trim();
            LB_DepartName.Text = ShareClass.GetDepartName(lbl_DepartCode.Text.Trim());
        }

        
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (TB_FinancialCode.Text.Trim() == "" || string.IsNullOrEmpty(TB_FinancialCode.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZTBMBNWKJC").ToString().Trim()+"')", true);
            TB_FinancialCode.Focus();
            return;
        }
        if (TB_FinancialName.Text.Trim() == "" || string.IsNullOrEmpty(TB_FinancialName.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZTMCBNWKJC").ToString().Trim()+"')", true);
            TB_FinancialName.Focus();
            return;
        }
        if (lbl_DepartCode.Text.Trim() == "" || string.IsNullOrEmpty(lbl_DepartCode.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSBMBNWKJC").ToString().Trim()+"')", true);
            lbl_DepartCode.Focus();
            return;
        }
        if (DL_CurrencyType.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_CurrencyType.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSBWBWBXJC").ToString().Trim()+"')", true);
            DL_CurrencyType.Focus();
            return;
        }
        if (DLC_StartTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSKSRBNWKJC").ToString().Trim()+"')", true);
            DLC_StartTime.Focus();
            return;
        }
        if (DLC_EndTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJSRBNWKJC").ToString().Trim()+"')", true);
            DLC_EndTime.Focus();
            return;
        }
        if (DateTime.Parse(DLC_StartTime.Text.Trim()) > DateTime.Parse(DLC_EndTime.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSKSRBNDYJSRJC").ToString().Trim()+"')", true);
            DLC_StartTime.Focus();
            DLC_EndTime.Focus();
            return;
        }
        if (IsAccountFinancialSet(TB_FinancialCode.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGZTBMYCZJC").ToString().Trim()+"')", true);
            TB_FinancialCode.Focus();
            return;
        }
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        AccountFinancialSet accountFinancialSet = new AccountFinancialSet();
        accountFinancialSet.CreaterCode = strUserCode.Trim();
        accountFinancialSet.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
        accountFinancialSet.DepartCode = lbl_DepartCode.Text.Trim();
        accountFinancialSet.DepartName = LB_DepartName.Text.Trim();
        accountFinancialSet.EndTime = DateTime.Parse(DLC_EndTime.Text.Trim());
        accountFinancialSet.FinancialName = TB_FinancialName.Text.Trim();
        accountFinancialSet.Industry = TB_Industry.Text.Trim();
        accountFinancialSet.StartTime = DateTime.Parse(DLC_StartTime.Text.Trim());
        accountFinancialSet.Status = DL_Status.SelectedValue.Trim();
        accountFinancialSet.ExchangeRate = GetExchangeRateCurrency(DL_CurrencyType.SelectedValue.Trim());
        accountFinancialSet.FinancialCode = TB_FinancialCode.Text.Trim();

        try
        {
            accountFinancialSetBLL.AddAccountFinancialSet(accountFinancialSet);
            TB_ID.Text = GetAccountFinancialSetID(accountFinancialSet);
            UpDateFinancialCode(accountFinancialSet.FinancialCode.Trim(), accountFinancialSet.FinancialCode.Trim());

            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;

            LoadAccountFinancialSetList(txt_FinancialInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZTXZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZTXZSB").ToString().Trim()+"')", true);
        }
    }

    protected string GetAccountFinancialSetID(AccountFinancialSet bmp)
    {
        string flag = string.Empty;
        string strHQL = "Select ID From T_AccountFinancialSet where CreaterCode='" + bmp.CreaterCode + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet").Tables[0];
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
        if (TB_FinancialCode.Text.Trim() == "" || string.IsNullOrEmpty(TB_FinancialCode.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZTBMBNWKJC").ToString().Trim()+"')", true);
            TB_FinancialCode.Focus();
            return;
        }
        if (TB_FinancialName.Text.Trim() == "" || string.IsNullOrEmpty(TB_FinancialName.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZTMCBNWKJC").ToString().Trim()+"')", true);
            TB_FinancialName.Focus();
            return;
        }
        if (lbl_DepartCode.Text.Trim() == "" || string.IsNullOrEmpty(lbl_DepartCode.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSBMBNWKJC").ToString().Trim()+"')", true);
            lbl_DepartCode.Focus();
            return;
        }
        if (DL_CurrencyType.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_CurrencyType.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSBWBWBXJC").ToString().Trim()+"')", true);
            DL_CurrencyType.Focus();
            return;
        }
        if (DLC_StartTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_StartTime.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSKSRBNWKJC").ToString().Trim()+"')", true);
            DLC_StartTime.Focus();
            return;
        }
        if (DLC_EndTime.Text.Trim() == "" || string.IsNullOrEmpty(DLC_EndTime.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJSRBNWKJC").ToString().Trim()+"')", true);
            DLC_EndTime.Focus();
            return;
        }
        if (DateTime.Parse(DLC_StartTime.Text.Trim()) > DateTime.Parse(DLC_EndTime.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSKSRBNDYJSRJC").ToString().Trim()+"')", true);
            DLC_StartTime.Focus();
            DLC_EndTime.Focus();
            return;
        }
        if (TB_ID.Text.Trim() == "" || string.IsNullOrEmpty(TB_ID.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGSJBCZJC").ToString().Trim()+"')", true);
            return;
        }
        if (IsAccountFinancialSet(TB_FinancialCode.Text.Trim(),TB_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGZTBMYCZJC").ToString().Trim()+"')", true);
            TB_FinancialCode.Focus();
            return;
        }
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        string strHQL = "from AccountFinancialSet as accountFinancialSet where accountFinancialSet.ID = '" + TB_ID.Text.Trim() + "' ";
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        if (lst != null && lst.Count > 0)
        {
            AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[0];
            accountFinancialSet.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            accountFinancialSet.DepartCode = lbl_DepartCode.Text.Trim();
            accountFinancialSet.DepartName = LB_DepartName.Text.Trim();
            accountFinancialSet.EndTime = DateTime.Parse(DLC_EndTime.Text.Trim());
            accountFinancialSet.FinancialName = TB_FinancialName.Text.Trim();
            accountFinancialSet.Industry = TB_Industry.Text.Trim();
            accountFinancialSet.StartTime = DateTime.Parse(DLC_StartTime.Text.Trim());
            accountFinancialSet.Status = DL_Status.SelectedValue.Trim();
            accountFinancialSet.ExchangeRate = GetExchangeRateCurrency(DL_CurrencyType.SelectedValue.Trim());
            accountFinancialSet.FinancialCode = TB_FinancialCode.Text.Trim();

            try
            {
                accountFinancialSetBLL.UpdateAccountFinancialSet(accountFinancialSet, accountFinancialSet.ID);
                UpDateFinancialCode(lbl_OldFinancialCode.Text.Trim(), accountFinancialSet.FinancialCode.Trim());

                BT_Delete.Visible = true;
                BT_Delete.Enabled = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;

                LoadAccountFinancialSetList(txt_FinancialInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZTGXCG").ToString().Trim()+"')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZTGXSB").ToString().Trim()+"')", true);
            }
        }
    }

    protected bool IsAccountFinancialSet(string strCode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_AccountFinancialSet Where FinancialCode='" + strCode + "' ";
        }
        else
            strHQL = "Select ID From T_AccountFinancialSet Where FinancialCode='" + strCode + "' and ID<>'" + strID + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet").Tables[0];
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGSJBCZJC").ToString().Trim()+"')", true);
            return;
        }
        strHQL = "Delete From T_AccountFinancialSet Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            BT_Update.Visible = false;
            BT_Delete.Visible = false;
            BT_Delete.Enabled = false;
            BT_Update.Enabled = false;

            LoadAccountFinancialSetList(txt_FinancialInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadAccountFinancialSetList(txt_FinancialInfo.Text.Trim(), ddlStatus.SelectedValue.Trim());
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            Button strID = (Button)e.Item.FindControl("BT_Financial");

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }
            e.Item.ForeColor = Color.Red;

            strHQL = "from AccountFinancialSet as accountFinancialSet where accountFinancialSet.ID = '" + strID.Text + "' ";
            AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
            lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
            if (lst != null && lst.Count > 0)
            {
                AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[0];
                TB_ID.Text = accountFinancialSet.ID.ToString();
                TB_FinancialName.Text = accountFinancialSet.FinancialName.Trim();
                TB_Industry.Text = accountFinancialSet.Industry.Trim();
                DL_CurrencyType.SelectedValue = accountFinancialSet.CurrencyType.Trim();
                DL_Status.SelectedValue = accountFinancialSet.Status.Trim();
                DLC_EndTime.Text = accountFinancialSet.EndTime.ToString("yyyy-MM-dd");
                DLC_StartTime.Text = accountFinancialSet.StartTime.ToString("yyyy-MM-dd");
                lbl_DepartCode.Text = accountFinancialSet.DepartCode.Trim();
                LB_DepartName.Text = accountFinancialSet.DepartName.Trim();
                TB_FinancialCode.Text = string.IsNullOrEmpty(accountFinancialSet.FinancialCode) ? "" : accountFinancialSet.FinancialCode.Trim();
                lbl_OldFinancialCode.Text = string.IsNullOrEmpty(accountFinancialSet.FinancialCode) ? "" : accountFinancialSet.FinancialCode.Trim();

                if (accountFinancialSet.CreaterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AccountFinancialSet");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected decimal GetExchangeRateCurrency(string strCurrency)
    {
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        string strHQL = "from CurrencyType as currencyType where currencyType.Type = '" + strCurrency.Trim() + "' ";
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            CurrencyType currencyType = (CurrencyType)lst[0];
            return currencyType.ExchangeRate;
        }
        else
            return 0;
    }

    protected void UpDateFinancialCode(string strOldFinancialCode, string strNewFinancialCode)
    {
        //更新出入账的帐套编码
        string strHQL = "From AccountGeneralLedger as accountGeneralLedger where accountGeneralLedger.FinancialCode = '" + strOldFinancialCode + "'";
        AccountGeneralLedgerBLL accountGeneralLedgerBLL = new AccountGeneralLedgerBLL();
        IList lst = accountGeneralLedgerBLL.GetAllAccountGeneralLedgers(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                AccountGeneralLedger accountGeneralLedger = (AccountGeneralLedger)lst[i];
                accountGeneralLedger.FinancialCode = strNewFinancialCode;
                accountGeneralLedgerBLL.UpdateAccountGeneralLedger(accountGeneralLedger, accountGeneralLedger.ID);
            }
        }
        //更新区间表中的帐套编码
        strHQL = "From AccountingIntervalSet as accountingIntervalSet where accountingIntervalSet.FinancialCode = '" + strOldFinancialCode + "'";
        AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
        lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int j = 0; j < lst.Count; j++)
            {
                AccountingIntervalSet accountingIntervalSet = (AccountingIntervalSet)lst[j];
                accountingIntervalSet.FinancialCode = strNewFinancialCode;
                accountingIntervalSetBLL.UpdateAccountingIntervalSet(accountingIntervalSet, accountingIntervalSet.ID);
            }
        }
    }
}