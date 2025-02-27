using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDOrderCodeList : System.Web.UI.Page
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
        GDOrderCodeBLL gDOrderCodeBLL = new GDOrderCodeBLL();
        string strGDOrderCodeHQL = "from GDOrderCode as gDOrderCode order by ID desc";
        IList listGDOrderCode = gDOrderCodeBLL.GetAllGDOrderCodes(strGDOrderCodeHQL);

        DG_List.DataSource = listGDOrderCode;
        DG_List.DataBind();

        LB_Sql.Text = strGDOrderCodeHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDOrderCodeBLL gDOrderCodeBLL = new GDOrderCodeBLL();
            string strGDOrderCodeSql = "from GDOrderCode as gDOrderCode where ID = " + cmdArges;
            IList listGDOrderCode = gDOrderCodeBLL.GetAllGDOrderCodes(strGDOrderCodeSql);
            if (listGDOrderCode != null && listGDOrderCode.Count == 1)
            {
                GDOrderCode gDOrderCode = (GDOrderCode)listGDOrderCode[0];
                gDOrderCodeBLL.DeleteGDOrderCode(gDOrderCode);
                
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

        GDOrderCodeBLL gDOrderCodeBLL = new GDOrderCodeBLL();
        string strGDOrderCodeHQL = LB_Sql.Text;
        IList listGDOrderCode = gDOrderCodeBLL.GetAllGDOrderCodes(strGDOrderCodeHQL);

        DG_List.DataSource = listGDOrderCode;
        DG_List.DataBind();
    }
}