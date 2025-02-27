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
using System.IO;
using System.Windows.Forms;

public partial class TTHSEPerEquipRecord : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","人员/设备备案", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_ValidityStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ValidityEnd.Text = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

            LoadHSEPerEquipRecord();
        }
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


                strHQL = "from HSEPerEquipRecord as hSEPerEquipRecord where hSEPerEquipRecord.Code = '" + strCode + "' ";
                HSEPerEquipRecordBLL hSEPerEquipRecordBLL = new HSEPerEquipRecordBLL();
                lst = hSEPerEquipRecordBLL.GetAllHSEPerEquipRecords(strHQL);
                HSEPerEquipRecord hSEPerEquipRecord = (HSEPerEquipRecord)lst[0];

                DL_Type.SelectedValue = hSEPerEquipRecord.Type.Trim();
                DLC_ValidityEnd.Text = hSEPerEquipRecord.ValidityEnd.ToString("yyyy-MM-dd");
                DL_AuditStatus.SelectedValue = hSEPerEquipRecord.AuditStatus.Trim();
                TB_Address.Text = hSEPerEquipRecord.Address.Trim();
                TB_BankName.Text = hSEPerEquipRecord.BankName.Trim();
                DLC_ValidityStart.Text = hSEPerEquipRecord.ValidityStart.ToString("yyyy-MM-dd");
                LB_Code.Text = hSEPerEquipRecord.Code.Trim();
                TB_CompanyFor.Text = hSEPerEquipRecord.CompanyFor.Trim();
                TB_EinNo.Text = hSEPerEquipRecord.EinNo.Trim();
                TB_Email.Text = hSEPerEquipRecord.Email.Trim();
                TB_Fax.Text = hSEPerEquipRecord.Fax.Trim();
                TB_Name.Text = hSEPerEquipRecord.Name.Trim();
                TB_LinkTel.Text = hSEPerEquipRecord.LinkTel.Trim();
                TB_Qualifications.Text = hSEPerEquipRecord.Qualifications.Trim();
                TB_SuppServScope.Text = hSEPerEquipRecord.SuppServScope.Trim();
                TB_WebAddress.Text = hSEPerEquipRecord.WebAddress.Trim();
                TB_ZipCode.Text = hSEPerEquipRecord.ZipCode.Trim();
                //if (hSEPerEquipRecord.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_UpdateAA.Visible = true;
                //    BT_DeleteAA.Visible = true;
                //    BT_UpdateAA.Enabled = true;
                //    BT_DeleteAA.Enabled = true;
                //}
                //else
                //{
                //    BT_DeleteAA.Visible = false;
                //    BT_UpdateAA.Visible = false;
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYMCBNWKJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSEPerEquipRecordName(TB_Name.Text.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYMCYCZJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }

        HSEPerEquipRecordBLL hSEPerEquipRecordBLL = new HSEPerEquipRecordBLL();
        HSEPerEquipRecord hSEPerEquipRecord = new HSEPerEquipRecord();

        hSEPerEquipRecord.Code = GetHSEPerEquipRecordCode();
        LB_Code.Text = hSEPerEquipRecord.Code.Trim();
        hSEPerEquipRecord.Address = TB_Address.Text.Trim();
        hSEPerEquipRecord.BankName = TB_BankName.Text.Trim();
        hSEPerEquipRecord.ValidityStart = DateTime.Parse(DLC_ValidityStart.Text.Trim());
        hSEPerEquipRecord.CompanyFor = TB_CompanyFor.Text.Trim();
        hSEPerEquipRecord.Type = DL_Type.SelectedValue.Trim();
        hSEPerEquipRecord.EinNo = TB_EinNo.Text.Trim();
        hSEPerEquipRecord.Email = TB_Email.Text.Trim();
        hSEPerEquipRecord.ValidityEnd = DateTime.Parse(DLC_ValidityEnd.Text.Trim());
        hSEPerEquipRecord.Fax = TB_Fax.Text.Trim();
        hSEPerEquipRecord.Name = TB_Name.Text.Trim();
        hSEPerEquipRecord.LinkTel = TB_LinkTel.Text.Trim();
        hSEPerEquipRecord.Qualifications = TB_Qualifications.Text.Trim();
        hSEPerEquipRecord.AuditStatus = DL_AuditStatus.SelectedValue.Trim();
        hSEPerEquipRecord.SuppServScope = TB_SuppServScope.Text.Trim();
        hSEPerEquipRecord.WebAddress = TB_WebAddress.Text.Trim();
        hSEPerEquipRecord.ZipCode = TB_ZipCode.Text.Trim();
        hSEPerEquipRecord.EnterCode = strUserCode.Trim();

        try
        {
            hSEPerEquipRecordBLL.AddHSEPerEquipRecord(hSEPerEquipRecord);

            LoadHSEPerEquipRecord();

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
    /// 判断名称是否已存在
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected bool IsHSEPerEquipRecordName(string strName, string strCode)
    {
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
            strHQL = "Select Code From T_HSEPerEquipRecord Where Name ='" + strName + "' ";
        else
            strHQL = "Select Code From T_HSEPerEquipRecord Where Name ='" + strName + "' and Code<>'" + strCode + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPerEquipRecord").Tables[0];
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
    /// 取得人员/设备备案最大编号 规则HSEPERX(X代表自增数字)。
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected string GetHSEPerEquipRecordCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEPerEquipRecord Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPerEquipRecord").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEPER" + pa.ToString();
        }
        else
        {
            flag = "HSEPER1";
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
        if (IsHSEPerEquipRecordName(TB_Name.Text.Trim(), LB_Code.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYMCYCZJC").ToString().Trim() + "')", true);
            TB_Name.Focus();
            return;
        }

        HSEPerEquipRecordBLL hSEPerEquipRecordBLL = new HSEPerEquipRecordBLL();
        string strHQL = "from HSEPerEquipRecord as hSEPerEquipRecord where hSEPerEquipRecord.Code = '" + strID + "' ";
        IList lst = hSEPerEquipRecordBLL.GetAllHSEPerEquipRecords(strHQL);
        HSEPerEquipRecord hSEPerEquipRecord = (HSEPerEquipRecord)lst[0];

        hSEPerEquipRecord.Address = TB_Address.Text.Trim();
        hSEPerEquipRecord.BankName = TB_BankName.Text.Trim();
        hSEPerEquipRecord.ValidityStart = DateTime.Parse(DLC_ValidityStart.Text.Trim());
        hSEPerEquipRecord.CompanyFor = TB_CompanyFor.Text.Trim();
        hSEPerEquipRecord.Type = DL_Type.SelectedValue.Trim();
        hSEPerEquipRecord.EinNo = TB_EinNo.Text.Trim();
        hSEPerEquipRecord.Email = TB_Email.Text.Trim();
        hSEPerEquipRecord.ValidityEnd = DateTime.Parse(DLC_ValidityEnd.Text.Trim());
        hSEPerEquipRecord.Fax = TB_Fax.Text.Trim();
        hSEPerEquipRecord.Name = TB_Name.Text.Trim();
        hSEPerEquipRecord.LinkTel = TB_LinkTel.Text.Trim();
        hSEPerEquipRecord.Qualifications = TB_Qualifications.Text.Trim();
        hSEPerEquipRecord.AuditStatus = DL_AuditStatus.SelectedValue.Trim();
        hSEPerEquipRecord.SuppServScope = TB_SuppServScope.Text.Trim();
        hSEPerEquipRecord.WebAddress = TB_WebAddress.Text.Trim();
        hSEPerEquipRecord.ZipCode = TB_ZipCode.Text.Trim();
        try
        {
            hSEPerEquipRecordBLL.UpdateHSEPerEquipRecord(hSEPerEquipRecord, hSEPerEquipRecord.Code);
            LoadHSEPerEquipRecord();

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
        if (IsHSEQualificationRecordExits(LB_Code.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBGBAYJXGZZBAJC").ToString().Trim() + "')", true);
            return;
        }

        strID = LB_Code.Text.Trim();

        strHQL = "delete from T_HSEPerEquipRecord where Code = '" + strID + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEPerEquipRecord();

            //BT_DeleteAA.Visible = false;
            //BT_UpdateAA.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    /// <summary>
    /// 判断是否已进行资质备案
    /// </summary>
    /// <param name="strCode"></param>
    /// <returns></returns>
    protected bool IsHSEQualificationRecordExits(string strCode)
    {
        bool flag1 = true;
        HSEQualificationRecordBLL hSEQualificationRecordBLL = new HSEQualificationRecordBLL();
        string strHQL = "from HSEQualificationRecord as hSEQualificationRecord where hSEQualificationRecord.PerEquipRecordCode = '" + strCode + "' ";
        IList lst = hSEQualificationRecordBLL.GetAllHSEQualificationRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            flag1 = true;
        }
        else
            flag1 = false;
        return flag1;
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPerEquipRecord");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void LoadHSEPerEquipRecord()
    {
        string strHQL;

        strHQL = "Select * From T_HSEPerEquipRecord Where 1=1 ";
        if (!string.IsNullOrEmpty(txt_SupplierInfo.Text.Trim()))
        {
            strHQL += " and (Code like '%" + txt_SupplierInfo.Text.Trim() + "%' or Name like '%" + txt_SupplierInfo.Text.Trim() + "%' or CompanyFor like '%" + txt_SupplierInfo.Text.Trim() + "%' " +
                "or SuppServScope like '%" + txt_SupplierInfo.Text.Trim() + "%' or Qualifications like '%" + txt_SupplierInfo.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and '" + TextBox2.Text.Trim() + "'::date-ValidityEnd::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-ValidityStart::date>=0 ";
        }
        strHQL += " Order by Code DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEPerEquipRecord");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEPerEquipRecord();
    }
}