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

public partial class TTGDLineWeldEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            BindPipelineData();
            BindPipeClassData();
            BindMediumData();
            BindPressureData();
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

            string strPipelineLevel = TXT_PipelineLevel.Text.Trim();
            string strArea = DDL_Area.SelectedValue;
            string strLineNumber = TXT_LineNumber.Text.Trim();
            string strOrderNumber = TXT_OrderNumber.Text.Trim();
            string strLineLevel = TXT_LineLevel.Text.Trim();
            string strMediumCode = TXT_MediumCode.Text.Trim();
            string strIsom_no = TXT_Isom_no.Text.Trim();
            string strPipelineRule = TXT_PipelineRule.Text.Trim();
            string strEdition = TXT_Edition.Text.Trim();
            string strPublicTime = TXT_PublicTime.Text.Trim();
            string strPressurePack1 = TXT_PressurePack1.Text.Trim();
            string strPressureMpa = TXT_PressureMpa.Text.Trim();
            string strDesignTemperature = TXT_DesignTemperature.Text.Trim();

            if (string.IsNullOrEmpty(strProjectCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMBNWKZ+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPipelineLevel))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGDDJBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strArea))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZYBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strLineNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGXHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strOrderNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZCXBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strLineLevel))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGXDJBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMediumCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZJZDHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strIsom_no))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDXTHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPipelineRule))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGDGGBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strEdition))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCBNWFFZF+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPublicTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZFBRBNWKBC+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPressurePack1))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSYB1BNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPressureMpa))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSJYLBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDesignTemperature))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSJWDBNWFFZF+"')", true);
                return;
            }

            GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //一个项目应该不允许有重复的单纯图号
                string strCheckProjectIsomSQL = string.Format(@"select * from T_GDLineWeld 
                    where ProjectCode = '{0}' 
                    and Isom_no = '{1}'", strProjectCode, strIsom_no);
                DataTable dtCheckProjectIsom = ShareClass.GetDataSetFromSql(strCheckProjectIsomSQL, "CheckProjectIsom").Tables[0];
                if (dtCheckProjectIsom != null && dtCheckProjectIsom.Rows.Count > 0)
                {
                    string strNewID = ShareClass.ObjectToString(dtCheckProjectIsom.Rows[0]["ID"]);

                    if (HF_ID.Value.Trim() != strNewID.Trim())
                    {
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMYJCZDDXTHZXTXDXTH+"')", true);
                        return;
                    }
                }

                //修改
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDLineWeldSql = "from GDLineWeld as gDLineWeld where id = " + intID;
                IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldSql);
                if (listGDLineWeld != null && listGDLineWeld.Count > 0)
                {
                    GDLineWeld gDLineWeld = (GDLineWeld)listGDLineWeld[0];

                    gDLineWeld.ProjectCode = strProjectCode;

                    gDLineWeld.PipelineLevel = strPipelineLevel;
                    gDLineWeld.Area = strArea;
                    gDLineWeld.LineNumber = strLineNumber;
                    gDLineWeld.OrderNumber = strOrderNumber;
                    gDLineWeld.LineLevel = strLineLevel;
                    gDLineWeld.MediumCode = strMediumCode;
                    gDLineWeld.Isom_no = strIsom_no;
                    gDLineWeld.PipelineRule = strPipelineRule;
                    gDLineWeld.Edition = strEdition;
                    gDLineWeld.PublicTime = strPublicTime;
                    gDLineWeld.PressurePack1 = strPressurePack1;
                    gDLineWeld.PressureMpa = strPressureMpa;
                    gDLineWeld.DesignTemperature = strDesignTemperature;

                    gDLineWeldBLL.UpdateGDLineWeld(gDLineWeld, intID);
                }
            }
            else
            {
                //一个项目应该不允许有重复的单纯图号
                string strCheckProjectIsomSQL = string.Format(@"select * from T_GDLineWeld 
                    where ProjectCode = '{0}' 
                    and Isom_no = '{1}'", strProjectCode, strIsom_no);
                DataTable dtCheckProjectIsom = ShareClass.GetDataSetFromSql(strCheckProjectIsomSQL, "CheckProjectIsom").Tables[0];
                if (dtCheckProjectIsom != null && dtCheckProjectIsom.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXMYJCZDDXTHZXTXDXTH+"')", true);
                    return;
                }

                //增加
                GDLineWeld gDLineWeld = new GDLineWeld();

                gDLineWeld.ProjectCode = strProjectCode;

                gDLineWeld.PipelineLevel = strPipelineLevel;
                gDLineWeld.Area = strArea;
                gDLineWeld.LineNumber = strLineNumber;
                gDLineWeld.OrderNumber = strOrderNumber;
                gDLineWeld.LineLevel = strLineLevel;
                gDLineWeld.MediumCode = strMediumCode;
                gDLineWeld.Isom_no = strIsom_no;
                gDLineWeld.PipelineRule = strPipelineRule;
                gDLineWeld.Edition = strEdition;
                gDLineWeld.PublicTime = strPublicTime;
                gDLineWeld.PressurePack1 = strPressurePack1;
                gDLineWeld.PressureMpa = strPressureMpa;
                gDLineWeld.DesignTemperature = strDesignTemperature;

                gDLineWeld.IsMark = 0;
                gDLineWeld.UserCode = strUserCode;

                gDLineWeldBLL.AddGDLineWeld(gDLineWeld);
            }

            //Response.Redirect("TTGDLineWeldList.aspx");
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDLineWeldBLL gDLineWeldBLL = new GDLineWeldBLL();
        string strGDLineWeldSql = "from GDLineWeld as gDLineWeld where id = " + id;
        IList listGDLineWeld = gDLineWeldBLL.GetAllGDLineWelds(strGDLineWeldSql);
        if (listGDLineWeld != null && listGDLineWeld.Count > 0)
        {
            GDLineWeld gDLineWeld = (GDLineWeld)listGDLineWeld[0];

            string strProjectCode = gDLineWeld.ProjectCode;
            DDL_GDProject.SelectedValue = strProjectCode;

            GDAreaBLL gDAreaBLL = new GDAreaBLL();
            string strGDAreaHQL = "from GDArea as gDArea where ProjectCode = '" + strProjectCode + "'";
            IList listGDArea = gDAreaBLL.GetAllGDAreas(strGDAreaHQL);

            DDL_Area.DataSource = listGDArea;
            DDL_Area.DataTextField = "Area";
            DDL_Area.DataValueField = "ID";
            DDL_Area.DataBind();

            TXT_PipelineLevel.Text = gDLineWeld.PipelineLevel;
            DDL_Area.SelectedValue = gDLineWeld.Area;
            TXT_LineNumber.Text = gDLineWeld.LineNumber;
            TXT_OrderNumber.Text = gDLineWeld.OrderNumber;
            TXT_LineLevel.Text = gDLineWeld.LineLevel;
            TXT_MediumCode.Text = gDLineWeld.MediumCode;
            TXT_Isom_no.Text = gDLineWeld.Isom_no;
            TXT_PipelineRule.Text = gDLineWeld.PipelineRule;
            TXT_Edition.Text = gDLineWeld.Edition;
            TXT_PublicTime.Text = gDLineWeld.PublicTime.ToString();
            TXT_PressurePack1.Text = gDLineWeld.PressurePack1;
            TXT_PressureMpa.Text = gDLineWeld.PressureMpa;
            TXT_DesignTemperature.Text = gDLineWeld.DesignTemperature;
        }
    }


    private void BindPipelineData()
    {
        string strPipelineHQL = "select * from T_GDPipeline";
        DataTable dtPipeline = ShareClass.GetDataSetFromSql(strPipelineHQL, "strPipeHQL").Tables[0];

        TXT_PipelineLevel.DataSource = dtPipeline;
        TXT_PipelineLevel.DataTextField = "PipeGrade";
        TXT_PipelineLevel.DataValueField = "PipeGrade";
        TXT_PipelineLevel.DataBind();

        TXT_PipelineLevel.Items.Insert(0, new ListItem("", ""));
    }


    private void BindPipeClassData()
    {
        string strPipeclassHQL = "select distinct LineLevel from T_GDPipingClass";
        DataTable dtPipeclass = ShareClass.GetDataSetFromSql(strPipeclassHQL, "strPipeclassHQL").Tables[0];

        TXT_LineLevel.DataSource = dtPipeclass;
        TXT_LineLevel.DataTextField = "LineLevel";
        TXT_LineLevel.DataValueField = "LineLevel";
        TXT_LineLevel.DataBind();

        TXT_LineLevel.Items.Insert(0, new ListItem("", ""));
    }


    private void BindMediumData()
    {
        string strMediumHQL = "select distinct MediumCode from T_GDPipingClass";
        DataTable dtMedium = ShareClass.GetDataSetFromSql(strMediumHQL, "strMediumHQL").Tables[0];

        TXT_MediumCode.DataSource = dtMedium;
        TXT_MediumCode.DataValueField = "MediumCode";
        TXT_MediumCode.DataTextField = "MediumCode";
        TXT_MediumCode.DataBind();

        TXT_MediumCode.Items.Insert(0, new ListItem("", ""));
    }


    private void BindPressureData()
    {
        string strPressureHQL = "select * from T_GDPressure";
        DataTable dtPressure = ShareClass.GetDataSetFromSql(strPressureHQL, "strPressureHQL").Tables[0];

        TXT_PressurePack1.DataSource = dtPressure;
        TXT_PressurePack1.DataTextField = "PressureCode";
        TXT_PressurePack1.DataValueField = "PressureCode";
        TXT_PressurePack1.DataBind();

        TXT_PressurePack1.Items.Insert(0, new ListItem("", ""));
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