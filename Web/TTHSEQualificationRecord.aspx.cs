using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;
using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTHSEQualificationRecord : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","资质备案", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            LoadHSEPerEquipRecordName();
            LoadHSEQualificationRecord();
        }
    }

    protected void LoadHSEPerEquipRecordName()
    {
        string strHQL = "from HSEPerEquipRecord as hSEPerEquipRecord Where hSEPerEquipRecord.AuditStatus='Qualified' Order by hSEPerEquipRecord.Code Desc ";
        HSEPerEquipRecordBLL hSEPerEquipRecordBLL = new HSEPerEquipRecordBLL();
        IList lst = hSEPerEquipRecordBLL.GetAllHSEPerEquipRecords(strHQL);
        DL_PerEquipRecordCode.DataSource = lst;
        DL_PerEquipRecordCode.DataBind();
        DL_PerEquipRecordCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strCode, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strCode = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;


                strHQL = "from HSEQualificationRecord as hSEQualificationRecord where hSEQualificationRecord.Code = '" + strCode + "' ";
                HSEQualificationRecordBLL hSEQualificationRecordBLL = new HSEQualificationRecordBLL();
                lst = hSEQualificationRecordBLL.GetAllHSEQualificationRecords(strHQL);
                HSEQualificationRecord hSEQualificationRecord = (HSEQualificationRecord)lst[0];

                DL_PerEquipRecordCode.SelectedValue = hSEQualificationRecord.PerEquipRecordCode.Trim();
                TB_BusinessScope.Text = hSEQualificationRecord.BusinessScope.Trim();
                LB_Code.Text = hSEQualificationRecord.Code.Trim();
                TB_Name.Text = hSEQualificationRecord.Name.Trim();
                TB_Construction.Text = hSEQualificationRecord.Construction.Trim();
                TB_SubcontractWork.Text = hSEQualificationRecord.SubcontractWork.Trim();
                //if (hSEQualificationRecord.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_DeleteAA.Visible = true;
                //    BT_UpdateAA.Visible = true;
                //    BT_UpdateAA.Enabled = true;
                //    BT_DeleteAA.Enabled = true;
                //}
                //else
                //{
                //    BT_UpdateAA.Visible = false;
                //    BT_DeleteAA.Visible = false;
                //}
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
            }

            if (e.CommandName == "Delete")
            {
                Delete();

            }
        }
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
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWZZBAMCBNWKJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEQualificationRecordName(TB_Name.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWZZBAMCYCZJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_PerEquipRecordCode.SelectedValue.Trim()) || DL_PerEquipRecordCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYMCWBXJC").ToString().Trim() + "')", true);
            DL_PerEquipRecordCode.Focus();
            return;
        }

        HSEQualificationRecordBLL hSEQualificationRecordBLL = new HSEQualificationRecordBLL();
        HSEQualificationRecord hSEQualificationRecord = new HSEQualificationRecord();

        hSEQualificationRecord.Code = GetHSEQualificationRecordCode();
        LB_Code.Text = hSEQualificationRecord.Code.Trim();
        hSEQualificationRecord.BusinessScope = TB_BusinessScope.Text.Trim();
        hSEQualificationRecord.PerEquipRecordCode = DL_PerEquipRecordCode.SelectedValue.Trim();
        hSEQualificationRecord.PerEquipRecordName = GetHSEPerEquipRecordName(hSEQualificationRecord.PerEquipRecordCode.Trim());
        hSEQualificationRecord.Name = TB_Name.Text.Trim();
        hSEQualificationRecord.Construction = TB_Construction.Text.Trim();
        hSEQualificationRecord.SubcontractWork = TB_SubcontractWork.Text.Trim();
        hSEQualificationRecord.EnterCode = strUserCode.Trim();

        try
        {
            hSEQualificationRecordBLL.AddHSEQualificationRecord(hSEQualificationRecord);

            LoadHSEQualificationRecord();

            //BT_DeleteAA.Visible = true;
            //BT_UpdateAA.Visible = true;
            //BT_UpdateAA.Enabled = true;
            //BT_DeleteAA.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }

    /// <summary>
    /// 取得人员/设备备案的名称。
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetHSEPerEquipRecordName(string strCode)
    {
        string strHQL = "Select * From T_HSEPerEquipRecord Where Code='" + strCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPerEquipRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return dt.Rows[0]["Name"].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 判断名称是否已存在
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected bool IsHSEQualificationRecordName(string strName, string strCode)
    {
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
            strHQL = "Select Code From T_HSEQualificationRecord Where Name ='" + strName + "' ";
        else
            strHQL = "Select Code From T_HSEQualificationRecord Where Name ='" + strName + "' and Code<>'" + strCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEQualificationRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 取得资质备案最大编号 规则HSEQFRX(X代表自增数字)。
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetHSEQualificationRecordCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEQualificationRecord Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEQualificationRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEQFR" + pa.ToString();
        }
        else
        {
            flag = "HSEQFR1";
        }
        return flag;
    }

    protected void Update()
    {
        string strID = LB_Code.Text.Trim();

        if (string.IsNullOrEmpty(TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYMCBNWKJC").ToString().Trim() + "')", true);
            return;
        }
        if (IsHSEQualificationRecordName(TB_Name.Text.Trim(), LB_Code.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWZZBAMCYCZJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_PerEquipRecordCode.SelectedValue.Trim()) || DL_PerEquipRecordCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYMCWBXJC").ToString().Trim() + "')", true);
            DL_PerEquipRecordCode.Focus();
            return;
        }

        HSEQualificationRecordBLL hSEQualificationRecordBLL = new HSEQualificationRecordBLL();
        string strHQL = "from HSEQualificationRecord as hSEQualificationRecord where hSEQualificationRecord.Code = '" + strID + "' ";
        IList lst = hSEQualificationRecordBLL.GetAllHSEQualificationRecords(strHQL);
        HSEQualificationRecord hSEQualificationRecord = (HSEQualificationRecord)lst[0];

        hSEQualificationRecord.BusinessScope = TB_BusinessScope.Text.Trim();
        hSEQualificationRecord.PerEquipRecordCode = DL_PerEquipRecordCode.SelectedValue.Trim();
        hSEQualificationRecord.PerEquipRecordName = GetHSEPerEquipRecordName(hSEQualificationRecord.PerEquipRecordCode.Trim());
        hSEQualificationRecord.Name = TB_Name.Text.Trim();
        hSEQualificationRecord.Construction = TB_Construction.Text.Trim();
        hSEQualificationRecord.SubcontractWork = TB_SubcontractWork.Text.Trim();
        try
        {
            hSEQualificationRecordBLL.UpdateHSEQualificationRecord(hSEQualificationRecord, hSEQualificationRecord.Code);
            LoadHSEQualificationRecord();

            //BT_UpdateAA.Visible = true;
            //BT_UpdateAA.Enabled = true;
            //BT_DeleteAA.Visible = true;
            //BT_DeleteAA.Enabled = true;

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
        string strID;
        string strHQL;

        strID = LB_Code.Text.Trim();

        strHQL = "delete from T_HSEQualificationRecord where Code = '" + strID + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEQualificationRecord();

            //BT_DeleteAA.Visible = false;
            //BT_UpdateAA.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEQualificationRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void LoadHSEQualificationRecord()
    {
        string strHQL;

        strHQL = "Select * From T_HSEQualificationRecord Where 1=1 ";
        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " and (Code like '%" + txt_SupplierInfo.Text.Trim() + "%' or Name like '%" + txt_SupplierInfo.Text.Trim() + "%' or PerEquipRecordName like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
                "or BusinessScope like '%" + txt_SupplierInfo.Text.Trim() + "%' or SubcontractWork like '%" + txt_SupplierInfo.Text.Trim() + "%' or Construction like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        strHQL += " Order by Code DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEQualificationRecord");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEQualificationRecord();
    }

    protected void DL_PerEquipRecordCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL = "from HSEPerEquipRecord as hSEPerEquipRecord Where hSEPerEquipRecord.Code = '" + DL_PerEquipRecordCode.SelectedValue.Trim() + "' ";
        HSEPerEquipRecordBLL hSEPerEquipRecordBLL = new HSEPerEquipRecordBLL();
        IList lst = hSEPerEquipRecordBLL.GetAllHSEPerEquipRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEPerEquipRecord hSEPerEquipRecord = (HSEPerEquipRecord)lst[0];
            TB_BusinessScope.Text = hSEPerEquipRecord.SuppServScope.Trim();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }
}