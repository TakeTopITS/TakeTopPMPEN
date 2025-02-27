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

public partial class TTHSEPenaltyNoticeReview : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","处罚通知评审", strUserCode);
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

    protected void LoadCurrency()
    {
        string strHQL = "From CurrencyType as currencyType Order By currencyType.SortNo ASC ";
        CurrencyTypeBLL currencyTypeBLL = new CurrencyTypeBLL();
        IList lst = currencyTypeBLL.GetAllCurrencyTypes(strHQL);
        DL_Currency.DataSource = lst;
        DL_Currency.DataBind();
    }

    protected void LoadHSERectificationName()
    {
        string strHQL;
        IList lst;
        //绑定整改名称Status = "Qualified"
        strHQL = "From HSERectification as hSERectification Order By hSERectification.Code Desc";
        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        lst = hSERectificationBLL.GetAllHSERectifications(strHQL);
        DL_RectificationCode.DataSource = lst;
        DL_RectificationCode.DataBind();
        DL_RectificationCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadHSEPenaltyNoticeList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEPenaltyNotice Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or AuditOpinion like '%" + TextBox1.Text.Trim() + "%' " +
            "or VerificationResults like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (RectificationCode like '%" + TextBox2.Text.Trim() + "%' or RectificationName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-AuditDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-AuditDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPenaltyNotice");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
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

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }

        string strHQL = "From HSEPenaltyNotice as hSEPenaltyNotice where hSEPenaltyNotice.Code = '" + LB_Code.Text.Trim() + "'";
        HSEPenaltyNoticeBLL hSEPenaltyNoticeBLL = new HSEPenaltyNoticeBLL();
        IList lst = hSEPenaltyNoticeBLL.GetAllHSEPenaltyNotices(strHQL);

        HSEPenaltyNotice hSEPenaltyNotice = (HSEPenaltyNotice)lst[0];

        //      hSEPenaltyNotice.Code = LB_Code.Text.Trim();
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
        hSEPenaltyNotice.Status = "Completed";
        DL_Status.SelectedValue = "Completed";

        try
        {
            hSEPenaltyNoticeBLL.UpdateHSEPenaltyNotice(hSEPenaltyNotice, hSEPenaltyNotice.Code);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            LoadHSEPenaltyNoticeList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
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
        hSEPenaltyNotice.Status = "New";
        DL_Status.SelectedValue = "New";

        try
        {
            hSEPenaltyNoticeBLL.UpdateHSEPenaltyNotice(hSEPenaltyNotice, hSEPenaltyNotice.Code);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);

            LoadHSEPenaltyNoticeList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);


        }
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_Code")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "From HSEPenaltyNotice as hSEPenaltyNotice where hSEPenaltyNotice.Code = '" + strId + "'";
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

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

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
}