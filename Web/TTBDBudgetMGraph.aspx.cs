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

public partial class TTBDBudgetMGraph : System.Web.UI.Page
{
    private List<Color> defaultColors = new List<Color>();
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "预算费用分析", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }      

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            lbl_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView2, strUserCode);

            DropDownList2.Enabled = false;
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            //LoadDepartInformation();
            TextBox1.Enabled = false;

            OnRenderGraph();
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;
        if (treeNode.Target != "0")
        {
            lbl_DCode.Text = treeNode.Target.Trim();
            lbl_DName.Text = ShareClass.GetDepartName(lbl_DCode.Text);
            TextBox1.Text = ShareClass.GetDepartName(lbl_DCode.Text);
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
            TextBox1.Text = ShareClass.GetDepartName(lbl_DCode.Text);
            DropDownList1.SelectedValue = "部门+科目"; 
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedValue.Trim() == "部门") 
        {
            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("--Select--", ""));
            DropDownList2.Enabled = false;
            TextBox1.Enabled = false;
        }
        else if (DropDownList1.SelectedValue.Trim() == "科目") 
        {
            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("--Select--", ""));
            DropDownList2.Enabled = false;
            TextBox1.Enabled = false;
        }
        else
        {
            //LoadDepartInformation();
            DropDownList2.Enabled = true;
            TextBox1.Enabled = true;
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txt_Month.Text) && txt_Month.Text.Trim() != "")
        {
            if (string.IsNullOrEmpty(txt_Year.Text) || txt_Year.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSNFBJC").ToString().Trim() + "')", true);
                txt_Year.Focus();
                return;
            }
        }
        if (DropDownList1.SelectedValue.Trim() == "部门+科目") 
        {
            if (string.IsNullOrEmpty(lbl_DCode.Text) || lbl_DCode.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTSBMBJC").ToString().Trim() + "')", true);
                return;
            }
        }
        

        OnRenderGraph();
    }


    private void OnRenderGraph()
    {
        string strResult = string.Empty;

        if (DropDownList1.SelectedValue.Trim() == "部门") 
        {

            strResult = LanguageHandle.GetWord("ABMTJFYFB").ToString().Trim();
        }
        else if (DropDownList1.SelectedValue.Trim() == "科目") 
        {

            strResult = LanguageHandle.GetWord("AKMTJFYFB").ToString().Trim();
        }
        else
        {

            strResult = lbl_DName.Text.Trim() + LanguageHandle.GetWord("DeFeiYongFenBu").ToString().Trim();
        }


        string stryear = txt_Year.Text.Trim();
        string strmonth = txt_Month.Text.Trim();
        string strdepartcode = lbl_DCode.Text.Trim();

        string strHQL;
        if (DropDownList1.SelectedValue.Trim() == "部门") 
        {
            strHQL = "Select DepartName as XName,SUM(MoneyNum) as YNumber From T_BDBaseDataRecord Where (Type='Operation' or Type='实际')"; 
            if (!string.IsNullOrEmpty(stryear) && stryear != "")
            {
                strHQL += " and YearNum='" + stryear + "' ";
            }
            if (!string.IsNullOrEmpty(strmonth) && strmonth != "")
            {
                strHQL += " and MonthNum='" + int.Parse(strmonth) + "' ";
            }
            strHQL += " Group By DepartName ";
        }
        else if (DropDownList1.SelectedValue.Trim() == "科目") 
        {
            strHQL = "Select B.AccountName as XName,SUM(A.MoneyNum) as YNumber From T_BDBaseDataRecord A,T_Account B Where A.AccountCode=B.AccountCode and (A.Type='Operation' or A.Type='实际')"; 
            if (!string.IsNullOrEmpty(stryear) && stryear != "")
            {
                strHQL += " and A.YearNum='" + stryear + "' ";
            }
            if (!string.IsNullOrEmpty(strmonth) && strmonth != "")
            {
                strHQL += " and A.MonthNum='" + int.Parse(strmonth) + "' ";
            }
            strHQL += " Group By B.AccountName ";
        }
        else
        {
            strHQL = "Select B.AccountName as XName,SUM(A.MoneyNum) as YNumber From T_BDBaseDataRecord A,T_Account B Where A.AccountCode=B.AccountCode and (A.Type='Operation' or A.Type='实际')"; 
            if (!string.IsNullOrEmpty(stryear) && stryear != "")
            {
                strHQL += " and A.YearNum='" + stryear + "' ";
            }
            if (!string.IsNullOrEmpty(strmonth) && strmonth != "")
            {
                strHQL += " and A.MonthNum='" + int.Parse(strmonth) + "' ";
            }
            if (!string.IsNullOrEmpty(strdepartcode) && strdepartcode != "")
            {
                strHQL += " and A.DepartCode='" + strdepartcode + "' ";
            }
            strHQL += " Group By B.AccountName ";
        }

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BDBaseData");

        string strName, strNameProfile = "";
        decimal intMoneyCount, intSum = 0;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strName = ds.Tables[0].Rows[i]["XName"].ToString();
            intMoneyCount = decimal.Parse(ds.Tables[0].Rows[i]["YNumber"].ToString());

            intSum += intMoneyCount;

            strNameProfile += strName + ":" + intMoneyCount.ToString() + LanguageHandle.GetWord("Yuan").ToString().Trim();
        }

        LB_ProjectStatus.Text = strResult + ": " + strNameProfile.Trim();

        string strChartTitle = strResult;
        IFrame_Chart_Budget.Src = "TTTakeTopAnalystChartSet.aspx?FormType=Single&ChartType=Pie&ChartName=" + strChartTitle + "&SqlCode=" + ShareClass.Escape(strHQL);

    }

    protected void LoadDepartInformation()
    {
        DataTable dt = GetDepartList("0");
        if (dt != null && dt.Rows.Count > 0)
        {
            DropDownList2.Items.Clear();
            SetInterval(DropDownList2, "0", " ");
        }
        else
        {
            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("--Select--", ""));
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

}
