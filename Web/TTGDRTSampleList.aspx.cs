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

public partial class TTGDRTSampleList : System.Web.UI.Page
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
        string strGDRTSampleHQL = @"select * from T_GDRTSample ";
        DataTable dtGDRTSample = ShareClass.GetDataSetFromSql(strGDRTSampleHQL, "GDRTSample").Tables[0];

        DG_List.DataSource = dtGDRTSample;
        DG_List.DataBind();

        LB_Sql.Text = strGDRTSampleHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDRTSampleBLL gDRTSampleBLL = new GDRTSampleBLL();
            string strGDRTSampleSql = "from GDRTSample as gDRTSample where ID = " + cmdArges;
            IList listGDRTSample = gDRTSampleBLL.GetAllGDRTSamples(strGDRTSampleSql);
            if (listGDRTSample != null && listGDRTSample.Count == 1)
            {
                GDRTSample gDRTSample = (GDRTSample)listGDRTSample[0];
                gDRTSampleBLL.DeleteGDRTSample(gDRTSample);

                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        string strGDRTSampleHQL = LB_Sql.Text;
        DataTable dtGDRTSample = ShareClass.GetDataSetFromSql(strGDRTSampleHQL, "GDRTSample").Tables[0];

        DG_List.DataSource = dtGDRTSample;
        DG_List.DataBind();
    }
}