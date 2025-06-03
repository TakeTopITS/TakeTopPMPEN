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

public partial class TTActorGroup : System.Web.UI.Page
{
    string strUserType;
    string strLangCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName, strDepartCode;

        strLangCode = Session["LangCode"].ToString();
        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();
        strUserType = Session["UserType"].ToString();

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;


        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "½ÇÉ«×éÉèÖÃ", strUserCode);
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
            if (strUserType == "INNER")
            {
                TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentStringByUserInfor(strUserCode);
            }
            else
            {
                LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            }

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid4);
            LB_DepartCode.Text = strDepartCode;

            LoadActorGroup(strLangCode);

            BT_New.Enabled = false;

            TakeTopCore.CoreShareClass.InitialUnderDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView2, strUserCode);
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

        strType = "All";

        if (strGroupName != "")
        {
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            ActorGroup actorGroup = new ActorGroup();
            strHQL = "from ActorGroup as actorGroup Where actorGroup.GroupName = " + "'" + strGroupName + "'";
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            if (lst.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJSZZMBNWK").ToString().Trim() + "')", true);
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

                LB_ActorGroup.Text = actorGroup.GroupName.Trim();

                BT_New.Enabled = true;

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

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strGroupName, strBelongDepartCode, strUserCode;
            string strDepartString;

            string strHQL;
            IList lst;

            DataGrid2.CurrentPageIndex = 0;

            for (int i = 0; i < Repeater1.Items.Count; i++)
            {
                ((Button)Repeater1.Items[i].FindControl("BT_GroupName")).ForeColor = Color.White;
            }
            ((Button)e.Item.FindControl("BT_GroupName")).ForeColor = Color.Red;

            strGroupName = ((Button)e.Item.FindControl("BT_GroupName")).Text.Trim();
            strUserCode = LB_UserCode.Text.Trim();

            strHQL = "From ActorGroup as actorGroup Where actorGroup.GroupName = " + "'" + strGroupName + "'";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            ActorGroup actorGroup = (ActorGroup)lst[0];

            LB_ActorGroupType.Text = actorGroup.Type.Trim();
            TB_BelongDepartName.Text = actorGroup.BelongDepartName;
            LB_BelongDepartCode.Text = actorGroup.BelongDepartCode;
            LB_GroupType.Text = actorGroup.Type.Trim();
            NB_SortNumber.Amount = actorGroup.SortNumber;

            LoadActorGroupDetail(strGroupName, LB_GroupType.Text);

            LB_ActorGroupName.Text = strGroupName;
            LB_ActorGroup.Text = strGroupName;

            strDepartString = LB_DepartString.Text.Trim();
            strBelongDepartCode = LB_BelongDepartCode.Text.Trim();

            if (strDepartString.IndexOf(strBelongDepartCode) < 0)
            {
                BT_New.Enabled = false;
                BT_SaveActorGroup.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
                BT_SaveActorGroup.Enabled = true;
            }
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

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid4);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

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

        TreeView2.SelectedNode.Selected = false;
    }

    protected void BT_SaveActorGroup_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strGroupName;
        int intID;
        strGroupName = LB_ActorGroupName.Text.Trim();

        try
        {
            strHQL = "From ActorGroup as actorGroup Where GroupName = " + "'" + strGroupName + "'";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);

            ActorGroup actorGroup = (ActorGroup)lst[0];

            intID = actorGroup.ID;
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
                LB_DepartCode.Text = actorGroupDetail.DepartCode;
                TB_Actor.Text = actorGroupDetail.Actor;
                TB_WorkDetail.Text = actorGroupDetail.WorkDetail;

                string strGroupName = actorGroupDetail.GroupName.Trim();
                string strUserCode = LB_UserCode.Text.Trim();

                string strDepartCode = LB_DepartCode.Text.Trim();
                try
                {
                    LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
                }
                catch
                {
                    LB_DepartCode.Text = ShareClass.GetDepartCodeFromUserCode(strUserCode);
                    LB_DepartName.Text = ShareClass.GetDepartName(LB_DepartCode.Text);
                }

                string strDepartString = LB_DepartString.Text.Trim();

                if (strDepartString.IndexOf(strDepartCode) < 0)
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
                string strHQL;

                string strGroupName = LB_ActorGroupName.Text.Trim();

                try
                {
                    strHQL = "Delete From T_ActorGroupDetail Where GroupID = " + strGroupID;
                    ShareClass.RunSqlCommand(strHQL);

                    LoadActorGroupDetail(strGroupName, LB_GroupType.Text);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuLanguageHandleGetWord").ToString().Trim()+"')", true); 
                }
            }
        }
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

                LoadActorGroupDetail(strGroupName, LB_GroupType.Text);
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

            LoadActorGroupDetail(strGroupName, LB_GroupType.Text);
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
              
                LoadActorGroupDetail(strGroupName, LB_GroupType.Text);
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

            LoadActorGroupDetail(strGroupName, LB_GroupType.Text);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZXiuGaiLanguageHandleGetWordZ").ToString().Trim() + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true); 
        }
    }
    

    protected void LoadActorGroup(string strLangCode)
    {
        string strHQL;
        string strUserCode, strDepartCode, strDepartString;

        strUserCode = LB_UserCode.Text.Trim();
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartString = LB_DepartString.Text.Trim();

        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        strHQL = "Select GroupName From T_ActorGroup Where Type <>'Part'";
        strHQL += " and (BelongDepartCode in " + strDepartString + " Or Type = 'Super'";
        strHQL += " Or MakeUserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and LangCode = " + "'" + strLangCode + "'";
        strHQL += " Order by SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroup");
        Repeater1.DataSource = ds;
        Repeater1.DataBind();
    }

    protected void LoadActorGroupDetail(string strGroupName, string strGroupType)
    {
        string strHQL;

        if (strGroupType != "Super")
        {
            strHQL = "Select * From T_ActorGroupDetail Where GroupName  = " + "'" + strGroupName + "'";
        }
        else
        {
            strHQL = "Select * From T_ActorGroupDetail Where GroupName  = " + "'" + strGroupName + "'";
            strHQL += " and UserCode in (Select UserCode From T_ProjectMember Where DepartCode in " + LB_DepartString.Text.Trim() + ")";
        }
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ActorGroupDetail");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
}
