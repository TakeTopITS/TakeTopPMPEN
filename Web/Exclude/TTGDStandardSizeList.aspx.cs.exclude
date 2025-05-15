using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDStandardSizeList : System.Web.UI.Page
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
        GDStandardSizeBLL gDStandardSizeBLL = new GDStandardSizeBLL();
        string strGDStandardSizeHQL = "from GDStandardSize as gDStandardSize order by ID desc";
        IList listGDStandardSize = gDStandardSizeBLL.GetAllGDStandardSizes(strGDStandardSizeHQL);

        DG_List.DataSource = listGDStandardSize;
        DG_List.DataBind();

        LB_Sql.Text = strGDStandardSizeHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDStandardSizeBLL gDStandardSizeBLL = new GDStandardSizeBLL();
            string strGDStandardSizeSql = "from GDStandardSize as gDStandardSize where ID = " + cmdArges;
            IList listGDStandardSize = gDStandardSizeBLL.GetAllGDStandardSizes(strGDStandardSizeSql);
            if (listGDStandardSize != null && listGDStandardSize.Count == 1)
            {
                GDStandardSize gDStandardSize = (GDStandardSize)listGDStandardSize[0];
                gDStandardSizeBLL.DeleteGDStandardSize(gDStandardSize);
                
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
        GDStandardSizeBLL gDStandardSizeBLL = new GDStandardSizeBLL();
        string strGDStandardSizeHQL = LB_Sql.Text;
        IList listGDStandardSize = gDStandardSizeBLL.GetAllGDStandardSizes(strGDStandardSizeHQL);

        DG_List.DataSource = listGDStandardSize;
        DG_List.DataBind();
    }
}