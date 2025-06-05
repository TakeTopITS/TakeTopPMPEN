using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTMakeProjectBudget : System.Web.UI.Page
{
    string strProjectID, strProjectStatus;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName, strProjectName;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strProjectID = Request.QueryString["ProjectID"];
        Project project = GetProject(strProjectID);

        strProjectName = project.ProjectName.Trim();
        strProjectStatus = project.Status.Trim();

        LB_CurrencyType.Text = project.CurrencyType;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_CreateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadProjectBudget(strProjectID);

            ShareClass.InitialProjectMemberTree(TreeView1, strProjectID);

            //取得会计科目列表
            ShareClass.LoadAccountForDDL(DL_Account);

            LB_ProjectID.Text = strProjectID;
            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = strUserName;


            if (strProjectStatus == "CaseClosed" || strProjectStatus == "Suspended" || strProjectStatus == "Cancel")
            {
                //BT_New.Enabled = false;
                //BT_Update.Enabled = false;
                //BT_Delete.Enabled = false;
            }

            LoadProjectExpenseByAccount(strProjectID, "All");
        }
    }

    protected void BT_CopyFromParentProject_Click(object sender, EventArgs e)
    {
        string strParentProjectID;

        strParentProjectID = ShareClass.GetProject(strProjectID).ParentID.ToString();

        LoadParentProjectBudget(strParentProjectID);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popParentPJBudgetWindow','false') ", true);
    }

    protected void CB_SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (CB_SelectAll.Checked == true)
        {
            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                ((CheckBox)DataGrid2.Items[i].FindControl("CB_Select")).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                ((CheckBox)DataGrid2.Items[i].FindControl("CB_Select")).Checked = false;
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popParentPJBudgetWindow','false') ", true);
    }

    protected void BT_CopyFromParentItem_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strID, strAmount, strAccount;

        for (int i = 0; i < DataGrid2.Items.Count; i++)
        {
            if (((CheckBox)DataGrid2.Items[i].FindControl("CB_Select")).Checked == true)
            {
                strID = DataGrid2.Items[i].Cells[1].Text;
                strAccount = DataGrid2.Items[i].Cells[2].Text;

                //判断预算科目是否存在
                if (CheckProjectBudgetAccuntIsExit(strAccount, strProjectID))
                {
                    continue;
                }

                strAmount = ((TextBox)(DataGrid2.Items[i].FindControl("TB_Amount"))).Text;
                try
                {
                    decimal.Parse(strAmount);
                }
                catch
                {
                    strAmount = "0";
                }

                try
                {
                    strHQL = string.Format(@"INSERT INTO T_ProjectBudget
                       (Account
                       ,Description
                       ,ProjectID
                       ,Amount
                       ,CreatorCode
                       ,CreatorName
                       ,CreateTime
                       ,AccountCode
                       ,CurrencyType
                       ,XuHao
                       ,XiangMuFenBu
                       ,XiangMuFenXiang
                       ,XiangMuFenLei
                       ,DanWei
                       ,YuSuanZongLiang
                       ,YuSuanDanJia
                       ,YuSuanZongE
                       ,BeiZhu
                       ,XiangMuMingChengHuoGuiGe
                       ,FromParentID)
                        Select 
                       Account
                       ,Description
                       ,{0}
                       ,{1}
                       ,CreatorCode
                       ,CreatorName
                       ,CreateTime
                       ,AccountCode
                       ,CurrencyType
                       ,XuHao
                       ,XiangMuFenBu
                       ,XiangMuFenXiang
                       ,XiangMuFenLei
                       ,DanWei
                       ,YuSuanZongLiang
                       ,YuSuanDanJia
                       ,YuSuanZongE
                       ,BeiZhu
                       ,XiangMuMingChengHuoGuiGe,{2} From T_ProjectBudget Where ID = {2}", strProjectID, strAmount, strID);

                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = string.Format(@"Update T_ProjectBudget Set Amount = Amount - {0} Where ID = {1}", strAmount, strID);
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch
                {
                }
            }
        }


        LoadProjectBudget(strProjectID);


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZWC") + "')", true);
        //ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popParentPJBudgetWindow','false') ", true);
    }

    //判断预算科目是否存在
    protected bool CheckProjectBudgetAccuntIsExit(string strAccount, string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectBudget as projectBudget where projectBudget.ProjectID = " + strProjectID;
        strHQL += " and projectBudget.Account = " + "'" + strAccount + "'";

        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
        if (lst.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            IList lst;

            string strID = e.Item.Cells[2].Text.Trim();
            string strAccount = e.Item.Cells[3].Text.Trim();
            string strUserCode = LB_UserCode.Text.Trim();

            if (e.CommandName == "Update" | e.CommandName == "Select")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From ProjectBudget as projectBudget Where projectBudget.ID = " + strID;
                ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
                lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);

                ProjectBudget projectBudget = (ProjectBudget)lst[0];

                LB_ID.Text = strID;
                lbl_AccountCode.Text = projectBudget.AccountCode;
                TB_Account.Text = strAccount;
                TB_Description.Text = projectBudget.Description;
                NB_Amount.Amount = projectBudget.Amount;
                LB_CurrencyType.Text = projectBudget.CurrencyType;

                DLC_CreateTime.Text = projectBudget.CreateTime.ToString("yyyy-MM-dd");

                LB_Account.Text = strAccount;
                LoadProjectExpenseByAccount(strProjectID, strAccount);

                if (strProjectStatus == "CaseClosed" || strProjectStatus == "Suspended" || strProjectStatus == "Cancel")
                {
                    //BT_New.Enabled = false;
                    //BT_Update.Enabled = false;
                    //BT_Delete.Enabled = false;
                }
                else
                {
                    //BT_New.Enabled = true;
                    //BT_Update.Enabled = true;
                    //BT_Delete.Enabled = true;
                }

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Select")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDetailWindow','true') ", true);
                }

            }

            if (e.CommandName == "Delete")
            {
                strHQL = "from ProjectBudget as projectBudget where projectBudget.ID = " + strID;
                ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
                lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
                ProjectBudget projectBudget = (ProjectBudget)lst[0];

                try
                {
                    //更改父项目相应预算金额
                    UpdateParentItemByFromParentItem(strProjectID, strID);

                    projectBudgetBLL.DeleteProjectBudget(projectBudget);

                    LoadProjectBudget(strProjectID);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    //更改父项目相应预算金额
    protected void UpdateParentItemByFromParentItem(string strProjectID, string strID)
    {
        string strHQL;
        string strFromParentID, strAccount, strParentProjectID, strAmount;

        strHQL = "Select FromParentID,Account,Amount From T_ProjectBudget Where ID =" + strID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectBudget");

        strFromParentID = ds.Tables[0].Rows[0]["FromParentID"].ToString();

        if (strFromParentID != "0" & strFromParentID != null)
        {
            strParentProjectID = ShareClass.GetProject(strProjectID).ParentID.ToString();
            strAccount = ds.Tables[0].Rows[0]["Account"].ToString().Trim();

            strHQL = "Select COALESCE(Sum(Amount),0) From T_ProjectBudget Where FromParentID = " + strFromParentID;
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectBudget");
            strAmount = ds.Tables[0].Rows[0][0].ToString();

            strHQL = "Update T_ProjectBudget Set Amount = Amount + " + strAmount + " Where Account = '" + strAccount + "' and ProjectID = " + strParentProjectID;
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void DL_Account_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strAccountCode = DL_Account.SelectedValue.Trim();
        lbl_AccountCode.Text = strAccountCode;
        TB_Account.Text = ShareClass.GetAccountName(strAccountCode);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddBudget();
        }
        else
        {
            UpdateBudget();
        }
    }

    protected void AddBudget()
    {
        string strHQL;
        IList lst;

        decimal deAmount = NB_Amount.Amount;

        string strProjectID = LB_ProjectID.Text.Trim();
        string strUserCode = LB_UserCode.Text.Trim();
        string strUserName = LB_UserName.Text.Trim();
        string strAccount = TB_Account.Text.Trim();
        string strDescription = TB_Description.Text.Trim();

        if (deAmount == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZXZSBYSJEBNW0JC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        ProjectBudget projectBudget = new ProjectBudget();

        strHQL = "From ProjectBudget as projectBudget where projectBudget.ProjectID = " + strProjectID;
        strHQL += " and projectBudget.Account = " + "'" + strAccount + "'";
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
        if (lst.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click33", "alert('" + LanguageHandle.GetWord("ZZXZSBBNZFZJXTKMJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        //检查正在编辑的预算是不是大于项目总预算
        if (CheckTotalAccountBudgetIsLargeProjectBudget(strProjectID, NB_Amount.Amount, "ADD"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click44", "alert('" + LanguageHandle.GetWord("ZZSBYSZHZGLXMDYDYSQJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        projectBudget.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        projectBudget.Account = strAccount;
        projectBudget.ProjectID = int.Parse(strProjectID);
        projectBudget.Description = strDescription;
        projectBudget.Amount = NB_Amount.Amount;
        projectBudget.CurrencyType = LB_CurrencyType.Text.Trim();
        projectBudget.CreatorCode = strUserCode;
        projectBudget.CreatorName = strUserName;
        projectBudget.CreateTime = DateTime.Parse(DLC_CreateTime.Text);

        try
        {
            projectBudgetBLL.AddProjectBudget(projectBudget);
            LB_ID.Text = ShareClass.GetMyCreatedMaxProBudgetID(strProjectID);

            LoadProjectBudget(strProjectID);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click55", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateBudget()
    {
        string strHQL, strHQL1;
        IList lst;

        string strUserCode;
        string strID, strProjectID, strAccount;
        decimal deAmount;

        strID = LB_ID.Text.Trim();
        strProjectID = LB_ProjectID.Text.Trim();
        strAccount = TB_Account.Text.Trim();
        deAmount = NB_Amount.Amount;

        if (deAmount == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click22", "alert('" + LanguageHandle.GetWord("ZZXZSBYSJEBNW0JC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        //检查正在编辑的预算是不是大于项目总预算
        if (CheckTotalAccountBudgetIsLargeProjectBudget(strProjectID, NB_Amount.Amount, "EDIT"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click44", "alert('" + LanguageHandle.GetWord("ZZSBYSZHZGLXMDYDYSQJC") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();

        strHQL = "from ProjectBudget as projectBudget where projectBudget.ID = " + strID;
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
        ProjectBudget projectBudget = (ProjectBudget)lst[0];

        strHQL1 = "Insert Into T_ProjectBudgetChangeLog(BudgetID,AccountCode,account,Description,Amount,CurrencyType,CreatorCode,CreatorName,UpdaterCode,UpdaterName,UpdateTime)";
        strHQL1 += " Values(" + strID + ",'" + projectBudget.AccountCode + "','" + projectBudget.Account + "','" + projectBudget.Description + "'," + projectBudget.Amount.ToString() + ",'" + projectBudget.CurrencyType + "','" + projectBudget.CreatorCode + "','" + projectBudget.CreatorName + "','" + Session["UserCode"].ToString() + "','" + ShareClass.GetUserName(Session["UserCode"].ToString()) + "',now())";

        projectBudget.AccountCode = string.IsNullOrEmpty(lbl_AccountCode.Text) ? "" : lbl_AccountCode.Text.Trim();
        projectBudget.Account = TB_Account.Text.Trim();
        projectBudget.Description = TB_Description.Text.Trim();
        projectBudget.Amount = NB_Amount.Amount;
        projectBudget.CurrencyType = LB_CurrencyType.Text.Trim();
        strUserCode = projectBudget.CreatorCode.Trim();
        strProjectID = projectBudget.ProjectID.ToString();

        projectBudget.CreateTime = DateTime.Parse(DLC_CreateTime.Text);

        try
        {
            projectBudgetBLL.UpdateProjectBudget(projectBudget, int.Parse(strID));
          
            LoadProjectBudget(strProjectID);

            //插入操作日志
            ShareClass.RunSqlCommand(strHQL1);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click55", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click66", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    //检查正在添加的预算是不是大于项目总预算
    protected bool CheckTotalAccountBudgetIsLargeProjectBudget(string strProjectID, decimal deAccountBudget, string strOperationType)
    {
        string strHQL;
        decimal deProjectBudget, deSumProjectBudget;

        deProjectBudget = GetProjectBudget(strProjectID);

        if (strOperationType == "ADD")
        {
            strHQL = "Select COALESCE(Sum(Amount),0) From T_ProjectBudget Where ProjectID = " + strProjectID;
        }
        else
        {
            strHQL = "Select COALESCE(Sum(Amount),0) From T_ProjectBudget Where ProjectID = " + strProjectID;
            strHQL += " And ID <> " + LB_ID.Text;
        }
        DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectBudget");
        if (ds2.Tables[0].Rows.Count > 0)
        {
            deSumProjectBudget = decimal.Parse(ds2.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            deSumProjectBudget = 0;
        }
        deSumProjectBudget += deAccountBudget;

        if (deSumProjectBudget > deProjectBudget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //检查正在添加的预算是不是大于项目总预算
    protected decimal GetProjectBudget(string strProjectID)
    {
        string strHQL;

        decimal deProjectBudget;

        strHQL = "Select Budget From T_Project Where ProjectID = " + strProjectID;
        DataSet ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_Project");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            deProjectBudget = decimal.Parse(ds1.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            deProjectBudget = 0;
        }

        return deProjectBudget;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        IList lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void BT_AllMember_Click(object sender, EventArgs e)
    {
        LB_OperatorCode.Text = "All";
        LB_OperatorName.Text = "";

        LB_Account.Text = "All";

        LoadProjectExpenseByAccount(strProjectID, "All");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDetailWindow','true') ", true);
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID;
        string strUserCode, strUserName;
        string strAccount;
        decimal deExpense = 0, deConfirmExpense = 0;

        strID = TreeView1.SelectedNode.Target.Trim();
        strAccount = LB_Account.Text.Trim();

        try
        {
            strID = int.Parse(strID).ToString();

            strHQL = "from ProRelatedUser as proRelatedUser Where proRelatedUser.ID = " + strID;
            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst.Count > 0)
            {
                ProRelatedUser proRelatedUser = (ProRelatedUser)lst[0];

                strUserCode = proRelatedUser.UserCode.Trim();
                strUserName = proRelatedUser.UserName.Trim();

                if (strAccount == "All")
                {
                    strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.UserCode = " + "'" + strUserCode + "'";
                    strHQL += " Order by proExpense.ID DESC";
                }
                else
                {
                    strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID + " and proExpense.UserCode = " + "'" + strUserCode + "'";
                    strHQL += " and proExpense.Account = " + "'" + strAccount + "'";
                    strHQL += " Order by proExpense.ID DESC";
                }
                ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
                lst = proExpenseBLL.GetAllProExpenses(strHQL);
                DataList1.DataSource = lst;
                DataList1.DataBind();

                ProExpense proExpense = new ProExpense();
                for (int i = 0; i < lst.Count; i++)
                {
                    proExpense = (ProExpense)lst[i];
                    deExpense += proExpense.Amount;
                    deConfirmExpense += proExpense.ConfirmAmount;
                }

                LB_Amount.Text = deExpense.ToString();
                LB_ConfirmAmount.Text = deConfirmExpense.ToString();

                LB_OperatorCode.Text = strUserCode;
                LB_OperatorName.Text = strUserName;

                LB_Sql.Text = strHQL;
            }
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popDetailWindow','true') ", true);
    }

    protected void LoadProjectBudget(string strProjectID)
    {
        string strHQL;
        IList lst;

        decimal deBudget = 0;

        strHQL = "From ProjectBudget as projectBudget Where projectBudget.ProjectID = " + strProjectID;
        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        ProjectBudget projectBudget = new ProjectBudget();

        for (int i = 0; i < lst.Count; i++)
        {
            projectBudget = (ProjectBudget)lst[i];
            deBudget += projectBudget.Amount;
        }

        LB_ProBudget.Text = GetProjectBudget(strProjectID).ToString();
        LB_TotalACBudget.Text = deBudget.ToString();

        LB_ProExpense.Text = GetProjectExpense(strProjectID);
        LB_ConfirmProExpense.Text = GetProjectConfirmExpense(strProjectID);

        FinishPercentPicture(strProjectID);
    }

    protected void LoadParentProjectBudget(string strParentProjectID)
    {
        string strHQL;
        IList lst;

        string strAccount;

        strHQL = "From ProjectBudget as projectBudget Where projectBudget.ProjectID = " + strParentProjectID;
        ProjectBudgetBLL projectBudgetBLL = new ProjectBudgetBLL();
        lst = projectBudgetBLL.GetAllProjectBudgets(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        ProjectBudget projectBudget;

        for (int i = 0; i < lst.Count; i++)
        {
            projectBudget = (ProjectBudget)lst[i];

            strAccount = projectBudget.Account.Trim();

            //判断预算科目是否存在
            if (CheckProjectBudgetAccuntIsExit(strAccount, strProjectID))
            {
                DataGrid2.Items[i].ForeColor = Color.Red;
            }
        }
    }


    protected void LoadProjectExpenseByAccount(string strProjectID, string strAccount)
    {
        string strHQL;
        IList lst;

        decimal deExpense = 0, deConfirmExpense = 0;

        if (strAccount == "All")
        {
            strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID;
            strHQL += " Order by proExpense.ID DESC";
        }
        else
        {
            strHQL = "from ProExpense as proExpense where proExpense.ProjectID = " + strProjectID;
            strHQL += " and proExpense.Account = " + "'" + strAccount + "'";
            strHQL += " Order by proExpense.ID DESC";
        }

        ProExpenseBLL proExpenseBLL = new ProExpenseBLL();
        lst = proExpenseBLL.GetAllProExpenses(strHQL);
        DataList1.DataSource = lst;
        DataList1.DataBind();

        ProExpense proExpense = new ProExpense();
        for (int i = 0; i < lst.Count; i++)
        {
            proExpense = (ProExpense)lst[i];
            deExpense += proExpense.Amount;
            deConfirmExpense += proExpense.ConfirmAmount;
        }

        LB_Amount.Text = deExpense.ToString();
        LB_ConfirmAmount.Text = deConfirmExpense.ToString();

        LB_OperatorCode.Text = "All";
        LB_OperatorName.Text = "";
    }

    protected void FinishPercentPicture(string strProjectID)
    {
        string strAccount;

        decimal deAccountExpense, deAccountBudget;
        decimal deWidth;

        int intWidth;
        int i, j = 0;

        for (i = 0; i < DataGrid1.Items.Count; i++)
        {
            strAccount = DataGrid1.Items[i].Cells[3].Text.Trim();

            deAccountBudget = decimal.Parse(DataGrid1.Items[i].Cells[4].Text.Trim());
            deAccountExpense = GetProjectAccountTotalAmount(strProjectID, strAccount);

            if (deAccountBudget == 0)
            {
                deAccountBudget = 1;
                j = 0;
            }
            else
            {
                j = 1;
            }

            deWidth = (deAccountExpense / deAccountBudget) * 100;
            deWidth = System.Decimal.Round(deWidth, 0);

            intWidth = int.Parse(deWidth.ToString());

            if (intWidth > 200)
            {
                intWidth = 200;
            }

            if (j == 0)
            {
                deAccountBudget = 0;
                deWidth = 0;
            }

            if (intWidth > 25)
            {
                ((System.Web.UI.WebControls.Label)DataGrid1.Items[i].FindControl("LB_FinishPercent")).Width = Unit.Pixel(intWidth);
            }

            ((System.Web.UI.WebControls.Label)DataGrid1.Items[i].FindControl("LB_FinishPercent")).Text = deAccountExpense.ToString();

            ((System.Web.UI.WebControls.Label)DataGrid1.Items[i].FindControl("LB_DefaultPercent")).Width = Unit.Pixel(100);

            ((System.Web.UI.WebControls.Label)DataGrid1.Items[i].FindControl("LB_DefaultPercent")).Text = deAccountBudget.ToString();
        }
    }

    protected decimal GetProjectAccountTotalAmount(string strProjectID, string strAccount)
    {
        string strHQL;

        strHQL = "Select Sum(ConfirmAmount) From T_ProExpense Where ProjectID = " + strProjectID + " and Account = " + "'" + strAccount + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");

        try
        {
            return decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        catch
        {
            return 0;
        }
    }

    protected string GetProjectExpense(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(Amount),0) From T_ProExpense Where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");


        try
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        catch
        {
            return "0";
        }
    }


    protected string GetProjectConfirmExpense(string strProjectID)
    {
        string strHQL;

        strHQL = "Select COALESCE(Sum(ConfirmAmount),0) From T_ProExpense Where ProjectID = " + strProjectID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProExpense");


        try
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        catch
        {
            return "0";
        }
    }

    protected Project GetProject(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project;
    }

    protected string GetProjectStatus(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.Status.Trim();
    }

    protected string GetProjectCurrencyType(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.CurrencyType.Trim();
    }



}
