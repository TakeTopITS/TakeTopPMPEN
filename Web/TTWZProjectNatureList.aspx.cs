using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZProjectNatureList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (!IsPostBack)
        {
            DataBinder();
        }
    }

    private void DataBinder()
    {
        string strWZProjectNatureHQL = @"select * from T_WZProjectNature order by NatureCode ";

        DataTable dtWZProjectNature = ShareClass.GetDataSetFromSql(strWZProjectNatureHQL, "WZProjectNature").Tables[0];

        DG_List.DataSource = dtWZProjectNature;
        DG_List.DataBind();

        LB_Sql.Text = strWZProjectNatureHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            WZProjectNatureBLL wZProjectNatureBLL = new WZProjectNatureBLL();
            string strWZProjectNatureSql = "from WZProjectNature as wZProjectNature where ID = " + cmdArges;
            IList listWZProjectNature = wZProjectNatureBLL.GetAllWZProjectNatures(strWZProjectNatureSql);
            if (listWZProjectNature != null && listWZProjectNature.Count == 1)
            {
                WZProjectNature wZProjectNature = (WZProjectNature)listWZProjectNature[0];
                wZProjectNatureBLL.DeleteWZProjectNature(wZProjectNature);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strWZProjectNatureHQL = LB_Sql.Text;
        DataTable dtWZProjectNature = ShareClass.GetDataSetFromSql(strWZProjectNatureHQL, "WZProjectNature").Tables[0];

        DG_List.DataSource = dtWZProjectNature;
        DG_List.DataBind();
    }
}