using System; using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using System.Security.Cryptography;
using System.Security.Permissions;
using System.Data.SqlClient;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;
using System.Timers;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        GetUNHandledWorkCount(strUserCode);
    }

    protected void GetUNHandledWorkCount(string strUserCode)
    {
        string strHQL;
        IList lst;
        int i = 0;

        strHQL = "from Collaboration as collaboration where ltrim(rtrim(collaboration.Status)) <> 'Closed' and  collaboration.CoID in  ";
        strHQL += "( select collaborationLog.CoID from CollaborationLog as collaborationLog,CollaborationMember as collaborationMember where collaborationLog.CoID = collaborationMember.CoID ";
        strHQL += " and collaborationLog.CreateTime > collaborationMember.LastLoginTime and rtrim(ltrim(collaborationLog.UserCode)) <> " + "'" + strUserCode + "'" + " and rtrim(ltrim(collaborationMember.UserCode))= " + "'" + strUserCode + "'" + ")";
        strHQL += " Order by collaboration.CoID DESC";
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);
        HL_Collaboration.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");
        HL_Collaboration.NavigateUrl = "TTCollaborationManage.aspx";

        strHQL = "from HeadLine as headLine";
        HeadLineBLL headLineBLL = new HeadLineBLL();
        lst = headLineBLL.GetAllHeadLines(strHQL);
        HL_HeadLine.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");
        HL_HeadLine.NavigateUrl = "TTHeadLine.aspx";

        strHQL = "from WorkFlowStepDetail as workFlowStepDetail where workFlowStepDetail.Status in ('InProgress','Reviewing','Signing','ReReview') ";
        strHQL += " and workFlowStepDetail.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and workFlowStepDetail.WLID in (Select workFlow.WLID from WorkFlow as workFlow where workFlow.Status not in ('Updating','Closed','CaseClosed'))";
        strHQL += " Order by workFlowStepDetail.StepID DESC";
        WorkFlowStepDetailBLL workFlowStepDetailBLL = new WorkFlowStepDetailBLL();
        lst = workFlowStepDetailBLL.GetAllWorkFlowStepDetails(strHQL);
        HL_UnCheckWL.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");
        HL_UnCheckWL.NavigateUrl = "TTWLManage.aspx";


        strHQL = "FROM Mails as mails where mails.ReaderFlag = 0 and mails.UserCode = " + "'" + strUserCode + "'";
        MailsBLL mailsBLL = new MailsBLL();
        lst = mailsBLL.GetAllMailss(strHQL);
        HL_UNReadEMail.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");
        HL_UNReadEMail.NavigateUrl = "TTMailIndex.aspx";

        strHQL = "from Meeting as meeting where meeting.ID in ( select meetingAttendant.MeetingID from MeetingAttendant as meetingAttendant where meetingAttendant.UserCode = " + "'" + strUserCode + "'" + ") and meeting.EndTime > now() order by meeting.ID DESC ";
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);
        HL_UNAttendMeeting.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");

        strHQL = "from ProjectRisk as projectRisk where projectRisk.Status not in ('Resolved','Occurred') and projectRisk.ProjectID in (select project.ProjectID from Project as project where PMCode = " + "'" + strUserCode + "'" + ")";
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);
        HL_UNHandledRisk.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");

        if (lst.Count > 0)
            i = 1;

        strHQL = "from TaskAssignRecord as taskAssignRecord where taskAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and taskAssignRecord.Status in ('Plan','Accepted','InProgress') and taskAssignRecord.ID not in (select taskAssignRecord.PriorID from TaskAssignRecord as taskAssignRecord) ";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.Status <> 'Closed')";
        strHQL += " and taskAssignRecord.TaskID in (select projectTask.TaskID from ProjectTask as projectTask where projectTask.ProjectID in (select project.ProjectID from Project as project where project.Status not in ('New','Hided','Deleted','Archived')))";
        strHQL += " Order by taskAssignRecord.ID DESC";
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);
        HL_UNHnadledTask.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");

        if (lst.Count > 0)
            i = 1;

        strHQL = "from ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
        strHQL += " and reqAssignRecord.Status in ('Plan','Accepted','InProgress') and reqAssignRecord.ID not in (select reqAssignRecord.PriorID from ReqAssignRecord as reqAssignRecord) ";
        strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
        strHQL += " Order by reqAssignRecord.ID DESC";
        ReqAssignRecordBLL reqAssignRecordBLL = new ReqAssignRecordBLL();
        lst = reqAssignRecordBLL.GetAllReqAssignRecords(strHQL);
        HL_UNHandledReq.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");

        if (lst.Count > 0)
            i = 1;

        strHQL = " from Project as project where project.Status = 'Plan' and project.PMCode = " + "'" + strUserCode + "'" + " Order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        HL_UNHandledPro.Text = lst.Count.ToString() + LanguageHandle.GetWord("Tiao");

        if (lst.Count > 0)
            i = 1;
    }
}
