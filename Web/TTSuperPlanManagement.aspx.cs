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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTSuperPlanManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //钟礼月作品（jack.erp@gmail.com)
        //泰顶软件2006－2012

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx","全局计划管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); 
        if (Page.IsPostBack == false)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = GetDepartName(strDepartCode);

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string strOperatorName = GetUserName(strOperatorCode);

        LB_OperatorCode.Text = strOperatorCode;
        LB_OperatorName.Text = strOperatorName;


        ShareClass.InitialPlanTreeByUserCode(TreeView2, strOperatorCode,"OTHER");
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strPlanID, strPlanName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target == "0")
        {
            strPlanID = treeNode.Target.Trim();
            strPlanName = LanguageHandle.GetWord("WoDeJiHua");
        }
        else
        {
            strPlanID = treeNode.Target.Trim();
            strPlanName = treeNode.Text.Trim();
        }

        NB_PlanID.Amount = decimal.Parse(strPlanID);

        LoadPlan(strPlanID);
        LoadPlanWorkLog(strPlanID);
        LoadPlanTarget(strPlanID);
        LoadPlanRelatedLeaderRecord(strPlanID);        
    }

    protected void BT_FindPlan_Click(object sender, EventArgs e)
    {
        string strPlanID;

        strPlanID = int.Parse(NB_PlanID.Amount.ToString()).ToString();

        LoadPlan(strPlanID);
        LoadPlanWorkLog(strPlanID);
        LoadPlanTarget(strPlanID);
        LoadPlanRelatedLeaderRecord(strPlanID);        
    }

    protected void BT_DeletePlan_Click(object sender, EventArgs e)
    {
        string strPlanID;
        string strHQL;

        strPlanID = int.Parse(NB_PlanID.Amount.ToString()).ToString();

        strHQL = "Select * From T_Plan Where PlanID in (Select ParentID From T_Plan) and PlanID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Plan");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSBCZZJHBNSCJC")+"')", true);
            return;
        }

        try
        {
            strHQL = "Delete From T_Plan Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_Plan_WorkLog Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_Plan_Target Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_Plan_RelatedLeader Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_Plan_LeaderReview Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            LoadPlan(strPlanID);
            LoadPlanWorkLog(strPlanID);
            LoadPlanTarget(strPlanID);
            LoadPlanRelatedLeaderRecord(strPlanID);

            try
            {
                ShareClass.InitialPlanTreeByUserCode(TreeView2, LB_OperatorCode.Text, "OTHER");
            }
            catch
            {
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC")+"')", true);
        }        
    }


    protected void LoadPlan(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Plan as plan where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataList2.DataSource = lst;
        DataList2.DataBind();
    }

    protected void LoadPlanWorkLog(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanWorkLog as planWorkLog Where PlanID = " + strPlanID;
        strHQL += " Order By planWorkLog.ID DESC";
        PlanWorkLogBLL planWorkLogBLL = new PlanWorkLogBLL();
        lst = planWorkLogBLL.GetAllPlanWorkLogs(strHQL);

        DataList3.DataSource = lst;
        DataList3.DataBind();
    }

    protected void LoadPlanTarget(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanTarget as planTarget Where planTarget.PlanID = " + strPlanID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadPlanRelatedLeaderRecord(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanLeaderReview as planLeaderReview where planLeaderReview.PlanID = " + strPlanID;
        strHQL += " Order By planLeaderReview.ID DESC";
        PlanLeaderReviewBLL planLeaderReviewBLL = new PlanLeaderReviewBLL();
        lst = planLeaderReviewBLL.GetAllPlanLeaderReviews(strHQL);

        DataList1.DataSource = lst;
        DataList1.DataBind();
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName.Trim();

    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }
}
