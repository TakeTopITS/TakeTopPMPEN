using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTProjectMemberClassList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserCode"] != null)
        {
            strUserCode = Session["UserCode"].ToString();
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            string strDepartString;

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;


            DataGradeBinder();

            DataBinder();
        }
    }


    private void DataGradeBinder()
    {
        string strGradeHQL;

        strGradeHQL = "select * from T_ProjectMemberGrade where DepartCode in " + LB_DepartString.Text.Trim();
        strGradeHQL += " order by ID desc";

        DataTable dtGrade = ShareClass.GetDataSetFromSql(strGradeHQL, "Grade").Tables[0];

        DDL_Grade.DataSource = dtGrade;

        DDL_Grade.DataTextField = "GradeName";
        DDL_Grade.DataValueField = "ID";

        DDL_Grade.DataBind();

    }

    private void DataBinder()
    {
        string strClassHQL = "select c.*,g.GradeName from T_ProjectMemberClass c , T_ProjectMemberGrade g where c.GradeID = g.ID";
        strClassHQL += " and g.DepartCode in " + LB_DepartString.Text.Trim();


        DataTable dtClass = ShareClass.GetDataSetFromSql(strClassHQL, "class").Tables[0];

        DG_List.DataSource = dtClass;
        DG_List.DataBind();

        LB_SQL.Text = strClassHQL;

        LB_RecordCount.Text = dtClass.Rows.Count.ToString();
    }


    protected void BT_Create_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectMemberClassBLL projectMemberClassBLL = new ProjectMemberClassBLL();
            ProjectMemberClass projectMemberClass = new ProjectMemberClass();

            int intGradeID = 0;
            int.TryParse(DDL_Grade.SelectedValue, out intGradeID);

            string strClassName = TXT_ClassName.Text.Trim();
            if (string.IsNullOrEmpty(strClassName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJBNWKBC").ToString().Trim()+"')", true);
                return;
            }

            if (!ShareClass.CheckStringRight(strClassName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJBNWFFZFCXG").ToString().Trim()+"')", true);
                return;
            }



            string strCheckClassHQL = string.Format("select count(1) as RowNumber from T_ProjectMemberClass where ClassName = '{0}'", strClassName);
            DataTable dtCheckClass = ShareClass.GetDataSetFromSql(strCheckClassHQL, "CheckClass").Tables[0];
            int intCheckClassCount = 0;
            int.TryParse(ShareClass.ObjectToString(dtCheckClass.Rows[0]["RowNumber"]), out intCheckClassCount);
            if (intCheckClassCount > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJYJCZBYZFTJ").ToString().Trim()+"')", true);
                return;
            }

            projectMemberClass.GradeID = intGradeID;
            projectMemberClass.ClassName = strClassName;

            projectMemberClass.UserCode = strUserCode;

            projectMemberClassBLL.AddProjectMemberClass(projectMemberClass);




            DataBinder();


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXJYCJC").ToString().Trim()+"')", true);
        }
    }

    protected void BT_Edit_Click(object sender, EventArgs e)
    {
        string strClassID = TXT_ID.Text.Trim();

        try
        {
            ProjectMemberClassBLL projectMemberClassBLL = new ProjectMemberClassBLL();
            ProjectMemberClass projectMemberClass = new ProjectMemberClass();

            int intGradeID = 0;
            int.TryParse(DDL_Grade.SelectedValue, out intGradeID);

            string strClassName = TXT_ClassName.Text.Trim();
            if (string.IsNullOrEmpty(strClassName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJBNWKBC").ToString().Trim()+"')", true);
                return;
            }

            if (!ShareClass.CheckStringRight(strClassName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJBNWFFZFCXG").ToString().Trim()+"')", true);
                return;
            }


            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strClassHQL = string.Format(@"from ProjectMemberClass as projectMemberClass where ID = " + strID);
                IList lstClass = projectMemberClassBLL.GetAllProjectMemberClasss(strClassHQL);
                if (lstClass != null && lstClass.Count > 0)
                {
                    projectMemberClass = (ProjectMemberClass)lstClass[0];

                    string strCheckClassHQL = string.Format(@"from ProjectMemberClass as projectMemberClass where ClassName = '{0}'", strClassName);
                    IList lstCheckClass = projectMemberClassBLL.GetAllProjectMemberClasss(strCheckClassHQL);
                    if (lstCheckClass != null && lstCheckClass.Count > 0)
                    {
                        ProjectMemberClass wZCheckClass = (ProjectMemberClass)lstCheckClass[0];
                        if (wZCheckClass.ID.ToString() != strID)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBJYJCZBYZFTJ").ToString().Trim()+"')", true);
                            return;
                        }
                    }

                    projectMemberClass.GradeID = intGradeID;

                    if (getExistedStudentCount(strClassID) == 0)
                    {
                        projectMemberClass.ClassName = strClassName;
                        projectMemberClass.UserCode = strUserCode;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZBCLCBJDSSNJXXMYGGMCXXYCBJXCZS").ToString().Trim()+"')", true);
                    }

                    projectMemberClassBLL.UpdateProjectMemberClass(projectMemberClass, int.Parse(strID));

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYXGDBJLB").ToString().Trim()+"')", true);
                return;

            }


            DataBinder();


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGXCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGXYCJC").ToString().Trim()+"')", true);
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < DG_List.Items.Count; i++)
        {
            DG_List.Items[i].ForeColor = Color.Black;
        }

        HF_ID.Value = "";
        TXT_ID.Text = "";
        DDL_Grade.SelectedIndex = 0;
        TXT_ClassName.Text = "";

    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strCommandName = e.CommandName;

        if (strCommandName != "Page")
        {
            string strCmdArgu = e.CommandArgument.ToString();

            ProjectMemberClassBLL projectMemberClassBLL = new ProjectMemberClassBLL();
            if (strCommandName.Trim() == "edit")
            {
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strClassHQL = string.Format(@"from ProjectMemberClass as projectMemberClass where ID = " + strCmdArgu);
                IList lstClass = projectMemberClassBLL.GetAllProjectMemberClasss(strClassHQL);
                if (lstClass != null && lstClass.Count > 0)
                {
                    ProjectMemberClass projectMemberClass = (ProjectMemberClass)lstClass[0];

                    HF_ID.Value = projectMemberClass.ID.ToString();
                    TXT_ID.Text = projectMemberClass.ID.ToString();
                    DDL_Grade.SelectedValue = projectMemberClass.GradeID.ToString();
                    TXT_ClassName.Text = projectMemberClass.ClassName;

                }
            }
            else if (strCommandName.Trim() == "del")
            {
                if (getExistedStudentCount(strCmdArgu) > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGCBJXCZSBNSCJC").ToString().Trim()+"')", true);
                    return;
                }


                string strClassHQL = string.Format(@"from ProjectMemberClass as projectMemberClass where ID = " + strCmdArgu);
                IList lstClass = projectMemberClassBLL.GetAllProjectMemberClasss(strClassHQL);
                if (lstClass != null && lstClass.Count > 0)
                {
                    ProjectMemberClass projectMemberClass = (ProjectMemberClass)lstClass[0];

                    projectMemberClassBLL.DeleteProjectMemberClass(projectMemberClass);

                    //÷ÿ–¬º”‘ÿ
                    DG_List.CurrentPageIndex = 0;

                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }
            }
        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strClassHQL = LB_SQL.Text;
        DataTable dtClass = ShareClass.GetDataSetFromSql(strClassHQL, "class").Tables[0];

        DG_List.DataSource = dtClass;
        DG_List.DataBind();
    }

    protected int getExistedStudentCount(string strClassID)
    {
        string strHQL = "Select * From T_ProjectMemberStudent Where ClassID = " + strClassID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberStudent");

        return ds.Tables[0].Rows.Count;
    }
}