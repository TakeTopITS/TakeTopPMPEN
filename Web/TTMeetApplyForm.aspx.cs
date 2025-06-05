using System; using System.Resources;
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

public partial class TTMeetApplyForm : System.Web.UI.Page
{
    string strUserCode;

    ArrayList hour, m;
    int i;

    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserCode = Session["UserCode"] == null ? "" : Session["UserCode"].ToString().Trim();

        string strHQL;
        IList lst;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        LB_UserCode.Text = strUserCode;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "库存管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "会议维护";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_Content.Visible = true;
            }
            else
            {
                TB_Content.Visible = true;
            }

            DLC_BeginTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"),TreeView2, strUserCode);

            string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);

            strHQL = "from MeetingType as meetingType order by meetingType.SortNumber ASC";
            MeetingTypeBLL meetingTypeBLL = new MeetingTypeBLL();
            lst = meetingTypeBLL.GetAllMeetingTypes(strHQL);

            DL_MeetingType.DataSource = lst;
            DL_MeetingType.DataBind();

            LoadMeetingRoom(strUserCode);

            strHQL = "from Meeting as meeting where meeting.BuilderCode = " + "'" + strUserCode + "'";
            strHQL += " Order by meeting.ID DESC";
            MeetingBLL meetingBLL = new MeetingBLL();
            lst = meetingBLL.GetAllMeetings(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            ShareClass.LoadWFTemplate(strUserCode, "MeetingRequest", DL_TemName);

            LB_Sql.Text = strHQL;

            InitialCalendar();
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);
        }
    }

    protected void DL_Room_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strMeetingRoom = DL_Room.Text.Trim();

        TB_MeetingRoom.Text = strMeetingRoom;
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {

        string strMeetingID;
        string strContent;

        string strUserCode = LB_UserCode.Text.Trim();
        string strMeetingName = TB_Name.Text.Trim();
        string strMeetingType = DL_MeetingType.SelectedValue;
        string strHost = TB_Host.Text.Trim();
        string strRecorder = TB_Recorder.Text.Trim();
        string strBuilderCode = LB_UserCode.Text.Trim();
        string strMeetingAddr = TB_MeetingRoom.Text.Trim();
        string strOrganizer = TB_Organizer.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HT_Content.Text.Trim();
        }
        else
        {
            strContent = TB_Content.Text.Trim();
        }


        DateTime dtBeginTime = DateTime.Parse(DateTime.Parse(DLC_BeginTime.Text).ToString("yyyy/MM/dd ") + DL_BeginHour.SelectedValue + ":" + DL_BeginMinute.SelectedValue);
        DateTime dtEndTime = DateTime.Parse(DateTime.Parse(DLC_EndTime.Text).ToString("yyyy/MM/dd ") + DL_EndHour.SelectedValue + ":" + DL_EndMinute.SelectedValue);

        string strStatus = DL_Status.SelectedValue.Trim();

        MeetingBLL meetingBLL = new MeetingBLL();
        Meeting meeting = new Meeting();

        meeting.Name = strMeetingName;
        meeting.Type = DL_MeetingType.SelectedValue;
        meeting.RelatedType = "Other";
        meeting.RelatedID = 0;
        meeting.Host = strHost;
        meeting.Recorder = strRecorder;
        meeting.Organizer = strOrganizer;
        meeting.BuilderCode = strUserCode;
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
            LB_ID.Text = strMeetingID;

            HL_RelatedDoc.NavigateUrl = "TTMeetingDoc.aspx?RelatedID=" + strMeetingID;


            HL_MeetingToTask.Enabled = true;
            HL_MeetingToTask.NavigateUrl = "TTMeetingToTask.aspx?ProjectID=1&RelatedType=MEETING&MeetingID=" + strMeetingID;

            HL_MakeCollaboration.Enabled = true;
            HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=MEETING&RelatedID=" + strMeetingID;

            LoadMeetingAttendant(strMeetingID);
            LoadMyCreatedMeetingList(strUserCode);

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            HL_MakeCollaboration.Enabled = true;
            HL_MeetingToTask.Enabled = true;
            HL_RelatedDoc.Enabled = true;

            BT_Send.Enabled = true;

            TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi") + strMeetingID + " " + meeting.Name.Trim() + "，" + meeting.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen") + meeting.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen") + meeting.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC")+"')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strContent;

        string strUserCode = LB_UserCode.Text.Trim();
        string strID = LB_ID.Text.Trim();
        string strMeetingName = TB_Name.Text.Trim();
        string strMeetingType = DL_MeetingType.SelectedValue;
        string strHost = TB_Host.Text.Trim();
        string strRecorder = TB_Recorder.Text.Trim();
        string strMeetingAddr = TB_MeetingRoom.Text.Trim();
        string strOrganizer = TB_Organizer.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HT_Content.Text.Trim();
        }
        else
        {
            strContent = TB_Content.Text.Trim();
        }


        DateTime dtBeginTime = DateTime.Parse(DateTime.Parse(DLC_BeginTime.Text).ToString("yyyy/MM/dd ") + DL_BeginHour.SelectedValue + ":" + DL_BeginMinute.SelectedValue);
        DateTime dtEndTime = DateTime.Parse(DateTime.Parse(DLC_EndTime.Text).ToString("yyyy/MM/dd ") + DL_EndHour.SelectedValue + ":" + DL_EndMinute.SelectedValue);

        string strStatus = DL_Status.SelectedValue.Trim();

        MeetingBLL meetingBLL = new MeetingBLL();
        Meeting meeting = new Meeting();
        strHQL = "from Meeting as meeting where meeting.ID = " + strID;
        meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);
        meeting = new Meeting();
        meeting = (Meeting)lst[0];

        meeting.Name = strMeetingName;
        meeting.Type = DL_MeetingType.SelectedValue;
        meeting.RelatedID = 0;
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
            meetingBLL.UpdateMeeting(meeting, int.Parse(strID));

            LoadMyCreatedMeetingList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGCG")+"')", true);

            TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi") + strID + " " + meeting.Name.Trim() + "，" + meeting.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen") + meeting.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen") + meeting.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia");

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGGSBJC")+"')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode = LB_UserCode.Text.Trim();
        string strID = LB_ID.Text.Trim();

        strHQL = "Delete From T_Meeting Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadMyCreatedMeetingList(strUserCode);

            BT_Delete.Enabled = false;
            BT_Update.Enabled = false;
            BT_Send.Enabled = false;

            HL_MakeCollaboration.Enabled = false;
            HL_MeetingToTask.Enabled = false;
            HL_RelatedDoc.Enabled = false;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZSCSBJC")+"')", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = LB_ID.Text.Trim();

        if (strID != "")
        {
            string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
            string strUserName = ShareClass.GetUserName(strUserCode);

            MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
            strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = " + strID + " and meetingAttendant.UserCode = " + "'" + strUserCode + "'";
            lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);

            if (lst.Count == 0)
            {
                MeetingAttendant meetingAttendant = new MeetingAttendant();

                meetingAttendant.UserCode = strUserCode;
                meetingAttendant.UserName = strUserName;
                meetingAttendant.MeetingID = int.Parse(strID);

                try
                {
                    meetingAttendantBLL.AddMeetingAttendant(meetingAttendant);

                    LoadMeetingAttendant(strID);
                }
                catch
                {
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCCYYCZBNZFJRJC")+"')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWKJLBNZJCY")+"')", true);
        }
    }


    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        int intWLNumber;
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text;

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
            TB_MeetingRoom.Text = meeting.Address.Trim();
            TB_Host.Text = meeting.Host;
            TB_Organizer.Text = meeting.Organizer;
            TB_Recorder.Text = meeting.Recorder;

            if (strIsMobileDevice == "YES")
            {
                HT_Content.Text = meeting.Content.Trim();
            }
            else
            {
                TB_Content.Text = meeting.Content.Trim();
            }

            DLC_BeginTime.Text = meeting.BeginTime.ToString("yyyy-MM-dd");
            DL_BeginHour.SelectedValue = meeting.BeginTime.Hour.ToString();
            DL_BeginMinute.SelectedValue = meeting.BeginTime.Minute.ToString();
            DLC_EndTime.Text = meeting.EndTime.ToString("yyyy-MM-dd");
            DL_EndHour.SelectedValue = meeting.EndTime.Hour.ToString();
            DL_EndMinute.SelectedValue = meeting.EndTime.Minute.ToString();

            DL_Status.SelectedValue = meeting.Status.Trim();

            LoadMeetingAttendant(strID);

            HL_MakeCollaboration.Enabled = true;
            HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=MEETING&RelatedID=" + strID;

            HL_MeetingToTask.Enabled = true;
            HL_MeetingToTask.NavigateUrl = "TTMeetingToTask.aspx?ProjectID=1&RelatedType=MEETING&MeetingID=" + strID;

            HL_RelatedDoc.NavigateUrl = "TTMeetingDoc.aspx?RelatedID=" + strID;

            TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi") + strID + " " + meeting.Name.Trim() + "," + meeting.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen") + meeting.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen") + meeting.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia");

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
            BT_Send.Enabled = true;

            HL_RelatedDoc.Enabled = true;
            HL_MakeCollaboration.Enabled = true;

            BT_SubmitApply.Enabled = true;

            LoadRelatedWL("MeetingRequest", "Other", int.Parse(strID));

            intWLNumber = GetRelatedWorkFlowNumber("MeetingRequest", "Other", strID);

            if (intWLNumber > 0)
            {
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;

                BT_SubmitApply.Enabled = false;
            }
        }
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

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

            string strHQL, strID;
            IList lst;

            strID = LB_ID.Text.Trim();
            strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = " + strID + " and meetingAttendant.UserName = " + "'" + strUserName + "'";
            MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
            lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);

            MeetingAttendant meetingAttendant = (MeetingAttendant)lst[0];

            meetingAttendantBLL.DeleteMeetingAttendant(meetingAttendant);

            LoadMeetingAttendant(strID);
        }
    }

    protected void InitialCalendar()
    {
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

        string strMeetingID = LB_ID.Text.Trim();

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
                    strSubject = LanguageHandle.GetWord("HuiYiTongZhi");

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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHYTZFSWB")+"')", true);
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

    protected void LoadMyCreatedMeetingList(string strUserCode)
    {
        string strHQL = "from Meeting as meeting where meeting.BuilderCode = " + "'" + strUserCode + "'" + " Order by meeting.ID DESC";
        MeetingBLL meetingBLL = new MeetingBLL();
        IList lst = meetingBLL.GetAllMeetings(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadMeetingRoom(string strUserCode)
    {
        string strHQL;
        string strDepartCode, strDepartString;

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByAuthority(strUserCode);

        strHQL = " Select RoomName From T_MeetingRoom Where ";
        strHQL += " (BelongDepartCode in (select ParentDepartCode from F_GetParentDepartCode(" + "'" + strDepartCode + "'" + "))";
        strHQL += " Or BelongDepartCode in " + strDepartString + ")";
        strHQL += " Order By ID ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MeetingRoom");


        DL_Room.DataSource = ds;
        DL_Room.DataBind();
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



    protected void BT_Reflash_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlowTemplate as workFlowTemplate where workFlowTemplate.Type = 'MeetingRequest'";
        strHQL += " and workFlowTemplate.Visible = 'YES' Order By workFlowTemplate.SortNumber ASC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DL_TemName.DataSource = lst;
        DL_TemName.DataBind();
    }


    protected void BT_ActiveYes_Click(object sender, EventArgs e)
    {
        string strWLID = SubmitApply();

        if (strWLID != "0")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop11", "popShowByURL('TTMyWorkDetailMain.aspx?RelatedType=Other&WLID=" + strWLID + "','workflow','99%','99%',window.location);", true);
        }
    }

    protected void BT_ActiveNo_Click(object sender, EventArgs e)
    {
        SubmitApply();
    }


    protected string SubmitApply()
    {
        string strHQL, strCmdText;
        string strID, strWLID, strXMLFileName, strXMLFile2;
        string strTemName;


        strID = LB_ID.Text.Trim();
        strWLID = "0";

        strTemName = DL_TemName.SelectedValue.Trim();
        if (strTemName == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWLCMBBNWKJC")+"')", true);
            return strWLID;
        }


        XMLProcess xmlProcess = new XMLProcess();

        strHQL = "Update T_Meeting Set Status = 'InProgress' Where ID = " + strID;


        try
        {
            ShareClass.RunSqlCommand(strHQL);

            strXMLFileName = "MeetingRequest" + DateTime.Now.ToString("yyyyMMddHHMMssff") + ".xml";
            strXMLFile2 = "Doc\\" + "XML" + "\\" + strXMLFileName;



            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            WorkFlow workFlow = new WorkFlow();

            workFlow.WLName = "MeetingRequest";
            workFlow.WLType = "MeetingRequest";
            workFlow.Status = "New";
            workFlow.TemName = DL_TemName.SelectedValue.Trim();
            workFlow.CreateTime = DateTime.Now;
            workFlow.CreatorCode = strUserCode;
            workFlow.CreatorName = GetUserName(strUserCode);
            workFlow.Description = "MeetingRequest";
            workFlow.XMLFile = strXMLFile2;
            workFlow.RelatedType = "Other";
            workFlow.RelatedID = int.Parse(strID);
            workFlow.DIYNextStep = "YES"; workFlow.IsPlanMainWorkflow = "NO";

            if (CB_SMS.Checked == true)
            {
                workFlow.ReceiveSMS = "YES";
            }
            else
            {
                workFlow.ReceiveSMS = "NO";
            }

            if (CB_Mail.Checked == true)
            {
                workFlow.ReceiveEMail = "YES";
            }
            else
            {
                workFlow.ReceiveEMail = "NO";
            }

            try
            {
                workFlowBLL.AddWorkFlow(workFlow);
                strWLID = ShareClass.GetMyCreatedWorkFlowID(strUserCode);

                //strCmdText = "select * from T_Meeting where ID = " + strID;
                strCmdText = string.Format(@"select ID,Name,Type,RelatedType,RelatedID,Host,Recorder,Content,
                            Address,
                            to_char(BeginTime, 'yyyy-mm-dd hh:mi:ss') as BeginTime,
                            to_char(EndTime, 'yyyy-mm-dd hh:mi:ss') as EndTime,
                            BuilderCode,
                            Organizer,
                            Record,
                            to_char(MakeTime, 'yyyy-mm-dd hh:mi:ss') as MakeTime,
                            Status from T_Meeting where ID = {0}", strID);

                strXMLFile2 = Server.MapPath(strXMLFile2);
                xmlProcess.DbToXML(strCmdText, "T_Meeting", strXMLFile2);

                LoadRelatedWL("MeetingRequest", "Other", int.Parse(strID));

                DL_Status.SelectedValue = "InProgress";

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;

                BT_SubmitApply.Enabled = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHYSSCCG")+"')", true);
            }
            catch
            {
                strWLID = "0";

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZHYSSCSBKNSMCZSGDJC")+"')", true);
            }

            LoadMeetApplyForm(strUserCode);
        }
        catch
        {
            strWLID = "0";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC")+"')", true);
        }

        return strWLID;
    }


    protected string GetUserName(string strUserCode)
    {
        string strUserName;

        string strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }

    protected void LoadRelatedWL(string strWLType, string strRelatedType, int intRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType=" + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + intRelatedID.ToString() + " Order by workFlow.WLID DESC";
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }


    protected void LoadMeetApplyForm(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Meeting as meeting Where meeting.BuilderCode = " + "'" + strUserCode + "'" + " Order By meeting.ID DESC";
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }


    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
    }

    private string CheckCurrentTimeMeetingNumber(DateTime strBeginTime, DateTime strEndTime)
    {
        string strResult = "0";
        //把输入的最小时间与数据库最大的时间比较，如果最小时间比数据库最大时间大，刚不在区间内
        //把输入的最大时间与数据库最小的时间比较，如果最大时间比数据库最小时间小，刚不在区间内
        string strMeetHQL = string.Format(@"
                        select COUNT(1) as MeentCount from
                        (
                        select * from T_MeetingAttendant
                        where MeetingID in
                        (
                            select ID from T_Meeting
                            where BeginTime < '{0}'
                            and EndTime > '{1}'
                        )
                        ) t", strEndTime, strBeginTime);
        DataTable dtMeet = ShareClass.GetDataSetFromSql(strMeetHQL, "strMeetHQL").Tables[0];
        if (dtMeet != null && dtMeet.Rows.Count > 0)
        {
            strResult = dtMeet.Rows[0]["MeentCount"] == DBNull.Value ? "0" : dtMeet.Rows[0]["MeentCount"].ToString();
        }
        return strResult;
    }
}