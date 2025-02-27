using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDPressureObjectEdit : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string strID = Request.QueryString["id"].ToString();
                HF_ID.Value = strID;

                BindData(strID);
            }
        }
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            string strPressureObject = TXT_PressureObject.Text.Trim();

            if (!ShareClass.CheckStringRight(strPressureObject))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSYJZBNWFFZF").ToString().Trim()+"')", true);
                return;
            }

            GDPressureObjectBLL gDPressureObjectBLL = new GDPressureObjectBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                string strID = HF_ID.Value;
                int intID = 0;
                int.TryParse(strID, out intID);

                string strGDPressureObjectSql = "from GDPressureObject as gDPressureObject where ID = " + intID;
                IList listGDPressureObject = gDPressureObjectBLL.GetAllGDPressureObjects(strGDPressureObjectSql);
                if (listGDPressureObject != null && listGDPressureObject.Count > 0)
                {
                    GDPressureObject gDPressureObject = (GDPressureObject)listGDPressureObject[0];

                    gDPressureObject.PressureObject = strPressureObject;

                    gDPressureObjectBLL.UpdateGDPressureObject(gDPressureObject, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDPressureObject gDPressureObject = new GDPressureObject();
                gDPressureObject.PressureObject = strPressureObject;

                gDPressureObject.IsMark = 0;
                gDPressureObject.UserCode = strUserCode;

                gDPressureObjectBLL.AddGDPressureObject(gDPressureObject);
            }

            Response.Redirect("TTGDPressureObjectList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(string strID)
    {
        GDPressureObjectBLL gDPressureObjectBLL = new GDPressureObjectBLL();
        string strGDPressureObjectSql = "from GDPressureObject as gDPressureObject where ID = " + strID;
        IList listGDPressureObject = gDPressureObjectBLL.GetAllGDPressureObjects(strGDPressureObjectSql);
        if (listGDPressureObject != null && listGDPressureObject.Count > 0)
        {
            GDPressureObject gDPressureObject = (GDPressureObject)listGDPressureObject[0];
            TXT_PressureObject.Text = gDPressureObject.PressureObject;
        }
    }
}