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

public partial class TTGDLineWeldList : System.Web.UI.Page
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
        string strGDLineWeldHQL = @"select w.*,p.ProjectName,a.Area as AreaName from T_GDLineWeld w
                    left join T_GDProject p on w.ProjectCode = p.ProjectCode
                    left join T_GDArea a on w.Area = a.ID";
        string strProjectCode = DDL_GDProject.SelectedValue;
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strGDLineWeldHQL += " where w.ProjectCode = '"+strProjectCode+"'";
        }
        strGDLineWeldHQL += " order by w.ID desc";
        DataTable dtGDLineWeld = ShareClass.GetDataSetFromSql(strGDLineWeldHQL, "GDLineWeld").Tables[0];

        DG_List.DataSource = dtGDLineWeld;
        DG_List.DataBind();

        LB_Sql.Text = strGDLineWeldHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "edit")
        {
            string cmdArges = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertAddEditPage('TTGDLineWeldEdit.aspx?id=" + cmdArges + "')", true);
            return;
        }
        else if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();
            string strGDLineWeldSql = "from GDLineWeld as gDLineWeld where ID = " + cmdArges;
            IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldSql);
            if (listGDLineWeld != null && listGDLineWeld.Count == 1)
            {
                GDLineWeld gDLineWeld = (GDLineWeld)listGDLineWeld[0];
                gDLineWeldBLL.DeleteGDLineWeld(gDLineWeld);
                
                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;
        
        string strGDLineWeldHQL = LB_Sql.Text;
        DataTable dtGDLineWeld = ShareClass.GetDataSetFromSql(strGDLineWeldHQL, "GDLineWeld").Tables[0];

        DG_List.DataSource = dtGDLineWeld;
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