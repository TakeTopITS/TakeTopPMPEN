using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTMeetAssignForm : System.Web.UI.Page
{
    string strUserCode;
    ArrayList hour, m;
    int i;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "库存管理", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            TB_Content.Visible = true;

            DLC_BeginTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");


            InitialCalendar();

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView2, strUserCode);

            string strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            LB_UserCode.Text = strUserCode;
            LB_UserName.Text = GetUserName(strUserCode);

            strHQL = "from MeetingType as meetingType order by meetingType.SortNumber ASC";
            MeetingTypeBLL meetingTypeBLL = new MeetingTypeBLL();
            lst = meetingTypeBLL.GetAllMeetingTypes(strHQL);

            DL_MeetingType.DataSource = lst;
            DL_MeetingType.DataBind();

            LoadMeetingRoom(strUserCode);

            LoadMeetingList(strUserCode);
            LoadMeetingAssignForm(strUserCode);

            GetMeetCount();
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

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strApplyFormID;

        if (e.CommandName != "Page")
        {
            strApplyFormID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

            NB_ApplyFormID.Amount = int.Parse(strApplyFormID);

            strHQL = "from Meeting as meeting where meeting.ID = " + strApplyFormID;
            MeetingBLL meetingBLL = new MeetingBLL();
            lst = meetingBLL.GetAllMeetings(strHQL);
            Meeting meeting = (Meeting)lst[0];

            NB_ApplyFormID.Amount = decimal.Parse(strApplyFormID);
            TB_Name.Text = meeting.Name.Trim();
            DL_MeetingType.Text = meeting.Type;
            TB_MeetingRoom.Text = meeting.Address;

            TB_Host.Text = meeting.Host;
            TB_Organizer.Text = meeting.Organizer.Trim();

            TB_Recorder.Text = meeting.Recorder;
            DLC_BeginTime.Text = meeting.BeginTime.ToString("yyyy-MM-dd");
            DL_BeginHour.SelectedValue = meeting.BeginTime.Hour.ToString();
            DL_BeginMinute.SelectedValue = meeting.BeginTime.Minute.ToString();

            DLC_EndTime.Text = meeting.EndTime.ToString("yyyy-MM-dd");
            DL_EndHour.SelectedValue = meeting.EndTime.Hour.ToString();
            DL_EndMinute.SelectedValue = meeting.EndTime.Minute.ToString();

            //加载与会人员
            LoadMeetingAttendant(strApplyFormID);
            TB_Content.Text = meeting.Content.Trim();


            LoadRelatedWL("VehicleRequest", "Other", int.Parse(strApplyFormID));
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = NB_ApplyFormID.Amount.ToString();//LB_ID.Text.Trim();

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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYCZBNZFJRJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWKJLBNZJCY") + "')", true);
        }
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strMeetingID;
        string strApplyFormID;
        string strContent;

        string strUserCode = LB_UserCode.Text.Trim();
        string strMeetingName = TB_Name.Text.Trim();
        string strMeetingType = DL_MeetingType.SelectedValue;
        string strHost = TB_Host.Text.Trim();
        string strRecorder = TB_Recorder.Text.Trim();
        string strBuilderCode = LB_UserCode.Text.Trim();
        string strMeetingAddr = TB_MeetingRoom.Text.Trim();
        string strOrganizer = TB_Organizer.Text.Trim();

        strContent = TB_Content.Text.Trim();

        DateTime dtBeginTime = DateTime.Parse(DateTime.Parse(DLC_BeginTime.Text).ToString("yyyy/MM/dd ") + DL_BeginHour.SelectedValue + ":" + DL_BeginMinute.SelectedValue);
        DateTime dtEndTime = DateTime.Parse(DateTime.Parse(DLC_EndTime.Text).ToString("yyyy/MM/dd ") + DL_EndHour.SelectedValue + ":" + DL_EndMinute.SelectedValue);

        string strStatus = DL_Status.SelectedValue.Trim();
        string strMainPss = TB_MainPass.Text.Trim();
        string strPassword = TXT_Password.Text.Trim();


        strApplyFormID = NB_ApplyFormID.Amount.ToString();

        MeetingAssignBLL meetingAssignBLL = new MeetingAssignBLL();
        MeetingAssign meetingAssign = new MeetingAssign();

        meetingAssign.MeetingApplyID = int.Parse(strApplyFormID);
        meetingAssign.Name = strMeetingName;
        meetingAssign.Type = DL_MeetingType.SelectedValue;
        meetingAssign.RelatedType = "Other";
        meetingAssign.RelatedID = 0;
        meetingAssign.Host = strHost;
        meetingAssign.Recorder = strRecorder;
        meetingAssign.Organizer = strOrganizer;
        meetingAssign.BuilderCode = strUserCode;
        meetingAssign.Address = strMeetingAddr;
        meetingAssign.Content = strContent;
        meetingAssign.BeginTime = dtBeginTime;
        meetingAssign.EndTime = dtEndTime;
        meetingAssign.MakeTime = DateTime.Now;
        meetingAssign.Status = strStatus;
        meetingAssign.MainPass = strMainPss;
        meetingAssign.MeetPassword = strPassword;

        try
        {

            string strJoinUserHQL = string.Format(@"select distinct a.UserCode,p.PasswordShal from T_MeetingAttendant a
                        left join T_ProjectMember p on a.UserCode = p.UserCode
                        where a.MeetingID = {0}", strApplyFormID);
            DataTable dtJoinUserPass = ShareClass.GetDataSetFromSql(strJoinUserHQL, "strJoinUserHQL").Tables[0];
            string strJoinMember = string.Empty;//参与人员用户ID，密码

            if (dtJoinUserPass != null && dtJoinUserPass.Rows.Count > 0)
            {
                foreach (DataRow dr in dtJoinUserPass.Rows)
                {
                    string strJoinUserCode = ShareClass.ObjectToString(dr["UserCode"]);
                    string strJoinPassword = PassWordAfterStr(strPassword); //ShareClass.ObjectToString(dr["PasswordShal"]);
                    strJoinMember += strJoinUserCode + "," + strJoinPassword + "|";
                }
            }
            //strJoinMember = strJoinMember.EndsWith("|") ? strJoinMember.TrimEnd('|') : strJoinMember;

            string strCreatePass = string.Empty;//创建用户密码
            string strCreatePassHQL = string.Format("select PasswordShal from T_ProjectMember where UserCode = '{0}'", meetingAssign.BuilderCode);
            DataTable dtCreatePass = ShareClass.GetDataSetFromSql(strCreatePassHQL, "strCreatePassHQL").Tables[0];
            if (dtCreatePass != null && dtCreatePass.Rows.Count > 0)
            {
                strCreatePass = ShareClass.ObjectToString(dtCreatePass.Rows[0]["PasswordShal"]);
            }
            strJoinMember += meetingAssign.BuilderCode + "," + strCreatePass;

            //调用视频会议接口
            string strResult = GetThreeMeetingMethod(meetingAssign.Name, meetingAssign.BuilderCode, meetingAssign.BeginTime,
                meetingAssign.EndTime, meetingAssign.MainPass, strCreatePass, strJoinMember);

            if (strResult == "succ")
            {
                meetingAssignBLL.AddMeetingAssign(meetingAssign);

                strMeetingID = GetMyCreatedMaxMeetAssignID(strUserCode);
                LB_ID.Text = strMeetingID;

                HL_RelatedDoc.NavigateUrl = "TTMeetingDoc.aspx?RelatedID=" + strApplyFormID;


                HL_MeetingToTask.Enabled = true;
                HL_MeetingToTask.NavigateUrl = "TTMeetingToTask.aspx?ProjectID=1&RelatedType=MEETING&MeetingID=" + strApplyFormID;

                HL_MakeCollaboration.Enabled = true;
                HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=MEETING&RelatedID=" + strApplyFormID;

                LoadMeetingAttendant(strApplyFormID);
                LoadMeetingAssignForm(strUserCode);

                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                HL_MakeCollaboration.Enabled = true;
                HL_MeetingToTask.Enabled = true;
                HL_RelatedDoc.Enabled = true;

                BT_Send.Enabled = true;

                TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi") + strMeetingID + " " + meetingAssign.Name.Trim() + "，" + meetingAssign.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen") + meetingAssign.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen") + meetingAssign.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia");



                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSTRRESULT") + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);
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

        strContent = TB_Content.Text.Trim();

        DateTime dtBeginTime = DateTime.Parse(DateTime.Parse(DLC_BeginTime.Text).ToString("yyyy/MM/dd ") + DL_BeginHour.SelectedValue + ":" + DL_BeginMinute.SelectedValue);
        DateTime dtEndTime = DateTime.Parse(DateTime.Parse(DLC_EndTime.Text).ToString("yyyy/MM/dd ") + DL_EndHour.SelectedValue + ":" + DL_EndMinute.SelectedValue);

        string strStatus = DL_Status.SelectedValue.Trim();
        string strMainPss = TB_MainPass.Text.Trim();
        string strApplyFormID = NB_ApplyFormID.Amount.ToString();

        MeetingAssignBLL meetingAssignBLL = new MeetingAssignBLL();
        MeetingAssign meetingAssign = new MeetingAssign();
        strHQL = "from MeetingAssign as meetingAssign where meetingAssign.ID = " + strID;
        lst = meetingAssignBLL.GetAllMeetingAssigns(strHQL);
        meetingAssign = (MeetingAssign)lst[0];

        meetingAssign.MeetingApplyID = int.Parse(strApplyFormID);
        meetingAssign.Name = strMeetingName;
        meetingAssign.Type = DL_MeetingType.SelectedValue;
        meetingAssign.RelatedID = 0;
        meetingAssign.Host = strHost;
        meetingAssign.Recorder = strRecorder;
        meetingAssign.Organizer = strOrganizer;
        meetingAssign.Address = strMeetingAddr;
        meetingAssign.Content = strContent;
        meetingAssign.BeginTime = dtBeginTime;
        meetingAssign.EndTime = dtEndTime;
        meetingAssign.MakeTime = DateTime.Now;
        meetingAssign.Status = strStatus;
        meetingAssign.MainPass = strMainPss;

        try
        {
            meetingAssignBLL.UpdateMeetingAssign(meetingAssign, int.Parse(strID));

            LoadMeetingAssignForm(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGCG") + "')", true);

            TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi") + strID + " " + meetingAssign.Name.Trim() + "，" + meetingAssign.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen") + meetingAssign.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen") + meetingAssign.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia");

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJC") + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strUserCode = LB_UserCode.Text.Trim();
        string strID = LB_ID.Text.Trim();

        strHQL = "Delete From T_MeetingAssign Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadMeetingAssignForm(strUserCode);

            BT_Delete.Enabled = false;
            BT_Update.Enabled = false;
            BT_Send.Enabled = false;

            HL_MakeCollaboration.Enabled = false;
            HL_MeetingToTask.Enabled = false;
            HL_RelatedDoc.Enabled = false;
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        int intWLNumber;
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text;

            for (int i = 0; i < DataGrid1.Items.Count; i++)
            {
                DataGrid1.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;


            strHQL = "from MeetingAssign as meetingAssign where meetingAssign.ID = " + strID;
            MeetingAssignBLL meetingAssignBLL = new MeetingAssignBLL();
            lst = meetingAssignBLL.GetAllMeetingAssigns(strHQL);

            MeetingAssign meetingAssign = new MeetingAssign();
            meetingAssign = (MeetingAssign)lst[0];

            NB_ApplyFormID.Amount = meetingAssign.MeetingApplyID;
            LB_ID.Text = meetingAssign.ID.ToString();
            TB_Name.Text = meetingAssign.Name.Trim();
            DL_MeetingType.SelectedValue = meetingAssign.Type;
            TB_MeetingRoom.Text = meetingAssign.Address.Trim();
            TB_Host.Text = meetingAssign.Host;
            TB_Organizer.Text = meetingAssign.Organizer;
            TB_Recorder.Text = meetingAssign.Recorder;
            TB_Content.Text = meetingAssign.Content.Trim();
            DLC_BeginTime.Text = meetingAssign.BeginTime.ToString("yyyy-MM-dd");
            DL_BeginHour.SelectedValue = meetingAssign.BeginTime.Hour.ToString();
            DL_BeginMinute.SelectedValue = meetingAssign.BeginTime.Minute.ToString();
            DLC_EndTime.Text = meetingAssign.EndTime.ToString("yyyy-MM-dd");
            DL_EndHour.SelectedValue = meetingAssign.EndTime.Hour.ToString();
            DL_EndMinute.SelectedValue = meetingAssign.EndTime.Minute.ToString();

            DL_Status.SelectedValue = meetingAssign.Status.Trim();
            TB_MainPass.Text = meetingAssign.MainPass;

            string strApplyFormID = meetingAssign.MeetingApplyID.ToString();

            LoadMeetingAttendant(strApplyFormID);

            HL_MakeCollaboration.Enabled = true;
            HL_MakeCollaboration.NavigateUrl = "TTMakeCollaboration.aspx?RelatedType=MEETING&RelatedID=" + strApplyFormID;

            HL_MeetingToTask.Enabled = true;
            HL_MeetingToTask.NavigateUrl = "TTMeetingToTask.aspx?ProjectID=1&RelatedType=MEETING&MeetingID=" + strApplyFormID;

            HL_RelatedDoc.NavigateUrl = "TTMeetingDoc.aspx?RelatedID=" + strApplyFormID;

            TB_Message.Text = LanguageHandle.GetWord("HuiYiTongZhi") + strID + " " + meetingAssign.Name.Trim() + "," + meetingAssign.Address.Trim() + LanguageHandle.GetWord("ZhuChiRen") + meetingAssign.Host.Trim() + LanguageHandle.GetWord("ShaoJiRen") + meetingAssign.Organizer.Trim() + LanguageHandle.GetWord("QingZhunShiCanJia");

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
            BT_Send.Enabled = true;

            HL_RelatedDoc.Enabled = true;
            HL_MakeCollaboration.Enabled = true;


            LoadRelatedWL("MeetingRequest", "Other", int.Parse(strApplyFormID));

            intWLNumber = GetRelatedWorkFlowNumber("MeetingRequest", "Other", strApplyFormID);

            //if (intWLNumber > 0)
            //{
            //    BT_Update.Enabled = false;
            //    BT_Delete.Enabled = false;

            //}
        }
    }

    protected void DataGrid1_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid1.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        MeetingAssignBLL meetingAssignBLL = new MeetingAssignBLL();
        IList lst = meetingAssignBLL.GetAllMeetingAssigns(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }




    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode, strReceiverCode;
        string strSubject, strMsg;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        string strMeetingID = NB_ApplyFormID.Amount.ToString();//LB_ID.Text.Trim();

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

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZHYTZFSWB") + "')", true);
    }

    protected void LoadMeetingAssignForm(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From MeetingAssign as meetingAssign Where meetingAssign.BuilderCode = " + "'" + strUserCode + "'" + " Order By meetingAssign.ID DESC";
        MeetingAssignBLL meetingAssignBLL = new MeetingAssignBLL();
        lst = meetingAssignBLL.GetAllMeetingAssigns(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
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

    protected string GetWorkFlowStatus(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        WorkFlow workFlow = (WorkFlow)lst[0];

        return workFlow.Status.Trim();
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

    protected int GetRelatedWorkFlowNumber(string strWLType, string strRelatedType, string strRelatedID)
    {
        string strHQL;
        IList lst;

        strHQL = "from WorkFlow as workFlow where workFlow.WLType = " + "'" + strWLType + "'" + " and workFlow.RelatedType = " + "'" + strRelatedType + "'" + " and workFlow.RelatedID = " + strRelatedID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);

        return lst.Count;
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

        DL_BeginMinute.DataSource = m;
        DL_BeginMinute.DataBind();
        DL_BeginMinute.Text = System.DateTime.Now.Minute.ToString();


        DL_EndHour.DataSource = hour;
        DL_EndHour.DataBind();
        DL_EndHour.Text = System.DateTime.Now.Hour.ToString();

        DL_EndMinute.DataSource = m;
        DL_EndMinute.DataBind();
        DL_EndMinute.Text = System.DateTime.Now.Minute.ToString();

    }

    protected void DL_Room_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strMeetingRoom = DL_Room.Text.Trim();

        TB_MeetingRoom.Text = strMeetingRoom;
    }


    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

            string strHQL, strID;
            IList lst;

            //strID = LB_ID.Text.Trim();
            strID = NB_ApplyFormID.Amount.ToString();
            strHQL = "from MeetingAttendant as meetingAttendant where meetingAttendant.MeetingID = " + strID + " and meetingAttendant.UserName = " + "'" + strUserName + "'";
            MeetingAttendantBLL meetingAttendantBLL = new MeetingAttendantBLL();
            lst = meetingAttendantBLL.GetAllMeetingAttendants(strHQL);

            MeetingAttendant meetingAttendant = (MeetingAttendant)lst[0];

            meetingAttendantBLL.DeleteMeetingAttendant(meetingAttendant);

            LoadMeetingAttendant(strID);
        }
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

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim(); ;
        MeetingBLL meetingBLL = new MeetingBLL();
        IList lst = meetingBLL.GetAllMeetings(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
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

    protected void LoadMeetingList(string strUserCode)
    {
        string strHQL = "from Meeting as meeting where meeting.BuilderCode = '" + strUserCode + "' and Status = 'Passed' Order by meeting.ID DESC";
        MeetingBLL meetingBLL = new MeetingBLL();
        IList lst = meetingBLL.GetAllMeetings(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();

        LB_Sql.Text = strHQL;
    }

    private void GetMeetCount()
    {
        string strMeetingCountHQL = "select * from T_MeetingSystemURL";
        DataTable dtMeetingCount = ShareClass.GetDataSetFromSql(strMeetingCountHQL, "Count").Tables[0];
        if (dtMeetingCount != null && dtMeetingCount.Rows.Count > 0)
        {
            LT_MeetingCount.Text = ShareClass.ObjectToString(dtMeetingCount.Rows[0]["MeetingCount"]);
        }
    }


    /// <summary>
    /// 查看还剩多少个视频位置
    /// </summary>
    /// <param name="strBeginTime"></param>
    /// <param name="strEndTime"></param>
    /// <returns></returns>
    private int CheckCurrentTimeMeetingNumber(DateTime strBeginTime, DateTime strEndTime)
    {
        int intResult = 0;
        //把输入的最小时间与数据库最大的时间比较，如果最小时间比数据库最大时间大，刚不在区间内
        //把输入的最大时间与数据库最小的时间比较，如果最大时间比数据库最小时间小，刚不在区间内
        string strMeetHQL = string.Format(@"
                        select COUNT(1) as MeentCount from
                        (
                        select * from T_MeetingAttendant
                        where MeetingID in
                        (
                            select ID from T_MeetingAssign
                            where BeginTime < '{0}'
                            and EndTime > '{1}'
                        )
                        ) t", strEndTime, strBeginTime);
        DataTable dtMeet = ShareClass.GetDataSetFromSql(strMeetHQL, "strMeetHQL").Tables[0];
        if (dtMeet != null && dtMeet.Rows.Count > 0)
        {
            string strResult = string.Empty;
            strResult = ShareClass.ObjectToString(dtMeet.Rows[0]["MeentCount"]);//dtMeet.Rows[0]["MeentCount"] == DBNull.Value ? "0" : dtMeet.Rows[0]["MeentCount"].ToString();
            int.TryParse(strResult, out intResult);
        }
        return intResult;
    }


    /// <summary>
    /// 调用视频会议接口
    /// </summary>
    /// <param name="strMeetingName">会议名称</param>
    /// <param name="strCreateUserCode">创建会议的用户ID</param>
    /// <param name="dtBeginTime">会议开始时间</param>
    /// <param name="dtEndTime">会议结束时间</param>
    /// <param name="strMainPass">会议的主席密码</param>
    /// <param name="strCreatePass">创建会议的用户密码</param>
    /// <param name="strJoinMember">参会人员，用户名称,用户密码|用户名称,用户密码|用户名称,用户密码 (多个用户参数时 加“|”)</param>
    /// <returns></returns>
    private string GetThreeMeetingMethod(string strMeetingName, string strCreateUserCode, DateTime dtBeginTime, DateTime dtEndTime, string strMainPass, string strCreatePass, string strJoinMember)
    {
        string strResult = string.Empty;

        string strMeetingUrl = string.Empty;// ConfigurationManager.AppSettings["MeetingSystemURL"].ToString();
        string strMeetingSystemHQL = "select MeetingSystemURL from T_MeetingSystemURL";
        DataTable dtMeetingSystem = ShareClass.GetDataSetFromSql(strMeetingSystemHQL, "strMeetingSystemHQL").Tables[0];
        if (dtMeetingSystem != null && dtMeetingSystem.Rows.Count > 0)
        {
            strMeetingUrl = ShareClass.ObjectToString(dtMeetingSystem.Rows[0]["MeetingSystemURL"]); //dtMeetingSystem.Rows[0]["MeetingSystemURL"].ToString();
        }

        Encoding encoding = Encoding.Default;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strMeetingUrl);
        string encrypt = string.Empty;
        long ticks = DateTime.Now.Ticks;

        request.Method = "post";
        request.Accept = "text/html, application/xhtml+xml, */*";
        request.ContentType = "application/x-www-form-urlencoded";

        string postString = string.Empty;
        if (!string.IsNullOrEmpty(strJoinMember) && strJoinMember.EndsWith("|"))
        {
            strJoinMember = strJoinMember.TrimEnd('|');
        }
        //postString = string.Format("createmeeting={0}&createuser={1}", "Test1|1|2013-06-19%2004:44:52|2013-06-20%2004:44:59|1212|admin", "10,10|11,11|12,12");
        postString = string.Format("createmeeting={0}&createuser={1}", Server.UrlEncode(strMeetingName) + "|" + strCreateUserCode + "|" + dtBeginTime.ToString("yyyy-MM-dd hh:mm:ss") + "|" + dtEndTime.ToString("yyyy-MM-dd hh:mm:ss") + "|" + strMainPass + "|" + strCreatePass, strJoinMember);

        byte[] buffer = encoding.GetBytes(postString);
        request.ContentLength = buffer.Length;

        try
        {
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8"));
            string responseMsg = reader.ReadToEnd();

            System.Runtime.Serialization.Json.DataContractJsonSerializer json = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(DataJson));
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(responseMsg)))
            {
                DataJson dataJson = (DataJson)json.ReadObject(stream);
                if (dataJson.status != 2000)
                {
                    //写错误日志
                    if (dataJson.status == 4002)
                    {
                        strResult = LanguageHandle.GetWord("ChuangJianYongHuDeYongHuMingHu");
                    }
                    else if (dataJson.status == 4003)
                    {
                        strResult = LanguageHandle.GetWord("HuiYiMingChenBuYunHuWeiKong");
                    }
                    else if (dataJson.status == 4004)
                    {
                        strResult = LanguageHandle.GetWord("ShiJianGeShiCuoWu");
                    }
                    else if (dataJson.status == 4005)
                    {
                        strResult = LanguageHandle.GetWord("ShiJianSheZhiYouWu");
                    }
                    else if (dataJson.status == 4006)
                    {
                        strResult = LanguageHandle.GetWord("ZhuXiMiMaGeShiCuoWu");
                    }
                    else if (dataJson.status == 4007)
                    {
                        strResult = LanguageHandle.GetWord("HuiYiBuCunZai");
                    }
                    else if (dataJson.status == 4008)
                    {
                        strResult = LanguageHandle.GetWord("HuiYiMiMaCuoWu");
                    }
                    else if (dataJson.status == 4009)
                    {
                        strResult = LanguageHandle.GetWord("PuTongYongHuMiMaGeShiCuoWu");
                    }
                    else
                    {
                        strResult = "fail";
                    }
                }
                else
                {
                    //正确
                    strResult = "succ";
                }
            }



        }
        catch (Exception ex)
        {
            strResult = LanguageHandle.GetWord("DiaoYongChuCuoQingJianCha");
        }
        return strResult;
    }

    protected void BT_SelectMeeting_Click(object sender, EventArgs e)
    {
        DateTime dtBeginTime = DateTime.Parse(DateTime.Parse(DLC_BeginTime.Text).ToString("yyyy/MM/dd ") + DL_BeginHour.SelectedValue + ":" + DL_BeginMinute.SelectedValue);
        DateTime dtEndTime = DateTime.Parse(DateTime.Parse(DLC_EndTime.Text).ToString("yyyy/MM/dd ") + DL_EndHour.SelectedValue + ":" + DL_EndMinute.SelectedValue);

        int count = CheckCurrentTimeMeetingNumber(dtBeginTime, dtEndTime);
        //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZDTBEGINTIMEDDTENDTIMEYYCOUNTGD")+"')", true);
    }


    //取得用户创建的会议最大会议号
    public static string GetMyCreatedMaxMeetAssignID(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from MeetingAssign as meetingAssign where meetingAssign.BuilderCode = " + "'" + strUserCode + "'" + " Order by meetingAssign.ID DESC";
        MeetingAssignBLL meetingAssignBLL = new MeetingAssignBLL();
        lst = meetingAssignBLL.GetAllMeetingAssigns(strHQL);

        MeetingAssign meetingAssign = (MeetingAssign)lst[0];

        return meetingAssign.ID.ToString();
    }


    public static string PassWordAfterStr(string plaintext)
    {
        byte[] srcBuffer = System.Text.Encoding.UTF8.GetBytes(plaintext);

        HashAlgorithm hash = HashAlgorithm.Create("SHA1"); //将参数换成“MD5”，则执行 MD5 加密。不区分大小写。
        byte[] destBuffer = hash.ComputeHash(srcBuffer);

        string hashedText = BitConverter.ToString(destBuffer).Replace("-", "");
        return hashedText.ToLower();
    }
}


[System.Runtime.Serialization.DataContract(Namespace = "http://www.mzwu.com/")]
public class DataJson
{
    private int _status { get; set; }
    private string _serial { get; set; }

    public DataJson(int status, string serial)
    {
        _status = status;
        _serial = serial;
    }


    [System.Runtime.Serialization.DataMember]
    public int status
    {
        set { _status = value; }
        get { return _status; }
    }


    [System.Runtime.Serialization.DataMember]
    public string serial
    {
        set { _serial = value; }
        get { return _serial; }
    }

}