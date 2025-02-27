using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDIsomFriList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();

            DataGDProjectBinder();
        }
    }

    private void DataBinder()
    {
        string strGDIsomFriHQL = @"select f.*,p.ProjectName from T_GDIsomFri f
                    left join T_GDProject p on f.ProjectCode = p.ProjectCode ";
        string strProjectCode = DDL_GDProject.SelectedValue;
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strGDIsomFriHQL += " where f.ProjectCode = '" + strProjectCode + "'";
        }
        DataTable dtGDIsomFri = ShareClass.GetDataSetFromSql(strGDIsomFriHQL, "GDIsomFri").Tables[0];

        DG_List.DataSource = dtGDIsomFri;
        DG_List.DataBind();

        LB_Sql.Text = strGDIsomFriHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "edit")
        {
            string cmdArges = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertAddEditPage('TTGDIsomFriEdit.aspx?id=" + cmdArges + "')", true);
            return;
        }
        else if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDIsomFriBLL gDIsomFriBLL = new GDIsomFriBLL();
            string strGDIsomFriSql = "from GDIsomFri as gDIsomFri where ID = " + cmdArges;
            IList listGDIsomFri = gDIsomFriBLL.GetAllGDIsomFris(strGDIsomFriSql);
            if (listGDIsomFri != null && listGDIsomFri.Count == 1)
            {
                GDIsomFri gDIsomFri = (GDIsomFri)listGDIsomFri[0];
                gDIsomFriBLL.DeleteGDIsomFri(gDIsomFri);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strGDIsomFriHQL = LB_Sql.Text;
        DataTable dtGDIsomFri = ShareClass.GetDataSetFromSql(strGDIsomFriHQL, "GDIsomFri").Tables[0];

        DG_List.DataSource = dtGDIsomFri;
        DG_List.DataBind();
    }


    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();
    }



    private void DataGDProjectBinder()
    {
        GDProjectBLL gDProjectBLL = new GDProjectBLL();
        string strGDProjectHQL = "from GDProject as gDProject";
        IList listGDProject = gDProjectBLL.GetAllGDProjects(strGDProjectHQL);

        DDL_GDProject.DataSource = listGDProject;
        DDL_GDProject.DataTextField = "ProjectName";
        DDL_GDProject.DataValueField = "ProjectCode";
        DDL_GDProject.DataBind();

        DDL_GDProject.Items.Insert(0, new ListItem("", ""));
    }

    protected void DDL_GDProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();
    }



    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();
    }
}