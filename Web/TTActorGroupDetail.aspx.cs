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

public partial class TTActorGroupDetail : System.Web.UI.Page
{
    string strUserType;
    string strGroupType;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strIdentifyString, strUserCode, strDepartCode, strGroupName;
        string strBelongDepartCode, strDepartString, strMakeUserCode;

        strUserCode = Session["UserCode"].ToString();
        strIdentifyString = Request.QueryString["IdentifyString"].Trim();

        ActorGroup actorGroup = GetActorGroup(strIdentifyString);
        strGroupName = actorGroup.GroupName.Trim();
        strGroupType = actorGroup.Type.Trim();
        strBelongDepartCode = actorGroup.BelongDepartCode.Trim();
        strMakeUserCode = actorGroup.MakeUserCode.Trim();
        strIdentifyString = actorGroup.IdentifyString.Trim();

        LB_UserCode.Text = strUserCode;
        LB_ActorGroupName.Text = strGroupName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll", "SaveScroll();");
        if (!Page.IsPostBack)
        {
            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView1, strUserCode);
            strDepartString = LB_DepartString.Text.Trim();

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid4);

            if (strDepartString.IndexOf(strBelongDepartCode) < 0)
            {
                BT_New.Enabled = false;
            }
            else
            {
                BT_New.Enabled = true;
            }

            LoadActorGroupDetail(strGroupName, strGroupType);

            if (Session["SuperWFAdmin"].ToString() == "YES" | strMakeUserCode == strUserCode)
            {
                Response.Redirect("TTSuperActorGroupDetail.aspx?IdentifyString=" + strIdentifyString);
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
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll();", true);

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
                LB_DepartCode.Text = actorGroupDetail.DepartCode;
                TB_Actor.Text = actorGroupDetail.Actor;
                TB_WorkDetail.Text = actorGroupDetail.WorkDetail;

                string strGroupName = actorGroupDetail.GroupName.Trim();
                string strMakeUserCode = GetMakeUserCode(strGroupName);
                string strUserCode = LB_UserCode.Text.Trim();

                try
                {
                    string strDepartCode = LB_DepartCode.Text.Trim();
                    LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
                }
                catch
                {
                    LB_DepartCode.Text = ShareClass.GetDepartCodeFromUserCode(strUserCode);
                    LB_DepartName.Text = ShareClass.GetDepartName(LB_DepartCode.Text);
                }

                BT_New.Enabled = true;

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

                    LoadActorGroupDetail(strGroupName, strGroupType);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZShanChuLanguageHandleGetWord").ToString().Trim()+"')", true); 
                }
            }
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();
        string strGroupName = LB_ActorGroupName.Text.Trim();

        LB_RelatedUserCode.Text = strUserCode;
        LB_RelatedUserName.Text = strUserName;
        LB_DepartCode.Text = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        LB_DepartName.Text = ShareClass.GetDepartName(LB_DepartCode.Text.Trim());

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

                LoadActorGroupDetail(strGroupName, strGroupType);
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

            LoadActorGroupDetail(strGroupName, strGroupType);
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

                LoadActorGroupDetail(strGroupName, strGroupType);
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

        string strID = LB_ID.Text.Trim();
        string strGroupName = LB_ActorGroupName.Text.Trim();
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

            LoadActorGroupDetail(strGroupName, strGroupType);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" +LanguageHandle.GetWord("ZZXiuGaiLanguageHandleGetWordZ").ToString().Trim()+"')", true); 
        }
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

    protected ActorGroup GetActorGroup(string strIdentifyString)
    {
        string strHQL;
        IList lst;

        strHQL = "from ActorGroup as actorGroup where actorGroup.IdentifyString= " + "'" + strIdentifyString + "'";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        lst = actorGroupBLL.GetAllActorGroups(strHQL);

        ActorGroup actorGroup = (ActorGroup)lst[0];

        return actorGroup;
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
