using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZObjectBigDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString(); ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["strDLCode"]))
            {
                string strDLCode = Request.QueryString["strDLCode"].ToString();
                HF_DLCode.Value = strDLCode;

                DataObjectBigBinder(strDLCode);
            }

            TXT_DLCode.ReadOnly = true;
            TXT_DLName.ReadOnly = true;
        }
    }


    private void DataObjectBigBinder(string strDLCode)
    {
        WZMaterialDLBLL wZMaterialDLBLL = new WZMaterialDLBLL();
        string strWZMaterialDLHQL = string.Format(@"from WZMaterialDL as wZMaterialDL where wZMaterialDL.DLCode = '{0}'", strDLCode);
        IList listWZMaterialDL = wZMaterialDLBLL.GetAllWZMaterialDLs(strWZMaterialDLHQL);

        if (listWZMaterialDL != null && listWZMaterialDL.Count > 0)
        {
            WZMaterialDL wZMaterialDL = (WZMaterialDL)listWZMaterialDL[0];

            TXT_DLCode.Text = wZMaterialDL.DLCode;
            TXT_DLName.Text = wZMaterialDL.DLName;
            TXT_DLDesc.Text = wZMaterialDL.DLDesc;

            TXT_DLDesc.BackColor = Color.CornflowerBlue;
        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        WZMaterialDLBLL wZMaterialDLBLL = new WZMaterialDLBLL();
        string strDLCode = TXT_DLCode.Text.Trim();
        string strDLName = TXT_DLName.Text.Trim();
        string strDLDesc = TXT_DLDesc.Text.Trim();

        if (!ShareClass.CheckStringRight(strDLDesc))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDLMSBNSFFZFC").ToString().Trim()+"')", true);
            return;
        }
        if (strDLDesc.Length > 30)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDLMSBNCG30GZFTXSY30GZFC").ToString().Trim()+"')", true);
            return;
        }

        if (!string.IsNullOrEmpty(strDLCode))
        {
            WZMaterialDL wZMaterialDL = new WZMaterialDL();

            wZMaterialDL.DLCode = strDLCode;
            wZMaterialDL.DLName = strDLName;
            wZMaterialDL.DLDesc = strDLDesc;

            wZMaterialDLBLL.UpdateWZMaterialDL(wZMaterialDL, strDLCode);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDJMZNSBJDLMSCZWX").ToString().Trim()+"')", true);
            return;
        }

        Response.Redirect("TTWZObjectBigList.aspx");
    }
}