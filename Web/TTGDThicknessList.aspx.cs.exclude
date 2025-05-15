using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDThicknessList : System.Web.UI.Page
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
        GDThicknessBLL gDThicknessBLL = new GDThicknessBLL();
        string strGDThicknessHQL = "from GDThickness as gDThickness order by ID desc";
        IList listGDThickness = gDThicknessBLL.GetAllGDThicknesss(strGDThicknessHQL);

        DG_List.DataSource = listGDThickness;
        DG_List.DataBind();

        LB_Sql.Text = strGDThicknessHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDThicknessBLL gDThicknessBLL = new GDThicknessBLL();
            string strGDThicknessSql = "from GDThickness as gDThickness where ID = " + cmdArges;
            IList listGDThickness = gDThicknessBLL.GetAllGDThicknesss(strGDThicknessSql);
            if (listGDThickness != null && listGDThickness.Count == 1)
            {
                GDThickness gDThickness = (GDThickness)listGDThickness[0];
                gDThicknessBLL.DeleteGDThickness(gDThickness);
                
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

        GDThicknessBLL gDThicknessBLL = new GDThicknessBLL();
        string strGDThicknessHQL = LB_Sql.Text;
        IList listGDThickness = gDThicknessBLL.GetAllGDThicknesss(strGDThicknessHQL);

        DG_List.DataSource = listGDThickness;
        DG_List.DataBind();
    }
}