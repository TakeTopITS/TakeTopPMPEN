using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPipingClassList : System.Web.UI.Page
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
        GDPipingClassBLL gDPipingClassBLL = new GDPipingClassBLL();
        string strGDPipingClassHQL = "from GDPipingClass as gDPipingClass order by ID desc";
        IList listGDPipingClass = gDPipingClassBLL.GetAllGDPipingClasss(strGDPipingClassHQL);

        DG_List.DataSource = listGDPipingClass;
        DG_List.DataBind();

        LB_Sql.Text = strGDPipingClassHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDPipingClassBLL gDPipingClassBLL = new GDPipingClassBLL();
            string strGDPipingClassSql = "from GDPipingClass as gDPipingClass where ID = " + cmdArges;
            IList listGDPipingClass = gDPipingClassBLL.GetAllGDPipingClasss(strGDPipingClassSql);
            if (listGDPipingClass != null && listGDPipingClass.Count == 1)
            {
                GDPipingClass gDPipingClass = (GDPipingClass)listGDPipingClass[0];
                gDPipingClassBLL.DeleteGDPipingClass(gDPipingClass);
                
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

        GDPipingClassBLL gDPipingClassBLL = new GDPipingClassBLL();
        string strGDPipingClassHQL = LB_Sql.Text;
        IList listGDPipingClass = gDPipingClassBLL.GetAllGDPipingClasss(strGDPipingClassHQL);

        DG_List.DataSource = listGDPipingClass;
        DG_List.DataBind();
    }
}