using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDIsomJointList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            string strIsom_no = Request.QueryString["Isom_no"].ToString();
            HF_Isom_no.Value = strIsom_no;

            DataBinder(strIsom_no);
        }
    }

    private void DataBinder(string strIsom_no)
    {
        GDIsomJointBLL gDIsomJointBLL = new GDIsomJointBLL();
        string strGDIsomJointHQL = "from GDIsomJoint as gDIsomJoint where Isom_no = '" + strIsom_no + "'";
        IList listGDIsomJoint = gDIsomJointBLL.GetAllGDIsomJoints(strGDIsomJointHQL);

        DG_List.DataSource = listGDIsomJoint;
        DG_List.DataBind();

        LB_Sql.Text = strGDIsomJointHQL;
    }


    protected void DG_List_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string cmdName = e.CommandName;
        if (cmdName == "edit")
        {
            string cmdArges = e.CommandArgument.ToString();
            string[] arrArges = cmdArges.Split('|');
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTGDIsomJointEdit.aspx?id=" + arrArges[0] + "&Isom_no=" + arrArges[1] + "')", true);
            return;
        }
        else if (cmdName == "del")
        {
            string cmdArges = e.CommandArgument.ToString();
            GDIsomJointBLL gDIsomJointBLL = new GDIsomJointBLL();
            string strGDIsomJointSql = "from GDIsomJoint as gDIsomJoint where ID = " + cmdArges;
            IList listGDIsomJoint = gDIsomJointBLL.GetAllGDIsomJoints(strGDIsomJointSql);
            if (listGDIsomJoint != null && listGDIsomJoint.Count == 1)
            {
                GDIsomJoint gDIsomJoint = (GDIsomJoint)listGDIsomJoint[0];
                gDIsomJointBLL.DeleteGDIsomJoint(gDIsomJoint);
                
                //重新加载列表
                string strIsom_no = HF_Isom_no.Value;
                DataBinder(strIsom_no);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSCCG+"')", true);
            }

        }
    }


    protected void DG_List_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DG_List.CurrentPageIndex = e.NewPageIndex;

        GDIsomJointBLL gDIsomJointBLL = new GDIsomJointBLL();
        string strGDIsomJointHQL = LB_Sql.Text;
        IList listGDIsomJoint = gDIsomJointBLL.GetAllGDIsomJoints(strGDIsomJointHQL);

        DG_List.DataSource = listGDIsomJoint;
        DG_List.DataBind();
    }



    /// <summary>
    ///  重新加载列表
    /// </summary>
    protected void BT_RelaceLoad_Click(object sender, EventArgs e)
    {
        DataBinder(HF_Isom_no.Value);

    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "AlertProjectPage('TTGDIsomJointEdit.aspx?id=&Isom_no=" + HF_Isom_no.Value + "')", true);
        return;
    }
}