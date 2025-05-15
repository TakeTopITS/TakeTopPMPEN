using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDProjectList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "管道管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        GDProjectBLL gDProjectBLL = new GDProjectBLL();
        string strGDProjectHQL = "from GDProject as gDProject order by ID desc";
        IList listGDProject = gDProjectBLL.GetAllGDProjects(strGDProjectHQL);

        DG_List.DataSource = listGDProject;
        DG_List.DataBind();

        LB_Sql.Text = strGDProjectHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDProjectBLL gDProjectBLL = new GDProjectBLL();
            string strGDProjectSql = "from GDProject as gDProject where ID = " + cmdArges;
            IList listGDProject = gDProjectBLL.GetAllGDProjects(strGDProjectSql);
            if (listGDProject != null && listGDProject.Count == 1)
            {
                GDProject gDProject = (GDProject)listGDProject[0];
                gDProjectBLL.DeleteGDProject(gDProject);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                //Response.Write("<script>alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"');</script>");
            }

        }
    }

    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        GDProjectBLL gDProjectBLL = new GDProjectBLL();
        string strGDProjectHQL = LB_Sql.Text;
        IList listGDProject = gDProjectBLL.GetAllGDProjects(strGDProjectHQL);

        DG_List.DataSource = listGDProject;
        DG_List.DataBind();
    }
}