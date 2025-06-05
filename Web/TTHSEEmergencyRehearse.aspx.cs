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

public partial class TTHSEEmergencyRehearse : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","应急预案演练", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LoadHSEEmergencyCompileName();

            LoadHSEEmergencyRehearseList();
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
        if (IsHSEEmergencyRehearse(string.Empty, TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_EmergencyCompileCode.SelectedValue.Trim()) || DL_EmergencyCompileCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYJYAWBXCZSBJC") + "')", true);
            DL_EmergencyCompileCode.Focus();
            return;
        }
        HSEEmergencyRehearseBLL hSEEmergencyRehearseBLL = new HSEEmergencyRehearseBLL();
        HSEEmergencyRehearse hSEEmergencyRehearse = new HSEEmergencyRehearse();
        hSEEmergencyRehearse.Code = GetHSEEmergencyRehearseCode();
        LB_Code.Text = hSEEmergencyRehearse.Code.Trim();
        hSEEmergencyRehearse.RehearseDate = DateTime.Parse(DLC_RehearseDate.Text.Trim());
        hSEEmergencyRehearse.Name = TB_Name.Text.Trim();
        hSEEmergencyRehearse.EmergencyCompileCode = DL_EmergencyCompileCode.SelectedValue.Trim();
        hSEEmergencyRehearse.EmergencyCompileName = GetHSEEmergencyCompileName(hSEEmergencyRehearse.EmergencyCompileCode.Trim());
        hSEEmergencyRehearse.RehearseFeedBack = TB_RehearseFeedBack.Text.Trim();
        hSEEmergencyRehearse.Header = TB_Header.Text.Trim();
        hSEEmergencyRehearse.Participants = TB_Participants.Text.Trim();
        hSEEmergencyRehearse.RehearseAddr = TB_RehearseAddr.Text.Trim();
        hSEEmergencyRehearse.RehearseQuestion = TB_RehearseQuestion.Text.Trim();
        hSEEmergencyRehearse.EnterCode = strUserCode.Trim();

        try
        {
            hSEEmergencyRehearseBLL.AddHSEEmergencyRehearse(hSEEmergencyRehearse);

            UpdateHSEEmergencyCompile(hSEEmergencyRehearse.EmergencyCompileCode.Trim());

            LoadHSEEmergencyRehearseList();

            //BT_Update.Visible = true;
            //BT_Delete.Visible = true;
            //BT_Update.Enabled = true;
            //BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);

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
        if (IsHSEEmergencyRehearse(LB_Code.Text.Trim(), TB_Name.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGMCYCZCZSBJC") + "')", true);
            TB_Name.Focus();
            return;
        }
        if (string.IsNullOrEmpty(DL_EmergencyCompileCode.SelectedValue.Trim()) || DL_EmergencyCompileCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGYJYAWBXCZSBJC") + "')", true);
            DL_EmergencyCompileCode.Focus();
            return;
        }

        string strHQL = "From HSEEmergencyRehearse as hSEEmergencyRehearse Where hSEEmergencyRehearse.Code='" + LB_Code.Text.Trim() + "' ";
        HSEEmergencyRehearseBLL hSEEmergencyRehearseBLL = new HSEEmergencyRehearseBLL();
        IList lst = hSEEmergencyRehearseBLL.GetAllHSEEmergencyRehearses(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEEmergencyRehearse hSEEmergencyRehearse = (HSEEmergencyRehearse)lst[0];
            hSEEmergencyRehearse.Name = TB_Name.Text.Trim();
            hSEEmergencyRehearse.EmergencyCompileCode = DL_EmergencyCompileCode.SelectedValue.Trim();
            hSEEmergencyRehearse.EmergencyCompileName = GetHSEEmergencyCompileName(hSEEmergencyRehearse.EmergencyCompileCode.Trim());
            hSEEmergencyRehearse.RehearseFeedBack = TB_RehearseFeedBack.Text.Trim();
            hSEEmergencyRehearse.Header = TB_Header.Text.Trim();
            hSEEmergencyRehearse.Participants = TB_Participants.Text.Trim();
            hSEEmergencyRehearse.RehearseAddr = TB_RehearseAddr.Text.Trim();
            hSEEmergencyRehearse.RehearseQuestion = TB_RehearseQuestion.Text.Trim();
            hSEEmergencyRehearse.RehearseDate = DateTime.Parse(DLC_RehearseDate.Text.Trim());
            try
            {
                hSEEmergencyRehearseBLL.UpdateHSEEmergencyRehearse(hSEEmergencyRehearse, hSEEmergencyRehearse.Code);

                LoadHSEEmergencyRehearseList();

                //BT_Update.Visible = true;
                //BT_Delete.Visible = true;
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
    }

    protected void Delete()
    {
        string strCode = LB_Code.Text.Trim();
        string strHQL = "Delete From T_HSEEmergencyRehearse Where Code = '" + strCode + "' ";
        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadHSEEmergencyRehearseList();

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC") + "')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadHSEEmergencyRehearseList();
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


                strHQL = "From HSEEmergencyRehearse as hSEEmergencyRehearse where hSEEmergencyRehearse.Code = '" + strCode + "'";
                HSEEmergencyRehearseBLL hSEEmergencyRehearseBLL = new HSEEmergencyRehearseBLL();
                lst = hSEEmergencyRehearseBLL.GetAllHSEEmergencyRehearses(strHQL);
                HSEEmergencyRehearse hSEEmergencyRehearse = (HSEEmergencyRehearse)lst[0];

                LB_Code.Text = hSEEmergencyRehearse.Code.Trim();
                DL_EmergencyCompileCode.SelectedValue = hSEEmergencyRehearse.EmergencyCompileCode.Trim();
                TB_Name.Text = hSEEmergencyRehearse.Name.Trim();
                TB_Header.Text = hSEEmergencyRehearse.Header.Trim();
                TB_RehearseAddr.Text = hSEEmergencyRehearse.RehearseAddr.Trim();
                TB_Participants.Text = hSEEmergencyRehearse.Participants.Trim();
                TB_RehearseQuestion.Text = hSEEmergencyRehearse.RehearseQuestion.Trim();
                TB_RehearseFeedBack.Text = hSEEmergencyRehearse.RehearseFeedBack.Trim();
                DLC_RehearseDate.Text = hSEEmergencyRehearse.RehearseDate.ToString("yyyy-MM-dd");
                //if (hSEEmergencyRehearse.EnterCode.Trim() == strUserCode.Trim())
                //{
                //    BT_Update.Visible = true;
                //    BT_Delete.Visible = true;
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
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEmergencyRehearse");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }


    protected void LoadHSEEmergencyCompileName()
    {
        string strHQL;
        IList lst;
        //绑定应急预案编制名称
        strHQL = "From HSEEmergencyCompile as hSEEmergencyCompile Order By hSEEmergencyCompile.Code Desc";
        HSEEmergencyCompileBLL hSEEmergencyCompileBLL = new HSEEmergencyCompileBLL();
        lst = hSEEmergencyCompileBLL.GetAllHSEEmergencyCompiles(strHQL);
        DL_EmergencyCompileCode.DataSource = lst;
        DL_EmergencyCompileCode.DataBind();
        DL_EmergencyCompileCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadHSEEmergencyRehearseList()
    {
        string strHQL;

        strHQL = "Select * From T_HSEEmergencyRehearse Where 1=1 ";

        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (Code like '%" + TextBox1.Text.Trim() + "%' or Name like '%" + TextBox1.Text.Trim() + "%' or RehearseAddr like '%" + TextBox1.Text.Trim() + "%' " +
                "or RehearseFeedBack like '%" + TextBox1.Text.Trim() + "%' or RehearseQuestion like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text.Trim()))
        {
            strHQL += " and (EmergencyCompileCode like '%" + TextBox2.Text.Trim() + "%' or EmergencyCompileName like '%" + TextBox2.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-RehearseDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
        {
            strHQL += " and '" + TextBox4.Text.Trim() + "'::date-RehearseDate::date>=0 ";
        }
        strHQL += " Order By Code DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEmergencyRehearse");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    /// <summary>
    /// 新增或更新时，检查应急预案演练名称是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected bool IsHSEEmergencyRehearse(string strCode, string strName)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strCode))
        {
            strHQL = "Select Code From T_HSEEmergencyRehearse Where Name='" + strName + "'";
        }
        else
            strHQL = "Select Code From T_HSEEmergencyRehearse Where Name='" + strName + "' and Code<>'" + strCode + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEmergencyRehearse").Tables[0];
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

    protected string GetHSEEmergencyCompileName(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定应急预案编制名称
        strHQL = "From HSEEmergencyCompile as hSEEmergencyCompile Where hSEEmergencyCompile.Code='" + strCode + "' ";
        HSEEmergencyCompileBLL hSEEmergencyCompileBLL = new HSEEmergencyCompileBLL();
        lst = hSEEmergencyCompileBLL.GetAllHSEEmergencyCompiles(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEEmergencyCompile hSEEmergencyCompile = (HSEEmergencyCompile)lst[0];
            return hSEEmergencyCompile.Name.Trim();
        }
        else
            return "";
    }

    protected void UpdateHSEEmergencyCompile(string strCode)
    {
        string strHQL;
        IList lst;
        //绑定应急预案编制名称
        strHQL = "From HSEEmergencyCompile as hSEEmergencyCompile Where hSEEmergencyCompile.Code='" + strCode + "' ";
        HSEEmergencyCompileBLL hSEEmergencyCompileBLL = new HSEEmergencyCompileBLL();
        lst = hSEEmergencyCompileBLL.GetAllHSEEmergencyCompiles(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            HSEEmergencyCompile hSEEmergencyCompile = (HSEEmergencyCompile)lst[0];
            hSEEmergencyCompile.Status = "Drilled";
            hSEEmergencyCompileBLL.UpdateHSEEmergencyCompile(hSEEmergencyCompile, hSEEmergencyCompile.Code);
        }
    }

    /// <summary>
    /// 新增时，获取表T_HSEEmergencyRehearse中最大编号 规则HSEENRX(X代表自增数字)。
    /// </summary>
    /// <param name="strBarCode"></param>
    /// <param name="strId"></param>
    /// <returns></returns>
    protected string GetHSEEmergencyRehearseCode()
    {
        string flag = string.Empty;
        string strHQL = "Select Code From T_HSEEmergencyRehearse Order by Code Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_HSEEmergencyRehearse").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            int pa = int.Parse(dt.Rows[0]["Code"].ToString().Substring(6)) + 1;
            flag = "HSEENR" + pa.ToString();
        }
        else
        {
            flag = "HSEENR1";
        }
        return flag;
    }


}