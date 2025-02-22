using System;
using System.Linq;
using TakeTopGantt.models;


public partial class TTWorkPlanGanttForProjectForTraditional : System.Web.UI.Page
{
    protected int pid = 1;
    protected int projectID = 1;

    protected long g_start = 0;
    protected long g_end = 0;

    protected static string strUserCode, strProjectID, strProjectName, strVerID;

    public string strSystemVerType, strBusinessType, strProjectStatus, strUserIsCanUpdatePlan;

    protected void Page_Load(object sender, EventArgs e)
    {
        //ToInt32�Ὣ����ʶ��Ϊint�����ݱ�Ϊ0
        pid = Convert.ToInt32(Request["pid"]);
        //��ȡ��С�Ŀ�ʼʱ������Ľ���ʱ��
        //pid = Math.Max(pid, 1);

        strProjectStatus = ShareClass.GetProjectStatus(pid.ToString());
        strSystemVerType = Session["SystemVersionType"].ToString();
        strBusinessType = Request.QueryString["BusinessType"];

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

        string strUserCode = Session["UserCode"].ToString();
        strProjectID = Request.QueryString["pid"];
        if (strProjectID == null)
        {
            strProjectID = "1";
            strProjectName = ShareClass.GetProjectName(strProjectID);
        }

        Session["ProjectIDForGantt"] = strProjectID;

        strVerID = Request.QueryString["VerID"];
        if (strVerID == null)
        {
            if (Session["VerIDForGantt"] != null)
            {
                strVerID = Session["VerIDForGantt"].ToString();
            }
            else
            {
                strVerID = ShareClass.GetProjectPlanVersion(strProjectID, "InUse").ToString();
                if (strVerID == "0")
                {
                    strVerID = ShareClass.GetLargestProjectPlanVerID(strProjectID);
                }
            }
        }
        else
        {
            Session["VerIDForGantt"] = strVerID;
        }

        //�жϵ�ǰ�û��ܷ���ļƻ�
        strUserIsCanUpdatePlan = LB_UserIsCanUpdatePlan.Text;

        LB_VerID.Text = strVerID;

        projectID = pid;

        pid = GetPIDForGantt(int.Parse(strProjectID), int.Parse(strVerID));

        if (pid <= 0)
        {
            Response.Redirect("TTWorkPlanGanttForProjectForTraditional.aspx?pid=1");
        }

        //��c#��,Ӧ��ʹ��DateTime.Now������new DateTime() ����ȡ��ǰʱ��
        DateTime today = DateTime.Now;
        g_start = datetime2MS(today);
        g_end = datetime2MS(today.AddYears(1));


        //��ȡ������Ŀ��ʱ�䷶Χ
        TakeTopGantt.models.extganttDataContext db = new extganttDataContext();
        var allTasks = db.task.Where(b => b.pid == pid);
        try
        {
            //�������ͼ��ʱ�䷶Χ
            g_start = datetime2MS(allTasks.Min(s => s.start_date).Value);
            g_end = datetime2MS(allTasks.Max(s => s.end_date).Value);
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
        }

        if (Page.IsPostBack == false)
        {

            try
            {
                //�����ð���Ŀ�ƻ����ȸ��ĵ�ǰʱ����Ŀ��ɽ���
                ShareClass.UpdateProjectScheduleByActivityPlanSchedule(strProjectID);

                //�Ѹ����˴���Ϊ�ո���Ϊ��Ϊ��
                ShareClass.UpdateProjectWorkPlanLeaderCodeToNotNull(strProjectID, strVerID);

                //���»�׼�汾�ƻ��Ľ���
                if (strVerID == ShareClass.GetProjectPlanVerID(strProjectID, "Baseline").ToString())
                {
                    string strHQL;

                    strHQL = "Update T_ImplePlan Set Percent_Done = 100,baseline_percent_done = 100,Expense = Budget,ActualHour = WorkHour";
                    strHQL += " Where ProjectID = " + strProjectID + " and VerID = " + strVerID + " And End_Date <= now()";
                    ShareClass.RunSqlCommand(strHQL);
                }

                //�жϵ�ǰ�û��ܷ���ļƻ�
                LB_UserIsCanUpdatePlan.Text = ShareClass.CheckUserIsCanUpdatePlan(projectID.ToString(), strVerID);
                strUserIsCanUpdatePlan = LB_UserIsCanUpdatePlan.Text;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);
            }
        }
    }

    protected long datetime2MS(DateTime dt)
    {
        return (long)(dt - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    protected int GetPIDForGantt(int intProjectID, int intVerID)
    {
        string strVerID, strPID;

        if (intVerID < 10)
        {
            strVerID = "0" + intVerID.ToString();
        }
        else
        {
            strVerID = intVerID.ToString();
        }

        strPID = strProjectID + strVerID;

        return int.Parse(strPID);
    }
}