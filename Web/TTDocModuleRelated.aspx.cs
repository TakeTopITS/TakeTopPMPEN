using ProjectMgt.BLL;
using ProjectMgt.Model;
using System; using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTDocModuleRelated : System.Web.UI.Page
{
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();
        strUserName = Session["UserName"] == null ? "" : Session["UserName"].ToString();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (!IsPostBack)
        {
            LoadProjectYYUPData();

            if (!string.IsNullOrEmpty(Request.QueryString["DocID"]))
            {
                string strDocID = ShareClass.ObjectToString(Request.QueryString["DocID"]);
                HF_DocID.Value = strDocID;
                //string strID = ShareClass.ObjectToString(Request.QueryString["ID"]);
                //HF_ID.Value = strID;

                BindDocModuleData(strDocID);
            }

            
        }
    }




    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strProductLine = DDL_ProductLine.SelectedValue;
            string strSubordinateIndustry = DDL_SubordinateIndustry.SelectedValue;
            string strModuleIDs = HF_ModuleIDs.Value.Trim();
            string strModuleNames = TXT_ModuleNames.Text.Trim();

            string strStartAmount = TXT_StartAmount.Text.Trim();
            string strEndAmount = TXT_EndAmount.Text.Trim();
            string strStartPersonDay = TXT_StartPersonDay.Text.Trim();
            string strEndPersonDay = TXT_EndPersonDay.Text.Trim();

            decimal decimalStartAmount = 0;
            decimal.TryParse(strStartAmount, out decimalStartAmount);
            decimal decimalEndAmount = 0;
            decimal.TryParse(strEndAmount, out decimalEndAmount);
            int intStartPersonDay = 0;
            int.TryParse(strStartPersonDay, out intStartPersonDay);
            int intEndPersonDay = 0;
            int.TryParse(strEndPersonDay, out intEndPersonDay);


            if (string.IsNullOrEmpty(strProductLine))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCPXBYXWKBC")+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strSubordinateIndustry))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXYBYXWKBC")+"')", true);
                return;
            }
            if (string.IsNullOrEmpty(strModuleIDs) || string.IsNullOrEmpty(strModuleNames))
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZMKBYXWKBC")+"')", true);
                return;
            }
            
            if (!string.IsNullOrEmpty(HF_ID.Value))
            {
                //ÐÞ¸Ä
                string strID = HF_ID.Value;

                string strUpdateDocModuleSQL = string.Format(@"update T_DocModuleRelated
                            set ProductLine = '{0}',
                            SubordinateIndustry = '{1}',
                            ModuleIDs = '{2}',
                            ModuleNames = '{3}',
                            StartAmount = {4},
                            EndAmount = {5},
                            StartPersonDay = {6},
                            EndPersonDay = {7}
                            where ID = {8}", strProductLine, strSubordinateIndustry, strModuleIDs, strModuleNames, decimalStartAmount, decimalEndAmount, intStartPersonDay, intEndPersonDay, strID);
                ShareClass.RunSqlCommand(strUpdateDocModuleSQL);
            }
            else
            {
                //Ôö¼Ó

                int intDocID = 0;
                int.TryParse(HF_DocID.Value, out intDocID);

                string strInsertDocModuleSQL = string.Format(@"insert into T_DocModuleRelated(ProductLine,SubordinateIndustry,ModuleIDs,ModuleNames,StartAmount,EndAmount,StartPersonDay,EndPersonDay,DocID)
                            values('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8})", strProductLine, strSubordinateIndustry, strModuleIDs, strModuleNames, decimalStartAmount, decimalEndAmount, intStartPersonDay, intEndPersonDay, intDocID);
                ShareClass.RunSqlCommand(strInsertDocModuleSQL);

            }


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "LoadParentLit();", true);
        }
        catch (Exception ex)
        { }
    }


    private void BindDocModuleData(string strDocID)
    {
        string strDocModuleHQL = string.Format(@"select * from T_DocModuleRelated
                        where DocID = {0}", strDocID);
        DataTable dtDocModule = ShareClass.GetDataSetFromSql(strDocModuleHQL, "DocModule").Tables[0];
        if (dtDocModule != null && dtDocModule.Rows.Count > 0)
        {
            DataRow drDocModule = dtDocModule.Rows[0];

            TXT_ID.Text = ShareClass.ObjectToString(drDocModule["ID"]);
            HF_ID.Value = ShareClass.ObjectToString(drDocModule["ID"]);

            DDL_ProductLine.SelectedValue = ShareClass.ObjectToString(drDocModule["ProductLine"]);
            DDL_SubordinateIndustry.SelectedValue = ShareClass.ObjectToString(drDocModule["SubordinateIndustry"]);
            HF_ModuleIDs.Value = ShareClass.ObjectToString(drDocModule["ModuleIDs"]);
            TXT_ModuleNames.Text = ShareClass.ObjectToString(drDocModule["ModuleNames"]);
            TXT_StartAmount.Text = ShareClass.ObjectToString(drDocModule["StartAmount"]);
            TXT_EndAmount.Text = ShareClass.ObjectToString(drDocModule["EndAmount"]);
            TXT_StartPersonDay.Text = ShareClass.ObjectToString(drDocModule["StartPersonDay"]);
            TXT_EndPersonDay.Text = ShareClass.ObjectToString(drDocModule["EndPersonDay"]);
            

        }
    }





    private void LoadProjectYYUPData()
    {
        string strProductLineSQL = @"select * from T_ProjectProductLine_YYUP";
        DataTable dtProductLine = ShareClass.GetDataSetFromSql(strProductLineSQL, "ProductLine").Tables[0];

        DDL_ProductLine.DataSource = dtProductLine;
        DDL_ProductLine.DataTextField = "Name";
        DDL_ProductLine.DataValueField = "Name";
        DDL_ProductLine.DataBind();

       


        string strSubordinateIndustrySQL = @"select * from T_ProjectSubordinateIndustry_YYUP";
        DataTable dtSubordinateIndustry = ShareClass.GetDataSetFromSql(strSubordinateIndustrySQL, "SubordinateIndustry").Tables[0];

        DDL_SubordinateIndustry.DataSource = dtSubordinateIndustry;
        DDL_SubordinateIndustry.DataTextField = "Name";
        DDL_SubordinateIndustry.DataValueField = "Name";
        DDL_SubordinateIndustry.DataBind();


    }


}