using System; using System.Resources;
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

public partial class TTHSERectificationReview : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","安全隐患整改评审", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_EstimateCompletionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ReviewDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_Reviewer.Text = GetUserName(strUserCode);

            LoadHSERectificationNoticeName();

            LoadHSERectificationList();
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL = "From HSERectification as hSERectification where hSERectification.Code = '" + LB_Code.Text.Trim() + "'";
        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        IList lst = hSERectificationBLL.GetAllHSERectifications(strHQL);
        HSERectification hSERectification = (HSERectification)lst[0];

        hSERectification.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());
        hSERectification.Reviewer = TB_Reviewer.Text.Trim();
        hSERectification.ReviewResult = TB_ReviewResult.Text.Trim();
        hSERectification.Status = "Qualified";
        DL_Status.SelectedValue = "Qualified";

        try
        {
            hSERectificationBLL.UpdateHSERectification(hSERectification, hSERectification.Code);
            UpdateHSERectificationNotice(hSERectification.RectificationNoticeId);//修改整改通知单的状态 完成
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPSHGCG").ToString().Trim()+"')", true);

            LoadHSERectificationList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPSHGSBJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }


    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL = "From HSERectification as hSERectification where hSERectification.Code = '" + LB_Code.Text.Trim() + "'";
        HSERectificationBLL hSERectificationBLL = new HSERectificationBLL();
        IList lst = hSERectificationBLL.GetAllHSERectifications(strHQL);
        HSERectification hSERectification = (HSERectification)lst[0];

        hSERectification.ReviewDate = DateTime.Parse(DLC_ReviewDate.Text.Trim());
        hSERectification.Reviewer = TB_Reviewer.Text.Trim();
        hSERectification.ReviewResult = TB_ReviewResult.Text.Trim();
        hSERectification.Status = "Unqualified";
        DL_Status.SelectedValue = "Unqualified";

        try
        {
            hSERectificationBLL.UpdateHSERectification(hSERectification, hSERectification.Code);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPSBHGCG").ToString().Trim()+"')", true);

            LoadHSERectificationList();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPSBHGSBJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
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

            strHQL = "From HSERectification as hSERectification where hSERectification.Code = '" + strId + "'";
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

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
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
            hSERectificationNotice.Status = "Completed";
            hSERectificationNoticeBLL.UpdateHSERectificationNotice(hSERectificationNotice, hSERectificationNotice.Code);
        }
    }

    /// <summary>
    /// 获取用户名称
    /// </summary>
    /// <param name="strUserCode"></param>
    /// <returns></returns>
    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = '" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            strUserName = projectMember.UserName.Trim();
        }
        else
            strUserName = "";
        return strUserName;
    }

    protected void LoadHSERectificationNoticeName()
    {
        string strHQL;
        IList lst;
        //
        strHQL = "From HSERectificationNotice as hSERectificationNotice Order By hSERectificationNotice.Code Desc";
        HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
        lst = hSERectificationNoticeBLL.GetAllHSERectificationNotices(strHQL);
        DL_RectificationNoticeId.DataSource = lst;
        DL_RectificationNoticeId.DataBind();
        DL_RectificationNoticeId.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadHSERectificationList()
    {
        string strHQL;

        strHQL = "Select * From T_HSERectification Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or RectificationOpinions like '%" + TextBox1.Text.Trim() + "%' " +
            "or ReviewResult like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (RectificationNoticeId like '%" + TextBox2.Text.Trim() + "%' or RectificationNoticeName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-ReviewDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-ReviewDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectification");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
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
}