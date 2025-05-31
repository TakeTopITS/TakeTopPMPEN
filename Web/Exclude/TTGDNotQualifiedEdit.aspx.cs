using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDNotQualifiedEdit : System.Web.UI.Page
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
            string strNotQualified = TXT_NotQualified.Text.Trim();
            string strDescription = TXT_Description.Text.Trim();
            string strWeldPosition = TXT_WeldPosition.Text.Trim();

            if (!ShareClass.CheckStringRight(strNotQualified))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBHGDHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDescription))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSMBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWeldPosition))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZHJWZBNWFFZF+"')", true);
                return;
            }

            GDRTNotQualifiedBLL gDRTNotQualifiedBLL = new GDRTNotQualifiedBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDRTNotQualifiedSql = "from GDRTNotQualified as gDRTNotQualified where id = " + intID;
                IList listGDRTNotQualified = gDRTNotQualifiedBLL.GetAllGDRTNotQualifieds(strGDRTNotQualifiedSql);
                if (listGDRTNotQualified != null && listGDRTNotQualified.Count > 0)
                {
                    GDRTNotQualified gDRTNotQualified = (GDRTNotQualified)listGDRTNotQualified[0];

                    gDRTNotQualified.NotQualified = strNotQualified;
                    gDRTNotQualified.Description = strDescription;
                    gDRTNotQualified.WeldPosition = strWeldPosition;

                    gDRTNotQualifiedBLL.UpdateGDRTNotQualified(gDRTNotQualified, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDRTNotQualified gDRTNotQualified = new GDRTNotQualified();
                gDRTNotQualified.NotQualified = strNotQualified;
                gDRTNotQualified.Description = strDescription;
                gDRTNotQualified.WeldPosition = strWeldPosition;

                gDRTNotQualified.IsMark = 0;
                gDRTNotQualified.UserCode = strUserCode;

                gDRTNotQualifiedBLL.AddGDRTNotQualified(gDRTNotQualified);
            }

            Response.Redirect("TTGDNotQualifiedList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDRTNotQualifiedBLL gDRTNotQualifiedBLL = new GDRTNotQualifiedBLL();
        string strGDRTNotQualifiedSql = "from GDRTNotQualified as gDRTNotQualified where id = " + id;
        IList listGDRTNotQualified = gDRTNotQualifiedBLL.GetAllGDRTNotQualifieds(strGDRTNotQualifiedSql);
        if (listGDRTNotQualified != null && listGDRTNotQualified.Count > 0)
        {
            GDRTNotQualified gDRTNotQualified = (GDRTNotQualified)listGDRTNotQualified[0];
            TXT_NotQualified.Text = gDRTNotQualified.NotQualified;
            TXT_Description.Text = gDRTNotQualified.Description;
            TXT_WeldPosition.Text = gDRTNotQualified.WeldPosition;
        }
    }
}