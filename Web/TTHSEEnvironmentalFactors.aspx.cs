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

public partial class TTHSEEnvironmentalFactors : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","环境因素记录", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadHSEEnvironmentalFactorsList();
        }
    }

    protected void LoadHSEEnvironmentalFactorsList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEEnvironmentalFactors Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or Process like '%" + TextBox1.Text.Trim() + "%' or Activity like '%" + TextBox1.Text.Trim() + "%' " +
            "Or EnvirImpact like '%" + TextBox1.Text.Trim() + "%' Or CopeStrategy like '%" + TextBox1.Text.Trim() + "%' or LawRegulationReq like '%" + TextBox1.Text.Trim() + "%' or TermFeatureReq like '%" + TextBox1.Text.Trim() + "%') ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvironmentalFactors");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_Code.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strCode;

        strCode = LB_Code.Text;

        if (strCode == "")
        {
            Add();
        }
        else
        {
            Update();
        }
    }

    protected void Add()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEEnvironmentalFactors(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }

        HSEEnvironmentalFactorsBLL hSEEnvironmentalFactorsBLL = new HSEEnvironmentalFactorsBLL();
        HSEEnvironmentalFactors hSEEnvironmentalFactors = new HSEEnvironmentalFactors();

        hSEEnvironmentalFactors.Code = GetHSEEnvironmentalFactorsCode();
        LB_Code.Text = hSEEnvironmentalFactors.Code.Trim();
        hSEEnvironmentalFactors.Process = TB_Process.Text.Trim();
        hSEEnvironmentalFactors.TermFeatureReq = TB_TermFeatureReq.Text.Trim();
        hSEEnvironmentalFactors.FactorType = TB_FactorType.Text.Trim();
        hSEEnvironmentalFactors.LawRegulationReq = TB_LawRegulationReq.Text.Trim();
        hSEEnvironmentalFactors.EnvirImpact = TB_EnvirImpact.Text.Trim();
        hSEEnvironmentalFactors.CopeStrategy = TB_CopeStrategy.Text.Trim();
        hSEEnvironmentalFactors.Name = TB_Name.Text.Trim();
        hSEEnvironmentalFactors.Tenses = DL_Tenses.SelectedValue.Trim();
        hSEEnvironmentalFactors.Activity = TB_Activity.Text.Trim();
        hSEEnvironmentalFactors.Status = DL_Status.SelectedValue.Trim();
        hSEEnvironmentalFactors.EnterCode = strUserCode.Trim();
        try
        {
            hSEEnvironmentalFactorsBLL.AddHSEEnvironmentalFactors(hSEEnvironmentalFactors);

            LoadHSEEnvironmentalFactorsList();

            //BT_Delete.Visible = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 新增或更新时，检查环境目标记录名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEEnvironmentalFactors(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSEEnvironmentalFactors Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSEEnvironmentalFactors Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvironmentalFactors").Tables[0];
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
    /// 新增时，获取表T_HSEEnvironmentalFactors中最大编号 规则HSEENFX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSEEnvironmentalFactorsCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEEnvironmentalFactors Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvironmentalFactors").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEENF" + pa.ToString();
        }
        else
        {
            flag = "HSEENF1";
        }
        return flag;
    }

    protected void Update()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEEnvironmentalFactors(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }

        string strHQL = "From HSEEnvironmentalFactors as hSEEnvironmentalFactors where hSEEnvironmentalFactors.Code = '" + LB_Code.Text.Trim() + "'";
        HSEEnvironmentalFactorsBLL hSEEnvironmentalFactorsBLL = new HSEEnvironmentalFactorsBLL();
        IList lst = hSEEnvironmentalFactorsBLL.GetAllHSEEnvironmentalFactorss(strHQL);

        HSEEnvironmentalFactors hSEEnvironmentalFactors = (HSEEnvironmentalFactors)lst[0];

        hSEEnvironmentalFactors.Process = TB_Process.Text.Trim();
        hSEEnvironmentalFactors.TermFeatureReq = TB_TermFeatureReq.Text.Trim();
        hSEEnvironmentalFactors.FactorType = TB_FactorType.Text.Trim();
        hSEEnvironmentalFactors.LawRegulationReq = TB_LawRegulationReq.Text.Trim();
        hSEEnvironmentalFactors.EnvirImpact = TB_EnvirImpact.Text.Trim();
        hSEEnvironmentalFactors.CopeStrategy = TB_CopeStrategy.Text.Trim();
        hSEEnvironmentalFactors.Name = TB_Name.Text.Trim();
        hSEEnvironmentalFactors.Tenses = DL_Tenses.SelectedValue.Trim();
        hSEEnvironmentalFactors.Activity = TB_Activity.Text.Trim();
        hSEEnvironmentalFactors.Status = DL_Status.SelectedValue.Trim();

        try
        {
            hSEEnvironmentalFactorsBLL.UpdateHSEEnvironmentalFactors(hSEEnvironmentalFactors, hSEEnvironmentalFactors.Code);

            LoadHSEEnvironmentalFactorsList();

            //BT_Delete.Visible = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = LB_Code.Text.Trim();
        if (IsHSEEnvirFactorSurDetailExits(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGHJYSYBYYWFSC") + "')", true);
            return;
        }

        strHQL = "Delete From T_HSEEnvironmentalFactors Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEEnvironmentalFactorsList();

            //BT_Delete.Visible = false;
            //BT_Update.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected bool IsHSEEnvirFactorSurDetailExits(string strCode)
    {
        string strHQL = "From HSEEnvirFactorSurDetail as hSEEnvirFactorSurDetail where hSEEnvirFactorSurDetail.FactorCode = '" + strCode + "'";
        HSEEnvirFactorSurDetailBLL hSEEnvirFactorSurDetailBLL = new HSEEnvirFactorSurDetailBLL();
        IList lst = hSEEnvirFactorSurDetailBLL.GetAllHSEEnvirFactorSurDetails(strHQL);
        if (lst.Count > 0 && lst != null)
            return true;
        else
            return false;
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strCode, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strCode = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "From HSEEnvironmentalFactors as hSEEnvironmentalFactors where hSEEnvironmentalFactors.Code = '" + strCode + "'";
                HSEEnvironmentalFactorsBLL hSEEnvironmentalFactorsBLL = new HSEEnvironmentalFactorsBLL();
                lst = hSEEnvironmentalFactorsBLL.GetAllHSEEnvironmentalFactorss(strHQL);

                HSEEnvironmentalFactors hSEEnvironmentalFactors = (HSEEnvironmentalFactors)lst[0];

                LB_Code.Text = hSEEnvironmentalFactors.Code.Trim();
                TB_Process.Text = hSEEnvironmentalFactors.Process.Trim();
                DL_Tenses.SelectedValue = hSEEnvironmentalFactors.Tenses.Trim();
                TB_TermFeatureReq.Text = hSEEnvironmentalFactors.TermFeatureReq.Trim();
                TB_LawRegulationReq.Text = hSEEnvironmentalFactors.LawRegulationReq.Trim();
                TB_EnvirImpact.Text = hSEEnvironmentalFactors.EnvirImpact.Trim();
                TB_CopeStrategy.Text = hSEEnvironmentalFactors.CopeStrategy.Trim();
                TB_Name.Text = hSEEnvironmentalFactors.Name.Trim();
                TB_Activity.Text = hSEEnvironmentalFactors.Activity.Trim();
                DL_Status.SelectedValue = hSEEnvironmentalFactors.Status.Trim();
                TB_FactorType.Text = hSEEnvironmentalFactors.FactorType.Trim();
                //if (hSEEnvironmentalFactors.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Delete.Visible = true;
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Update.Visible = false;
                //    BT_Delete.Visible = false;
                //}
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                Delete();

            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEnvironmentalFactors");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEEnvironmentalFactorsList();
    }
}