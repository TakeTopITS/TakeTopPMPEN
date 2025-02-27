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
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMakeWLMeeting : System.Web.UI.Page
{
    ArrayList year, month, day, hour, m;
    int i;

    string strWLID, strWLName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL, strUserCode;
        string strDepartCode;

        IList lst;

        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;

        strWLID = Request.QueryString["MeetingID"].ToString();
        strWLName = ShareClass.GetWorkFlowName(strWLID);

        //this.Title = "工作流会议：" + strWLID + " " + strWLName + " 的会议维护";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);

            strDepartCode = GetDepartCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            strHQL = "from MeetingType as meetingType order by meetingType.SortNumber ASC";
            MeetingTypeBLL meetingTypeBLL = new MeetingTypeBLL();
            lst = meetingTypeBLL.GetAllMeetingTypes(strHQL);

            DL_MeetingType.DataSource = lst;
            DL_MeetingType.DataBind();

            strHQL = "from MeetingRoom as meetingRoom order by meetingRoom.ID ASC";
            MeetingRoomBLL meetingRoomBLL = new MeetingRoomBLL();
            lst = meetingRoomBLL.GetAllMeetingRooms(strHQL);

            DL_Room.DataSource = lst;
            DL_Room.DataBind();

            strHQL = "from Meeting as meeting where meeting.RelatedType = 'Workflow' and meeting.RelatedID = " + strWLID + " order by meeting.ID DESC";
            MeetingBLL meetingBLL = new MeetingBLL();
            lst = meetingBLL.GetAllMeetings(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_Sql.Text = strHQL;

            InitialCalendar();
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_Room_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strMeetingRoom = DL_Room.SelectedValue.Trim();

        TB_MeetingRoom.Text = strMeetingRoom;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_ID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddMeeting();
        }
        else
        {
            UpdateMeeting();
        }
    }

    protected void AddMeeting()
    {
        string strMeetingID;
        string strUserCode = LB_UserCode.Text.Trim();
        string strWLName = TB_Name.Text.Trim();
        string strMeetingType = DL_MeetingType.SelectedValue;
        string strHost = TB_Host.Text.Trim();
        string strRecorder = TB_Recorder.Text.Trim();
        string strBuilderCode = LB_UserCode.Text.Trim();
        string strMeetingAddr = TB_MeetingRoom.Text.Trim();
        string strOrganizer = TB_Organizer.Text.Trim();
        string strContent = TB_Content.Text.Trim();
        string strBeginTime = this.DL_BeginYear.SelectedValue.ToString() + "-" + this.DL_BeginMonth.SelectedValue.ToString() + "-" + this.DL_BeginDay.SelectedValue.ToString() + "  " + this.DL_BeginHour.SelectedValue.ToString() + ":" + this.DL_BeginMinute.SelectedValue.ToString();
        string strEndTime = this.DL_EndYear.SelectedValue.ToString() + "-" + this.DL_EndMonth.SelectedValue.ToString() + "-" + this.DL_EndDay.SelectedValue.ToString() + "  " + this.DL_EndHour.SelectedValue.ToString() + ":" + this.DL_EndMinute.SelectedValue.ToString();
        string strStatus = DL_Status.SelectedValue.Trim();

        MeetingBLL meetingBLL = new MeetingBLL();
        Meeting meeting = new Meeting();

        meeting.Name = strWLName;
        meeting.Type = DL_MeetingType.SelectedValue;
        meeting.RelatedType = "Workflow";
        meeting.RelatedID = int.Parse(strWLID);
        meeting.Host = strHost;
        meeting.Recorder = strRecorder;
        meeting.Organizer = strOrganizer;
        meeting.BuilderCode = strUserCode;
        meeting.Address = strMeetingAddr;
        meeting.Content = strContent;
        meeting.BeginTime = DateTime.Parse(strBeginTime);
        meeting.EndTime = DateTime.Parse(strEndTime);
        meeting.MakeTime = DateTime.Now;
        meeting.Status = strStatus;

        try
        {
            meetingBLL.AddMeeting(meeting);
            strMeetingID = ShareClass.GetMyCreatedMaxMeetingID(strUserCode);
            LB_ID.Text = strMeetingID;

            #region 增加参与人员 By LiuJianping 2013-09-23

            if (RP_Attendant.Items.Count > 0)
            {
                for (int i = 0; i < RP_Attendant.Items.Count; i++)
                {
                    if (((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim() == "")
                    {
                    }
                    else
                    {
                        AddCollaborationMember(strMeetingID, ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim(), ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim());
                    }
                }
            }
            #endregion

            HL_RelatedDoc.NavigateUrl = "TTMeetingDoc.aspx?RelatedID=" + strMeetingID;

            HL_MeetingToTask.Enabled = true;
            HL_MeetingToTask.NavigateUrl = "TTMeetingToTask.aspx?ProjectID=1&RelatedType=MEETING&MeetingID=" + strMeetingID;

            HL_MakeCollaboration.Enabled = true;
            HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=MEETING&RelatedID=" + strMeetingID;

            LoadMeetingAttendant(strMeetingID);

            LoadMyCreatedMeetingList(strWLID, strUserCode);

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "');</script>");

            BT_Send.Enabled = true;

            HL_MakeCollaboration.Enabled = true;
            HL_MeetingToTask.Enabled = true;
            HL_RelatedDoc.Enabled = true;

            TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi").ToString().Trim() + strMeetingID + " " + meeting.Name.Trim() + "，" + meeting.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen").ToString().Trim() + meeting.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen").ToString().Trim() + meeting.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia").ToString().Trim();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateMeeting()
    {
        string strHQL;
        IList lst;

        string strUserCode = LB_UserCode.Text.Trim();
        string strID = LB_ID.Text.Trim();
        string strWLName = TB_Name.Text.Trim();
        string strMeetingType = DL_MeetingType.SelectedValue;
        string strHost = TB_Host.Text.Trim();
        string strRecorder = TB_Recorder.Text.Trim();
        string strMeetingAddr = TB_MeetingRoom.Text.Trim();
        string strOrganizer = TB_Organizer.Text.Trim();
        string strContent = TB_Content.Text.Trim();
        string strBeginTime = this.DL_BeginYear.SelectedValue.ToString() + "-" + this.DL_BeginMonth.SelectedValue.ToString() + "-" + this.DL_BeginDay.SelectedValue.ToString() + "  " + this.DL_BeginHour.SelectedValue.ToString() + ":" + this.DL_BeginMinute.SelectedValue.ToString();
        string strEndTime = this.DL_EndYear.SelectedValue.ToString() + "-" + this.DL_EndMonth.SelectedValue.ToString() + "-" + this.DL_EndDay.SelectedValue.ToString() + "  " + this.DL_EndHour.SelectedValue.ToString() + ":" + this.DL_EndMinute.SelectedValue.ToString();
        string strStatus = DL_Status.SelectedValue.Trim();

        MeetingBLL meetingBLL = new MeetingBLL();
        Meeting meeting = new Meeting();
        strHQL = "from Meeting as meeting where meeting.ID = " + strID;
        meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);
        meeting = new Meeting();
        meeting = (Meeting)lst[0];

        meeting.Name = strWLName;
        meeting.Type = DL_MeetingType.SelectedValue;
        meeting.RelatedType = "Workflow";
        meeting.RelatedID = int.Parse(strWLID);
        meeting.Host = strHost;
        meeting.Recorder = strRecorder;
        meeting.Organizer = strOrganizer;
        meeting.Address = strMeetingAddr;
        meeting.Content = strContent;
        meeting.BeginTime = DateTime.Parse(strBeginTime);
        meeting.EndTime = DateTime.Parse(strEndTime);
        meeting.MakeTime = DateTime.Now;
        meeting.Status = strStatus;

        try
        {
            meetingBLL.UpdateMeeting(meeting, int.Parse(strID));

            #region 增加参与人员 By LiuJianping 2013-09-23
            string strMemID = "0";
            if (RP_Attendant.Items.Count > 0)
            {
                for (int i = 0; i < RP_Attendant.Items.Count; i++)
                {
                    if (((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim() == "")
                    {
                    }
                    else
                    {
                        string memid = UpdateCollaborationMember(strID, ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim(), ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim());
                        strMemID += "," + memid;
                    }
                }
                DeleteCollaborationMember(strID, strMemID);
            }
            #endregion

            LoadMyCreatedMeetingList(strWLID, strUserCode);

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZGGCG").ToString().Trim() + "');</script>");

            TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi").ToString().Trim() + strID + " " + meeting.Name.Trim() + "，" + meeting.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen").ToString().Trim() + meeting.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen").ToString().Trim() + meeting.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia").ToString().Trim();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        #region 新增 By LiuJianping 2013-09-23

        string struserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string struserName = ((Button)e.Item.FindControl("BT_UserName")).Text;
        DataSet ds = GetCollaborationMemberModule(RP_Attendant);
        DataTable dt = ds.Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["UserCode"].ToString().Trim() == struserCode.Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYCZBNZFJRJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }
        }

        DataRow dr = dt.NewRow();
        dr["UserName"] = struserName.Trim();
        dr["UserCode"] = struserCode.Trim();
        dt.Rows.Add(dr);

        RP_Attendant.DataSource = ds;
        RP_Attendant.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

        #endregion
    }

    /// <summary>
    /// 增加参与人员 By LiuJianping  2013-09-23
    /// </summary>
    /// <param name="strCoID"></param>
    /// <param name="struserCode"></param>
    /// <param name="struserName"></param>
    protected void AddCollaborationMember(string strCoID, string struserCode, string struserName)
    {
        MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
        MeetingAttendant meetingAttendant = new MeetingAttendant();
        meetingAttendant.MeetingID = int.Parse(strCoID);
        meetingAttendant.UserCode = struserCode.Trim();
        meetingAttendant.UserName = struserName.Trim();

        meetingAttendantBLL.AddMeetingAttendant(meetingAttendant);
    }

    /// <summary>
    /// 更新参与人员 By LiuJianping  2013-09-23
    /// </summary>
    /// <param name="strCoID"></param>
    /// <param name="struserCode"></param>
    /// <param name="struserName"></param>
    protected string UpdateCollaborationMember(string strCoID, string struserCode, string struserName)
    {
        string MemID = "0";
        MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
        string strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = '" + strCoID + "' and meetingAttendant.UserName = '" + struserName + "' and meetingAttendant.UserCode='" + struserCode + "' ";
        IList lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            MeetingAttendant meetingAttendant = (MeetingAttendant)lst[0];
            MemID = meetingAttendant.ID.ToString();
        }
        else
        {
            AddCollaborationMember(strCoID, struserCode, struserName);
            MemID = GetCollaborationMemberID(strCoID, struserCode, struserName);
        }
        return MemID;
    }

    /// <summary>
    /// 删除参与人员 By LiuJianping  2013-09-23
    /// </summary>
    /// <param name="strCoID"></param>
    /// <param name="strMemIDList">如1,2,3,4</param>
    protected void DeleteCollaborationMember(string strCoID, string strMemIDList)
    {
        MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
        string strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = '" + strCoID + "' and meetingAttendant.ID not in (" + strMemIDList + ") ";
        IList lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                MeetingAttendant meetingAttendant = (MeetingAttendant)lst[i];
                meetingAttendantBLL.DeleteMeetingAttendant(meetingAttendant);
            }
        }
    }

    /// <summary>
    /// 获取创建参与人员的即时ID By LiuJianping  2013-09-23
    /// </summary>
    /// <param name="strUserCode"></param>
    /// <returns></returns>
    protected string GetCollaborationMemberID(string strCoID, string struserCode, string struserName)
    {
        string strHQL = "Select Max(ID) From T_MeetingAttendant Where MeetingID = '" + strCoID + "' and UserName = '" + struserName + "' and UserCode='" + struserCode + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MeetingAttendant");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim(); ;
        MeetingBLL meetingBLL = new MeetingBLL();
        IList lst = meetingBLL.GetAllMeetings(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from Meeting as meeting where meeting.ID = " + strID;
                MeetingBLL meetingBLL = new MeetingBLL();
                lst = meetingBLL.GetAllMeetings(strHQL);

                Meeting meeting = new Meeting();
                meeting = (Meeting)lst[0];

                LB_ID.Text = meeting.ID.ToString();
                TB_Name.Text = meeting.Name.Trim();
                DL_MeetingType.SelectedValue = meeting.Type;
                TB_MeetingRoom.Text = meeting.Address;
                TB_Host.Text = meeting.Host;
                TB_Organizer.Text = meeting.Organizer;
                TB_Recorder.Text = meeting.Recorder;
                TB_Content.Text = meeting.Content.Trim();

                DL_BeginYear.SelectedValue = meeting.BeginTime.Year.ToString();
                DL_BeginMonth.SelectedValue = meeting.BeginTime.Month.ToString();
                DL_BeginDay.SelectedValue = meeting.BeginTime.Day.ToString();
                DL_BeginHour.SelectedValue = meeting.BeginTime.Hour.ToString();
                DL_BeginMinute.SelectedValue = meeting.BeginTime.Minute.ToString();

                DL_EndYear.SelectedValue = meeting.EndTime.Year.ToString();
                DL_EndMonth.SelectedValue = meeting.EndTime.Month.ToString();
                DL_EndDay.SelectedValue = meeting.EndTime.Day.ToString();
                DL_EndHour.SelectedValue = meeting.EndTime.Hour.ToString();
                DL_EndMinute.SelectedValue = meeting.EndTime.Minute.ToString();
                DL_Status.SelectedValue = meeting.Status.Trim();

                LoadMeetingAttendant(strID);

                HL_RelatedDoc.NavigateUrl = "TTMeetingDoc.aspx?RelatedID=" + strID;

                HL_MeetingToTask.Enabled = true;
                HL_MeetingToTask.NavigateUrl = "TTMeetingToTask.aspx?ProjectID=1&RelatedType=MEETING&MeetingID=" + strID;

                HL_MakeCollaboration.Enabled = true;
                HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=MEETING&RelatedID=" + strID;

                TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi").ToString().Trim() + strWLID + " " + strWLName + LanguageHandle.GetWord("HuiYi").ToString().Trim() + strID + " " + meeting.Name.Trim() + "，" + meeting.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen").ToString().Trim() + meeting.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen").ToString().Trim() + meeting.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia").ToString().Trim();

                HL_MakeCollaboration.Enabled = true;
                HL_MeetingToTask.Enabled = true;
                HL_RelatedDoc.Enabled = true;
     
                BT_Send.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }


            if (e.CommandName == "Delete")
            {
                string strUserCode = LB_UserCode.Text.Trim();
                MeetingBLL meetingBLL = new MeetingBLL();
                Meeting meeting = new Meeting();
                strHQL = "from Meeting as meeting where meeting.ID = " + strID;
                meetingBLL = new MeetingBLL();
                lst = meetingBLL.GetAllMeetings(strHQL);
                meeting = new Meeting();
                meeting = (Meeting)lst[0];

                try
                {
                    meetingBLL.DeleteMeeting(meeting);

                    #region 删除参与人员 By LiuJianping 2013-09-23
                    DeleteCollaborationMember(strID, "0");

                    LoadMeetingAttendant(strID);

                    #endregion

                    LoadMyCreatedMeetingList(strWLID, strUserCode);
                    
                    BT_Send.Enabled = false;

                    HL_MakeCollaboration.Enabled = false;
                    HL_MeetingToTask.Enabled = false;
                    HL_RelatedDoc.Enabled = false;
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "');</script>");
                }
            }
        }
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            #region 新增  By LiuJianping  2013-09-23
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;
            DataTable dt = GetCollaborationMemberModule(RP_Attendant).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["UserName"].ToString().Trim() == strUserName.Trim())
                {
                    dt.Rows.Remove(dt.Rows[i]);
                }
            }
            RP_Attendant.DataSource = dt;
            RP_Attendant.DataBind();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            #endregion
        }
    }

    protected void InitialCalendar()
    {
        year = new ArrayList();
        month = new ArrayList();
        day = new ArrayList();
        hour = new ArrayList();
        m = new ArrayList();

        for (i = 2000; i <= 2100; i++)
            year.Add(i.ToString());
        for (i = 1; i <= 12; i++)
            month.Add(i.ToString());
        for (i = 1; i <= 31; i++)
            day.Add(i.ToString());
        for (i = 0; i <= 23; i++)
            hour.Add(i.ToString());
        for (i = 00; i <= 59; i++)
            m.Add(i.ToString());

        DL_BeginYear.DataSource = year;
        DL_BeginYear.DataBind();
        DL_BeginYear.Text = System.DateTime.Now.Year.ToString();
        DL_EndYear.Text = System.DateTime.Now.Year.ToString();

        DL_BeginMonth.DataSource = month;
        DL_BeginMonth.DataBind();
        DL_BeginMonth.Text = System.DateTime.Now.Month.ToString();
        DL_EndMonth.Text = System.DateTime.Now.Month.ToString();

        DL_BeginDay.DataSource = day;
        DL_BeginDay.DataBind();
        DL_BeginDay.Text = System.DateTime.Now.Day.ToString();
        DL_EndDay.Text = System.DateTime.Now.Day.ToString();

        DL_BeginHour.DataSource = hour;
        DL_BeginHour.DataBind();
        DL_BeginHour.Text = System.DateTime.Now.Hour.ToString();
        DL_EndHour.Text = System.DateTime.Now.Hour.ToString();
        DL_BeginMinute.DataSource = m;
        DL_BeginMinute.DataBind();
        DL_BeginMinute.Text = System.DateTime.Now.Minute.ToString();

        this.DL_EndYear.DataSource = year;
        this.DL_EndYear.DataBind();

        this.DL_EndMonth.DataSource = month;
        this.DL_EndMonth.DataBind();

        this.DL_EndDay.DataSource = day;
        this.DL_EndDay.DataBind();

        this.DL_EndHour.DataSource = hour;
        this.DL_EndHour.DataBind();
        this.DL_EndMinute.DataSource = m;
        this.DL_EndMinute.DataBind();
        DL_EndMinute.Text = System.DateTime.Now.Minute.ToString();
    }

    /// <summary>
    /// 获取当前参与人员列表 By LiuJianping  2013-09-23
    /// </summary>
    /// <param name="RP"></param>
    /// <returns></returns>
    protected DataSet GetCollaborationMemberModule(Repeater RP)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("CollaborationMemberModule");
        DataColumn dc = new DataColumn();
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("UserCode", typeof(string));
        if (RP.Items.Count > 0)
        {
            for (int i = 0; i < RP.Items.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["UserName"] = ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim();
                dr["UserCode"] = ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim();
                dt.Rows.Add(dr);
            }
        }
        ds.Tables.Add(dt);
        return ds;
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode,  strReceiverCode;
        string strSubject, strMsg;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        string strWLID = LB_ID.Text.Trim();

        strHQL = "from Meeting as meeting where meeting.ID = " + strWLID;
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);
        Meeting meeting = (Meeting)lst[0];

        strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = " + strWLID;
        MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
        lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);

        MeetingAttendant meetingAttendant = new MeetingAttendant();

        Msg msg = new Msg();

        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                meetingAttendant = (MeetingAttendant)lst[i];
                strReceiverCode = meetingAttendant.UserCode.Trim();
                strMsg = TB_Message.Text.Trim();

                if (CB_MSM.Checked == true | CB_Mail.Checked == true)
                {
                    strSubject = LanguageHandle.GetWord("HuiYiTongZhi").ToString().Trim();

                    if (CB_MSM.Checked == true)
                    {
                        msg.SendMSM("Message",strReceiverCode, strMsg, strUserCode);
                    }

                    if (CB_Mail.Checked == true)
                    {
                        msg.SendMail(strReceiverCode, strSubject, strMsg, strUserCode);
                    }
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHYTZFSWB").ToString().Trim() + "')", true);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }


    protected int GetMeetingAttendantCount(string strMeetingID, string strUserName)
    {
        string strHQL;
        IList lst;

        strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = " + strMeetingID + " and meetingAttendant.UserName = " + "'" + strUserName + "'";
        MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
        lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);

        return lst.Count;
    }

    protected void LoadMeetingAttendant(string strMeetingID)
    {
        string strHQL;
        IList lst;

        strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = " + strMeetingID;
        MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
        lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);

        RP_Attendant.DataSource = lst;
        RP_Attendant.DataBind();
    }

    protected void LoadMyCreatedMeetingList(string strWLID, string strUserCode)
    {
        string strHQL = "from Meeting as meeting where meeting.RelatedType = 'Workflow' and meeting.RelatedID = " + strWLID + " order by meeting.ID DESC";
        MeetingBLL meetingBLL = new MeetingBLL();
        IList lst = meetingBLL.GetAllMeetings(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected string GetMeetingName(string strWLID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Meeting as meeting where meeting.ID = " + strWLID;
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);

        Meeting meeting = (Meeting)lst[0];

        string strWLName = meeting.Name.Trim();

        return strWLName;
    }

    protected string GetDepartCode(string strUserCode)
    {
        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        string strDepartCode = projectMember.DepartCode;

        return strDepartCode;
    }
}
