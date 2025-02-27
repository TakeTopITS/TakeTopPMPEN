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

public partial class TTHSERectification : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","安全隐患整改", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_EstimateCompletionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ReviewDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadHSERectificationNoticeName();

            LoadHSERectificationList();
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
        if (IsHSERectificationCode(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_RectificationNoticeId.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_RectificationNoticeId.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYHZGTZWBXCZSBJC").ToString().Trim() + "')", true);
            DL_RectificationNoticeId.Focus();
            return;
        }
        string status = GetHSERectificationNoticeStatus(DL_RectificationNoticeId.SelectedValue.Trim());
        if (status == "Completed")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGYHZGTZYWCWXZGJC").ToString().Trim() + "')", true);
            DL_RectificationNoticeId.Focus();
            return;
        }

        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        HSERectification hSERectification = new HSERectification();

        hSERectification.Code = GetHSERectificationCode();
        LB_Code.Text = hSERectification.Code.Trim();
        hSERectification.DepartCode = TB_DepartCode.Text.Trim();
        hSERectification.CauseAnalysis = TB_CauseAnalysis.Text.Trim();
        hSERectification.CorrectiveAction = TB_CorrectiveAction.Text.Trim();
        hSERectification.EstimateCompletionDate = DateTime.Parse(DLC_EstimateCompletionDate.Text.Trim());
        hSERectification.ImplementationHeader = TB_ImplementationHeader.Text.Trim();
        hSERectification.Name = TB_Name.Text.Trim();
        hSERectification.NoFactDescribe = TB_NoFactDescribe.Text.Trim();
        hSERectification.RectificationNoticeId = DL_RectificationNoticeId.SelectedValue.Trim();
        hSERectification.RectificationNoticeName = GetHSERectificationNoticeName(hSERectification.RectificationNoticeId.Trim());
        hSERectification.RectificationOpinions = TB_RectificationOpinions.Text.Trim();
        hSERectification.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());
        hSERectification.Reviewer = TB_Reviewer.Text.Trim();
        hSERectification.ReviewResult = TB_ReviewResult.Text.Trim();
        hSERectification.Type = TB_Type.Text.Trim();
        hSERectification.UnitDepartCode = TB_UnitDepartCode.Text.Trim();
        hSERectification.Status = DL_Status.SelectedValue.Trim();
        hSERectification.EnterCode = strUserCode.Trim();

        try
        {
            hSERectificationBLL.AddHSERectification(hSERectification);
            UpdateHSERectificationNotice(hSERectification.RectificationNoticeId);//修改整改通知单的状态 整改中

            LoadHSERectificationList();

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
        if (IsHSERectificationCode(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_RectificationNoticeId.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_RectificationNoticeId.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYHZGTZWBXCZSBJC").ToString().Trim() + "')", true);
            DL_RectificationNoticeId.Focus();
            return;
        }
        string status = GetHSERectificationNoticeStatus(DL_RectificationNoticeId.SelectedValue.Trim());
        if (status == "Completed")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGYHZGTZYWCWXZGJC").ToString().Trim() + "')", true);
            DL_RectificationNoticeId.Focus();
            return;
        }

        string strHQL = "From HSERectification as hSERectification where hSERectification.Code = '" + LB_Code.Text.Trim() + "'";
        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        IList lst = hSERectificationBLL.GetAllHSERectifications(strHQL);

        HSERectification hSERectification = (HSERectification)lst[0];

        //   hSERectification.Code = LB_Code.Text.Trim();
        hSERectification.DepartCode = TB_DepartCode.Text.Trim();
        hSERectification.CauseAnalysis = TB_CauseAnalysis.Text.Trim();
        hSERectification.CorrectiveAction = TB_CorrectiveAction.Text.Trim();
        hSERectification.EstimateCompletionDate = DateTime.Parse(DLC_EstimateCompletionDate.Text.Trim());
        hSERectification.ImplementationHeader = TB_ImplementationHeader.Text.Trim();
        hSERectification.Name = TB_Name.Text.Trim();
        hSERectification.NoFactDescribe = TB_NoFactDescribe.Text.Trim();
        hSERectification.RectificationNoticeId = DL_RectificationNoticeId.SelectedValue.Trim();
        hSERectification.RectificationNoticeName = GetHSERectificationNoticeName(hSERectification.RectificationNoticeId.Trim());
        hSERectification.RectificationOpinions = TB_RectificationOpinions.Text.Trim();
        hSERectification.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());
        hSERectification.Reviewer = TB_Reviewer.Text.Trim();
        hSERectification.ReviewResult = TB_ReviewResult.Text.Trim();
        hSERectification.Type = TB_Type.Text.Trim();
        hSERectification.UnitDepartCode = TB_UnitDepartCode.Text.Trim();
        hSERectification.Status = DL_Status.SelectedValue.Trim();

        try
        {
            hSERectificationBLL.UpdateHSERectification(hSERectification, hSERectification.Code);

            LoadHSERectificationList();

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
        if (IsHSEPenaltyNotice(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCFTZZYDYGZGDWFSC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Delete From T_HSERectification Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSERectificationList();

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

                strHQL = "From HSERectification as hSERectification where hSERectification.Code = '" + strCode + "'";
                HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
                lst = hSERectificationBLL.GetAllHSERectifications(strHQL);

                HSERectification hSERectification = (HSERectification)lst[0];

                LB_Code.Text = hSERectification.Code.Trim();
                TB_DepartCode.Text = hSERectification.DepartCode.Trim();
                DL_RectificationNoticeId.SelectedValue = hSERectification.RectificationNoticeId.Trim();
                TB_ReviewResult.Text = hSERectification.ReviewResult.Trim();
                DLC_EstimateCompletionDate.Text = hSERectification.EstimateCompletionDate.ToString("yyyy-MM-dd");
                TB_ImplementationHeader.Text = hSERectification.ImplementationHeader.Trim();
                DLC_ReviewDate.Text = hSERectification.ReviewDate.ToString("yyyy-MM-dd");
                TB_NoFactDescribe.Text = hSERectification.NoFactDescribe.Trim();
                TB_Name.Text = hSERectification.Name.Trim();
                TB_Reviewer.Text = hSERectification.Reviewer.Trim();
                TB_UnitDepartCode.Text = hSERectification.UnitDepartCode.Trim();
                DL_Status.SelectedValue = hSERectification.Status.Trim();
                TB_Type.Text = hSERectification.Type.Trim();
                TB_RectificationOpinions.Text = hSERectification.RectificationOpinions.Trim();
                TB_CorrectiveAction.Text = hSERectification.CorrectiveAction.Trim();
                TB_CauseAnalysis.Text = hSERectification.CauseAnalysis.Trim();
                //if (hSERectification.EnterCode.Trim() == strUserCode.Trim())
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

    /// <summary>
    /// 新增或更新时，整改名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSERectificationCode(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
            strHQL = "Select Code From T_HSERectification Where Name='" + strName + "'";
        else
            strHQL = "Select Code From T_HSERectification Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectification").Tables[0];
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
    /// 新增时，获取表T_HSERectification中最大编号 规则HSERTFX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSERectificationCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSERectification Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectification").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSERTF" + pa.ToString();
        }
        else
        {
            flag = "HSERTF1";
        }
        return flag;
    }

    protected string GetHSERectificationNoticeName(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定整改通知名称
        strHQL = "From HSERectificationNotice as hSERectificationNotice Where hSERectificationNotice.Code='" + strCode + "' ";
        HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
        lst = hSERectificationNoticeBLL.GetAllHSERectificationNotices(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSERectificationNotice hSERectificationNotice = (HSERectificationNotice)lst[0];
            return hSERectificationNotice.Name.Trim();
        }
        else
            return "";
    }

    protected void UpdateHSERectificationNotice(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定整改通知名称
        strHQL = "From HSERectificationNotice as hSERectificationNotice Where hSERectificationNotice.Code='" + strCode + "' ";
        HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
        lst = hSERectificationNoticeBLL.GetAllHSERectificationNotices(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSERectificationNotice hSERectificationNotice = (HSERectificationNotice)lst[0];
            hSERectificationNotice.Status = "Rectifying";
            hSERectificationNoticeBLL.UpdateHSERectificationNotice(hSERectificationNotice, hSERectificationNotice.Code);
        }
    }

    /// <summary>
    /// 删除时，检查处罚通知是否存在该安全隐患整改单，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEPenaltyNotice(string strCode)
    {
        bool flag = true;
        string strHQL = "Select Code From T_HSEPenaltyNotice Where RectificationCode='" + strCode + "'";
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

    protected void DL_RectificationNoticeId_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        //绑定整改通知名称
        strHQL = "From HSERectificationNotice as hSERectificationNotice Where hSERectificationNotice.Code='" + DL_RectificationNoticeId.SelectedValue.Trim() + "' ";
        HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
        lst = hSERectificationNoticeBLL.GetAllHSERectificationNotices(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSERectificationNotice hSERectificationNotice = (HSERectificationNotice)lst[0];
            TB_RectificationOpinions.Text = hSERectificationNotice.RectificationOpinions.Trim();
            TB_DepartCode.Text = hSERectificationNotice.DepartCode.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectification");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSERectificationList();
    }


    protected void LoadHSERectificationNoticeName()
    {
        string strHQL;
        IList lst;
        //绑定安检名称
        strHQL = "From HSERectificationNotice as hSERectificationNotice Order By hSERectificationNotice.Code Desc";
        HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
        lst = hSERectificationNoticeBLL.GetAllHSERectificationNotices(strHQL);
        DL_RectificationNoticeId.DataSource = lst;
        DL_RectificationNoticeId.DataBind();
        DL_RectificationNoticeId.Items.Insert(0, new ListItem("--Select--", ""));
    }

    /// <summary>
    /// 完成状态的安全隐患整改通知单排除
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetHSERectificationNoticeStatus(string strCode)
    {
        string strHQL = "From HSERectificationNotice as hSERectificationNotice Where hSERectificationNotice.Code='" + strCode.Trim() + "'";
        HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
        IList lst = hSERectificationNoticeBLL.GetAllHSERectificationNotices(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSERectificationNotice hSERectificationNotice = (HSERectificationNotice)lst[0];
            return hSERectificationNotice.Status.Trim();
        }
        else
            return "";
    }

    protected void LoadHSERectificationList()
    {
        string strHQL;

        strHQL = "Select * From T_HSERectification Where 1=1";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or RectificationOpinions like '%" + TextBox1.Text.Trim() + "%' " +
            "or NoFactDescribe like '%" + TextBox1.Text.Trim() + "%' or CauseAnalysis like '%" + TextBox1.Text.Trim() + "%' or CorrectiveAction like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (RectificationNoticeId like '%" + TextBox2.Text.Trim() + "%' or RectificationNoticeName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-EstimateCompletionDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-EstimateCompletionDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectification");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

}