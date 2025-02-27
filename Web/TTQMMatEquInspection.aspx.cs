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

public partial class TTQMMatEquInspection : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","采购验收", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_InspectionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_InspectionPer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMPurchasingContractName();

            LoadQMMatEquInspectionList();
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

    protected void LoadQMMatEquInspectionList()
    {
        string strHQL;

        strHQL = "Select * From T_QMMatEquInspection Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or Supplier like '%" + TextBox1.Text.Trim() + "%' or InspectionPer like '%" + TextBox1.Text.Trim() + "%' or InspectionResults like '%" + TextBox1.Text.Trim() + "%') ";
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

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMMatEquInspection");

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
        if (IsQMMatEquInspectionName(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGMCYCZCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }
        if (DL_PurchasingContractCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }
        if (IsQMMatEquInspectionPCCode(string.Empty, DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGCGHTYYSCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }

        QMMatEquInspectionBLL qMMatEquInspectionBLL = new QMMatEquInspectionBLL();
        QMMatEquInspection qMMatEquInspection = new QMMatEquInspection();

        qMMatEquInspection.Code = GetQMMatEquInspectionCode();
        LB_Code.Text = qMMatEquInspection.Code.Trim();
        qMMatEquInspection.Name = TB_Name.Text.Trim();
        qMMatEquInspection.ReceivingUnit = TB_ReceivingUnit.Text.Trim();
        qMMatEquInspection.InspectionResults = TB_InspectionResults.Text.Trim();
        qMMatEquInspection.InspectionDate = DateTime.Parse(DLC_InspectionDate.Text.Trim());
        qMMatEquInspection.TransportUnit = TB_TransportUnit.Text.Trim();
        qMMatEquInspection.Supplier = TB_Supplier.Text.Trim();
        qMMatEquInspection.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMMatEquInspection.InspectionPer = TB_InspectionPer.Text.Trim();
        qMMatEquInspection.PurchasingContractName = GetQMPurchasingContractName(qMMatEquInspection.PurchasingContractCode.Trim());
        qMMatEquInspection.Status = DL_Status.SelectedValue.Trim();
        qMMatEquInspection.EnterCode = strUserCode.Trim();

        try
        {
            qMMatEquInspectionBLL.AddQMMatEquInspection(qMMatEquInspection);

            LoadQMMatEquInspectionList();

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
    /// 新增或更新时，检查采购合同是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMMatEquInspectionPCCode(string strCode, string strPurchasingContractCode)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_QMMatEquInspection Where PurchasingContractCode='" + strPurchasingContractCode + "' ";
        }
        else
            strHQL = "Select Code From T_QMMatEquInspection Where PurchasingContractCode='" + strPurchasingContractCode + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMMatEquInspection").Tables[0];
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
    /// 新增或更新时，检查验收单名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMMatEquInspectionName(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_QMMatEquInspection Where Name='" + strName + "' ";
        }
        else
            strHQL = "Select Code From T_QMMatEquInspection Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMMatEquInspection").Tables[0];
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
    /// 新增时，获取表T_QMMatEquInspection中最大编号 规则QMMEIX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMMatEquInspectionCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMMatEquInspection Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMMatEquInspection").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMMEI" + pa.ToString();
        }
        else
        {
            flag = "QMMEI1";
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
        if (IsQMMatEquInspectionName(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGMCYCZCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }
        if (DL_PurchasingContractCode.SelectedValue.Trim() == "" || string.IsNullOrEmpty(DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCGHTWBXCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }
        if (IsQMMatEquInspectionPCCode(LB_Code.Text.Trim(), DL_PurchasingContractCode.SelectedValue.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGCGHTYYSCZSBJC").ToString().Trim() + "')", true);
            DL_PurchasingContractCode.Focus();
            return;
        }


        string strHQL = "From QMMatEquInspection as qMMatEquInspection where qMMatEquInspection.Code = '" + LB_Code.Text.Trim() + "'";
        QMMatEquInspectionBLL qMMatEquInspectionBLL = new QMMatEquInspectionBLL();
        IList lst = qMMatEquInspectionBLL.GetAllQMMatEquInspections(strHQL);

        QMMatEquInspection qMMatEquInspection = (QMMatEquInspection)lst[0];

        qMMatEquInspection.ReceivingUnit = TB_ReceivingUnit.Text.Trim();
        qMMatEquInspection.InspectionResults = TB_InspectionResults.Text.Trim();
        qMMatEquInspection.InspectionDate = DateTime.Parse(DLC_InspectionDate.Text.Trim());
        qMMatEquInspection.TransportUnit = TB_TransportUnit.Text.Trim();
        qMMatEquInspection.Supplier = TB_Supplier.Text.Trim();
        qMMatEquInspection.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMMatEquInspection.InspectionPer = TB_InspectionPer.Text.Trim();
        qMMatEquInspection.PurchasingContractName = GetQMPurchasingContractName(qMMatEquInspection.PurchasingContractCode.Trim());
        qMMatEquInspection.Status = DL_Status.SelectedValue.Trim();
        qMMatEquInspection.Name = TB_Name.Text.Trim();

        try
        {
            qMMatEquInspectionBLL.UpdateQMMatEquInspection(qMMatEquInspection, qMMatEquInspection.Code);

            LoadQMMatEquInspectionList();

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
        if (IsQMMatEquInspection(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGYSDYBDYWFSC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "Delete From T_QMMatEquInspection Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMMatEquInspectionList();

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

                strHQL = "From QMMatEquInspection as qMMatEquInspection where qMMatEquInspection.Code = '" + strCode + "'";
                QMMatEquInspectionBLL qMMatEquInspectionBLL = new QMMatEquInspectionBLL();
                lst = qMMatEquInspectionBLL.GetAllQMMatEquInspections(strHQL);

                QMMatEquInspection qMMatEquInspection = (QMMatEquInspection)lst[0];

                LB_Code.Text = qMMatEquInspection.Code.Trim();
                TB_ReceivingUnit.Text = qMMatEquInspection.ReceivingUnit.Trim();
                DL_PurchasingContractCode.SelectedValue = qMMatEquInspection.PurchasingContractCode.Trim();
                TB_InspectionResults.Text = qMMatEquInspection.InspectionResults.Trim();
                TB_TransportUnit.Text = qMMatEquInspection.TransportUnit.Trim();
                TB_Supplier.Text = qMMatEquInspection.Supplier.Trim();
                TB_InspectionPer.Text = qMMatEquInspection.InspectionPer.Trim();
                DL_Status.SelectedValue = qMMatEquInspection.Status.Trim();
                DLC_InspectionDate.Text = qMMatEquInspection.InspectionDate.ToString("yyyy-MM-dd");
                TB_Name.Text = qMMatEquInspection.Name.Trim();

                //if (qMMatEquInspection.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Delete.Visible = true;
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Enabled = true;
                //}
                //else
                //{
                //    BT_Update.Visible = false;
                //    BT_Delete.Visible = false;
                //}
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
    /// 删除时，检查验收单是否被调用，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsQMMatEquInspection(string strCode)
    {
        bool flag = true;
        string strHQL = "Select Code From T_QMQualityDefectNotice Where MatEquInsCode='" + strCode + "'";
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

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMMatEquInspection");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMMatEquInspectionList();
    }

    protected void DL_PurchasingContractCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "From QMPurchasingContract as qMPurchasingContract Where qMPurchasingContract.Code='" + DL_PurchasingContractCode.SelectedValue.Trim() + "' ";
        QMPurchasingContractBLL qMPurchasingContractBLL = new QMPurchasingContractBLL();
        IList lst = qMPurchasingContractBLL.GetAllQMPurchasingContracts(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            QMPurchasingContract proj = (QMPurchasingContract)lst[0];
            TB_ReceivingUnit.Text = proj.ReceivingUnit.Trim();
            TB_Supplier.Text = proj.CompanyName.Trim();
            TB_TransportUnit.Text = proj.TransportUnit.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }
}