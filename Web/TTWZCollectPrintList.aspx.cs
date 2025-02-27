using System;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;
using System.Collections;
using System.Data;
using System.Text;
using System.Drawing;

public partial class TTWZCollectPrintList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "期初数据导入", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataCompactBander();
            DataProjectBinder();

            DataCollectBinder();
        }
    }

    private void DataCollectBinder()
    {
        string strCollectCode = TXT_CollectCode.Text.Trim();
        string strProjectCode = DDL_Project.SelectedValue;
        string strCompactCode = DDL_Compact.SelectedValue;

        string strCollectHQL = @"select c.*,o.ObjectName,s.SupplierName,h.UserName as CheckerName,
                    a.UserName as SafekeeperName,n.UserName as ContacterName,
                    f.UserName as FinanceApproveName
                    from T_WZCollect c
                    left join T_WZObject o on c.ObjectCode = o.ObjectCode
                    left join T_WZSupplier s on c.SupplierCode = s.SupplierCode
                    left join T_ProjectMember h on c.Checker = h.UserCode
                    left join T_ProjectMember a on c.Safekeeper = a.UserCode
                    left join T_ProjectMember n on c.Contacter = n.UserCode 
                    left join T_ProjectMember f on c.FinanceApprove = f.UserCode
                    where 1=1 ";
        if (!string.IsNullOrEmpty(strCollectCode))
        {
            strCollectHQL += " and c.CollectCode = '" + strCollectCode + "'";
        }
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strCollectHQL += " and c.ProjectCode = '" + strProjectCode + "'";
        }
        if (!string.IsNullOrEmpty(strCompactCode))
        {
            strCollectHQL += " and c.CompactCode = '" + strCompactCode + "'";
        }
        strCollectHQL += "  order by c.TicketTime desc";
        DataTable dtCollect = ShareClass.GetDataSetFromSql(strCollectHQL, "Collect").Tables[0];

        DG_Collect.DataSource = dtCollect;
        DG_Collect.DataBind();
    }


    private void DataCompactBander()
    {
        WZCompactBLL wZCompactBLL = new WZCompactBLL();
        string strWZCompactHQL = string.Format("from WZCompact as wZCompact where Compacter = '{0}' and Progress = '材检' and CollectMoney > 0", strUserCode); 
        IList listCompact = wZCompactBLL.GetAllWZCompacts(strWZCompactHQL);

        DDL_Compact.DataSource = listCompact;
        DDL_Compact.DataBind();

        DDL_Compact.Items.Insert(0, new ListItem("", ""));
    }

    private void DataProjectBinder()
    {
        WZProjectBLL wZProjectBLL = new WZProjectBLL();
        string strProjectHQL = "from WZProject as wZProject order by MarkTime desc";
        IList listProject = wZProjectBLL.GetAllWZProjects(strProjectHQL);

        DDL_Project.DataSource = listProject;
        DDL_Project.DataBind();

        DDL_Project.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataCollectBinder();
    }



    protected void BT_Print_Click(object sender, EventArgs e)
    {
        string strCollectCodes = Request.Form["cb_Collect_Code"];
        if (!string.IsNullOrEmpty(strCollectCodes))
        {
            string strUrl = "TTWZCollectPrintPag.aspx?collectCode=" + strCollectCodes;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('" + strUrl + "');", true);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "window.open('" + strUrl + "')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZYDYDSLD").ToString().Trim() + "')", true);
            return;
        }
    }
}
