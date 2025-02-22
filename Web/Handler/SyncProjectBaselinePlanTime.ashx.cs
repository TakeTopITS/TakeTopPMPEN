using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace TakeTopGantt.handler
{
    /// <summary>
    /// SyncProjectBaselinePlanTime 的摘要说明
    /// </summary>
    public class SyncProjectBaselinePlanTime : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            //string strPID = context.Request.Form["pid"];   //文本框内容(Get传参对应QueryString)

            string strPID = context.Request.QueryString["pid"].ToString().Trim();

            string strProjectID, strPlanVerID;

            if (strPID != "0" | strPID == null)
            {
                strProjectID = strPID.Substring(0, strPID.Length - 2);

                strPlanVerID = strPID.Substring(strProjectID.Length, 2);
                if (strPlanVerID.Substring(0, 1) == "0")
                {
                    strPlanVerID = strPlanVerID.Substring(1, 1);
                }
            }
            else
            {
                return;
            }

            if (GanttShareClass.CheckUserCanUpdatePlan(strPID) == false || GanttShareClass.CheckIsCanUpdatePlanByProjectStatus(strPID) == false)
            {
                return;
            }

            string strHQL;
            strHQL = "Update T_ImplePLan Set BaseLine_Start_Date = BeginTime,BaseLine_End_Date = EndTime,baseline_percent_done = Percent_Done Where ProjectID = " + strProjectID + " and VerID = " + strPlanVerID;

            try
            {
                GanttShareClass.RunSqlCommand(strHQL);
            }
            catch
            {
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}