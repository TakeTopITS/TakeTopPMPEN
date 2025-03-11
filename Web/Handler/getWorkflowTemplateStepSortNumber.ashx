<%@ WebHandler Language="C#" Class="getWorkflowTemplateStepSortNumber" %>

using System;
using System.Resources;
using System.Web;
using System.Collections;
using ProjectMgt.BLL;
using Newtonsoft.Json;
using System.Data;


public class getWorkflowTemplateStepSortNumber : IHttpHandler
{
    string result = "";

    public void ProcessRequest(HttpContext context)
    {
        ////获取一同发送过来的参数
        //string command = context.Request["cmd"];

        context.Response.ContentType = "text/plain";

        //用来传回去的内容
        //context.Response.Write("Hello World");

        Get_Data01(context);
    }

    public void Get_Data01(HttpContext context)
    {
        string strHQL;
        string strTSortNumber, strTNextSortNumber;

        string strGUID = context.Request["GUID"];

        HttpContext.Current.Response.ContentType = "text/plain";

        try
        {
            DataSet ds1;
            strHQL = string.Format(@"Select SortNumber,NextSortNumber from T_WorkFlowTStep where GUID = '{0}'", strGUID);
            ds1 = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTStep");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                strTSortNumber = ds1.Tables[0].Rows[0]["SortNumber"].ToString();
                strTNextSortNumber = ds1.Tables[0].Rows[0]["NextSortNumber"].ToString();
                HttpContext.Current.Response.Write(strTSortNumber + ">" + strTNextSortNumber);
            }
            else
            {
                HttpContext.Current.Response.Write("0>0");
            }
        }
        catch (Exception err)
        {
            HttpContext.Current.Response.Write("0>0");
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
