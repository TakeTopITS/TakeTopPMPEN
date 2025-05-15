using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDWeldTypeList : System.Web.UI.Page
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
        GDWeldTypeBLL gDWeldTypeBLL = new GDWeldTypeBLL();
        string strGDWeldTypeHQL = "from GDWeldType as gDWeldType order by ID desc";
        IList listGDWeldType = gDWeldTypeBLL.GetAllGDWeldTypes(strGDWeldTypeHQL);
        DG_List.DataSource = listGDWeldType;
        DG_List.DataBind();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDWeldTypeBLL gDWeldTypeBLL = new GDWeldTypeBLL();
            string strGDWeldTypeSql = "from GDWeldType as gDWeldType where ID = " + cmdArges;
            IList listGDWeldType = gDWeldTypeBLL.GetAllGDWeldTypes(strGDWeldTypeSql);
            if (listGDWeldType != null && listGDWeldType.Count == 1)
            {
                GDWeldType gDWeldType = (GDWeldType)listGDWeldType[0];
                gDWeldTypeBLL.DeleteGDWeldType(gDWeldType);
                
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
        
        GDWeldTypeBLL gDWeldTypeBLL = new GDWeldTypeBLL();
        string strGDWeldTypeHQL = LB_Sql.Text;
        IList listGDWeldType = gDWeldTypeBLL.GetAllGDWeldTypes(strGDWeldTypeHQL);

        DG_List.DataSource = listGDWeldType;
        DG_List.DataBind();
    }
}