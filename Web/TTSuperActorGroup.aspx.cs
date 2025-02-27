using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTSuperActorGroup : System.Web.UI.Page
{
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName, strDepartCode;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "全局角色组设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            TakeTopCore.CoreShareClass.InitialAllDepartmentTree(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1);

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid4);

            LoadActorGroup(strLangCode);

            BT_New.Enabled = false;

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
        }
    }

    protected void BT_CreateActorGroup_Click(object sender, EventArgs e)
    {
        string strGroupName, strUserCode, strDepartCode, strDepartName;
        string strType;

        string strHQL;
        IList lst;

        strGroupName = TB_ActorGroup.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        strType = DL_Type.SelectedValue.Trim();

        if (strGroupName != "")
        {
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            ActorGroup actorGroup = new ActorGroup();

            strHQL = "from ActorGroup as actorGroup Where actorGroup.GroupName = " + "'" + strGroupName + "'";
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            if (lst.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCZTMDJSZQJC").ToString().Trim() + "')", true);
                return;
            }

            actorGroup.GroupName = strGroupName;
            actorGroup.MakeUserCode = strUserCode;
            actorGroup.Type = strType;
            actorGroup.IdentifyString = DateTime.Now.ToString("yyyyMMddHHMMssff");
            actorGroup.BelongDepartCode = strDepartCode;
            actorGroup.BelongDepartName = strDepartName;
            actorGroup.HomeName = strGroupName;
            actorGroup.LangCode = strLangCode;
            actorGroup.MakeType = "DIY";
            actorGroup.SortNumber = int.Parse(NB_SortNumber.Amount.ToString());

            try
            {
                actorGroupBLL.AddActorGroup(actorGroup);

                LoadActorGroup(strLangCode);
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


    protected void BT_FindActorGroup_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strGroupName = "%" + TB_ActorGroup.Text.Trim()+ "%";

        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        strHQL = "from ActorGroup as actorGroup ";
        strHQL += " where actorGroup.LangCode = " + "'" + strLangCode + "'";
        strHQL += " and actorGroup.GroupName Like '" + strGroupName + "'";
        strHQL += " order by actorGroup.SortNumber ASC";
        lst = actorGroupBLL.GetAllActorGroups(strHQL);
        Repeater1.DataSource = lst;
        Repeater1.DataBind();
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strGroupName, strMakeUserCode, strUserCode;
            string strHQL;
            IList lst;

            DataGrid2.CurrentPageIndex = 0;

            for (int i = 0; i < Repeater1.Items.Count; i++)
            {
                ((Button)Repeater1.Items[i].FindControl("BT_GroupName")).ForeColor = Color.White;
            }
            ((Button)e.Item.FindControl("BT_GroupName")).ForeColor = Color.Red;

            strGroupName = ((Button)e.Item.FindControl("BT_GroupName")).Text.Trim();
            strMakeUserCode = GetMakeUserCode(strGroupName);
            strUserCode = LB_UserCode.Text.Trim();

            strHQL = "From ActorGroup as actorGroup Where actorGroup.GroupName = " + "'" + strGroupName + "'";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            ActorGroup actorGroup = (ActorGroup)lst[0];

            DL_Type.SelectedValue = actorGroup.Type.Trim();
            TB_BelongDepartName.Text = actorGroup.BelongDepartName;
            LB_BelongDepartCode.Text = actorGroup.BelongDepartCode;
            NB_SortNumber.Amount = actorGroup.SortNumber;

            ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
            strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strGroupName + "'";
            lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_SqlGM.Text = strHQL;

            LB_ActorGroupName.Text = strGroupName;
            LB_ActorGroup.Text = strGroupName;

            BT_SaveActorGroup.Enabled = true;
            BT_New.Enabled = true;

            if (strGroupName != LanguageHandle.GetWord("GeRen").ToString().Trim() & strGroupName != LanguageHandle.GetWord("QuanTi").ToString().Trim() & strGroupName != LanguageHandle.GetWord("BuMen").ToString().Trim() & strGroupName != LanguageHandle.GetWord("GongSi").ToString().Trim() & strGroupName != LanguageHandle.GetWord("JiTuan").ToString().Trim() & strGroupName != "All")
            {
                BT_DeleteActorGroup.Enabled = true;
            }
            else
            {
                BT_DeleteActorGroup.Enabled = false;
            }
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            strDepartName = ShareClass.GetDepartName(strDepartCode);

            LB_BelongDepartCode.Text = strDepartCode;
            TB_BelongDepartName.Text = strDepartName;
        }
    }

    protected void BT_SaveActorGroup_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strGroupName, strType;
        int intID;
        strGroupName = LB_ActorGroupName.Text.Trim();
        strType = DL_Type.SelectedValue.Trim();

        try
        {
            strHQL = "From ActorGroup as actorGroup Where GroupName = " + "'" + strGroupName + "'";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);

            ActorGroup actorGroup = (ActorGroup)lst[0];

            intID = actorGroup.ID;
            actorGroup.Type = strType;
            actorGroup.BelongDepartCode = LB_BelongDepartCode.Text.Trim();
            actorGroup.BelongDepartName = TB_BelongDepartName.Text.Trim();
            actorGroup.SortNumber = int.Parse(NB_SortNumber.Amount.ToString());

            actorGroupBLL.UpdateActorGroup(actorGroup, intID);

            LoadActorGroup(strLangCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteActorGroup_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strGroupName = LB_ActorGroup.Text.Trim();

        strHQL = "Delete From T_ActorGroup Where GroupName = " + "'" + strGroupName + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadActorGroup(strLangCode);

            BT_DeleteActorGroup.Enabled = false;
            BT_SaveActorGroup.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCJSZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();

            LB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);
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

       
                LB_ID.Text = actorGroupDetail.GroupID.ToString();
                LB_RelatedUserCode.Text = actorGroupDetail.UserCode;
                LB_RelatedUserName.Text = actorGroupDetail.UserName;
         
                TB_Actor.Text = actorGroupDetail.Actor;
                TB_WorkDetail.Text = actorGroupDetail.WorkDetail;

                string strGroupName = actorGroupDetail.GroupName.Trim();
                string strMakeUserCode = GetMakeUserCode(strGroupName);
                string strUserCode = LB_UserCode.Text.Trim();
                string strDepartCode = LB_DepartCode.Text.Trim();


                try
                {
                    LB_DepartCode.Text = actorGroupDetail.DepartCode;
                    LB_DepartName.Text = ShareClass.GetDepartName(LB_DepartCode.Text);
                }
                catch
                {
                    LB_DepartCode.Text = ShareClass.GetDepartCodeFromUserCode(Session["UserCode"].ToString());
                    LB_DepartName.Text = ShareClass.GetDepartName(LB_DepartCode.Text);
                }

                BT_New.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strHQL;

                string strGroupName = LB_ActorGroupName.Text.Trim();

                string strUserCode = LB_RelatedUserCode.Text.Trim();
                string strUserName = LB_RelatedUserName.Text.Trim();
                string strDepartCode = LB_DepartCode.Text.Trim();
                string strDepartName = ShareClass.GetDepartName(strDepartCode);
                string strActor = TB_Actor.Text.Trim();
                string strWorkDetail = TB_WorkDetail.Text.Trim();

                try
                {
                    strHQL = "Delete From T_ActorGroupDetail Where GroupID = " + strGroupID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadActorGroupDetail(strGroupName);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCJC").ToString().Trim() + "')", true);
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

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
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

    protected void CB_SelectAllUser_CheckedChanged(object sender, EventArgs e)
    {
        if (CB_SelectAllUser.Checked == true)
        {
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_SelectUser")).Checked = true;
            }
        }
        else
        {
            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                ((CheckBox)DataGrid4.Items[i].FindControl("CB_SelectUser")).Checked = false;
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }


    protected void BT_AddAllSelect_Click(object sender, EventArgs e)
    {
        AddAllSelectMember();
    }

    protected void AddAllSelectMember()
    {
        string strGroupID;
        string strGroupName = LB_ActorGroupName.Text.Trim();
        string strRelatedUserCode;
        string strRelatedUserName;
        string strDepartCode, strDepartName;

        string strActor = TB_Actor.Text.Trim();
        string strWorkDetail = TB_WorkDetail.Text.Trim();

        if (strActor != "")
        {
            try
            {
                ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
                ActorGroupDetail actorGroupDetail = new ActorGroupDetail();

                for (int i = 0; i < DataGrid4.Items.Count; i++)
                {
                    if (((CheckBox)DataGrid4.Items[i].FindControl("CB_SelectUser")).Checked == true)
                    {
                        actorGroupDetail.GroupName = strGroupName;
                        strRelatedUserCode = ((Button)DataGrid4.Items[i].FindControl("BT_UserCode")).Text.Trim();
                        strRelatedUserName = ((Button)DataGrid4.Items[i].FindControl("BT_UserName")).Text.Trim();
                        actorGroupDetail.UserCode = strRelatedUserCode;
                        actorGroupDetail.UserName = strRelatedUserName;

                        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strRelatedUserCode);
                        strDepartName = ShareClass.GetDepartName(strDepartCode);
                        actorGroupDetail.DepartCode = strDepartCode;
                        actorGroupDetail.DepartName = strDepartName;
                        actorGroupDetail.Actor = strActor;
                        actorGroupDetail.WorkDetail = strWorkDetail;

                        actorGroupDetailBLL.AddActorGroupDetail(actorGroupDetail);

                        strGroupID = ShareClass.GetMyCreatedMaxActorGroupDetailID(strGroupName);
                        LB_ID.Text = strGroupID;

                        LB_RelatedUserCode.Text = strRelatedUserCode;
                        LB_RelatedUserName.Text = strRelatedUserName;
                    }
                }

                LoadActorGroupDetail(strGroupName);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZJingGaoJiaoSeBuNengWeiKongQi").ToString().Trim()+"')", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
        }
    }

    protected void BT_DeleteAllSelect_Click(object sender, EventArgs e)
    {
        DeleteAllSelectMember();
    }

    protected void DeleteAllSelectMember()
    {
        string strHQL;

        string strGroupName = LB_ActorGroupName.Text.Trim();
        string strRelatedUserCode;
        string strRelatedUserName;

        try
        {
            ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
            ActorGroupDetail actorGroupDetail = new ActorGroupDetail();

            for (int i = 0; i < DataGrid4.Items.Count; i++)
            {
                if (((CheckBox)DataGrid4.Items[i].FindControl("CB_SelectUser")).Checked == true)
                {
                    strRelatedUserCode = ((Button)DataGrid4.Items[i].FindControl("BT_UserCode")).Text.Trim();
                    strRelatedUserName = ((Button)DataGrid4.Items[i].FindControl("BT_UserName")).Text.Trim();
                    strHQL = "Delete From T_ActorGroupDetail Where GroupName = '" + strGroupName + "' and UserCode = '" + strRelatedUserCode + "'";
                    ShareClass.RunSqlCommand(strHQL);
                }
            }

            LoadActorGroupDetail(strGroupName);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSB").ToString().Trim() + "')", true);
        }
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
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }
    }

    protected void UpdateMember()
    {
        string strHQL;
        IList lst;

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
            strHQL = "From ActorGroupDetail as actorGroupDetail Where actorGroupDetail.GroupID = " + strID;
            lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);
            ActorGroupDetail actorGroupDetail = (ActorGroupDetail)lst[0];

            actorGroupDetail.GroupName = strGroupName;
            actorGroupDetail.UserCode = strRelatedUserCode;
            actorGroupDetail.UserName = strRelatedUserName;
            actorGroupDetail.DepartCode = strDepartCode;
            actorGroupDetail.DepartName = strDepartName;
            actorGroupDetail.Actor = strActor;
            actorGroupDetail.WorkDetail = strWorkDetail;

            actorGroupDetailBLL.UpdateActorGroupDetail(actorGroupDetail, int.Parse(strID));

            LoadActorGroupDetail(strGroupName);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strGroupName, strMakeUserCode, strUserCode;
            string strHQL;
            IList lst;

            strGroupName = ((Button)e.Item.FindControl("BT_GroupName")).Text.Trim();
            strMakeUserCode = GetMakeUserCode(strGroupName);
            strUserCode = LB_UserCode.Text.Trim();

            ActorGroupDetailBLL actorGroupDetailBLL = new ActorGroupDetailBLL();
            strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strGroupName + "'";
            lst = actorGroupDetailBLL.GetAllActorGroupDetails(strHQL);

            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LB_ActorGroupName.Text = strGroupName;
            LB_ActorGroup.Text = strGroupName;

            BT_New.Enabled = true;
        }
    }

    protected void LoadActorGroup(string strLangCode)
    {
        string strHQL;
        IList lst;

        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        strHQL = "from ActorGroup as actorGroup ";
        strHQL += " where actorGroup.LangCode = " + "'" + strLangCode + "'";
        strHQL += " order by actorGroup.SortNumber ASC";
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




}