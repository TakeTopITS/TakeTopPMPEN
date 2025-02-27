using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDWeldersEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string strWelders = Request.QueryString["id"].ToString();
                HF_Welders.Value = strWelders;

                TXT_Welders.ReadOnly = true;

                BindData(strWelders);
            }
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strWelders = TXT_Welders.Text.Trim();
            string strPublicTime = TXT_PublicTime.Text.Trim();
            string strStatus = TXT_Status.Text.Trim();
            string strWelderName = TXT_WelderName.Text.Trim();
            string strRequestCode = TXT_RequestCode.Text.Trim();
            string strCompanyName = TXT_CompanyName.Text.Trim();
            string strQualification = TXT_Qualification.SelectedValue.Trim();
            string strWeldPosition1 = TXT_WeldPosition1.Text.Trim();
            string strWeldPosition2 = TXT_WeldPosition2.Text.Trim();
            string strRemarks = TXT_Remarks.Text.Trim();

            if (!ShareClass.CheckStringRight(strWelders))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHGHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strPublicTime))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZFBRBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strStatus))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZTBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWelderName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMCBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRequestCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCompanyName))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZGongSiLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
                return;
            }
            if (!ShareClass.CheckStringRight(strQualification))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZGBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWeldPosition1))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHJWZ1BNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWeldPosition2))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHJWZ2BNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemarks))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }


            GDWeldersBLL gDWeldersBLL = new GDWeldersBLL();


            if (!string.IsNullOrEmpty(HF_Welders.Value))
            {
                //ÐÞ¸Ä
                string strNewWelders = HF_Welders.Value;

                string strGDWeldersSql = "from GDWelders as gDWelders where Welders = '" + strNewWelders + "'";
                IList listGDWelders = gDWeldersBLL.GetAllGDWelderss(strGDWeldersSql);
                if (listGDWelders != null && listGDWelders.Count > 0)
                {
                    GDWelders gDWelders = (GDWelders)listGDWelders[0];

                    gDWelders.Welders = strWelders;
                    gDWelders.PublicTime = strPublicTime;
                    gDWelders.Status = strStatus;
                    gDWelders.WelderName = strWelderName;
                    gDWelders.RequestCode = strRequestCode;
                    gDWelders.CompanyName = strCompanyName;
                    gDWelders.Qualification = strQualification;
                    gDWelders.WeldPosition1 = strWeldPosition1;
                    gDWelders.WeldPosition2 = strWeldPosition2;
                    gDWelders.Remarks = strRemarks;

                    gDWeldersBLL.UpdateGDWelders(gDWelders, strNewWelders);
                }
            }
            else
            {
                //Ôö¼Ó
                GDWelders gDWelders = new GDWelders();
                gDWelders.Welders = strWelders;
                gDWelders.PublicTime = strPublicTime;
                gDWelders.Status = strStatus;
                gDWelders.WelderName = strWelderName;
                gDWelders.RequestCode = strRequestCode;
                gDWelders.CompanyName = strCompanyName;
                gDWelders.Qualification = strQualification;
                gDWelders.WeldPosition1 = strWeldPosition1;
                gDWelders.WeldPosition2 = strWeldPosition2;
                gDWelders.Remarks = strRemarks;

                gDWelders.IsMark = 0;
                gDWelders.UserCode = strUserCode;

                gDWeldersBLL.AddGDWelders(gDWelders);
            }

            Response.Redirect("TTGDWeldersList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(string strWelders)
    {
        GDWeldersBLL gDWeldersBLL = new GDWeldersBLL();
        string strGDWeldersSql = "from GDWelders as gDWelders where Welders = '" + strWelders + "'";
        IList listGDWelders = gDWeldersBLL.GetAllGDWelderss(strGDWeldersSql);
        if (listGDWelders != null && listGDWelders.Count > 0)
        {
            GDWelders gDWelders = (GDWelders)listGDWelders[0];
            TXT_Welders.Text = gDWelders.Welders;
            TXT_PublicTime.Text = gDWelders.PublicTime.ToString();
            TXT_Status.Text = gDWelders.Status;
            TXT_WelderName.Text = gDWelders.WelderName;
            TXT_RequestCode.Text = gDWelders.RequestCode;
            TXT_CompanyName.Text = gDWelders.CompanyName;
            TXT_Qualification.SelectedValue = gDWelders.Qualification;
            TXT_WeldPosition1.Text = gDWelders.WeldPosition1;
            TXT_WeldPosition2.Text = gDWelders.WeldPosition2;
            TXT_Remarks.Text = gDWelders.Remarks;
        }
    }
}
