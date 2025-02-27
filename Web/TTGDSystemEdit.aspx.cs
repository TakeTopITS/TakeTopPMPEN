using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDSystemEdit : System.Web.UI.Page
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
            string strTheSystem = TXT_TheSystem.Text.Trim();
            string strInstructions = TXT_Instructions.Text.Trim();
            string strMCDate = TXT_MCDate.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();

            if (!ShareClass.CheckStringRight(strTheSystem))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXTHBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strInstructions))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSMBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strMCDate))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMCRBNWKBC").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemark))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }


            GDSystemBLL gDSystemBLL = new GDSystemBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDSystemSql = "from GDSystem as gDSystem where id = " + intID;
                IList listGDSystem = gDSystemBLL.GetAllGDSystems(strGDSystemSql);
                if (listGDSystem != null && listGDSystem.Count > 0)
                {
                    GDSystem gDSystem = (GDSystem)listGDSystem[0];

                    gDSystem.TheSystem = strTheSystem;
                    gDSystem.Instructions = strInstructions;
                    gDSystem.MCDate = strMCDate;
                    gDSystem.Remark = strRemark;

                    gDSystemBLL.UpdateGDSystem(gDSystem, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDSystem gDSystem = new GDSystem();
                gDSystem.TheSystem = strTheSystem;
                gDSystem.Instructions = strInstructions;
                gDSystem.MCDate = strMCDate;
                gDSystem.Remark = strRemark;

                gDSystem.IsMark = 0;
                gDSystem.UserCode = strUserCode;

                gDSystemBLL.AddGDSystem(gDSystem);
            }

            Response.Redirect("TTGDSystemList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDSystemBLL gDSystemBLL = new GDSystemBLL();
        string strGDSystemSql = "from GDSystem as gDSystem where id = " + id;
        IList listGDSystem = gDSystemBLL.GetAllGDSystems(strGDSystemSql);
        if (listGDSystem != null && listGDSystem.Count > 0)
        {
            GDSystem gDSystem = (GDSystem)listGDSystem[0];
            TXT_TheSystem.Text = gDSystem.TheSystem;
            TXT_Instructions.Text = gDSystem.Instructions;
            TXT_MCDate.Text = gDSystem.MCDate.ToString();
            TXT_Remark.Text = gDSystem.Remark;
        }
    }
}