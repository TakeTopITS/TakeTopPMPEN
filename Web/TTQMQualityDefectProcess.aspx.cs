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

public partial class TTQMQualityDefectProcess : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","质量缺陷处理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMQualityDefectNoticeName();

            LoadQMQualityDefectProcessList();
        }
    }

    protected void LoadQMQualityDefectNoticeName()
    {
        string strHQL;
        IList lst;
        strHQL = "From QMQualityDefectNotice as qMQualityDefectNotice Order By qMQualityDefectNotice.Code Desc";
        QMQualityDefectNoticeBLL qMQualityDefectNoticeBLL = new QMQualityDefectNoticeBLL();
        lst = qMQualityDefectNoticeBLL.GetAllQMQualityDefectNotices(strHQL);
        DL_QualityDefectNoticeCode.DataSource = lst;
        DL_QualityDefectNoticeCode.DataBind();
        DL_QualityDefectNoticeCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadQMQualityDefectProcessList()
    {
        string strHQL;

        strHQL = "Select * From T_QMQualityDefectProcess Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or DealRemark like '%" + TextBox1.Text.Trim() + "%' or CreatePer like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (QualityDefectNoticeCode like '%" + TextBox2.Text.Trim() + "%' or QualityDefectNoticeName like '%" + TextBox2.Text.Trim() + "%') ";
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

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectProcess");

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
        if (DL_QualityDefectNoticeCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_QualityDefectNoticeCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZLXTZDWBXCZSBJC").ToString().Trim() + "')", true);
            DL_QualityDefectNoticeCode.Focus();
            return;
        }

        QMQualityDefectProcessBLL qMQualityDefectProcessBLL = new QMQualityDefectProcessBLL();
        QMQualityDefectProcess qMQualityDefectProcess = new QMQualityDefectProcess();

        qMQualityDefectProcess.Code = GetQMQualityDefectProcessCode();
        LB_Code.Text = qMQualityDefectProcess.Code.Trim();
        qMQualityDefectProcess.DealRemark = TB_DealRemark.Text.Trim();
        qMQualityDefectProcess.QualityDefectNoticeCode = DL_QualityDefectNoticeCode.SelectedValue.Trim();
        qMQualityDefectProcess.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityDefectProcess.QualityDefectNoticeName = GetQMQualityDefectNoticeName(qMQualityDefectProcess.QualityDefectNoticeCode.Trim());
        qMQualityDefectProcess.CreateTime = DateTime.Now;
        qMQualityDefectProcess.EnterCode = strUserCode.Trim();

        try
        {
            qMQualityDefectProcessBLL.AddQMQualityDefectProcess(qMQualityDefectProcess);

            LoadQMQualityDefectProcessList();

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
    /// 新增时，获取表T_QMQualityDefectProcess中最大编号 规则QMQDPX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMQualityDefectProcessCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMQualityDefectProcess Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectProcess").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMQDP" + pa.ToString();
        }
        else
        {
            flag = "QMQDP1";
        }
        return flag;
    }

    protected string GetQMQualityDefectNoticeName(string strPCCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From QMQualityDefectNotice as qMQualityDefectNotice Where qMQualityDefectNotice.Code='" + strPCCode + "' ";
        QMQualityDefectNoticeBLL qMQualityDefectNoticeBLL = new QMQualityDefectNoticeBLL();
        lst = qMQualityDefectNoticeBLL.GetAllQMQualityDefectNotices(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMQualityDefectNotice proj = (QMQualityDefectNotice)lst[0];
            return proj.Name.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        if (DL_QualityDefectNoticeCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_QualityDefectNoticeCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZLXTZDWBXCZSBJC").ToString().Trim() + "')", true);
            DL_QualityDefectNoticeCode.Focus();
            return;
        }

        string strHQL = "From QMQualityDefectProcess as qMQualityDefectProcess where qMQualityDefectProcess.Code = '" + LB_Code.Text.Trim() + "'";
        QMQualityDefectProcessBLL qMQualityDefectProcessBLL = new QMQualityDefectProcessBLL();
        IList lst = qMQualityDefectProcessBLL.GetAllQMQualityDefectProcesss(strHQL);

        QMQualityDefectProcess qMQualityDefectProcess = (QMQualityDefectProcess)lst[0];

        qMQualityDefectProcess.DealRemark = TB_DealRemark.Text.Trim();
        qMQualityDefectProcess.QualityDefectNoticeCode = DL_QualityDefectNoticeCode.SelectedValue.Trim();
        qMQualityDefectProcess.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityDefectProcess.QualityDefectNoticeName = GetQMQualityDefectNoticeName(qMQualityDefectProcess.QualityDefectNoticeCode.Trim());

        try
        {
            qMQualityDefectProcessBLL.UpdateQMQualityDefectProcess(qMQualityDefectProcess, qMQualityDefectProcess.Code);

            LoadQMQualityDefectProcessList();

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

        strHQL = "Delete From T_QMQualityDefectProcess Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMQualityDefectProcessList();

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

                strHQL = "From QMQualityDefectProcess as qMQualityDefectProcess where qMQualityDefectProcess.Code = '" + strCode + "'";
                QMQualityDefectProcessBLL qMQualityDefectProcessBLL = new QMQualityDefectProcessBLL();
                lst = qMQualityDefectProcessBLL.GetAllQMQualityDefectProcesss(strHQL);

                QMQualityDefectProcess qMQualityDefectProcess = (QMQualityDefectProcess)lst[0];

                LB_Code.Text = qMQualityDefectProcess.Code.Trim();
                DL_QualityDefectNoticeCode.SelectedValue = qMQualityDefectProcess.QualityDefectNoticeCode.Trim();
                TB_DealRemark.Text = qMQualityDefectProcess.DealRemark.Trim();
                TB_CreatePer.Text = qMQualityDefectProcess.CreatePer.Trim();
                //if (qMQualityDefectProcess.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectProcess");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMQualityDefectProcessList();
    }
}