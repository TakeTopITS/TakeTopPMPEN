using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDThicknessEdit : System.Web.UI.Page
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
            string strLineLevel = TXT_LineLevel.Text.Trim();
            string strSize = TXT_Size.Text.Trim();
            string strRules = TXT_Rules.Text.Trim();
            string strThickness = TXT_Thickness.Text.Trim();
            string strHotHandler = TXT_HotHandler.Text.Trim();

            if (!ShareClass.CheckStringRight(strLineLevel))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGXJBBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strSize))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCCBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRules))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strThickness))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHDZNWXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strHotHandler))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZRCLBNWFFZF").ToString().Trim()+"')", true);
                return;
            }

            decimal decimalThickness = 0;
            decimal.TryParse(strThickness, out decimalThickness);

            GDThicknessBLL gDThicknessBLL = new GDThicknessBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDThicknessSql = "from GDThickness as gDThickness where id = " + intID;
                IList listGDThickness = gDThicknessBLL.GetAllGDThicknesss(strGDThicknessSql);
                if (listGDThickness != null && listGDThickness.Count > 0)
                {
                    GDThickness gDThickness = (GDThickness)listGDThickness[0];

                    gDThickness.LineLevel = strLineLevel;
                    gDThickness.Size = strSize;
                    gDThickness.Rules = strRules;
                    gDThickness.Thickness = decimalThickness;
                    gDThickness.HotHandler = strHotHandler;

                    gDThicknessBLL.UpdateGDThickness(gDThickness, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDThickness gDThickness = new GDThickness();
                gDThickness.LineLevel = strLineLevel;
                gDThickness.Size = strSize;
                gDThickness.Rules = strRules;
                gDThickness.Thickness = decimalThickness;
                gDThickness.HotHandler = strHotHandler;

                gDThickness.IsMark = 0;
                gDThickness.UserCode = strUserCode;

                gDThicknessBLL.AddGDThickness(gDThickness);
            }

            Response.Redirect("TTGDThicknessList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDThicknessBLL gDThicknessBLL = new GDThicknessBLL();
        string strGDThicknessSql = "from GDThickness as gDThickness where id = " + id;
        IList listGDThickness = gDThicknessBLL.GetAllGDThicknesss(strGDThicknessSql);
        if (listGDThickness != null && listGDThickness.Count > 0)
        {
            GDThickness gDThickness = (GDThickness)listGDThickness[0];
            TXT_LineLevel.Text = gDThickness.LineLevel;
            TXT_Size.Text = gDThickness.Size;
            TXT_Rules.Text = gDThickness.Rules;
            TXT_Thickness.Text = gDThickness.Thickness.ToString();
            TXT_HotHandler.Text = gDThickness.HotHandler;
        }
    }
}