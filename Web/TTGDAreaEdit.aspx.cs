using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDAreaEdit : System.Web.UI.Page
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
            string strPlace = TXT_Place.Text.Trim();
            string strMainArea = TXT_MainArea.Text.Trim();
            string strArea = TXT_Area.Text.Trim();
            string strSubcontractor = TXT_Subcontractor.Text.Trim();
            string strAreaDescription = TXT_AreaDescription.Text.Trim();
            string strTheSystem = TXT_TheSystem.Text.Trim();
            string strUnitCode = TXT_UnitCode.Text.Trim();
            string strUnitName = TXT_UnitName.Text.Trim();
            string strProjectCode = DDL_GDProject.SelectedValue;

            if (!ShareClass.CheckStringRight(strPlace))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDDBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMainArea))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strArea))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSubcontractor))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFBSBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAreaDescription))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZYMSBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strTheSystem))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXTBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strUnitCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWGCBHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strUnitName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDWGCMCBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZXM").ToString().Trim()+"')", true);
                return;
            }

            GDAreaBLL gDAreaBLL = new GDAreaBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDAreaSql = "from GDArea as gDArea where id = " + intID;
                IList listGDArea = gDAreaBLL.GetAllGDAreas(strGDAreaSql);
                if (listGDArea != null && listGDArea.Count > 0)
                {
                    GDArea gDArea = (GDArea)listGDArea[0];

                    gDArea.Place = strPlace;
                    gDArea.MainArea = strMainArea;
                    gDArea.Area = strArea;
                    gDArea.Subcontractor = strSubcontractor;
                    gDArea.AreaDescription = strAreaDescription;
                    gDArea.TheSystem = strTheSystem;
                    gDArea.UnitCode = strUnitCode;
                    gDArea.UnitName = strUnitName;

                    gDArea.ProjectCode = strProjectCode;

                    gDAreaBLL.UpdateGDArea(gDArea, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDArea gDArea = new GDArea();
                gDArea.Place = strPlace;
                gDArea.MainArea = strMainArea;
                gDArea.Area = strArea;
                gDArea.Subcontractor = strSubcontractor;
                gDArea.AreaDescription = strAreaDescription;
                gDArea.TheSystem = strTheSystem;
                gDArea.UnitCode = strUnitCode;
                gDArea.UnitName = strUnitName;

                gDArea.ProjectCode = strProjectCode;

                gDArea.IsMark = 0;
                gDArea.UserCode = strUserCode;

                gDAreaBLL.AddGDArea(gDArea);
            }

            //Response.Redirect("TTGDAreaList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDAreaBLL gDAreaBLL = new GDAreaBLL();
        string strGDAreaSql = "from GDArea as gDArea where id = " + id;
        IList listGDArea = gDAreaBLL.GetAllGDAreas(strGDAreaSql);
        if (listGDArea != null && listGDArea.Count > 0)
        {
            GDArea gDArea = (GDArea)listGDArea[0];
            TXT_Place.Text = gDArea.Place;
            TXT_MainArea.Text = gDArea.MainArea;
            TXT_Area.Text = gDArea.Area;
            TXT_Subcontractor.Text = gDArea.Subcontractor;
            TXT_AreaDescription.Text = gDArea.AreaDescription;
            TXT_TheSystem.Text = gDArea.TheSystem;
            TXT_UnitCode.Text = gDArea.UnitCode;
            TXT_UnitName.Text = gDArea.UnitName;

            DDL_GDProject.SelectedValue = gDArea.ProjectCode;
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
}