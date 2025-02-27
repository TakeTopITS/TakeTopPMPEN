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

public partial class TTGDHotHandlerList : System.Web.UI.Page
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
        string strGDHotHandlerHQL = @"select * from T_GDHotHandler ";
        DataTable dtGDHotHandler = ShareClass.GetDataSetFromSql(strGDHotHandlerHQL, "GDHotHandler").Tables[0];

        DG_List.DataSource = dtGDHotHandler;
        DG_List.DataBind();

        LB_Sql.Text = strGDHotHandlerHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDHotHandlerBLL gDHotHandlerBLL = new GDHotHandlerBLL();
            string strGDHotHandlerSql = "from GDHotHandler as gDHotHandler where ID = " + cmdArges;
            IList listGDHotHandler = gDHotHandlerBLL.GetAllGDHotHandlers(strGDHotHandlerSql);
            if (listGDHotHandler != null && listGDHotHandler.Count == 1)
            {
                GDHotHandler gDHotHandler = (GDHotHandler)listGDHotHandler[0];
                gDHotHandlerBLL.DeleteGDHotHandler(gDHotHandler);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strGDHotHandlerHQL = LB_Sql.Text;
        DataTable dtGDHotHandler = ShareClass.GetDataSetFromSql(strGDHotHandlerHQL, "GDHotHandler").Tables[0];

        DG_List.DataSource = dtGDHotHandler;
        DG_List.DataBind();
    }
}