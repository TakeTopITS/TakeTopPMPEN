using System;
using System.Resources;
using System.Drawing;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using Stimulsoft.Report.Dictionary;
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using Stimulsoft.Report.Events;

public partial class TTMakeReport : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "分析模型设计", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView2, strUserCode);
            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT"), TreeView3, strUserCode);

            LoadReportType();
            if (Session["ReportType"] != null)
            {
                for (int i = 0; i < LLB_ReportType.Items.Count; i++)
                {
                    if (LLB_ReportType.Items[i].Value.Trim() == Session["ReportType"].ToString().Trim())
                    {
                        LLB_ReportType.Items[i].Selected = true;
                    }
                }

                LoadReportTemplateByType(Session["ReportType"].ToString());

                if (Session["ReportTemName"] == null)
                {
                    LoadReportByType(Session["ReportType"].ToString());
                }
            }

            if (Session["ReportTemName"] != null)
            {
                for (int i = 0; i < LLB_ReportTemplate.Items.Count; i++)
                {
                    if (LLB_ReportTemplate.Items[i].Text.Trim() == Session["ReportTemName"].ToString().Trim())
                    {
                        LLB_ReportTemplate.Items[i].Selected = true;

                        HL_ReportDesigner.Enabled = true;
                        HL_ReportDesigner.NavigateUrl = "TTReportDesigner.aspx?TemID=" + LLB_ReportTemplate.Items[i].Value + "&ReportID=0&DesignerType=" + DL_DesignerType.SelectedValue;

                        string strHQL = "From ReportTemplate as reportTemplate where reportTemplate.ID = " + LLB_ReportTemplate.Items[i].Value;
                        ReportTemplateBLL reportTemplateBLL = new ReportTemplateBLL();
                        IList lst = reportTemplateBLL.GetAllReportTemplates(strHQL);
                        ReportTemplate reportTemplate = (ReportTemplate)lst[0];

                        LB_TemID.Text = LLB_ReportTemplate.Items[i].Value;
                        TB_TemName.Text = Session["ReportTemName"].ToString();
                        TB_TemComment.Text = reportTemplate.TemComment.Trim();

                        LB_BelongDepartCode.Text = reportTemplate.BelongDepartCode.Trim();
                        LB_BelongDepartName.Text = reportTemplate.BelongDepartName.Trim();
                    }
                }

                LoadReportByTem(Session["ReportTemName"].ToString());
            }
        }
    }

    protected void LLB_ReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strReportType;

        strReportType = LLB_ReportType.SelectedValue.Trim();
        Session["ReportType"] = strReportType;

        LoadReportTemplateByType(strReportType);
        LoadReportByType(strReportType);
    }

    protected void LLB_ReportTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strTemID, strReportType, strTemName;

        strTemID = LLB_ReportTemplate.SelectedValue;
        strReportType = LLB_ReportType.SelectedValue.Trim();
        strTemName = LLB_ReportTemplate.SelectedItem.Text.Trim();

        Session["ReportTemName"] = strTemName;

        strHQL = "From ReportTemplate as reportTemplate where reportTemplate.ID = " + strTemID;
        ReportTemplateBLL reportTemplateBLL = new ReportTemplateBLL();
        lst = reportTemplateBLL.GetAllReportTemplates(strHQL);
        ReportTemplate reportTemplate = (ReportTemplate)lst[0];

        LB_TemID.Text = strTemID;
        TB_TemName.Text = strTemName;
        TB_TemComment.Text = reportTemplate.TemComment.Trim();

        LB_BelongDepartCode.Text = reportTemplate.BelongDepartCode.Trim();
        LB_BelongDepartName.Text = reportTemplate.BelongDepartName.Trim();

        DL_ReportType.SelectedValue = reportTemplate.ReportType;

        BT_UpdateTem.Enabled = true;
        BT_DeleteTem.Enabled = true;
        HL_ReportDesigner.Enabled = true;

        LoadReportByTem(strTemName);

        HL_ReportDesigner.Enabled = true;
        HL_ReportDesigner.NavigateUrl = "TTReportDesigner.aspx?TemID=" + strTemID + "&ReportID=0&DesignerType=" + DL_DesignerType.SelectedValue;
    }

    protected void DL_DesignerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHref;

        strHref = HL_ReportDesigner.NavigateUrl.Trim();
        strHref = strHref.Replace("DesignerType=FLEX", "").Replace("DesignerType=HTML5", "");

        strHref += "DesignerType=" + DL_DesignerType.SelectedValue;

        HL_ReportDesigner.NavigateUrl = strHref;
    }
    protected void BT_Select_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popVisualWindow','true', 'popwindow') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;

        string strReportID, strReportURL;

        string strUserCode = Session["UserCode"].ToString();


        if (e.CommandName != "Page")
        {
            strReportID = e.Item.Cells[0].Text;

            if (e.CommandName == "Select")
            {
                LB_ReportID.Text = strReportID;

                TB_ReportName.Text = GetReportName(strReportID);

                for (int i = 0; i < DataGrid1.Items.Count; i++)
                {
                    DataGrid1.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                BT_Send.Enabled = true;
                TB_Message.Text = LanguageHandle.GetWord("JiTongTongZhiYouXinBaoBiao") + TB_ReportName.Text.Trim() + LanguageHandle.GetWord("QingJiShiYueDou");

                LoadReportRelatedUser(strReportID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popVisualWindow','false') ", true);
            }

            if (e.CommandName == "Open")
            {
                strHQL = "Select ReportURL From T_Report Where ID = " + strReportID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Reoprt");
                strReportURL = ds.Tables[0].Rows[0][0].ToString().Trim();

                if (strReportURL.IndexOf(".mrt") > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "popShowByURL('TTReportView.aspx?ReportID=" + strReportID + "', 'Report', 800, 600,window.location);", true);
                }
                else
                {
                    strReportURL = strReportURL.Replace(@"\", @"//");
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "popShowByURL('" + strReportURL + "', 'Report', 800, 600,window.location);", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                strHQL = " Delete from T_Report where ID = " + strReportID;
                ShareClass.RunSqlCommand(strHQL);

                BT_Send.Enabled = false;

                string strReportType = LLB_ReportType.SelectedItem.Text;
                try
                {
                    string strTemName = LLB_ReportTemplate.SelectedItem.Text;
                    if (strTemName != "")
                    {
                        LoadReportByTem(strTemName);
                    }
                    else
                    {
                        LoadReportByType(strReportType);
                    }
                }
                catch
                {
                    LoadReportByType(strReportType);
                }

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("XGKSRYYJQBQC") + "')", true);
            }
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        IList lst;

        ReportBLL reportBLL = new ReportBLL();
        lst = reportBLL.GetAllReports(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void BT_CopyTemplate_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strOldTemID, strNewTemName, strReportType;
        int intCount;

        strOldTemID = LLB_ReportTemplate.SelectedValue;
        strNewTemName = TB_NewTemName.Text.Trim();
        strReportType = DL_ReportType.SelectedValue.Trim();

        intCount = GetSameNameReportTemplateCount(strNewTemName);
        if (intCount > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJSBCZTMBBMBJC") + "')", true);
            return;
        }

        strHQL = string.Format(@"INSERT INTO T_ReportTemplate
           (ReportType
           ,TemName
           ,TemComment
           ,TemDefinition
           ,CreatorCode
           ,CreatorName
           ,CreateTime
           ,BelongDepartCode
           ,BelongDepartName)
        
         Select 
            '{0}'
           ,'{1}'
           ,TemComment
           ,TemDefinition
           ,CreatorCode
           ,CreatorName
           ,CreateTime
           ,BelongDepartCode
           ,BelongDepartName
		   From T_ReportTemplate Where ID = {2}", strReportType, strNewTemName, strOldTemID);


        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadReportTemplateByType(strReportType);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZSBJC") + "')", true);
        }

    }


    protected void BT_NewTem_Click(object sender, EventArgs e)
    {
        string strTemID, strType, strTemName, strTemComment;
        string strBelongDepartCode, strBelongDepartName;
        int intCount;

        strType = LLB_ReportType.SelectedValue.Trim();
        strTemName = TB_TemName.Text.Trim();
        strTemComment = TB_TemComment.Text.Trim();

        strBelongDepartCode = LB_BelongDepartCode.Text.Trim();
        strBelongDepartName = LB_BelongDepartName.Text.Trim();

        intCount = GetSameNameReportTemplateCount(strTemName);

        if (intCount > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJSBCZTMBBMBJC") + "')", true);
            return;
        }


        if (strType == "" | strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXJSBMBLXHMCBNWKJC") + "')", true);
            return;
        }
        else
        {
            ReportTemplateBLL reportTemplateBLL = new ReportTemplateBLL();
            ReportTemplate reportTemplate = new ReportTemplate();

            reportTemplate.ReportType = strType;
            reportTemplate.TemName = strTemName;
            reportTemplate.TemComment = strTemComment;
            reportTemplate.TemDefinition = "";
            reportTemplate.CreatorCode = strUserCode;
            reportTemplate.CreatorName = ShareClass.GetUserName(strUserCode);
            reportTemplate.CreateTime = DateTime.Now;

            reportTemplate.BelongDepartCode = strBelongDepartCode;
            reportTemplate.BelongDepartName = strBelongDepartName;

            try
            {
                reportTemplateBLL.AddReportTemplate(reportTemplate);

                strTemID = ShareClass.GetMyCreatedMaxReportTemplateID(strUserCode);
                LB_TemID.Text = strTemID;

                LoadReportTemplateByType(strType);

                BT_UpdateTem.Enabled = true;
                BT_DeleteTem.Enabled = true;

                HL_ReportDesigner.Enabled = true;
                HL_ReportDesigner.NavigateUrl = "TTReportDesigner.aspx?TemID=" + strTemID + "&ReportID=0";

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
            }
        }
    }

    protected void BT_UpdateTem_Click(object sender, EventArgs e)
    {
        string strID, strType, strTemName, strTemComment;
        string strBelongDepartCode, strBelongDepartName;

        string strHQL;
        IList lst;

        int intCount;

        strID = LB_TemID.Text.Trim();
        strType = LLB_ReportType.SelectedValue.Trim();
        strTemName = TB_TemName.Text.Trim();
        strTemComment = TB_TemComment.Text.Trim();

        strBelongDepartCode = LB_BelongDepartCode.Text.Trim();
        strBelongDepartName = LB_BelongDepartName.Text.Trim();

        intCount = GetSameNameReportTemplateCountByID(strTemName, strID);
        if (intCount > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBCZTMBBMBJC") + "')", true);
            return;
        }

        if (strType == "" | strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBMBLXHMCBNWKJC") + "')", true);
            return;
        }
        else
        {
            strHQL = "From ReportTemplate as reportTemplate where reportTemplate.ID = " + strID;
            ReportTemplateBLL reportTemplateBLL = new ReportTemplateBLL();
            lst = reportTemplateBLL.GetAllReportTemplates(strHQL);

            ReportTemplate reportTemplate = (ReportTemplate)lst[0];

            reportTemplate.ReportType = strType;
            reportTemplate.TemName = strTemName;
            reportTemplate.TemComment = strTemComment;

            reportTemplate.BelongDepartCode = strBelongDepartCode;
            reportTemplate.BelongDepartName = strBelongDepartName;

            try
            {
                reportTemplateBLL.UpdateReportTemplate(reportTemplate, int.Parse(strID));

                LoadReportTemplateByType(strType);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
            }
        }
    }

    protected void BT_DeleteTem_Click(object sender, EventArgs e)
    {
        string strID, strType;
        string strHQL;

        strID = LB_TemID.Text.Trim();
        strType = LLB_ReportType.SelectedValue.Trim();

        strHQL = " Delete From T_ReportTemplate Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LB_TemID.Text = "";

            LoadReportTemplateByType(strType);

            BT_UpdateTem.Enabled = false;
            BT_DeleteTem.Enabled = false;
            HL_ReportDesigner.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected void BT_UpdateReportName_Click(object sender, EventArgs e)
    {
        string strReportID, strReportName, strReportTemplate;

        string strHQL;

        strReportID = LB_ReportID.Text.Trim();
        strReportName = TB_ReportName.Text.Trim();

        strHQL = "Update T_Report Set ReportName = " + "'" + strReportName + "'" + " Where ID = " + strReportID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            string strReportType = LLB_ReportType.SelectedItem.Text;
            try
            {
                string strTemName = LLB_ReportTemplate.SelectedItem.Text;
                if (strTemName != "")
                {
                    LoadReportByTem(strTemName);
                }
                else
                {
                    LoadReportByType(strReportType);
                }
            }
            catch
            {
                LoadReportByType(strReportType);
            }

            TB_Message.Text = LanguageHandle.GetWord("JiTongTongZhiYouXinBaoBiao") + TB_ReportName.Text.Trim() + LanguageHandle.GetWord("QingJiShiYueDou");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGMCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGMSBJC") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popVisualWindow','false') ", true);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popVisualWindow','true', 'popwindow') ", true);
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            LB_BelongDepartCode.Text = strDepartCode;
            LB_BelongDepartName.Text = ShareClass.GetDepartName(strDepartCode);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strReportID, strOperatorCode;

        strReportID = LB_ReportID.Text.Trim();

        if (strReportID != "")
        {
            strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

            ReportRelatedUserBLL reportRelatedUserBLL = new ReportRelatedUserBLL();
            ReportRelatedUser reportRelatedUser = new ReportRelatedUser();

            reportRelatedUser.ReportID = int.Parse(strReportID);
            reportRelatedUser.UserCode = strOperatorCode;
            reportRelatedUser.UserName = ShareClass.GetUserName(strOperatorCode);
            reportRelatedUser.Status = "New";

            try
            {
                reportRelatedUserBLL.AddReportRelatedUser(reportRelatedUser);

                LoadReportRelatedUser(strReportID);
            }
            catch
            {
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZBBCNZJKSRY") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popVisualWindow','true', 'popwindow') ", true);
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

            string strHQL;

            string strReportID = LB_ReportID.Text.Trim();

            strHQL = "Delete From T_ReportRelatedUser Where ReportID = " + strReportID;
            strHQL += " and UserName = " + "'" + strUserName + "'";
            ShareClass.RunSqlCommand(strHQL);

            LoadReportRelatedUser(strReportID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popVisualWindow','false') ", true);
        }
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode, strReceiverCode;
        string strSubject, strMsg;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        string strReportID = LB_ReportID.Text.Trim();


        strHQL = "from ReportRelatedUser as reportRelatedUser where reportRelatedUser.ReportID = " + strReportID;
        ReportRelatedUserBLL reportRelatedUserBLL = new ReportRelatedUserBLL();
        lst = reportRelatedUserBLL.GetAllReportRelatedUsers(strHQL);
        ReportRelatedUser reportRelatedUser = new ReportRelatedUser();

        Msg msg = new Msg();

        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                reportRelatedUser = (ReportRelatedUser)lst[i];
                strReceiverCode = reportRelatedUser.UserCode.Trim();

                strMsg = TB_Message.Text.Trim();

                if (CB_MSM.Checked == true | CB_Mail.Checked == true)
                {
                    strSubject = LanguageHandle.GetWord("BaoBiaoYueDouTongZhi");

                    if (CB_MSM.Checked == true)
                    {
                        msg.SendMSM("Message", strReceiverCode, strMsg, strUserCode);
                    }

                    if (CB_Mail.Checked == true)
                    {
                        msg.SendMail(strReceiverCode, strSubject, strMsg, strUserCode);
                    }
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTZFSWB") + "')", true);
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popVisualWindow','false') ", true);
    }

    protected void LoadReportType()
    {
        string strHQL;
        IList lst;

        DataGrid1.CurrentPageIndex = 0;

        strHQL = "from ReportType as reportType Order By reportType.SortNumber ASC";
        ReportTypeBLL reportTypeBLL = new ReportTypeBLL();
        lst = reportTypeBLL.GetAllReportTypes(strHQL);

        LLB_ReportType.DataSource = lst;
        LLB_ReportType.DataBind();

        DL_ReportType.DataSource = lst;
        DL_ReportType.DataBind();
    }

    protected void LoadReportTemplateByType(string strReportType)
    {
        string strHQL;
        IList lst;

        DataGrid1.CurrentPageIndex = 0;

        strHQL = "From ReportTemplate as reportTemplate where reportTemplate.ReportType = " + "'" + strReportType + "'";
        strHQL += " and reportTemplate.BelongDepartCode In " + LB_DepartString.Text;
        strHQL += " Order By reportTemplate.ID DESC";

        ReportTemplateBLL reportTemplateBLL = new ReportTemplateBLL();
        lst = reportTemplateBLL.GetAllReportTemplates(strHQL);

        LLB_ReportTemplate.DataSource = lst;
        LLB_ReportTemplate.DataBind();
    }

    protected void LoadReportByTem(string strReportTemplate)
    {
        string strHQL;
        IList lst;

        DataGrid1.CurrentPageIndex = 0;

        strHQL = "From Report as report Where report.TemName = " + "'" + strReportTemplate + "'";
        strHQL += " and report.TemName in (Select reportTemplate.TemName From ReportTemplate as reportTemplate Where reportTemplate.BelongDepartCode In " + LB_DepartString.Text + ")";
        strHQL += " Order By report.ID DESC";

        ReportBLL reportBLL = new ReportBLL();
        lst = reportBLL.GetAllReports(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadReportByType(string strReportType)
    {
        string strHQL;
        IList lst;

        DataGrid1.CurrentPageIndex = 0;

        strHQL = "From Report as report Where report.ReportType = " + "'" + strReportType + "'";
        strHQL += " and report.TemName in (Select reportTemplate.TemName From ReportTemplate as reportTemplate Where reportTemplate.BelongDepartCode In " + LB_DepartString.Text + ")";
        strHQL += " Order By report.ID DESC";

        ReportBLL reportBLL = new ReportBLL();
        lst = reportBLL.GetAllReports(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadReportRelatedUser(string strReportID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ReportRelatedUser as reportRelatedUser where reportRelatedUser.ReportID = " + strReportID;
        ReportRelatedUserBLL reportRelatedUserBLL = new ReportRelatedUserBLL();
        lst = reportRelatedUserBLL.GetAllReportRelatedUsers(strHQL);

        RP_ReportRelatedUser.DataSource = lst;
        RP_ReportRelatedUser.DataBind();
    }

    protected string GetReportCreatorCode(string strReportID)
    {
        string strHQL;
        IList lst;

        strHQL = "From Report as report Where report.ID = " + strReportID;

        ReportBLL reportBLL = new ReportBLL();
        lst = reportBLL.GetAllReports(strHQL);
        Report report = (Report)lst[0];

        return report.CreatorCode.Trim();
    }

    protected string GetReportName(string strReportID)
    {
        string strHQL;
        IList lst;

        strHQL = "From Report as report Where report.ID = " + strReportID;

        ReportBLL reportBLL = new ReportBLL();
        lst = reportBLL.GetAllReports(strHQL);
        Report report = (Report)lst[0];

        return report.ReportName.Trim();
    }

    protected int GetSameNameReportTemplateCount(string strTemName)
    {
        string strHQL;
        IList lst;

        strHQL = "From ReportTemplate as reportTemplate where reportTemplate.TemName = " + "'" + strTemName + "'";
        ReportTemplateBLL reportTemplateBLL = new ReportTemplateBLL();
        lst = reportTemplateBLL.GetAllReportTemplates(strHQL);

        return lst.Count;
    }

    protected int GetSameNameReportTemplateCountByID(string strTemName, string strID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ReportTemplate as reportTemplate where reportTemplate.TemName = " + "'" + strTemName + "'" + " and reportTemplate.ID != " + strID;
        ReportTemplateBLL reportTemplateBLL = new ReportTemplateBLL();
        lst = reportTemplateBLL.GetAllReportTemplates(strHQL);

        return lst.Count;
    }

}