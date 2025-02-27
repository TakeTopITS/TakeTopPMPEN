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

public partial class TTQMSatisfactionSurvey : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","满意度调查", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_EvaluationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMPurchasingContractName();

            LoadQMSatisfactionSurveyList();
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

    protected void LoadQMSatisfactionSurveyList()
    {
        string strHQL;

        strHQL = "Select * From T_QMSatisfactionSurvey Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Supplier like '%" + TextBox1.Text.Trim() + "%' or SatisfactionDegree like '%" + TextBox1.Text.Trim() + "%' or Remark like '%" + TextBox1.Text.Trim() + "%' or CreatePer like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (PurchasingContractCode like '%" + TextBox2.Text.Trim() + "%' or PurchasingContractName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-EvaluationDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-EvaluationDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMSatisfactionSurvey");

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
        if (DL_PurchasingContractCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }

        QMSatisfactionSurveyBLL qMSatisfactionSurveyBLL = new QMSatisfactionSurveyBLL();
        QMSatisfactionSurvey qMSatisfactionSurvey = new QMSatisfactionSurvey();

        qMSatisfactionSurvey.Code = GetQMSatisfactionSurveyCode();
        LB_Code.Text = qMSatisfactionSurvey.Code.Trim();
        qMSatisfactionSurvey.Remark = TB_Remark.Text.Trim();
        qMSatisfactionSurvey.EvaluationDate = DateTime.Parse(DLC_EvaluationDate.Text.Trim());
        qMSatisfactionSurvey.Supplier = TB_Supplier.Text.Trim();
        qMSatisfactionSurvey.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMSatisfactionSurvey.CreatePer = TB_CreatePer.Text.Trim();
        qMSatisfactionSurvey.PurchasingContractName = GetQMPurchasingContractName(qMSatisfactionSurvey.PurchasingContractCode.Trim());
        qMSatisfactionSurvey.SatisfactionDegree = DL_SatisfactionDegree.SelectedValue.Trim();
        qMSatisfactionSurvey.CreateTime = DateTime.Now;
        qMSatisfactionSurvey.EnterCode = strUserCode.Trim();

        try
        {
            qMSatisfactionSurveyBLL.AddQMSatisfactionSurvey(qMSatisfactionSurvey);

            LoadQMSatisfactionSurveyList();

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
    /// 新增时，获取表T_QMSatisfactionSurvey中最大编号 规则QMSFSX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMSatisfactionSurveyCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMSatisfactionSurvey Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMSatisfactionSurvey").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMSFS" + pa.ToString();
        }
        else
        {
            flag = "QMSFS1";
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

    protected void Update()
    {
        if (DL_PurchasingContractCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }

        string strHQL = "From QMSatisfactionSurvey as qMSatisfactionSurvey where qMSatisfactionSurvey.Code = '" + LB_Code.Text.Trim() + "'";
        QMSatisfactionSurveyBLL qMSatisfactionSurveyBLL = new QMSatisfactionSurveyBLL();
        IList lst = qMSatisfactionSurveyBLL.GetAllQMSatisfactionSurveys(strHQL);

        QMSatisfactionSurvey qMSatisfactionSurvey = (QMSatisfactionSurvey)lst[0];

        qMSatisfactionSurvey.Remark = TB_Remark.Text.Trim();
        qMSatisfactionSurvey.EvaluationDate = DateTime.Parse(DLC_EvaluationDate.Text.Trim());
        qMSatisfactionSurvey.Supplier = TB_Supplier.Text.Trim();
        qMSatisfactionSurvey.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMSatisfactionSurvey.CreatePer = TB_CreatePer.Text.Trim();
        qMSatisfactionSurvey.PurchasingContractName = GetQMPurchasingContractName(qMSatisfactionSurvey.PurchasingContractCode.Trim());
        qMSatisfactionSurvey.SatisfactionDegree = DL_SatisfactionDegree.SelectedValue.Trim();

        try
        {
            qMSatisfactionSurveyBLL.UpdateQMSatisfactionSurvey(qMSatisfactionSurvey, qMSatisfactionSurvey.Code);

            LoadQMSatisfactionSurveyList();

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

        strHQL = "Delete From T_QMSatisfactionSurvey Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMSatisfactionSurveyList();

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


                strHQL = "From QMSatisfactionSurvey as qMSatisfactionSurvey where qMSatisfactionSurvey.Code = '" + strCode + "'";
                QMSatisfactionSurveyBLL qMSatisfactionSurveyBLL = new QMSatisfactionSurveyBLL();
                lst = qMSatisfactionSurveyBLL.GetAllQMSatisfactionSurveys(strHQL);

                QMSatisfactionSurvey qMSatisfactionSurvey = (QMSatisfactionSurvey)lst[0];

                LB_Code.Text = qMSatisfactionSurvey.Code.Trim();
                DL_PurchasingContractCode.SelectedValue = qMSatisfactionSurvey.PurchasingContractCode.Trim();
                TB_Remark.Text = qMSatisfactionSurvey.Remark.Trim();
                TB_Supplier.Text = qMSatisfactionSurvey.Supplier.Trim();
                TB_CreatePer.Text = qMSatisfactionSurvey.CreatePer.Trim();
                DL_SatisfactionDegree.SelectedValue = qMSatisfactionSurvey.SatisfactionDegree.Trim();
                DLC_EvaluationDate.Text = qMSatisfactionSurvey.EvaluationDate.ToString("yyyy-MM-dd");
                //if (qMSatisfactionSurvey.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMSatisfactionSurvey");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMSatisfactionSurveyList();
    }

    protected void DL_PurchasingContractCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From QMPurchasingContract as qMPurchasingContract Where qMPurchasingContract.Code='" + DL_PurchasingContractCode.SelectedValue.Trim() + "' ";
        QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
        IList lst = qMPurchasingContractBLL.GetAllQMPurchasingContracts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMPurchasingContract proj = (QMPurchasingContract)lst[0];
            TB_Supplier.Text = proj.CompanyName.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }
}