using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDWPSCodeEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string strWPSNo = Request.QueryString["id"].ToString();
                HF_WPSNo.Value = strWPSNo;

                BindData(strWPSNo);
            }
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strWPSNo = TXT_WPSNo.Text.Trim();
            string strDescription = TXT_Description.Text.Trim();
            string strRemarks = TXT_Remarks.Text.Trim();

            if (!ShareClass.CheckStringRight(strWPSNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWPSNOBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDescription))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDESCRIPTIONBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemarks))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZREMARKSBNWFFZF").ToString().Trim()+"')", true);
                return;
            }

            GDWPSCodeBLL gDWPSCodeBLL = new GDWPSCodeBLL();


            if (!string.IsNullOrEmpty(HF_WPSNo.Value))
            {
                //ÐÞ¸Ä
                string strNewWPSNo = HF_WPSNo.Value;

                string strGDWPSCodeSql = "from GDWPSCode as gDWPSCode where WPSNo = '" + strNewWPSNo + "'";
                IList listGDWPSCode = gDWPSCodeBLL.GetAllGDWPSCodes(strGDWPSCodeSql);
                if (listGDWPSCode != null && listGDWPSCode.Count > 0)
                {
                    GDWPSCode gDWPSCode = (GDWPSCode)listGDWPSCode[0];

                    gDWPSCode.WPSNo = strWPSNo;
                    gDWPSCode.Description = strDescription;
                    gDWPSCode.Remarks = strRemarks;

                    gDWPSCodeBLL.UpdateGDWPSCode(gDWPSCode, strNewWPSNo);
                }
            }
            else
            {
                //Ôö¼Ó
                GDWPSCode gDWPSCode = new GDWPSCode();
                gDWPSCode.WPSNo = strWPSNo;
                gDWPSCode.Description = strDescription;
                gDWPSCode.Remarks = strRemarks;

                gDWPSCode.IsMark = 0;
                gDWPSCode.UserCode = strUserCode;

                gDWPSCodeBLL.AddGDWPSCode(gDWPSCode);
            }

            Response.Redirect("TTGDWPSCodeList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(string strWPSNo)
    {
        GDWPSCodeBLL gDWPSCodeBLL = new GDWPSCodeBLL();
        string strGDWPSCodeSql = "from GDWPSCode as gDWPSCode where WPSNo = '" + strWPSNo + "'";
        IList listGDWPSCode = gDWPSCodeBLL.GetAllGDWPSCodes(strGDWPSCodeSql);
        if (listGDWPSCode != null && listGDWPSCode.Count > 0)
        {
            GDWPSCode gDWPSCode = (GDWPSCode)listGDWPSCode[0];
            TXT_WPSNo.Text = gDWPSCode.WPSNo;
            TXT_Description.Text = gDWPSCode.Description;
            TXT_Remarks.Text = gDWPSCode.Remarks;
        }
    }
}