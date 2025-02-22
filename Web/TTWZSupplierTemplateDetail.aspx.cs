using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTWZSupplierTemplateDetail : System.Web.UI.Page
{
    public string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] != null ? Session["UserCode"].ToString().Trim() : "";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {

            BindSupplierWorkFlowData();

        }
    }

    protected void BT_Save_Click(object sender, EventArgs e)
    {
        string strID = HF_ID.Value;
        string strTemplateContent = TXT_TemplateContent.Value.Trim();

        if (string.IsNullOrEmpty(strTemplateContent))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZSRRWLCNR+"')", true);
            return;
        }
        if (!ShareClass.CheckStringRight(strTemplateContent))
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZRWLCNRBNWFFZFC+"')", true);
            return;
        }

        //strTemplateContent = strTemplateContent.StartsWith("<br />") ? strTemplateContent : "<br />" + strTemplateContent;

        if (!string.IsNullOrEmpty(strID))
        {
            //ÐÞ¸Ä
            try
            {
                string strSupplierWorkFlowSQL = string.Format(@"update T_WZSupplierWorkFlow set TemplateContent = '{0}' where ID = {1}", strTemplateContent, strID);
                ShareClass.RunSqlCommand(strSupplierWorkFlowSQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZBCCG+"')", true);
            }
            catch (Exception ex) { }
        }
        else
        {
            //ÐÂÔö
            try
            {
                string strSupplierWorkFlowSQL = string.Format(@"insert into T_WZSupplierWorkFlow(TemplateContent,CreateTime) values('{0}',now())", strTemplateContent);
                ShareClass.RunSqlCommand(strSupplierWorkFlowSQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+Resources.lang.ZZTJCG+"')", true);
            }
            catch (Exception ex) { }
        }
    }




    private void BindSupplierWorkFlowData()
    {
        string strWZSupplierWorkFlowSql = @"select * from T_WZSupplierWorkFlow";
        DataTable dtWZSupplierWorkFlow = ShareClass.GetDataSetFromSql(strWZSupplierWorkFlowSql, "SupplierWorkFlow").Tables[0];
        if (dtWZSupplierWorkFlow != null && dtWZSupplierWorkFlow.Rows.Count > 0)
        {
            DataRow drSupplierWorkFlow = dtWZSupplierWorkFlow.Rows[0];

            string strID = ShareClass.ObjectToString(drSupplierWorkFlow["ID"]);
            string strTemplateContent = ShareClass.ObjectToString(drSupplierWorkFlow["TemplateContent"]);

            HF_ID.Value = strID;
            TXT_ID.Text = strID;
            TXT_TemplateContent.Value = strTemplateContent;
        }
    }
}