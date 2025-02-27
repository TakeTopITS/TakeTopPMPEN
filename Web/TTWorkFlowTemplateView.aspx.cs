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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTWorkFlowTemplateView : System.Web.UI.Page
{
    string strUserCode, strIdentifyString;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
     
        string strTempName;
        IList lst;

        try
        {
            if (Request.QueryString["IdentifyString"] == null)
            {
                strIdentifyString = Session["IdentifyString"].ToString().Trim();
            }
            else
            {
                strIdentifyString = Request.QueryString["IdentifyString"].Trim();
            }  
        }
        catch
        {
            Response.Redirect("TTWorkFlowDesignerError.htm");
            return;
        }

        strTempName = GetWLTemplate(strIdentifyString);
        //this.Title = "工作流模板：" + strTempName + "审批步骤列表";    

        strUserCode = Session["UserCode"].ToString();      

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.TemName = " + "'" + strTempName + "'" + " Order by workFlowTStep.SortNumber ASC";
            WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
            lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);

            DataGrid2 .DataSource = lst;
            DataGrid2.DataBind();

            LB_WorkFlow.Text = strTempName;
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strStepID = ((Button)e.Item.FindControl("BT_StepID")).Text.ToString();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            strHQL = "from WorkFlowTStep as workFlowTStep where workFlowTStep.StepID = " + strStepID;
            WorkFlowTStepBLL workFlowTStepBLL = new WorkFlowTStepBLL();
            lst = workFlowTStepBLL.GetAllWorkFlowTSteps(strHQL);
            WorkFlowTStep workFlowTStep = (WorkFlowTStep)lst[0];

            LB_StepID.Text = strStepID;
            LB_StepName.Text = workFlowTStep.StepName.Trim();
            LB_SortNumber.Text = workFlowTStep.SortNumber.ToString();

            strHQL = "from WorkFlowTStepOperator as workFlowTStepOperator where workFlowTStepOperator.StepID = " + strStepID;
            WorkFlowTStepOperatorBLL workFlowTStepOperatorBLL = new WorkFlowTStepOperatorBLL();
            lst = workFlowTStepOperatorBLL.GetAllWorkFlowTStepOperators(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();    
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SqlWL.Text;

        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetWLTemplate(string strIdentityString)
    {
        string strHQL;
        IList lst;

        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.IdentifyString = " + "'" +strIdentityString+ "'";
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        return workFlowTemplate.TemName.Trim();
    }
   

    protected void SaveWFTemplateDefinationXML(string strIdentifyString, string strMark)
    {
        string strHQL;

        string strXML;

        strXML = Session["WFXMLSession"].ToString().Trim();

        try
        {
            strHQL = "update T_WorkFlowTemplate Set WFDefinition = " + "'" + strXML + "'" + " Where IdentifyString = " + "'" + strIdentifyString + "'";
            ShareClass.RunSqlCommand(strHQL);

            if (strMark == "0")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGZLMBDYBCCG").ToString().Trim()+"')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWBCSBJC").ToString().Trim()+"')", true);
        }
    }
}
