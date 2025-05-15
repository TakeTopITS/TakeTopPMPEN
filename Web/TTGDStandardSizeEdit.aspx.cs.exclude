using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDStandardSizeEdit : System.Web.UI.Page
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
            string strSize = TXT_Size.Text.Trim();
            string strDB = TXT_DB.Text.Trim();
            string strNPS = TXT_NPS.Text.Trim();
            string strODGB = TXT_ODGB.Text.Trim();
            string strODANSI = TXT_ODANSI.Text.Trim();
            string strBQMainCode = TXT_BQMainCode.Text.Trim();
            string strBQSubCode = TXT_BQSubCode.Text.Trim();

            if (string.IsNullOrEmpty(strSize))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCCBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strDB))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDBZNWXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsAllNumber(strNPS))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZNPSZNWZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strODGB))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZODGBZNWXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strODANSI))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZODANSIZNWXSHZZS").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strBQMainCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZZYBHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strBQSubCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }

            decimal decimalDB = 0;
            decimal.TryParse(strDB, out decimalDB);
            int intNPS = 0;
            int.TryParse(strNPS, out intNPS);
            decimal decimalODGB = 0;
            decimal.TryParse(strODGB, out decimalODGB);
            decimal decimalODANSI = 0;
            decimal.TryParse(strODANSI, out decimalODANSI);

            GDStandardSizeBLL gDStandardSizeBLL = new GDStandardSizeBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDStandardSizeSql = "from GDStandardSize as gDStandardSize where id = " + intID;
                IList listGDStandardSize = gDStandardSizeBLL.GetAllGDStandardSizes(strGDStandardSizeSql);
                if (listGDStandardSize != null && listGDStandardSize.Count > 0)
                {
                    GDStandardSize gDStandardSize = (GDStandardSize)listGDStandardSize[0];

                    gDStandardSize.Size = strSize;
                    gDStandardSize.DB = decimalDB;
                    gDStandardSize.NPS = intNPS;
                    gDStandardSize.ODGB = decimalODGB;
                    gDStandardSize.ODANSI = decimalODANSI;
                    gDStandardSize.BQMainCode = strBQMainCode;
                    gDStandardSize.BQSubCode = strBQSubCode;

                    gDStandardSizeBLL.UpdateGDStandardSize(gDStandardSize, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDStandardSize gDStandardSize = new GDStandardSize();
                gDStandardSize.Size = strSize;
                gDStandardSize.DB = decimalDB;
                gDStandardSize.NPS = intNPS;
                gDStandardSize.ODGB = decimalODGB;
                gDStandardSize.ODANSI = decimalODANSI;
                gDStandardSize.BQMainCode = strBQMainCode;
                gDStandardSize.BQSubCode = strBQSubCode;

                gDStandardSize.IsMark = 0;
                gDStandardSize.UserCode = strUserCode;

                gDStandardSizeBLL.AddGDStandardSize(gDStandardSize);
            }

            Response.Redirect("TTGDStandardSizeList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDStandardSizeBLL gDStandardSizeBLL = new GDStandardSizeBLL();
        string strGDStandardSizeSql = "from GDStandardSize as gDStandardSize where id = " + id;
        IList listGDStandardSize = gDStandardSizeBLL.GetAllGDStandardSizes(strGDStandardSizeSql);
        if (listGDStandardSize != null && listGDStandardSize.Count > 0)
        {
            GDStandardSize gDStandardSize = (GDStandardSize)listGDStandardSize[0];
            TXT_Size.Text = gDStandardSize.Size;
            TXT_DB.Text = gDStandardSize.DB.ToString();
            TXT_NPS.Text = gDStandardSize.NPS.ToString();
            TXT_ODGB.Text = gDStandardSize.ODGB.ToString();
            TXT_ODANSI.Text = gDStandardSize.ODANSI.ToString();
            TXT_BQMainCode.Text = gDStandardSize.BQMainCode;
            TXT_BQSubCode.Text = gDStandardSize.BQSubCode;
        }
    }
}