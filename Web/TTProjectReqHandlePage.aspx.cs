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

public partial class TTProjectReqHandlePage : System.Web.UI.Page
{
    string strLangCode;
    string strUserCode, strUserName, strProjectID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strLangCode = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strProjectID = Request.QueryString["ProjectID"];
        if (strProjectID == null)
        {
            strProjectID = "0";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        if (Page.IsPostBack != true)
        {
            LoadReqAssignRecord(strUserCode);
            LoadRequirement(strUserCode);
        }
    }
    protected void BT_UpdateStatus_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strStatus;

        strID = LB_SourceID.Value;
        strStatus = LB_TargetStatus.Value;

        strHQL = "Update T_ReqAssignRecord Set Status =  '" + strStatus + "',MoveTime = now() Where ID = " + strID;

        ShareClass.RunSqlCommand(strHQL);

        LoadReqAssignRecord(strUserCode);

        //Response.Redirect("TTProjectReqHandlePage.aspx?ProjectID=" + strProjectID);
    }

    protected void LoadReqAssignRecord(string strUserCode)
    {
        string strHQL;
        IList lst;

        if (strProjectID != "0")
        {
            strHQL = "Select * from T_ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
            strHQL += " and reqAssignRecord.Status in ('Plan','Accepted','ToHandle')";
            strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
            strHQL += " and reqAssignRecord.ReqID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + ")";
            strHQL += " Order by reqAssignRecord.MoveTime DESC limit 40";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");
            DataList_ToBeHandled.DataSource = ds;
            DataList_ToBeHandled.DataBind();

            strHQL = "Select * from T_ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
            strHQL += " and reqAssignRecord.Status in ('InProgress','InProgress')";
            strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
            strHQL += " and reqAssignRecord.ReqID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + ")";
            strHQL += " Order by reqAssignRecord.MoveTime DESC limit 40";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");
            DataList_Handling.DataSource = ds;
            DataList_Handling.DataBind();

            strHQL = "Select * from T_ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
            strHQL += " and reqAssignRecord.Status in ('Rejected','Suspended','Cancel','Completed','Completed')";   
            strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
            strHQL += " and reqAssignRecord.ReqID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + ")";
            strHQL += " Order by reqAssignRecord.MoveTime DESC limit 40";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");
            DataList_FinishedUnAssigned.DataSource = ds;
            DataList_FinishedUnAssigned.DataBind();

            strHQL = "Select * from T_ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
            strHQL += " and reqAssignRecord.status = 'Assigned'";   
            strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
            strHQL += " and reqAssignRecord.ReqID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID = " + strProjectID + ")";
            strHQL += " Order by reqAssignRecord.MoveTime DESC limit 40";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");
            DataList_Assigned.DataSource = ds;
            DataList_Assigned.DataBind();
        }
        else
        {
            strHQL = "Select * from T_ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
            strHQL += " and reqAssignRecord.Status in ('Plan','Accepted','ToHandle')";
            strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
            strHQL += " and reqAssignRecord.ReqID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID not in (select project.ProjectID from T_Project as project where project.Status in ('New','Review','Hided','Deleted','Archived')))";
            strHQL += " Order by reqAssignRecord.MoveTime DESC limit 40";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");
            DataList_ToBeHandled.DataSource = ds;
            DataList_ToBeHandled.DataBind();

            strHQL = "Select *from T_ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
            strHQL += " and reqAssignRecord.Status in ('InProgress','InProgress')";
            strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
            strHQL += " and reqAssignRecord.ReqID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID not in (select project.ProjectID from T_Project as project where project.Status in ('New','Review','Hided','Deleted','Archived')))";
            strHQL += " Order by reqAssignRecord.MoveTime DESC limit 40";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");
            DataList_Handling.DataSource = ds;
            DataList_Handling.DataBind();

            strHQL = "Select * from T_ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
            strHQL += " and reqAssignRecord.Status in ('Rejected','Suspended','Cancel','Completed','Completed')";   
            strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
            strHQL += " and reqAssignRecord.ReqID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID not in (select project.ProjectID from T_Project as project where project.Status in ('New','Review','Hided','Deleted','Archived')))";
            strHQL += " Order by reqAssignRecord.MoveTime DESC limit 40";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");
            DataList_FinishedUnAssigned.DataSource = ds;
            DataList_FinishedUnAssigned.DataBind();

            strHQL = "Select * from T_ReqAssignRecord as reqAssignRecord where reqAssignRecord.OperatorCode = " + "'" + strUserCode + "'";
            strHQL += " and reqAssignRecord.Status = 'Assigned'";   
            strHQL += " and reqAssignRecord.ReqID in (select requirement.ReqID from T_Requirement as requirement where requirement.Status not in ('Closed','Hided','Deleted','Archived'))";
            strHQL += " and reqAssignRecord.ReqID in (select relatedReq.ReqID from T_RelatedReq as relatedReq where relatedReq.ProjectID not in (select project.ProjectID from T_Project as project where project.Status in ('New','Review','Hided','Deleted','Archived')))";
            strHQL += " Order by reqAssignRecord.MoveTime DESC limit 40";
            ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");
            DataList_Assigned.DataSource = ds;
            DataList_Assigned.DataBind();
        }
    }

    protected void DataGrid5_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid5.CurrentPageIndex = e.NewPageIndex;
        IList lst;

        string strHQL = LB_Sql5.Text;
        TaskAssignRecordBLL taskAssignRecordBLL = new TaskAssignRecordBLL();
        lst = taskAssignRecordBLL.GetAllTaskAssignRecords(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();
    }

    protected void SetReqRecordColor(IList lst, DataList dataList, string strTaskStatus)
    {
        int i;
        DateTime dtNowDate, dtFinishedDate;
        string strStatus;

        //for (i = 0; i < lst.Count; i++)
        //{
        //    dtFinishedDate = DateTime.Parse(((ReqAssignRecord)lst[i]).EndDate.ToString());

        //    dtNowDate = DateTime.Now;

        //    strStatus = ((ReqAssignRecord)lst[i]).Status.Trim();

        //    if (strStatus != "Completed" & strStatus != "Completed")
        //    {
        //        if (strTaskStatus != "Assigned")
        //        {
        //            if (dtFinishedDate < dtNowDate)
        //            {
        //                dataList.Items[i].BackColor = Color.LightPink;
        //            }
        //        }
        //        else
        //        {
        //            dataList.Items[i].BackColor = Color.LightGreen;
        //        }
        //    }
        //    else
        //    {
        //        if (strTaskStatus == "Assigned")
        //        {
        //            dataList.Items[i].BackColor = Color.Green;
        //        }
        //        else
        //        {
        //            dataList.Items[i].BackColor = Color.LightGreen;
        //        }
        //    }
        //}

    }

    protected void LoadRequirement(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Requirement as requirement where requirement.ApplicantCode = " + "'" + strUserCode + "'" + " order by requirement.ReqID DESC";
        RequirementBLL requirementBLL = new RequirementBLL();
        lst = requirementBLL.GetAllRequirements(strHQL);

        DataGrid5.DataSource = lst;
        DataGrid5.DataBind();

        LB_Sql5.Text = strHQL;
    }
}
