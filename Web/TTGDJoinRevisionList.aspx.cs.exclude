using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDJoinRevisionList : System.Web.UI.Page
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
        GDJointRevisionBLL gDJointRevisionBLL = new GDJointRevisionBLL();
        string strGDJointRevisionHQL = "from GDJointRevision as gDJointRevision order by ID desc";
        IList listGDJointRevision = gDJointRevisionBLL.GetAllGDJointRevisions(strGDJointRevisionHQL);

        DG_List.DataSource = listGDJointRevision;
        DG_List.DataBind();

        LB_Sql.Text = strGDJointRevisionHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDJointRevisionBLL gDJointRevisionBLL = new GDJointRevisionBLL();
            string strGDJointRevisionSql = "from GDJointRevision as gDJointRevision where ID = " + cmdArges;
            IList listGDJointRevision = gDJointRevisionBLL.GetAllGDJointRevisions(strGDJointRevisionSql);
            if (listGDJointRevision != null && listGDJointRevision.Count == 1)
            {
                GDJointRevision gDJointRevision = (GDJointRevision)listGDJointRevision[0];
                gDJointRevisionBLL.DeleteGDJointRevision(gDJointRevision);
                
                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                //Response.Write("<script>alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"');</script>");
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        GDJointRevisionBLL gDJointRevisionBLL = new GDJointRevisionBLL();
        string strGDJointRevisionHQL = LB_Sql.Text;
        IList listGDJointRevision = gDJointRevisionBLL.GetAllGDJointRevisions(strGDJointRevisionHQL);

        DG_List.DataSource = listGDJointRevision;
        DG_List.DataBind();
    }
}