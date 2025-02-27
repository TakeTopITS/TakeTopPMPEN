using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZProjectAttributeList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();

            DataProjectNatureBinder();

        }
    }

    private void DataBinder()
    {
        string strProjectAttributeHQL = @"select * from T_WZProjectAttribute order by AttributeCode ";

        DataTable dtProjectAttribute = ShareClass.GetDataSetFromSql(strProjectAttributeHQL, "ProjectAttribute").Tables[0];

        DG_List.DataSource = dtProjectAttribute;
        DG_List.DataBind();

        LB_Sql.Text = strProjectAttributeHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            WZProjectAttributeBLL wZProjectAttributeBLL = new WZProjectAttributeBLL();
            string strWZProjectAttributeSql = "from WZProjectAttribute as wZProjectAttribute where ID = " + cmdArges;
            IList listWZProjectAttribute = wZProjectAttributeBLL.GetAllWZProjectAttributes(strWZProjectAttributeSql);
            if (listWZProjectAttribute != null && listWZProjectAttribute.Count == 1)
            {
                WZProjectAttribute wZProjectAttribute = (WZProjectAttribute)listWZProjectAttribute[0];
                wZProjectAttributeBLL.DeleteWZProjectAttribute(wZProjectAttribute);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strWZProjectAttributeHQL = LB_Sql.Text;
        DataTable dtWZProjectAttribute = ShareClass.GetDataSetFromSql(strWZProjectAttributeHQL, "WZProjectAttribute").Tables[0];

        DG_List.DataSource = dtWZProjectAttribute;
        DG_List.DataBind();
    }









    private void DataProjectNatureBinder()
    {
        string strWZProjectNatureHQL = @"select * from T_WZProjectNature order by NatureCode ";

        DataTable dtWZProjectNature = ShareClass.GetDataSetFromSql(strWZProjectNatureHQL, "ProjectNature").Tables[0];

        DG_ProjectNature.DataSource = dtWZProjectNature;
        DG_ProjectNature.DataBind();

        LB_ProjectNatureSQL.Text = strWZProjectNatureHQL;
    }


    protected void DG_ProjectNature_ItemCommand(object source, DataGridCommandEventArgs e)
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
                DataProjectNatureBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }


    protected void DG_ProjectNature_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_ProjectNature.CurrentPageIndex = e.NewPageIndex;

        string strWZProjectNatureHQL = LB_ProjectNatureSQL.Text;
        DataTable dtWZProjectNature = ShareClass.GetDataSetFromSql(strWZProjectNatureHQL, "WZProjectNature").Tables[0];

        DG_ProjectNature.DataSource = dtWZProjectNature;
        DG_ProjectNature.DataBind();
    }




}