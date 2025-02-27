using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZDivideList : System.Web.UI.Page
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
        WZDivideBLL wZDivideBLL = new WZDivideBLL();
        string strDivideHQL = "from WZDivide as wZDivide";
        IList listDivide = wZDivideBLL.GetAllWZDivides(strDivideHQL);

        DG_List.DataSource = listDivide;
        DG_List.DataBind();
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Pager)
        {
            string cmdName = e.CommandName;
            if (cmdName == "del")
            {
                string cmdArges = e.CommandArgument.ToString();
                WZDivideBLL wZDivideBLL = new WZDivideBLL();
                string strDivideSql = "from WZDivide as wZDivide where DivideCode = '" + cmdArges + "'";
                IList divideList = wZDivideBLL.GetAllWZDivides(strDivideSql);
                if (divideList != null && divideList.Count == 1)
                {
                    WZDivide wZDivide = (WZDivide)divideList[0];
                    if (wZDivide.IsMark == -1)
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYBJW1BYXSC").ToString().Trim()+"')", true);
                        return;
                    }

                    wZDivideBLL.DeleteWZDivide(wZDivide);

                    //重新加载列表
                    DataBinder();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
                }

            }
        }
    }
}