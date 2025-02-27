using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPressureList : System.Web.UI.Page
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
        GDPressureBLL gDPressureBLL = new GDPressureBLL();
        string strGDPressureHQL = "from GDPressure as gDPressure";
        string strProjectCode = DDL_GDProject.SelectedValue;
        if (!string.IsNullOrEmpty(strProjectCode))
        {
            strGDPressureHQL += " where gDPressure.ProjectCode = '" + strProjectCode + "'";
        }
        IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureHQL);

        DG_List.DataSource = listGDPressure;
        DG_List.DataBind();

        LB_Sql.Text = strGDPressureHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "edit")
        {
            string cmdArges = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertAddEditPage('TTGDPressureEdit.aspx?id=" + cmdArges + "')", true);
            return;
        }
        else if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDPressureBLL gDPressureBLL = new GDPressureBLL();
            string strGDPressureSql = "from GDPressure as gDPressure where PressureCode = '" + cmdArges + "'";
            IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureSql);
            if (listGDPressure != null && listGDPressure.Count == 1)
            {
                GDPressure gDPressure = (GDPressure)listGDPressure[0];
                gDPressureBLL.DeleteGDPressure(gDPressure);
                
                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        GDPressureBLL gDPressureBLL = new GDPressureBLL();
        string strGDPressureHQL = LB_Sql.Text;
        IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureHQL);

        DG_List.DataSource = listGDPressure;
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