using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDJoinRevisionEdit : System.Web.UI.Page
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
            string strCode = TXT_Code.Text.Trim();
            string strDescription = TXT_Description.Text.Trim();

            if (!ShareClass.CheckStringRight(strCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDMBNWFFZF").ToString().Trim()+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDescription))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSMBNWFFZF").ToString().Trim()+"')", true);
                return;
            }

            GDJointRevisionBLL gDJointRevisionBLL = new GDJointRevisionBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDJointRevisionSql = "from GDJointRevision as gDJointRevision where id = " + intID;
                IList listGDJointRevision = gDJointRevisionBLL.GetAllGDJointRevisions(strGDJointRevisionSql);
                if (listGDJointRevision != null && listGDJointRevision.Count > 0)
                {
                    GDJointRevision gDJointRevision = (GDJointRevision)listGDJointRevision[0];

                    gDJointRevision.Code = strCode;
                    gDJointRevision.Description = strDescription;

                    gDJointRevisionBLL.UpdateGDJointRevision(gDJointRevision, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDJointRevision gDJointRevision = new GDJointRevision();
                gDJointRevision.Code = strCode;
                gDJointRevision.Description = strDescription;

                gDJointRevision.IsMark = 0;
                gDJointRevision.UserCode = strUserCode;

                gDJointRevisionBLL.AddGDJointRevision(gDJointRevision);
            }

            Response.Redirect("TTGDJoinRevisionList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDJointRevisionBLL gDJointRevisionBLL = new GDJointRevisionBLL();
        string strGDJointRevisionSql = "from GDJointRevision as gDJointRevision where id = " + id;
        IList listGDJointRevision = gDJointRevisionBLL.GetAllGDJointRevisions(strGDJointRevisionSql);
        if (listGDJointRevision != null && listGDJointRevision.Count > 0)
        {
            GDJointRevision gDJointRevision = (GDJointRevision)listGDJointRevision[0];
            TXT_Code.Text = gDJointRevision.Code;
            TXT_Description.Text = gDJointRevision.Description;
        }
    }
}