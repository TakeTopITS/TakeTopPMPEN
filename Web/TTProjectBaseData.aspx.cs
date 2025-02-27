using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTProjectBaseData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","项目基础设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "基础数据设定---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            LoadProjectDataLink();//数据库接口
            LoadCurrencyType();
        }
    }

    protected void BT_NewProjectDataLink_Click(object sender, EventArgs e)
    {
        string strCode = TB_Code.Text.Trim();
        string strHost = TB_Host.Text.Trim();
        string strDataBaseName = TB_DataBaseName.Text.Trim();
        string strLoginNo = TB_LoginNo.Text.Trim();
        string strPassWord = TB_PassWord.Text.Trim();

        if (IsProjectDataLink(strCode, string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGJKBMZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            return;
        }

        ProjectDataLinkBLL projectDataLinkBLL = new ProjectDataLinkBLL();
        ProjectDataLink projectDataLink = new ProjectDataLink();

        try
        {
            projectDataLink.Code = strCode;
            projectDataLink.Host = strHost;
            projectDataLink.DataBaseName = strDataBaseName;
            projectDataLink.LoginNo = strLoginNo;
            projectDataLink.PassWord = strPassWord;

            projectDataLinkBLL.AddProjectDataLink(projectDataLink);

            LoadProjectDataLink();
        }
        catch
        {
        }
    }

    protected void BT_UpdateProjectDataLink_Click(object sender, EventArgs e)
    {
        string strCode = TB_Code.Text.Trim();
        string strHost = TB_Host.Text.Trim();
        string strDataBaseName = TB_DataBaseName.Text.Trim();
        string strLoginNo = TB_LoginNo.Text.Trim();
        string strPassWord = TB_PassWord.Text.Trim();
        string CodeGold = lbl_Code.Text.Trim();

        if (IsProjectDataLink(strCode, CodeGold))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGJKBMZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_Code.Focus();
            return;
        }

        ProjectDataLinkBLL projectDataLinkBLL = new ProjectDataLinkBLL();
        ProjectDataLink projectDataLink = new ProjectDataLink();

        try
        {
            projectDataLink.Code = strCode;
            projectDataLink.Host = strHost;
            projectDataLink.DataBaseName = strDataBaseName;
            projectDataLink.LoginNo = strLoginNo;
            projectDataLink.PassWord = strPassWord;

            projectDataLinkBLL.UpdateProjectDataLink(projectDataLink, projectDataLink.Code);

            LoadProjectDataLink();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

        }
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        for (int i = 0; i < DataGrid1.Items.Count; i++)
        {
            DataGrid1.Items[i].ForeColor = Color.Black;
        }
        e.Item.ForeColor = Color.Red;

        string strCode = ((Button)e.Item.FindControl("BT_Code")).Text.Trim();
        string strHost = e.Item.Cells[1].Text.Trim();
        string strDataBaseName = e.Item.Cells[2].Text.Trim();
        string strLoginNo = e.Item.Cells[3].Text.Trim();
        string strPassWord = e.Item.Cells[4].Text.Trim();

        TB_Code.Text = strCode;
        TB_Host.Text = strHost;
        TB_DataBaseName.Text = strDataBaseName;
        TB_LoginNo.Text = strLoginNo;
        TB_PassWord.Text = strPassWord;
        lbl_Code.Text = strCode;
    }

    /// <summary>
    /// 数据库接口
    /// </summary>
    protected void LoadProjectDataLink()
    {
        string strHQL = "from ProjectDataLink as projectDataLink order by projectDataLink.Code ASC";
        ProjectDataLinkBLL projectDataLinkBLL = new ProjectDataLinkBLL();
        IList lst = projectDataLinkBLL.GetAllProjectDataLinks(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    /// <summary>
    /// 货币类型
    /// </summary>
    protected void LoadCurrencyType()
    {
        string strHQL = "from CurrencyType as currencyType order by currencyType.SortNo ASC";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    /// <summary>
    /// 判断数据库接口编码是否存在  存在返回true；不存在则返回false
    /// </summary>
    protected bool IsProjectDataLink(string strcode, string strcodegold)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strcodegold))//新增
        {
            strHQL = "from ProjectDataLink as projectDataLink Where projectDataLink.Code='" + strcode + "' ";
        }
        else//更新
        {
            strHQL = "from ProjectDataLink as projectDataLink Where projectDataLink.Code='" + strcode + "' and projectDataLink.Code<>'" + strcodegold + "' ";
        }
        ProjectDataLinkBLL projectDataLinkBLL = new ProjectDataLinkBLL();
        IList lst = projectDataLinkBLL.GetAllProjectDataLinks(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;
        return flag;
    }

    protected void BT_NewCurrencyType_Click(object sender, EventArgs e)
    {
        string ExchangeRate = string.IsNullOrEmpty(TB_ExchangeRate.Text.Trim()) || TB_ExchangeRate.Text.Trim() == "" ? "1" : TB_ExchangeRate.Text.Trim();
        string SortNo = string.IsNullOrEmpty(TB_SortNo.Text.Trim()) || TB_SortNo.Text.Trim() == "" ? "1" : TB_SortNo.Text.Trim();
        if (IsCurrencyType(TB_Type.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBZZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_Type.Focus();
            return;
        }
        if (!IsNumeric(ExchangeRate))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDHLYDY0DSZJC").ToString().Trim() + "')", true);
            TB_ExchangeRate.Focus();
            return;
        }
        if (ExchangeRate.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDHLYDY0DSZJC").ToString().Trim() + "')", true);
            TB_ExchangeRate.Focus();
            return;
        }
        if (!IsNumeric(SortNo))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDPXYDY0DZSJC").ToString().Trim() + "')", true);
            TB_SortNo.Focus();
            return;
        }
        if (SortNo.Contains(".") || SortNo.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDPXYDY0DZSJC").ToString().Trim() + "')", true);
            TB_SortNo.Focus();
            return;
        }

        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        CurrencyType currencyType = new CurrencyType();

        try
        {
            currencyType.ExchangeRate = decimal.Parse(TB_ExchangeRate.Text.Trim());
            currencyType.SortNo = int.Parse(TB_SortNo.Text.Trim());
            currencyType.Type = TB_Type.Text.Trim();

            currencyTypeBLL.AddCurrencyType(currencyType);

            LoadCurrencyType();
        }
        catch
        {
        }
    }

    protected void BT_UpdateCurrencyType_Click(object sender, EventArgs e)
    {
        string ExchangeRate = string.IsNullOrEmpty(TB_ExchangeRate.Text.Trim()) || TB_ExchangeRate.Text.Trim() == "" ? "1" : TB_ExchangeRate.Text.Trim();
        string SortNo = string.IsNullOrEmpty(TB_SortNo.Text.Trim()) || TB_SortNo.Text.Trim() == "" ? "1" : TB_SortNo.Text.Trim();
        if (IsCurrencyType(TB_Type.Text.Trim(), lbl_TypeOld.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBZZSJBZYCZJC").ToString().Trim() + "')", true);
            TB_Type.Focus();
            return;
        }
        if (!IsNumeric(ExchangeRate))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDHLYDY0DSZJC").ToString().Trim() + "')", true);
            TB_ExchangeRate.Focus();
            return;
        }
        if (ExchangeRate.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDHLYDY0DSZJC").ToString().Trim() + "')", true);
            TB_ExchangeRate.Focus();
            return;
        }
        if (!IsNumeric(SortNo))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDPXYDY0DZSJC").ToString().Trim() + "')", true);
            TB_SortNo.Focus();
            return;
        }
        if (SortNo.Contains(".") || SortNo.Contains("-"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSRZDPXYDY0DZSJC").ToString().Trim() + "')", true);
            TB_SortNo.Focus();
            return;
        }

        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        CurrencyType currencyType = new CurrencyType();

        try
        {
            currencyType.ExchangeRate = decimal.Parse(TB_ExchangeRate.Text.Trim());
            currencyType.SortNo = int.Parse(TB_SortNo.Text.Trim());
            currencyType.Type = TB_Type.Text.Trim();

            currencyTypeBLL.UpdateCurrencyType(currencyType, currencyType.Type);

            LoadCurrencyType();

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        for (int i = 0; i < DataGrid2.Items.Count; i++)
        {
            DataGrid2.Items[i].ForeColor = Color.Black;
        }
        e.Item.ForeColor = Color.Red;

        string strType = ((Button)e.Item.FindControl("BT_Type")).Text.Trim();
        string strExchangeRate = e.Item.Cells[1].Text.Trim();
        string strSortNo = e.Item.Cells[2].Text.Trim();

        TB_Type.Text = strType;
        TB_ExchangeRate.Text = strExchangeRate;
        TB_SortNo.Text = strSortNo;
        lbl_TypeOld.Text = strType;
    }

    /// <summary>
    /// 判断货币币种是否存在  存在返回true；不存在则返回false
    /// </summary>
    protected bool IsCurrencyType(string strType, string strTypeOld)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strType))//新增
        {
            strHQL = "from CurrencyType as currencyType Where currencyType.Type='" + strType + "' ";
        }
        else//更新
        {
            strHQL = "from CurrencyType as currencyType Where currencyType.Type='" + strType + "' and currencyType.Type<>'" + strTypeOld + "' ";
        }
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag = true;
        }
        else
            flag = false;
        return flag;
    }

    //判断输入的字符是否是数字
    private bool IsNumeric(string str)
    {
        System.Text.RegularExpressions.Regex reg1
            = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
        return reg1.IsMatch(str);
    }
}