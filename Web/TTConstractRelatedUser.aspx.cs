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

using System.Text;
using System.IO;
using System.Web.Mail;

using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTConstractRelatedUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName;
        string strProjectID, strProjectName;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        strUserName = Session["UserName"].ToString();

        strProjectID = Request.QueryString["ProjectID"];
        LB_ProjectID.Text = strProjectID;
        strProjectName = GetProjectName(strProjectID);

        strProjectName = GetProjectName(strProjectID);

        //this.Title = LanguageHandle.GetWord("Project").ToString().Trim() + strProjectID + " " + strProjectName + "的项目成员";

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            DLC_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "from Department as department";
            DepartmentBLL departmentBLL = new DepartmentBLL();
            lst = departmentBLL.GetAllDepartments(strHQL);

            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();

            strHQL = "from ProjectMember as projectMember where projectMember.UserCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ")";

            ProjectMemberBLL projectMember = new ProjectMemberBLL();
            lst = projectMember.GetAllProjectMembers(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();

            strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID  = " + strProjectID;
            RelatedUserBLL relatedUerBLL = new RelatedUserBLL();
            lst = relatedUerBLL.GetAllRelatedUsers(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_SqlGM.Text = strHQL;

            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();

            strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName not in ('个人','部门','公司')"; 
            lst = actorGroupBLL.GetAllActorGroups(strHQL);

            Repeater1.DataSource = lst;
            Repeater1.DataBind();

            TB_SMS.Text = strUserName.Trim() + LanguageHandle.GetWord("BaNiJiaRuLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();
            TB_PersonalSMS.Text = strUserName.Trim() + LanguageHandle.GetWord("BaNiJiaRuLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strID = ((Button)e.Item.FindControl("BT_ID")).Text.ToString();
            string strUserCode = e.Item.Cells[0].Text.Trim();
            string strProjectID = LB_ProjectID.Text.Trim();

            for (int i = 0; i < DataGrid2.Items.Count; i++)
            {
                DataGrid2.Items[i].ForeColor = Color.Black;
            }

            e.Item.ForeColor = Color.Red;

            string strHQL = "from RelatedUser as relatedUser where relatedUser.ID = " + strID;

            RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
            IList lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
            RelatedUser relatedUser = (RelatedUser)lst[0];

            LB_ID.Visible = true;

            LB_ID.Text = relatedUser.ID.ToString();
            LB_RelatedUserCode.Text = relatedUser.UserCode.Trim();
            LB_RelatedUserName.Text = relatedUser.UserName.Trim();
            LB_DepartCode.Text = relatedUser.DepartCode.Trim();
            LB_DepartName.Text = GetDepartName(relatedUser.DepartCode.Trim());
            TB_Actor.Text = relatedUser.Actor.Trim();
            DLC_JoinDate.Text = relatedUser.JoinDate.ToString("yyyy-MM-dd");
            TB_WorkDetail.Text = relatedUser.WorkDetail;
            NB_UnitHourSalary.Amount = relatedUser.UnitHourSalary;
            DL_Status.SelectedValue = relatedUser.Status.Trim();

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
            BT_SendMsg.Enabled = true;
            BT_PersonalSMS.Enabled = true;
        }
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_SqlGM.Text;

        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strDepartCode = ((Button)e.Item.FindControl("BT_DepartCode")).Text.Trim();

            string strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";

            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();
        string strProjectID = LB_ProjectID.Text.Trim();

        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        LB_RelatedUserCode.Text = strUserCode;
        LB_RelatedUserName.Text = strUserName;
        LB_DepartCode.Text = projectMember.DepartCode;
        LB_DepartName.Text = GetDepartName(projectMember.DepartCode.Trim());

        BT_New.Enabled = true;
        BT_Update.Enabled = false;
        BT_Delete.Enabled = false;
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strProjectID = LB_ProjectID.Text.Trim();
        string strProjectName = GetProjectName(strProjectID);
        string strUserCode = LB_RelatedUserCode.Text.Trim();
        string strUserName = LB_RelatedUserName.Text.Trim();
        string strDepartCode = LB_DepartCode.Text.Trim();
        string strDepartName = GetDepartName(strDepartCode);
        string strActor = TB_Actor.Text.Trim();
        string strJoinDate = DLC_JoinDate.Text;
        string strWorkDetail = TB_WorkDetail.Text.Trim();
        decimal deHourSalary = NB_UnitHourSalary.Amount;

        if (strActor != "")
        {
            try
            {
                RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
                RelatedUser relatedUser = new RelatedUser();

                relatedUser.ProjectID = int.Parse(strProjectID);
                relatedUser.ProjectName = strProjectName;
                relatedUser.UserCode = strUserCode;
                relatedUser.UserName = strUserName;
                relatedUser.DepartCode = strDepartCode;
                relatedUser.DepartName = GetDepartName(strDepartCode);
                relatedUser.Actor = strActor;
                relatedUser.JoinDate = DateTime.Parse(strJoinDate);
                relatedUser.Status = DL_Status.SelectedValue.Trim();
                relatedUser.WorkDetail = strWorkDetail;
                relatedUser.SMSCount = 0;
                relatedUser.UnitHourSalary = deHourSalary;

                relatedUserBLL.AddRelatedUser(relatedUser);

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;

                string strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID;
                relatedUserBLL = new RelatedUserBLL();
                IList lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
                DataGrid2.DataSource = lst;
                DataGrid2.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCCJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJSBNWKJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid3_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = "from Department as department";

        DepartmentBLL departmentBLL = new DepartmentBLL();
        IList lst = departmentBLL.GetAllDepartments(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        if (LB_ID.Text != "")
        {
            string strProjectID = LB_ProjectID.Text.Trim();
            string strID = LB_ID.Text.Trim();
            string strUserCode = LB_RelatedUserCode.Text.Trim();
            string strUserName = LB_RelatedUserName.Text.Trim();
            string strDepartCode = LB_DepartCode.Text.Trim();
            string strDepartName = GetDepartName(strDepartCode);
            string strActor = TB_Actor.Text.Trim();
            string strWorkDetail = TB_WorkDetail.Text.Trim();
            DateTime dtJoinDate = DateTime.Parse(DLC_JoinDate.Text);
            decimal deUnitHourSalary = NB_UnitHourSalary.Amount;
            string strStatus = DL_Status.SelectedValue.Trim();

            if (strActor != "")
            {
                string strHQL = "from RelatedUser as relatedUser where relatedUser.ID = " + strID;
                RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
                IList lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
                RelatedUser relatedUser = (RelatedUser)lst[0];

                relatedUser.UserCode = strUserCode;
                relatedUser.UserName = strUserName;
                relatedUser.DepartCode = strDepartCode;
                relatedUser.DepartName = strDepartName;
                relatedUser.ProjectID = int.Parse(strProjectID);
                relatedUser.ProjectName = GetProjectName(strProjectID);
                relatedUser.Actor = strActor;
                relatedUser.WorkDetail = strWorkDetail;
                relatedUser.JoinDate = dtJoinDate;
                relatedUser.UnitHourSalary = deUnitHourSalary;
                relatedUser.Status = strStatus;

                try
                {
                    relatedUserBLL.UpdateRelatedUser(relatedUser, int.Parse(strID));

                    strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID  = " + strProjectID;
                    relatedUserBLL = new RelatedUserBLL();
                    lst = relatedUserBLL.GetAllRelatedUsers(strHQL);

                    DataGrid2.DataSource = lst;
                    DataGrid2.DataBind();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJSBNWKJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXJLBNXGJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        if (LB_ID.Text != "")
        {
            string strProjectID = LB_ProjectID.Text.Trim();
            string strID = LB_ID.Text.Trim();
            string strUserCode = LB_RelatedUserCode.Text.Trim();
            string strUserName = LB_RelatedUserName.Text.Trim();
            string strDepartCode = LB_DepartCode.Text.Trim();
            string strDepartName = GetDepartName(strDepartCode);
            string strActor = TB_Actor.Text.Trim();
            string strWorkDetail = TB_WorkDetail.Text.Trim();
            DateTime dtJoinDate = DateTime.Parse(DLC_JoinDate.Text);

            string strHQL = "from RelatedUser as relatedUser where relatedUser.ID = " + strID;
            RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
            IList lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
            RelatedUser relatedUser = (RelatedUser)lst[0];

            try
            {
                relatedUserBLL.DeleteRelatedUser(relatedUser);

                //删除此项目触色组的相关成员
                strHQL = "  delete from T_ActorGroupDetail  where UserCode = " + "'" + strUserCode + "'" + " and GroupName";
                strHQL += "  in (Select ActorGroupName from T_ProjectRelatedActorGroup where ProjectID = " + strProjectID + ")";

                ShareClass.RunSqlCommand(strHQL);


                strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID  = " + strProjectID;
                relatedUserBLL = new RelatedUserBLL();
                lst = relatedUserBLL.GetAllRelatedUsers(strHQL);

                DataGrid2.DataSource = lst;
                DataGrid2.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXJLBNSCJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_CreateActorGroup_Click(object sender, EventArgs e)
    {
        string strHQL, strGroupName;
        string strUserCode;
        IList lst;

        strGroupName = TB_ActorGroup.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        if (strGroupName != "")
        {
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            ActorGroup actorGroup = new ActorGroup();

            strHQL = "from ActorGroup as actorGroup";
            lst = actorGroupBLL.GetAllActorGroups(strHQL);

            actorGroup.GroupName = strGroupName;
            actorGroup.MakeUserCode = strUserCode;
            actorGroup.SortNumber = lst.Count + 1;

            try
            {
                actorGroupBLL.AddActorGroup(actorGroup);

                strHQL = "from ActorGroup as actorGroup";
                lst = actorGroupBLL.GetAllActorGroups(strHQL);

                Repeater1.DataSource = lst;
                Repeater1.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJSZZMBNWK").ToString().Trim() + "')", true);
        }
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strProjectID, strGroupName;
            string strHQL;
            string strUserCode, strActor;
            IList lst, lst1;
            string strAction;
            int i = 0;

            strProjectID = LB_ProjectID.Text.Trim();

            strGroupName = ((Button)e.Item.FindControl("BT_GroupName")).Text.Trim();
            DataGrid2.CurrentPageIndex = 0;

            strAction = LB_AddDelete.Text.Trim();

            RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
            RelatedUser relatedUser = new RelatedUser();

            ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
            ActorGroupDetail actorGroupDetail = new ActorGroupDetail();

            if (strAction == "Add")
            {
                strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strGroupName + "'";
                lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

                for (i = 0; i < lst.Count; i++)
                {
                    actorGroupDetail = (ActorGroupDetail)lst[i];

                    strUserCode = actorGroupDetail.UserCode.Trim();
                    strActor = actorGroupDetail.Actor.Trim();
                    strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and  relatedUser.UserCode = " + "'" + strUserCode + "'" + " and relatedUser.Actor = " + "'" + strActor + "'";
                    lst1 = relatedUserBLL.GetAllRelatedUsers(strHQL);

                    if (lst1.Count == 0)
                    {
                        relatedUser.ProjectID = int.Parse(strProjectID);
                        relatedUser.ProjectName = GetProjectName(strProjectID);
                        relatedUser.UserCode = actorGroupDetail.UserCode;
                        relatedUser.UserName = actorGroupDetail.UserName;
                        relatedUser.DepartCode = actorGroupDetail.DepartCode;
                        relatedUser.DepartName = actorGroupDetail.DepartName;
                        relatedUser.Actor = actorGroupDetail.Actor;
                        relatedUser.WorkDetail = actorGroupDetail.WorkDetail;
                        relatedUser.JoinDate = DateTime.Now;
                        relatedUser.ActorGroup = strGroupName;
                        relatedUser.UnitHourSalary = 0;
                        relatedUser.Status = "Plan";

                        relatedUserBLL.AddRelatedUser(relatedUser);
                    }
                }

                LB_AddDelete.Text = "Delete";
            }
            else
            {
                strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID + " and relatedUser.ActorGroup = " + "'" + strGroupName + "'";
                lst = relatedUserBLL.GetAllRelatedUsers(strHQL);

                for (i = 0; i < lst.Count; i++)
                {
                    relatedUser = (RelatedUser)lst[i];
                    strUserCode = relatedUser.UserCode.Trim();

                    try
                    {
                        relatedUserBLL.DeleteRelatedUser(relatedUser);

                        //删除此项目触色组的相关成员
                        strHQL = "  delete from T_ActorGroupDetail  where UserCode = " + "'" + strUserCode + "'" + " and GroupName";
                        strHQL += "  in (Select ActorGroupName from T_ProjectRelatedActorGroup where ProjectID = " + strProjectID + ")";

                        ShareClass.RunSqlCommand(strHQL);
                    }
                    catch
                    {
                    }
                }

                LB_AddDelete.Text = "Add";
            }

            strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID  = " + strProjectID;
            lst = relatedUserBLL.GetAllRelatedUsers(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_ActorGroup.Text = strGroupName;
            BT_TransferActorGroup.Enabled = true;
        }
    }


    protected void BT_TransferActorGroup_Click(object sender, EventArgs e)
    {
        IList lst1, lst2;

        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        ActorGroupDetail actorGroupDetail = new ActorGroupDetail();

        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        RelatedUser relatedUser = new RelatedUser();

        string strProjectID = LB_ProjectID.Text.Trim();
        string strGroupName = LB_ActorGroup.Text.Trim();

        string strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID;
        lst1 = relatedUserBLL.GetAllRelatedUsers(strHQL);

        try
        {
            for (int i = 0; i < lst1.Count; i++)
            {
                relatedUser = (RelatedUser)lst1[i];

                strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strGroupName + "'" + " and actorGroupDetail.UserCode = " + "'" + relatedUser.UserCode.Trim() + "'";
                lst2 = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

                if (lst2.Count == 0)
                {
                    actorGroupDetail.GroupName = strGroupName;
                    actorGroupDetail.UserCode = relatedUser.UserCode;
                    actorGroupDetail.UserName = relatedUser.UserName;
                    actorGroupDetail.DepartCode = relatedUser.DepartCode;
                    actorGroupDetail.DepartName = relatedUser.DepartName;
                    actorGroupDetail.Actor = relatedUser.Actor;
                    actorGroupDetail.WorkDetail = relatedUser.WorkDetail;

                    actorGroupDetailBLL.AddActorGroupDetail(actorGroupDetail);
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYWCCYZR").ToString().Trim() + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZRSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_SendMsg_Click(object sender, EventArgs e)
    {
        string strProjectID, strHQL;
        string strMobilePhone, strUserCode, strUserName, strRelatedUserCode, strRelatedUserName;
        string strPMName;
        IList lst1, lst2;
        string strProject, strMsg, strSubject;
        int intSMSCount, intID;

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = GetUserName(strUserCode);

        strPMName = LB_UserName.Text.Trim();
        strProjectID = LB_ProjectID.Text.Trim();
        strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID;
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        lst1 = relatedUserBLL.GetAllRelatedUsers(strHQL);

        RelatedUser relatedUser = new RelatedUser();

        strMsg = TB_SMS.Text.Trim();
        strSubject = LanguageHandle.GetWord("JiaRuXiangMuTongZhi").ToString().Trim();

        Msg msg = new Msg();

        try
        {
            for (int i = 0; i < lst1.Count; i++)
            {
                relatedUser = (RelatedUser)lst1[i];

                strRelatedUserCode = relatedUser.UserCode.Trim();
                strRelatedUserName = relatedUser.UserName.Trim();
                strProject = relatedUser.ProjectName.Trim();
                intID = relatedUser.ID;
                intSMSCount = relatedUser.SMSCount;

                if (CB_Sms.Checked == true | CB_Mail.Checked == true)
                {
                    if (CB_Sms.Checked == true)
                    {
                        msg.SendMSM("Message", strRelatedUserCode, strMsg, strUserCode);
                    }

                    if (CB_Mail.Checked == true)
                    {
                        msg.SendMail(strRelatedUserCode, strSubject, strMsg, strUserCode);
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXFSCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXFSSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_PersonalSMS_Click(object sender, EventArgs e)
    {
        string strProjectID, strHQL;
        string strRelatedUserCode, strRelatedUserName;
        string strMobilePhone, strUserCode, strUserName;
        string strPMName;
        IList lst1, lst2;
        string strProject, strMsg, strSubject;
        int intSMSCount, intID;

        Msg msg = new Msg();

        strMsg = TB_PersonalSMS.Text.Trim();

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        strRelatedUserCode = LB_RelatedUserCode.Text.Trim();
        strRelatedUserName = LB_RelatedUserName.Text.Trim();

        try
        {
            if (CB_SendSMS.Checked == true | CB_SendEMail.Checked == true)
            {
                if (CB_SendSMS.Checked == true)
                {
                    msg.SendMSM("Message", strRelatedUserCode, strMsg, strUserCode);
                }

                if (CB_SendEMail.Checked == true)
                {

                    strSubject = LanguageHandle.GetWord("XiangMuChengYuanJiaRuTongZhi").ToString().Trim();
                    msg.SendMail(strRelatedUserCode, strSubject, strMsg, strUserCode);
                }

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXFSCG").ToString().Trim() + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXXFSSBJC").ToString().Trim() + "')", true);
        }
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

    protected string GetProjectStatus(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        string strStatus = project.Status.Trim();

        return strStatus;
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }

    protected string GetDepartName(string strDepartCode)
    {
        string strHQL = "from Department as department where department.DepartCode = " + "'" + strDepartCode + "'";
        DepartmentBLL departmentBLL = new DepartmentBLL();
        IList lst = departmentBLL.GetAllDepartments(strHQL);

        Department department = (Department)lst[0];

        return department.DepartName;
    }


}
