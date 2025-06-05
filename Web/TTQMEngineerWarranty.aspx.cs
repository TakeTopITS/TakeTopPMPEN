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

public partial class TTQMEngineerWarranty : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","工程保修", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_WarrantyDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_NotificationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_CreatePer.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadQMPurchasingContractName();

            LoadQMEngineerWarrantyList();
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

    protected void LoadQMEngineerWarrantyList()
    {
        string strHQL;

        strHQL = "Select * From T_QMEngineerWarranty Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or BrieflyDescribe like '%" + TextBox1.Text.Trim() + "%' or CreatePer like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (PurchasingContractCode like '%" + TextBox2.Text.Trim() + "%' or PurchasingContractName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-WarrantyDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-WarrantyDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMEngineerWarranty");

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

        QMEngineerWarrantyBLL qMEngineerWarrantyBLL = new QMEngineerWarrantyBLL();
        QMEngineerWarranty qMEngineerWarranty = new QMEngineerWarranty();

        qMEngineerWarranty.Code = GetQMEngineerWarrantyCode();
        LB_Code.Text = qMEngineerWarranty.Code.Trim();
        qMEngineerWarranty.BrieflyDescribe = TB_BrieflyDescribe.Text.Trim();
        qMEngineerWarranty.WarrantyDate = DateTime.Parse(DLC_WarrantyDate.Text.Trim());
        qMEngineerWarranty.NotificationDate = DateTime.Parse(DLC_NotificationDate.Text.Trim());
        qMEngineerWarranty.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMEngineerWarranty.CreatePer = TB_CreatePer.Text.Trim();
        qMEngineerWarranty.PurchasingContractName = GetQMPurchasingContractName(qMEngineerWarranty.PurchasingContractCode.Trim());
        qMEngineerWarranty.CreateTime = DateTime.Now;
        qMEngineerWarranty.EnterCode = strUserCode.Trim();

        try
        {
            qMEngineerWarrantyBLL.AddQMEngineerWarranty(qMEngineerWarranty);

            LoadQMEngineerWarrantyList();

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
    /// 新增时，获取表T_QMEngineerWarranty中最大编号 规则QMEGWX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetQMEngineerWarrantyCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_QMEngineerWarranty Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_QMEngineerWarranty").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(5)) + 1;
            flag = "QMEGW" + pa.ToString();
        }
        else
        {
            flag = "QMEGW1";
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

        string strHQL = "From QMEngineerWarranty as qMEngineerWarranty where qMEngineerWarranty.Code = '" + LB_Code.Text.Trim() + "'";
        QMEngineerWarrantyBLL qMEngineerWarrantyBLL = new QMEngineerWarrantyBLL();
        IList lst = qMEngineerWarrantyBLL.GetAllQMEngineerWarrantys(strHQL);

        QMEngineerWarranty qMEngineerWarranty = (QMEngineerWarranty)lst[0];

        qMEngineerWarranty.BrieflyDescribe = TB_BrieflyDescribe.Text.Trim();
        qMEngineerWarranty.WarrantyDate = DateTime.Parse(DLC_WarrantyDate.Text.Trim());
        qMEngineerWarranty.NotificationDate = DateTime.Parse(DLC_NotificationDate.Text.Trim());
        qMEngineerWarranty.PurchasingContractCode = DL_PurchasingContractCode.SelectedValue.Trim();
        qMEngineerWarranty.CreatePer = TB_CreatePer.Text.Trim();
        qMEngineerWarranty.PurchasingContractName = GetQMPurchasingContractName(qMEngineerWarranty.PurchasingContractCode.Trim());

        try
        {
            qMEngineerWarrantyBLL.UpdateQMEngineerWarranty(qMEngineerWarranty, qMEngineerWarranty.Code);

            LoadQMEngineerWarrantyList();

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

        strHQL = "Delete From T_QMEngineerWarranty Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadQMEngineerWarrantyList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

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


                strHQL = "From QMEngineerWarranty as qMEngineerWarranty where qMEngineerWarranty.Code = '" + strCode + "'";
                QMEngineerWarrantyBLL qMEngineerWarrantyBLL = new QMEngineerWarrantyBLL();
                lst = qMEngineerWarrantyBLL.GetAllQMEngineerWarrantys(strHQL);

                QMEngineerWarranty qMEngineerWarranty = (QMEngineerWarranty)lst[0];

                LB_Code.Text = qMEngineerWarranty.Code.Trim();
                DL_PurchasingContractCode.SelectedValue = qMEngineerWarranty.PurchasingContractCode.Trim();
                TB_BrieflyDescribe.Text = qMEngineerWarranty.BrieflyDescribe.Trim();
                DLC_NotificationDate.Text = qMEngineerWarranty.NotificationDate.ToString("yyyy-MM-dd");
                TB_CreatePer.Text = qMEngineerWarranty.CreatePer.Trim();
                DLC_WarrantyDate.Text = qMEngineerWarranty.WarrantyDate.ToString("yyyy-MM-dd");
                //if (qMEngineerWarranty.EnterCode.Trim() == strUserCode.Trim())
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_QMEngineerWarranty");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadQMEngineerWarrantyList();
    }
}