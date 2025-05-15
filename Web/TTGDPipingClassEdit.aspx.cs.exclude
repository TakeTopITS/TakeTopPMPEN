using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPipingClassEdit : System.Web.UI.Page
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
            string strLevelClass = TXT_LevelClass.Text.Trim();
            string strLineLevel = TXT_LineLevel.Text.Trim();
            string strMediumCode = TXT_MediumCode.Text.Trim();
            string strSinceNumber = TXT_SinceNumber.Text.Trim();
            string strPNo = TXT_PNo.Text.Trim();
            string strRT = TXT_RT.Text.Trim();
            string strDocking = TXT_Docking.Text.Trim();
            string strBranch = TXT_Branch.Text.Trim();
            string strSplice = TXT_Splice.Text.Trim();
            string strAttached = TXT_Attached.Text.Trim();
            string strHotHandler = TXT_HotHandler.Text.Trim();
            string strPMIMaterial = TXT_PMIMaterial.Text.Trim();
            string strMaterial = TXT_Material.Text.Trim();
            string strWeldingMaterial = TXT_WeldingMaterial.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();

            if (!ShareClass.CheckStringRight(strLevelClass))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDJBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strLineLevel))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGXJBBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMediumCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJZDHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSinceNumber))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZBHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPNOBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRT))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRTBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDocking))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDJBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strBranch))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZGBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSplice))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCJBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strAttached))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFSBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strHotHandler))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRCLBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strPMIMaterial))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZPMIGPCLBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strMaterial))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWeldingMaterial))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHJCLBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemark))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }

            GDPipingClassBLL gDPipingClassBLL = new GDPipingClassBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDPipingClassSql = "from GDPipingClass as gDPipingClass where id = " + intID;
                IList listGDPipingClass = gDPipingClassBLL.GetAllGDPipingClasss(strGDPipingClassSql);
                if (listGDPipingClass != null && listGDPipingClass.Count > 0)
                {
                    GDPipingClass gDPipingClass = (GDPipingClass)listGDPipingClass[0];

                    gDPipingClass.LevelClass = strLevelClass;
                    gDPipingClass.LineLevel = strLineLevel;
                    gDPipingClass.MediumCode = strMediumCode;
                    gDPipingClass.SinceNumber = strSinceNumber;
                    gDPipingClass.PNo = strPNo;
                    gDPipingClass.RT = strRT;
                    gDPipingClass.Docking = strDocking;
                    gDPipingClass.Branch = strBranch;
                    gDPipingClass.Splice = strSplice;
                    gDPipingClass.Attached = strAttached;
                    gDPipingClass.HotHandler = strHotHandler;
                    gDPipingClass.PMIMaterial = strPMIMaterial;
                    gDPipingClass.Material = strMaterial;
                    gDPipingClass.WeldingMaterial = strWeldingMaterial;
                    gDPipingClass.Remark = strRemark;

                    gDPipingClassBLL.UpdateGDPipingClass(gDPipingClass, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDPipingClass gDPipingClass = new GDPipingClass();
                gDPipingClass.LevelClass = strLevelClass;
                gDPipingClass.LineLevel = strLineLevel;
                gDPipingClass.MediumCode = strMediumCode;
                gDPipingClass.SinceNumber = strSinceNumber;
                gDPipingClass.PNo = strPNo;
                gDPipingClass.RT = strRT;
                gDPipingClass.Docking = strDocking;
                gDPipingClass.Branch = strBranch;
                gDPipingClass.Splice = strSplice;
                gDPipingClass.Attached = strAttached;
                gDPipingClass.HotHandler = strHotHandler;
                gDPipingClass.PMIMaterial = strPMIMaterial;
                gDPipingClass.Material = strMaterial;
                gDPipingClass.WeldingMaterial = strWeldingMaterial;
                gDPipingClass.Remark = strRemark;

                gDPipingClass.IsMark = 0;
                gDPipingClass.UserCode = strUserCode;

                gDPipingClassBLL.AddGDPipingClass(gDPipingClass);
            }

            Response.Redirect("TTGDPipingClassList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDPipingClassBLL gDPipingClassBLL = new GDPipingClassBLL();
        string strGDPipingClassSql = "from GDPipingClass as gDPipingClass where id = " + id;
        IList listGDPipingClass = gDPipingClassBLL.GetAllGDPipingClasss(strGDPipingClassSql);
        if (listGDPipingClass != null && listGDPipingClass.Count > 0)
        {
            GDPipingClass gDPipingClass = (GDPipingClass)listGDPipingClass[0];
            TXT_LevelClass.Text = gDPipingClass.LevelClass;
            TXT_LineLevel.Text = gDPipingClass.LineLevel;
            TXT_MediumCode.Text = gDPipingClass.MediumCode;
            TXT_SinceNumber.Text = gDPipingClass.SinceNumber;
            TXT_PNo.Text = gDPipingClass.PNo;
            TXT_RT.Text = gDPipingClass.RT;
            TXT_Docking.Text = gDPipingClass.Docking;
            TXT_Branch.Text = gDPipingClass.Branch;
            TXT_Splice.Text = gDPipingClass.Splice;
            TXT_Attached.Text = gDPipingClass.Attached;
            TXT_HotHandler.Text = gDPipingClass.HotHandler;
            TXT_PMIMaterial.Text = gDPipingClass.PMIMaterial;
            TXT_Material.Text = gDPipingClass.Material;
            TXT_WeldingMaterial.Text = gDPipingClass.WeldingMaterial;
            TXT_Remark.Text = gDPipingClass.Remark;
        }
    }
}