using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using System.Text;
using System.IO;
using System.Web.Mail;

using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;
using TakeTopWF;

public partial class TTWFChartViewJS : System.Web.UI.Page
{
    public string strIdentifyString;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL, strTemName, strWFDefinition, strWLID, strGUID, strGUIDStep, strNewGUIDStep;
        string strIdentifyString;

        string strUserCode = Session["UserCode"].ToString();

        strWLID = Request.QueryString["WLID"];

        try
        {
            strWLID = Request.QueryString["WLID"];
            strTemName = ShareClass.GetWorkflowTemNameByWLID(strWLID);
        }
        catch
        {
            strWLID = "0";
            strTemName = Request.QueryString["TemName"];
        }

        strIdentifyString = ShareClass.GetWLTemplateIdentifyString(strTemName);

        _WFDesignerFrame.Src = "WFDesigner/TTTakeTopWFChartViewJS.aspx?IdentifyString=" + strIdentifyString + "&WLID=" + strWLID;

        strHQL = "Select TemName,WFDefinition From T_WorkFlowTemplate Where TemName = " + "'" + strTemName + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");

        if (ds.Tables[0].Rows.Count == 0 )
        {
            Response.Redirect("TTDisplayErrors.aspx");
        }

        strWFDefinition = ds.Tables[0].Rows[0][1].ToString().Trim();

        this.Title = LanguageHandle.GetWord("GongZuoLiu") + ": " + strTemName + LanguageHandle.GetWord("LiuChengTu");

        if (Page.IsPostBack == false)
        {
            TB_CopyRight.Text = System.Configuration.ConfigurationManager.AppSettings["CopyRight"];
            TB_WFIdentifyString.Text = strIdentifyString;
            TB_WFName.Text = strTemName;

            //取得流程图定义
            TB_WFXML.Text = WFMFFlowDefinitionHandle.GetWorkflowDefinition(strIdentifyString, strWLID, strTemName, strWFDefinition);
        }
    }

  
}