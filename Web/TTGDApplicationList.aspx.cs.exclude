using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDApplicationList : System.Web.UI.Page
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
        GDApplicationBLL gDApplicationBLL = new GDApplicationBLL();
        string strGDApplicationHQL = "from GDApplication as gDApplication order by ID desc";
        IList listGDApplication = gDApplicationBLL.GetAllGDApplications(strGDApplicationHQL);

        DG_List.DataSource = listGDApplication;
        DG_List.DataBind();

        LB_Sql.Text = strGDApplicationHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDApplicationBLL gDApplicationBLL = new GDApplicationBLL();
            string strGDApplicationSql = "from GDApplication as gDApplication where ID = " + cmdArges;
            IList listGDApplication = gDApplicationBLL.GetAllGDApplications(strGDApplicationSql);
            if (listGDApplication != null && listGDApplication.Count == 1)
            {
                GDApplication gDApplication = (GDApplication)listGDApplication[0];
                gDApplicationBLL.DeleteGDApplication(gDApplication);
                
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

        GDApplicationBLL gDApplicationBLL = new GDApplicationBLL();
        string strGDApplicationHQL = LB_Sql.Text;
        IList listGDApplication = gDApplicationBLL.GetAllGDApplications(strGDApplicationHQL);

        DG_List.DataSource = listGDApplication;
        DG_List.DataBind();
    }
}