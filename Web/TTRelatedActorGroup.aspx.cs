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

public partial class TTRelatedActorGroup : System.Web.UI.Page
{
    string strRelatedType, strRelatedID, strRelatedName;
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName;
        string strUserType;

        string strHQL;
        IList lst;

        strRelatedType = Request.QueryString["RelatedType"].Trim();
        strRelatedID = Request.QueryString["RelatedID"].Trim();

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        strUserType = ShareClass.GetUserType(strUserCode);

        if (strRelatedType == "Project")
        {
            strRelatedName = GetProjectName(strRelatedID);
            //this.Title = "Project" + strRelatedID + " " + strRelatedName + " 角色组设置！";
        }

        if (strRelatedType == "ProjectTask")
        {
            strRelatedName = GetProjectTaskName(strRelatedID);
            //this.Title = "项目任务：" + strRelatedID + " " + strRelatedName + " 角色组设置！";
        }

        if (strRelatedType == "Req")
        {
            strRelatedName = GetRequirementName(strRelatedID);
            //this.Title = "需求：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "Meeting")
        {
            strRelatedName = GetMeetingName(strRelatedID);
            //this.Title = "会议：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        if (strRelatedType == "ProjectRisk")
        {
            strRelatedName = GetProjectRiskName(strRelatedID);
            //this.Title = "风险：" + strRelatedID + " " + strRelatedName + " 的工作流模板设置";
        }

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            if (strRelatedType == "Project")
            {
                TB_ActorGroup.Text = Resources.lang.Project + strRelatedID + "          组";
            }

            if (strRelatedType == "ProjectTask")
            {
                TB_ActorGroup.Text = "项目任务：" + strRelatedID + "        组";
            }

            if (strRelatedType == "Req")
            {
                TB_ActorGroup.Text = "需求：" + strRelatedID + "        组";
            }

            if (strRelatedType == "会议")
            {
                TB_ActorGroup.Text = "会议：" + strRelatedID + "        组";
            }

            if (strRelatedType == "ProjectRisk")
            {
                TB_ActorGroup.Text = "风险：" + strRelatedID + "        组";
            }

            if (strUserType == "INNER")
            {
                TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(Resources.lang.ZZJGT,TreeView1, strUserCode);
            }
            else
            {
                TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(Resources.lang.ZZJGT,TreeView1, strUserCode);
            }

            if (strRelatedType == "Project")
            {
                strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ProjectID = " + strRelatedID;
                ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
                lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

                DataGrid4.DataSource = lst;
                DataGrid4.DataBind();
            }

            if (strRelatedType == "ProjectTask")
            {
                strHQL = "from ProRelatedUser as proRelatedUser where ";
                strHQL += " proRelatedUser.ProjectID in (Select projectTask.ProjectID from ProjectTask as projectTask where projectTask.TaskID = " + strRelatedID + ")";

                ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
                lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

                DataGrid4.DataSource = lst;
                DataGrid4.DataBind();
            }

            if (strRelatedType == "ProjectRisk")
            {
                strHQL = "from ProRelatedUser as proRelatedUser where ";
                strHQL += " proRelatedUser.ProjectID in (Select projectRisk.ProjectID from ProjectRisk as projectRisk where projectRisk.ID = " + strRelatedID + ")";

                ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
                lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

                DataGrid4.DataSource = lst;
                DataGrid4.DataBind();
            }

            if (strRelatedType == "Req")
            {
                strHQL = "select Distinct OperatorCode as UserCode,OperatorName as UserName from T_ReqAssignRecord where ReqID = " + strRelatedID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");

                DataGrid4.DataSource = ds;
                DataGrid4.DataBind();
            }

            LoadRelatedActorGroup(strRelatedType, strRelatedID,strLangCode);

            BT_New.Enabled = false;
      

            LB_RelatedID.Text = strRelatedID;
        }
    }

    protected void BT_CreateActorGroup_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strGroupName, strUserCode, strRelatedID, strDepartCode, strDepartName;
        string strType;

        strGroupName = TB_ActorGroup.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        strType = "Part";
        strRelatedID = LB_RelatedID.Text.Trim();

        if (strGroupName != "")
        {
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            ActorGroup actorGroup = new ActorGroup();

            strHQL = "from ActorGroup as actorGroup Where actorGroup.GroupName = " + "'" + strGroupName + "'";
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            if (lst.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZCZTMDJSZQJC + "')", true);
                return;
            }

