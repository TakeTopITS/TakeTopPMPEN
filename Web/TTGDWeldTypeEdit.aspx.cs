using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDWeldTypeEdit : System.Web.UI.Page
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
            string strType = TXT_Type.Text.Trim();
            string strDescription = TXT_Description.Text.Trim();
            string strFactor = TXT_Factor.Text.Trim();
            string strCode = TXT_Code.Text.Trim();
            string strKeyCode = TXT_KeyCode.Text.Trim();

            if (!ShareClass.CheckStringRight(strType))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZLXBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDescription))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSMBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckIsNumber(strFactor))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZXSZNWXSHZZS+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strKeyCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZKEYBNWFFZF+"')", true);
                return;
            }

            decimal decimalFactor = 0;
            decimal.TryParse(strFactor, out decimalFactor);

            GDWeldTypeBLL gDWeldTypeBLL = new GDWeldTypeBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDWeldTypeSql = "from GDWeldType as gDWeldType where id = " + intID;
                IList listGDWeldType = gDWeldTypeBLL.GetAllGDWeldTypes(strGDWeldTypeSql);
                if (listGDWeldType != null && listGDWeldType.Count > 0)
                {
                    GDWeldType gDWeldType = (GDWeldType)listGDWeldType[0];

                    gDWeldType.Type = strType;
                    gDWeldType.Description = strDescription;
                    gDWeldType.Factor = decimalFactor;
                    gDWeldType.Code = strCode;
                    gDWeldType.KeyCode = strKeyCode;

                    gDWeldTypeBLL.UpdateGDWeldType(gDWeldType, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDWeldType gDWeldType = new GDWeldType();
                gDWeldType.Type = strType;
                gDWeldType.Description = strDescription;
                gDWeldType.Factor = decimalFactor;
                gDWeldType.Code = strCode;
                gDWeldType.KeyCode = strKeyCode;

                gDWeldType.IsMark = 0;
                gDWeldType.UserCode = strUserCode;

                gDWeldTypeBLL.AddGDWeldType(gDWeldType);
            }

            Response.Redirect("TTGDWeldTypeList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDWeldTypeBLL gDWeldTypeBLL = new GDWeldTypeBLL();
        string strGDWeldTypeSql = "from GDWeldType as gDWeldType where id = " + id;
        IList listGDWeldType = gDWeldTypeBLL.GetAllGDWeldTypes(strGDWeldTypeSql);
        if (listGDWeldType != null && listGDWeldType.Count > 0)
        {
            GDWeldType gDWeldType = (GDWeldType)listGDWeldType[0];
            TXT_Type.Text = gDWeldType.Type;
            TXT_Description.Text = gDWeldType.Description;
            TXT_Factor.Text = gDWeldType.Factor.ToString();
            TXT_Code.Text = gDWeldType.Code;
            TXT_KeyCode.Text = gDWeldType.KeyCode;
        }
    }
}