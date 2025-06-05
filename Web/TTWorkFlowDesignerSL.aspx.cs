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

public partial class TTWorkFlowDesignerSL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL, strTemName, strWFDefinition;
        string strIdentifyString;

        strIdentifyString = Request.QueryString["IdentifyString"];

        strHQL = "Select TemName,WFDefinition From T_WorkFlowTemplate Where DesignType = 'SL' and IdentifyString = " + "'" + strIdentifyString + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");
        if(ds.Tables[0].Rows.Count == 0)
        {
            return;
        }

        strTemName = ds.Tables[0].Rows[0][0].ToString().Trim();
        strWFDefinition = ds.Tables[0].Rows[0][1].ToString().Trim();

        //this.Title = "工作流模板：" + strTemName + "设计" ;             

        if (Page.IsPostBack == false)
        {
            TB_CopyRight.Text = System.Configuration.ConfigurationManager.AppSettings["CopyRight"];
            TB_WFIdentifyString.Text = strIdentifyString;
            TB_WFXML.Text = strWFDefinition;
            TB_WFName.Text = strTemName;

            BT_SaveWFDefinition.Attributes.Add("onclick", "if (self.frames['_WFSetAreaFrame'].document.all( 'BT_SaveWFDefinition') != undefined) {self.frames['_WFSetAreaFrame'].document.forms[0]['TB_WFChartString'].value= self.frames['_WFSetAreaFrame'].parent.document.getElementById('TB_WFChartString').value; self.frames['_WFSetAreaFrame'].document.all( 'BT_SaveWFDefinition').click();}");

            try
            {
                DeleteWorkflowTStepByWFDefinition(strTemName, strWFDefinition);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }

        }
    }

    protected void BT_SaveWFChart_Click(object sender, EventArgs e)
    {
        string strTemName, strWFCharString;

        strTemName = TB_WFName.Text.Trim();

        strWFCharString = TB_WFChartString1.Text.Trim() + TB_WFChartString2.Text.Trim() + TB_WFChartString3.Text.Trim() + TB_WFChartString4.Text.Trim();

        try
        {
            SaveWFChart(strTemName, strWFCharString);

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCCG") + "');</script>");
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "');</script>");
        }
    }

    //删除在流程图中不存在的步骤
    protected void DeleteWorkflowTStepByWFDefinition(string strTemName, string strWFDefinition)
    {
        string strHQL;
        IList lst;

        string strGUID, strStepID;

        strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.TemName =" + "'" + strTemName + "'";
        WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
        lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);
        WorkFlowTStep workFlowTStep = new WorkFlowTStep();

        for (int i = 0; i < lst.Count; i++)
        {
            workFlowTStep = (WorkFlowTStep)lst[i];
            strGUID = workFlowTStep.GUID.Trim();
            strStepID = workFlowTStep.StepID.ToString();

            if (strWFDefinition.IndexOf(strGUID) < 0)
            {
                workFlowTStepBLL.DeleteWorkFlowTStep(workFlowTStep);
            }
        }
    }

    protected void SaveWFChart(string strTemName, string strWFChartString)
    {
        if (strWFChartString != "")
        {
            var binaryData = Convert.FromBase64String(strWFChartString);

            string strDateTime = DateTime.Now.ToString("yyyyMMddHHMMssff");
            string strChartURL = "Doc\\" + "WorkFlowTemplate\\" + strTemName + strDateTime + ".jpg";
            var imageFilePath = Server.MapPath("Doc") + "\\" + "WorkFlowTemplate\\" + strTemName + strDateTime + ".jpg";


            if (File.Exists(imageFilePath))
            { File.Delete(imageFilePath); }
            var stream = new System.IO.FileStream(imageFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Write(binaryData, 0, binaryData.Length);
            stream.Close();

            string strHQL = "Update T_WorkFlowTemplate Set ChartURL = " + "'" + strChartURL + "'" + " Where TemName = " + "'" + strTemName + "'";
            ShareClass.RunSqlCommand(strHQL);
        }
    }
}
