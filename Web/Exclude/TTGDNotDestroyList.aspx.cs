using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDNotDestroyList : System.Web.UI.Page
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
        GDNotDestroyBLL gDNotDestroyBLL = new GDNotDestroyBLL();
        string strGDNotDestroyHQL = "from GDNotDestroy as gDNotDestroy where 1=1 ";
        string strProjectCode = DDL_GDProject.SelectedValue;
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strGDNotDestroyHQL += " and ProjectCode = '" + strProjectCode + "'";
        }
        string strIsom_no = DDL_Isom_no.SelectedValue;
        if (!string.IsNullOrEmpty(strIsom_no))
        {
            strGDNotDestroyHQL += " and Isom_no = '" + strIsom_no + "'";
        }
        IList listGDNotDestroy = gDNotDestroyBLL.GetAllGDNotDestroys(strGDNotDestroyHQL);

        if (listGDNotDestroy != null && listGDNotDestroy.Count > 0)
        {
            RT_List.DataSource = listGDNotDestroy;
            RT_List.DataBind();
        }
        else {
            GDNotDestroy gDNotDestroy = new GDNotDestroy();
            listGDNotDestroy.Add(gDNotDestroy);

            RT_List.DataSource = listGDNotDestroy;
            RT_List.DataBind();
        }
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
        string strSelectProject = DDL_GDProject.SelectedValue;

        GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();
        string strGDLineWeldHQL = "from GDLineWeld as gDLineWeld where ProjectCode = '" + strSelectProject + "'";
        IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldHQL);

        DDL_Isom_no.DataSource = listGDLineWeld;
        DDL_Isom_no.DataTextField = "Isom_no";
        DDL_Isom_no.DataValueField = "Isom_no";
        DDL_Isom_no.DataBind();

        DDL_Isom_no.Items.Insert(0, new ListItem("", ""));

        DataBinder();
    }


    protected void DDL_Isom_no_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        DataBinder();
    }

    
    #region 注释
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
    //    {
    //        DataBinder();
    //    }
    //}

    //private void DataBinder()
    //{
    //    GDNotDestroyBLL gDNotDestroyBLL = new GDNotDestroyBLL();
    //    string strGDNotDestroyHQL = "from GDNotDestroy as gDNotDestroy";
    //    IList listGDNotDestroy = gDNotDestroyBLL.GetAllGDNotDestroys(strGDNotDestroyHQL);

    //    DG_List.DataSource = listGDNotDestroy;
    //    DG_List.DataBind();

    //    LB_Sql.Text = strGDNotDestroyHQL;
    //}


    //protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    //{
    //    string cmdName = e.CommandName;
    //    if (cmdName == "del")
    //    {
    //        string cmdArges = e.CommandArgument.ToString();
    //        GDNotDestroyBLL gDNotDestroyBLL = new GDNotDestroyBLL();
    //        string strGDNotDestroySql = "from GDNotDestroy as gDNotDestroy where ID = " + cmdArges;
    //        IList listGDNotDestroy = gDNotDestroyBLL.GetAllGDNotDestroys(strGDNotDestroySql);
    //        if (listGDNotDestroy != null && listGDNotDestroy.Count == 1)
    //        {
    //            GDNotDestroy gDNotDestroy = (GDNotDestroy)listGDNotDestroy[0];
    //            gDNotDestroyBLL.DeleteGDNotDestroy(gDNotDestroy);
                
    //            //重新加载列表
    //            DataBinder();

    //            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
    //        }

    //    }
    //}


    //protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    //{
    //    DG_List.CurrentPageIndex = e.NewPageIndex;

    //    string strGDNotDestroyHQL = LB_Sql.Text;
    //    GDNotDestroyBLL gDNotDestroyBLL = new GDNotDestroyBLL();
        
    //    IList listGDNotDestroy = gDNotDestroyBLL.GetAllGDNotDestroys(strGDNotDestroyHQL);

    //    DG_List.DataSource = listGDNotDestroy;
    //    DG_List.DataBind();
    //}
    #endregion
}