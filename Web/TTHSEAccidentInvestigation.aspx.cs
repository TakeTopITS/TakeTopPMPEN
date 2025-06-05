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

public partial class TTHSEAccidentInvestigation : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","事故调查报告", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_HappenDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadHSEAccidentDescriptionName();

            LoadHSEAccidentInvestigationList();

            LoadCurrency();
        }
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
        if (IsHSEAccidentInvestigation(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_AccidentDescriptionCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_AccidentDescriptionCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSGMSWBXCZSBJC") + "')", true);
            DL_AccidentDescriptionCode.Focus();
            return;
        }
        HSEAccidentInvestigation hSEAccidentInvestigation = new HSEAccidentInvestigation();
        HSEAccidentInvestigationBLL hSEAccidentInvestigationBLL = new HSEAccidentInvestigationBLL();

        hSEAccidentInvestigation.Code = GetHSEAccidentInvestigationCode();
        LB_Code.Text = hSEAccidentInvestigation.Code.Trim();
        hSEAccidentInvestigation.HappenDate = DateTime.Parse(DLC_HappenDate.Text.Trim());
        hSEAccidentInvestigation.Name = TB_Name.Text.Trim();
        hSEAccidentInvestigation.AccidentDescriptionCode = DL_AccidentDescriptionCode.SelectedValue.Trim();
        hSEAccidentInvestigation.AccidentDescriptionName = GetHSEAccidentDescriptionName(hSEAccidentInvestigation.AccidentDescriptionCode.Trim());
        hSEAccidentInvestigation.LessonText = TB_LessonText.Text.Trim();
        hSEAccidentInvestigation.PropertyDamage = NB_PropertyDamage.Amount;
        hSEAccidentInvestigation.AccidentAddr = TB_AccidentAddr.Text.Trim();
        hSEAccidentInvestigation.InfluenceHarm = TB_InfluenceHarm.Text.Trim();
        hSEAccidentInvestigation.EngineeringSolutions = TB_EngineeringSolutions.Text.Trim();
        hSEAccidentInvestigation.CauseResponsibility = TB_CauseResponsibility.Text.Trim();
        hSEAccidentInvestigation.AccidentType = TB_AccidentType.Text.Trim();
        hSEAccidentInvestigation.DeathNum = int.Parse(NB_DeathNum.Amount.ToString());
        hSEAccidentInvestigation.DepartCode = TB_DepartCode.Text.Trim();
        hSEAccidentInvestigation.TakeMeasures = TB_TakeMeasures.Text.Trim();
        hSEAccidentInvestigation.MinorInjury = int.Parse(NB_MinorInjury.Amount.ToString());
        hSEAccidentInvestigation.Currency = DL_Currency.SelectedValue;
        hSEAccidentInvestigation.SeriousInjury = int.Parse(NB_SeriousInjury.Amount.ToString());
        hSEAccidentInvestigation.ExchangeRate = NB_ExchangeRate.Amount;
        hSEAccidentInvestigation.EnterCode = strUserCode.Trim();

        try
        {
            hSEAccidentInvestigationBLL.AddHSEAccidentInvestigation(hSEAccidentInvestigation);
            UpdateHSEAccidentDescription(hSEAccidentInvestigation.AccidentDescriptionCode.Trim());

            LoadHSEAccidentInvestigationList();

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
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



    protected void Update()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEAccidentInvestigation(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_AccidentDescriptionCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_AccidentDescriptionCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSGMSWBXCZSBJC") + "')", true);
            DL_AccidentDescriptionCode.Focus();
            return;
        }

        string strHQL = "From HSEAccidentInvestigation as hSEAccidentInvestigation Where hSEAccidentInvestigation.Code='" + LB_Code.Text.Trim() + "' ";
        HSEAccidentInvestigationBLL hSEAccidentInvestigationBLL = new HSEAccidentInvestigationBLL();
        IList lst = hSEAccidentInvestigationBLL.GetAllHSEAccidentInvestigations(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEAccidentInvestigation hSEAccidentInvestigation = (HSEAccidentInvestigation)lst[0];
            hSEAccidentInvestigation.HappenDate = DateTime.Parse(DLC_HappenDate.Text.Trim());
            hSEAccidentInvestigation.Name = TB_Name.Text.Trim();
            hSEAccidentInvestigation.AccidentDescriptionCode = DL_AccidentDescriptionCode.SelectedValue.Trim();
            hSEAccidentInvestigation.AccidentDescriptionName = GetHSEAccidentDescriptionName(hSEAccidentInvestigation.AccidentDescriptionCode.Trim());
            hSEAccidentInvestigation.LessonText = TB_LessonText.Text.Trim();
            hSEAccidentInvestigation.PropertyDamage = NB_PropertyDamage.Amount;
            hSEAccidentInvestigation.AccidentAddr = TB_AccidentAddr.Text.Trim();
            hSEAccidentInvestigation.InfluenceHarm = TB_InfluenceHarm.Text.Trim();
            hSEAccidentInvestigation.EngineeringSolutions = TB_EngineeringSolutions.Text.Trim();
            hSEAccidentInvestigation.CauseResponsibility = TB_CauseResponsibility.Text.Trim();
            hSEAccidentInvestigation.AccidentType = TB_AccidentType.Text.Trim();
            hSEAccidentInvestigation.DeathNum = int.Parse(NB_DeathNum.Amount.ToString());
            hSEAccidentInvestigation.DepartCode = TB_DepartCode.Text.Trim();
            hSEAccidentInvestigation.TakeMeasures = TB_TakeMeasures.Text.Trim();
            hSEAccidentInvestigation.MinorInjury = int.Parse(NB_MinorInjury.Amount.ToString());
            hSEAccidentInvestigation.Currency = DL_Currency.SelectedValue;
            hSEAccidentInvestigation.SeriousInjury = int.Parse(NB_SeriousInjury.Amount.ToString());
            hSEAccidentInvestigation.ExchangeRate = NB_ExchangeRate.Amount;
            try
            {
                hSEAccidentInvestigationBLL.UpdateHSEAccidentInvestigation(hSEAccidentInvestigation, hSEAccidentInvestigation.Code);

                LoadHSEAccidentInvestigationList();

                //BT_Update.Visible = true;
                //BT_Delete.Visible = true;
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
    }

    protected void Delete()
    {
        string strCode = LB_Code.Text.Trim();

        string strHQL = "Delete From T_HSEAccidentInvestigation Where Code = '" + strCode + "' ";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEAccidentInvestigationList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }


    protected void UpdateHSEAccidentDescription(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定事故描述名称T_HSEAccidentDescription
        strHQL = "From HSEAccidentDescription as hSEAccidentDescription Where hSEAccidentDescription.Code='" + strCode + "' ";
        HSEAccidentDescriptionBLL hSEAccidentDescriptionBLL = new HSEAccidentDescriptionBLL();
        lst = hSEAccidentDescriptionBLL.GetAllHSEAccidentDescriptions(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEAccidentDescription hSEAccidentDescription = (HSEAccidentDescription)lst[0];
            hSEAccidentDescription.Status = "Investigated";
            hSEAccidentDescriptionBLL.UpdateHSEAccidentDescription(hSEAccidentDescription, hSEAccidentDescription.Code);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEAccidentInvestigationList();
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


                strHQL = "From HSEAccidentInvestigation as hSEAccidentInvestigation where hSEAccidentInvestigation.Code = '" + strCode + "'";
                HSEAccidentInvestigationBLL hSEAccidentInvestigationBLL = new HSEAccidentInvestigationBLL();
                lst = hSEAccidentInvestigationBLL.GetAllHSEAccidentInvestigations(strHQL);
                HSEAccidentInvestigation hSEAccidentInvestigation = (HSEAccidentInvestigation)lst[0];

                LB_Code.Text = hSEAccidentInvestigation.Code.Trim();
                DL_AccidentDescriptionCode.SelectedValue = hSEAccidentInvestigation.AccidentDescriptionCode.Trim();
                TB_Name.Text = hSEAccidentInvestigation.Name.Trim();
                DL_Currency.SelectedValue = hSEAccidentInvestigation.Currency.Trim();
                TB_LessonText.Text = hSEAccidentInvestigation.LessonText.Trim();
                NB_PropertyDamage.Amount = hSEAccidentInvestigation.PropertyDamage;
                TB_TakeMeasures.Text = hSEAccidentInvestigation.TakeMeasures.Trim();
                TB_DepartCode.Text = hSEAccidentInvestigation.DepartCode.Trim();
                TB_AccidentType.Text = hSEAccidentInvestigation.AccidentType.Trim();
                TB_CauseResponsibility.Text = hSEAccidentInvestigation.CauseResponsibility.Trim();
                TB_EngineeringSolutions.Text = hSEAccidentInvestigation.EngineeringSolutions.Trim();
                TB_InfluenceHarm.Text = hSEAccidentInvestigation.InfluenceHarm.Trim();
                TB_AccidentAddr.Text = hSEAccidentInvestigation.AccidentAddr.Trim();
                NB_SeriousInjury.Amount = hSEAccidentInvestigation.SeriousInjury;
                NB_MinorInjury.Amount = hSEAccidentInvestigation.MinorInjury;
                NB_DeathNum.Amount = hSEAccidentInvestigation.DeathNum;
                DLC_HappenDate.Text = hSEAccidentInvestigation.HappenDate.ToString("yyyy-MM-dd");
                NB_ExchangeRate.Amount = hSEAccidentInvestigation.ExchangeRate;
                //if (hSEAccidentInvestigation.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Delete.Visible = true;
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentDescription");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DL_AccidentDescriptionCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        //绑定事故描述名称T_HSEAccidentDescription
        strHQL = "From HSEAccidentDescription as hSEAccidentDescription Where hSEAccidentDescription.Code='" + DL_AccidentDescriptionCode.SelectedValue.Trim() + "' ";
        HSEAccidentDescriptionBLL hSEAccidentDescriptionBLL = new HSEAccidentDescriptionBLL();
        lst = hSEAccidentDescriptionBLL.GetAllHSEAccidentDescriptions(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEAccidentDescription hSEAccidentDescription = (HSEAccidentDescription)lst[0];
            DLC_HappenDate.Text = hSEAccidentDescription.AccidentDate.ToString("yyyy-MM-dd");
            TB_AccidentAddr.Text = hSEAccidentDescription.AccidentAddr.Trim();
            TB_AccidentType.Text = hSEAccidentDescription.AccidentType.Trim();
            TB_DepartCode.Text = hSEAccidentDescription.DepartCode.Trim();
            NB_DeathNum.Amount = hSEAccidentDescription.DeathNum;
            NB_MinorInjury.Amount = hSEAccidentDescription.MinorInjury;
            NB_SeriousInjury.Amount = hSEAccidentDescription.SeriousInjury;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DL_Currency_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From CurrencyType as currencyType Where currencyType.Type='" + DL_Currency.SelectedValue.Trim() + "' ";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            CurrencyType currencyType = (CurrencyType)lst[0];
            NB_ExchangeRate.Amount = currencyType.ExchangeRate;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    /// <summary>
    /// 新增或更新时，检查事故调查报告名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEAccidentInvestigation(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSEAccidentInvestigation Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSEAccidentInvestigation Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentInvestigation").Tables[0];
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

    protected string GetHSEAccidentDescriptionName(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定事故描述名称T_HSEAccidentDescription
        strHQL = "From HSEAccidentDescription as hSEAccidentDescription Where hSEAccidentDescription.Code='" + strCode + "' ";
        HSEAccidentDescriptionBLL hSEAccidentDescriptionBLL = new HSEAccidentDescriptionBLL();
        lst = hSEAccidentDescriptionBLL.GetAllHSEAccidentDescriptions(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEAccidentDescription hSEAccidentDescription = (HSEAccidentDescription)lst[0];
            return hSEAccidentDescription.Name.Trim();
        }
        else
            return "";
    }

    /// <summary>
    /// 新增时，获取表T_HSEAccidentInvestigation中最大编号 规则HSEADIX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSEAccidentInvestigationCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEAccidentInvestigation Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentInvestigation").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEADI" + pa.ToString();
        }
        else
        {
            flag = "HSEADI1";
        }
        return flag;
    }


    protected void LoadHSEAccidentDescriptionName()
    {
        string strHQL;
        IList lst;
        //绑定事故描述名称
        strHQL = "From HSEAccidentDescription as hSEAccidentDescription Order By hSEAccidentDescription.Code Desc";
        HSEAccidentDescriptionBLL hSEAccidentDescriptionBLL = new HSEAccidentDescriptionBLL();
        lst = hSEAccidentDescriptionBLL.GetAllHSEAccidentDescriptions(strHQL);
        DL_AccidentDescriptionCode.DataSource = lst;
        DL_AccidentDescriptionCode.DataBind();
        DL_AccidentDescriptionCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadCurrency()
    {
        string strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC ";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        DL_Currency.DataSource = lst;
        DL_Currency.DataBind();
        DL_Currency.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadHSEAccidentInvestigationList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEAccidentInvestigation Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or InfluenceHarm like '%" + TextBox1.Text.Trim() + "%' " +
                "or CauseResponsibility like '%" + TextBox1.Text.Trim() + "%' or EngineeringSolutions like '%" + TextBox1.Text.Trim() + "%' or TakeMeasures like '%" + TextBox1.Text.Trim() + "%' " +
                "or LessonText like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (AccidentDescriptionCode like '%" + TextBox2.Text.Trim() + "%' or AccidentDescriptionName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-HappenDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-HappenDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEAccidentInvestigation");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

}