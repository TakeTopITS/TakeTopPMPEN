using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDIsomFriEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            DataGDProjectBinder();


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
            string strProjectCode = DDL_GDProject.SelectedValue;
            string strIsom_no = DDL_Isom_no.SelectedValue;
            string strFri1 = TXT_Fri1.Text.Trim();
            string strFri2 = TXT_Fri2.Text.Trim();
            string strFri3 = TXT_Fri3.Text.Trim();
            string strFri4 = TXT_Fri4.Text.Trim();
            string strFri5 = TXT_Fri5.Text.Trim();
            string strFri6 = TXT_Fri6.Text.Trim();
            string strFri7 = TXT_Fri7.Text.Trim();
            string strFri8 = TXT_Fri8.Text.Trim();
            string strFri9 = TXT_Fri9.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMBNWKZ+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strIsom_no))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDXTHBNWKZ+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri1))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI1BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri2))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI2HBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri3))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI3BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri4))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI4BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri5))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI5BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri6))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI6BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri7))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI7BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri8))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI8BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFri9))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFRI9BNWFFZF+"')", true);
                return;
            }

            GDIsomFriBLL gDIsomFriBLL = new GDIsomFriBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDIsomFriSql = "from GDIsomFri as gDIsomFri where id = " + intID;
                IList listGDIsomFri = gDIsomFriBLL.GetAllGDIsomFris(strGDIsomFriSql);
                if (listGDIsomFri != null && listGDIsomFri.Count > 0)
                {
                    GDIsomFri gDIsomFri = (GDIsomFri)listGDIsomFri[0];

                    gDIsomFri.ProjectCode = strProjectCode;
                    gDIsomFri.Isom_no = strIsom_no;
                    gDIsomFri.Fri1 = strFri1;
                    gDIsomFri.Fri2 = strFri2;
                    gDIsomFri.Fri3 = strFri3;
                    gDIsomFri.Fri4 = strFri4;
                    gDIsomFri.Fri5 = strFri5;
                    gDIsomFri.Fri6 = strFri6;
                    gDIsomFri.Fri7 = strFri7;
                    gDIsomFri.Fri8 = strFri8;
                    gDIsomFri.Fri9 = strFri9;

                    gDIsomFriBLL.UpdateGDIsomFri(gDIsomFri, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDIsomFri gDIsomFri = new GDIsomFri();
                gDIsomFri.ProjectCode = strProjectCode;
                gDIsomFri.Isom_no = strIsom_no;
                gDIsomFri.Fri1 = strFri1;
                gDIsomFri.Fri2 = strFri2;
                gDIsomFri.Fri3 = strFri3;
                gDIsomFri.Fri4 = strFri4;
                gDIsomFri.Fri5 = strFri5;
                gDIsomFri.Fri6 = strFri6;
                gDIsomFri.Fri7 = strFri7;
                gDIsomFri.Fri8 = strFri8;
                gDIsomFri.Fri9 = strFri9;

                gDIsomFri.IsMark = 0;
                gDIsomFri.UserCode = strUserCode;

                gDIsomFriBLL.AddGDIsomFri(gDIsomFri);
            }

            //Response.Redirect("TTGDIsomFriList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDIsomFriBLL gDIsomFriBLL = new GDIsomFriBLL();
        string strGDIsomFriSql = "from GDIsomFri as gDIsomFri where id = " + id;
        IList listGDIsomFri = gDIsomFriBLL.GetAllGDIsomFris(strGDIsomFriSql);
        if (listGDIsomFri != null && listGDIsomFri.Count > 0)
        {
            GDIsomFri gDIsomFri = (GDIsomFri)listGDIsomFri[0];

            string strProjectCode = gDIsomFri.ProjectCode;
            DDL_GDProject.SelectedValue = strProjectCode;

            GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();
            string strGDLineWeldHQL = "from GDLineWeld as gDLineWeld where ProjectCode = '" + strProjectCode + "'";
            IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldHQL);

            DDL_Isom_no.DataSource = listGDLineWeld;
            DDL_Isom_no.DataTextField = "Isom_no";
            DDL_Isom_no.DataValueField = "Isom_no";
            DDL_Isom_no.DataBind();

            DDL_Isom_no.SelectedValue = gDIsomFri.Isom_no;
            TXT_Fri1.Text = gDIsomFri.Fri1;
            TXT_Fri2.Text = gDIsomFri.Fri2;
            TXT_Fri3.Text = gDIsomFri.Fri3;
            TXT_Fri4.Text = gDIsomFri.Fri4;
            TXT_Fri5.Text = gDIsomFri.Fri5;
            TXT_Fri6.Text = gDIsomFri.Fri6;
            TXT_Fri7.Text = gDIsomFri.Fri7;
            TXT_Fri8.Text = gDIsomFri.Fri8;
            TXT_Fri9.Text = gDIsomFri.Fri9;
        }
    }


    private void DataGDProjectBinder()
    {
        GDProjectBLL gDProjectBLL = new GDProjectBLL();
        string strGDProjectHQL = "from GDProject as gDProject";
        IList listGDProject = gDProjectBLL.GetAllGDProjects(strGDProjectHQL);

        DDL_GDProject.DataSource = listGDProject;
        DDL_GDProject.DataTextField = "ProjectName";
        DDL_GDProject.DataValueField = "ProjectCode";
        DDL_GDProject.DataBind();

        DDL_GDProject.Items.Insert(0, new ListItem("", ""));
    }

    protected void DDL_GDProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSelectProject = DDL_GDProject.SelectedValue;

        GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();
        string strGDLineWeldHQL = "from GDLineWeld as gDLineWeld where ProjectCode = '" + strSelectProject + "'";
        IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldHQL);

        DDL_Isom_no.DataSource = listGDLineWeld;
        DDL_Isom_no.DataTextField = "Isom_no";
        DDL_Isom_no.DataValueField = "Isom_no";
        DDL_Isom_no.DataBind();
    }
}