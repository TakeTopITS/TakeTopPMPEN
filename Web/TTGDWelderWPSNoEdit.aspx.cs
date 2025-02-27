using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDWelderWPSNoEdit : System.Web.UI.Page
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
            string strWelder_no = TXT_Welder_no.Text.Trim();
            string strWPSNo = TXT_WPSNo.Text.Trim();
            string strRemarks = TXT_Remarks.Text.Trim();

            if (!ShareClass.CheckStringRight(strWelder_no))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWELDERNOBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strWPSNo))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZWPSNOBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemarks))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZREMARKSBNWFFZF").ToString().Trim()+"')", true);
                return;
            }

            GDWelderWPSNoBLL gDWelderWPSNoBLL = new GDWelderWPSNoBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDWelderWPSNoSql = "from GDWelderWPSNo as gDWelderWPSNo where id = " + intID;
                IList listGDWelderWPSNo = gDWelderWPSNoBLL.GetAllGDWelderWPSNos(strGDWelderWPSNoSql);
                if (listGDWelderWPSNo != null && listGDWelderWPSNo.Count > 0)
                {
                    GDWelderWPSNo gDWelderWPSNo = (GDWelderWPSNo)listGDWelderWPSNo[0];

                    gDWelderWPSNo.Welder_no = strWelder_no;
                    gDWelderWPSNo.WPSNo = strWPSNo;
                    gDWelderWPSNo.Remarks = strRemarks;

                    gDWelderWPSNoBLL.UpdateGDWelderWPSNo(gDWelderWPSNo, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDWelderWPSNo gDWelderWPSNo = new GDWelderWPSNo();
                gDWelderWPSNo.Welder_no = strWelder_no;
                gDWelderWPSNo.WPSNo = strWPSNo;
                gDWelderWPSNo.Remarks = strRemarks;

                gDWelderWPSNo.IsMark = 0;
                gDWelderWPSNo.UserCode = strUserCode;

                gDWelderWPSNoBLL.AddGDWelderWPSNo(gDWelderWPSNo);
            }

            Response.Redirect("TTGDWelderWPSNoList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDWelderWPSNoBLL gDWelderWPSNoBLL = new GDWelderWPSNoBLL();
        string strGDWelderWPSNoSql = "from GDWelderWPSNo as gDWelderWPSNo where id = " + id;
        IList listGDWelderWPSNo = gDWelderWPSNoBLL.GetAllGDWelderWPSNos(strGDWelderWPSNoSql);
        if (listGDWelderWPSNo != null && listGDWelderWPSNo.Count > 0)
        {
            GDWelderWPSNo gDWelderWPSNo = (GDWelderWPSNo)listGDWelderWPSNo[0];
            TXT_Welder_no.Text = gDWelderWPSNo.Welder_no;
            TXT_WPSNo.Text = gDWelderWPSNo.WPSNo;
            TXT_Remarks.Text = gDWelderWPSNo.Remarks;
        }
    }
}