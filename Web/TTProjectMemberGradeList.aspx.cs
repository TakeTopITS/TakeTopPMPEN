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

public partial class TTProjectMemberGradeList : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strDepartString;

        if (Session["UserCode"] != null)
        {
            strUserCode = Session["UserCode"].ToString();
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            LB_DepartString.Text = strDepartString;

            strHQL = "from Department as department Where department.DepartCode in " + strDepartString;

            DataBinder();
        }
    }


    private void DataBinder()
    {
        string strGradeHQL;

        ProjectMemberGradeBLL projectMemberGradeBLL = new ProjectMemberGradeBLL();
        strGradeHQL = "from ProjectMemberGrade as projectMemberGrade where projectMemberGrade.DepartCode in " + LB_DepartString.Text.Trim();
        strGradeHQL += " order by projectMemberGrade.ID desc";
        IList listGrade = projectMemberGradeBLL.GetAllProjectMemberGrades(strGradeHQL);

        DG_List.DataSource = listGrade;
        DG_List.DataBind();

        LB_SQL.Text = strGradeHQL;

        LB_RecordCount.Text = listGrade.Count.ToString();
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
            TB_BelongDepartName.Text = strDepartName;
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        try
        {
            ProjectMemberGradeBLL projectMemberGradeBLL = new ProjectMemberGradeBLL();
            ProjectMemberGrade projectMemberGrade = new ProjectMemberGrade();

            string strGradeName = TXT_GradeName.Text.Trim();
            if (string.IsNullOrEmpty(strGradeName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZNJBNWKBC").ToString().Trim()+"')", true);
                return;
            }

            if (!ShareClass.CheckStringRight(strGradeName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZNJBNWFFZFCXG").ToString().Trim()+"')", true);
                return;
            }

            string strDepartCode = LB_BelongDepartCode.Text.Trim();
            string strDepartName = TB_BelongDepartName.Text.Trim();

            if (strDepartCode == "" | strDepartName == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGSBMBNWKJC").ToString().Trim()+"')", true);
                return;
            }

            strDepartName = ShareClass.GetDepartName(strDepartCode);

            string strCheckGradeHQL = string.Format("select count(1) as RowNumber from T_ProjectMemberGrade where GradeName = '{0}'", strGradeName);
            DataTable dtCheckGrade = ShareClass.GetDataSetFromSql(strCheckGradeHQL, "CheckGrade").Tables[0];
            int intCheckGradeCount = 0;
            int.TryParse(ShareClass.ObjectToString(dtCheckGrade.Rows[0]["RowNumber"]), out intCheckGradeCount);
            if (intCheckGradeCount > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZNJYJCZBYZFTJ").ToString().Trim()+"')", true);
                return;
            }

            projectMemberGrade.GradeName = strGradeName;

            projectMemberGrade.UserCode = strUserCode;

            projectMemberGrade.DepartCode = strDepartCode;
            projectMemberGrade.DepartName = strDepartName;


            projectMemberGradeBLL.AddProjectMemberGrade(projectMemberGrade);

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
        string strGradeID = TXT_ID.Text.Trim();
      

        try
        {
            ProjectMemberGradeBLL projectMemberGradeBLL = new ProjectMemberGradeBLL();
            ProjectMemberGrade projectMemberGrade = new ProjectMemberGrade();

            string strGradeName = TXT_GradeName.Text.Trim();
            if (string.IsNullOrEmpty(strGradeName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZNJBNWKBC").ToString().Trim()+"')", true);
                return;
            }

            if (!ShareClass.CheckStringRight(strGradeName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZNJBNWFFZFCXG").ToString().Trim()+"')", true);
                return;
            }
            string strDepartCode = LB_BelongDepartCode.Text.Trim();
            string strDepartName = TB_BelongDepartName.Text.Trim();

            if (strDepartCode == "" | strDepartName == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGSBMBNWKJC").ToString().Trim()+"')", true);
                return;
            }

            strDepartName = ShareClass.GetDepartName(strDepartCode);

            if (!string.IsNullOrEmpty(HF_ID.Value) && HF_ID.Value != "0")
            {
                string strID = HF_ID.Value;
                string strGradeHQL = string.Format(@"from ProjectMemberGrade as projectMemberGrade where ID = " + strID);
                IList lstGrade = projectMemberGradeBLL.GetAllProjectMemberGrades(strGradeHQL);
                if (lstGrade != null && lstGrade.Count > 0)
                {
                    projectMemberGrade = (ProjectMemberGrade)lstGrade[0];

                    string strCheckGradeHQL = string.Format(@"from ProjectMemberGrade as projectMemberGrade where GradeName = '{0}'", strGradeName);
                    IList lstCheckGrade = projectMemberGradeBLL.GetAllProjectMemberGrades(strCheckGradeHQL);
                    if (lstCheckGrade != null && lstCheckGrade.Count > 0)
                    {
                        ProjectMemberGrade wZCheckGrade = (ProjectMemberGrade)lstCheckGrade[0];
                        if (wZCheckGrade.ID.ToString() != strID)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZNJYJCZBYZFTJ").ToString().Trim()+"')", true);
                            return;
                        }
                    }

                    projectMemberGrade.DepartCode = strDepartCode;
                    projectMemberGrade.DepartName = strDepartName;

                    if (getExistedClassCount(strGradeID) == 0)
                    {
                        projectMemberGrade.GradeName = strGradeName;
                        projectMemberGrade.UserCode = strUserCode;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTSZBCLCNJDGSBMXXMYGGMCXXYCNJXCZBJ").ToString().Trim()+"')", true);
                    }
                 
                    projectMemberGradeBLL.UpdateProjectMemberGrade(projectMemberGrade, int.Parse(strID));

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZYXGDNJLB").ToString().Trim()+"')", true);
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

        HF_ID.Value = "0";
        TXT_ID.Text = "";
        TXT_GradeName.Text = "";
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string strCommandName = e.CommandName;
        if (strCommandName != "Page")
        {
            string strCmdArgu = e.CommandArgument.ToString();

            ProjectMemberGradeBLL projectMemberGradeBLL = new ProjectMemberGradeBLL();
            if (strCommandName.Trim() == "edit")
            {
                for (int i = 0; i < DG_List.Items.Count; i++)
                {
                    DG_List.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                string strGradeHQL = string.Format(@"from ProjectMemberGrade as projectMemberGrade where ID = " + strCmdArgu);
                IList lstGrade = projectMemberGradeBLL.GetAllProjectMemberGrades(strGradeHQL);
                if (lstGrade != null && lstGrade.Count > 0)
                {
                    ProjectMemberGrade projectMemberGrade = (ProjectMemberGrade)lstGrade[0];

                    HF_ID.Value = projectMemberGrade.ID.ToString();
                    TXT_ID.Text = projectMemberGrade.ID.ToString();
                    TXT_GradeName.Text = projectMemberGrade.GradeName;

                    TB_BelongDepartName.Text = projectMemberGrade.DepartName.Trim();
                    LB_BelongDepartCode.Text = projectMemberGrade.DepartCode.Trim();

                }
            }
            else if (strCommandName.Trim() == "del")
            {
                if (getExistedClassCount(strCmdArgu) > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGCNJXCZBJBNSCJC").ToString().Trim()+"')", true);
                    return;
                }

                string strGradeHQL = string.Format(@"from ProjectMemberGrade as projectMemberGrade where ID = " + strCmdArgu);
                IList lstGrade = projectMemberGradeBLL.GetAllProjectMemberGrades(strGradeHQL);
                if (lstGrade != null && lstGrade.Count > 0)
                {
                    ProjectMemberGrade projectMemberGrade = (ProjectMemberGrade)lstGrade[0];

                    projectMemberGradeBLL.DeleteProjectMemberGrade(projectMemberGrade);

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

        ProjectMemberGradeBLL projectMemberGradeBLL = new ProjectMemberGradeBLL();
        string strGradeHQL = LB_SQL.Text;
        IList listGrade = projectMemberGradeBLL.GetAllProjectMemberGrades(strGradeHQL);

        DG_List.DataSource = listGrade;
        DG_List.DataBind();
    }
    
    protected int getExistedClassCount(string strGradeID)
    {
        string strHQL = "Select * From T_ProjectMemberClass Where GradeID = " + strGradeID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMemberClass");

        return ds.Tables[0].Rows.Count;
    }
}