using System; using System.Resources;
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
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using TakeTopSecurity;

public partial class TTUserInfor_StudentCost : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);


        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentStringByAuthoritySuperUser(strUserCode);
            LB_DepartString.Text = strDepartString;

            DataGradeBinder();
        }
    }



    protected void LoadProjectMemberStudentCostInfo(int intGradeID, int intClassID)
    {
        string strHQL = string.Format(@"select c.* from T_ProjectMemberStudentCost c
                    left join T_ProjectMemberStudent s on c.StudentCode = s.UserCode
                    left join T_DepartmentUser u on c.CreatUserCode = u.UserCode
                    where s.ClassID={0}
                    and (u.DepartCode in {1}
                    or c.CreatUserCode = '{2}') ", intClassID, LB_DepartString.Text, strUserCode);

        string strName = txt_Name.Text.Trim();
        if (!string.IsNullOrEmpty(strName))
        {
            strHQL += " and s.StudentName like '%" + strName + "%'";
        }

        strHQL += " Order By c.StudentCode DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "ProjectMemberStudentCost");

        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }



    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strGradeID = DDL_ProjectMemberGrade.SelectedValue;
        string strClassID = DDL_ProjectMemberClass.SelectedValue;

        if (string.IsNullOrEmpty(strGradeID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZNJ").ToString().Trim()+"')", true);
            return;
        }
        if (string.IsNullOrEmpty(strClassID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZBJ").ToString().Trim()+"')", true);
            return;
        }

        int intGradeID = 0;
        int.TryParse(strGradeID, out intGradeID);
        int intClassID = 0;
        int.TryParse(strClassID, out intClassID);

        string strHQL = string.Format(@"select * from T_ProjectMemberStudent
                    where ClassID  ={0} ", intClassID);
        
        DataTable dtStudent = ShareClass.GetDataSetFromSql(strHQL, "ProjectMemberStudent").Tables[0];
        if (dtStudent != null && dtStudent.Rows.Count > 0)
        {
            //先删除学生收费记录
            string strDeleteStudentSQL = "delete T_ProjectMemberStudentCost where ClassID = " + intClassID;
            ShareClass.RunSqlCommand(strDeleteStudentSQL);

            ProjectMemberStudentCostBLL projectMemberStudentCostBLL = new ProjectMemberStudentCostBLL();
            foreach (DataRow drStudent in dtStudent.Rows)
            {
                ProjectMemberStudentCost projectMemberStudentCost = new ProjectMemberStudentCost();

                //projectMemberStudentCost.GradeID = intGradeID;
                //projectMemberStudentCost.ClassID = intClassID;
                //projectMemberStudentCost.UserCode = ShareClass.ObjectToString(drStudent["UserCode"]);
                //projectMemberStudentCost.UserName = ShareClass.ObjectToString(drStudent["UserName"]);

                //decimal decimalWangFeePerSemester = 0;
                //decimal.TryParse(ShareClass.ObjectToString(drStudent["WangFeePerSemester"]), out decimalWangFeePerSemester);
                //projectMemberStudentCost.WangFeePerSemester = decimalWangFeePerSemester;

                //decimal decimalMeals = 0;
                //decimal.TryParse(ShareClass.ObjectToString(drStudent["Meals"]), out decimalMeals);
                //projectMemberStudentCost.Meals = decimalMeals;

                //decimal decimalActivityCost = 0;
                //decimal.TryParse(ShareClass.ObjectToString(drStudent["ActivityCost"]), out decimalActivityCost);
                //projectMemberStudentCost.ActivityCost = decimalActivityCost;

                //decimal decimalCustodyAfterClass = 0;
                //decimal.TryParse(ShareClass.ObjectToString(drStudent["CustodyAfterClass"]), out decimalCustodyAfterClass);
                //projectMemberStudentCost.CustodyAfterClass = decimalCustodyAfterClass;

                //decimal decimalReplaceCosts = 0;
                //decimal.TryParse(ShareClass.ObjectToString(drStudent["ReplaceCosts"]), out decimalReplaceCosts);
                //projectMemberStudentCost.ReplaceCosts = decimalReplaceCosts;

                projectMemberStudentCost.CreatUserCode = strUserCode;

                projectMemberStudentCostBLL.AddProjectMemberStudentCost(projectMemberStudentCost);
            }

            //重新加载
            LoadProjectMemberStudentCostInfo(intGradeID, intClassID);
        }
        else {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZWSJL").ToString().Trim()+"')", true);
            return;
        }
    }

    protected void BT_Query_Click(object sender, EventArgs e)
    {
        string strGradeID = DDL_ProjectMemberGrade.SelectedValue;
        string strClassID = DDL_ProjectMemberClass.SelectedValue;

        int intGradeID = 0;
        int.TryParse(strGradeID, out intGradeID);
        int intClassID = 0;
        int.TryParse(strClassID, out intClassID);

        LoadProjectMemberStudentCostInfo(intGradeID, intClassID);
    }


    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strGradeID = DDL_ProjectMemberGrade.SelectedValue;
        string strClassID = DDL_ProjectMemberClass.SelectedValue;

        if (string.IsNullOrEmpty(strGradeID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZNJ").ToString().Trim()+"')", true);
            return;
        }
        if (string.IsNullOrEmpty(strClassID))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZBJ").ToString().Trim()+"')", true);
            return;
        }

        int intGradeID = 0;
        int.TryParse(strGradeID, out intGradeID);
        int intClassID = 0;
        int.TryParse(strClassID, out intClassID);

        LoadProjectMemberStudentCostInfo(intGradeID, intClassID);
    }

    
    private void DataGradeBinder()
    {
        string strGradeHQL = string.Format(@"select * from T_ProjectMemberGrade where UserCode = '{0}'", strUserCode);
        DataTable dtGrade = ShareClass.GetDataSetFromSql(strGradeHQL, "Grade").Tables[0];

        DDL_ProjectMemberGrade.DataSource = dtGrade;

        DDL_ProjectMemberGrade.DataTextField = "GradeName";
        DDL_ProjectMemberGrade.DataValueField = "ID";

        DDL_ProjectMemberGrade.DataBind();

        DDL_ProjectMemberGrade_SelectedIndexChanged(null, null);
    }



    private void DataClassBinder(int intGradeID)
    {
        string strClassHQL = string.Format(@"select * from T_ProjectMemberClass where GradeID = {0} and UserCode = '{1}'", intGradeID, strUserCode);
        DataTable dtClass = ShareClass.GetDataSetFromSql(strClassHQL, "class").Tables[0];

        DDL_ProjectMemberClass.DataSource = dtClass;
        DDL_ProjectMemberClass.DataTextField = "ClassName";
        DDL_ProjectMemberClass.DataValueField = "ID";
        DDL_ProjectMemberClass.DataBind();

        string strClassID = DDL_ProjectMemberClass.SelectedValue;

        int intClassID = 0;
        int.TryParse(strClassID, out intClassID);

        LoadProjectMemberStudentCostInfo(intGradeID, intClassID);
    }

    protected void DDL_ProjectMemberGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID = DDL_ProjectMemberGrade.SelectedValue;
        if (!string.IsNullOrEmpty(strID))
        {
            int intID = 0;
            int.TryParse(strID, out intID);
            DataClassBinder(intID);
        };
    }
}