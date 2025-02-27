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

public partial class TTMakeProMeeting : System.Web.UI.Page
{
    ArrayList year, month, day, hour, m;
    int i;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;

        string strProjectID = Request.QueryString["ProjectID"];
        string strProjectName = GetProjectName(strProjectID);

        LB_ProjectID.Text = strProjectID;

        //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + strProjectName + "的会议安排！";      

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            DLC_BeginTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ShareClass.InitialProjectMemberTree(TreeView1, strProjectID);

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

            strHQL = "from Meeting as meeting where meeting.RelatedType = 'Project' and meeting.RelatedID = " + strProjectID + " Order by meeting.ID DESC";
            MeetingBLL meetingBLL = new MeetingBLL();
            lst = meetingBLL.GetAllMeetings(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            HL_RelatedDoc.NavigateUrl = "TTProjectRelatedDoc.aspx?ProjectID=" + strProjectID;

            InitialCalendar();
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strProjectID, strID, strMeetingID;
        string strUserCode, strUserName;

        strProjectID = LB_ProjectID.Text.Trim();
        strID = TreeView1.SelectedNode.Target.Trim();
        strMeetingID = LB_MeetingID.Text.Trim();

        try
        {
            #region 注销 By LiuJianping 2013-09-23
            //strMeetingID = int.Parse(strMeetingID).ToString();
            #endregion

            #region 新增 By LiuJianping 2013-09-23
            strMeetingID = int.Parse(strMeetingID == "" ? "0" : strMeetingID).ToString();
            #endregion

            strHQL = "from ProRelatedUser as proRelatedUser Where proRelatedUser.ID = " + strID;
            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            if (lst.Count > 0)
            {
                ProRelatedUser proRelatedUser = (ProRelatedUser)lst[0];

                strUserCode = proRelatedUser.UserCode.Trim();
                strUserName = proRelatedUser.UserName.Trim();

                if (GetMeetingAttendantCount(strMeetingID, strUserName) == 0)
                {
                    #region 注销 By LiuJianping 2013-09-23
                    //MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
                    //MeetingAttendant meetingAttendant = new MeetingAttendant();

                    //meetingAttendant.UserCode = strUserCode;
                    //meetingAttendant.UserName = strUserName;
                    //meetingAttendant.MeetingID = int.Parse(strMeetingID);

                    //try
                    //{
                    //    meetingAttendantBLL.AddMeetingAttendant(meetingAttendant);

                    //    LoadMeetingAttendant(strMeetingID);
                    //}
                    //catch
                    //{
                    //}
                    #endregion

                    #region 新增 By LiuJianping 2013-09-23

                    DataSet ds = GetCollaborationMemberModule(RP_Attendant);
                    DataTable dt = ds.Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["UserCode"].ToString().Trim() == strUserCode)
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYCZBNZFJRJC").ToString().Trim() + "')", true);
                            return;
                        }
                    }

                    DataRow dr = dt.NewRow();
                    dr["UserName"] = strUserName;
                    dr["UserCode"] = strUserCode;
                    dt.Rows.Add(dr);

                    RP_Attendant.DataSource = ds;
                    RP_Attendant.DataBind();

                    #endregion
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCCYCZBNZFTJ").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXJLBNTJHYCY").ToString().Trim() + "')", true);
            }
        }
        catch
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_Room_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strMeetingRoom;

        strMeetingRoom = DL_Room.SelectedValue.Trim();

        TB_MeetingRoom.Text = strMeetingRoom;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_MeetingID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_MeetingID.Text.Trim();

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
        string strProjectID = LB_ProjectID.Text.Trim();
        string strMeetingName = TB_Name.Text.Trim();
        string strMeetingType = DL_MeetingType.SelectedValue;
        string strHost = TB_Host.Text.Trim();
        string strRecorder = TB_Recorder.Text.Trim();
        string strBuilderCode = LB_UserCode.Text.Trim();
        string strMeetingAddr = TB_MeetingRoom.Text.Trim();
        string strOrganizer = TB_Organizer.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        DateTime dtBeginTime = DateTime.Parse(DateTime.Parse(DLC_BeginTime.Text).ToString("yyyy/MM/dd ") + DL_BeginHour.SelectedValue + ":" + DL_BeginMinute.SelectedValue);
        DateTime dtEndTime = DateTime.Parse(DateTime.Parse(DLC_EndTime.Text).ToString("yyyy/MM/dd ") + DL_EndHour.SelectedValue + ":" + DL_EndMinute.SelectedValue);

        string strStatus = DL_Status.SelectedValue.Trim();

        MeetingBLL meetingBLL = new MeetingBLL();
        Meeting meeting = new Meeting();

        meeting.Name = strMeetingName;
        meeting.Type = strMeetingType;
        meeting.RelatedType = "Project";
        meeting.RelatedID = int.Parse(strProjectID);
        meeting.Host = strHost;
        meeting.Recorder = strRecorder;
        meeting.Organizer = strOrganizer;
        meeting.BuilderCode = strBuilderCode;
        meeting.Address = strMeetingAddr;
        meeting.Content = strContent;
        meeting.BeginTime = dtBeginTime;
        meeting.EndTime = dtEndTime;
        meeting.MakeTime = DateTime.Now;
        meeting.Status = strStatus;

        try
        {
            meetingBLL.AddMeeting(meeting);

            strMeetingID = ShareClass.GetMyCreatedMaxMeetingID(strUserCode);
            LB_MeetingID.Text = strMeetingID;

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

            strProjectID = LB_ProjectID.Text.Trim();
            HL_MeetingToTask.Enabled = true;
            HL_MeetingToTask.NavigateUrl = "TTMeetingToTask.aspx?ProjectID=1&RelatedType=MEETING&MeetingID=" + strMeetingID;

            HL_MakeCollaboration.Enabled = true;
            HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=MEETING&RelatedID=" + strMeetingID;

            LoadMeetingAttendant(strMeetingID);
            LoadMyCreatedMeetingList(strProjectID, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);

         
            HL_RelatedDoc.Enabled = true;
            BT_Send.Enabled = true;

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
        string strMeetingID = LB_MeetingID.Text.Trim();
        string strProjectID = LB_ProjectID.Text.Trim();
        string strMeetingName = TB_Name.Text.Trim();
        string strMeetingType = DL_MeetingType.SelectedValue;
        string strHost = TB_Host.Text.Trim();
        string strRecorder = TB_Recorder.Text.Trim();
        string strMeetingAddr = TB_MeetingRoom.Text.Trim();
        string strOrganizer = TB_Organizer.Text.Trim();
        string strContent = TB_Content.Text.Trim();

        DateTime dtBeginTime = DateTime.Parse(DateTime.Parse(DLC_BeginTime.Text).ToString("yyyy/MM/dd ") + DL_BeginHour.SelectedValue + ":" + DL_BeginMinute.SelectedValue);
        DateTime dtEndTime = DateTime.Parse(DateTime.Parse(DLC_EndTime.Text).ToString("yyyy/MM/dd ") + DL_EndHour.SelectedValue + ":" + DL_EndMinute.SelectedValue);

        string strStatus = DL_Status.SelectedValue.Trim();

        strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
        MeetingBLL meetingBLL = new MeetingBLL();
        meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);
        Meeting meeting = (Meeting)lst[0];

        meeting.Name = strMeetingName;
        meeting.Type = strMeetingType;
        meeting.RelatedType = "Project";
        meeting.RelatedID = int.Parse(strProjectID);
        meeting.Host = strHost;
        meeting.Recorder = strRecorder;
        meeting.Organizer = strOrganizer;
        meeting.Address = strMeetingAddr;
        meeting.Content = strContent;
        meeting.BeginTime = dtBeginTime;
        meeting.EndTime = dtEndTime;
        meeting.MakeTime = DateTime.Now;
        meeting.Status = strStatus;

        try
        {
            meetingBLL.UpdateMeeting(meeting, int.Parse(strMeetingID));

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
                        string memid = UpdateCollaborationMember(strMeetingID, ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim(), ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim());
                        strMemID += "," + memid;
                    }
                }
                DeleteCollaborationMember(strMeetingID, strMemID);
            }
            #endregion

            LoadMyCreatedMeetingList(strProjectID, strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGCG").ToString().Trim() + "')", true);

            TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi").ToString().Trim() + strMeetingID + " " + meeting.Name.Trim() + "，" + meeting.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen").ToString().Trim() + meeting.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen").ToString().Trim() + meeting.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia").ToString().Trim();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
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

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strMeetingID, strProjectID;

        if (e.CommandName != "Page")
        {
            strMeetingID = e.Item.Cells[2].Text.Trim();
            strProjectID = LB_ProjectID.Text.Trim();
          
            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
                MeetingBLL meetingBLL = new MeetingBLL();
                lst = meetingBLL.GetAllMeetings(strHQL);

                Meeting meeting = new Meeting();
                meeting = (Meeting)lst[0];

                LB_MeetingID.Text = meeting.ID.ToString();
                TB_Name.Text = meeting.Name.Trim();
                DL_MeetingType.SelectedValue = meeting.Type;
                TB_MeetingRoom.Text = meeting.Address.Trim();
                TB_Host.Text = meeting.Host;
                TB_Organizer.Text = meeting.Organizer;
                TB_Recorder.Text = meeting.Recorder;
                TB_Content.Text = meeting.Content.Trim();

                DLC_BeginTime.Text = meeting.BeginTime.ToString("yyyy-MM-dd");
                DL_BeginHour.SelectedValue = meeting.BeginTime.Hour.ToString();
                DL_BeginMinute.SelectedValue = meeting.BeginTime.Minute.ToString();
                DLC_EndTime.Text = meeting.EndTime.ToString("yyyy-MM-dd");
                DL_EndHour.SelectedValue = meeting.EndTime.Hour.ToString();
                DL_EndMinute.SelectedValue = meeting.EndTime.Minute.ToString();

                DL_Status.SelectedValue = meeting.Status.Trim();

                LoadMeetingAttendant(strMeetingID);

                TB_Message.Text = LanguageHandle.GetWord("NiYiBeiYaoQingCanJiaHuiYi").ToString().Trim() + strMeetingID + " " + meeting.Name.Trim() + LanguageHandle.GetWord("ZhuChiRen").ToString().Trim() + meeting.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen").ToString().Trim() + meeting.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia").ToString().Trim();


                HL_RelatedDoc.Enabled = true;
                HL_RelatedDoc.NavigateUrl = "TTMeetingDoc.aspx?RelatedID=" + strMeetingID;
             
                HL_MeetingToTask.Enabled = true;
                HL_MeetingToTask.NavigateUrl = "TTMeetingToTask.aspx?ProjectID=1&RelatedType=MEETING&MeetingID=" + strMeetingID;

                HL_MakeCollaboration.Enabled = true;
                HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=MEETING&RelatedID=" + strMeetingID;

                BT_Send.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strUserCode = LB_UserCode.Text.Trim();
             
                strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
                MeetingBLL meetingBLL = new MeetingBLL();
                meetingBLL = new MeetingBLL();
                lst = meetingBLL.GetAllMeetings(strHQL);
                Meeting meeting = new Meeting();
                meeting = (Meeting)lst[0];

                try
                {
                    meetingBLL.DeleteMeeting(meeting);

                    #region 删除参与人员 By LiuJianping 2013-09-23
                    DeleteCollaborationMember(strMeetingID, "0");

                    LoadMeetingAttendant(strMeetingID);

                    #endregion

                    LoadMyCreatedMeetingList(strProjectID, strUserCode);

                 

                    HL_MakeCollaboration.Enabled = false;
                    HL_MeetingToTask.Enabled = false;

                    HL_RelatedDoc.Enabled = false;
                    BT_Send.Enabled = false;

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
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

    protected string GetProjectName(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        string strProjectName = project.ProjectName.Trim();
        return strProjectName;
    }

    protected void InitialCalendar()
    {
        year = new ArrayList();
        month = new ArrayList();
        day = new ArrayList();
        hour = new ArrayList();
        m = new ArrayList();

        for (i = 0; i <= 23; i++)
            hour.Add(i.ToString());
        for (i = 00; i <= 59; i++)
            m.Add(i.ToString());

        DL_BeginHour.DataSource = hour;
        DL_BeginHour.DataBind();
        DL_BeginHour.Text = System.DateTime.Now.Hour.ToString();
        DL_EndHour.Text = System.DateTime.Now.Hour.ToString();
        DL_BeginMinute.DataSource = m;
        DL_BeginMinute.DataBind();
        DL_BeginMinute.Text = System.DateTime.Now.Minute.ToString();

        this.DL_EndHour.DataSource = hour;
        this.DL_EndHour.DataBind();
        this.DL_EndMinute.DataSource = m;
        this.DL_EndMinute.DataBind();
        DL_EndMinute.Text = System.DateTime.Now.Minute.ToString();
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode, strReceiverCode;
        string strSubject, strMsg;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        string strMeetingID = LB_MeetingID.Text.Trim();

        strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);
        Meeting meeting = (Meeting)lst[0];

        strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = " + strMeetingID;
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
                    strSubject = LanguageHandle.GetWord("XiangMuHuiYiTongZhi").ToString().Trim();

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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXMHYTZFSWB").ToString().Trim() + "')", true);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void LoadMyCreatedMeetingList(string strProjectID, string strUserCode)
    {

        string strHQL = "from Meeting as meeting where meeting.RelatedType = 'Project' and meeting.RelatedID = " + strProjectID + " Order by meeting.ID DESC";
        MeetingBLL meetingBLL = new MeetingBLL();
        IList lst = meetingBLL.GetAllMeetings(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected string GetMeetingName(string strMeetingID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);

        Meeting meeting = (Meeting)lst[0];

        string strMeetingName = meeting.Name.Trim();

        return strMeetingName;
    }

    protected Requirement GetRequirement(string strReqID)
    {
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();

        IList lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement;
    }
}
