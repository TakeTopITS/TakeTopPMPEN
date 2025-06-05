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

public partial class TTQMRewardPunishment : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","质量奖惩通知", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMQualityInspectionName();

            LoadQMRewardPunishmentList();
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

    protected void LoadQMRewardPunishmentList()
    {
        string strHQL;

        strHQL = "Select * From T_QMRewardPunishment Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or PenUnit like '%" + TextBox1.Text.Trim() + "%' or RewardsPunishment like '%" + TextBox1.Text.Trim() + "%' " +
                "or CreatePer like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (QualityInspectionCode like '%" + TextBox2.Text.Trim() + "%' or QualityInspectionName like '%" + TextBox2.Text.Trim() + "%') ";
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

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMRewardPunishment");

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
        if (DL_QualityInspectionCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_QualityInspectionCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZLJCDWBXCZSBJC") + "')", true);
            DL_QualityInspectionCode.Focus();
            return;
        }

        QMRewardPunishmentBLL qMRewardPunishmentBLL = new QMRewardPunishmentBLL();
        QMRewardPunishment qMRewardPunishment = new QMRewardPunishment();

        qMRewardPunishment.Code = GetQMRewardPunishmentCode();
        LB_Code.Text = qMRewardPunishment.Code.Trim();
        qMRewardPunishment.QualityInspectionCode = DL_QualityInspectionCode.SelectedValue.Trim();
        qMRewardPunishment.CreatePer = TB_CreatePer.Text.Trim();
        qMRewardPunishment.QualityInspectionName = GetQMQualityInspectionName(qMRewardPunishment.QualityInspectionCode.Trim());
        qMRewardPunishment.CreateTime = DateTime.Now;
        qMRewardPunishment.PenUnit = TB_PenUnit.Text.Trim();
        qMRewardPunishment.RewardsPunishment = TB_RewardsPunishment.Text.Trim();
        qMRewardPunishment.EnterCode = strUserCode.Trim();
        try
        {
            qMRewardPunishmentBLL.AddQMRewardPunishment(qMRewardPunishment);

            LoadQMRewardPunishmentList();

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
    /// 新增时，获取表T_QMRewardPunishment中最大编号 规则QMRPTX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMRewardPunishmentCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMRewardPunishment Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMRewardPunishment").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMRPT" + pa.ToString();
        }
        else
        {
            flag = "QMRPT1";
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
        if (DL_QualityInspectionCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_QualityInspectionCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZLJCDWBXCZSBJC") + "')", true);
            DL_QualityInspectionCode.Focus();
            return;
        }

        string strHQL = "From QMRewardPunishment as qMRewardPunishment where qMRewardPunishment.Code = '" + LB_Code.Text.Trim() + "'";
        QMRewardPunishmentBLL qMRewardPunishmentBLL = new QMRewardPunishmentBLL();
        IList lst = qMRewardPunishmentBLL.GetAllQMRewardPunishments(strHQL);

        QMRewardPunishment qMRewardPunishment = (QMRewardPunishment)lst[0];

        qMRewardPunishment.QualityInspectionCode = DL_QualityInspectionCode.SelectedValue.Trim();
        qMRewardPunishment.CreatePer = TB_CreatePer.Text.Trim();
        qMRewardPunishment.QualityInspectionName = GetQMQualityInspectionName(qMRewardPunishment.QualityInspectionCode.Trim());
        qMRewardPunishment.PenUnit = TB_PenUnit.Text.Trim();
        qMRewardPunishment.RewardsPunishment = TB_RewardsPunishment.Text.Trim();

        try
        {
            qMRewardPunishmentBLL.UpdateQMRewardPunishment(qMRewardPunishment, qMRewardPunishment.Code);

            LoadQMRewardPunishmentList();

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

        strHQL = "Delete From T_QMRewardPunishment Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMRewardPunishmentList();

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

                strHQL = "From QMRewardPunishment as qMRewardPunishment where qMRewardPunishment.Code = '" + strCode + "'";
                QMRewardPunishmentBLL qMRewardPunishmentBLL = new QMRewardPunishmentBLL();
                lst = qMRewardPunishmentBLL.GetAllQMRewardPunishments(strHQL);

                QMRewardPunishment qMRewardPunishment = (QMRewardPunishment)lst[0];

                LB_Code.Text = qMRewardPunishment.Code.Trim();
                DL_QualityInspectionCode.SelectedValue = qMRewardPunishment.QualityInspectionCode.Trim();
                TB_RewardsPunishment.Text = qMRewardPunishment.RewardsPunishment.Trim();
                TB_CreatePer.Text = qMRewardPunishment.CreatePer.Trim();
                TB_PenUnit.Text = qMRewardPunishment.PenUnit.Trim();
                //if (qMRewardPunishment.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMRewardPunishment");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMRewardPunishmentList();
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
                TB_PenUnit.Text = qMPurchasingContract.CompanyName.Trim();
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }
}