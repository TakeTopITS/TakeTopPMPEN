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

public partial class TTGDAreaList : System.Web.UI.Page
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
        string strGDAreaHQL = @"select a.*,p.ProjectName from T_GDArea a
                    left join T_GDProject p on a.ProjectCode = p.ProjectCode where 1=1 ";
        string strProjectCode = DDL_GDProject.SelectedValue;
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strGDAreaHQL += " and a.ProjectCode = '"+strProjectCode+"'";
        }

        strGDAreaHQL += " order by a.ID desc ";

        DataTable dtGDArea = ShareClass.GetDataSetFromSql(strGDAreaHQL, "GDArea").Tables[0];

        DG_List.DataSource = dtGDArea;
        DG_List.DataBind();

        LB_Sql.Text = strGDAreaHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "edit")
        {
            string cmdArges = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertAddEditPage('TTGDAreaEdit.aspx?id=" + cmdArges + "')", true);
            return;
        }
        else if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDAreaBLL gDAreaBLL = new GDAreaBLL();
            string strGDAreaSql = "from GDArea as gDArea where ID = " + cmdArges;
            IList listGDArea = gDAreaBLL.GetAllGDAreas(strGDAreaSql);
            if (listGDArea != null && listGDArea.Count == 1)
            {
                GDArea gDArea = (GDArea)listGDArea[0];
                gDAreaBLL.DeleteGDArea(gDArea);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
                //Response.Write("<script>alert('"+Resources.lang.ZZSCCG+"');</script>");
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strGDAreaHQL = LB_Sql.Text;
        DataTable dtGDArea = ShareClass.GetDataSetFromSql(strGDAreaHQL, "GDArea").Tables[0];

        DG_List.DataSource = dtGDArea;
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