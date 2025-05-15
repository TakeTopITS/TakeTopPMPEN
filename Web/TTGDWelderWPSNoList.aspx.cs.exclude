using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDWelderWPSNoList : System.Web.UI.Page
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
        GDWelderWPSNoBLL gDWelderWPSNoBLL = new GDWelderWPSNoBLL();
        string strWelderWPSNoHQL = "from GDWelderWPSNo as gDWelderWPSNo order by ID desc";
        IList listGDWelderWPSNo = gDWelderWPSNoBLL.GetAllGDWelderWPSNos(strWelderWPSNoHQL);

        DG_List.DataSource = listGDWelderWPSNo;
        DG_List.DataBind();

        LB_Sql.Text = strWelderWPSNoHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDWelderWPSNoBLL gDWelderWPSNoBLL = new GDWelderWPSNoBLL();
            string strGDWelderWPSNoSql = "from GDWelderWPSNo as gDWelderWPSNo where ID = " + cmdArges;
            IList listGDWelderWPSNo = gDWelderWPSNoBLL.GetAllGDWelderWPSNos(strGDWelderWPSNoSql);
            if (listGDWelderWPSNo != null && listGDWelderWPSNo.Count == 1)
            {
                GDWelderWPSNo gDWelderWPSNo = (GDWelderWPSNo)listGDWelderWPSNo[0];
                gDWelderWPSNoBLL.DeleteGDWelderWPSNo(gDWelderWPSNo);
                
                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }




    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        GDWelderWPSNoBLL gDWelderWPSNoBLL = new GDWelderWPSNoBLL();
        string strWelderWPSNoHQL = LB_Sql.Text;
        IList listGDWelderWPSNo = gDWelderWPSNoBLL.GetAllGDWelderWPSNos(strWelderWPSNoHQL);

        DG_List.DataSource = listGDWelderWPSNo;
        DG_List.DataBind();
    }
}