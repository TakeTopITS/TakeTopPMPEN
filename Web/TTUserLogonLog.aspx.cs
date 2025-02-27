using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;

public partial class TTUserLogonLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();
        string strUserName;

        //this.Title = "用户登录日志---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        LB_UserCode.Text = strUserCode;
        strUserName = Session["UserName"].ToString();
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "查看所有项目", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        Calendar1.SelectedDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
        Calendar1.VisibleDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

        //设置日程控件语言
        Culture = ShareClass.GetDayPilotLanguage();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            string strHQL1;
            IList lst;
            DateTime dtSelectedDate = DateTime.Now;

            strHQL1 = "from LogonLog as logonLog where to_char(logonLog.LoginTime,'yyyymmdd') = " + "'" + dtSelectedDate.ToString("yyyyMMdd") + "'" + " Order by logonLog.ID DESC";
            LogonLogBLL logonLogBLL = new LogonLogBLL();
            lst = logonLogBLL.GetAllLogonLogs(strHQL1);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            strHQL1 = "Select Distinct(UserCode) From T_LogonLog where to_char(LoginTime,'yyyymmdd') = " + "'" + dtSelectedDate.ToString("yyyyMMdd") + "'" + " Group By UserCode";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL1, "T_LogonLog");


            LB_QueryScope.Text = LanguageHandle.GetWord("RiQi").ToString().Trim() + ":" + dtSelectedDate.ToShortDateString() + "," + LanguageHandle.GetWord("CiShu").ToString().Trim() + lst.Count.ToString() + "," + LanguageHandle.GetWord("RenShu").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();

        }
    }

    protected void BT_AllUserLogonLog_Click(object sender, EventArgs e)
    {
        LoadUserLogonLog();
    }

    protected void BT_GetPosition_Click(object sender, EventArgs e)
    {
        string strHQL1, strHQL2;
        IList lst;

        string strID, strIP, strPosition;

        strHQL1 = "from LogonLog as logonLog Order by logonLog.ID DESC";
        LogonLogBLL logonLogBLL = new LogonLogBLL();
        lst = logonLogBLL.GetAllLogonLogs(strHQL1);

        LogonLog logonLog = new LogonLog();

        for (int i = 0; i < lst.Count; i++)
        {
            logonLog = (LogonLog)lst[i];

            strID = logonLog.ID.ToString();
            strIP = logonLog.UserIP.Trim();
            strPosition = ShareClass.GetIPinArea(strIP);

            strHQL2 = "Update T_LogonLog Set Position = " + "'" + strPosition + "'" + " Where ID = " + strID;
            ShareClass.RunSqlCommand(strHQL2);
        }

        LoadUserLogonLog();

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJSWC").ToString().Trim() + "')", true);
    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        DateTime dtSelectedDate = Calendar1.SelectedDate;

        strHQL = "from LogonLog as logonLog where to_char(logonLog.LoginTime,'yyyymmdd') = " + "'" + dtSelectedDate.ToString("yyyyMMdd") + "'" + " Order by logonLog.ID DESC";
        LogonLogBLL logonLogBLL = new LogonLogBLL();
        lst = logonLogBLL.GetAllLogonLogs(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        strHQL = "Select Distinct(UserCode) From T_LogonLog where to_char(LoginTime,'yyyymmdd') = " + "'" + dtSelectedDate.ToString("yyyyMMdd") + "'" + " Group By UserCode";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_LogonLog");

        LB_QueryScope.Text = LanguageHandle.GetWord("RiQi").ToString().Trim() + ":" + dtSelectedDate.ToShortDateString() + "," + LanguageHandle.GetWord("CiShu").ToString().Trim() + lst.Count.ToString() + "," + LanguageHandle.GetWord("RenShu").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
    }

    protected void BT_FindUserName_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserName;

        strUserName = TB_UserName.Text.Trim();
        strUserName = "%" + strUserName + "%";

        strHQL = "from LogonLog as logonLog where logonLog.UserName like " + "'" + strUserName + "'";
        strHQL += " Order by logonLog.ID DESC";
        LogonLogBLL logonLogBLL = new LogonLogBLL();
        lst = logonLogBLL.GetAllLogonLogs(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_QueryScope.Text = LanguageHandle.GetWord("UserName").ToString().Trim() + strUserName + "，" + LanguageHandle.GetWord("CiShu").ToString().Trim() + lst.Count.ToString();
    }

    protected void BT_FindIP_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserIP;

        strUserIP = TB_IP.Text.Trim();
        strUserIP = "%" + strUserIP + "%";

        strHQL = "from LogonLog as logonLog where logonLog.UserIP like " + "'" + strUserIP + "'";
        strHQL += " Order by logonLog.ID DESC";
        LogonLogBLL logonLogBLL = new LogonLogBLL();
        lst = logonLogBLL.GetAllLogonLogs(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_QueryScope.Text = "IP" + strUserIP + "，" + LanguageHandle.GetWord("CiShu").ToString().Trim() + ":" + lst.Count.ToString();
    }

    protected void BT_FindDeviceType_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strDeviceType;

        strDeviceType = DL_DeviceType.SelectedValue.Trim();

        strHQL = "from LogonLog as logonLog where logonLog.DeviceType = " + "'" + strDeviceType + "'";
        strHQL += " Order by logonLog.ID DESC";
        LogonLogBLL logonLogBLL = new LogonLogBLL();
        lst = logonLogBLL.GetAllLogonLogs(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        LB_QueryScope.Text = LanguageHandle.GetWord("LeiXin").ToString().Trim() + strDeviceType + "，" + LanguageHandle.GetWord("CiShu").ToString().Trim() + ":" + lst.Count.ToString();
    }

    protected void LoadUserLogonLog()
    {
        string strHQL1;
        IList lst;

        strHQL1 = "from LogonLog as logonLog  Order by logonLog.ID DESC";
        LogonLogBLL logonLogBLL = new LogonLogBLL();
        lst = logonLogBLL.GetAllLogonLogs(strHQL1);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();

        strHQL1 = "Select Distinct(UserCode) From T_LogonLog  Group By UserCode";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL1, "T_LogonLog");

        LB_QueryScope.Text = LanguageHandle.GetWord("CiShu").ToString().Trim() + lst.Count.ToString() + LanguageHandle.GetWord("RenShu").ToString().Trim() + ds.Tables[0].Rows.Count.ToString();
    }

}
