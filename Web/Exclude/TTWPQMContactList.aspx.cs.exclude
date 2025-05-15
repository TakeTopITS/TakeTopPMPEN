using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTWPQMContactList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","联络单管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DLC_CommissionedDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TB_TaskSendUnit.Text = ShareClass.GetDepartName(ShareClass.GetDepartCodeFromUserCode(strUserCode.Trim()));
            TB_ContactPersonTel.Text = ShareClass.GetUserName(strUserCode.Trim());

            LoadWPQMWeldProQuaName();
            LoadWPQMContactListList();
        }
    }

    protected void LoadWPQMWeldProQuaName()
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Order By wPQMWeldProQua.Code Desc";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        DL_WeldProCode.DataSource = lst;
        DL_WeldProCode.DataBind();
        DL_WeldProCode.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void LoadWPQMContactListList()
    {
        string strHQL;

        strHQL = "Select * From T_WPQMContactList Where 1=1";
        
        if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
        {
            strHQL += " and (WeldProCode like '%" + TextBox1.Text.Trim() + "%' or ContactClient like '%" + TextBox1.Text.Trim() + "%' or GroupForm like '%" + TextBox1.Text.Trim() + "%' or ReceivePerson like '%" + TextBox1.Text.Trim() + "%' " +
            "or ContactNote like '%" + TextBox1.Text.Trim() + "%' or TaskSendUnit like '%" + TextBox1.Text.Trim() + "%' or TaskReceiveUnit like '%" + TextBox1.Text.Trim() + "%' or SendPerson like '%" + TextBox1.Text.Trim() + "%') ";
        }
        if (!string.IsNullOrEmpty(TextBox2.Text) && TextBox2.Text.Trim() != "")
        {
            strHQL += " and '"+TextBox2.Text.Trim()+"'::date-ContactDate::date<=0 ";
        }
        if (!string.IsNullOrEmpty(TextBox3.Text) && TextBox3.Text.Trim() != "")
        {
            strHQL += " and '" + TextBox3.Text.Trim() + "'::date-ContactDate::date>=0 ";
        }
        strHQL += " Order By ID DESC ";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMContactList");

        DataGrid2.CurrentPageIndex = 0;
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJGYPDWBXJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMContactList(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGLLDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        WPQMContactListBLL wPQMContactListBLL = new WPQMContactListBLL();
        WPQMContactList wPQMContactList = new WPQMContactList();
        wPQMContactList.CommissionedDate = DateTime.Parse(string.IsNullOrEmpty(DLC_CommissionedDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_CommissionedDate.Text.Trim());
        wPQMContactList.ContactClient = TB_ContactClient.Text.Trim();
        wPQMContactList.ContactDate = DateTime.Now;
        wPQMContactList.ContactNote = TB_ContactNote.Text.Trim();
        wPQMContactList.ContactPersonTel = TB_ContactPersonTel.Text.Trim();
        wPQMContactList.EnterCode = strUserCode.Trim();
        wPQMContactList.ExecutionStandard = TB_ExecutionStandard.Text.Trim();
        wPQMContactList.GroupForm = TB_GroupForm.Text.Trim();
        wPQMContactList.MechanicalPerReq = TB_MechanicalPerReq.Text.Trim();
        wPQMContactList.MechanizationDegree = TB_MechanizationDegree.Text.Trim();
        wPQMContactList.OtherPerReq = TB_OtherPerReq.Text.Trim();
        wPQMContactList.ReceivePerson = TB_ReceivePerson.Text.Trim();
        wPQMContactList.SendPerson = TB_SendPerson.Text.Trim();
        wPQMContactList.TaskReceiveUnit = TB_TaskReceiveUnit.Text.Trim();
        wPQMContactList.TaskSendUnit = TB_TaskSendUnit.Text.Trim();
        wPQMContactList.VisualInspection = TB_VisualInspection.Text.Trim();
        wPQMContactList.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        try
        {
            wPQMContactListBLL.AddWPQMContactList(wPQMContactList);
            lbl_ID.Text = GetMaxWPQMContactListID(wPQMContactList).ToString();
            UpdateWPQMWeldProQuaData(wPQMContactList.WeldProCode.Trim());
            LoadWPQMContactListList();

            BT_Update.Visible = true;
            BT_Update.Enabled = true;
            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected int GetMaxWPQMContactListID(WPQMContactList bmbp)
    {
        string strHQL = "Select ID From T_WPQMContactList where EnterCode='" + bmbp.EnterCode.Trim() + "' and WeldProCode='" + bmbp.WeldProCode.Trim() + "' Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMContactList").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            return int.Parse(dt.Rows[0]["ID"].ToString());
        }
        else
        {
            return 0;
        }
    }

    protected bool IsWPQMContactList(string strWeldProCode,string strusercode, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WPQMContactList Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' ";
        }
        else
            strHQL = "Select ID From T_WPQMContactList Where WeldProCode ='" + strWeldProCode + "' and EnterCode = '" + strusercode + "' and ID<>'" + strID + "' ";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMContactList").Tables[0];
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

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(DL_WeldProCode.SelectedValue) || DL_WeldProCode.SelectedValue.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSHJGYPDWBXJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        if (IsWPQMContactList(DL_WeldProCode.SelectedValue.Trim(), strUserCode.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSGLLDYCZWXZJJC").ToString().Trim()+"')", true);
            DL_WeldProCode.Focus();
            return;
        }
        string strHQL = "From WPQMContactList as wPQMContactList where wPQMContactList.ID = '" + lbl_ID.Text.Trim() + "'";
        WPQMContactListBLL wPQMContactListBLL = new WPQMContactListBLL();
        IList lst = wPQMContactListBLL.GetAllWPQMContactLists(strHQL);
        WPQMContactList wPQMContactList = (WPQMContactList)lst[0];

        wPQMContactList.CommissionedDate = DateTime.Parse(string.IsNullOrEmpty(DLC_CommissionedDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : DLC_CommissionedDate.Text.Trim());
        wPQMContactList.ContactClient = TB_ContactClient.Text.Trim();
        wPQMContactList.ContactNote = TB_ContactNote.Text.Trim();
        wPQMContactList.ContactPersonTel = TB_ContactPersonTel.Text.Trim();
        wPQMContactList.ExecutionStandard = TB_ExecutionStandard.Text.Trim();
        wPQMContactList.GroupForm = TB_GroupForm.Text.Trim();
        wPQMContactList.MechanicalPerReq = TB_MechanicalPerReq.Text.Trim();
        wPQMContactList.MechanizationDegree = TB_MechanizationDegree.Text.Trim();
        wPQMContactList.OtherPerReq = TB_OtherPerReq.Text.Trim();
        wPQMContactList.ReceivePerson = TB_ReceivePerson.Text.Trim();
        wPQMContactList.SendPerson = TB_SendPerson.Text.Trim();
        wPQMContactList.TaskReceiveUnit = TB_TaskReceiveUnit.Text.Trim();
        wPQMContactList.TaskSendUnit = TB_TaskSendUnit.Text.Trim();
        wPQMContactList.VisualInspection = TB_VisualInspection.Text.Trim();
        wPQMContactList.WeldProCode = DL_WeldProCode.SelectedValue.Trim();
        try {
            wPQMContactListBLL.UpdateWPQMContactList(wPQMContactList, wPQMContactList.ID);
            UpdateWPQMWeldProQuaData(wPQMContactList.WeldProCode.Trim());
            LoadWPQMContactListList();

            BT_Delete.Visible = true;
            BT_Delete.Enabled = true;
            BT_Update.Visible = true;
            BT_Update.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strID = lbl_ID.Text.Trim();
        string strHQL = "Delete From T_WPQMContactList Where ID = '" + strID + "'";
        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadWPQMContactListList();
            BT_Update.Visible = false;
            BT_Delete.Visible = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWPQMContactListList();
    }

    protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strId, strHQL;
        IList lst;
        if (e.CommandName != "Page")
        {
            strId = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;
            strHQL = "From WPQMContactList as wPQMContactList where wPQMContactList.ID = '" + strId + "'";
            WPQMContactListBLL wPQMContactListBLL = new WPQMContactListBLL();
            lst = wPQMContactListBLL.GetAllWPQMContactLists(strHQL);
            WPQMContactList wPQMContactList = (WPQMContactList)lst[0];
            TB_ContactClient.Text = wPQMContactList.ContactClient.Trim();
            TB_ContactNote.Text = wPQMContactList.ContactNote.Trim();
            TB_ContactPersonTel.Text = wPQMContactList.ContactPersonTel.Trim();
            TB_ExecutionStandard.Text = wPQMContactList.ExecutionStandard.Trim();
            TB_GroupForm.Text = wPQMContactList.GroupForm.Trim();
            TB_MechanicalPerReq.Text = wPQMContactList.MechanicalPerReq.Trim();
            TB_MechanizationDegree.Text = wPQMContactList.MechanizationDegree.Trim();
            TB_OtherPerReq.Text = wPQMContactList.OtherPerReq.Trim();
            TB_ReceivePerson.Text = wPQMContactList.ReceivePerson.Trim();
            TB_SendPerson.Text = wPQMContactList.SendPerson.Trim();
            TB_TaskReceiveUnit.Text = wPQMContactList.TaskReceiveUnit.Trim();
            TB_TaskSendUnit.Text = wPQMContactList.TaskSendUnit.Trim();
            TB_VisualInspection.Text = wPQMContactList.VisualInspection.Trim();
            DL_WeldProCode.SelectedValue = wPQMContactList.WeldProCode.Trim();
            DLC_CommissionedDate.Text = wPQMContactList.CommissionedDate.ToString("yyyy-MM-dd");
            lbl_ID.Text = wPQMContactList.ID.ToString();

            if (wPQMContactList.EnterCode.Trim() == strUserCode.Trim())
            {
                BT_Delete.Visible = true;
                BT_Update.Visible = true;
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;
            }
            else
            {
                BT_Update.Visible = false;
                BT_Delete.Visible = false;
            }
        }
    }

    protected void DataGrid2_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WPQMContactList");
        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }

    protected void DL_WeldProCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + DL_WeldProCode.SelectedValue.Trim() + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            TB_GroupForm.Text = string.IsNullOrEmpty(wPQMWeldProQua.GroupForm) ? "" : wPQMWeldProQua.GroupForm.Trim();
            TB_MechanicalPerReq.Text = string.IsNullOrEmpty(wPQMWeldProQua.MechanicalPerReq) ? "" : wPQMWeldProQua.MechanicalPerReq.Trim();
            TB_MechanizationDegree.Text = string.IsNullOrEmpty(wPQMWeldProQua.MechanizationDegree) ? "" : wPQMWeldProQua.MechanizationDegree.Trim();
            TB_OtherPerReq.Text = string.IsNullOrEmpty(wPQMWeldProQua.OtherPerReq) ? "" : wPQMWeldProQua.OtherPerReq.Trim();
        }
        else
        {
            TB_GroupForm.Text = "";
            TB_MechanicalPerReq.Text = "";
            TB_MechanizationDegree.Text = "";
            TB_OtherPerReq.Text = "";
        }
    }

    protected void UpdateWPQMWeldProQuaData(string strCode)
    {
        WPQMWeldProQuaBLL wPQMWeldProQuaBLL = new WPQMWeldProQuaBLL();
        string strHQL = "From WPQMWeldProQua as wPQMWeldProQua Where wPQMWeldProQua.Code='" + strCode + "'";
        IList lst = wPQMWeldProQuaBLL.GetAllWPQMWeldProQuas(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            WPQMWeldProQua wPQMWeldProQua = (WPQMWeldProQua)lst[0];
            wPQMWeldProQua.OtherPerReq = TB_OtherPerReq.Text.Trim();
            wPQMWeldProQua.MechanizationDegree = TB_MechanizationDegree.Text.Trim();
            wPQMWeldProQua.MechanicalPerReq = TB_MechanicalPerReq.Text.Trim();
            wPQMWeldProQua.GroupForm = TB_GroupForm.Text.Trim();
            wPQMWeldProQuaBLL.UpdateWPQMWeldProQua(wPQMWeldProQua, wPQMWeldProQua.Code.Trim());
        }
    }
}