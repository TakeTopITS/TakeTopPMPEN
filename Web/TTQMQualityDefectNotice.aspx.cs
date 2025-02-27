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

public partial class TTQMQualityDefectNotice : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","质量缺陷通知", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMMatEquInspectionName();

            LoadQMQualityDefectNoticeList();
        }
    }

    protected void LoadQMMatEquInspectionName()
    {
        string strHQL;
        IList lst;
        strHQL = "From QMMatEquInspection as qMMatEquInspection Order By qMMatEquInspection.Code Desc";
        QMMatEquInspectionBLL qMMatEquInspectionBLL = new QMMatEquInspectionBLL();
        lst = qMMatEquInspectionBLL.GetAllQMMatEquInspections(strHQL);
        DL_MatEquInsCode.DataSource = lst;
        DL_MatEquInsCode.DataBind();
        DL_MatEquInsCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    /// <summary>
    /// 合格，不合格
    /// </summary>
    /// <param name="strQMMatEquInspectionId"></param>
    /// <returns></returns>
    protected string GetQMMatEquInspectionStatus(string strQMMatEquInspectionId)
    {
        string strHQL = "From QMMatEquInspection as qMMatEquInspection Where qMMatEquInspection.Code='" + strQMMatEquInspectionId.Trim() + "' ";
        QMMatEquInspectionBLL qMMatEquInspectionBLL = new QMMatEquInspectionBLL();
        IList lst = qMMatEquInspectionBLL.GetAllQMMatEquInspections(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMMatEquInspection qMMatEquInspection = (QMMatEquInspection)lst[0];
            return qMMatEquInspection.Status.Trim();
        }
        else
            return "";
    }

    protected void LoadQMQualityDefectNoticeList()
    {
        string strHQL;

        strHQL = "Select * From T_QMQualityDefectNotice Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or Supplier like '%" + TextBox1.Text.Trim() + "%' or DefectDescription like '%" + TextBox1.Text.Trim() + "%' or CreatePer like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (MatEquInsCode like '%" + TextBox2.Text.Trim() + "%' or MatEquInsName like '%" + TextBox2.Text.Trim() + "%') ";
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

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectNotice");

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
        if (TB_Name.Text.Trim() == "" || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCWBTXCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsQMQualityDefectNotice(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_MatEquInsCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_MatEquInsCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGYSDWBXCZSBJC").ToString().Trim() + "')", true);
            DL_MatEquInsCode.Focus();
            return;
        }
        string status = GetQMMatEquInspectionStatus(DL_MatEquInsCode.SelectedValue.Trim());
        if (status.Trim() == "Qualified")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGCGYSDYSHGWXTZJC").ToString().Trim() + "')", true);
            DL_MatEquInsCode.Focus();
            return;
        }

        QMQualityDefectNoticeBLL qMQualityDefectNoticeBLL = new QMQualityDefectNoticeBLL();
        QMQualityDefectNotice qMQualityDefectNotice = new QMQualityDefectNotice();

        qMQualityDefectNotice.Code = GetQMQualityDefectNoticeCode();
        LB_Code.Text = qMQualityDefectNotice.Code.Trim();
        qMQualityDefectNotice.Name = TB_Name.Text.Trim();
        qMQualityDefectNotice.DefectDescription = TB_DefectDescription.Text.Trim();
        qMQualityDefectNotice.Supplier = TB_Supplier.Text.Trim();
        qMQualityDefectNotice.MatEquInsCode = DL_MatEquInsCode.SelectedValue.Trim();
        qMQualityDefectNotice.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityDefectNotice.MatEquInsName = GetQMMatEquInspectionName(qMQualityDefectNotice.MatEquInsCode.Trim());
        qMQualityDefectNotice.Status = DL_Status.SelectedValue.Trim();
        qMQualityDefectNotice.CreateTime = DateTime.Now;
        qMQualityDefectNotice.EnterCode = strUserCode.Trim();

        try
        {
            qMQualityDefectNoticeBLL.AddQMQualityDefectNotice(qMQualityDefectNotice);

            LoadQMQualityDefectNoticeList();

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
    /// 新增或更新时，检查缺陷通知名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMQualityDefectNotice(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_QMQualityDefectNotice Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select Code From T_QMQualityDefectNotice Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectNotice").Tables[0];
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
    /// 新增时，获取表T_QMQualityDefectNotice中最大编号 规则QMQDNX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMQualityDefectNoticeCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMQualityDefectNotice Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectNotice").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMQDN" + pa.ToString();
        }
        else
        {
            flag = "QMQDN1";
        }
        return flag;
    }

    protected string GetQMMatEquInspectionName(string strPCCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From QMMatEquInspection as qMMatEquInspection Where qMMatEquInspection.Code='" + strPCCode + "' ";
        QMMatEquInspectionBLL qMMatEquInspectionBLL = new QMMatEquInspectionBLL();
        lst = qMMatEquInspectionBLL.GetAllQMMatEquInspections(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMMatEquInspection proj = (QMMatEquInspection)lst[0];
            return proj.Name.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        if (TB_Name.Text.Trim() == "" || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCWBTXCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsQMQualityDefectNotice(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_MatEquInsCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_MatEquInsCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGYSDWBXCZSBJC").ToString().Trim() + "')", true);
            DL_MatEquInsCode.Focus();
            return;
        }
        string status = GetQMMatEquInspectionStatus(DL_MatEquInsCode.SelectedValue.Trim());
        if (status.Trim() == "Qualified")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGCGYSDYSHGWXTZJC").ToString().Trim() + "')", true);
            DL_MatEquInsCode.Focus();
            return;
        }

        string strHQL = "From QMQualityDefectNotice as qMQualityDefectNotice where qMQualityDefectNotice.Code = '" + LB_Code.Text.Trim() + "'";
        QMQualityDefectNoticeBLL qMQualityDefectNoticeBLL = new QMQualityDefectNoticeBLL();
        IList lst = qMQualityDefectNoticeBLL.GetAllQMQualityDefectNotices(strHQL);

        QMQualityDefectNotice qMQualityDefectNotice = (QMQualityDefectNotice)lst[0];

        qMQualityDefectNotice.Name = TB_Name.Text.Trim();
        qMQualityDefectNotice.DefectDescription = TB_DefectDescription.Text.Trim();
        qMQualityDefectNotice.Supplier = TB_Supplier.Text.Trim();
        qMQualityDefectNotice.MatEquInsCode = DL_MatEquInsCode.SelectedValue.Trim();
        qMQualityDefectNotice.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityDefectNotice.MatEquInsName = GetQMMatEquInspectionName(qMQualityDefectNotice.MatEquInsCode.Trim());
        qMQualityDefectNotice.Status = DL_Status.SelectedValue.Trim();

        try
        {
            qMQualityDefectNoticeBLL.UpdateQMQualityDefectNotice(qMQualityDefectNotice, qMQualityDefectNotice.Code);

            LoadQMQualityDefectNoticeList();

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
        if (IsQMQualityDefectNotice(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGXTZDYBDYWFSC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Delete From T_QMQualityDefectNotice Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMQualityDefectNoticeList();

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


                strHQL = "From QMQualityDefectNotice as qMQualityDefectNotice where qMQualityDefectNotice.Code = '" + strCode + "'";
                QMQualityDefectNoticeBLL qMQualityDefectNoticeBLL = new QMQualityDefectNoticeBLL();
                lst = qMQualityDefectNoticeBLL.GetAllQMQualityDefectNotices(strHQL);

                QMQualityDefectNotice qMQualityDefectNotice = (QMQualityDefectNotice)lst[0];

                LB_Code.Text = qMQualityDefectNotice.Code.Trim();
                DL_MatEquInsCode.SelectedValue = qMQualityDefectNotice.MatEquInsCode.Trim();
                TB_DefectDescription.Text = qMQualityDefectNotice.DefectDescription.Trim();
                TB_Supplier.Text = qMQualityDefectNotice.Supplier.Trim();
                TB_CreatePer.Text = qMQualityDefectNotice.CreatePer.Trim();
                DL_Status.SelectedValue = qMQualityDefectNotice.Status.Trim();
                TB_Name.Text = qMQualityDefectNotice.Name.Trim();
                //if (qMQualityDefectNotice.EnterCode.Trim() == strUserCode.Trim())
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

    /// <summary>
    /// 删除时，检查缺陷通知单是否被调用，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMQualityDefectNotice(string strCode)
    {
        bool flag = true;
        string strHQL = "Select Code From T_QMQualityDefectProcess Where QualityDefectNoticeCode='" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectProcess").Tables[0];
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

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityDefectNotice");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMQualityDefectNoticeList();
    }

    protected void DL_MatEquInsCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From QMMatEquInspection as qMMatEquInspection Where qMMatEquInspection.Code='" + DL_MatEquInsCode.SelectedValue.Trim() + "' ";
        QMMatEquInspectionBLL qMMatEquInspectionBLL = new QMMatEquInspectionBLL();
        IList lst = qMMatEquInspectionBLL.GetAllQMMatEquInspections(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMMatEquInspection proj = (QMMatEquInspection)lst[0];
            TB_Supplier.Text = proj.Supplier.Trim();
        }
        
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }
}