using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;

public partial class TTWorkPlanGanttForProjectStandardActivityCompareMain : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strProjectID;
        string strStandardPlanURL, strActivityPlanURL;

        strProjectID = Request.QueryString["ProjectID"];


        try
        {
            if (Session["WeekendFirstDay"] == null)
            {
                //ȡ����ĩ��ʼ��
                Session["WeekendFirstDay"] = ShareClass.GetWeekendFirstDay();
            }

            if (Session["WeekendSecondDay"] == null)
            {
                //ȡ����ĩ������
                Session["WeekendSecondDay"] = ShareClass.GetWeekendSecondDay();
            }

            if (Session["WeekendsAreWorkdays"] == null)
            {
                //ȡ����ĩ�Ƿ�����
                Session["WeekendsAreWorkdays"] = ShareClass.GetWeekendsAreWorkdays();
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }

        //�����ð���Ŀ�ƻ����ȸ��ĵ�ǰʱ����Ŀ��ɽ���
        ShareClass.UpdateProjectScheduleByActivityPlanSchedule(strProjectID);

        strActivityPlanURL = "TTWorkPlanGanttForProject.aspx?pid=" + strProjectID + "&verID=" + ShareClass.GetProjectPlanVerID(strProjectID, "InUse") + "&BusinessType=COMPARE";
        IFrame_ActivityGanttPlan.Attributes.Add("src", strActivityPlanURL);

        strStandardPlanURL = "TTWorkPlanGanttForProject.aspx?pid=" + strProjectID + "&verID=" + ShareClass.GetProjectPlanVerID(strProjectID, "Baseline") + "&BusinessType=COMPARE";
        IFrame_StandardGanttPlan.Attributes.Add("src", strStandardPlanURL);
    }
}