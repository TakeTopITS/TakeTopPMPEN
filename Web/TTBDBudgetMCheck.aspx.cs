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

public partial class TTBDBudgetMCheck : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "预算费用查询", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            lbl_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);

            if (DropDownList1.SelectedValue.Trim() == "预算标准") 
            {
                Budget_Base.Visible = true;
                Budget_Operation.Visible = false;
                Button1.Visible = true;
            }
            else if (DropDownList1.SelectedValue.Trim() == "预算记录") 
            {
                Budget_Base.Visible = false;
                Budget_Operation.Visible = true;
                Button1.Visible = true;
            }
            else
            {
                Budget_Base.Visible = true;
                Budget_Operation.Visible = true;
                Button1.Visible = false;
            }
            LoadBMBaseDataList();
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;
        if (treeNode.Target != "0")
        {
            DropDownList1.SelectedValue = "";
            txt_AccountName.Text = "";
            txt_Year.Text = "";
            txt_Month.Text = "";

            lbl_DepartCode.Text = treeNode.Target.Trim();
            txt_DepartName.Text = ShareClass.GetDepartName(lbl_DepartCode.Text);

            if (DropDownList1.SelectedValue.Trim() == "预算标准") 
            {
                Budget_Base.Visible = true;
                Budget_Operation.Visible = false;
            }
            else if (DropDownList1.SelectedValue.Trim() == "预算记录") 
            {
                Budget_Base.Visible = false;
                Budget_Operation.Visible = true;
            }
            else
            {
                Budget_Base.Visible = true;
                Budget_Operation.Visible = true;
            }

            LoadBMBaseDataList();
            LoadBMBaseDataRecordList();

          

            GetBMBaseDataMoneyNumByDepartment(lbl_DepartCode.Text,"Base");
        }
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_BudgetID")).Text.Trim();

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From BDBaseData as bdBaseData where bdBaseData.ID = '" + strId + "'";
            BDBaseDataBLL bdBaseDataBLL = new BDBaseDataBLL();
            lst = bdBaseDataBLL.GetAllBDBaseDatas(strHQL);
            BDBaseData bdBaseData = (BDBaseData)lst[0];
       
            txt_DepartName.Text = bdBaseData.DepartName.Trim();
            txt_AccountName.Text = bdBaseData.AccountName.Trim();
            txt_Year.Text = bdBaseData.YearNum.ToString();
            txt_Month.Text = bdBaseData.MonthNum.ToString();
            lbl_DepartCode.Text = bdBaseData.DepartCode.Trim();
            lbl_BudgetID.Text = bdBaseData.ID.ToString();

            LoadBMBaseDataRecordList();

            //取得剩余预算
            GetBMBaseDataMoneyNumByAccount(bdBaseData.DepartCode.Trim(), bdBaseData.AccountName.Trim(), bdBaseData.YearNum, bdBaseData.MonthNum, bdBaseData.Type.Trim());
        }
    }

    protected void LoadBMBaseDataList()
    {
        string strHQL;
        string strDepartName, strDepartString;

        strDepartString = lbl_DepartString.Text.Trim();
        strDepartName = txt_DepartName.Text.Trim();

        if (strDepartName == "")
        {
            lbl_DepartCode.Text = "";
        }

        strHQL = "Select * From T_BDBaseData Where (Type='Base' or Type='Budget')";
        strHQL += " and DepartCode In " + strDepartString;

     
        if (!string.IsNullOrEmpty(txt_DepartName.Text.Trim()))
        {
            strHQL += " and DepartName like '%" + txt_DepartName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(lbl_DepartCode.Text.Trim()))
        {
            strHQL += " and DepartCode = '" + lbl_DepartCode.Text.Trim() + "'";
        }
        if (!string.IsNullOrEmpty(txt_AccountName.Text.Trim()))
        {
            strHQL += " and AccountName like '%" + txt_AccountName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(txt_Year.Text.Trim()))
        {
            strHQL += " and YearNum='" + txt_Year.Text.Trim() + "' ";
        }
        if (!string.IsNullOrEmpty(txt_Month.Text.Trim()))
        {
            strHQL += " and MonthNum='" + int.Parse(txt_Month.Text.Trim()) + "' ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void LoadBMBaseDataRecordList()
    {
        string strHQL;
        string strDepartName, strDepartString;

        strDepartString = lbl_DepartString.Text.Trim();
        strDepartName = txt_DepartName.Text.Trim();

        if (strDepartName == "")
        {
            lbl_DepartCode.Text = "";
        }

        strHQL = "Select A.*,B.UserName From T_BDBaseDataRecord A,T_ProjectMember B Where A.EnterCode=B.UserCode and (A.Type='Operation' or A.Type='实际')"; 
        strHQL += " and B.DepartCode In " + strDepartString;

       
        if (!string.IsNullOrEmpty(txt_DepartName.Text.Trim()))
        {
            strHQL += " and A.DepartName like '%" + txt_DepartName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(lbl_DepartCode.Text.Trim()))
        {
            strHQL += " and A.DepartCode = '" + lbl_DepartCode.Text.Trim() + "'";
        }
        if (!string.IsNullOrEmpty(txt_AccountName.Text.Trim()))
        {
            strHQL += " and A.AccountName like '%" + txt_AccountName .Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(txt_Year.Text.Trim()))
        {
            strHQL += " and A.YearNum='" + txt_Year.Text.Trim() + "' ";
        }
        if (!string.IsNullOrEmpty(txt_Month.Text.Trim()))
        {
            strHQL += " and A.MonthNum='" + int.Parse(txt_Month.Text.Trim()) + "' ";
        }
        strHQL += " Order By A.ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql1.Text = strHQL;
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedValue.Trim() == "预算标准") 
        {
            Budget_Base.Visible = true;
            Budget_Operation.Visible = false;
        }
        else if (DropDownList1.SelectedValue.Trim() == "预算记录") 
        {
            Budget_Base.Visible = false;
            Budget_Operation.Visible = true;
        }
        else
        {
            Budget_Base.Visible = true;
            Budget_Operation.Visible = true;
        }
        LoadBMBaseDataList();
        LoadBMBaseDataRecordList();
    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql1.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DropDownList1.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCSJYSLXBJC").ToString().Trim() + "')", true);
            return;
        }
        if (Page.IsValid)
        {
            try
            {
                Random a = new Random();
                string fileName;
                if (DropDownList1.SelectedValue.Trim() == "预算标准") 
                {
                    fileName = LanguageHandle.GetWord("YuSuanBiaoZhun").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                    CreateExcel(getExportBookList(), fileName);
                }
                else
                {
                    fileName = LanguageHandle.GetWord("YuSuanJiLu").ToString().Trim() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + a.Next(100, 999) + ".xls";
                    CreateExcel(getExportBookList1(), fileName);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDCDSJYWJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedValue.Trim() == "预算标准") 
        {
            Button1.Visible = true;
        }
        else if (DropDownList1.SelectedValue.Trim() == "预算记录") 
        {
            Button1.Visible = true;
        }
        else
        {
            Button1.Visible = false;
        }
    }


    protected void GetBMBaseDataMoneyNumByDepartment(string strDepartCode, string strType)
    {
        decimal deMoneyBase = 0;
        decimal deMoneyUsed = 0;
        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.Type = '" + strType + "'";
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
        strHQL = "From BDBaseDataRecord as bdBaseDataRecord where bdBaseDataRecord.DepartCode = '" + strDepartCode + "' and bdBaseDataRecord.Type = 'Operation'";
        lst = bdBaseDataRecordBLL.GetAllBDBaseDataRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int j = 0; j < lst.Count; j++)
            {
                BDBaseDataRecord bdBaseDataRecord = (BDBaseDataRecord)lst[j];
                deMoneyUsed += bdBaseDataRecord.MoneyNum;
            }
        }

        lbl_MoneyNum.Text = (deMoneyBase - deMoneyUsed).ToString();
    }


    protected void GetBMBaseDataMoneyNumByAccount(string strDepartCode, string strAccountName, int strYearNum, int strMonthNum, string strType)
    {
        decimal deMoneyBase = 0;
        decimal deMoneyUsed = 0;
        string strHQL = "From BDBaseData as bDBaseData where bDBaseData.DepartCode = '" + strDepartCode + "' and bDBaseData.AccountName='" + strAccountName + "' and " +
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
        strHQL = "From BDBaseDataRecord as bdBaseDataRecord where bdBaseDataRecord.DepartCode = '" + strDepartCode + "' and bdBaseDataRecord.AccountName='" + strAccountName + "' and " +
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

        lbl_MoneyNum.Text = (deMoneyBase - deMoneyUsed).ToString();
    }


    protected DataTable getExportBookList()
    {
        string strHQL = "Select A.ID '编号',A.DepartName '部门名称',B.AccountName '会计科目',A.YearNum '年份',A.MonthNum '月份',A.MoneyNum '金额' From T_BDBaseData A,T_Account B Where A.AccountName=B.AccountCode and (A.Type='Base' or A.Type='Budget')"; 


        if (!string.IsNullOrEmpty(txt_DepartName.Text.Trim()))
        {
            strHQL += " and DepartName like '%" + txt_DepartName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(txt_AccountName.Text.Trim()))
        {
            strHQL += " and AccountName like '%" + txt_AccountName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(txt_Year.Text.Trim()))
        {
            strHQL += " and A.YearNum='" + txt_Year.Text.Trim() + "' ";
        }
        if (!string.IsNullOrEmpty(txt_Month.Text.Trim()))
        {
            strHQL += " and A.MonthNum='" + int.Parse(txt_Month.Text.Trim()) + "' ";
        }
        strHQL += " Order By A.ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        return ds.Tables[0];
    }

    protected DataTable getExportBookList1()
    {
        string strHQL = "Select A.ID '编号',A.DepartName '部门名称',C.AccountName '会计科目',A.YearNum '年份',A.MonthNum '月份',A.MoneyNum '金额',B.UserName '申请人' From T_BDBaseData A,T_ProjectMember B,T_Account C Where A.EnterCode=B.UserCode and A.AccountName=C.AccountCode and (A.Type='Operation' or A.Type='实际')"; 


        if (!string.IsNullOrEmpty(txt_DepartName.Text.Trim()))
        {
            strHQL += " and A.DepartName like '%" + txt_DepartName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(txt_AccountName.Text.Trim()))
        {
            strHQL += " and A.AccountName like '%" + txt_AccountName.Text.Trim() + "%'";
        }
        if (!string.IsNullOrEmpty(txt_Year.Text.Trim()))
        {
            strHQL += " and A.YearNum='" + txt_Year.Text.Trim() + "' ";
        }
        if (!string.IsNullOrEmpty(txt_Month.Text.Trim()))
        {
            strHQL += " and A.MonthNum='" + int.Parse(txt_Month.Text.Trim()) + "' ";
        }
        strHQL += " Order By A.ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        return ds.Tables[0];
    }

    private void CreateExcel(DataTable dt, string fileName)
    {
        DataGrid dg = new DataGrid();
        dg.DataSource = dt;
        dg.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dg.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
    }

}
