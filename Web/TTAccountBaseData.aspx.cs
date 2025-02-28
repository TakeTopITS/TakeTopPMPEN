using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTAccountBaseData : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "会计科目设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            AccountTree(TreeView1);//绑定会计科目树形结构

            LoadAccount();//绑定科目DDL
        }
    }

    protected void BT_AccountAdd_Click(object sender, EventArgs e)
    {
        if (TB_AccountName.Text.Trim() == "" || TB_AccountType.Text.Trim() == "" || TB_SortNumber.Text.Trim() == "" || TB_TypeArea.Text.Trim() == "" || TB_AccountCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSZDNRBNWKJC").ToString().Trim() + "')", true);
            TB_AccountName.Focus();
            TB_AccountType.Focus();
            TB_SortNumber.Focus();
            TB_TypeArea.Focus();
            TB_AccountCode.Focus();
            return;
        }
        if (IsAccount(TB_AccountCode.Text.Trim(), TB_AccountType.Text.Trim(), TB_TypeArea.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSGKMXXYCZJC").ToString().Trim() + "')", true);
            TB_AccountCode.Focus();
            TB_AccountType.Focus();
            TB_TypeArea.Focus();
            return;
        }

        AccountBLL accountBLL = new AccountBLL();
        Account account = new Account();

        account.AccountName = TB_AccountName.Text.Trim();
        account.SortNumber = int.Parse(TB_SortNumber.Text.Trim());
        account.ParentID = int.Parse(DL_ParentID.SelectedValue.Trim());
        account.AccountType = TB_AccountType.Text.Trim();
        account.TypeArea = TB_TypeArea.Text.Trim();
        account.AccountCode = TB_AccountCode.Text.Trim();

        try
        {
            accountBLL.AddAccount(account);
            lbl_ID.Text = GetMaxAccountID(account).ToString();
            UpDateAccountGeneralLedger(account.AccountCode.Trim(), account.AccountName.Trim(), account.AccountCode.Trim());

            AccountTree(TreeView1);
            LoadAccount();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected bool IsAccount(string strAccountCode, string strAccountType, string strTypeArea, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            //    strHQL = "Select ID From T_Account Where AccountCode='" + strAccountCode + "' and AccountType='" + strAccountType + "' and TypeArea='" + strTypeArea + "' ";
            strHQL = "Select ID From T_Account Where AccountCode='" + strAccountCode + "' ";
        }
        else
            strHQL = "Select ID From T_Account Where AccountCode='" + strAccountCode + "' and ID<>'" + strID + "'";
        //strHQL = "Select ID From T_Account Where AccountCode='" + strAccountCode + "' and AccountType='" + strAccountType + "' and TypeArea='" + strTypeArea + "' and ID<>'" + strID + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_Account").Tables[0];
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

    protected void BT_AccountUpdate_Click(object sender, EventArgs e)
    {
        if (TB_AccountName.Text.Trim() == "" || TB_AccountType.Text.Trim() == "" || TB_SortNumber.Text.Trim() == "" || TB_TypeArea.Text.Trim() == "" || TB_AccountCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSZDNRBNWKJC").ToString().Trim() + "')", true);
            TB_AccountName.Focus();
            TB_AccountType.Focus();
            TB_SortNumber.Focus();
            TB_TypeArea.Focus();
            TB_AccountCode.Focus();
            return;
        }
        if (IsAccount(TB_AccountCode.Text.Trim(), TB_AccountType.Text.Trim(), TB_TypeArea.Text.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSGKMXXYCZJC").ToString().Trim() + "')", true);
            TB_AccountCode.Focus();
            TB_AccountType.Focus();
            TB_TypeArea.Focus();
            return;
        }

        if(DL_ParentID.SelectedValue.Trim() == lbl_ID.Text.Trim())
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoBuNengShuaZiShenZuoWe").ToString().Trim()+"')", true);
            return;
        }

        string strHQL = "From Account as account where account.ID = '" + lbl_ID.Text.Trim() + "'";
        AccountBLL accountBLL = new AccountBLL();
        IList lst = accountBLL.GetAllAccounts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            Account account = (Account)lst[0];
            account.AccountName = TB_AccountName.Text.Trim();
            account.ParentID = int.Parse(DL_ParentID.SelectedValue.Trim());
            account.SortNumber = int.Parse(TB_SortNumber.Text.Trim());
            account.TypeArea = TB_TypeArea.Text.Trim();
            account.AccountType = TB_AccountType.Text.Trim();
            account.AccountCode = TB_AccountCode.Text.Trim();

            try
            {
                accountBLL.UpdateAccount(account, account.ID);
                UpDateAccountGeneralLedger(lbl_OldCode.Text.Trim(), account.AccountName.Trim(), account.AccountCode.Trim());

                AccountTree(TreeView1);
                LoadAccount();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSJBCZJC").ToString().Trim() + "')", true);
        }
    }

    protected void UpDateAccountGeneralLedger(string strOldAccountCode, string strAccountName, string strNewAccountCode)
    {
        //更新出入账的会计科目
        string strHQL = "From AccountGeneralLedger as accountGeneralLedger where accountGeneralLedger.AccountCode = '" + strOldAccountCode + "'";
        AccountGeneralLedgerBLL accountGeneralLedgerBLL = new AccountGeneralLedgerBLL();
        IList lst = accountGeneralLedgerBLL.GetAllAccountGeneralLedgers(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                AccountGeneralLedger accountGeneralLedger = (AccountGeneralLedger)lst[i];
                accountGeneralLedger.AccountCode = strNewAccountCode;
                accountGeneralLedger.AccountName = strAccountName;
                accountGeneralLedgerBLL.UpdateAccountGeneralLedger(accountGeneralLedger, accountGeneralLedger.ID);
            }
        }
        //更新应付，应收表的会计科目
        strHQL = "From ConstractPayable as constractPayable where constractPayable.AccountCode = '" + strOldAccountCode + "'";
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int j = 0; j < lst.Count; j++)
            {
                ConstractPayable constractPayable = (ConstractPayable)lst[j];
                constractPayable.AccountCode = strNewAccountCode;
                constractPayable.Account = strAccountName;
                constractPayableBLL.UpdateConstractPayable(constractPayable, constractPayable.ID);
            }
        }

        strHQL = "From ConstractReceivables as constractReceivables where constractReceivables.AccountCode = '" + strOldAccountCode + "'";
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int j = 0; j < lst.Count; j++)
            {
                ConstractReceivables constractReceivables = (ConstractReceivables)lst[j];
                constractReceivables.AccountCode = strNewAccountCode;
                constractReceivables.Account = strAccountName;
                constractReceivablesBLL.UpdateConstractReceivables(constractReceivables, constractReceivables.ID);
            }
        }
    }

    /// <summary>
    /// 获取即时会计科目ID
    /// </summary>
    /// <param name="bmbp"></param>
    /// <returns></returns>
    protected int GetMaxAccountID(Account bmbp)
    {
        string strHQL = "Select ID From T_Account where AccountName='" + bmbp.AccountName.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_Account").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        string strAccountID = treeNode.Target.Trim();

        string strHQL = "From Account as account Where account.ID='" + strAccountID + "'";
        AccountBLL accountBLL = new AccountBLL();
        IList lst = accountBLL.GetAllAccounts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            Account account = (Account)lst[0];
            DL_ParentID.SelectedValue = account.ParentID.ToString();
            TB_AccountName.Text = account.AccountName.Trim();
            TB_SortNumber.Text = account.SortNumber.ToString();
            lbl_ID.Text = account.ID.ToString();
            TB_AccountType.Text = account.AccountType.Trim();
            TB_TypeArea.Text = account.TypeArea.Trim();
            TB_AccountCode.Text = string.IsNullOrEmpty(account.AccountCode) ? "" : account.AccountCode.Trim();
            lbl_OldCode.Text = string.IsNullOrEmpty(account.AccountCode) ? "" : account.AccountCode.Trim();
        }
    }

    /// <summary>
    /// 绑定会计科目树形结构
    /// </summary>
    /// <param name="tv">树形控件</param>
    protected void AccountTree(TreeView tv)
    {
        //添加根节点
        tv.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();

        node1.Text = "0：" + LanguageHandle.GetWord("HuiJiKeMuLieBiao").ToString().Trim();
        node1.Target = "0";
        node1.Expanded = true;
        tv.Nodes.Add(node1);

        string strHQL = "From Account as account Where account.ParentID='0' Order By account.SortNumber ";
        AccountBLL accountBLL = new AccountBLL();
        IList lst = accountBLL.GetAllAccounts(strHQL);
        if (lst != null && lst.Count > 0)
        {
            for (int j = 0; j < lst.Count; j++)
            {
                node3 = new TreeNode();
                Account account = (Account)lst[j];
                node3.Text = account.AccountCode.Trim() + " " + account.AccountName;
                node3.Target = account.ID.ToString();
                node3.Expanded = true;
                node1.ChildNodes.Add(node3);

                GetAccountTreeView(account.ID.ToString(), node3);

                tv.DataBind();
            }
        }
    }

    /// <summary>
    /// 会计科目树形结构循环
    /// </summary>
    /// <param name="strParentID">上级科目ID</param>
    /// <param name="node">树形节点</param>
    protected void GetAccountTreeView(string strParentID, TreeNode node)
    {
        string strHQL = "From Account as account Where account.ParentID='" + strParentID + "' Order By account.SortNumber ";
        AccountBLL accountBLL = new AccountBLL();
        IList lst = accountBLL.GetAllAccounts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                Account account = (Account)lst[i];
                TreeNode node1 = new TreeNode();
                node1.Text = account.AccountCode.Trim() + " " + account.AccountName;
                node1.Target = account.ID.ToString();
                node1.Expanded = true;
                node.ChildNodes.Add(node1);

                GetAccountTreeView(account.ID.ToString(), node1);
            }
        }
    }

    /// <summary>
    /// 绑定科目DDL
    /// </summary>
    protected void LoadAccount()
    {
        DataTable dt = GetAccountList(string.Empty);
        if (dt != null && dt.Rows.Count > 0)
        {
            DL_ParentID.Items.Clear();
            DL_ParentID.Items.Insert(0, new ListItem("PrimarySubject", "0"));  //ChineseWord //ChineseWord
            SetInterval(DL_ParentID, "0", " ");
        }
        else
        {
            DL_ParentID.Items.Clear();
            DL_ParentID.Items.Insert(0, new ListItem("PrimarySubject", "0"));  //ChineseWord //ChineseWord
        }
    }

    protected DataTable GetAccountList(string strParentID)
    {
        string strHQL = "Select * From T_Account ";
        if (!string.IsNullOrEmpty(strParentID))
        {
            strHQL += " Where ParentID='" + strParentID.Trim() + "' ";
        }
        strHQL += " Order By SortNumber ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Account");
        return ds.Tables[0];
    }

    protected void SetInterval(DropDownList DDL, string strParentID, string interval)
    {
        interval += "├";

        DataTable list = GetAccountList(strParentID);
        if (list.Rows.Count > 0 && list != null)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                DDL.Items.Add(new ListItem(string.Format("{0}{1}", interval, list.Rows[i]["AccountType"].ToString().Trim() + "-" + list.Rows[i]["AccountName"].ToString().Trim()), list.Rows[i]["ID"].ToString().Trim()));

                ///递归
                SetInterval(DDL, list.Rows[i]["ID"].ToString().Trim(), interval);
            }
        }
    }

    protected void DL_ParentID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From Account as account Where account.ID='" + DL_ParentID.SelectedValue.Trim() + "' ";
        AccountBLL accountBLL = new AccountBLL();
        IList lst = accountBLL.GetAllAccounts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            Account account = (Account)lst[0];
            TB_AccountType.Text = account.AccountType.Trim();
            TB_TypeArea.Text = account.TypeArea.Trim();
        }
    }

    protected void DL_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_AccountType.Text = DL_Type.SelectedValue.Trim();
    }

    protected void DL_Area_SelectedIndexChanged(object sender, EventArgs e)
    {
        TB_TypeArea.Text = DL_Area.SelectedValue.Trim();
    }

    protected void BT_AccountDelete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCode = lbl_ID.Text.Trim();
        if (lbl_ID.Text.Trim() == "" || string.IsNullOrEmpty(lbl_ID.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZDiShiGaiLanguageHandleGetWor").ToString().Trim()+"')", true); 
            return;
        }
        if (IsAccountChild(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSGKMXYZKMXSCZKM").ToString().Trim() + "')", true);
            return;
        }
        strHQL = "From Account as account where account.ID = '" + strCode + "'";
        AccountBLL accountBLL = new AccountBLL();
        IList lst = accountBLL.GetAllAccounts(strHQL);
        Account account = (Account)lst[0];

        strHQL = "Delete From T_Account Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            AccountTree(TreeView1);
            LoadAccount();
            if (IsAccountCode(account.AccountCode.Trim()))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCGGKMBMYBYYJKBCGKMBM").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected bool IsAccountChild(string strID)
    {
        bool flag1 = false;
        string strHQL = "From Account as account where account.ParentID = '" + strID + "'";
        AccountBLL accountBLL = new AccountBLL();
        IList lst = accountBLL.GetAllAccounts(strHQL);
        if (lst != null && lst.Count > 0)
        {
            flag1 = true;
        }
        return flag1;
    }

    protected bool IsAccountCode(string strAccountCode)
    {
        bool flag1 = false, flag2 = false, flag3 = false;
        string strHQL = "From AccountGeneralLedger as accountGeneralLedger where accountGeneralLedger.AccountCode = '" + strAccountCode + "'";
        AccountGeneralLedgerBLL accountGeneralLedgerBLL = new AccountGeneralLedgerBLL();
        IList lst = accountGeneralLedgerBLL.GetAllAccountGeneralLedgers(strHQL);
        if (lst != null && lst.Count > 0)
        {
            flag1 = true;
        }

        strHQL = "From ConstractPayable as constractPayable where constractPayable.AccountCode = '" + strAccountCode + "'";
        ConstractPayableBLL constractPayableBLL = new ConstractPayableBLL();
        lst = constractPayableBLL.GetAllConstractPayables(strHQL);
        if (lst != null && lst.Count > 0)
        {
            flag2 = true;
        }

        strHQL = "From ConstractReceivables as constractReceivables where constractReceivables.AccountCode = '" + strAccountCode + "'";
        ConstractReceivablesBLL constractReceivablesBLL = new ConstractReceivablesBLL();
        lst = constractReceivablesBLL.GetAllConstractReceivabless(strHQL);
        if (lst != null && lst.Count > 0)
        {
            flag3 = true;
        }

        if (flag1 || flag2 || flag3)
        {
            return true;
        }
        else
            return false;
    }
}
