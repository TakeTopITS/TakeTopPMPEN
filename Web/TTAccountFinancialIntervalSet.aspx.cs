using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTAccountFinancialIntervalSet : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","财务管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            GetFinancialIntervalName();
            LoadAccountFinancialSet();
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

    protected void GetFinancialIntervalName()
    {
        string strHQL = "From AccountFinancialSet as accountFinancialSet Where accountFinancialSet.Status='OPEN' Order By accountFinancialSet.ID ASC ";
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        if (lst != null && lst.Count == 1)
        {
            AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[0];
            lbl_FinancialName.Text = accountFinancialSet.FinancialName.Trim();
        }
        else
            lbl_FinancialName.Text = LanguageHandle.GetWord("WeiShuaDing").ToString().Trim();

        strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.Status='OPEN' Order By accountingIntervalSet.ID ASC ";
        AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
        lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
        if (lst != null && lst.Count == 1)
        {
            AccountingIntervalSet accountingIntervalSet = (AccountingIntervalSet)lst[0];
            lbl_IntervalName.Text = accountingIntervalSet.IntervalName.Trim();
        }
        else
            lbl_IntervalName.Text = LanguageHandle.GetWord("WeiShuaDing").ToString().Trim();
    }

    protected void DL_FinancialID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string FinancialID = DL_FinancialID.SelectedValue.Trim();
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        string strHQL = "from AccountFinancialSet as accountFinancialSet where accountFinancialSet.FinancialCode = '" + FinancialID + "' ";
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[0];

            strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.FinancialCode='" + accountFinancialSet.FinancialCode.Trim() + "' Order By accountingIntervalSet.ID ASC ";
            AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
            lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
            DL_IntervalID.DataSource = lst;
            DL_IntervalID.DataBind();
            DL_IntervalID.Items.Insert(0, new ListItem("--Select--", ""));
        }
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (DL_FinancialID.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZTMCWBXJC").ToString().Trim()+"')", true);
            DL_FinancialID.Focus();
            return;
        }
        if (DL_IntervalID.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSJMCWBXJC").ToString().Trim()+"')", true);
            DL_IntervalID.Focus();
            return;
        }
        AccountFinancialSetBLL accountFinancialSetBLL = new AccountFinancialSetBLL();
        string strHQL = "from AccountFinancialSet as accountFinancialSet where accountFinancialSet.FinancialCode = '" + DL_FinancialID.SelectedValue.Trim() + "' ";
        IList lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        if (lst != null && lst.Count > 0)
        {
            AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[0];
            accountFinancialSet.Status = "OPEN";

            accountFinancialSetBLL.UpdateAccountFinancialSet(accountFinancialSet, accountFinancialSet.ID);
        }

        strHQL = "from AccountFinancialSet as accountFinancialSet where accountFinancialSet.FinancialCode <> '" + DL_FinancialID.SelectedValue.Trim() + "' ";
        lst = accountFinancialSetBLL.GetAllAccountFinancialSets(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                AccountFinancialSet accountFinancialSet = (AccountFinancialSet)lst[i];
                accountFinancialSet.Status = "CLOSE";

                accountFinancialSetBLL.UpdateAccountFinancialSet(accountFinancialSet, accountFinancialSet.ID);
            }
        }

        AccountingIntervalSetBLL accountingIntervalSetBLL = new AccountingIntervalSetBLL();
        strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.IntervalCode = '" + DL_IntervalID.SelectedValue.Trim() + "' ";
        lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
        if (lst != null && lst.Count > 0)
        {
            AccountingIntervalSet accountingIntervalSet = (AccountingIntervalSet)lst[0];
            accountingIntervalSet.Status = "OPEN";

            accountingIntervalSetBLL.UpdateAccountingIntervalSet(accountingIntervalSet, accountingIntervalSet.ID);
        }

        strHQL = "From AccountingIntervalSet as accountingIntervalSet Where accountingIntervalSet.IntervalCode <> '" + DL_IntervalID.SelectedValue.Trim() + "' ";
        lst = accountingIntervalSetBLL.GetAllAccountingIntervalSets(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int j = 0; j < lst.Count; j++)
            {
                AccountingIntervalSet accountingIntervalSet = (AccountingIntervalSet)lst[j];
                accountingIntervalSet.Status = "CLOSE";

                accountingIntervalSetBLL.UpdateAccountingIntervalSet(accountingIntervalSet, accountingIntervalSet.ID);
            }
        }

        lbl_FinancialName.Text = DL_FinancialID.SelectedItem.Text;
        lbl_IntervalName.Text = DL_IntervalID.SelectedItem.Text;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZTJDCG").ToString().Trim()+"')", true);
    }
}