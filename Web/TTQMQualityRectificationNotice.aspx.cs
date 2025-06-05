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

public partial class TTQMQualityRectificationNotice : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","质量隐患整改通知", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_InspectDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMQualityInspectionName();

            LoadQMQualityRectificationNoticeList();
        }
    }

    protected void LoadQMQualityInspectionName()
    {
        string strHQL;
        IList lst;
        strHQL = "From QMQualityInspection as qMQualityInspection Order By qMQualityInspection.Code Desc";
        QMQualityInspectionBLL qMQualityInspectionBLL = new QMQualityInspectionBLL();
        lst = qMQualityInspectionBLL.GetAllQMQualityInspections(strHQL);
        DL_QualityInspectionCode.DataSource = lst;
        DL_QualityInspectionCode.DataBind();
        DL_QualityInspectionCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    /// <summary>
    /// 合格，不合格
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetQMQualityInspectionStatus(string strCode)
    {
        string strHQL;
        IList lst;
        strHQL = "From QMQualityInspection as qMQualityInspection Where qMQualityInspection.Code ='" + strCode + "' ";
        QMQualityInspectionBLL qMQualityInspectionBLL = new QMQualityInspectionBLL();
        lst = qMQualityInspectionBLL.GetAllQMQualityInspections(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMQualityInspection qMQualityInspection = (QMQualityInspection)lst[0];
            return qMQualityInspection.Status.Trim();
        }
        else
            return "";
    }

    protected void LoadQMQualityRectificationNoticeList()
    {
        string strHQL;

        strHQL = "Select * From T_QMQualityRectificationNotice Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or ResponsibilityUnit like '%" + TextBox1.Text.Trim() + "%' " +
                "or CreatePer like '%" + TextBox1.Text.Trim() + "%' or Status like '%" + TextBox1.Text.Trim() + "%' or InformContent like '%" + TextBox1.Text.Trim() + "%' or InspectionMembers like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (QualityInspectionCode like '%" + TextBox2.Text.Trim() + "%' or QualityInspectionName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-InspectDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-InspectDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectificationNotice");

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCWBTXCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsQMQualityRectificationNotice(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_QualityInspectionCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_QualityInspectionCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZLJCDWBXCZSBJC") + "')", true);
            DL_QualityInspectionCode.Focus();
            return;
        }
        string status = GetQMQualityInspectionStatus(DL_QualityInspectionCode.SelectedValue.Trim());
        if (status.Trim() == "Qualified")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZLJCDHGWXZGCZSBJC") + "')", true);
            DL_QualityInspectionCode.Focus();
            return;
        }

        QMQualityRectificationNoticeBLL qMQualityRectificationNoticeBLL = new QMQualityRectificationNoticeBLL();
        QMQualityRectificationNotice qMQualityRectificationNotice = new QMQualityRectificationNotice();

        qMQualityRectificationNotice.Code = GetQMQualityRectificationNoticeCode();
        LB_Code.Text = qMQualityRectificationNotice.Code.Trim();
        qMQualityRectificationNotice.Name = TB_Name.Text.Trim();
        qMQualityRectificationNotice.InformContent = TB_InformContent.Text.Trim();
        qMQualityRectificationNotice.InspectDate = DateTime.Parse(DLC_InspectDate.Text.Trim());
        qMQualityRectificationNotice.QualityInspectionCode = DL_QualityInspectionCode.SelectedValue.Trim();
        qMQualityRectificationNotice.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityRectificationNotice.QualityInspectionName = GetQMQualityInspectionName(qMQualityRectificationNotice.QualityInspectionCode.Trim());
        qMQualityRectificationNotice.Status = DL_Status.SelectedValue.Trim();
        qMQualityRectificationNotice.CreateTime = DateTime.Now;
        qMQualityRectificationNotice.InspectionMembers = TB_InspectionMembers.Text.Trim();
        qMQualityRectificationNotice.ResponsibilityUnit = TB_ResponsibilityUnit.Text.Trim();
        qMQualityRectificationNotice.EnterCode = strUserCode.Trim();

        try
        {
            qMQualityRectificationNoticeBLL.AddQMQualityRectificationNotice(qMQualityRectificationNotice);

            LoadQMQualityRectificationNoticeList();

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

    /// <summary>
    /// 新增或更新时，检查名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMQualityRectificationNotice(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_QMQualityRectificationNotice Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select Code From T_QMQualityRectificationNotice Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectificationNotice").Tables[0];
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
    /// 新增时，获取表T_QMQualityRectificationNotice中最大编号 规则QMQRNX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMQualityRectificationNoticeCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMQualityRectificationNotice Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectificationNotice").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMQRN" + pa.ToString();
        }
        else
        {
            flag = "QMQRN1";
        }
        return flag;
    }

    protected string GetQMQualityInspectionName(string strPCCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From QMQualityInspection as qMQualityInspection Where qMQualityInspection.Code='" + strPCCode + "' ";
        QMQualityInspectionBLL qMQualityInspectionBLL = new QMQualityInspectionBLL();
        lst = qMQualityInspectionBLL.GetAllQMQualityInspections(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMQualityInspection proj = (QMQualityInspection)lst[0];
            return proj.Name.Trim();
        }
        else
            return "";
    }

    protected void Update()
    {
        if (TB_Name.Text.Trim() == "" || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCWBTXCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsQMQualityRectificationNotice(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_QualityInspectionCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_QualityInspectionCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZLJCDWBXCZSBJC") + "')", true);
            DL_QualityInspectionCode.Focus();
            return;
        }
        string status = GetQMQualityInspectionStatus(DL_QualityInspectionCode.SelectedValue.Trim());
        if (status.Trim() == "Qualified")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZLJCDHGWXZGCZSBJC") + "')", true);
            DL_QualityInspectionCode.Focus();
            return;
        }

        string strHQL = "From QMQualityRectificationNotice as qMQualityRectificationNotice where qMQualityRectificationNotice.Code = '" + LB_Code.Text.Trim() + "'";
        QMQualityRectificationNoticeBLL qMQualityRectificationNoticeBLL = new QMQualityRectificationNoticeBLL();
        IList lst = qMQualityRectificationNoticeBLL.GetAllQMQualityRectificationNotices(strHQL);

        QMQualityRectificationNotice qMQualityRectificationNotice = (QMQualityRectificationNotice)lst[0];

        qMQualityRectificationNotice.Name = TB_Name.Text.Trim();
        qMQualityRectificationNotice.InformContent = TB_InformContent.Text.Trim();
        qMQualityRectificationNotice.InspectDate = DateTime.Parse(DLC_InspectDate.Text.Trim());
        qMQualityRectificationNotice.QualityInspectionCode = DL_QualityInspectionCode.SelectedValue.Trim();
        qMQualityRectificationNotice.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityRectificationNotice.QualityInspectionName = GetQMQualityInspectionName(qMQualityRectificationNotice.QualityInspectionCode.Trim());
        qMQualityRectificationNotice.Status = DL_Status.SelectedValue.Trim();
        qMQualityRectificationNotice.InspectionMembers = TB_InspectionMembers.Text.Trim();
        qMQualityRectificationNotice.ResponsibilityUnit = TB_ResponsibilityUnit.Text.Trim();

        try
        {
            qMQualityRectificationNoticeBLL.UpdateQMQualityRectificationNotice(qMQualityRectificationNotice, qMQualityRectificationNotice.Code);

            LoadQMQualityRectificationNoticeList();

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
        if (IsQMQualityRectificationNotice(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZLYHZGTZDYBDYWFSC") + "')", true);
            return;
        }

        strHQL = "Delete From T_QMQualityRectificationNotice Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMQualityRectificationNoticeList();

            //BT_Delete.Visible = false;
            //BT_Update.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
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


                strHQL = "From QMQualityRectificationNotice as qMQualityRectificationNotice where qMQualityRectificationNotice.Code = '" + strCode + "'";
                QMQualityRectificationNoticeBLL qMQualityRectificationNoticeBLL = new QMQualityRectificationNoticeBLL();
                lst = qMQualityRectificationNoticeBLL.GetAllQMQualityRectificationNotices(strHQL);

                QMQualityRectificationNotice qMQualityRectificationNotice = (QMQualityRectificationNotice)lst[0];

                LB_Code.Text = qMQualityRectificationNotice.Code.Trim();
                DL_QualityInspectionCode.SelectedValue = qMQualityRectificationNotice.QualityInspectionCode.Trim();
                TB_InformContent.Text = qMQualityRectificationNotice.InformContent.Trim();
                TB_CreatePer.Text = qMQualityRectificationNotice.CreatePer.Trim();
                DL_Status.SelectedValue = qMQualityRectificationNotice.Status.Trim();
                DLC_InspectDate.Text = qMQualityRectificationNotice.InspectDate.ToString("yyyy-MM-dd");
                TB_Name.Text = qMQualityRectificationNotice.Name.Trim();
                TB_ResponsibilityUnit.Text = qMQualityRectificationNotice.ResponsibilityUnit.Trim();
                //TB_InspectionMembers.Text = qMQualityRectificationNotice.InspectionMembers.Trim();
                //if (qMQualityRectificationNotice.EnterCode.Trim() == strUserCode.Trim())
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
    /// 删除时，检查质量隐患整改通知单是否被调用，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMQualityRectificationNotice(string strCode)
    {
        bool flag = true;
        string strHQL = "Select Code From T_QMQualityRectification Where RectificationNoticeCode='" + strCode + "'";
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

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectificationNotice");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMQualityRectificationNoticeList();
    }

    protected void DL_QualityInspectionCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From QMQualityInspection as qMQualityInspection Where qMQualityInspection.Code ='" + DL_QualityInspectionCode.SelectedValue.Trim() + "' ";
        QMQualityInspectionBLL qMQualityInspectionBLL = new QMQualityInspectionBLL();
        IList lst = qMQualityInspectionBLL.GetAllQMQualityInspections(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMQualityInspection qMQualityInspection = (QMQualityInspection)lst[0];

            strHQL = "From QMPurchasingContract as qMPurchasingContract Where qMPurchasingContract.Code ='" + qMQualityInspection.PurchasingContractCode.Trim() + "' ";
            QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
            lst = qMPurchasingContractBLL.GetAllQMPurchasingContracts(strHQL);
            if (lst.Count > 0 && lst != null)
            {
                QMPurchasingContract qMPurchasingContract = (QMPurchasingContract)lst[0];
                TB_ResponsibilityUnit.Text = qMPurchasingContract.CompanyName.Trim();
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }
}