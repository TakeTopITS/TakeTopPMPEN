using System;
using System.Resources;
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
using System.IO;
using System.Drawing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTUserAttendanceRecord : System.Web.UI.Page
{
    ArrayList hour, m;
    int i;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass. GetUserName(strUserCode);

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "员工出勤记录", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTUserAttendanceRecordSAAS.aspx");
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_AttendanceDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_SelectedAttendanceDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);

            try
            {
                strHQL = "Insert Into T_UserAttendanceRule(UserCode,UserName,CreateDate,MCheckInStart,MCheckInEnd,MCheckOutStart,MCheckOutEnd,";
                strHQL += "ACheckInStart,ACheckInEnd,ACheckOutStart,ACheckOutEnd,NCheckInStart,NCheckInEnd,NCheckOutStart,NCheckOutEnd,";
                strHQL += "OCheckInStart,OCheckInEnd,OCheckOutStart,OCheckOutEnd,Status,MCheckInIsMust,MCheckOutIsMust,ACheckInIsMust,ACheckOutIsMust,NCheckInIsMust,NCheckOutIsMust,OCheckInIsMust,OCheckOutIsMust,LargestDistance,LeaderCode,LeaderName,OfficeLongitude,OfficeLatitude)";
                strHQL += " Select A.UserCode,A.UserName,now(),B.MCheckInStart,B.MCheckInEnd,B.MCheckOutStart,B.MCheckOutEnd,";
                strHQL += "B.ACheckInStart,B.ACheckInEnd,B.ACheckOutStart,B.ACheckOutEnd,B.NCheckInStart,B.NCheckInEnd,B.NCheckOutStart,B.NCheckOutEnd,";
                strHQL += "B.OCheckInStart,B.OCheckInEnd,B.OCheckOutStart,B.OCheckOutEnd,'InProgress',B.MCheckInIsMust,B.MCheckOutIsMust,B.ACheckInIsMust,B.ACheckOutIsMust,B.NCheckInIsMust,B.NCheckOutIsMust,B.OCheckInIsMust,B.OCheckOutIsMust,B.LargestDistance,'','',B.OfficeLongitude,B.OfficeLatitude";
                strHQL += " From T_ProjectMember A, T_AttendanceRule B";
                strHQL += " Where A.UserCode not in (Select UserCode From T_UserAttendanceRule) and A.Status not in ('Resign','Stop') ";

                //
                ShareClass.RunSqlCommand(strHQL);
            }
            catch
            {
            }

            InitialCalendar();

            DLC_SelectedAttendanceDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_AttendanceDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

  
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);
        }
    }

    protected void BT_CreateAttendanceRecord_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strAttendanceDate2, strAttendanceDate3;
       
        strAttendanceDate2 = DLC_SelectedAttendanceDate.Text;
        strAttendanceDate3 = DateTime.Parse(DLC_SelectedAttendanceDate.Text).ToString("yyyy-MM-dd");

        strHQL = "Insert Into T_UserAttendanceRecord(UserCode,UserName,AttendanceDate,MCheckIn,MCheckOut,";
        strHQL += "ACheckIn,ACheckOut,NCheckIn,NCheckOut,OCheckIn,OCheckOut,LateMinute,EarlyMinute,MCheckInIsMust,MCheckOutIsMust,ACheckInIsMust,ACheckOutIsMust,NCheckInIsMust,NCheckOutIsMust,OCheckInIsMust,OCheckOutIsMust,LargestDistance,LeaderCode,LeaderName,OfficeLongitude,OfficeLatitude)";
        strHQL += " Select A.UserCode,A.UserName,'" + strAttendanceDate2 + "'::timestamp,('" + strAttendanceDate3 + " '||B.MCheckInEnd || ':00.000')::timestamp ,('" + strAttendanceDate3 + " '||B.MCheckOutEnd || ':00.000')::timestamp,";
        strHQL += " ('" + strAttendanceDate3 + " '||B.ACheckInEnd || ':00.000')::timestamp,('" + strAttendanceDate3 + " '|| B.ACheckOutEnd || ':00.000')::timestamp,('" + strAttendanceDate3 + " '||B.NCheckInEnd || ':00.000')::timestamp,('" + strAttendanceDate3 + " '||B.NCheckOutEnd || ':00.000')::timestamp,";
        strHQL += " ('" + strAttendanceDate3 + " '||B.OCheckInEnd || ':00.000')::timestamp,('" + strAttendanceDate3 + " '||B.OCheckOutEnd || ':00.000')::timestamp,0,0,B.MCheckInIsMust,B.MCheckOutIsMust,B.ACheckInIsMust,B.ACheckOutIsMust,B.NCheckInIsMust,B.NCheckOutIsMust,B.OCheckInIsMust,B.OCheckOutIsMust,B.LargestDistance,'','',B.OfficeLongitude,B.OfficeLatitude";
        strHQL += " From T_ProjectMember A, T_UserAttendanceRule B";
        strHQL += " Where A.UserCode = B.UserCode and A.UserCode not in (Select UserCode From T_UserAttendanceRecord Where to_char(AttendanceDate,'yyyymmdd') = " + "'" + strAttendanceDate3 + "'" + ") and A.Status not in ('Resign','Stop') ";
        strHQL += " and B.Status = 'InUse' ";

        //

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCHENGCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCHENGSBQJC") + "')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        LB_ID.Text = "";
        BT_UpdateUserAttendanceRule.Enabled = false;
        BT_DeleteUserAttendanceRule.Enabled = false;

        LoadUserAttendanceRecord(strUserCode);
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();
        string strShiftType = DL_ShiftType.SelectedValue.Trim();

        for (int i = 0; i < DataGrid3.Items.Count; i++)
        {
            DataGrid3.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;

        strHQL = "From UserAttendanceRecord as userAttendanceRecord Where userAttendanceRecord.ID = " + strID;
        UserAttendanceRecordBLL userAttendanceRecordBLL = new UserAttendanceRecordBLL();
        lst = userAttendanceRecordBLL.GetAllUserAttendanceRecords(strHQL);

        UserAttendanceRecord userAttendanceRecord = (UserAttendanceRecord)lst[0];

        LB_ID.Text = userAttendanceRecord.ID.ToString();
        LB_UserCode.Text = userAttendanceRecord.UserCode.Trim();
        LB_UserName.Text = userAttendanceRecord.UserName.Trim();


        if (strShiftType == "MorningWorkStartTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.MCheckIn.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.MCheckIn.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.MCheckIn.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.MCheckInDistance;
        }
        if (strShiftType == "MorningWorkEndTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.MCheckOut.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.MCheckOut.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.MCheckOut.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.MCheckOutDistance;
        }
        if (strShiftType == "AfternoonWorkStartTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.ACheckIn.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.ACheckIn.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.ACheckIn.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.ACheckInDistance;
        }
        if (strShiftType == "AfternoonWorkEndTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.ACheckOut.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.ACheckOut.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.ACheckOut.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.ACheckOutDistance;
        }
        if (strShiftType == "NightShiftStartTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.NCheckIn.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.NCheckIn.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.NCheckIn.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.NCheckInDistance;
        }
        if (strShiftType == "NightShiftEndTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.NCheckOut.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.NCheckOut.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.NCheckOut.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.NCheckOutDistance;
        }
        if (strShiftType == "MidnightWorkStartTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.OCheckIn.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.OCheckIn.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.OCheckIn.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.OCheckInDistance;
        }
        if (strShiftType == "MidnightWorkEndTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.OCheckOut.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.OCheckOut.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.OCheckOut.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.OCheckOutDistance;
        }

        BT_UpdateUserAttendanceRule.Enabled = true;
        BT_DeleteUserAttendanceRule.Enabled = true;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql3.Text;

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserAttendanceRecord");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_ShiftType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = LB_ID.Text.Trim();
        string strShiftType = DL_ShiftType.SelectedValue.Trim();

        strHQL = "From UserAttendanceRecord as userAttendanceRecord Where ID = " + strID;
        UserAttendanceRecordBLL userAttendanceRecordBLL = new UserAttendanceRecordBLL();
        lst = userAttendanceRecordBLL.GetAllUserAttendanceRecords(strHQL);

        UserAttendanceRecord userAttendanceRecord = (UserAttendanceRecord)lst[0];


        if (strShiftType == "MorningWorkStartTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.MCheckIn.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.MCheckIn.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.MCheckIn.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.MCheckInDistance;
        }
        if (strShiftType == "MorningWorkEndTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.MCheckOut.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.MCheckOut.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.MCheckOut.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.MCheckOutDistance;
        }
        if (strShiftType == "AfternoonWorkStartTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.ACheckIn.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.ACheckIn.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.ACheckIn.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.ACheckInDistance;
        }
        if (strShiftType == "AfternoonWorkEndTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.ACheckOut.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.ACheckOut.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.ACheckOut.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.ACheckOutDistance;
        }
        if (strShiftType == "NightShiftStartTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.NCheckIn.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.NCheckIn.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.NCheckIn.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.NCheckInDistance;
        }
        if (strShiftType == "NightShiftEndTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.NCheckOut.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.NCheckOut.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.NCheckOut.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.NCheckOutDistance;
        }
        if (strShiftType == "MidnightWorkStartTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.OCheckIn.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.OCheckIn.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.OCheckIn.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.OCheckInDistance;
        }
        if (strShiftType == "MidnightWorkEndTime")
        {
            DLC_AttendanceDate.Text = userAttendanceRecord.OCheckOut.ToString("yyyy-MM-dd");
            DL_Hour.SelectedValue = userAttendanceRecord.OCheckOut.Hour.ToString();
            DL_Minute.SelectedValue = userAttendanceRecord.OCheckOut.Minute.ToString();
            NB_Distance.Amount = userAttendanceRecord.OCheckOutDistance;
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void BT_UpdateUserAttendanceRule_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID;
        string strUserCode, strUserName;
        string strShiftType, strAttendanceTime;


        strID = LB_ID.Text.Trim();

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        strShiftType = DL_ShiftType.SelectedValue.Trim();
        strAttendanceTime = DateTime.Parse(DLC_AttendanceDate.Text).ToString("yyyy/MM/dd") + " " + DL_Hour.SelectedValue.Trim() + ":" + DL_Minute.SelectedValue.Trim() + ":00.000";

        strHQL = "From UserAttendanceRecord as userAttendanceRecord Where userAttendanceRecord.ID = " + strID;
        UserAttendanceRecordBLL userAttendanceRecordBLL = new UserAttendanceRecordBLL();
        lst = userAttendanceRecordBLL.GetAllUserAttendanceRecords(strHQL);
        UserAttendanceRecord userAttendanceRecord = (UserAttendanceRecord)lst[0];

        try
        {
            if (strShiftType == "MorningWorkStartTime")
            {
                userAttendanceRecord.MCheckIn = DateTime.Parse(strAttendanceTime);
                userAttendanceRecord.MCheckInDistance = NB_Distance.Amount;
            }
            if (strShiftType == "MorningWorkEndTime")
            {
                userAttendanceRecord.MCheckOut = DateTime.Parse(strAttendanceTime);
                userAttendanceRecord.MCheckOutDistance = NB_Distance.Amount;
            }
            if (strShiftType == "AfternoonWorkStartTime")
            {
                userAttendanceRecord.ACheckIn = DateTime.Parse(strAttendanceTime);
                userAttendanceRecord.ACheckInDistance = NB_Distance.Amount;
            }
            if (strShiftType == "AfternoonWorkEndTime")
            {
                userAttendanceRecord.ACheckOut = DateTime.Parse(strAttendanceTime);
                userAttendanceRecord.ACheckOutDistance = NB_Distance.Amount;
            }
            if (strShiftType == "NightShiftStartTime")
            {
                userAttendanceRecord.NCheckIn = DateTime.Parse(strAttendanceTime);
                userAttendanceRecord.NCheckInDistance = NB_Distance.Amount;
            }
            if (strShiftType == "NightShiftEndTime")
            {
                userAttendanceRecord.NCheckOut = DateTime.Parse(strAttendanceTime);
                userAttendanceRecord.NCheckOutDistance = NB_Distance.Amount;
            }
            if (strShiftType == "MidnightWorkStartTime")
            {
                userAttendanceRecord.OCheckIn = DateTime.Parse(strAttendanceTime);
                userAttendanceRecord.OCheckInDistance = NB_Distance.Amount;
            }
            if (strShiftType == "MidnightWorkEndTime")
            {
                userAttendanceRecord.OCheckIn = DateTime.Parse(strAttendanceTime);
                userAttendanceRecord.OCheckOutDistance = NB_Distance.Amount;
            }

            userAttendanceRecordBLL.UpdateUserAttendanceRecord(userAttendanceRecord, int.Parse(strID));

            UpdateLateMinute(strID);
            UpdateEarlyMinute(strID);

            LoadUserAttendanceRecord(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXGSBJC") + "')", true);
        }
    }

    protected void BT_DeleteUserAttendanceRule_Click(object sender, EventArgs e)
    {
        string strID;
        string strHQL;

        string strUserCode = LB_UserCode.Text.Trim();

        strID = LB_ID.Text.Trim();
        strHQL = "Delete From T_UserAttendanceRecord Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadUserAttendanceRecord(strUserCode);

            BT_UpdateUserAttendanceRule.Enabled = false;
            BT_DeleteUserAttendanceRule.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSCSBJC") + "')", true);
        }
    }

    protected void UpdateLateMinute(string strID)
    {
        string strHQL;
        int intSumLateMinute, intMLateMinute = 0, intALateMinute = 0, intNLateMinute = 0, intOLateMinute = 0;

        DataSet ds;

        strHQL = " select extract(epoch FROM (A.MCheckIn-(to_char(A.MCheckIn,'yyyymmdd') || ' ' || rtrim(ltrim(B.MCheckInEnd)) ||':00.000')::timestamp))/60";
        strHQL += " From T_UserAttendanceRecord A,T_userAttendanceRule B ";
        strHQL += " Where A.UserCode = B.UserCode and A.ID = " + strID + " and B.Status = 'InProgress'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_LateMinute");

        if (ds.Tables[0].Rows.Count > 0)
        {
            intMLateMinute = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            if (intMLateMinute < 0)
            {
                intMLateMinute = 0;
            }
        }

        strHQL = " Select extract(epoch FROM (A.ACheckIn-(to_char(A.ACheckIn,'yyyymmdd') || ' ' || rtrim(ltrim(B.ACheckInEnd)) ||':00.000')::timestamp))/60";
        strHQL += " From T_UserAttendanceRecord A,T_userAttendanceRule B ";
        strHQL += " Where A.UserCode = B.UserCode and A.ID = " + strID + " and B.Status = 'InProgress'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_LateMinute");

        if (ds.Tables[0].Rows.Count > 0)
        {
            intALateMinute = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            if (intALateMinute < 0)
            {
                intALateMinute = 0;
            }
        }

        strHQL = " Select extract(epoch FROM (A.NCheckIn-(to_char(A.NCheckIn,'yyyymmdd') || ' ' || rtrim(ltrim(B.NCheckInEnd)) ||':00.000')::timestamp))/60";
        strHQL += " From T_UserAttendanceRecord A,T_userAttendanceRule B ";
        strHQL += " Where A.UserCode = B.UserCode and A.ID = " + strID + " and B.Status = 'InProgress'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_LateMinute");

        if (ds.Tables[0].Rows.Count > 0)
        {
            intNLateMinute = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            if (intNLateMinute < 0)
            {
                intNLateMinute = 0;
            }
        }


        strHQL = " Select extract(epoch FROM (A.OCheckIn-(to_char(A.OCheckIn,'yyyymmdd') || ' ' || rtrim(ltrim(B.OCheckInEnd)) ||':00.000')::timestamp))/60";
        strHQL += " From T_UserAttendanceRecord A,T_userAttendanceRule B ";
        strHQL += " Where A.UserCode = B.UserCode and A.ID = " + strID + " and B.Status = 'InProgress'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_LateMinute");

        if (ds.Tables[0].Rows.Count > 0)
        {
            intOLateMinute = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            if (intOLateMinute < 0)
            {
                intOLateMinute = 0;
            }
        }

        intSumLateMinute = intMLateMinute + intALateMinute + intNLateMinute + intOLateMinute;

        strHQL = "Update T_UserAttendanceRecord Set LateMinute = " + intSumLateMinute.ToString() + " Where ID = " + strID;

        ShareClass.RunSqlCommand(strHQL);
    }

    protected void UpdateEarlyMinute(string strID)
    {
        string strHQL;
        int intSumEarlyMinute, intMEarlyMinute = 0, intAEarlyMinute = 0, intNEarlyMinute = 0, intOEarlyMinute = 0;
        DataSet ds;

        strHQL = " Select extract(epoch FROM ((to_char(A.MCheckOut,'yyyymmdd') || ' ' || rtrim(ltrim(B.MCheckOutStart)) ||':00.000')::timestamp)-A.MCheckOut)/60";
        strHQL += " From T_UserAttendanceRecord A,T_userAttendanceRule B ";
        strHQL += " Where A.UserCode = B.UserCode and A.ID = " + strID + " and B.Status = 'InProgress'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_EarlyMinute");

        if (ds.Tables[0].Rows.Count > 0)
        {
            intMEarlyMinute = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            if (intMEarlyMinute < 0)
            {
                intMEarlyMinute = 0;
            }
        }

        strHQL = " Select extract(epoch FROM ((to_char(A.ACheckOut,'yyyymmdd') || ' ' || rtrim(ltrim(B.ACheckOutStart)) ||':00.000')::timestamp)-A.ACheckOut)/60";
        strHQL += " From T_UserAttendanceRecord A,T_userAttendanceRule B ";
        strHQL += " Where A.UserCode = B.UserCode and A.ID = " + strID + " and B.Status = 'InProgress'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_EarlyMinute");

        if (ds.Tables[0].Rows.Count > 0)
        {
            intAEarlyMinute = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            if (intAEarlyMinute < 0)
            {
                intAEarlyMinute = 0;
            }
        }

        strHQL = " Select extract(epoch FROM ((to_char(A.NCheckOut,'yyyymmdd') || ' ' || rtrim(ltrim(B.NCheckOutStart)) ||':00.000')::timestamp)-A.NCheckOut)/60";
        strHQL += " From T_UserAttendanceRecord A,T_userAttendanceRule B ";
        strHQL += " Where A.UserCode = B.UserCode and A.ID = " + strID + " and B.Status = 'InProgress'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_EarlyMinute");

        if (ds.Tables[0].Rows.Count > 0)
        {
            intNEarlyMinute = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            if (intNEarlyMinute < 0)
            {
                intNEarlyMinute = 0;
            }

        }

        strHQL = " Select extract(epoch FROM ((to_char(A.OCheckOut,'yyyymmdd') || ' ' || rtrim(ltrim(B.OCheckOutStart)) ||':00.000')::timestamp)-A.OCheckOut)/60";
        strHQL += " From T_UserAttendanceRecord A,T_userAttendanceRule B ";
        strHQL += " Where A.UserCode = B.UserCode and A.ID = " + strID + " and B.Status = 'InProgress'";
        ds = ShareClass.GetDataSetFromSql(strHQL, "T_EarlyMinute");

        if (ds.Tables[0].Rows.Count > 0)
        {
            intOEarlyMinute = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            if (intOEarlyMinute < 0)
            {
                intOEarlyMinute = 0;
            }
        }

        intSumEarlyMinute = intMEarlyMinute + intAEarlyMinute + intNEarlyMinute + intOEarlyMinute;

        strHQL = "Update T_UserAttendanceRecord Set EarlyMinute = " + intSumEarlyMinute.ToString() + " Where ID = " + strID;

        ShareClass.RunSqlCommand(strHQL);
    }



    protected void InitialCalendar()
    {
        hour = new ArrayList();
        m = new ArrayList();


        for (i = 0; i <= 23; i++)
            hour.Add(i.ToString());
        for (i = 00; i <= 59; i++)
            m.Add(i.ToString());

        DL_Hour.DataSource = hour;
        DL_Hour.DataBind();
        // DL_Hour.Text = System.DateTime.Now.Hour.ToString();

        DL_Minute.DataSource = m;
        DL_Minute.DataBind();
        // DL_Minute.Text = System.DateTime.Now.Minute.ToString();       
    }

    protected void LoadUserAttendanceRecord(string strUserCode)
    {
        string strHQL;

        strHQL = "Select * From T_UserAttendanceRecord Where UserCode = " + "'" + strUserCode + "'";
        strHQL += " Order by to_char(AttendanceDate,'yyyymmdd') DESC limit 10";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserAttendanceRecord");

        DataGrid3.DataSource = ds;
        DataGrid3.DataBind();

        LB_Sql3.Text = strHQL;
    }

    protected void LoadProjectMember(string strDepartCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

}
