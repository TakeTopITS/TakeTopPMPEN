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

public partial class TTQMQualityInspection : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","质量检查", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_InspectionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMPurchasingContractName();

            LoadQMQualityInspectionList();
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

    protected void LoadQMQualityInspectionList()
    {
        string strHQL;

        strHQL = "Select * From T_QMQualityInspection Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or ExaminationContent like '%" + TextBox1.Text.Trim() + "%' or CreatePer like '%" + TextBox1.Text.Trim() + "%' or Status like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (PurchasingContractCode like '%" + TextBox2.Text.Trim() + "%' or PurchasingContractName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-InspectionDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-InspectionDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityInspection");

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
        if (IsQMQualityInspection(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_PurchasingContractCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }

        QMQualityInspectionBLL qMQualityInspectionBLL = new QMQualityInspectionBLL();
        QMQualityInspection qMQualityInspection = new QMQualityInspection();

        qMQualityInspection.Code = GetQMQualityInspectionCode();
        LB_Code.Text = qMQualityInspection.Code.Trim();
        qMQualityInspection.Name = TB_Name.Text.Trim();
        qMQualityInspection.ExaminationContent = TB_ExaminationContent.Text.Trim();
        qMQualityInspection.InspectionDate = DateTime.Parse(DLC_InspectionDate.Text.Trim());
        qMQualityInspection.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMQualityInspection.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityInspection.PurchasingContractName = GetQMPurchasingContractName(qMQualityInspection.PurchasingContractCode.Trim());
        qMQualityInspection.Status = DL_Status.SelectedValue.Trim();
        qMQualityInspection.CreateTime = DateTime.Now;
        qMQualityInspection.EnterCode = strUserCode.Trim();

        try
        {
            qMQualityInspectionBLL.AddQMQualityInspection(qMQualityInspection);

            LoadQMQualityInspectionList();

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
    /// 新增或更新时，检查名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMQualityInspection(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_QMQualityInspection Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select Code From T_QMQualityInspection Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityInspection").Tables[0];
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
    /// 新增时，获取表T_QMQualityInspection中最大编号 规则QMQINX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMQualityInspectionCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMQualityInspection Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityInspection").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMQIN" + pa.ToString();
        }
        else
        {
            flag = "QMQIN1";
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
        if (TB_Name.Text.Trim() == "" || string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCWBTXCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsQMQualityInspection(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGMCYCZCZSBJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (DL_PurchasingContractCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }

        string strHQL = "From QMQualityInspection as qMQualityInspection where qMQualityInspection.Code = '" + LB_Code.Text.Trim() + "'";
        QMQualityInspectionBLL qMQualityInspectionBLL = new QMQualityInspectionBLL();
        IList lst = qMQualityInspectionBLL.GetAllQMQualityInspections(strHQL);

        QMQualityInspection qMQualityInspection = (QMQualityInspection)lst[0];

        qMQualityInspection.Name = TB_Name.Text.Trim();
        qMQualityInspection.ExaminationContent = TB_ExaminationContent.Text.Trim();
        qMQualityInspection.InspectionDate = DateTime.Parse(DLC_InspectionDate.Text.Trim());
        qMQualityInspection.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMQualityInspection.CreatePer = TB_CreatePer.Text.Trim();
        qMQualityInspection.PurchasingContractName = GetQMPurchasingContractName(qMQualityInspection.PurchasingContractCode.Trim());
        qMQualityInspection.Status = DL_Status.SelectedValue.Trim();

        try
        {
            qMQualityInspectionBLL.UpdateQMQualityInspection(qMQualityInspection, qMQualityInspection.Code);

            LoadQMQualityInspectionList();

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
        if (IsQMQualityInspection(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGZLJCDYBDYWFSC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Delete From T_QMQualityInspection Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMQualityInspectionList();

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

                strHQL = "From QMQualityInspection as qMQualityInspection where qMQualityInspection.Code = '" + strCode + "'";
                QMQualityInspectionBLL qMQualityInspectionBLL = new QMQualityInspectionBLL();
                lst = qMQualityInspectionBLL.GetAllQMQualityInspections(strHQL);

                QMQualityInspection qMQualityInspection = (QMQualityInspection)lst[0];

                LB_Code.Text = qMQualityInspection.Code.Trim();
                DL_PurchasingContractCode.SelectedValue = qMQualityInspection.PurchasingContractCode.Trim();
                TB_ExaminationContent.Text = qMQualityInspection.ExaminationContent.Trim();
                TB_CreatePer.Text = qMQualityInspection.CreatePer.Trim();
                DL_Status.SelectedValue = qMQualityInspection.Status.Trim();
                DLC_InspectionDate.Text = qMQualityInspection.InspectionDate.ToString("yyyy-MM-dd");
                TB_Name.Text = qMQualityInspection.Name.Trim();
                //if (qMQualityInspection.EnterCode.Trim() == strUserCode.Trim())
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
    /// 删除时，检查质量检查单是否被调用，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMQualityInspection(string strCode)
    {
        bool flag = true;
        bool flag1 = true;
        string strHQL = "Select Code From T_QMQualityRectificationNotice Where QualityInspectionCode='" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityRectificationNotice").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = true;
        }
        else
        {
            flag = false;
        }
        strHQL = "Select Code From T_QMRewardPunishment Where QualityInspectionCode='" + strCode + "'";
        dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMRewardPunishment").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag1 = true;
        }
        else
        {
            flag1 = false;
        }

        if (flag || flag1)
            return true;
        else
            return false;
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMQualityInspection");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMQualityInspectionList();
    }
}