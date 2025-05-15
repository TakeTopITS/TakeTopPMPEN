using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPressureObjectList : System.Web.UI.Page
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
        GDPressureObjectBLL gDPressureObjectBLL = new GDPressureObjectBLL();
        string strGDPressureObjectHQL = "from GDPressureObject as gDPressureObject";
        IList listGDPressureObject = gDPressureObjectBLL.GetAllGDPressureObjects(strGDPressureObjectHQL);

        DG_List.DataSource = listGDPressureObject;
        DG_List.DataBind();

        LB_Sql.Text = strGDPressureObjectHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDPressureObjectBLL gDPressureObjectBLL = new GDPressureObjectBLL();
            string strGDPressureObjectSql = "from GDPressureObject as gDPressureObject where ID = " + cmdArges;
            IList listGDPressureObject = gDPressureObjectBLL.GetAllGDPressureObjects(strGDPressureObjectSql);
            if (listGDPressureObject != null && listGDPressureObject.Count == 1)
            {
                GDPressureObject gDPressureObject = (GDPressureObject)listGDPressureObject[0];
                gDPressureObjectBLL.DeleteGDPressureObject(gDPressureObject);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        GDPressureObjectBLL gDPressureObjectBLL = new GDPressureObjectBLL();
        string strGDPressureObjectHQL = LB_Sql.Text;
        IList listGDPressureObject = gDPressureObjectBLL.GetAllGDPressureObjects(strGDPressureObjectHQL);

        DG_List.DataSource = listGDPressureObject;
        DG_List.DataBind();
    }
}