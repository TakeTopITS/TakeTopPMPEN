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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAttachWorkFlowTemplate : System.Web.UI.Page
{
    string strRelatedType, strRelatedID, strRelatedName;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName;

        IList lst;

        strLangCode = Session["LangCode"].ToString();
        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];
        strRelatedName = Request.QueryString["RelatedName"];

        if (strRelatedType == "ProjectTask")
        {
            strRelatedName = GetProjectTaskName(strRelatedID);
            //this.Title = "项目任务：" + strRelatedID + " " + strRelatedName + " 的相关工作流模板";
        }

        if (strRelatedType == "ProjectPlan")
        {
            strRelatedName = GetProjectPlanName(strRelatedID);
            //this.Title = "项目计划：" + strRelatedID + " " + strRelatedName + " 的相关工作流模板";
        }

        if (strRelatedType == "Req")
        {
            strRelatedName = GetRequirementName(strRelatedID);
            //this.Title = "需求：" + strRelatedID + " " + strRelatedName + "的工作流模板设置";
        }

        if (strRelatedType == "Meeting")
        {
            strRelatedName = GetMeetingName(strRelatedID);
            //this.Title = "会议：" + strRelatedID + " " + strRelatedName + "的工作流模板设置";
        }

        if (strRelatedType == "ProjectRisk")
        {
            strRelatedName = GetProjectRiskName(strRelatedID);
            //this.Title = "风险：" + strRelatedID + " " + strRelatedName + "的工作流模板设置";
        }

        if (strRelatedType == "Constract")
        {
            strRelatedName = GetConstractName(strRelatedID);
            //this.Title = "合同：" + GetConstractCode(strRelatedID) + " " + strRelatedName + "的工作流模板设置";
        }

        if(strRelatedType == "Tender")
        {
            strRelatedName = GetTenderName(strRelatedID);
        }

        if (strRelatedType == "CustomerService")
        {
            strRelatedName = GetCustomerQuestionName(strRelatedID);
            //this.Title = LanguageHandle.GetWord("KEHUFUWU").ToString().Trim()  + strRelatedID + " " + strRelatedName + "的工作流模板设置";
        }

        if (strRelatedType == "ProjectType")
        {
            strRelatedID = "0";
        }

        if (strRelatedType == "BMBidType")
        {
            strRelatedID = "0";
        }

        if (strRelatedType == "ConstractType")
        {
            strRelatedID = "0";
        }


        LB_RelatedID.Text = strRelatedID;

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            strHQL = "Select * from T_WLType as wlType ";
            strHQL += " Where wlType.LangCode = " + "'" + strLangCode + "'";
            strHQL += " and wlType.Type In (Select Type From T_WorkFlowTemplate)";
            strHQL += " Order By wlType.SortNumber ASC";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WLType");
            DataGrid3.DataSource = ds;
            DataGrid3.DataBind();

            LoadRelatedWorkFlowTemplate();
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strID = e.Item.Cells[0].Text.Trim();
            string strRelatedID = LB_RelatedID.Text.Trim();

            strHQL = "from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType = " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + " and relatedWorkFlowTemplate.ID = " + strID;
            RelatedWorkFlowTemplateBLL relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
            lst = relatedWorkFlowTemplateBLL.GetAllRelatedWorkFlowTemplates(strHQL);
            RelatedWorkFlowTemplate relatedWorkFlowTemplate = (RelatedWorkFlowTemplate)lst[0];
            relatedWorkFlowTemplateBLL.DeleteRelatedWorkFlowTemplate(relatedWorkFlowTemplate);

            LoadRelatedWorkFlowTemplate();
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strWFType = ((Button)e.Item.FindControl("BT_WFType")).ToolTip.Trim();

            string strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = " + "'" + strWFType + "'";
            strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
            WorkFlowTemplateBLL workFlowTemplateBLL = new ProjectMgt.BLL.WorkFlowTemplateBLL();
            IList lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LB_WFType.Text = strWFType;
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strWFTemName = ((Button)e.Item.FindControl("BT_WFTemName")).Text.Trim();

            if (strRelatedType == "ProjectType" | strRelatedType == "BMBidType")
            {
                strHQL = "from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType =  " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedName = '" + strRelatedName + "' and relatedWorkFlowTemplate.WFTemplateName = " + "'" + strWFTemName + "'";
            }
            else
            {
                strHQL = "from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType =  " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID + " and relatedWorkFlowTemplate.WFTemplateName = " + "'" + strWFTemName + "'";
            }
            RelatedWorkFlowTemplateBLL relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
            lst = relatedWorkFlowTemplateBLL.GetAllRelatedWorkFlowTemplates(strHQL);

            if (lst.Count == 0)
            {
                RelatedWorkFlowTemplate relatedWorkFlowTemplate = new RelatedWorkFlowTemplate();
                relatedWorkFlowTemplate.RelatedType = strRelatedType;
                relatedWorkFlowTemplate.RelatedID = int.Parse(strRelatedID);
                relatedWorkFlowTemplate.RelatedName = strRelatedName;
                relatedWorkFlowTemplate.WFTemplateName = strWFTemName;
                relatedWorkFlowTemplate.IdentifyString = GetWFTemplateIdentifyString(strWFTemName);

                try
                {
                    relatedWorkFlowTemplateBLL.AddRelatedWorkFlowTemplate(relatedWorkFlowTemplate);

                    LoadRelatedWorkFlowTemplate();
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSBZGGZLMBYBTJBNZFTJ").ToString().Trim() + "')", true);
            }
        }
    }

    protected void LoadRelatedWorkFlowTemplate()
    {
        string strHQL;
        IList lst;

        RelatedWorkFlowTemplateBLL relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
        if (strRelatedType == "ProjectType")
        {
            strHQL = "from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType =  " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedName = " + "'" + strRelatedName + "'";
            relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
            lst = relatedWorkFlowTemplateBLL.GetAllRelatedWorkFlowTemplates(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
        }
        else if (strRelatedType == "BMBidType")
        {
            strHQL = "from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType =  " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedName = " + "'" + strRelatedName + "'";
            relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
            lst = relatedWorkFlowTemplateBLL.GetAllRelatedWorkFlowTemplates(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
        }
        else if (strRelatedType == "ConstractType")
        {
            strHQL = "from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType =  " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedName = " + "'" + strRelatedName + "'";
            relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
            lst = relatedWorkFlowTemplateBLL.GetAllRelatedWorkFlowTemplates(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
        }
        else
        {
            strHQL = "from RelatedWorkFlowTemplate as relatedWorkFlowTemplate where relatedWorkFlowTemplate.RelatedType =  " + "'" + strRelatedType + "'" + " and relatedWorkFlowTemplate.RelatedID = " + strRelatedID;
            relatedWorkFlowTemplateBLL = new RelatedWorkFlowTemplateBLL();
            lst = relatedWorkFlowTemplateBLL.GetAllRelatedWorkFlowTemplates(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();
        }
    }


    protected string GetWFTemplateIdentifyString(string strTemName)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.TemName = " + "'" + strTemName + "'";
        WorkFlowTemplateBLL workFlowTemplateBLL = new WorkFlowTemplateBLL();
        lst = workFlowTemplateBLL.GetAllWorkFlowTemplates(strHQL);

        WorkFlowTemplate workFlowTemplate = (WorkFlowTemplate)lst[0];

        return workFlowTemplate.IdentifyString.Trim();
    }

    protected string GetProjectName(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID = " + strRelatedID;
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.ProjectName.Trim();
    }

    protected string GetProjectTaskName(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.Task.Trim();
    }

    protected string GetProjectPlanName(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkPlan as workPlan where workPlan.ID = " + strPlanID;
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        lst = workPlanBLL.GetAllWorkPlans(strHQL);
        WorkPlan workPlan = (WorkPlan)lst[0];

        return workPlan.Name.Trim();
    }


    protected string GetRequirementName(string strReqID)
    {
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();

        IList lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement.ReqName.Trim();
    }

    protected string GetMeetingName(string strMeetingID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);

        Meeting meeting = (Meeting)lst[0];

        return meeting.Name.Trim();

    }

    protected string GetProjectRiskName(string strRiskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strRiskID;
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);

        ProjectRisk projectRisk = (ProjectRisk)lst[0];

        return projectRisk.Risk.Trim();
    }

    protected string GetConstractCode(string strConstractID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.ConstractID = " + strConstractID;

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        Constract constract = (Constract)lst[0];

        return constract.ConstractCode.Trim();

    }

    protected string GetConstractName(string strConstractID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.ConstractID = " + strConstractID;

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        Constract constract = (Constract)lst[0];

        return constract.ConstractName.Trim();
    }


    protected string GetTenderName(string strTenderID)
    {
        string strHQL;
        IList lst;

        Tender_HYYQBLL tender_HYYQBLL = new Tender_HYYQBLL();
        strHQL = "from Tender_HYYQ as tender_HYYQ where tender_HYYQ.id = " + strTenderID;
        lst = tender_HYYQBLL.GetAllTender_HYYQs(strHQL);

        if (lst.Count > 0)
        {
            Tender_HYYQ tender_HYYQ = (Tender_HYYQ)lst[0];

            return tender_HYYQ.ProjectName.Trim();
        }
        else
        {
            return "";
        }
    }

    protected string GetCustomerQuestionName(string strQuestionID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestion as customerQuestion where customerQuestion.ID = " + strQuestionID;
        CustomerQuestionBLL customerQuestionBLL = new CustomerQuestionBLL();
        lst = customerQuestionBLL.GetAllCustomerQuestions(strHQL);

        CustomerQuestion customerQuestion = (CustomerQuestion)lst[0];

        return customerQuestion.Question.Trim();
    }
    
}
