using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDWeldersList : System.Web.UI.Page
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
        GDWeldersBLL gDWeldersBLL = new GDWeldersBLL();
        string strGDWeldersHQL = "from GDWelders as gDWelders order by CreateTime desc";
        IList listGDWelders = gDWeldersBLL.GetAllGDWelderss(strGDWeldersHQL);

        DG_List.DataSource = listGDWelders;
        DG_List.DataBind();

        LB_Sql.Text = strGDWeldersHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDWeldersBLL gDWeldersBLL = new GDWeldersBLL();
            string strGDWeldersSql = "from GDWelders as gDWelders where Welders = '" + cmdArges + "'";
            IList listGDWelders = gDWeldersBLL.GetAllGDWelderss(strGDWeldersSql);
            if (listGDWelders != null && listGDWelders.Count == 1)
            {
                GDWelders gDWelders = (GDWelders)listGDWelders[0];
                gDWeldersBLL.DeleteGDWelders(gDWelders);
                
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

        GDWeldersBLL gDWeldersBLL = new GDWeldersBLL();
        string strGDWeldersHQL = LB_Sql.Text;
        IList listGDWelders = gDWeldersBLL.GetAllGDWelderss(strGDWeldersHQL);

        DG_List.DataSource = listGDWelders;
        DG_List.DataBind();
    }
}