<%@ WebHandler Language="C#" Class="addTableNameMappingHandler" %>

using System; using System.Resources;
using System.Web;

using ProjectMgt.Model;
using ProjectMgt.BLL;

public class addTableNameMappingHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string result = string.Empty;
        try
        {
            string strTableName = context.Request.Form["strTableName"];
            string strDescription = context.Request.Form["strDescription"];
            string strID = context.Request.Form["intID"];
            int ID = 0;
            int.TryParse(strID, out ID);

            TableNameMapping tableNameMapping = new TableNameMapping();
            tableNameMapping.TableName = strTableName;
            tableNameMapping.Description = strDescription;

            TableNameMappingBLL tableNameMappingBLL = new TableNameMappingBLL();
            if (ID == 0)
            {
                tableNameMappingBLL.AddTableNameMapping(tableNameMapping);
            }
            else {
                tableNameMappingBLL.UpdateTableNameMapping(tableNameMapping, ID);
            }
            result = "Save successful!";
        }
        catch (Exception ex) {
            result = "Save failed, reason:" + ex.Message.ToString();
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(result);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}