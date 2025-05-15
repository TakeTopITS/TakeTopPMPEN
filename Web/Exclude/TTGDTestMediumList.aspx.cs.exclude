using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDTestMediumList : System.Web.UI.Page
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
        GDTestMediumBLL gDTestMediumBLL = new GDTestMediumBLL();
        string strGDTestMediumHQL = "from GDTestMedium as gDTestMedium order by ID desc";
        IList listGDTestMedium = gDTestMediumBLL.GetAllGDTestMediums(strGDTestMediumHQL);

        DG_List.DataSource = listGDTestMedium;
        DG_List.DataBind();

        LB_Sql.Text = strGDTestMediumHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDTestMediumBLL gDTestMediumBLL = new GDTestMediumBLL();
            string strGDTestMediumSql = "from GDTestMedium as gDTestMedium where ID = " + cmdArges;
            IList listGDTestMedium = gDTestMediumBLL.GetAllGDTestMediums(strGDTestMediumSql);
            if (listGDTestMedium != null && listGDTestMedium.Count == 1)
            {
                GDTestMedium gDTestMedium = (GDTestMedium)listGDTestMedium[0];
                gDTestMediumBLL.DeleteGDTestMedium(gDTestMedium);
                
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

        GDTestMediumBLL gDTestMediumBLL = new GDTestMediumBLL();
        string strGDTestMediumHQL = LB_Sql.Text;
        IList listGDTestMedium = gDTestMediumBLL.GetAllGDTestMediums(strGDTestMediumHQL);

        DG_List.DataSource = listGDTestMedium;
        DG_List.DataBind();
    }
}