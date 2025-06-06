using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

using TakeTopGantt;

public partial class Handler_UpdateAllTardyPlansToRedColor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        //string strPID = context.Request.Form["pid"];   //文本框内容(Get传参对应QueryString)

        string strPID = Request.QueryString["pid"].Trim();

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
            HttpContext.Current.Response.Write("The project number is empty, please check!");
            return;
        }

        if (GanttShareClass.CheckUserCanUpdatePlan(strPID) == false || GanttShareClass.CheckIsCanUpdatePlanByProjectStatus(strPID) == false)
        {
            HttpContext.Current.Response.Write("You don't have the permission to change the plan. Please check！");
            return;
        }

        string strHQL;
        strHQL = "Update T_ImplePLan Set TaskColor = 'FF0000' Where ";
        strHQL += " to_char(end_Date,'yyyymmdd') < to_char(now(),'yyyymmdd')";
        strHQL += " And Percent_Done < 100";
        strHQL += " And ProjectID = " + strProjectID + " and VerID = " + strPlanVerID;

        try
        {
            GanttShareClass.RunSqlCommand(strHQL);
            HttpContext.Current.Response.Write("Sucess, " + strHQL);
        }
        catch
        {
            HttpContext.Current.Response.Write("Failer, " + strHQL);
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