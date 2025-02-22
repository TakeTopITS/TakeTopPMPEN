using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDApplicationEdit : System.Web.UI.Page
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
            string strLineUser = TXT_LineUser.Text.Trim();
            string strInstructions = TXT_Instructions.Text.Trim();
            string strTheSystem = TXT_TheSystem.Text.Trim();
            string strRemark = TXT_Remark.Text.Trim();

            if (!ShareClass.CheckStringRight(strLineUser))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZGXYTBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strInstructions))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSMBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strTheSystem))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXTBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemark))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBZBNWFFZF+"')", true);
                return;
            }

            GDApplicationBLL gDApplicationBLL = new GDApplicationBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDApplicationSql = "from GDApplication as gDApplication where id = " + intID;
                IList listGDApplication = gDApplicationBLL.GetAllGDApplications(strGDApplicationSql);
                if (listGDApplication != null && listGDApplication.Count > 0)
                {
                    GDApplication gDApplication = (GDApplication)listGDApplication[0];

                    gDApplication.LineUser = strLineUser;
                    gDApplication.Instructions = strInstructions;
                    gDApplication.TheSystem = strTheSystem;
                    gDApplication.Remark = strRemark;

                    gDApplicationBLL.UpdateGDApplication(gDApplication, intID);
                }
            }
            else
            {
                //Ôö¼Ó

                GDApplication gDApplication = new GDApplication();
                gDApplication.LineUser = strLineUser;
                gDApplication.Instructions = strInstructions;
                gDApplication.TheSystem = strTheSystem;
                gDApplication.Remark = strRemark;
                gDApplication.IsMark = 0;
                gDApplication.UserCode = strUserCode;

                gDApplicationBLL.AddGDApplication(gDApplication);
            }

            Response.Redirect("TTGDApplicationList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDApplicationBLL gDApplicationBLL = new GDApplicationBLL();
        string strGDApplicationSql = "from GDApplication as gDApplication where id = " + id;
        IList listGDApplication = gDApplicationBLL.GetAllGDApplications(strGDApplicationSql);
        if (listGDApplication != null && listGDApplication.Count > 0)
        {
            GDApplication gDApplication = (GDApplication)listGDApplication[0];
            TXT_LineUser.Text = gDApplication.LineUser;
            TXT_Instructions.Text = gDApplication.Instructions;
            TXT_TheSystem.Text = gDApplication.TheSystem;
            TXT_Remark.Text = gDApplication.Remark;
        }
    }
}