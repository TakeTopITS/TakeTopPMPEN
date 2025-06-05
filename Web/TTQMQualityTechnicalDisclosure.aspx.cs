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

public partial class TTQMQualityTechnicalDisclosure : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","质量技术交底", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMPurchasingContractName();

            LoadQMQualityTechnicalDisclosureList();
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

    protected void LoadQMQualityTechnicalDisclosureList()
    {
        string strHQL;

        strHQL = "Select * From T_QMQualityTechnicalDisclosure Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or ClarificationUnit like '%" + TextBox1.Text.Trim() + "%' or AcceptClariUnit like '%" + TextBox1.Text.Trim() + "%' " +
                "or CreatePer like '%" + TextBox1.Text.Trim() + "%' or DisclosureContent like '%" + TextBox1.Text.Trim() + "%' or Status like '%" + TextBox1.Text.Trim() + "%') ";
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

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityTechnicalDisclosure");

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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC") + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }
        if (IsQMQualityTechnicalDisclosure(string.Empty, DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGCGHTYCZCZSBJC") + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }

        QMQualityTechnicalDisclosureBLL qMQualityTechnicalDisclosureBLL = new QMQualityTechnicalDisclosureBLL();
        QMQualityTechnicalDisclosure qMQualityTechnicalDisclosure = new QMQualityTechnicalDisclosure();

        qMQualityTechnicalDisclosure.Code = GetQMQualityTechnicalDisclosureCode();
        LB_Code.Text = qMQualityTechnicalDisclosure.Code.Trim();
        qMQualityTechnicalDisclosure.AcceptClariUnit = TB_AcceptClariUnit.Text.Trim();
        qMQualityTechnicalDisclosure.DisclosureContent = TB_DisclosureContent.Text.Trim();
        qMQualityTechnicalDisclosure.CreateTime = DateTime.Now;
        qMQualityTechnicalDisclosure.ClarificationUnit = TB_ClarificationUnit.Text.Trim();
        qMQualityTechnicalDisclosure.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMQualityTechnicalDisclosure.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityTechnicalDisclosure.PurchasingContractName = GetQMPurchasingContractName(qMQualityTechnicalDisclosure.PurchasingContractCode.Trim());
        qMQualityTechnicalDisclosure.Status = DL_Status.SelectedValue.Trim();
        qMQualityTechnicalDisclosure.EnterCode = strUserCode.Trim();

        try
        {
            qMQualityTechnicalDisclosureBLL.AddQMQualityTechnicalDisclosure(qMQualityTechnicalDisclosure);

            LoadQMQualityTechnicalDisclosureList();

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
    /// 新增或更新时，检查采购合同是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMQualityTechnicalDisclosure(string strCode, string strPurchasingContractCode)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_QMQualityTechnicalDisclosure Where PurchasingContractCode='" + strPurchasingContractCode + "' ";
        }
        else
            strHQL = "Select Code From T_QMQualityTechnicalDisclosure Where PurchasingContractCode='" + strPurchasingContractCode + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityTechnicalDisclosure").Tables[0];
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
    /// 新增时，获取表T_QMQualityTechnicalDisclosure中最大编号 规则QMQTDX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMQualityTechnicalDisclosureCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMQualityTechnicalDisclosure Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityTechnicalDisclosure").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMQTD" + pa.ToString();
        }
        else
        {
            flag = "QMQTD1";
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC") + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }
        if (IsQMQualityTechnicalDisclosure(LB_Code.Text.Trim(), DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGCGHTYCZCZSBJC") + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }

        string strHQL = "From QMQualityTechnicalDisclosure as qMQualityTechnicalDisclosure where qMQualityTechnicalDisclosure.Code = '" + LB_Code.Text.Trim() + "'";
        QMQualityTechnicalDisclosureBLL qMQualityTechnicalDisclosureBLL = new QMQualityTechnicalDisclosureBLL();
        IList lst = qMQualityTechnicalDisclosureBLL.GetAllQMQualityTechnicalDisclosures(strHQL);

        QMQualityTechnicalDisclosure qMQualityTechnicalDisclosure = (QMQualityTechnicalDisclosure)lst[0];

        qMQualityTechnicalDisclosure.AcceptClariUnit = TB_AcceptClariUnit.Text.Trim();
        qMQualityTechnicalDisclosure.DisclosureContent = TB_DisclosureContent.Text.Trim();
        qMQualityTechnicalDisclosure.ClarificationUnit = TB_ClarificationUnit.Text.Trim();
        qMQualityTechnicalDisclosure.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMQualityTechnicalDisclosure.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityTechnicalDisclosure.PurchasingContractName = GetQMPurchasingContractName(qMQualityTechnicalDisclosure.PurchasingContractCode.Trim());
        qMQualityTechnicalDisclosure.Status = DL_Status.SelectedValue.Trim();

        try
        {
            qMQualityTechnicalDisclosureBLL.UpdateQMQualityTechnicalDisclosure(qMQualityTechnicalDisclosure, qMQualityTechnicalDisclosure.Code);

            LoadQMQualityTechnicalDisclosureList();

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

        strHQL = "Delete From T_QMQualityTechnicalDisclosure Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMQualityTechnicalDisclosureList();

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


                strHQL = "From QMQualityTechnicalDisclosure as qMQualityTechnicalDisclosure where qMQualityTechnicalDisclosure.Code = '" + strCode + "'";
                QMQualityTechnicalDisclosureBLL qMQualityTechnicalDisclosureBLL = new QMQualityTechnicalDisclosureBLL();
                lst = qMQualityTechnicalDisclosureBLL.GetAllQMQualityTechnicalDisclosures(strHQL);

                QMQualityTechnicalDisclosure qMQualityTechnicalDisclosure = (QMQualityTechnicalDisclosure)lst[0];

                LB_Code.Text = qMQualityTechnicalDisclosure.Code.Trim();
                TB_AcceptClariUnit.Text = qMQualityTechnicalDisclosure.AcceptClariUnit.Trim();
                DL_PurchasingContractCode.SelectedValue = qMQualityTechnicalDisclosure.PurchasingContractCode.Trim();
                TB_DisclosureContent.Text = qMQualityTechnicalDisclosure.DisclosureContent.Trim();
                TB_ClarificationUnit.Text = qMQualityTechnicalDisclosure.ClarificationUnit.Trim();
                TB_CreatePer.Text = qMQualityTechnicalDisclosure.CreatePer.Trim();
                DL_Status.SelectedValue = qMQualityTechnicalDisclosure.Status.Trim();
                //if (qMQualityTechnicalDisclosure.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityTechnicalDisclosure");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMQualityTechnicalDisclosureList();
    }

    protected void DL_PurchasingContractCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From QMPurchasingContract as qMPurchasingContract Where qMPurchasingContract.Code='" + DL_PurchasingContractCode.SelectedValue.Trim() + "' ";
        QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
        IList lst = qMPurchasingContractBLL.GetAllQMPurchasingContracts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMPurchasingContract proj = (QMPurchasingContract)lst[0];
            TB_AcceptClariUnit.Text = proj.ReceivingUnit.Trim();
            TB_ClarificationUnit.Text = proj.CompanyName.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }
}