            actorGroup.GroupName = strGroupName;
            actorGroup.MakeUserCode = strUserCode;
            actorGroup.SortNumber = lst.Count + 1;
            actorGroup.Type = strType;
            actorGroup.IdentifyString = DateTime.Now.ToString("yyyyMMddHHMMssff");
            actorGroup.BelongDepartCode = strDepartCode;
            actorGroup.BelongDepartName = strDepartName;
            actorGroup.HomeName = strGroupName;
            actorGroup.LangCode = strLangCode;
            actorGroup.MakeType = "DIY";
            actorGroup.SortNumber = 1;

            try
            {
                actorGroupBLL.AddActorGroup(actorGroup);

                RelatedActorGroupBLL relatedActorGroupBLL = new RelatedActorGroupBLL();
                RelatedActorGroup relatedActorGroup = new RelatedActorGroup();
                relatedActorGroup.RelatedType = strRelatedType;
                relatedActorGroup.RelatedID = int.Parse(strRelatedID);
                relatedActorGroup.ActorGroupName = strGroupName;

                try
                {
                    relatedActorGroupBLL.AddRelatedActorGroup(relatedActorGroup);
                    LoadRelatedActorGroup(strRelatedType, strRelatedID, strLangCode);
                }
                catch
                {
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZXZSBJC + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJSZZMBNWK + "')", true);
        }
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strGroupName, strMakeUserCode, strUserCode;

            DataGrid2.CurrentPageIndex = 0;

            for (int i = 0; i < Repeater1.Items.Count; i++)
            {
                ((Button)Repeater1.Items[i].FindControl("BT_GroupName")).ForeColor = Color.White;
            }
            ((Button)e.Item.FindControl("BT_GroupName")).ForeColor = Color.Red;


            strGroupName = ((Button)e.Item.FindControl("BT_GroupName")).Text.Trim();
            strMakeUserCode = GetMakeUserCode(strGroupName);
            strUserCode = LB_UserCode.Text.Trim();

            LoadActorGroupDetail(strGroupName);

            LB_ActorGroupName.Text = strGroupName;
            LB_ActorGroup.Text = strGroupName;

            string strRelatedUserCode = LB_RelatedUserCode.Text.Trim();

