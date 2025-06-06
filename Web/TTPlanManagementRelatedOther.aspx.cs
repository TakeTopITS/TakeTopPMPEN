using System; using System.Resources;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTPlanManagementRelatedOther : System.Web.UI.Page
{
    string strUserCode;
    //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)2013-5-18
    string strRelatedType, strRelatedID, strRelatedCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        //ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","我的计划", strUserCode);
        //if (blVisible == false)
        //{
        //    Response.Redirect("TTDisplayErrors.aspx");
        //    return;
        //}

        //this.Title = "我的计划";

        //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)2013-5-18
        strRelatedType = Request.QueryString["RelatedType"];
        if (strRelatedType == null)
        {
            strRelatedType = "OTHER";
        }

        strRelatedID = Request.QueryString["RelatedID"];
        if (strRelatedID == null)
        {
            strRelatedID = "0";
        }

        strRelatedCode = Request.QueryString["RelatedCode"];
        if (strRelatedCode == null)
        {
            strRelatedCode = "";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            PlanBLL planBLL = new PlanBLL();

            strHQL = "from Plan as plan where plan.PlanID in ";
            strHQL += " (Select planRelatedLeader.PlanID From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + " and planRelatedLeader.Status = 'New')";
            strHQL += " and plan.Status not in ('New','Deleted','Archived') ";
            if (!string.IsNullOrEmpty(strRelatedCode))
            {
                strHQL += " and plan.RelatedCode = '" + strRelatedCode + "'";
            }
            strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
            lst = planBLL.GetAllPlans(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();
            LB_Sql4.Text = strHQL;

            strHQL = "from Plan as plan where plan.PlanID in ";
            strHQL += " (Select planRelatedLeader.PlanID From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + " and planRelatedLeader.Status ='Approved')";
            strHQL += " and plan.Status not in ('New','Deleted','Archived') ";
            if (!string.IsNullOrEmpty(strRelatedCode))
            {
                strHQL += " and plan.RelatedCode = '" + strRelatedCode + "'";
            }
            strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
            lst = planBLL.GetAllPlans(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();
            LB_Sql1.Text = strHQL;

            strHQL = "from Plan as plan where plan.PlanID in ";
            strHQL += " (Select planRelatedLeader.PlanID From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + " and planRelatedLeader.Status = 'Completed')";
            strHQL += " and plan.Status not in ('New','Deleted','Archived') ";
            if (!string.IsNullOrEmpty(strRelatedCode))
            {
                strHQL += " and plan.RelatedCode = '" + strRelatedCode + "'";
            }
            strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
            lst = planBLL.GetAllPlans(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
            LB_Sql2.Text = strHQL;


            strHQL = "from Plan as plan where plan.UserCode = " + "'" + strUserCode + "'";
            strHQL += " and plan.Status not in ('Deleted','Archived') ";
            if (!string.IsNullOrEmpty(strRelatedCode))
            {
                strHQL += " and plan.RelatedCode = '" + strRelatedCode + "'";
            }
            strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
            lst = planBLL.GetAllPlans(strHQL);

            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
            LB_Sql3.Text = strHQL;
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql1.Text;
        IList lst;

        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql2.Text;
        IList lst;

        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql4.Text;
        IList lst;

        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;
        IList lst;

        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void BT_MakePlan_Click(object sender, EventArgs e)
    {
        Response.Redirect("TTMakePlan.aspx?RelatedType=WORKFLOW&RelatedID=0&RelatedCode=" + strRelatedCode);
    }

    protected void BT_AllPlan_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        PlanBLL planBLL = new PlanBLL();

        strHQL = "from Plan as plan where plan.PlanID in ";
        strHQL += " (Select planRelatedLeader.PlanID From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + " and planRelatedLeader.Status = 'New')";
        if (!string.IsNullOrEmpty(strRelatedCode))
        {
            strHQL += " and plan.RelatedCode = '" + strRelatedCode + "'";
        }
        strHQL += " Order By plan.PlanID DESC";
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
        LB_Sql4.Text = strHQL;

        strHQL = "from Plan as plan where plan.PlanID in ";
        strHQL += " (Select planRelatedLeader.PlanID From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + " and planRelatedLeader.Status ='Approved')";
        if (!string.IsNullOrEmpty(strRelatedCode))
        {
            strHQL += " and plan.RelatedCode = '" + strRelatedCode + "'";
        }
        strHQL += " Order By plan.PlanID DESC";
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
        LB_Sql1.Text = strHQL;

        strHQL = "from Plan as plan where plan.PlanID in ";
        strHQL += " (Select planRelatedLeader.PlanID From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.LeaderCode = " + "'" + strUserCode + "'" + " and planRelatedLeader.Status = 'Completed')";
        if (!string.IsNullOrEmpty(strRelatedCode))
        {
            strHQL += " and plan.RelatedCode = '" + strRelatedCode + "'";
        }
        strHQL += " Order By plan.PlanID DESC";
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
        LB_Sql2.Text = strHQL;


        strHQL = "from Plan as plan where plan.UserCode = " + "'" + strUserCode + "'";
        if (!string.IsNullOrEmpty(strRelatedCode))
        {
            strHQL += " and plan.RelatedCode = '" + strRelatedCode + "'";
        }
        strHQL += " Order By plan.PlanID DESC";
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
        LB_Sql3.Text = strHQL;
    }
}