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

public partial class TTQMQualityRectification : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","质量隐患整改", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMQualityRectificationNoticeName();

            LoadQMQualityRectificationList();
        }
    }

    protected void LoadQMQualityRectificationNoticeName()
    {
        string strHQL;
        IList lst;
        strHQL = "From QMQualityRectificationNotice as qMQualityRectificationNotice Order By qMQualityRectificationNotice.Code Desc";
        QMQualityRectificationNoticeBLL qMQualityRectificationNoticeBLL = new QMQualityRectificationNoticeBLL();
        lst = qMQualityRectificationNoticeBLL.GetAllQMQualityRectificationNotices(strHQL);
        DL_RectificationNoticeCode.DataSource = lst;
        DL_RectificationNoticeCode.DataBind();
        DL_RectificationNoticeCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadQMQualityRectificationList()
    {
        string strHQL;

        strHQL = "Select * From T_QMQualityRectification Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or HeadUnit like '%" + TextBox1.Text.Trim() + "%' or ResponsibilityUnit like '%" + TextBox1.Text.Trim() + "%' " +
                "or CreatePer like '%" + TextBox1.Text.Trim() + "%' or RectificationRemark like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (RectificationNoticeCode like '%" + TextBox2.Text.Trim() + "%' or RectificationNoticeName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-CreateTime::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-CreateTime::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectification");

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
        if (DL_RectificationNoticeCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_RectificationNoticeCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZGTZDWBXCZSBJC").ToString().Trim() + "')", true);
            DL_RectificationNoticeCode.Focus();
            return;
        }

        QMQualityRectificationBLL qMQualityRectificationBLL = new QMQualityRectificationBLL();
        QMQualityRectification qMQualityRectification = new QMQualityRectification();

        qMQualityRectification.Code = GetQMQualityRectificationCode();
        LB_Code.Text = qMQualityRectification.Code.Trim();
        qMQualityRectification.HeadUnit = TB_HeadUnit.Text.Trim();
        qMQualityRectification.RectificationRemark = TB_RectificationRemark.Text.Trim();
        qMQualityRectification.RectificationNoticeCode = DL_RectificationNoticeCode.SelectedValue.Trim();
        qMQualityRectification.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityRectification.RectificationNoticeName = GetQMQualityRectificationNoticeName(qMQualityRectification.RectificationNoticeCode.Trim());
        qMQualityRectification.CreateTime = DateTime.Now;
        qMQualityRectification.ResponsibilityUnit = TB_ResponsibilityUnit.Text.Trim();
        qMQualityRectification.EnterCode = strUserCode.Trim();

        try
        {
            qMQualityRectificationBLL.AddQMQualityRectification(qMQualityRectification);
            UpdateQMQualityRectificationNotice(qMQualityRectification.RectificationNoticeCode.Trim(), "Rectified");
            LoadQMQualityRectificationList();

            //BT_Delete.Visible = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 新增时，获取表T_QMQualityRectification中最大编号 规则QMQRFX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMQualityRectificationCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMQualityRectification Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectification").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMQRF" + pa.ToString();
        }
        else
        {
            flag = "QMQRF1";
        }
        return flag;
    }

    protected string GetQMQualityRectificationNoticeName(string strPCCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From QMQualityRectificationNotice as qMQualityRectificationNotice Where qMQualityRectificationNotice.Code='" + strPCCode + "' ";
        QMQualityRectificationNoticeBLL qMQualityRectificationNoticeBLL = new QMQualityRectificationNoticeBLL();
        lst = qMQualityRectificationNoticeBLL.GetAllQMQualityRectificationNotices(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMQualityRectificationNotice proj = (QMQualityRectificationNotice)lst[0];
            return proj.Name.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        if (DL_RectificationNoticeCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_RectificationNoticeCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZGTZDWBXCZSBJC").ToString().Trim() + "')", true);
            DL_RectificationNoticeCode.Focus();
            return;
        }

        string strHQL = "From QMQualityRectification as qMQualityRectification where qMQualityRectification.Code = '" + LB_Code.Text.Trim() + "'";
        QMQualityRectificationBLL qMQualityRectificationBLL = new QMQualityRectificationBLL();
        IList lst = qMQualityRectificationBLL.GetAllQMQualityRectifications(strHQL);

        QMQualityRectification qMQualityRectification = (QMQualityRectification)lst[0];

        string RectificationNoticeCodeOld = qMQualityRectification.RectificationNoticeCode.Trim();
        qMQualityRectification.HeadUnit = TB_HeadUnit.Text.Trim();
        qMQualityRectification.RectificationRemark = TB_RectificationRemark.Text.Trim();
        qMQualityRectification.RectificationNoticeCode = DL_RectificationNoticeCode.SelectedValue.Trim();
        qMQualityRectification.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityRectification.RectificationNoticeName = GetQMQualityRectificationNoticeName(qMQualityRectification.RectificationNoticeCode.Trim());
        qMQualityRectification.ResponsibilityUnit = TB_ResponsibilityUnit.Text.Trim();

        try
        {
            qMQualityRectificationBLL.UpdateQMQualityRectification(qMQualityRectification, qMQualityRectification.Code);
            if (!RectificationNoticeCodeOld.Trim().Equals(qMQualityRectification.RectificationNoticeCode.Trim()))
            {
                if (!IsQMQualityRectification(qMQualityRectification.Code.Trim(), RectificationNoticeCodeOld.Trim()))
                {
                    UpdateQMQualityRectificationNotice(RectificationNoticeCodeOld.Trim(), "New");
                }

                UpdateQMQualityRectificationNotice(qMQualityRectification.RectificationNoticeCode.Trim(), "Rectified");
            }
            LoadQMQualityRectificationList();

            //BT_Delete.Visible = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
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

        if (!IsQMQualityRectification(strCode, DL_RectificationNoticeCode.SelectedValue.Trim()))
        {
            UpdateQMQualityRectificationNotice(DL_RectificationNoticeCode.SelectedValue.Trim(), "New");
        }

        strHQL = "Delete From T_QMQualityRectification Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMQualityRectificationList();

            //BT_Delete.Visible = false;
            //BT_Update.Visible = false;

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


                strHQL = "From QMQualityRectification as qMQualityRectification where qMQualityRectification.Code = '" + strCode + "'";
                QMQualityRectificationBLL qMQualityRectificationBLL = new QMQualityRectificationBLL();
                lst = qMQualityRectificationBLL.GetAllQMQualityRectifications(strHQL);

                QMQualityRectification qMQualityRectification = (QMQualityRectification)lst[0];

                LB_Code.Text = qMQualityRectification.Code.Trim();
                DL_RectificationNoticeCode.SelectedValue = qMQualityRectification.RectificationNoticeCode.Trim();
                TB_RectificationRemark.Text = qMQualityRectification.RectificationRemark.Trim();
                TB_CreatePer.Text = qMQualityRectification.CreatePer.Trim();
                TB_HeadUnit.Text = qMQualityRectification.HeadUnit.Trim();
                TB_ResponsibilityUnit.Text = qMQualityRectification.ResponsibilityUnit.Trim();
                //if (qMQualityRectification.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Delete.Visible = true;
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Delete.Visible = false;
                //    BT_Update.Visible = false;
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectification");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMQualityRectificationList();
    }

    protected void DL_RectificationNoticeCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From QMQualityRectificationNotice as qMQualityRectificationNotice Where qMQualityRectificationNotice.Code ='" + DL_RectificationNoticeCode.SelectedValue.Trim() + "' ";
        QMQualityRectificationNoticeBLL qMQualityRectificationNoticeBLL = new QMQualityRectificationNoticeBLL();
        IList lst = qMQualityRectificationNoticeBLL.GetAllQMQualityRectificationNotices(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMQualityRectificationNotice qMQualityRectificationNotice = (QMQualityRectificationNotice)lst[0];
            TB_ResponsibilityUnit.Text = qMQualityRectificationNotice.ResponsibilityUnit.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    protected void UpdateQMQualityRectificationNotice(string strCode, string strstatus)
    {
        string strHQL = "From QMQualityRectificationNotice as qMQualityRectificationNotice Where qMQualityRectificationNotice.Code ='" + strCode.Trim() + "' ";
        QMQualityRectificationNoticeBLL qMQualityRectificationNoticeBLL = new QMQualityRectificationNoticeBLL();
        IList lst = qMQualityRectificationNoticeBLL.GetAllQMQualityRectificationNotices(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMQualityRectificationNotice qMQualityRectificationNotice = (QMQualityRectificationNotice)lst[0];
            if (strstatus.Trim() == "New")
            {
                qMQualityRectificationNotice.Status = "New";
            }
            else
                qMQualityRectificationNotice.Status = "Rectified";
            qMQualityRectificationNoticeBLL.UpdateQMQualityRectificationNotice(qMQualityRectificationNotice, qMQualityRectificationNotice.Code);
        }
    }

    /// <summary>
    /// 删除时，检查整改通知单是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMQualityRectification(string strCode, string strNoticeCode)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_QMQualityRectification Where RectificationNoticeCode='" + strNoticeCode + "' ";
        }
        else
            strHQL = "Select Code From T_QMQualityRectification Where RectificationNoticeCode='" + strNoticeCode + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectification").Tables[0];
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
}