using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDWPSCodeList : System.Web.UI.Page
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
        GDWPSCodeBLL gDWPSCodeBLL = new GDWPSCodeBLL();
        string strGDWPSCodeHQL = "from GDWPSCode as gDWPSCode";
        IList listGDWPSCode = gDWPSCodeBLL.GetAllGDWPSCodes(strGDWPSCodeHQL);

        DG_List.DataSource = listGDWPSCode;
        DG_List.DataBind();

        LB_Sql.Text = strGDWPSCodeHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDWPSCodeBLL gDWPSCodeBLL = new GDWPSCodeBLL();
            string strGDWPSCodeSql = "from GDWPSCode as GDWPSCode where WPSNo = '" + cmdArges + "'";
            IList listGDWPSCode = gDWPSCodeBLL.GetAllGDWPSCodes(strGDWPSCodeSql);
            if (listGDWPSCode != null && listGDWPSCode.Count == 1)
            {
                GDWPSCode gDWPSCode = (GDWPSCode)listGDWPSCode[0];
                gDWPSCodeBLL.DeleteGDWPSCode(gDWPSCode);
                
                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }



    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        GDWPSCodeBLL gDWPSCodeBLL = new GDWPSCodeBLL();
        string strGDWPSCodeHQL = LB_Sql.Text;
        IList listGDWPSCode = gDWPSCodeBLL.GetAllGDWPSCodes(strGDWPSCodeHQL);

        DG_List.DataSource = listGDWPSCode;
        DG_List.DataBind();
    }
}