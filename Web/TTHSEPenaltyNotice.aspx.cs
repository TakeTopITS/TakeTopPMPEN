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

public partial class TTHSEPenaltyNotice : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","处罚通知", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_PenaltyDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_AuditDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadHSERectificationName();

            LoadHSEPenaltyNoticeList();

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_RectificationCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_RectificationCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGAYHZGWBXCZSBJC").ToString().Trim() + "')", true);
            DL_RectificationCode.Focus();
            return;
        }
        string status = GetHSERectificationStatus(DL_RectificationCode.SelectedValue.Trim());
        if (!status.Equals("Qualified"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGAYHZGDBHGCZSBJC").ToString().Trim() + "')", true);
            DL_RectificationCode.Focus();
            return;
        }
        if (IsHSEPenaltyNoticeCode(string.Empty, TB_Name.Text.Trim(), DL_RectificationCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCHAYHZGDYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            DL_RectificationCode.Focus();
            return;
        }

        HSEPenaltyNoticeBLL hSEPenaltyNoticeBLL = new HSEPenaltyNoticeBLL();
        HSEPenaltyNotice hSEPenaltyNotice = new HSEPenaltyNotice();

        hSEPenaltyNotice.Code = GetHSEPenaltyNoticeCode();
        LB_Code.Text = hSEPenaltyNotice.Code.Trim();
        hSEPenaltyNotice.AuditDate = DateTime.Parse(DLC_AuditDate.Text.Trim());
        hSEPenaltyNotice.AuditDepartCode = TB_AuditDepartCode.Text.Trim();
        hSEPenaltyNotice.AuditOpinion = TB_AuditOpinion.Text.Trim();
        hSEPenaltyNotice.Auditor = TB_Auditor.Text.Trim();
        hSEPenaltyNotice.Currency = DL_Currency.SelectedValue.Trim();
        hSEPenaltyNotice.Name = TB_Name.Text.Trim();
        hSEPenaltyNotice.Measures = TB_Measures.Text.Trim();
        hSEPenaltyNotice.RectificationCode = DL_RectificationCode.SelectedValue.Trim();
        hSEPenaltyNotice.RectificationName = GetHSERectificationName(hSEPenaltyNotice.RectificationCode.Trim());
        hSEPenaltyNotice.PenaltyDate = DateTime.Parse(DLC_PenaltyDate.Text.Trim());
        hSEPenaltyNotice.PenaltyDepartCode = TB_PenaltyDepartCode.Text.Trim();
        hSEPenaltyNotice.PenaltyMoney = NB_PenaltyMoney.Amount;
        hSEPenaltyNotice.PenaltyRemark = TB_PenaltyRemark.Text.Trim();
        hSEPenaltyNotice.VerificationResults = TB_VerificationResults.Text.Trim();
        hSEPenaltyNotice.Status = DL_Status.SelectedValue.Trim();
        hSEPenaltyNotice.EnterCode = strUserCode.Trim();

        try
        {
            hSEPenaltyNoticeBLL.AddHSEPenaltyNotice(hSEPenaltyNotice);

            LoadHSEPenaltyNoticeList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }


    protected void Update()
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_RectificationCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_RectificationCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGAYHZGWBXCZSBJC").ToString().Trim() + "')", true);
            DL_RectificationCode.Focus();
            return;
        }
        string status = GetHSERectificationStatus(DL_RectificationCode.SelectedValue.Trim());
        if (!status.Equals("Qualified"))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGAYHZGDBHGCZSBJC").ToString().Trim() + "')", true);
            DL_RectificationCode.Focus();
            return;
        }
        if (IsHSEPenaltyNoticeCode(LB_Code.Text.Trim(), TB_Name.Text.Trim(), DL_RectificationCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCHAYHZGDYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            DL_RectificationCode.Focus();
            return;
        }

        string strHQL = "From HSEPenaltyNotice as hSEPenaltyNotice where hSEPenaltyNotice.Code = '" + LB_Code.Text.Trim() + "'";
        HSEPenaltyNoticeBLL hSEPenaltyNoticeBLL = new HSEPenaltyNoticeBLL();
        IList lst = hSEPenaltyNoticeBLL.GetAllHSEPenaltyNotices(strHQL);

        HSEPenaltyNotice hSEPenaltyNotice = (HSEPenaltyNotice)lst[0];

        //hSEPenaltyNotice.Code = LB_Code.Text.Trim();
        hSEPenaltyNotice.AuditDate = DateTime.Parse(DLC_AuditDate.Text.Trim());
        hSEPenaltyNotice.AuditDepartCode = TB_AuditDepartCode.Text.Trim();
        hSEPenaltyNotice.AuditOpinion = TB_AuditOpinion.Text.Trim();
        hSEPenaltyNotice.Auditor = TB_Auditor.Text.Trim();
        hSEPenaltyNotice.Currency = DL_Currency.SelectedValue.Trim();
        hSEPenaltyNotice.Name = TB_Name.Text.Trim();
        hSEPenaltyNotice.Measures = TB_Measures.Text.Trim();
        hSEPenaltyNotice.RectificationCode = DL_RectificationCode.SelectedValue.Trim();
        hSEPenaltyNotice.RectificationName = GetHSERectificationName(hSEPenaltyNotice.RectificationCode.Trim());
        hSEPenaltyNotice.PenaltyDate = DateTime.Parse(DLC_PenaltyDate.Text.Trim());
        hSEPenaltyNotice.PenaltyDepartCode = TB_PenaltyDepartCode.Text.Trim();
        hSEPenaltyNotice.PenaltyMoney = NB_PenaltyMoney.Amount;
        hSEPenaltyNotice.PenaltyRemark = TB_PenaltyRemark.Text.Trim();
        hSEPenaltyNotice.VerificationResults = TB_VerificationResults.Text.Trim();
        hSEPenaltyNotice.Status = DL_Status.SelectedValue.Trim();

        try
        {
            hSEPenaltyNoticeBLL.UpdateHSEPenaltyNotice(hSEPenaltyNotice, hSEPenaltyNotice.Code);

            LoadHSEPenaltyNoticeList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = LB_Code.Text.Trim();

        strHQL = "Delete From T_HSEPenaltyNotice Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEPenaltyNoticeList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
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

                strHQL = "From HSEPenaltyNotice as hSEPenaltyNotice where hSEPenaltyNotice.Code = '" + strCode + "'";
                HSEPenaltyNoticeBLL hSEPenaltyNoticeBLL = new HSEPenaltyNoticeBLL();
                lst = hSEPenaltyNoticeBLL.GetAllHSEPenaltyNotices(strHQL);

                HSEPenaltyNotice hSEPenaltyNotice = (HSEPenaltyNotice)lst[0];

                LB_Code.Text = hSEPenaltyNotice.Code.Trim();
                TB_PenaltyDepartCode.Text = hSEPenaltyNotice.PenaltyDepartCode.Trim();
                DL_RectificationCode.SelectedValue = hSEPenaltyNotice.RectificationCode.Trim();
                TB_AuditOpinion.Text = hSEPenaltyNotice.AuditOpinion.Trim();
                DLC_PenaltyDate.Text = hSEPenaltyNotice.PenaltyDate.ToString("yyyy-MM-dd");
                TB_Auditor.Text = hSEPenaltyNotice.Auditor.Trim();
                DLC_AuditDate.Text = hSEPenaltyNotice.AuditDate.ToString("yyyy-MM-dd");
                TB_Measures.Text = hSEPenaltyNotice.Measures.Trim();
                TB_Name.Text = hSEPenaltyNotice.Name.Trim();
                TB_AuditDepartCode.Text = hSEPenaltyNotice.AuditDepartCode.Trim();
                NB_PenaltyMoney.Amount = hSEPenaltyNotice.PenaltyMoney;
                DL_Status.SelectedValue = hSEPenaltyNotice.Status.Trim();
                DL_Currency.SelectedValue = hSEPenaltyNotice.Currency.Trim();
                TB_PenaltyRemark.Text = hSEPenaltyNotice.PenaltyRemark.Trim();
                TB_VerificationResults.Text = hSEPenaltyNotice.VerificationResults.Trim();

                //if (hSEPenaltyNotice.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Visible = true;
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPenaltyNotice");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEPenaltyNoticeList();
    }

    protected void DL_RectificationCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        //绑定整改通知名称
        strHQL = "From HSERectification as hSERectification Where hSERectification.Code='" + DL_RectificationCode.SelectedValue.Trim() + "' ";
        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        lst = hSERectificationBLL.GetAllHSERectifications(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSERectification hSERectification = (HSERectification)lst[0];
            TB_PenaltyDepartCode.Text = hSERectification.DepartCode.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void LoadHSERectificationName()
    {
        string strHQL;
        IList lst;
        //绑定整改名称Status = "Qualified"
        strHQL = "From HSERectification as hSERectification Order By hSERectification.Code DESC";
        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        lst = hSERectificationBLL.GetAllHSERectifications(strHQL);
        DL_RectificationCode.DataSource = lst;
        DL_RectificationCode.DataBind();
        DL_RectificationCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadCurrency()
    {
        string strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC ";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        DL_Currency.DataSource = lst;
        DL_Currency.DataBind();
    }

    /// <summary>
    /// 合格的整改单才可以进行罚款通知
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetHSERectificationStatus(string strCode)
    {
        string strHQL = "From HSERectification as hSERectification Where hSERectification.Code ='" + strCode.Trim() + "' ";
        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        IList lst = hSERectificationBLL.GetAllHSERectifications(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSERectification hSERectification = (HSERectification)lst[0];
            return hSERectification.Status.Trim();
        }
        else
            return "";
    }

    protected void LoadHSEPenaltyNoticeList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEPenaltyNotice Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or PenaltyRemark like '%" + TextBox1.Text.Trim() + "%' " +
            "or Measures like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (RectificationCode like '%" + TextBox2.Text.Trim() + "%' or RectificationName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-PenaltyDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-PenaltyDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPenaltyNotice");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    
    /// <summary>
    /// 新增或修改时，处罚通知名称及整改单是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEPenaltyNoticeCode(string strCode, string strName, string strRectificationCode)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
            strHQL = "Select Code From T_HSEPenaltyNotice Where Name='" + strName + "' or RectificationCode='" + strRectificationCode + "' ";
        else
            strHQL = "Select Code From T_HSEPenaltyNotice Where (Name='" + strName + "' or RectificationCode='" + strRectificationCode + "') and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPenaltyNotice").Tables[0];
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
    /// 新增时，获取表T_HSEPenaltyNotice中最大编号 规则HSEPNNX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSEPenaltyNoticeCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEPenaltyNotice Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPenaltyNotice").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEPNN" + pa.ToString();
        }
        else
        {
            flag = "HSEPNN1";
        }
        return flag;
    }

    protected string GetHSERectificationName(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定整改名称
        strHQL = "From HSERectification as hSERectification Where hSERectification.Code='" + strCode + "' ";
        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        lst = hSERectificationBLL.GetAllHSERectifications(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSERectification hSERectification = (HSERectification)lst[0];
            return hSERectification.Name.Trim();
        }
        else
            return "";
    }

}