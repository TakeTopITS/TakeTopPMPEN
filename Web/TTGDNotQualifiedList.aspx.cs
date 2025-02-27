using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDNotQualifiedList : System.Web.UI.Page
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
        GDRTNotQualifiedBLL gDRTNotQualifiedBLL = new GDRTNotQualifiedBLL();
        string strGDRTNotQualifiedHQL = "from GDRTNotQualified as gDRTNotQualified order by ID desc     ";
        IList listGDRTNotQualified = gDRTNotQualifiedBLL.GetAllGDRTNotQualifieds(strGDRTNotQualifiedHQL);

        DG_List.DataSource = listGDRTNotQualified;
        DG_List.DataBind();

        LB_Sql.Text = strGDRTNotQualifiedHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDRTNotQualifiedBLL gDRTNotQualifiedBLL = new GDRTNotQualifiedBLL();
            string strGDRTNotQualifiedSql = "from GDRTNotQualified as gDRTNotQualified where ID = " + cmdArges;
            IList listGDRTNotQualified = gDRTNotQualifiedBLL.GetAllGDRTNotQualifieds(strGDRTNotQualifiedSql);
            if (listGDRTNotQualified != null && listGDRTNotQualified.Count == 1)
            {
                GDRTNotQualified gDRTNotQualified = (GDRTNotQualified)listGDRTNotQualified[0];
                gDRTNotQualifiedBLL.DeleteGDRTNotQualified(gDRTNotQualified);
                
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

        GDRTNotQualifiedBLL gDRTNotQualifiedBLL = new GDRTNotQualifiedBLL();
        string strGDRTNotQualifiedHQL = LB_Sql.Text;
        IList listGDRTNotQualified = gDRTNotQualifiedBLL.GetAllGDRTNotQualifieds(strGDRTNotQualifiedHQL);

        DG_List.DataSource = listGDRTNotQualified;
        DG_List.DataBind();
    }
}