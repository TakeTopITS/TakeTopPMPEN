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

public partial class TTQMOverAllEvaluation : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","采购业务评价", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMPurchasingContractName();

            LoadQMOverAllEvaluationList();
        }
    }

    protected void LoadQMPurchasingContractName()
    {
        string strHQL;
        IList lst;
        strHQL = "From QMPurchasingContract as qMPurchasingContract Order By qMPurchasingContract.Code Desc";
        QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
        lst = qMPurchasingContractBLL.GetAllQMPurchasingContracts(strHQL);
        DL_PurchasingContractCode.DataSource = lst;
        DL_PurchasingContractCode.DataBind();
        DL_PurchasingContractCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadQMOverAllEvaluationList()
    {
        string strHQL;

        strHQL = "Select * From T_QMOverAllEvaluation Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or OverAllEvaluation like '%" + TextBox1.Text.Trim() + "%' or CreatePer like '%" + TextBox1.Text.Trim() + "%' or Status like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (PurchasingContractCode like '%" + TextBox2.Text.Trim() + "%' or PurchasingContractName like '%" + TextBox2.Text.Trim() + "%') ";
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

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMOverAllEvaluation");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (DL_PurchasingContractCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC").ToString().Trim()+"')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }
        if (IsQMOverAllEvaluation(string.Empty, DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGCGHTYPJCZSBJC").ToString().Trim()+"')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }

        QMOverAllEvaluationBLL qMOverAllEvaluationBLL = new QMOverAllEvaluationBLL();
        QMOverAllEvaluation qMOverAllEvaluation = new QMOverAllEvaluation();

        qMOverAllEvaluation.Code = GetQMOverAllEvaluationCode();
        LB_Code.Text = qMOverAllEvaluation.Code.Trim();
        qMOverAllEvaluation.OverAllEvaluation = TB_OverAllEvaluation.Text.Trim();
        qMOverAllEvaluation.CreateTime = DateTime.Now;
        qMOverAllEvaluation.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMOverAllEvaluation.CreatePer = TB_CreatePer.Text.Trim();
        qMOverAllEvaluation.PurchasingContractName = GetQMPurchasingContractName(qMOverAllEvaluation.PurchasingContractCode.Trim());
        qMOverAllEvaluation.Status = DL_Status.SelectedValue.Trim();
        qMOverAllEvaluation.EnterCode = strUserCode.Trim();

        try
        {
            qMOverAllEvaluationBLL.AddQMOverAllEvaluation(qMOverAllEvaluation);

            LoadQMOverAllEvaluationList();

            BT_Delete.Visible = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    /// <summary>
    /// 新增或更新时，检查采购合同是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMOverAllEvaluation(string strCode, string strPurchasingContractCode)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_QMOverAllEvaluation Where PurchasingContractCode='" + strPurchasingContractCode + "' ";
        }
        else
            strHQL = "Select Code From T_QMOverAllEvaluation Where PurchasingContractCode='" + strPurchasingContractCode + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMOverAllEvaluation").Tables[0];
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
    /// 新增时，获取表T_QMOverAllEvaluation中最大编号 规则QMOAEX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMOverAllEvaluationCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMOverAllEvaluation Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMOverAllEvaluation").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMOAE" + pa.ToString();
        }
        else
        {
            flag = "QMOAE1";
        }
        return flag;
    }

    protected string GetQMPurchasingContractName(string strPCCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From QMPurchasingContract as qMPurchasingContract Where qMPurchasingContract.Code='" + strPCCode + "' ";
        QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
        lst = qMPurchasingContractBLL.GetAllQMPurchasingContracts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMPurchasingContract proj = (QMPurchasingContract)lst[0];
            return proj.Name.Trim();
        }
        else
            return "";
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (DL_PurchasingContractCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC").ToString().Trim()+"')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }
        if (IsQMOverAllEvaluation(LB_Code.Text.Trim(), DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGCGHTYPJCZSBJC").ToString().Trim()+"')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }


        string strHQL = "From QMOverAllEvaluation as qMOverAllEvaluation where qMOverAllEvaluation.Code = '" + LB_Code.Text.Trim() + "'";
        QMOverAllEvaluationBLL qMOverAllEvaluationBLL = new QMOverAllEvaluationBLL();
        IList lst = qMOverAllEvaluationBLL.GetAllQMOverAllEvaluations(strHQL);

        QMOverAllEvaluation qMOverAllEvaluation = (QMOverAllEvaluation)lst[0];

        qMOverAllEvaluation.OverAllEvaluation = TB_OverAllEvaluation.Text.Trim();
        qMOverAllEvaluation.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMOverAllEvaluation.CreatePer = TB_CreatePer.Text.Trim();
        qMOverAllEvaluation.PurchasingContractName = GetQMPurchasingContractName(qMOverAllEvaluation.PurchasingContractCode.Trim());
        qMOverAllEvaluation.Status = DL_Status.SelectedValue.Trim();

        try
        {
            qMOverAllEvaluationBLL.UpdateQMOverAllEvaluation(qMOverAllEvaluation, qMOverAllEvaluation.Code);

            LoadQMOverAllEvaluationList();

            BT_Delete.Visible = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strCode = LB_Code.Text.Trim();
        
        strHQL = "Delete From T_QMOverAllEvaluation Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMOverAllEvaluationList();

            BT_Delete.Visible = false;
            BT_Update.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
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

            strHQL = "From QMOverAllEvaluation as qMOverAllEvaluation where qMOverAllEvaluation.Code = '" + strId + "'";
            QMOverAllEvaluationBLL qMOverAllEvaluationBLL = new QMOverAllEvaluationBLL();
            lst = qMOverAllEvaluationBLL.GetAllQMOverAllEvaluations(strHQL);

            QMOverAllEvaluation qMOverAllEvaluation = (QMOverAllEvaluation)lst[0];

            LB_Code.Text = qMOverAllEvaluation.Code.Trim();
            DL_PurchasingContractCode.SelectedValue = qMOverAllEvaluation.PurchasingContractCode.Trim();
            TB_OverAllEvaluation.Text = qMOverAllEvaluation.OverAllEvaluation.Trim();
            TB_CreatePer.Text = qMOverAllEvaluation.CreatePer.Trim();
            DL_Status.SelectedValue = qMOverAllEvaluation.Status.Trim();
            if (qMOverAllEvaluation.EnterCode.Trim() == strUserCode.Trim())
            {
                BT_Delete.Visible = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_Delete.Visible = false;
                BT_Update.Visible = false;
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMOverAllEvaluation");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMOverAllEvaluationList();
    }
}