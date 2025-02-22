using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDSystemList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        GDSystemBLL gDSystemBLL = new GDSystemBLL();
        string strGDSystemHQL = "from GDSystem as gDSystem order by ID desc";
        IList listGDSystem = gDSystemBLL.GetAllGDSystems(strGDSystemHQL);

        DG_List.DataSource = listGDSystem;
        DG_List.DataBind();

        LB_Sql.Text = strGDSystemHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDSystemBLL gDSystemBLL = new GDSystemBLL();
            string strGDSystemSql = "from GDSystem as gDSystem where ID = " + cmdArges;
            IList listGDSystem = gDSystemBLL.GetAllGDSystems(strGDSystemSql);
            if (listGDSystem != null && listGDSystem.Count == 1)
            {
                GDSystem gDSystem = (GDSystem)listGDSystem[0];
                gDSystemBLL.DeleteGDSystem(gDSystem);
                
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

        GDSystemBLL gDSystemBLL = new GDSystemBLL();
        string strGDSystemHQL = LB_Sql.Text;
        IList listGDSystem = gDSystemBLL.GetAllGDSystems(strGDSystemHQL);

        DG_List.DataSource = listGDSystem;
        DG_List.DataBind();
    }
}