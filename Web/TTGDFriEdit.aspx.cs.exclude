using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDFriEdit : System.Web.UI.Page
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
            string strArea = DDL_Area.SelectedValue;
            string strCodeName = TXT_CodeName.Text.Trim();
            string strFRICode = TXT_FRICode.Text.Trim();
            string strEdition = TXT_Edition.Text.Trim();
            string strPublicTime = TXT_PublicTime.Text.Trim();
            string strDescription = TXT_Description.Text.Trim();
            string strRemarks = TXT_Remarks.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXMBNWKZ").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strArea))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYBNWKZ").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCodeName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strFRICode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFRIHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strEdition))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPublicTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFBRBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDescription))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSMBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemarks))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }


            GDFriBLL gDFriBLL = new GDFriBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDFriSql = "from GDFri as gDFri where id = " + intID;
                IList listGDFri = gDFriBLL.GetAllGDFris(strGDFriSql);
                if (listGDFri != null && listGDFri.Count > 0)
                {
                    GDFri gDFri = (GDFri)listGDFri[0];

                    gDFri.ProjectCode = strProjectCode;
                    gDFri.Area = strArea;
                    gDFri.CodeName = strCodeName;
                    gDFri.FRICode = strFRICode;
                    gDFri.Edition = strEdition;
                    gDFri.PublicTime = strPublicTime;
                    gDFri.Description = strDescription;
                    gDFri.Remarks = strRemarks;

                    gDFriBLL.UpdateGDFri(gDFri, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDFri gDFri = new GDFri();
                gDFri.ProjectCode = strProjectCode;
                gDFri.Area = strArea;
                gDFri.CodeName = strCodeName;
                gDFri.FRICode = strFRICode;
                gDFri.Edition = strEdition;
                gDFri.PublicTime = strPublicTime;
                gDFri.Description = strDescription;
                gDFri.Remarks = strRemarks;

                gDFri.IsMark = 0;
                gDFri.UserCode = strUserCode;

                gDFriBLL.AddGDFri(gDFri);
            }

            //Response.Redirect("TTGDFriList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDFriBLL gDFriBLL = new GDFriBLL();
        string strGDFriSql = "from GDFri as gDFri where id = " + id;
        IList listGDFri = gDFriBLL.GetAllGDFris(strGDFriSql);
        if (listGDFri != null && listGDFri.Count > 0)
        {
            GDFri gDFri = (GDFri)listGDFri[0];

            string strProjectCode = gDFri.ProjectCode;

            DDL_GDProject.SelectedValue = strProjectCode;

            GDAreaBLL gDAreaBLL = new GDAreaBLL();
            string strGDAreaHQL = "from GDArea as gDArea where ProjectCode = '" + strProjectCode + "'";
            IList listGDArea = gDAreaBLL.GetAllGDAreas(strGDAreaHQL);

            DDL_Area.DataSource = listGDArea;
            DDL_Area.DataTextField = "Area";
            DDL_Area.DataValueField = "ID";
            DDL_Area.DataBind();


            DDL_Area.SelectedValue = gDFri.Area;
            TXT_CodeName.Text = gDFri.CodeName;
            TXT_FRICode.Text = gDFri.FRICode;
            TXT_Edition.Text = gDFri.Edition;
            TXT_PublicTime.Text = gDFri.PublicTime.ToString();
            TXT_Description.Text = gDFri.Description;
            TXT_Remarks.Text = gDFri.Remarks;
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

        GDAreaBLL gDAreaBLL = new GDAreaBLL();
        string strGDAreaHQL = "from GDArea as gDArea where ProjectCode = '" + strSelectProject + "'";
        IList listGDArea = gDAreaBLL.GetAllGDAreas(strGDAreaHQL);

        DDL_Area.DataSource = listGDArea;
        DDL_Area.DataTextField = "Area";
        DDL_Area.DataValueField = "ID";
        DDL_Area.DataBind();
    }
}