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

public partial class TTHSERectificationNotice : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","安全隐患整改通知", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_InspectionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_ReqRecDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SignDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SignDate2.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadHSESafeInspectionRecordName();

            LoadHSERectificationNoticeList();
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
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSERectificationNoticeCode(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_SafeInspectId.SelectedValue.Trim()) || DL_SafeInspectId.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGAJCJLBXCZSBJC") + "')", true);
            DL_SafeInspectId.Focus();
            return;
        }

        HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
        HSERectificationNotice hSERectificationNotice = new HSERectificationNotice();

        hSERectificationNotice.Code = GetHSERectificationNoticeCode();
        LB_Code.Text = hSERectificationNotice.Code.Trim();
        hSERectificationNotice.DepartCode = TB_DepartCode.Text.Trim();
        hSERectificationNotice.Inspectors = TB_Inspectors.Text.Trim();
        hSERectificationNotice.InspectionDate = DateTime.Parse(DLC_InspectionDate.Text.Trim());
        hSERectificationNotice.QESEngineerCode = TB_QESEngineerCode.Text.Trim();
        hSERectificationNotice.RectificationOpinions = TB_RectificationOpinions.Text.Trim();
        hSERectificationNotice.ReqRecDate = DateTime.Parse(DLC_ReqRecDate.Text.Trim());
        hSERectificationNotice.Name = TB_Name.Text.Trim();
        hSERectificationNotice.SafeInspectId = DL_SafeInspectId.SelectedValue.Trim();
        hSERectificationNotice.ProjectManager = TB_ProjectManager.Text.Trim();
        hSERectificationNotice.SafeInspectName = GetSafeInspectName(hSERectificationNotice.SafeInspectId.Trim());
        hSERectificationNotice.SignDate1 = DateTime.Parse(DLC_SignDate1.Text.Trim());
        hSERectificationNotice.SignDate2 = DateTime.Parse(DLC_SignDate2.Text.Trim());
        hSERectificationNotice.Status = DL_Status.SelectedValue.Trim();
        hSERectificationNotice.EnterCode = strUserCode.Trim();

        try
        {
            hSERectificationNoticeBLL.AddHSERectificationNotice(hSERectificationNotice);

            LoadHSERectificationNoticeList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

        }
    }



    protected void Update()

    {
        if (string.IsNullOrEmpty(TB_Name.Text.Trim()) || TB_Name.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCBNWKCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (IsHSERectificationNoticeCode(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_SafeInspectId.SelectedValue.Trim()) || DL_SafeInspectId.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGAJCJLBXCZSBJC") + "')", true);
            DL_SafeInspectId.Focus();
            return;
        }

        string strHQL = "From HSERectificationNotice as hSERectificationNotice where hSERectificationNotice.Code = '" + LB_Code.Text.Trim() + "'";
        HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
        IList lst = hSERectificationNoticeBLL.GetAllHSERectificationNotices(strHQL);

        HSERectificationNotice hSERectificationNotice = (HSERectificationNotice)lst[0];

        //   hSERectificationNotice.Code = LB_Code.Text.Trim();
        hSERectificationNotice.DepartCode = TB_DepartCode.Text.Trim();
        hSERectificationNotice.Inspectors = TB_Inspectors.Text.Trim();
        hSERectificationNotice.InspectionDate = DateTime.Parse(DLC_InspectionDate.Text.Trim());
        hSERectificationNotice.QESEngineerCode = TB_QESEngineerCode.Text.Trim();
        hSERectificationNotice.RectificationOpinions = TB_RectificationOpinions.Text.Trim();
        hSERectificationNotice.ReqRecDate = DateTime.Parse(DLC_ReqRecDate.Text.Trim());
        hSERectificationNotice.Name = TB_Name.Text.Trim();
        hSERectificationNotice.SafeInspectId = DL_SafeInspectId.SelectedValue.Trim();
        hSERectificationNotice.ProjectManager = TB_ProjectManager.Text.Trim();
        hSERectificationNotice.SafeInspectName = GetSafeInspectName(hSERectificationNotice.SafeInspectId.Trim());
        hSERectificationNotice.SignDate1 = DateTime.Parse(DLC_SignDate1.Text.Trim());
        hSERectificationNotice.SignDate2 = DateTime.Parse(DLC_SignDate2.Text.Trim());
        hSERectificationNotice.Status = DL_Status.SelectedValue.Trim();

        try
        {
            hSERectificationNoticeBLL.UpdateHSERectificationNotice(hSERectificationNotice, hSERectificationNotice.Code);

            LoadHSERectificationNoticeList();

            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Visible = true;
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
        if (IsHSERectification(strCode))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZAYHZGZYDYGZGTZDWFSC") + "')", true);
            return;
        }

        strHQL = "Delete From T_HSERectificationNotice Where Code = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSERectificationNoticeList();

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

                strHQL = "From HSERectificationNotice as hSERectificationNotice where hSERectificationNotice.Code = '" + strCode + "'";
                HSERectificationNoticeBLL hSERectificationNoticeBLL = new HSERectificationNoticeBLL();
                lst = hSERectificationNoticeBLL.GetAllHSERectificationNotices(strHQL);

                HSERectificationNotice hSERectificationNotice = (HSERectificationNotice)lst[0];

                LB_Code.Text = hSERectificationNotice.Code.Trim();
                TB_DepartCode.Text = hSERectificationNotice.DepartCode.Trim();
                DL_SafeInspectId.SelectedValue = hSERectificationNotice.SafeInspectId.Trim();
                TB_RectificationOpinions.Text = hSERectificationNotice.RectificationOpinions.Trim();
                DLC_ReqRecDate.Text = hSERectificationNotice.ReqRecDate.ToString("yyyy-MM-dd");
                DLC_SignDate1.Text = hSERectificationNotice.SignDate1.ToString("yyyy-MM-dd");
                DLC_SignDate2.Text = hSERectificationNotice.SignDate2.ToString("yyyy-MM-dd");
                TB_Inspectors.Text = hSERectificationNotice.Inspectors.Trim();
                TB_Name.Text = hSERectificationNotice.Name.Trim();
                TB_ProjectManager.Text = hSERectificationNotice.ProjectManager.Trim();
                TB_QESEngineerCode.Text = hSERectificationNotice.QESEngineerCode.Trim();
                DL_Status.SelectedValue = hSERectificationNotice.Status.Trim();
                DLC_InspectionDate.Text = hSERectificationNotice.InspectionDate.ToString("yyyy-MM-dd");
                //if (hSERectificationNotice.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Update.Enabled = true;
                //    BT_Delete.Visible = true;
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

    /// <summary>
    /// 删除时，检查安全隐患整改是否存在该整改通知单，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSERectification(string strCode)
    {
        bool flag = true;
        string strHQL = "Select Code From T_HSERectification Where RectificationNoticeId='" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectification").Tables[0];
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

    protected void DL_SafeInspectId_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        //绑定安检名称
        strHQL = "From HSESafeInspectionRecord as hSESafeInspectionRecord Where hSESafeInspectionRecord.Code='" + DL_SafeInspectId.SelectedValue.Trim() + "' ";
        HSESafeInspectionRecordBLL hSESafeInspectionRecordBLL = new HSESafeInspectionRecordBLL();
        lst = hSESafeInspectionRecordBLL.GetAllHSESafeInspectionRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSESafeInspectionRecord hSESafeInspectionRecord = (HSESafeInspectionRecord)lst[0];
            DLC_InspectionDate.Text = hSESafeInspectionRecord.InspectionDate.ToString("yyyy-MM-dd");
            TB_QESEngineerCode.Text = hSESafeInspectionRecord.QESEngineer.Trim();
            TB_Inspectors.Text = hSESafeInspectionRecord.InspectionTeamLeader.Trim() + LanguageHandle.GetWord("ZuChang") + hSESafeInspectionRecord.InspectorsCode.Trim();
            TB_ProjectManager.Text = hSESafeInspectionRecord.ProjectManager.Trim();
            if (!string.IsNullOrEmpty(hSESafeInspectionRecord.InspectionOverView.Trim()) && !string.IsNullOrEmpty(hSESafeInspectionRecord.InspectContentFindings.Trim()))
            {
                lbl_InspectionOverViews.Text = LanguageHandle.GetWord("JianChaQingKuangGaiShu") + hSESafeInspectionRecord.InspectionOverView.Trim() + LanguageHandle.GetWord("brJianChaNeiRongJiJiFaXian") + hSESafeInspectionRecord.InspectContentFindings.Trim();
            }
            else if (string.IsNullOrEmpty(hSESafeInspectionRecord.InspectionOverView.Trim()) && !string.IsNullOrEmpty(hSESafeInspectionRecord.InspectContentFindings.Trim()))
            {
                lbl_InspectionOverViews.Text = LanguageHandle.GetWord("JianChaNeiRongJiJiFaXian") + hSESafeInspectionRecord.InspectContentFindings.Trim();
            }
            else if (!string.IsNullOrEmpty(hSESafeInspectionRecord.InspectionOverView.Trim()) && string.IsNullOrEmpty(hSESafeInspectionRecord.InspectContentFindings.Trim()))
            {
                lbl_InspectionOverViews.Text = LanguageHandle.GetWord("JianChaQingKuangGaiShu") + hSESafeInspectionRecord.InspectionOverView.Trim() + "。";
            }
            else
            {
                lbl_InspectionOverViews.Text = "";
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectificationNotice");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSERectificationNoticeList();
    }

    /// <summary>
    /// 新增或修改时，整改通知名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSERectificationNoticeCode(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
            strHQL = "Select Code From T_HSERectificationNotice Where Name='" + strName + "'";
        else
            strHQL = "Select Code From T_HSERectificationNotice Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectificationNotice").Tables[0];
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
    /// 新增时，获取表T_HSERectificationNotice中最大编号 规则HSERTNX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSERectificationNoticeCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSERectificationNotice Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectificationNotice").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSERTN" + pa.ToString();
        }
        else
        {
            flag = "HSERTN1";
        }
        return flag;
    }

    protected string GetSafeInspectName(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定安检名称
        strHQL = "From HSESafeInspectionRecord as hSESafeInspectionRecord Where hSESafeInspectionRecord.Code='" + strCode + "' ";
        HSESafeInspectionRecordBLL hSESafeInspectionRecordBLL = new HSESafeInspectionRecordBLL();
        lst = hSESafeInspectionRecordBLL.GetAllHSESafeInspectionRecords(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSESafeInspectionRecord hSESafeInspectionRecord = (HSESafeInspectionRecord)lst[0];
            return hSESafeInspectionRecord.Name.Trim();
        }
        else
            return "";
    }

    protected void LoadHSESafeInspectionRecordName()
    {
        string strHQL;
        IList lst;
        //绑定安检名称
        strHQL = "From HSESafeInspectionRecord as hSESafeInspectionRecord Where hSESafeInspectionRecord.Status='Unqualified' Order By hSESafeInspectionRecord.Code Desc";
        HSESafeInspectionRecordBLL hSESafeInspectionRecordBLL = new HSESafeInspectionRecordBLL();
        lst = hSESafeInspectionRecordBLL.GetAllHSESafeInspectionRecords(strHQL);
        DL_SafeInspectId.DataSource = lst;
        DL_SafeInspectId.DataBind();
        DL_SafeInspectId.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadHSERectificationNoticeList()
    {
        string strHQL;

        strHQL = "Select * From T_HSERectificationNotice Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or RectificationOpinions like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (SafeInspectId like '%" + TextBox2.Text.Trim() + "%' or SafeInspectName like '%" + TextBox2.Text.Trim() + "%') ";
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

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSERectificationNotice");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

}