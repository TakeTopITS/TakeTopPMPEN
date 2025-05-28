using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTUserInfor_StudentCostList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserCode"] != null)
        {
            strUserCode = Session["UserCode"].ToString();
        }


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthoritySuperUser(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;


            DataBinder(LB_BelongDepartCode.Text.Trim());
        }
    }


    //    private void DataBinder()
    //    {
    //        string strHQL;


    //        strHQL = string.Format(@"select s.*,m.ClassName from T_ProjectMemberStudent s
    //                    left join T_ProjectMemberClass  m on s.ClassID = m.ID
    //                    left join T_ProjectMemberGrade g on m.GradeID = g.ID
    //                    where g.DepartCode in {0} Order By s.UserCode DESC ", LB_DepartString.Text);

    //        DataTable dtStudent = ShareClass.GetDataSetFromSql(strHQL, "Student").Tables[0];

    //        DG_List.DataSource = dtStudent;
    //        DG_List.DataBind();

    //        LB_Sql.Text = strHQL;
    //    }

    private void DataBinder(string strBelongDepartCode)
    {
        string strHQL;

        if (strBelongDepartCode == "")
        {
            strHQL = string.Format(@"select s.*,m.ClassName from T_ProjectMemberStudent s
                    , T_ProjectMemberClass  m ,
                     T_ProjectMemberGrade g where s.ClassID =  m.ID and m.GradeID = g.ID
                                        and (g.DepartCode in
                    {0} 
                    or 
                    s.CreatUserCode = '{1}') Order By s.UserCode DESC", LB_DepartString.Text, strUserCode);
        }
        else
        {
            strHQL = string.Format(@"select s.*,m.ClassName from T_ProjectMemberStudent s
                    , T_ProjectMemberClass  m ,
                     T_ProjectMemberGrade g where s.ClassID =  m.ID and m.GradeID = g.ID
                                        and (g.DepartCode in
                    {0} 
                    or 
                    s.CreatUserCode = '{1}') and g.DepartCode = '{2}'  Order By s.UserCode DESC", LB_DepartString.Text, strUserCode, strBelongDepartCode);
        }

        DataTable dtStudent = ShareClass.GetDataSetFromSql(strHQL, "Student").Tables[0];

        DG_List.DataSource = dtStudent;
        DG_List.DataBind();

        LB_Sql.Text = strHQL;

        LB_StudentNumber.Text = LanguageHandle.GetWord("XueShengShu").ToString().Trim() + dtStudent.Rows.Count.ToString();
    }


    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strStudentCode, strStudentName;
        strStudentCode = "'" + "%" + TB_UserCode.Text.Trim() + "%" + "'";
        strStudentName = "'" + "%" + TB_UserName.Text.Trim() + "%" + "'";

        strHQL = string.Format(@"select s.*,m.ClassName from T_ProjectMemberStudent s
                    , T_ProjectMemberClass  m ,
                     T_ProjectMemberGrade g where s.ClassID =  m.ID and m.GradeID = g.ID
                                        and (g.DepartCode in
                    {0} 
                    or 
                    s.CreatUserCode = '{1}') and ( s.UserCode Like {2} and s.UserName Like {3} ) Order By s.UserCode DESC", LB_DepartString.Text, strUserCode, strStudentCode, strStudentName);

        DataTable dtStudent = ShareClass.GetDataSetFromSql(strHQL, "Student").Tables[0];

        DG_List.DataSource = dtStudent;
        DG_List.DataBind();

        LB_Sql.Text = strHQL;

        LB_StudentNumber.Text = LanguageHandle.GetWord("XueShengShu").ToString().Trim() + dtStudent.Rows.Count.ToString();
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

            LB_BelongDepartCode.Text = strDepartCode;
            LB_BelongDepartName.Text = strDepartName;

            DataBinder(strDepartCode);
        }
    }



    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "Finished")
        {
            string strCmdArgu = e.CommandArgument.ToString();

            ProjectMemberStudentCostBLL projectMemberStudentCostBLL = new ProjectMemberStudentCostBLL();
            string strProjectMemberStudentCostHQL = string.Format(@"from ProjectMemberStudentCost as projectMemberStudentCost where ID = " + strCmdArgu);
            IList lstProjectMemberStudentCost = projectMemberStudentCostBLL.GetAllProjectMemberStudentCosts(strProjectMemberStudentCostHQL);
            if (lstProjectMemberStudentCost != null && lstProjectMemberStudentCost.Count > 0)
            {
                ProjectMemberStudentCost projectMemberStudentCost = (ProjectMemberStudentCost)lstProjectMemberStudentCost[0];

                projectMemberStudentCost.Status = "FINISHED";

                projectMemberStudentCostBLL.UpdateProjectMemberStudentCost(projectMemberStudentCost, int.Parse(strCmdArgu));


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        DataTable dtStudent = ShareClass.GetDataSetFromSql(strHQL, "Student").Tables[0];

        DG_List.DataSource = dtStudent;
        DG_List.DataBind();
    }



}