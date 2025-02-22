using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTGDOrderCodeEdit : System.Web.UI.Page
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
            string strRemark = TXT_Remark.Text.Trim();

            if (!ShareClass.CheckStringRight(strCode))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZDHBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strDescription))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSMBNWFFZF+"')", true);
                return;
            }
            if (!ShareClass.CheckStringRight(strRemark))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBZBNWFFZF+"')", true);
                return;
            }

            GDOrderCodeBLL gDOrderCodeBLL = new GDOrderCodeBLL();


            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                int intID = 0;
                int.TryParse(HF_ID.Value, out intID);

                string strGDOrderCodeSql = "from GDOrderCode as gDOrderCode where id = " + intID;
                IList listGDOrderCode = gDOrderCodeBLL.GetAllGDOrderCodes(strGDOrderCodeSql);
                if (listGDOrderCode != null && listGDOrderCode.Count > 0)
                {
                    GDOrderCode gDOrderCode = (GDOrderCode)listGDOrderCode[0];

                    gDOrderCode.Code = strCode;
                    gDOrderCode.Description = strDescription;
                    gDOrderCode.Remark = strRemark;

                    gDOrderCodeBLL.UpdateGDOrderCode(gDOrderCode, intID);
                }
            }
            else
            {
                //Ôö¼Ó
                GDOrderCode gDOrderCode = new GDOrderCode();
                gDOrderCode.Code = strCode;
                gDOrderCode.Description = strDescription;
                gDOrderCode.Remark = strRemark;

                gDOrderCode.IsMark = 0;
                gDOrderCode.UserCode = strUserCode;

                gDOrderCodeBLL.AddGDOrderCode(gDOrderCode);
            }

            Response.Redirect("TTGDOrderCodeList.aspx");
        }
        catch (Exception ex)
        { }
    }


    private void BindData(int id)
    {
        GDOrderCodeBLL gDOrderCodeBLL = new GDOrderCodeBLL();
        string strGDOrderCodeSql = "from GDOrderCode as gDOrderCode where id = " + id;
        IList listGDOrderCode = gDOrderCodeBLL.GetAllGDOrderCodes(strGDOrderCodeSql);
        if (listGDOrderCode != null && listGDOrderCode.Count > 0)
        {
            GDOrderCode gDOrderCode = (GDOrderCode)listGDOrderCode[0];
            TXT_Code.Text = gDOrderCode.Code;
            TXT_Description.Text = gDOrderCode.Description;
            TXT_Remark.Text = gDOrderCode.Remark;
        }
    }
}