            if (strUserCode != strMakeUserCode | strGroupName == "个人" | strGroupName == "全体" | strGroupName == "公司" | strGroupName == "集团")
            {
                BT_New.Enabled = false;
            
                BT_DeleteActorGroup.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
             
                BT_DeleteActorGroup.Enabled = true;
            }

        }
    }

    protected void BT_DeleteActorGroup_Click(object sender, EventArgs e)
    {
        string strHQL, strGroupName;

        strGroupName = LB_ActorGroup.Text.Trim();
        strHQL = "Delete From T_ActorGroup Where GroupName = " + "'" + strGroupName + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
            LoadRelatedActorGroup(strRelatedType, strRelatedID, strLangCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZSCJSZCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZJGSCSBJC + "')", true);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid4);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
    }

    protected void BT_AllRelatedMember_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        if (strRelatedType == "Project")
        {
            strHQL = "from ProRelatedUser as proRelatedUser where proRelatedUser.ProjectID = " + strRelatedID;
            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();
        }

        if (strRelatedType == "ProjectTask")
        {
            strHQL = "from ProRelatedUser as proRelatedUser where ";
            strHQL += " proRelatedUser.ProjectID in (Select projectTask.ProjectID from ProjectTask as projectTask where projectTask.TaskID = " + strRelatedID + ")";

            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();
        }

        if (strRelatedType == "ProjectRisk")
        {
            strHQL = "from ProRelatedUser as proRelatedUser where ";
            strHQL += " proRelatedUser.ProjectID in (Select projectRisk.ProjectID from ProjectRisk as projectRisk where projectRisk.ID = " + strRelatedID + ")";

            ProRelatedUserBLL proRelatedUserBLL = new ProRelatedUserBLL();
            lst = proRelatedUserBLL.GetAllProRelatedUsers(strHQL);

            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();
        }

        if (strRelatedType == "Req")
        {
            strHQL = "select Distinct OperatorCode as UserCode,OperatorName as UserName from T_ReqAssignRecord where ReqID = " + strRelatedID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ReqAssignRecord");

            DataGrid4.DataSource = ds;
            DataGrid4.DataBind();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strGroupID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                string strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupID = " + strGroupID;
                ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
                IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

                ActorGroupDetail actorGroupDetail = (ActorGroupDetail)lst[0];

                LB_ID.Visible = true;
                LB_ID.Text = actorGroupDetail.GroupID.ToString();
                LB_RelatedUserCode.Text = actorGroupDetail.UserCode;
                LB_RelatedUserName.Text = actorGroupDetail.UserName;
                LB_DepartCode.Text = actorGroupDetail.DepartCode;
                TB_Actor.Text = actorGroupDetail.Actor;
                TB_WorkDetail.Text = actorGroupDetail.WorkDetail;

                string strGroupName = actorGroupDetail.GroupName.Trim();
                string strMakeUserCode = GetMakeUserCode(strGroupName);
                string strUserCode = LB_UserCode.Text.Trim();
                string strDepartCode = LB_DepartCode.Text.Trim();


                LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);


                if (strMakeUserCode != strUserCode)
                {
                    BT_New.Enabled = false;
               
                }
                else
                {
                    BT_New.Enabled = true;
                
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strGroupName = LB_ActorGroupName.Text.Trim();
            
                string strUserCode = LB_RelatedUserCode.Text.Trim();
                string strUserName = LB_RelatedUserName.Text.Trim();
                string strDepartCode = LB_DepartCode.Text.Trim();
                string strDepartName = ShareClass.GetDepartName(strDepartCode);
                string strActor = TB_Actor.Text.Trim();
                string strWorkDetail = TB_WorkDetail.Text.Trim();

                try
                {
                    ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
                    ActorGroupDetail actorGroupDetail = new ActorGroupDetail();

                    actorGroupDetail.GroupID = int.Parse(strGroupID);
                    actorGroupDetail.GroupName = strGroupName;
                    actorGroupDetail.UserCode = strUserCode;
                    actorGroupDetail.UserName = strUserName;
                    actorGroupDetail.DepartCode = strDepartCode;
                    actorGroupDetail.DepartName = strDepartName;
                    actorGroupDetail.Actor = strActor;
                    actorGroupDetail.WorkDetail = strWorkDetail;

                    actorGroupDetailBLL.DeleteActorGroupDetail(actorGroupDetail);

                    LoadActorGroupDetail(strGroupName);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('删除" + Resources.lang.ZZCCJC + "')", true);
                }
            }
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

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();
        string strGroupName = LB_ActorGroupName.Text.Trim();

        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];

        LB_RelatedUserCode.Text = strUserCode;
        LB_RelatedUserName.Text = strUserName;
        LB_DepartCode.Text = projectMember.DepartCode;
        LB_DepartName.Text = ShareClass.GetDepartName(projectMember.DepartCode.Trim());

        BT_New.Enabled = true;

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
            AddMember();
        }
        else
        {
            UpdateMember();
        }
    }

    protected void AddMember()
    {
        string strGroupID;
        string strGroupName = LB_ActorGroupName.Text.Trim();
        string strRelatedUserCode = LB_RelatedUserCode.Text.Trim();
        string strRelatedUserName = LB_RelatedUserName.Text.Trim();
        string strDepartCode = LB_DepartCode.Text.Trim();
        string strDepartName = ShareClass.GetDepartName(strDepartCode);
        string strActor = TB_Actor.Text.Trim();
        string strWorkDetail = TB_WorkDetail.Text.Trim();

        if (strActor != "")
        {
            try
            {
                ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
                ActorGroupDetail actorGroupDetail = new ActorGroupDetail();

                actorGroupDetail.GroupName = strGroupName;
                actorGroupDetail.UserCode = strRelatedUserCode;
                actorGroupDetail.UserName = strRelatedUserName;
                actorGroupDetail.DepartCode = strDepartCode;
                actorGroupDetail.DepartName = strDepartName;
                actorGroupDetail.Actor = strActor;
                actorGroupDetail.WorkDetail = strWorkDetail;

                actorGroupDetailBLL.AddActorGroupDetail(actorGroupDetail);

                strGroupID = ShareClass.GetMyCreatedMaxActorGroupDetailID(strGroupName);
                LB_ID.Text = strGroupID;
                
                LoadActorGroupDetail(strGroupName);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCSB + "')", true);
        }
    }

    protected void UpdateMember()
    {
        string strGroupName = LB_ActorGroupName.Text.Trim();
        string strID = LB_ID.Text.Trim();
        string strRelatedUserCode = LB_RelatedUserCode.Text.Trim();
        string strRelatedUserName = LB_RelatedUserName.Text.Trim();
        string strDepartCode = LB_DepartCode.Text.Trim();
        string strDepartName = ShareClass.GetDepartName(strDepartCode);
        string strActor = TB_Actor.Text.Trim();
        string strWorkDetail = TB_WorkDetail.Text.Trim();

        string strUserCode = LB_UserCode.Text.Trim();
        string strMakeUserCode = GetMakeUserCode(strGroupName);

        try
        {
            ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
            ActorGroupDetail actorGroupDetail = new ActorGroupDetail();

            actorGroupDetail.GroupName = strGroupName;
            actorGroupDetail.UserCode = strRelatedUserCode;
            actorGroupDetail.UserName = strRelatedUserName;
            actorGroupDetail.DepartCode = strDepartCode;
            actorGroupDetail.DepartName = strDepartName;
            actorGroupDetail.Actor = strActor;
            actorGroupDetail.WorkDetail = strWorkDetail;

            actorGroupDetailBLL.UpdateActorGroupDetail(actorGroupDetail, int.Parse(strID));

            LoadActorGroupDetail(strGroupName);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + Resources.lang.ZZBCCG + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('修改" + Resources.lang.ZZBCSB + "')", true);
        }
    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strGroupName, strMakeUserCode, strUserCode;

            strGroupName = ((Button)e.Item.FindControl("BT_GroupName")).Text.Trim();
            strMakeUserCode = GetMakeUserCode(strGroupName);
            strUserCode = LB_UserCode.Text.Trim();

            LoadActorGroupDetail(strGroupName);

            LB_ActorGroupName.Text = strGroupName;
            LB_ActorGroup.Text = strGroupName;

            string strRelatedUserCode = LB_RelatedUserCode.Text.Trim();

            if (strUserCode != strMakeUserCode)
            {
                BT_New.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
            }
        }
    }

    protected void LoadRelatedActorGroup(string strRelatedType, string strRelatedID, string strLangCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ActorGroup as actorGroup ";
        strHQL += " where actorGroup.GroupName in (select relatedActorGroup.ActorGroupName from RelatedActorGroup as relatedActorGroup where relatedActorGroup.RelatedType = " + "'" + strRelatedType + "'" + " and  relatedActorGroup.RelatedID = " + strRelatedID + ")";
        strHQL += " and actorGroup.LangCode = " + "'" + strLangCode + "'";
        strHQL += " order by actorGroup.SortNumber ASC";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        lst = actorGroupBLL.GetAllActorGroups(strHQL);

        Repeater1.DataSource = lst;
        Repeater1.DataBind();
    }


    protected void LoadActorGroup(string strUserCode, string strDepartString)
    {
        string strHQL;
        IList lst;


        strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName not in ('个人','部门','公司','集团','All')";
        strHQL += " and (actorGroup.BelongDepartCode in " + strDepartString;
        strHQL += " Or actorGroup.MakeUserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and actorGroup.LangCode = " + "'" + strLangCode + "'";

        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        lst = actorGroupBLL.GetAllActorGroups(strHQL);
        Repeater1.DataSource = lst;
        Repeater1.DataBind();
    }

    protected void LoadActorGroupDetail(string strGroupName)
    {
        string strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strGroupName + "'";
        ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
        IList lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_SqlGM.Text = strHQL;
    }

    protected string GetMakeUserCode(string strGroupName)
    {
        IList lst;
        string strHQL, strMakeUserCode;

        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        ActorGroup actorGroup = new ActorGroup();
        strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName = " + "'" + strGroupName + "'";
        lst = actorGroupBLL.GetAllActorGroups(strHQL);
        actorGroup = (ActorGroup)lst[0];

        strMakeUserCode = actorGroup.MakeUserCode.Trim();

        return strMakeUserCode;
    }

    protected string GetProjectName(string strRelatedID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strRelatedID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];
        string strRelatedName = project.ProjectName.Trim();
        return strRelatedName;
    }

    protected string GetProjectTaskName(string strTaskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectTask as projectTask where projectTask.TaskID = " + strTaskID;
        ProjectTaskBLL projectTaskBLL = new ProjectTaskBLL();
        lst = projectTaskBLL.GetAllProjectTasks(strHQL);

        ProjectTask projectTask = (ProjectTask)lst[0];

        return projectTask.Task.Trim();
    }

    protected string GetMeetingName(string strMeetingID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Meeting as meeting where meeting.ID = " + strMeetingID;
        MeetingBLL meetingBLL = new MeetingBLL();
        lst = meetingBLL.GetAllMeetings(strHQL);

        Meeting meeting = (Meeting)lst[0];

        return meeting.Name.Trim();
    }

    protected string GetProjectRiskName(string strRiskID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectRisk as projectRisk where projectRisk.ID = " + strRiskID;
        ProjectRiskBLL projectRiskBLL = new ProjectRiskBLL();
        lst = projectRiskBLL.GetAllProjectRisks(strHQL);

        ProjectRisk projectRisk = (ProjectRisk)lst[0];

        return projectRisk.Risk.Trim();
    }

    protected string GetRequirementName(string strReqID)
    {
        string strHQL = "from Requirement as requirement where requirement.ReqID = " + strReqID;
        RequirementBLL requirementBLL = new RequirementBLL();

        IList lst = requirementBLL.GetAllRequirements(strHQL);

        Requirement requirement = (Requirement)lst[0];

        return requirement.ReqName.Trim();
    }
}
