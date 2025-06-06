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

using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTProjectMemberManHourReportForAlone : System.Web.UI.Page
{
    string strProjectID, strProjectName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strProjectID = Request.QueryString["ProjectID"];
        strProjectName = ShareClass.GetProjectName(strProjectID);

        strUserCode = Session["UserCode"].ToString();

        LB_ReportName.Text =  LanguageHandle.GetWord("XiangMu") + ": " + strProjectID + " " + strProjectName + LanguageHandle.GetWord("ChengYuanGongShiBiao");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            string strHQL;
            
            strHQL = "Select UserCode,UserName,DepartCode,DepartName,WorkDate,sum(ManHour) as ManHour,sum(ConfirmManHour) as ConfirmManHour From V_ProjectMemberManHourSummary";
            strHQL += " Where ProjectID = " + strProjectID;
            strHQL += " Group By UserCode,UserName,DepartCode,DepartName,WorkDate";
            strHQL += " Order By WorkDate";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DailyWork");
            DataList1.DataSource = ds;
            DataList1.DataBind();

            SumManHour(ds);

            strHQL = "Select min(WorkDate),max(WorkDate) From V_ProjectMemberManHourSummary";
            strHQL += " Where ProjectID = " + strProjectID;
            DataSet ds2 = ShareClass.GetDataSetFromSql(strHQL, "T_DailyWork");

            if (ds.Tables[0].Rows.Count > 0)
            {
                DLC_BeginDate.Text = DateTime.Parse(ds2.Tables[0].Rows[0][0].ToString()).ToString("yyyy-MM-dd");
                DLC_EndDate.Text = DateTime.Parse(ds2.Tables[0].Rows[0][1].ToString()).ToString("yyyy-MM-dd");
            }
            else
            {

                DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
    }


    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strOpretorName;
        string strBeginTime, strEndTime;

      
        strOpretorName = "%" + TB_MemberName.Text.Trim() + "%";
      
        strBeginTime = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndDate.Text).ToString("yyyyMMdd");

        strHQL = "Select UserCode,UserName,DepartCode,DepartName,WorkDate,sum(ManHour) as ManHour,sum(ConfirmManHour) as ConfirmManHour From V_ProjectMemberManHourSummary";
        strHQL += " Where ProjectID = " + strProjectID;
        strHQL += " and to_char(WorkDate,'yyyymmdd') >= " + "'" + strBeginTime + "'";
        strHQL += " and to_char(WorkDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
        strHQL += " and UserName Like " + "'" + strOpretorName + "'";
        strHQL += " Group By UserCode,UserName,DepartCode,DepartName,WorkDate"; 
        strHQL += " Order By WorkDate";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DailyWork");
        DataList1.DataSource = ds;
        DataList1.DataBind();

        SumManHour(ds);
    }

    protected void SumManHour(DataSet ds)
    {
        decimal deTotalManHour = 0, DeTotalConfirmManHour = 0;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            deTotalManHour += decimal.Parse(ds.Tables[0].Rows[i][5].ToString());
            DeTotalConfirmManHour += decimal.Parse(ds.Tables[0].Rows[i][6].ToString());
        }

        LB_TotalManHour.Text = deTotalManHour.ToString();
        LB_TotalConfirmManHour.Text = DeTotalConfirmManHour.ToString();
    }

  
    protected void BT_Export_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strOpretorName;
        string strBeginTime, strEndTime;

  
        strOpretorName = "%" + TB_MemberName.Text.Trim() + "%";
    
        strBeginTime = DateTime.Parse(DLC_BeginDate.Text).ToString("yyyyMMdd");
        strEndTime = DateTime.Parse(DLC_EndDate.Text).ToString("yyyyMMdd");

        strHQL = @"Select DepartCode as Department,  
                   UserName as Name,   
                   DepartCode as DepartmentCode,   
                   DepartName as DepartmentName,   
                   WorkDate as WorkingHours,   
                   sum(ManHour) as DeclaredLaborHours,   
                   sum(ConfirmManHour) as ConfirmedLaborHours   
                   From V_ProjectMemberManHourSummary";
        strHQL += " Where ProjectID = " + strProjectID;
        strHQL += " and to_char(WorkDate,'yyyymmdd') >= " + "'" + strBeginTime + "'";
        strHQL += " and to_char(WorkDate,'yyyymmdd') <= " + "'" + strEndTime + "'";
        strHQL += " and UserName Like " + "'" + strOpretorName + "'";
        strHQL += " Group By UserCode,UserName,DepartCode,DepartName,WorkDate";
        strHQL += " Order By WorkDate";

        DataTable dtProject = ShareClass.GetDataSetFromSql(strHQL, "project").Tables[0];

        Export3Excel(dtProject, LanguageHandle.GetWord("XiangMu") + ": " + strProjectID + " " + strProjectName + LanguageHandle.GetWord("ChengYuanGongShiHuiZongBiaoxls"));

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("DaoChuChengGong")+"��');", true);   
    }

    public void Export3Excel(DataTable dtData, string strFileName)
    {
        DataGrid dgControl = new DataGrid();
        dgControl.DataSource = dtData;
        dgControl.DataBind();


        Response.Clear();
        Response.Buffer = true;
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.ContentType = "application/shlnd.ms-excel";
        Response.Charset = "GB2312";
        EnableViewState = false;
        System.Globalization.CultureInfo mycitrad = new System.Globalization.CultureInfo("ZH-CN", true);
        System.IO.StringWriter ostrwrite = new System.IO.StringWriter(mycitrad);
        System.Web.UI.HtmlTextWriter ohtmt = new HtmlTextWriter(ostrwrite);
        dgControl.RenderControl(ohtmt);
        Response.Clear();
        Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + ostrwrite.ToString());
        Response.End();
    }
}
