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

public partial class TTPlanCopyToOtherMembers : System.Web.UI.Page
{
    //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
    string strRelatedType, strRelatedID, strRelatedCode;
    string strUserCode, strUserName;
    string strIsMobileDevice;
    string strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        //Session["UserCode"] = "C0001";
        //Session["IsMobileDevice"] = "NO";

        strLangCode = Session["LangCode"].ToString();


        string strDepartCode, strPosition;

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_PlanDetail);

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);


        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx");
        bool blVisible =TakeTopSecurity.TakeTopLicense. GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
        strRelatedType = Request.QueryString["RelatedType"];
        if (strRelatedType == null)
        {
            strRelatedType = "OTHER";
        }

        strRelatedID = Request.QueryString["RelatedID"];
        if (strRelatedID == null)
        {
            strRelatedID = "0";
        }

        strRelatedCode = Request.QueryString["RelatedCode"];
        if (strRelatedCode == null)
        {
            strRelatedCode = "";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_PlanDetail.Visible = true;
            }
            else
            {
                HE_PlanDetail.Visible = true;
            }

            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_JoinTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
            ShareClass.InitialPlanTreeByUserCode(TreeView1, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
            ShareClass.InitialPlanTreeByUserCode(TreeView2, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3, strUserCode);

            LoadPlanType();

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            strPosition = ShareClass.GetUserDuty(strUserCode);

            LoadKPI(strDepartCode, strPosition);

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView4, strUserCode);
            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
         
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid4.DataSource = lst;
            DataGrid4.DataBind();

            LoadActorGroup(strUserCode, LB_DepartString.Text, strLangCode);

            BT_DeletePlanToAllSystemUser.Attributes.Add("onclick", "return confirm('你确定要删除从此计划复制的其它成员的计划吗？');");   
        }
    }
    

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strPlanID, strPlanName, strParentID;
        int intLeaderReviewCount, intWorkLogCount;
        int intChildCount;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strPlanID = treeNode.Target.Trim();
        strPlanName = treeNode.Text.Trim();

        strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        Plan plan = new Plan();

        if (lst.Count > 0)
        {
            plan = (Plan)lst[0];

            LB_PlanID.Text = plan.PlanID.ToString();
            DL_PlanType.SelectedValue = plan.PlanType;
            LB_ParentPlanID.Text = plan.ParentID.ToString();

            string strParentPlanID, strParentCreatorCode;
            Plan parentPlan;

            strParentPlanID = plan.ParentID.ToString();
            parentPlan = GetPlan(strParentPlanID);
            if (parentPlan != null)
            {
                strParentCreatorCode = parentPlan.CreatorCode.Trim();
                if (strParentCreatorCode == strUserCode)
                {
                    LB_SelectedPlanID.Text = parentPlan.PlanID.ToString();
                }
                else
                {
                    LB_SelectedPlanID.Text = parentPlan.BackupPlanID.ToString();
                }
            }
            else
            {
                LB_SelectedPlanID.Text = "0";
            }

            strParentID = plan.ParentID.ToString();
            TB_ParentPlanName.Text = GetPlanNameByPlanID(strParentID);

            TB_PlanName.Text = plan.PlanName.Trim();

            if (strIsMobileDevice == "YES")
            {
                HT_PlanDetail.Text = plan.PlanDetail.Trim();
            }
            else
            {
                HE_PlanDetail.Text = plan.PlanDetail.Trim();
            }

            DLC_StartTime.Text = plan.StartTime.ToString("yyyy-MM-dd");
            DLC_EndTime.Text = plan.EndTime.ToString("yyyy-MM-dd");
            DL_Status.SelectedValue = plan.Status.Trim();

            LB_SubmitTime.Text = plan.SubmitTime;

            intLeaderReviewCount = GetPlanRelatedLeaderCountByActiveFinish(strPlanID);
            intWorkLogCount = GetPlanWorkLogCountByActiveFinish(strPlanID);

            intChildCount = treeNode.ChildNodes.Count;

            if (intLeaderReviewCount == 0 & intWorkLogCount == 0)
            {
                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                BT_AddTarget.Enabled = true;
                BT_AddLeader.Enabled = true;

                BT_SubmitApprove.Enabled = true;
            }
            else
            {
                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;

                DL_Status.Enabled = false;

                BT_SubmitApprove.Enabled = false;

                BT_AddTarget.Enabled = false;
                BT_UpdateTarget.Enabled = false;
                BT_DeleteTarget.Enabled = false;

                BT_AddLeader.Enabled = false;
                BT_UpdateLeader.Enabled = false;
                BT_DeleteLeader.Enabled = false;
            }

            if (intChildCount > 0)
            {
                BT_Delete.Enabled = false;
                DL_Status.Enabled = false;
            }

            BT_CopyPlanToAllSystemUserTest.Enabled = true;
            BT_CopyPlanToAllSystemUser.Enabled = true;
            BT_DeletePlanToAllSystemUser.Enabled = true;


            LoadPlanCopyRelatedUser(strPlanID);

            LoadPlanTarget(strPlanID);
            LoadPlanLeader(strPlanID);

            HL_RelatedDoc.Enabled = true;
            HL_RelatedDoc.NavigateUrl = "TTPlanRelatedDoc.aspx?PlanID=" + strPlanID;
        }
    }

    protected Plan GetPlan(string strPlanID)
    {
        string strHQL;

        strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        IList lst = planBLL.GetAllPlans(strHQL);
        if (lst.Count > 0)
        {
            Plan plan = (Plan)lst[0];
            return plan;
        }
        else
        {
            return null;
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strPlanID, strPlanName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target == "0")
        {
            strPlanID = treeNode.Target.Trim();
            strPlanName = strUserName + LanguageHandle.GetWord("DeJiHua").ToString().Trim();

            LB_SelectedPlanID.Text = strPlanID;
        }
        else
        {
            strPlanID = treeNode.Target.Trim();
            strPlanName = treeNode.Text.Trim();

            string strCreatorCode;
            Plan plan;

            plan = GetPlan(strPlanID);
            strCreatorCode = plan.CreatorCode.Trim();
            if (strCreatorCode != strUserCode)
            {
                LB_SelectedPlanID.Text = plan.BackupPlanID.ToString();
            }
            else
            {
                LB_SelectedPlanID.Text = plan.PlanID.ToString();
            }
        }

        LB_ParentPlanID.Text = strPlanID;
        TB_ParentPlanName.Text = strPlanName;
    }

    protected void BT_Add_Click(object sender, EventArgs e)
    {
        string strPlanID, strSelectedPlanID, strParentID, strPlanType, strPlanName, strPlanDetail, strStatus;
        DateTime dtStartTime, dtEndTime;
        int intCount;

        string strHQL;

        strPlanType = DL_PlanType.SelectedValue.Trim();
        strPlanName = TB_PlanName.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strPlanDetail = HT_PlanDetail.Text.Trim();
        }
        else
        {
            strPlanDetail = HE_PlanDetail.Text.Trim();
        }
        strStatus = DL_Status.SelectedValue.Trim();

        dtStartTime = DateTime.Parse(DLC_StartTime.Text);
        dtEndTime = DateTime.Parse(DLC_EndTime.Text);

        strParentID = LB_ParentPlanID.Text.Trim();
        strSelectedPlanID = LB_SelectedPlanID.Text.Trim();
        if (strSelectedPlanID == "")
        {
            strSelectedPlanID = "0";
        }

        intCount = GetSameNamePlanCount(strUserCode, strPlanName);
        if (intCount > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZTMDJHBNXZJC").ToString().Trim() + "')", true);
            return;
        }

        if (strParentID == "" | strPlanType == "" | strPlanName == "" | strPlanDetail == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBFJHJHLXJHMCHJHNRDBNWKJC").ToString().Trim() + "')", true);
            return;
        }

        PlanBLL planBLL = new PlanBLL();
        Plan plan = new Plan();

        plan.ParentID = int.Parse(strSelectedPlanID);
        plan.PlanType = strPlanType;
        plan.PlanName = strPlanName;
        plan.PlanDetail = strPlanDetail;
        plan.StartTime = dtStartTime;
        plan.EndTime = dtEndTime;
        plan.SubmitTime = "";
        plan.Progress = 0;
        plan.ScoringBySelf = 0;
        plan.ScoringByLeader = 0;

        plan.UserCode = strUserCode;
        plan.UserName = strUserName;
        plan.CreatorCode = strUserCode;
        plan.CreatorName = strUserName;
        plan.SubmitTime = "";

        //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
        plan.RelatedType = strRelatedType;
        int intRelatedID = 0;
        int.TryParse(strRelatedID, out intRelatedID);
        plan.RelatedID = intRelatedID;
        plan.RelatedCode = strRelatedCode;

        plan.Status = "New";

        try
        {
            planBLL.AddPlan(plan);
            strPlanID = ShareClass.GetMyCreatedMaxPlanID(strUserCode);
            LB_PlanID.Text = strPlanID;

            strHQL = "Update T_Plan Set BackupPlanID = " + strPlanID + " Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
            ShareClass.InitialPlanTreeByUserCode(TreeView1, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
            ShareClass.InitialPlanTreeByUserCode(TreeView2, strUserCode, strRelatedType, strRelatedID, strRelatedCode);

            HL_RelatedDoc.Enabled = true;
            HL_RelatedDoc.NavigateUrl = "TTPlanRelatedDoc.aspx?PlanID=" + strPlanID;

            DL_Status.SelectedValue = "New";

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;

            BT_AddTarget.Enabled = true;
            BT_AddLeader.Enabled = true;

            BT_SubmitApprove.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strParentID, strPlanID, strPlanType, strPlanName, strPlanDetail, strStatus, strCreatorCode;
        DateTime dtStartTime, dtEndTime;
        int intParentID, intCount;

        strPlanID = LB_PlanID.Text.Trim();
        strPlanType = DL_PlanType.SelectedValue.Trim();
        strPlanName = TB_PlanName.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strPlanDetail = HT_PlanDetail.Text.Trim();
        }
        else
        {
            strPlanDetail = HE_PlanDetail.Text.Trim();
        }
        strStatus = DL_Status.SelectedValue.Trim();

        dtStartTime = DateTime.Parse(DLC_StartTime.Text);
        dtEndTime = DateTime.Parse(DLC_EndTime.Text);

        strParentID = LB_ParentPlanID.Text.Trim();

        if (strParentID == strPlanID)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZJBNZWZJDFJHJC").ToString().Trim() + "')", true);
            return;
        }

        intCount = GetSameNamePlanCount(strUserCode, strPlanName, strPlanID);
        if (intCount > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZTMDJHBNXGJC").ToString().Trim() + "')", true);
            return;
        }

        if (strParentID == "" | strPlanType == "" | strPlanName == "" | strPlanDetail == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBFJHJHLXJHMCHJHNRDBNWKJC").ToString().Trim() + "')", true);
            return;
        }

        intParentID = int.Parse(LB_SelectedPlanID.Text.Trim());

        strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        Plan plan = new Plan();

        if (lst.Count > 0)
        {
            plan = (Plan)lst[0];

            strCreatorCode = plan.CreatorCode.Trim();
            if (strCreatorCode == strUserCode)
            {
                plan.ParentID = intParentID;
                plan.PlanType = strPlanType;
                plan.PlanName = strPlanName;
                plan.PlanDetail = strPlanDetail;
                plan.StartTime = dtStartTime;
                plan.EndTime = dtEndTime;

                //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
                plan.RelatedType = strRelatedType;
                int intRelatedID = 0;
                int.TryParse(strRelatedID, out intRelatedID);
                plan.RelatedID = intRelatedID;
                plan.RelatedCode = strRelatedCode;

                plan.Status = strStatus;
            }
            else
            {
                plan.PlanDetail = strPlanDetail;
            }

            try
            {
                planBLL.UpdatePlan(plan, int.Parse(strPlanID));

                //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
                ShareClass.InitialPlanTreeByUserCode(TreeView1, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
                ShareClass.InitialPlanTreeByUserCode(TreeView2, strUserCode, strRelatedType, strRelatedID, strRelatedCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strPlanID;

        strPlanID = LB_PlanID.Text.Trim();

        strHQL = "Select * From T_Plan Where PlanID in (Select ParentID From T_Plan) and PlanID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Plan");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZZJHBNSCJC").ToString().Trim() + "')", true);
            return;
        }


        Plan plan = GetPlan(strPlanID);
        if (plan.CreatorCode.Trim() != strUserCode)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFZGLDJHBNSCJC").ToString().Trim() + "')", true);
            return;
        }

        try
        {
            strHQL = "Delete From T_Plan Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_Plan_WorkLog Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_Plan_Target Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_Plan_RelatedLeader Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_Plan_LeaderReview Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);

            LB_SelectedPlanID.Text = LB_ParentPlanID.Text;

            //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
            ShareClass.InitialPlanTreeByUserCode(TreeView1, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
            ShareClass.InitialPlanTreeByUserCode(TreeView2, strUserCode, strRelatedType, strRelatedID, strRelatedCode);

            HL_RelatedDoc.Enabled = false;
            HL_RelatedDoc.NavigateUrl = "TTPlanRelatedDoc.aspx?PlanID=" + strPlanID;

            BT_Update.Enabled = false;
            BT_Delete.Enabled = false;

            BT_SubmitApprove.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_SubmitApprove_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strParentID, strPlanID, strPlanType, strPlanName, strPlanDetail, strStatus;
        DateTime dtStartTime, dtEndTime;
        int intParentID;

        strPlanID = LB_PlanID.Text.Trim();
        strPlanType = DL_PlanType.SelectedValue.Trim();
        strPlanName = TB_PlanName.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strPlanDetail = HT_PlanDetail.Text.Trim();
        }
        else
        {
            strPlanDetail = HE_PlanDetail.Text.Trim();
        }

        strStatus = DL_Status.SelectedValue.Trim();

        dtStartTime = DateTime.Parse(DLC_StartTime.Text);
        dtEndTime = DateTime.Parse(DLC_EndTime.Text);

        strParentID = LB_ParentPlanID.Text.Trim();

        if (strParentID == "" | strPlanType == "" | strPlanName == "" | strPlanDetail == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBFJHJHLXJHMCHJHNRDBNWKJC").ToString().Trim() + "')", true);
            return;
        }

        strHQL = "From PlanRelatedLeader as planRelatedLeader where planRelatedLeader.PlanID = " + strPlanID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

        if (lst.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCJHMYSZXGLDTJSBJC").ToString().Trim() + "')", true);
            return;
        }


        strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        Plan plan = new Plan();

        if (lst.Count > 0)
        {
            plan = (Plan)lst[0];

            intParentID = int.Parse(strParentID);
            plan.ParentID = intParentID;
            plan.PlanType = strPlanType;
            plan.PlanName = strPlanName;
            plan.PlanDetail = strPlanDetail;
            plan.StartTime = dtStartTime;
            plan.EndTime = dtEndTime;

            //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
            plan.RelatedType = strRelatedType;
            int intRelatedID = 0;
            int.TryParse(strRelatedID, out intRelatedID);
            plan.RelatedID = intRelatedID;
            plan.RelatedCode = strRelatedCode;

            plan.SubmitTime = DateTime.Now.ToString("yyyy-MM-dd");

            plan.Status = "Pending Review";

            try
            {
                planBLL.UpdatePlan(plan, int.Parse(strPlanID));

                LB_SubmitTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DL_Status.SelectedValue = "Pending Review";
                //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
                ShareClass.InitialPlanTreeByUserCode(TreeView1, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
                ShareClass.InitialPlanTreeByUserCode(TreeView2, strUserCode, strRelatedType, strRelatedID, strRelatedCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSHCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSHSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        string strPlanID, strTargetID;
        int intCount;

        strPlanID = LB_PlanID.Text.Trim();

        ShareClass.ColorDataGridSelectRow(DataGrid1, e);

        strTargetID = ((Button)e.Item.FindControl("BT_TargetID")).Text.Trim();

        strHQL = "From PlanTarget as planTarget Where planTarget.ID = " + strTargetID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        PlanTarget planTarget = new PlanTarget();

        if (lst.Count > 0)
        {
            planTarget = (PlanTarget)lst[0];

            LB_TargetID.Text = planTarget.ID.ToString();
            TB_Target.Text = planTarget.Target.Trim();
            NB_Progress.Amount = planTarget.Progress;

            intCount = GetPlanRelatedLeaderCountByActiveFinish(strPlanID);

            if (intCount == 0)
            {
                BT_UpdateTarget.Enabled = true;
                BT_DeleteTarget.Enabled = true;
            }
        }
    }

    protected void DL_UserKPI_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strKPI;

        strKPI = DL_UserKPI.SelectedValue.Trim();

        TB_Target.Text = strKPI;
    }

    protected void BT_AddTarget_Click(object sender, EventArgs e)
    {
        string strPlanID, strTargetID, strTarget;
        int intProgress;

        strTargetID = LB_TargetID.Text.Trim();
        strTarget = TB_Target.Text.Trim();
        intProgress = int.Parse(NB_Progress.Amount.ToString());

        strPlanID = LB_PlanID.Text.Trim();

        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        PlanTarget planTarget = new PlanTarget();

        planTarget.PlanID = int.Parse(strPlanID);
        planTarget.Target = strTarget;
        planTarget.Progress = intProgress;

        try
        {
            planTargetBLL.AddPlanTarget(planTarget);
            LoadPlanTarget(strPlanID);

            LB_TargetID.Text = ShareClass.GetMyCreatedMaxPlanTargetID(strPlanID);

            BT_UpdateTarget.Enabled = true;
            BT_DeleteTarget.Enabled = true;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UpdateTarget_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
        string strPlanID, strTargetID;

        strPlanID = LB_PlanID.Text.Trim();
        strTargetID = LB_TargetID.Text.Trim();

        strHQL = "From PlanTarget as planTarget Where planTarget.ID = " + strTargetID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        PlanTarget planTarget = new PlanTarget();

        if (lst.Count > 0)
        {
            planTarget = (PlanTarget)lst[0];

            planTarget.Target = TB_Target.Text.Trim();
            planTarget.Progress = int.Parse(NB_Progress.Amount.ToString());

            try
            {
                planTargetBLL.UpdatePlanTarget(planTarget, int.Parse(strTargetID));

                LoadPlanTarget(strPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
            }
        }
    }


    protected void BT_DeleteTarget_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strPlanID, strTargetID;

        strPlanID = LB_PlanID.Text.Trim();
        strTargetID = LB_TargetID.Text.Trim();

        strHQL = "From PlanTarget as planTarget Where planTarget.ID = " + strTargetID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        PlanTarget planTarget = new PlanTarget();

        if (lst.Count > 0)
        {
            planTarget = (PlanTarget)lst[0];

            try
            {
                planTargetBLL.DeletePlanTarget(planTarget);

                LoadPlanTarget(strPlanID);

                BT_UpdateTarget.Enabled = false;
                BT_DeleteTarget.Enabled = false;

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strPlanID, strLeaderID;
        int intCount;

        ShareClass.ColorDataGridSelectRow(DataGrid2, e);

        strPlanID = LB_PlanID.Text.Trim();

        strLeaderID = ((Button)e.Item.FindControl("BT_LeaderID")).Text.Trim();

        strHQL = "From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.ID = " + strLeaderID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

        PlanRelatedLeader planRelatedLeader = new PlanRelatedLeader();

        if (lst.Count > 0)
        {
            planRelatedLeader = (PlanRelatedLeader)lst[0];

            LB_LeaderID.Text = planRelatedLeader.ID.ToString();
            LB_LeaderCode.Text = planRelatedLeader.LeaderCode.Trim();
            LB_LeaderName.Text = planRelatedLeader.LeaderName.Trim();
            TB_Actor.Text = planRelatedLeader.Actor.Trim();
            DLC_JoinTime.Text = planRelatedLeader.JoinTime.ToString("yyyy-MM-dd");
            LB_LeaderStatus.Text = planRelatedLeader.Status.Trim();

            intCount = GetPlanRelatedLeaderCountByActiveFinish(strPlanID);

            if (intCount == 0)
            {
                BT_UpdateLeader.Enabled = true;
                BT_DeleteLeader.Enabled = true;
            }
        }
    }

    protected void BT_AddLeader_Click(object sender, EventArgs e)
    {
        string strPlanID;
        string strLeaderCode, strLeaderName, strActor, strStatus;
        DateTime dtJoinTime;

        strPlanID = LB_PlanID.Text.Trim();
        strLeaderCode = LB_LeaderCode.Text.Trim();
        strLeaderName = LB_LeaderName.Text.Trim();
        strActor = TB_Actor.Text.Trim();
        dtJoinTime = DateTime.Parse(DLC_JoinTime.Text);
        strStatus = "New";


        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        PlanRelatedLeader planRelatedLeader = new PlanRelatedLeader();

        planRelatedLeader.PlanID = int.Parse(strPlanID);
        planRelatedLeader.LeaderCode = strLeaderCode;
        planRelatedLeader.LeaderName = strLeaderName;
        planRelatedLeader.Actor = strActor;
        planRelatedLeader.JoinTime = dtJoinTime;
        planRelatedLeader.Status = strStatus;

        try
        {
            planRelatedLeaderBLL.AddPlanRelatedLeader(planRelatedLeader);

            LB_LeaderID.Text = ShareClass.GetMyCreatedMaxPlanRelatedLeaderID(strPlanID);

            LoadPlanLeader(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_UpdateLeader_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strPlanID, strLeaderID;
        string strLeaderCode, strLeaderName, strActor, strStatus;
        DateTime dtJoinTime;

        strLeaderID = LB_LeaderID.Text.Trim();
        strPlanID = LB_PlanID.Text.Trim();
        strLeaderCode = LB_LeaderCode.Text.Trim();
        strLeaderName = LB_LeaderName.Text.Trim();
        strActor = TB_Actor.Text.Trim();
        dtJoinTime = DateTime.Parse(DLC_JoinTime.Text);
        strStatus = "New";

        strHQL = "From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.ID = " + strLeaderID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

        PlanRelatedLeader planRelatedLeader = new PlanRelatedLeader();

        planRelatedLeader = (PlanRelatedLeader)lst[0];


        planRelatedLeader.LeaderCode = strLeaderCode;
        planRelatedLeader.LeaderName = strLeaderName;
        planRelatedLeader.Actor = strActor;
        planRelatedLeader.JoinTime = dtJoinTime;
        planRelatedLeader.Status = strStatus;

        try
        {
            planRelatedLeaderBLL.UpdatePlanRelatedLeader(planRelatedLeader, int.Parse(strLeaderID));

            LoadPlanLeader(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_DeleteLeader_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strPlanID, strLeaderID;

        strPlanID = LB_PlanID.Text.Trim();
        strLeaderID = LB_LeaderID.Text.Trim();


        strHQL = "From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.ID = " + strLeaderID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

        PlanRelatedLeader planRelatedLeader = new PlanRelatedLeader();

        planRelatedLeader = (PlanRelatedLeader)lst[0];

        try
        {
            planRelatedLeaderBLL.DeletePlanRelatedLeader(planRelatedLeader);
            LoadPlanLeader(strPlanID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void BT_SelectMember_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowPlanMember','false') ", true);
    }

    protected void BT_SelectActorGroup_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowActorGroup','false') ", true);
    }
    protected void BT_SelectLeader_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLeader','false') ", true);
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLeader','false') ", true);
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode, strUserName;

        ShareClass.ColorDataGridSelectRow(DataGrid3, e);

        strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        LB_LeaderCode.Text = strUserCode;
        LB_LeaderName.Text = strUserName;

        TB_Actor.Text = ShareClass.GetUserDuty(strUserCode);

        //ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowLeader','false') ", true);
    }

    protected void TreeView4_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView4.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid4);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowPlanMember','false') ", true);
    }

    protected void RP_Attendant_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strHQL;
            string strPlanID = LB_PlanID.Text.Trim();
            string strSelectedUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

            strHQL = "Select * From T_Plan Where BackupPlanID = " + strPlanID + " and UserCode  = " + "'" + strSelectedUserCode + "'";
            strHQL += " and PlanID in (Select PlanID From T_Plan_RelatedLeader Where Status  in ('Approved','Completed'))";
            strHQL += " and PlanID in (Select PlanID From T_Plan_WorkLog)";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Plan");

            if (ds.Tables[0].Rows.Count == 0)
            {
                strHQL = "Delete From T_PlanCopyRelatedUser Where PlanID = " + strPlanID + " and UserCode = " + "'" + strSelectedUserCode + "'";
                strHQL += " and UserCode <> " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Delete From T_Plan Where BackupPlanID = " + strPlanID + " and UserCode  = " + "'" + strSelectedUserCode + "'";
                strHQL += " and UserCode <> " + "'" + strUserCode + "'";
                strHQL += " and PlanID not in (Select PlanID From T_Plan_RelatedLeader Where Status  in ('Approved','Completed'))";
                strHQL += " and PlanID not in (Select PlanID From T_Plan_WorkLog)";
                ShareClass.RunSqlCommand(strHQL);

                LoadPlanCopyRelatedUser(strPlanID);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBCCYDXGJHYWCHYYRZBNSCJC").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_CopyPlanToAllSystemUserTest_Click(object sender, EventArgs e)
    {
        string strHQL1, strHQL2;
        string strPlanUserCode, strPlanUserName;
        string strPlanID, strParentPlanID, strPlanName;
        string strDuplicatePlanUser = "";
        int j = 0;

        DataSet ds1, ds2;

        strPlanID = LB_PlanID.Text.Trim();
        strPlanName = TB_PlanName.Text.Trim();

        strParentPlanID = LB_ParentPlanID.Text.Trim();

        if (strPlanID != "")
        {
            strHQL1 = "Select UserCode,UserName From T_PlanCopyRelatedUser Where PlanID = " + strPlanID + " and UserCode <> " + "'" + strUserCode + "'";
            ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_PlanCopyRelatedUser");
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                strPlanUserCode = ds1.Tables[0].Rows[i][0].ToString();
                strPlanUserName = ds1.Tables[0].Rows[i][1].ToString();

                //strHQL2 = "Select A.BackupPlanID From T_Plan A,T_Plan B Where A.PlanName = B.PlanName and B.PlanID = " + strParentPlanID;
                //strHQL2 += " and A.UserCode = " + "'" + strPlanUserCode + "'";

                strHQL2 = "Select BackupPlanID From T_Plan Where PlanName = '" + strPlanName + "'";
                strHQL2 += " and UserCode = " + "'" + strPlanUserCode + "'";
                ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_Plan");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        strDuplicatePlanUser += strPlanUserCode.Trim() + strPlanUserName.Trim() + ",";
                        j++;
                    }
                    catch
                    {
                    }
                }
            }

            if (j > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + strDuplicatePlanUser + " " + LanguageHandle.GetWord("ZZCZCFJH").ToString().Trim() + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCZCFJH").ToString().Trim() + "')", true);
            }
        }
    }

    protected void BT_CopyPlanToAllSystemUser_Click(object sender, EventArgs e)
    {
        string strHQL1, strHQL2, strHQL3, strHQL4;
        string strPlanUserCode, strPlanUserName;
        string strPlanID, strPlanName, strParentPlanID, strParentPlanName, strOldParentPlanID, strNewPlanID;

        DataSet ds1, ds2, ds4;

        strPlanID = LB_PlanID.Text.Trim();
        strPlanName = TB_PlanName.Text.Trim();

        //strParentPlanID = LB_ParentPlanID.Text.Trim();
        //strParentPlanName = TB_ParentPlanName.Text.Trim();

        if (strPlanID != "")
        {
            strHQL1 = "Select UserCode,UserName From T_PlanCopyRelatedUser Where PlanID = " + strPlanID + " and UserCode <> " + "'" + strUserCode + "'";
            ds1 = ShareClass.GetDataSetFromSql(strHQL1, "T_PlanCopyRelatedUser");
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                strPlanUserCode = ds1.Tables[0].Rows[i][0].ToString();
                strPlanUserName = ds1.Tables[0].Rows[i][1].ToString();

                strHQL2 = "Select BackupPlanID From T_Plan Where PlanName = '" + strPlanName + "'";
                strHQL2 += " and UserCode = " + "'" + strPlanUserCode + "'";
                ds2 = ShareClass.GetDataSetFromSql(strHQL2, "T_Plan");
                if (ds2.Tables[0].Rows.Count == 0)
                {
                    strParentPlanID = GetPlan(strPlanID).ParentID.ToString();
                    if (strParentPlanID != "0")
                    {
                        Plan Plan4 = GetPlan(strParentPlanID);
                        strParentPlanName = Plan4.PlanName.Trim();
                    }
                    else
                    {
                        strParentPlanName = strPlanName;
                    }

                    strHQL4 = "Select BackupPlanID From T_Plan Where PlanName ='" + strParentPlanName + "'" + " and UserCode = '" + strPlanUserCode + "'";
                    ds4 = ShareClass.GetDataSetFromSql(strHQL4, "T_Paln");
                    if (ds4.Tables[0].Rows.Count > 0)
                    {
                        strOldParentPlanID = ds4.Tables[0].Rows[0][0].ToString();

                        strHQL3 = "Insert Into T_Plan(PlanType,PlanName,PlanDetail,StartTime,EndTime,SubmitTime,Progress,ScoringBySelf,ScoringByLeader,ParentID,BackupPlanID,UserCode,UserName,CreatorCode,CreatorName,Status,RelatedCode,RelatedType,RelatedID)";
                        strHQL3 += " Select PlanType,PlanName,PlanDetail,StartTime,EndTime,SubmitTime,Progress,ScoringBySelf,ScoringByLeader," + strOldParentPlanID + ",BackupPlanID," + "'" + strPlanUserCode + "'" + "," + "'" + strPlanUserName + "'" + "," + "'" + strUserCode + "'" + "," + "'" + strUserName + "'" + ",Status,RelatedCode,RelatedType,RelatedID From T_Plan ";
                        strHQL3 += " Where PlanID = " + strPlanID;

                        try
                        {
                            ShareClass.RunSqlCommand(strHQL3);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        strHQL3 = "Insert Into T_Plan(PlanType,PlanName,PlanDetail,StartTime,EndTime,SubmitTime,Progress,ScoringBySelf,ScoringByLeader,ParentID,BackupPlanID,UserCode,UserName,CreatorCode,CreatorName,Status,RelatedCode,RelatedType,RelatedID)";
                        strHQL3 += " Select PlanType,PlanName,PlanDetail,StartTime,EndTime,SubmitTime,Progress,ScoringBySelf,ScoringByLeader,ParentID,BackupPlanID," + "'" + strPlanUserCode + "'" + "," + "'" + strPlanUserName + "'" + "," + "'" + strUserCode + "'" + "," + "'" + strUserName + "'" + ",Status,RelatedCode,RelatedType,RelatedID From T_Plan ";
                        strHQL3 += " Where PlanID = " + strPlanID;

                        try
                        {
                            ShareClass.RunSqlCommand(strHQL3);
                        }
                        catch
                        {
                        }
                    }

                    //strNewPlanID = ShareClass.GetMyCreatedMaxPlanID(strPlanUserCode);
                    //string strHQL;
                    //IList lst;
                    //strHQL = "From PlanRelatedLeader as planRelatedLeader where planRelatedLeader.PlanID = " + strNewPlanID;
                    //PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
                    //lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);
                    //if (lst.Count == 0)
                    //{
                    //    strHQL = "Insert Into T_Plan_RelatedLeader (PlanID,LeaderCode,LeaderName ,JoinTime ,Actor,Status )";
                    //    strHQL += " Select " + strPlanID + ",LeaderCode,LeaderName ,now() ,Actor,'New' From T_Plan_RelatedLeader ";
                    //    strHQL += " Where PlanID in (Select MAX(PlanID) From T_Plan Where UserCode = '" + strPlanUserCode + "' and PlanID <> " + strNewPlanID + ")";
                    //    strHQL += " And LeaderCode Not In (Select LeaderCode From T_Plan_RelatedLeader Where PlanID = " + strNewPlanID + ")";
                    //    ShareClass.RunSqlCommand(strHQL);
                    //}
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZWC").ToString().Trim() + "')", true);
    }

    protected void BT_DeletePlanToAllSystemUser_Click(object sender, EventArgs e)
    {
        string strHQL1, strHQL2;
        string strPlanID;


        strPlanID = LB_PlanID.Text.Trim();

        strHQL1 = "Delete From T_Plan Where BackupPlanID = " + strPlanID + " and UserCode <> " + "'" + strUserCode + "'";

        try
        {
            ShareClass.RunSqlCommand(strHQL1);

            strHQL2 = "Delete From T_PlanCopyRelatedUser Where PlanID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL2);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);

            LoadPlanCopyRelatedUser("0");
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string struserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string struserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

        string strPlanID = LB_PlanID.Text.Trim();

        if (strPlanID != "")
        {
            string strHQL = "Select * From T_PlanCopyRelatedUser Where UserCode = " + "'" + struserCode + "'" + " and PlanID = " + strPlanID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_PlanCopyRelatedUser");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYCZBNZFZJ").ToString().Trim() + "')", true);
            }
            else
            {
                strHQL = "Insert Into T_PlanCopyRelatedUser(PlanID,UserCode,UserName) Values(" + strPlanID + "," + "'" + struserCode + "'" + "," + "'" + struserName + "'" + ")";
                ShareClass.RunSqlCommand(strHQL);

                LoadPlanCopyRelatedUser(strPlanID);
            }
        }

        //ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowPlanMember','false') ", true);
    }

    protected void RT_ActorGroup_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strPlanID, strGroupName;
            string strHQL;

            strPlanID = LB_PlanID.Text.Trim();

            if (strPlanID != "")
            {
                strGroupName = ((Button)e.Item.FindControl("BT_GroupName")).Text.Trim();

                strHQL = "Insert Into T_PlanCopyRelatedUser(PlanID,UserCode,UserName)";
                strHQL += "Select " + strPlanID + ",UserCode,UserName From T_ActorGroupDetail Where GroupName = " + "'" + strGroupName + "'";
                strHQL += " and UserCode not in (Select UserCode From T_PlanCopyRelatedUser Where PlanID = " + strPlanID + ")";
                ShareClass.RunSqlCommand(strHQL);

                LoadPlanCopyRelatedUser(strPlanID);
            }

            //ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindowActorGroup','false') ", true);
        }
    }

    protected void LoadKPI(string strDepartCode, string strPosition)
    {
        string strHQL;
        IList lst;

        strHQL = "From KPITemplateForDepartPosition as kpiTemplateForDepartPosition where kpiTemplateForDepartPosition.DepartCode = " + "'" + strDepartCode + "'" + " and kpiTemplateForDepartPosition.Position = " + "'" + strPosition + "'";
        strHQL += " Order by kpiTemplateForDepartPosition.ID ASC";

        KPITemplateForDepartPositionBLL kpiTemplateForDepartPositionBLL = new KPITemplateForDepartPositionBLL();
        lst = kpiTemplateForDepartPositionBLL.GetAllKPITemplateForDepartPositions(strHQL);

        DL_UserKPI.DataSource = lst;
        DL_UserKPI.DataBind();
    }

    protected int GetSameNamePlanCount(string strUserCode, string strPlanName)
    {
        string strHQL;
        IList lst;

        strHQL = "from Plan as plan where plan.UserCode = " + "'" + strUserCode + "'";
        strHQL += " and plan.PlanName = " + "'" + strPlanName + "'";

        strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        return lst.Count;
    }

    protected int GetSameNamePlanCount(string strUserCode, string strPlanName, string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Plan as plan where plan.UserCode = " + "'" + strUserCode + "'";
        strHQL += " and plan.PlanName = " + "'" + strPlanName + "'";
        strHQL += " and plan.PlanID != " + strPlanID;
        strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        return lst.Count;
    }

    protected void LoadPlanCopyRelatedUser(string strPlanID)
    {
        string strHQL;

        strHQL = "Select * From T_PlanCopyRelatedUser Where PlanID = " + strPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_PlanCopyRelatedUser");

        RP_Attendant.DataSource = ds;
        RP_Attendant.DataBind();
    }

    protected void LoadPlanType()
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanType as planType Order By planType.SortNumber ASC";
        PlanTypeBLL planTypeBLL = new PlanTypeBLL();
        lst = planTypeBLL.GetAllPlanTypes(strHQL);

        DL_PlanType.DataSource = lst;
        DL_PlanType.DataBind();
    }

    protected void LoadPlanTarget(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanTarget as planTarget Where planTarget.PlanID = " + strPlanID;
        PlanTargetBLL planTargetBLL = new PlanTargetBLL();
        lst = planTargetBLL.GetAllPlanTargets(strHQL);

        DataGrid1.DataSource = lst;
        DataGrid1.DataBind();
    }

    protected void LoadPlanLeader(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.PlanID = " + strPlanID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void LoadActorGroup(string strUserCode, string strDepartString, string strLangCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName not in ('Individual','Department','Company','Group','All')";  
        strHQL += " and (actorGroup.BelongDepartCode in " + strDepartString;
        strHQL += " Or actorGroup.MakeUserCode = " + "'" + strUserCode + "'" + ")";
        strHQL += " and actorGroup.LangCode = '" + strLangCode + "'";
        ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
        lst = actorGroupBLL.GetAllActorGroups(strHQL);
        RT_ActorGroup.DataSource = lst;
        RT_ActorGroup.DataBind();
    }


    protected int GetParentBackupPlanID(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);
        if (lst.Count > 0)
        {
            Plan plan = (Plan)lst[0];

            return plan.BackupPlanID;
        }
        else
        {
            return int.Parse(strPlanID);
        }
    }

    protected int GetPlanRelatedLeaderCountByActiveFinish(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.Status in ('Approved','Completed')";
        strHQL += " and planRelatedLeader.PlanID = " + strPlanID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

        return lst.Count;
    }

    protected int GetPlanWorkLogCountByActiveFinish(string strPlanID)
    {
        string strHQL;
        IList lst;

        strHQL = "From PlanWorkLog as planWorkLog Where PlanID = " + strPlanID;
        strHQL += " Order By planWorkLog.ID DESC";
        PlanWorkLogBLL planWorkLogBLL = new PlanWorkLogBLL();
        lst = planWorkLogBLL.GetAllPlanWorkLogs(strHQL);

        return lst.Count;
    }

    protected string GetPlanNameByPlanID(string strPlanID)
    {
        string strHQL;
        IList lst;

        if (strPlanID != "0")
        {
            strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
            PlanBLL planBLL = new PlanBLL();
            lst = planBLL.GetAllPlans(strHQL);

            if (lst.Count > 0)
            {
                Plan plan = new Plan();

                plan = (Plan)lst[0];

                return plan.PlanName.Trim();
            }
            else
            {
                return strUserName + LanguageHandle.GetWord("DeJiHua").ToString().Trim();
            }
        }
        else
        {
            return strUserName + LanguageHandle.GetWord("DeJiHua").ToString().Trim();
        }
    }


}
