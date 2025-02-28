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

public partial class TTBDBaseOperationData : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "预算费用管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            DLC_YearNum.Text = DateTime.Now.ToString("yyyy");
            DLC_MonthNum.Text = DateTime.Now.ToString("MM");
            lbl_Creater.Text = ShareClass.GetUserName(strUserCode.Trim());

            lbl_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);

            ShareClass.LoadAccountForDDL(ddl_AccountName);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;
        if (treeNode.Target != "0")
        {
            lbl_DCode.Text = treeNode.Target.Trim();
            lbl_DName.Text = ShareClass.GetDepartName(lbl_DCode.Text);
            TB_DepartName.Text = ShareClass.GetDepartName(lbl_DCode.Text);
            //DL_DepartCode.SelectedValue = lbl_DCode.Text.Trim();
            lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim()), "Base");

            LoadBMBaseDataList(treeNode.Target.Trim());
            LoadBMBaseDataRecordList();
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddBudgetBaseDataRecord();
        }
        else
        {
            UpdateBudgetBaseDataRecord();
        }
    }

    protected void AddBudgetBaseDataRecord()
    {
        if (string.IsNullOrEmpty(lbl_DCode.Text) || lbl_DCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBMBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        if (string.IsNullOrEmpty(DLC_YearNum.Text.Trim()) || DLC_YearNum.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYSNFBTJC").ToString().Trim() + "')", true);
            DLC_YearNum.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        //if (IsBMBaseDataExits(string.Empty, lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim()), strUserCode.Trim()))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGNFBMYSYCZJC").ToString().Trim() + "')", true);
        //    ddl_AccountName.Focus();
        //    DLC_YearNum.Focus();
        //    DLC_MonthNum.Focus();

        //    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        //    return;
        //}

        lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(DLC_MonthNum.Text.Trim()), "Base");
        if (NB_MoneyNum.Amount > decimal.Parse(lbl_MoneyNum.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGNFBMYSFYBZJC").ToString().Trim() + "')", true);
            NB_MoneyNum.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        BDBaseDataRecordBLL bdBaseDataRecordBLL = new BDBaseDataRecordBLL();
        BDBaseDataRecord bdBaseDataRecord = new BDBaseDataRecord();

        bdBaseDataRecord.AccountCode = ddl_AccountName.SelectedValue.Trim();
        bdBaseDataRecord.AccountName = ddl_AccountName.SelectedItem.Text.Trim();
        bdBaseDataRecord.BDBaseDataID = int.Parse(LB_BudgetID.Text);
        bdBaseDataRecord.DepartCode = lbl_DCode.Text.Trim();
        bdBaseDataRecord.DepartName = lbl_DName.Text.Trim();
        bdBaseDataRecord.EnterCode = strUserCode.Trim();
        bdBaseDataRecord.MoneyNum = NB_MoneyNum.Amount;
        bdBaseDataRecord.MonthNum = int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim());
        bdBaseDataRecord.Type = "Operation";
        bdBaseDataRecord.YearNum = int.Parse(DLC_YearNum.Text.Trim());

        try
        {
            bdBaseDataRecordBLL.AddBDBaseDataRecord(bdBaseDataRecord);
            LB_ID.Text = GetMaxBMBaseDataRecordID().ToString();

            LoadBMBaseDataRecordList();

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;

            lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim()), "Base");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateBudgetBaseDataRecord()
    {
        if (string.IsNullOrEmpty(lbl_DCode.Text) || lbl_DCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBMBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        if (string.IsNullOrEmpty(DLC_YearNum.Text.Trim()) || DLC_YearNum.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYSNFBTJC").ToString().Trim() + "')", true);
            DLC_YearNum.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        //if (IsBMBaseDataExits(String.Empty, lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(DLC_MonthNum.Text.Trim()), strUserCode.Trim()))
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGNFBMYSYCZJC").ToString().Trim() + "')", true);
        //    ddl_AccountName.Focus();
        //    DLC_YearNum.Focus();
        //    DLC_MonthNum.Focus();

        //    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        //    return;
        //}

        lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim()), "Base");

        string strHQL = "From BDBaseDataRecord as bDBaseData where bDBaseData.ID = '" + LB_ID.Text.Trim() + "'";
        BDBaseDataRecordBLL bdBaseDataRecordBLL = new BDBaseDataRecordBLL();
        IList lst = bdBaseDataRecordBLL.GetAllBDBaseDataRecords(strHQL);
        BDBaseDataRecord bdBaseDataRecord = (BDBaseDataRecord)lst[0];

        decimal strMoneyOld = bdBaseDataRecord.MoneyNum;
        if (NB_MoneyNum.Amount > decimal.Parse(lbl_MoneyNum.Text.Trim()) + strMoneyOld)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGNFBMYSFYBZJC").ToString().Trim() + "')", true);
            NB_MoneyNum.Focus();
            return;
        }
        bdBaseDataRecord.BDBaseDataID = int.Parse(LB_BudgetID.Text);
        bdBaseDataRecord.AccountCode = ddl_AccountName.SelectedValue.Trim();
        bdBaseDataRecord.AccountName = ddl_AccountName.SelectedItem.Text.Trim();
        bdBaseDataRecord.DepartCode = lbl_DCode.Text.Trim();
        bdBaseDataRecord.DepartName = lbl_DName.Text.Trim();
        bdBaseDataRecord.MoneyNum = NB_MoneyNum.Amount;
        bdBaseDataRecord.MonthNum = int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim());
        bdBaseDataRecord.YearNum = int.Parse(DLC_YearNum.Text.Trim());

        try
        {
            bdBaseDataRecordBLL.UpdateBDBaseDataRecord(bdBaseDataRecord, bdBaseDataRecord.ID);

            LoadBMBaseDataRecordList();

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim()), "Base");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From BDBaseDataRecord as BDBaseDataRecord where BDBaseDataRecord.ID = '" + strId + "'";
                BDBaseDataRecordBLL bdBaseDataRecordBLL = new BDBaseDataRecordBLL();
                lst = bdBaseDataRecordBLL.GetAllBDBaseDataRecords(strHQL);
                BDBaseDataRecord bdBaseDataRecord = (BDBaseDataRecord)lst[0];

                LB_ID.Text = bdBaseDataRecord.ID.ToString().Trim();
                lbl_DCode.Text = bdBaseDataRecord.DepartCode.Trim();
                lbl_DName.Text = bdBaseDataRecord.DepartName.Trim();
                DLC_YearNum.Text = bdBaseDataRecord.YearNum.ToString();
                DLC_MonthNum.Text = bdBaseDataRecord.MonthNum >= 10 ? bdBaseDataRecord.MonthNum.ToString() : bdBaseDataRecord.MonthNum == 0 ? "" : "0" + bdBaseDataRecord.MonthNum.ToString();
                NB_MoneyNum.Amount = bdBaseDataRecord.MoneyNum;
                ddl_AccountName.SelectedValue = bdBaseDataRecord.AccountCode.Trim();
                lbl_Creater.Text = ShareClass.GetUserName(bdBaseDataRecord.EnterCode.Trim());

                lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(bdBaseDataRecord.DepartCode.Trim(), bdBaseDataRecord.AccountCode, bdBaseDataRecord.YearNum, bdBaseDataRecord.MonthNum, "Base");

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }

            if (e.CommandName == "Delete")
            {
                strHQL = "Delete From T_BDBaseDataRecord Where ID = " + strId;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadBMBaseDataRecordList();

                    //BT_Update.Visible = false;
                    //BT_Delete.Visible = false;

                    lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(DLC_MonthNum.Text.Trim()), "Base");

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
                }
            }
        }
    }

    protected void DataGrid3_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = e.Item.Cells[0].Text.Trim();

            if (e.CommandName == "Select")
            {
                for (int i = 0; i < DataGrid3.Items.Count; i++)
                {
                    DataGrid3.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From BDBaseData as bdBaseData where bdBaseData.ID = '" + strId + "'";
                BDBaseDataBLL bdBaseDataBLL = new BDBaseDataBLL();
                lst = bdBaseDataBLL.GetAllBDBaseDatas(strHQL);
                BDBaseData bdBaseData = (BDBaseData)lst[0];

                LB_BudgetID.Text = bdBaseData.ID.ToString().Trim();
                lbl_DCode.Text = bdBaseData.DepartCode.Trim();
                lbl_DName.Text = bdBaseData.DepartName.Trim();
                DLC_YearNum.Text = bdBaseData.YearNum.ToString();
                DLC_MonthNum.Text = bdBaseData.MonthNum >= 10 ? bdBaseData.MonthNum.ToString() : bdBaseData.MonthNum == 0 ? "" : "0" + bdBaseData.MonthNum.ToString();
                ddl_AccountName.SelectedValue = bdBaseData.AccountCode.Trim();

                lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(bdBaseData.DepartCode.Trim(), bdBaseData.AccountCode.Trim(), bdBaseData.YearNum, bdBaseData.MonthNum, "Base");

                TB_DepartName.Text = bdBaseData.DepartName;
                TB_AccountName.Text = bdBaseData.AccountName;
                TB_Year.Text = bdBaseData.YearNum.ToString();
                TB_Month.Text = bdBaseData.MonthNum.ToString();

                LoadBMBaseDataRecordList();
            }
        }
    }

    protected void UpdateProjectCostManage(string strID)
    {
        ProjectCostManageBLL projectCostManageBLL = new ProjectCostManageBLL();
        string strHQL = "from ProjectCostManage as projectCostManage where projectCostManage.ID = '" + strID + "' ";
        IList lst = projectCostManageBLL.GetAllProjectCostManages(strHQL);
        ProjectCostManage projectCostManage = (ProjectCostManage)lst[0];

        projectCostManage.Total = NB_MoneyNum.Amount;

        projectCostManageBLL.UpdateProjectCostManage(projectCostManage, projectCostManage.ID);
    }



    /// <summary>
    /// 判断是否已设置标准金额
    /// </summary>
    /// <param name="strID"></param>
    /// <param name="strDepartCode"></param>
    /// <param name="strAccountName"></param>
    /// <param name="strYearNum"></param>
    /// <param name="strMonthNum"></param>
    /// <returns></returns>
    protected bool IsBMBaseDataExits(string strID, string strDepartCode, string strAccountName, int strYearNum, int strMonthNum, string strusercode)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountName='" + strAccountName + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.EnterCode='" + strusercode + "' and bDBaseData.Type='Operation' ";
        }
        else
        {
            strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountName='" + strAccountName + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.EnterCode='" + strusercode + "' and bDBaseData.Type='Operation' and bDBaseData.ID<>'" + strID + "' ";
        }
        BDBaseDataBLL bdBaseDataRecordBLL = new BDBaseDataBLL();
        IList lst = bdBaseDataRecordBLL.GetAllBDBaseDatas(strHQL);
        if (lst.Count > 0 && lst != null)
            flag = true;
        else
            flag = false;

        return flag;
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseDataRecord");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DataGrid3_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql3.Text.Trim();
        DataGrid3.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");
        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMBaseDataRecordList();
    }

    protected void ddl_AccountName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim()), "Base");

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DLC_YearNum_TextChanged(object sender, EventArgs e)
    {
        lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim()), "Base");
    }

    protected void DLC_MonthNum_TextChanged(object sender, EventArgs e)
    {
        lbl_MoneyNum.Text = GetBMBaseDataMoneyNum(lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim()), "Base");
    }

    protected String GetBMBaseDataMoneyNum(string strDepartCode, string strAccountCode, int strYearNum, int strMonthNum, string strType)
    {
        decimal deMoneyBase = 0;
        decimal deMoneyUsed = 0;
        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountCode='" + strAccountCode + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.Type='" + strType + "' ";
        BDBaseDataBLL bdBaseDataBLL = new BDBaseDataBLL();
        IList lst = bdBaseDataBLL.GetAllBDBaseDatas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                BDBaseData bdBaseData = (BDBaseData)lst[i];
                deMoneyBase += bdBaseData.MoneyNum;
            }
        }

        BDBaseDataRecordBLL bdBaseDataRecordBLL = new BDBaseDataRecordBLL();
        strHQL = "From BDBaseDataRecord as bdBaseDataRecord where bdBaseDataRecord.DepartCode = '" + strDepartCode + "' and bdBaseDataRecord.AccountCode='" + strAccountCode + "' and " +
                "bdBaseDataRecord.YearNum='" + strYearNum.ToString() + "' and bdBaseDataRecord.MonthNum = '" + strMonthNum.ToString() + "' and bdBaseDataRecord.Type='Operation' ";
        lst = bdBaseDataRecordBLL.GetAllBDBaseDataRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int j = 0; j < lst.Count; j++)
            {
                BDBaseDataRecord bdBaseDataRecord = (BDBaseDataRecord)lst[j];
                deMoneyUsed += bdBaseDataRecord.MoneyNum;
            }
        }

        return (deMoneyBase - deMoneyUsed).ToString();
    }

    protected void AddBMBaseDataRecordData(string strID, string strParaValue)
    {
        BDBaseDataRecordBLL bdBaseDataRecordRecordBLL = new BDBaseDataRecordBLL();
        BDBaseDataRecord bdBaseDataRecordRecord = new BDBaseDataRecord();

        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.ID = '" + strID + "'";
        BDBaseDataBLL bdBaseDataRecordBLL = new BDBaseDataBLL();
        IList lst = bdBaseDataRecordBLL.GetAllBDBaseDatas(strHQL);
        BDBaseData bmp = (BDBaseData)lst[0];

        bdBaseDataRecordRecord.AccountCode = bmp.AccountCode.Trim();
        bdBaseDataRecordRecord.AccountName = bmp.AccountName.Trim();
        bdBaseDataRecordRecord.BDBaseDataID = bmp.ID;
        bdBaseDataRecordRecord.DepartCode = bmp.DepartCode.Trim();
        bdBaseDataRecordRecord.DepartName = bmp.DepartName.Trim();
        bdBaseDataRecordRecord.EnterCode = bmp.EnterCode.Trim();
        bdBaseDataRecordRecord.MoneyNum = bmp.MoneyNum;
        bdBaseDataRecordRecord.MonthNum = bmp.MonthNum;
        bdBaseDataRecordRecord.Type = bmp.Type.Trim();
        bdBaseDataRecordRecord.YearNum = bmp.YearNum;

        if (strParaValue == "1")
            bdBaseDataRecordRecord.OperationType = "Increase";   
        else if (strParaValue == "2")
            bdBaseDataRecordRecord.OperationType = "Update";   
        else if (strParaValue == "3")
            bdBaseDataRecordRecord.OperationType = "Deleted";

        bdBaseDataRecordRecordBLL.AddBDBaseDataRecord(bdBaseDataRecordRecord);
    }

    /// <summary>
    /// 获取部门信息
    /// </summary>
    protected DataTable GetDepartList(string strParentCode)
    {
        string strHQL;
        if (strParentCode.Trim() == "0")
        {
            strHQL = "Select * From T_Department Where ParentCode not in (select DepartCode from T_Department) ";
        }
        else
        {
            strHQL = "Select * From T_Department Where ParentCode = '" + strParentCode.Trim() + "' ";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Department");
        return ds.Tables[0];
    }

    protected void SetInterval(DropDownList DDL, string strParentCode, string interval)
    {
        interval += "├";

        DataTable list = GetDepartList(strParentCode);
        if (list.Rows.Count > 0 && list != null)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                DDL.Items.Add(new ListItem(string.Format("{0}{1}", interval, list.Rows[i]["DepartName"].ToString().Trim()), list.Rows[i]["DepartCode"].ToString().Trim()));

                ///递归
                SetInterval(DDL, list.Rows[i]["DepartCode"].ToString().Trim(), interval);
            }
        }
    }

    protected void LoadAccountName()
    {
        string strHQL = "select * From T_Account Order By SortNumber ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Account");
        ddl_AccountName.DataSource = ds;
        ddl_AccountName.DataBind();
        ddl_AccountName.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadBMBaseDataList(string strDepartCode)
    {
        string strHQL;
        string strDepartString;

        strDepartString = lbl_DepartString.Text.Trim();

        strHQL = "Select * From T_BDBaseData Where (Type='Base' or Type='Budget')";
        strHQL += " and DepartCode In " + strDepartString;
        strHQL += " and DepartCode  = " + "'" + strDepartCode + "'";
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        lbl_sql3.Text = strHQL;
    }


    protected void LoadBMBaseDataRecordList()
    {
        string strHQL;
        string strDepartName, strDepartString;

        strDepartString = lbl_DepartString.Text.Trim();
        strDepartName = TB_DepartName.Text.Trim();

        if (strDepartName == "")
        {
            LB_DepartCode.Text = "";
        }

        strHQL = "Select * From T_BDBaseDataRecord Where (Type='Operation' or Type='Actual')";   
        strHQL += " and DepartCode In " + strDepartString;

        if (!string.IsNullOrEmpty(TB_DepartName.Text.Trim()))
        {
            strHQL += " and DepartName like '%" + TB_DepartName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(LB_DepartCode.Text.Trim()))
        {
            strHQL += " and DepartCode = '" + LB_DepartCode.Text.Trim() + "'";
        }
        if (!string.IsNullOrEmpty(TB_AccountName.Text.Trim()))
        {
            strHQL += " and AccountName like '%" + TB_AccountName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(TB_Year.Text.Trim()))
        {
            strHQL += " and YearNum='" + TB_Year.Text.Trim() + "' ";
        }
        if (!string.IsNullOrEmpty(TB_Month.Text.Trim()))
        {
            strHQL += " and MonthNum='" + int.Parse(TB_Month.Text.Trim()) + "' ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }


    /// <summary>
    /// 新增时，获取表T_BMBaseData中最大编号。
    /// </summary>
    /// <param name="bmbp"></param>
    /// <returns></returns>
    protected int GetMaxBMBaseDataID()
    {
        string strHQL = "Select max(ID) From T_BDBaseData ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 新增时，获取表T_BMBaseData中最大编号。
    /// </summary>
    /// <param name="bmbp"></param>
    /// <returns></returns>
    protected int GetMaxBMBaseDataRecordID()
    {
        string strHQL = "Select max(ID) From T_BDBaseDataRecord";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseDataRecord");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }
}
