using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPressureTestList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataBinder();

            DataGDPressureBinder();
        }
    }

    private void DataBinder()
    {
        GDPressureTestBLL gDPressureTestBLL = new GDPressureTestBLL();
        string strGDPressureTestHQL = "from GDPressureTest as gDPressureTest order by ID desc";
        string strTestLoopNo = DDL_Pressure.SelectedValue;
        if (!string.IsNullOrEmpty(strTestLoopNo))
        {
            strGDPressureTestHQL += " where gDPressureTest.TestLoopNo = '" + strTestLoopNo + "'";
        }
        IList listGDPressureTest = gDPressureTestBLL.GetAllGDPressureTests(strGDPressureTestHQL);

        DG_List.DataSource = listGDPressureTest;
        DG_List.DataBind();

        LB_Sql.Text = strGDPressureTestHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "edit")
        {
            string cmdArges = e.CommandArgument.ToString();
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertAddEditPage('TTGDPressureTestEdit.aspx?id=" + cmdArges + "')", true);
            return;
        }
        else if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDPressureTestBLL gDPressureTestBLL = new GDPressureTestBLL();
            string strGDPressureTestSql = "from GDPressureTest as gDPressureTest where ID = " + cmdArges;
            IList listGDPressureTest = gDPressureTestBLL.GetAllGDPressureTests(strGDPressureTestSql);
            if (listGDPressureTest != null && listGDPressureTest.Count == 1)
            {
                GDPressureTest gDPressureTest = (GDPressureTest)listGDPressureTest[0];
                gDPressureTestBLL.DeleteGDPressureTest(gDPressureTest);
                
                //重新加载列表
                DataBinder();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCCG").ToString().Trim()+"')", true);
            }

        }
    }



    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        GDPressureTestBLL gDPressureTestBLL = new GDPressureTestBLL();
        string strGDPressureTestHQL = LB_Sql.Text;
        IList listGDPressureTest = gDPressureTestBLL.GetAllGDPressureTests(strGDPressureTestHQL);

        DG_List.DataSource = listGDPressureTest;
        DG_List.DataBind();
    }



    private void DataGDPressureBinder()
    {
        GDPressureBLL gDPressureBLL = new GDPressureBLL();
        string strGDPressureHQL = "from GDPressure as gDPressure";
        IList listGDPressure = gDPressureBLL.GetAllGDPressures(strGDPressureHQL);

        DDL_Pressure.DataSource = listGDPressure;
        DDL_Pressure.DataTextField = "PressureCode";
        DDL_Pressure.DataValueField = "PressureCode";
        DDL_Pressure.DataBind();

        DDL_Pressure.Items.Insert(0, new ListItem("", ""));
    }


    protected void DDL_Pressure_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBinder();
    }



    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder();
    }


    protected void BT_Search_Click(object sender, EventArgs e)
    {
        DataBinder();
    }
}