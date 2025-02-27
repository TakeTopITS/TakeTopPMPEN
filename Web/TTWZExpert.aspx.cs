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


public partial class TTWZExpert : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "专家库档案", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);

            LoadWZExpertList(txt_ExpertInfo.Text.Trim(), ddlType.SelectedValue.Trim());

            LoadBMBidType();
        }
    }

    protected void LoadWZExpertList(string strExpertInfo, string strType)
    {
        string strHQL = "Select *,(ExpertType || ';' || ExpertType2) AS AllExpertType From T_WZExpert Where 1=1 ";
        if (!string.IsNullOrEmpty(strExpertInfo))
        {
            strHQL += " and (ExpertCode like '%" + strExpertInfo + "%' or JobTitle like '%" + strExpertInfo + "%' or ExpertType like '%" + strExpertInfo + "%' or ExpertType2 like '%" + strExpertInfo + "%' or Name like '%" + strExpertInfo + "%' or WorkUnit like '%" + strExpertInfo + "%' " +
                "or Job like '%" + strExpertInfo + "%') ";
        }
        if (strType.Trim() != "PleaseSelect")
        {
            strHQL += " and Type = '" + strType + "' ";
        }
        strHQL += " Order By ExpertCode DESC ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert");

        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
        lbl_sql.Text = strHQL;
    }

    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "from WZExpert as wZExpert where wZExpert.ID = '" + strID + "' ";
                WZExpertBLL wZExpertBLL = new WZExpertBLL();
                lst = wZExpertBLL.GetAllWZExperts(strHQL);
                if (lst != null && lst.Count > 0)
                {
                    WZExpert wZExpert = (WZExpert)lst[0];
                    TB_ExpertCode.Text = wZExpert.ExpertCode.Trim();

                    TB_ExpertType.Text = wZExpert.ExpertType.Trim();
                    TB_ExpertType2.Text = wZExpert.ExpertType2.Trim();
                    TB_Job.Text = wZExpert.Job.Trim();
                    TB_JobTitle.Text = wZExpert.JobTitle.Trim();
                    TB_Phone.Text = wZExpert.Phone.Trim();
                    LB_Name.Text = wZExpert.Name.Trim();
                    lbl_WorkUnit.Text = wZExpert.WorkUnit.Trim();
                    lbl_ID.Text = wZExpert.ID.ToString();

                    TB_ActionOutstanding.Text = wZExpert.ActionOutstanding.Trim();
                    TB_BadTrackRecord.Text = wZExpert.BadTrackRecord.Trim();
                    TB_EngagedCategory.Text = wZExpert.EngagedCategory.Trim();
                    TB_GoodPerformance.Text = wZExpert.GoodPerformance.Trim();
                    TB_LaborExpertise.Text = wZExpert.LaborExpertise.Trim();
                    TB_LiteratureWorks.Text = wZExpert.LiteratureWorks.Trim();
                    TB_ManagementInnovation.Text = wZExpert.ManagementInnovation.Trim();
                    TB_NotLaborExpertise.Text = wZExpert.NotLaborExpertise.Trim();
                    TB_PatentInvention.Text = wZExpert.PatentInvention.Trim();
                    ddl_ProcurementCategory.SelectedValue = wZExpert.ProcurementCategory.Trim();
                    TB_ScientificAchieve.Text = wZExpert.ScientificAchieve.Trim();
                    TB_SuccessfulCasePro.Text = wZExpert.SuccessfulCasePro.Trim();

                    ddl_Type.SelectedValue = wZExpert.Type;

                    //if (wZExpert.CreateCode.Trim() == strUserCode.Trim())
                    //{
                    //    BT_Update.Visible = true;
                    //    BT_Delete.Visible = true;
                    //    BT_Delete.Enabled = true;
                    //    BT_Update.Enabled = true;
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
    }

    protected void DataGrid3_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        ProjectMember projectMember = GetUserName(strUserCode);
        if (projectMember != null)
        {
            if (projectMember.UserType.Trim().ToUpper() == "INNER")
            {
                LB_Name.Text = projectMember.UserName.Trim();
                TB_Job.Text = projectMember.Duty.Trim();
                TB_JobTitle.Text = projectMember.JobTitle == null ? "" : projectMember.JobTitle.Trim(); //projectMember.WorkType.Trim();
                TB_Phone.Text = projectMember.MobilePhone.Trim();
                TB_ExpertType.Text = projectMember.WorkScope == null ? "" : projectMember.WorkScope.Trim();
                lbl_WorkUnit.Text = projectMember.DepartName.Trim();
                TB_ExpertCode.Text = strUserCode;
            }
            else//OUTER
            {
                LB_Name.Text = "";
                TB_Job.Text = "";
                TB_JobTitle.Text = "";
                TB_Phone.Text = "";
                TB_ExpertType.Text = "";
                lbl_WorkUnit.Text = "";
                TB_ExpertCode.Text = "";

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGRYWWBRYBNWNBZJJC").ToString().Trim() + "')", true);
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;
        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;
        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

    }

    protected void DataGrid1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        string strHQL = lbl_sql.Text.Trim();
        DataGrid1.CurrentPageIndex = e.NewPageIndex;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BookInformation");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        LoadWZExpertList(txt_ExpertInfo.Text.Trim(), ddlType.SelectedValue.Trim());
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        lbl_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = lbl_ID.Text.Trim();

        if (strID == "")
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
        if (TB_ExpertCode.Text.Trim() == "" || string.IsNullOrEmpty(TB_ExpertCode.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZJBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_ExpertCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            return;
        }
        if (IsWZExpert(TB_ExpertCode.Text.Trim(), ddl_Type.SelectedValue.Trim(), string.Empty))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGZJYCZJC").ToString().Trim() + "')", true);
            TB_ExpertCode.Focus();
            ddl_Type.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            return;
        }
        WZExpertBLL wZExpertBLL = new WZExpertBLL();
        WZExpert wZExpert = new WZExpert();
        wZExpert.Type = ddl_Type.SelectedValue.Trim();
        wZExpert.WorkingPoint = 0;
        wZExpert.WorkUnit = lbl_WorkUnit.Text.Trim();
        wZExpert.Phone = TB_Phone.Text.Trim();
        wZExpert.Name = LB_Name.Text.Trim();
        wZExpert.JobTitle = TB_JobTitle.Text.Trim();
        wZExpert.Job = TB_Job.Text.Trim();
        wZExpert.ExpertType = TB_ExpertType.Text.Trim();
        wZExpert.ExpertType2 = TB_ExpertType2.Text.Trim();
        wZExpert.ExpertCode = TB_ExpertCode.Text.Trim();
        wZExpert.CreateTime = DateTime.Now;
        wZExpert.CreateCode = strUserCode.Trim();
        wZExpert.ActionOutstanding = TB_ActionOutstanding.Text.Trim();
        wZExpert.BadTrackRecord = TB_BadTrackRecord.Text.Trim();
        wZExpert.EngagedCategory = TB_EngagedCategory.Text.Trim();
        wZExpert.GoodPerformance = TB_GoodPerformance.Text.Trim();
        wZExpert.LaborExpertise = TB_LaborExpertise.Text.Trim();
        wZExpert.LiteratureWorks = TB_LiteratureWorks.Text.Trim();
        wZExpert.ManagementInnovation = TB_ManagementInnovation.Text.Trim();
        wZExpert.NotLaborExpertise = TB_NotLaborExpertise.Text.Trim();
        wZExpert.PatentInvention = TB_PatentInvention.Text.Trim();
        wZExpert.ProcurementCategory = ddl_ProcurementCategory.SelectedValue.Trim();
        wZExpert.ScientificAchieve = TB_ScientificAchieve.Text.Trim();
        wZExpert.SuccessfulCasePro = TB_SuccessfulCasePro.Text.Trim();

        try
        {
            wZExpertBLL.AddWZExpert(wZExpert);
            lbl_ID.Text = GetWZExpertID();

            //BT_Delete.Visible = true;
            //BT_Delete.Enabled = true;
            //BT_Update.Visible = true;
            //BT_Update.Enabled = true;

            LoadWZExpertList(txt_ExpertInfo.Text.Trim(), ddlType.SelectedValue.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void Update()
    {
        if (TB_ExpertCode.Text.Trim() == "" || string.IsNullOrEmpty(TB_ExpertCode.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZJBNWKCZSBJC").ToString().Trim() + "')", true);
            TB_ExpertCode.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            return;
        }
        if (IsWZExpert(TB_ExpertCode.Text.Trim(), ddl_Type.SelectedValue.Trim(), lbl_ID.Text.Trim()))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGZJYCZJC").ToString().Trim() + "')", true);
            TB_ExpertCode.Focus();
            ddl_Type.Focus();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            return;
        }
        if (lbl_ID.Text.Trim() == "" || string.IsNullOrEmpty(lbl_ID.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGZJSJBCZJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            return;
        }
        WZExpertBLL wZExpertBLL = new WZExpertBLL();
        string strHQL = "from WZExpert as wZExpert where wZExpert.ID = '" + lbl_ID.Text.Trim() + "' ";
        IList lst = wZExpertBLL.GetAllWZExperts(strHQL);
        if (lst != null && lst.Count > 0)
        {
            WZExpert wZExpert = (WZExpert)lst[0];
            wZExpert.Type = ddl_Type.SelectedValue.Trim();
            wZExpert.WorkUnit = lbl_WorkUnit.Text.Trim();
            wZExpert.Phone = TB_Phone.Text.Trim();
            wZExpert.Name = LB_Name.Text.Trim();
            wZExpert.JobTitle = TB_JobTitle.Text.Trim();
            wZExpert.Job = TB_Job.Text.Trim();
            wZExpert.ExpertType = TB_ExpertType.Text.Trim();
            wZExpert.ExpertType2 = TB_ExpertType2.Text.Trim();
            wZExpert.ExpertCode = TB_ExpertCode.Text.Trim();
            wZExpert.ActionOutstanding = TB_ActionOutstanding.Text.Trim();
            wZExpert.BadTrackRecord = TB_BadTrackRecord.Text.Trim();
            wZExpert.EngagedCategory = TB_EngagedCategory.Text.Trim();
            wZExpert.GoodPerformance = TB_GoodPerformance.Text.Trim();
            wZExpert.LaborExpertise = TB_LaborExpertise.Text.Trim();
            wZExpert.LiteratureWorks = TB_LiteratureWorks.Text.Trim();
            wZExpert.ManagementInnovation = TB_ManagementInnovation.Text.Trim();
            wZExpert.NotLaborExpertise = TB_NotLaborExpertise.Text.Trim();
            wZExpert.PatentInvention = TB_PatentInvention.Text.Trim();
            wZExpert.ProcurementCategory = ddl_ProcurementCategory.SelectedValue.Trim();
            wZExpert.ScientificAchieve = TB_ScientificAchieve.Text.Trim();
            wZExpert.SuccessfulCasePro = TB_SuccessfulCasePro.Text.Trim();

            try
            {
                wZExpertBLL.UpdateWZExpert(wZExpert, wZExpert.ID);

                //BT_Delete.Visible = true;
                //BT_Delete.Enabled = true;
                //BT_Update.Visible = true;
                //BT_Update.Enabled = true;

                LoadWZExpertList(txt_ExpertInfo.Text.Trim(), ddlType.SelectedValue.Trim());

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJGXCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJGXSB").ToString().Trim() + "')", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);

            }
        }
    }

    protected void Delete()
    {
        string strHQL;
        string strCode = lbl_ID.Text.Trim();
        if (lbl_ID.Text.Trim() == "" || string.IsNullOrEmpty(lbl_ID.Text))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGGZJSJBCZJC").ToString().Trim() + "')", true);

            return;
        }
        strHQL = "Delete From T_WZExpert Where ID = '" + strCode + "' ";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            //BT_Update.Visible = false;
            //BT_Delete.Visible = false;
            //BT_Delete.Enabled = false;
            //BT_Update.Enabled = false;

            LoadWZExpertList(txt_ExpertInfo.Text.Trim(), ddlType.SelectedValue.Trim());

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected ProjectMember GetUserName(string strUserCode)
    {
        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = '" + strUserCode + "' ";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            ProjectMember projectMember = (ProjectMember)lst[0];
            return projectMember;
        }
        else
            return null;
    }

    /// <summary>
    /// 新增或更新时，检查专家编码及专家类型是否存在，存在返回true；不存在返回false。
    /// </summary>
    /// <param name="strCode"></param>
    /// <param name="strType"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    protected bool IsWZExpert(string strCode, string strType, string strID)
    {
        bool flag = true;
        string strHQL;
        if (string.IsNullOrEmpty(strID))
        {
            strHQL = "Select ID From T_WZExpert Where ExpertCode='" + strCode + "' and Type='" + strType + "' ";
        }
        else
            strHQL = "Select ID From T_WZExpert Where ExpertCode='" + strCode + "' and Type='" + strType + "' and ID<>'" + strID + "'";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert").Tables[0];
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
    /// 新增时，获取表T_WZExpert中最大编号。
    /// </summary>
    /// <returns></returns>
    protected string GetWZExpertID()
    {
        string flag = string.Empty;
        string strHQL = "Select ID From T_WZExpert Order by ID Desc";
        DataTable dt = ShareClass.GetDataSetFromSql(strHQL, "T_WZExpert").Tables[0];
        if (dt.Rows.Count > 0 && dt != null)
        {
            flag = dt.Rows[0]["ID"].ToString().Trim();
        }
        else
        {
            flag = "0";
        }
        return flag;
    }

    protected void LoadBMBidType()
    {
        string strHQL;

        strHQL = "Select * From T_BMBidType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_BMBidType");

        ddl_Type.DataSource = ds;
        ddl_Type.DataBind();
    }

}