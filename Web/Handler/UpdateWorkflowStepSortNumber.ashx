<%@ WebHandler Language="C#" Class="UpdateWorkflowStepSortNumber" %>

using System;
using System.Web;
using Newtonsoft.Json;

using System.Data.SqlClient;
using System.Data;
using System.Text;

using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.SessionState;


public class UpdateWorkflowStepSortNumber : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string strGUID = context.Request["GUID"];
        string strSortNumber = context.Request["SortNumber"];
        string strStepName = context.Request["StepName"];

        string strHQL;

        try
        {
            strHQL = string.Format(@"Update t_workflowtstep Set SortNumber = {0} Where GUID = '{1}'", strSortNumber, strGUID);
            ShareClass.RunSqlCommand(strHQL);

            //strHQL = string.Format(@"Update t_workflowtstep Set StepName = '{0}' Where GUID = '{1}'", strStepName, strGUID);
            //ShareClass.RunSqlCommand(strHQL);

            context.Response.ContentType = "text/plain";
            context.Response.Write("Sucessfully");
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile(err.Message.ToString());

            context.Response.ContentType = "text/plain";
            context.Response.Write("False");
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