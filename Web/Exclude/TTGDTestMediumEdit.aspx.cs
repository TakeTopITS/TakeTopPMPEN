using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDTestMediumEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string id = Request.QueryString["id"].ToString();
                HF_ID.Value = id;
                int intID = 0;
                int.TryParse(id, out intID);

                BindData(intID);
            }
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strTestMedium = TXT_TestMedium.Text.Trim();

            if (string.IsNullOrEmpty(strTestMedium))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYJZBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strTestMedium))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYJZBNWFFZF+"')", true);
                return;
            }

            GDTestMediumBLL gDTestMediumBLL = new GDTestMediumBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDTestMediumSql = "from GDTestMedium as gDTestMedium where id = " + intID;
                IList listGDTestMedium = gDTestMediumBLL.GetAllGDTestMediums(strGDTestMediumSql);
                if (listGDTestMedium != null && listGDTestMedium.Count > 0)
                {
                    GDTestMedium gDTestMedium = (GDTestMedium)listGDTestMedium[0];

                    gDTestMedium.TestMedium = strTestMedium;

                    gDTestMediumBLL.UpdateGDTestMedium(gDTestMedium, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDTestMedium gDTestMedium = new GDTestMedium();
                gDTestMedium.TestMedium = strTestMedium;

                gDTestMedium.IsMark = 0;
                gDTestMedium.UserCode = strUserCode;

                gDTestMediumBLL.AddGDTestMedium(gDTestMedium);
            }

            Response.Redirect("TTGDTestMediumList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDTestMediumBLL gDTestMediumBLL = new GDTestMediumBLL();
        string strGDTestMediumSql = "from GDTestMedium as gDTestMedium where id = " + id;
        IList listGDTestMedium = gDTestMediumBLL.GetAllGDTestMediums(strGDTestMediumSql);
        if (listGDTestMedium != null && listGDTestMedium.Count > 0)
        {
            GDTestMedium gDTestMedium = (GDTestMedium)listGDTestMedium[0];
            TXT_TestMedium.Text = gDTestMedium.TestMedium;
        }
    }
}