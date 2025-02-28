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

public partial class TTBDBaseData : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","预算标准设置", strUserCode);
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

            lbl_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);

            //LoadDepartInformation();
            ShareClass.LoadAccountForDDL(ddl_AccountName);
            //LoadAccountName();

            LoadBMBaseDataList();
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

            TB_DepartName .Text = ShareClass.GetDepartName(lbl_DCode.Text);
            LB_DepartCode.Text = treeNode.Target.Trim();

            LoadBMBaseDataList();
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
            AddBudget();
        }
        else
        {
            UpdateBudget();
        }
    }

    protected void AddBudget()
    {
        if (string.IsNullOrEmpty(lbl_DCode.Text) || lbl_DCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGBMBJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        if (string.IsNullOrEmpty(DLC_YearNum.Text.Trim()) || DLC_YearNum.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGYSNFBTJC").ToString().Trim()+"')", true);
            DLC_YearNum.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        if (IsBMBaseDataExits(string.Empty, lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(DLC_MonthNum.Text.Trim())))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGNFBMYSYCZJC").ToString().Trim()+"')", true);
            ddl_AccountName.Focus();
            DLC_YearNum.Focus();
            DLC_MonthNum.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        BDBaseDataBLL bMBaseDataBLL = new BDBaseDataBLL();
        BDBaseData bMBaseData = new BDBaseData();

        bMBaseData.AccountCode = ddl_AccountName.SelectedValue.Trim();
        bMBaseData.AccountName = ddl_AccountName.SelectedItem.Text.Trim();
        bMBaseData.DepartCode = lbl_DCode.Text.Trim();
        bMBaseData.DepartName = lbl_DName.Text.Trim();
        bMBaseData.MoneyNum = NB_MoneyNum.Amount;
        bMBaseData.MonthNum = int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim());
        bMBaseData.YearNum = int.Parse(DLC_YearNum.Text.Trim());
        bMBaseData.EnterCode = strUserCode.Trim();
        bMBaseData.Type = "Base";
        bMBaseData.ProjectCostID = 0;

        try
        {
            bMBaseDataBLL.AddBDBaseData(bMBaseData);
            LB_ID.Text = GetMaxBMBaseDataID(bMBaseData).ToString();

            //AddBMBaseDataRecordData(LB_ID.Text.Trim(), "1");

            LoadBMBaseDataList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    /// <summary>
    /// 新增时，获取表T_BMBaseData中最大编号。
    /// </summary>
    /// <param name="bmbp"></param>
    /// <returns></returns>
    protected int GetMaxBMBaseDataID(BDBaseData bmbp)
    {
        string strHQL = "Select ID From T_BDBaseData where EnterCode='" + bmbp.EnterCode.Trim() + "' and DepartCode='" + bmbp.DepartCode.Trim() + "' and Type='Base' Order by ID Desc";
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

    protected void UpdateBudget()
    {
        if (string.IsNullOrEmpty(lbl_DCode.Text) || lbl_DCode.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGBMBJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        if (string.IsNullOrEmpty(DLC_YearNum.Text.Trim()) || DLC_YearNum.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGYSNFBTJC").ToString().Trim()+"')", true);
            DLC_YearNum.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }
        if (IsBMBaseDataExits(LB_ID.Text.Trim(), lbl_DCode.Text.Trim(), ddl_AccountName.SelectedValue.Trim(), int.Parse(DLC_YearNum.Text.Trim()), int.Parse(DLC_MonthNum.Text.Trim())))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGNFBMYSYCZJC").ToString().Trim()+"')", true);
            ddl_AccountName.Focus();
            DLC_YearNum.Focus();
            DLC_MonthNum.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.ID = '" + LB_ID.Text.Trim() + "'";
        BDBaseDataBLL bMBaseDataBLL = new BDBaseDataBLL();
        IList lst = bMBaseDataBLL.GetAllBDBaseDatas(strHQL);
        BDBaseData bMBaseData = (BDBaseData)lst[0];

        bMBaseData.AccountCode = ddl_AccountName.SelectedValue.Trim();
        bMBaseData.AccountName = ddl_AccountName.SelectedItem.Text.Trim();
        bMBaseData.DepartCode = lbl_DCode.Text.Trim();
        bMBaseData.DepartName = lbl_DName.Text.Trim();
        bMBaseData.MoneyNum = NB_MoneyNum.Amount;
        bMBaseData.MonthNum = int.Parse(string.IsNullOrEmpty(DLC_MonthNum.Text) || DLC_MonthNum.Text.Trim() == "" ? "0" : DLC_MonthNum.Text.Trim());
        bMBaseData.YearNum = int.Parse(DLC_YearNum.Text.Trim());

        try
        {
            bMBaseDataBLL.UpdateBDBaseData(bMBaseData, bMBaseData.ID);

            LoadBMBaseDataList();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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

                strHQL = "From BDBaseData as bDBaseData where bDBaseData.ID = '" + strId + "'";
                BDBaseDataBLL bMBaseDataBLL = new BDBaseDataBLL();
                lst = bMBaseDataBLL.GetAllBDBaseDatas(strHQL);
                BDBaseData bMBaseData = (BDBaseData)lst[0];

                LB_ID.Text = bMBaseData.ID.ToString().Trim();
                lbl_DCode.Text = bMBaseData.DepartCode.Trim();
                DLC_YearNum.Text = bMBaseData.YearNum.ToString();
                DLC_MonthNum.Text = bMBaseData.MonthNum >= 10 ? bMBaseData.MonthNum.ToString() : bMBaseData.MonthNum == 0 ? "" : "0" + bMBaseData.MonthNum.ToString();
                NB_MoneyNum.Amount = bMBaseData.MoneyNum;
                ddl_AccountName.SelectedValue = bMBaseData.AccountCode.Trim();
                lbl_DName.Text = bMBaseData.DepartName.Trim();

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                AddBMBaseDataRecordData(strId, "3");

                strHQL = "Delete From T_BDBaseData Where ID = " + strId;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadBMBaseDataList();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
                }
            }
        }
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
    protected bool IsBMBaseDataExits(string strID, string strDepartCode, string strAccountCode, int strYearNum, int strMonthNum)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountCode='" + strAccountCode + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.Type='Base' ";
        }
        else
        {
            strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountCode='" + strAccountCode + "' and " +
                "bDBaseData.YearNum='" + strYearNum.ToString() + "' and bDBaseData.MonthNum = '" + strMonthNum.ToString() + "' and bDBaseData.Type='Base' and bDBaseData.ID<>'" + strID + "' ";
        }
        BDBaseDataBLL bMBaseDataBLL = new BDBaseDataBLL();
        IList lst = bMBaseDataBLL.GetAllBDBaseDatas(strHQL);
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadBMBaseDataList();
    }

    protected void AddBMBaseDataRecordData(string strID, string strParaValue)
    {
        BDBaseDataRecordBLL bMBaseDataRecordBLL = new BDBaseDataRecordBLL();
        BDBaseDataRecord bMBaseDataRecord = new BDBaseDataRecord();

        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.ID = '" + strID + "'";
        BDBaseDataBLL bMBaseDataBLL = new BDBaseDataBLL();
        IList lst = bMBaseDataBLL.GetAllBDBaseDatas(strHQL);
        BDBaseData bmp = (BDBaseData)lst[0];

        bMBaseDataRecord.AccountName = bmp.AccountName.Trim();
        bMBaseDataRecord.BDBaseDataID = bmp.ID;
        bMBaseDataRecord.DepartCode = bmp.DepartCode.Trim();
        bMBaseDataRecord.DepartName = bmp.DepartName.Trim();
        bMBaseDataRecord.EnterCode = bmp.EnterCode.Trim();
        bMBaseDataRecord.MoneyNum = bmp.MoneyNum;
        bMBaseDataRecord.MonthNum = bmp.MonthNum;
        bMBaseDataRecord.Type = bmp.Type.Trim();
        bMBaseDataRecord.YearNum = bmp.YearNum;

        if (strParaValue == "1")
            bMBaseDataRecord.OperationType = "Increase";   //ChineseWord
        else if (strParaValue == "2")
            bMBaseDataRecord.OperationType = "Update";   //ChineseWord
        else if (strParaValue == "3")
            bMBaseDataRecord.OperationType = "Deleted";

        bMBaseDataRecordBLL.AddBDBaseDataRecord(bMBaseDataRecord);
    }


    /// <summary>
    /// 绑定部门信息
    /// </summary>
    protected void LoadDepartInformation()
    {
        DataTable dt = GetDepartList("0");
        if (dt != null && dt.Rows.Count > 0)
        {
            DL_DepartCode.Items.Clear();
            DL_DepartCode.Items.Insert(0, new ListItem("--Select--", ""));
            SetInterval(DL_DepartCode, "0", " ");
        }
        else
        {
            DL_DepartCode.Items.Clear();
            DL_DepartCode.Items.Insert(0, new ListItem("--Select--", ""));
        }
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

    protected void LoadBMBaseDataList()
    {
        string strHQL;
        string strDepartName, strDepartString;


        strDepartString = lbl_DepartString.Text.Trim();
        strDepartName = TB_DepartName.Text.Trim();

        if (strDepartName == "")
        {
            LB_DepartCode.Text = "";
        }


        strHQL = "Select * From T_BDBaseData Where (Type='Base' or Type='Budget')";
        strHQL += " and (EnterCode = '" + strUserCode + "'";
        strHQL += " Or DepartCode In " + strDepartString + ")";

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

        //DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();

        lbl_sql.Text = strHQL;
    }


}
