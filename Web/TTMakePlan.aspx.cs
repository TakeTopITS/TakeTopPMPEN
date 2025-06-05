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
using System.Data.SqlClient;
using System.IO;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMakePlan : System.Web.UI.Page
{
    //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
    string strRelatedType, strRelatedID, strRelatedCode;
    string strUserCode, strUserName;
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strDepartCode, strPosition;

        //Session["UserCode"] = "C0001";
        //Session["IsMobileDevice"] = "NO";

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_PlanDetail);
HE_PlanDetail.Language = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

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
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickB", "setTreeviewHigh();", true);
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

            LoadMyPlan(strUserCode);

            //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
            ShareClass.InitialPlanTreeByUserCode(TreeView1, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
            ShareClass.InitialPlanTreeByUserCode(TreeView2, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"),TreeView3, strUserCode);
            LoadPlanType();

            strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            strPosition = ShareClass.GetUserDuty(strUserCode);

            LoadKPI(strDepartCode, strPosition);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strPlanID, strPlanName, strParentID, strCreatorCode;
        int intLeaderReviewCount, intWorkLogCount;
        int intChildCount;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strPlanID = treeNode.Target.Trim();
        strPlanName = treeNode.Text.Trim();

        strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);
        
        if (lst.Count > 0)
        {
            Plan plan = (Plan)lst[0];

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

            strUserCode = plan.UserCode.Trim();
            strCreatorCode = plan.CreatorCode.Trim();

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
            LB_SubmitTime.Text = plan.SubmitTime;

            if (plan.Status.Trim() == "New")
            {
                DL_Status.SelectedValue = "Pending Review";
            }

            intLeaderReviewCount = GetPlanRelatedLeaderCountByActiveFinish(strPlanID);
            intWorkLogCount = GetPlanWorkLogCountByActiveFinish(strPlanID);

            intChildCount = treeNode.ChildNodes.Count;

            BT_SubmitApprove.Visible = true;

            if (intLeaderReviewCount == 0 & intWorkLogCount == 0)
            {
                BT_SubmitApprove.Enabled = true;
            }
            else
            {
                DL_Status.Enabled = false;
                BT_SubmitApprove.Enabled = false;
            }


            if (intChildCount > 0)
            {
                DL_Status.Enabled = false;
            }

            LoadPlanTarget(strPlanID);
            LoadPlanLeader(strPlanID);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
            strPlanName = strUserName + LanguageHandle.GetWord("DeJiHua");

            LB_SelectedPlanID.Text = "";
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strPlanID, strParentID, strCreatorCode;
        int intLeaderReviewCount, intWorkLogCount;

        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strPlanID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update" || e.CommandName == "Target" || e.CommandName == "Leader")
            {
                for (int i = 0; i < DataGrid4.Items.Count; i++)
                {
                    DataGrid4.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From Plan as plan Where plan.PlanID = " + strPlanID;
                PlanBLL planBLL = new PlanBLL();
                lst = planBLL.GetAllPlans(strHQL);
                
                if (lst.Count > 0)
                {
                    Plan plan = (Plan)lst[0];

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

                    strUserCode = plan.UserCode.Trim();
                    strCreatorCode = plan.CreatorCode.Trim();

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
                    LB_SubmitTime.Text = plan.SubmitTime;

                    if (plan.Status.Trim() == "New")
                    {
                        DL_Status.SelectedValue = "Pending Review";
                    }

                    intLeaderReviewCount = GetPlanRelatedLeaderCountByActiveFinish(strPlanID);
                    intWorkLogCount = GetPlanWorkLogCountByActiveFinish(strPlanID);

                    BT_SubmitApprove.Visible = true;

                    if (intLeaderReviewCount == 0 & intWorkLogCount == 0)
                    {
                        BT_SubmitApprove.Enabled = true;
                    }
                    else
                    {
                        DL_Status.Enabled = false;

                        BT_SubmitApprove.Enabled = false;
                    }


                    LoadPlanTarget(strPlanID);
                    LoadPlanLeader(strPlanID);
                }

                if (e.CommandName == "Update")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }

                if (e.CommandName == "Target")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popTargetWindow','true') ", true);
                }

                if (e.CommandName == "Leader")
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true') ", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                strHQL = "Select * From T_Plan Where PlanID in (Select ParentID From T_Plan) and PlanID = " + strPlanID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Plan");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZZJHBNSCJC") + "')", true);
                    return;
                }

                Plan plan = GetPlan(strPlanID);
                if (plan.CreatorCode.Trim() != strUserCode)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFZGLDJHBNSCJC") + "')", true);
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

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_PlanID.Text = "";
        DL_Status.SelectedValue = "New";

        BT_SubmitApprove.Visible = false;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strPlanID, strStatus;

        strPlanID = LB_PlanID.Text.Trim();
        strStatus = DL_Status.SelectedValue;

        if (strPlanID == "")
        {
            AddPlan();
        }
        else
        {
            UpdatePlan();
        }
    }


    protected void AddPlan()
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZTMDJHBNXZJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        if (strParentID == "" | strPlanType == "" | strPlanName == "" | strPlanDetail == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBFJHJHLXJHMCHJHNRDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

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

            LoadPlanTarget(strPlanID);
            LoadPlanLeader(strPlanID);

            DL_Status.SelectedValue = "Pending Review";


            BT_SubmitApprove.Enabled = true;

            LoadMyPlan(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdatePlan()
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

        string strSelectedPlanID = LB_SelectedPlanID.Text.Trim();
        if (strSelectedPlanID == "")
        {
            strSelectedPlanID = "0";
        }

        dtStartTime = DateTime.Parse(DLC_StartTime.Text);
        dtEndTime = DateTime.Parse(DLC_EndTime.Text);

        strParentID = LB_ParentPlanID.Text.Trim();

        if (strParentID == strPlanID)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZJBNZWZJDFJHJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        intCount = GetSameNamePlanCount(strUserCode, strPlanName, strPlanID);
        if (intCount > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZTMDJHBNXGJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        if (strParentID == "" | strPlanType == "" | strPlanName == "" | strPlanDetail == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBFJHJHLXJHMCHJHNRDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        intParentID = int.Parse(strSelectedPlanID);

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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);

                LoadMyPlan(strUserCode);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            }
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
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBFJHJHLXJHMCHJHNRDBNWKJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        strHQL = "From PlanRelatedLeader as planRelatedLeader where planRelatedLeader.PlanID = " + strPlanID;
        PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
        lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);
        if (lst.Count == 0)
        {
            strHQL = "Insert Into T_Plan_RelatedLeader (PlanID,LeaderCode,LeaderName ,JoinTime ,Actor,Status )";
            strHQL += " Select " + strPlanID + ",LeaderCode,LeaderName ,now() ,Actor,'New' From T_Plan_RelatedLeader ";
            strHQL += " Where PlanID in (Select MAX(PlanID) From T_Plan Where UserCode = '" + strUserCode + "' and PlanID <> " + strPlanID + ")";
            strHQL += " And LeaderCode Not In (Select LeaderCode From T_Plan_RelatedLeader Where PlanID = " + strPlanID + ")";
            ShareClass.RunSqlCommand(strHQL);
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
            plan.SubmitTime = DateTime.Now.ToString();

            //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
            plan.RelatedType = strRelatedType;
            int intRelatedID = 0;
            int.TryParse(strRelatedID, out intRelatedID);
            plan.RelatedID = intRelatedID;
            plan.RelatedCode = strRelatedCode;

            plan.Status = "Pending Review";

            try
            {
                planBLL.UpdatePlan(plan, int.Parse(strPlanID));

                LB_SubmitTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DL_Status.SelectedValue = "Pending Review";
                //加上关联RelatedID,RelatedType,RelatedCode TODO:CAOJIAN(曹健)
                ShareClass.InitialPlanTreeByUserCode(TreeView1, strUserCode, strRelatedType, strRelatedID, strRelatedCode);
                ShareClass.InitialPlanTreeByUserCode(TreeView2, strUserCode, strRelatedType, strRelatedID, strRelatedCode);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSHCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZTJSHSBJC") + "')", true);
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;
        string strPlanID, strTargetID;
        int intCount;

        strPlanID = LB_PlanID.Text.Trim();

        ShareClass.ColorDataGridSelectRow(DataGrid1, e);

        strTargetID = e.Item.Cells[2].Text.Trim();

        if (e.CommandName == "Update")
        {
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
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popTargetWindow','true','popTargetDetailWindow') ", true);
        }

        if (e.CommandName == "Delete")
        {
            strPlanID = LB_PlanID.Text.Trim();

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

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popTargetWindow','true') ", true);
        }
    }


    protected void DL_UserKPI_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strKPI;

        strKPI = DL_UserKPI.SelectedValue.Trim();

        TB_Target.Text = strKPI;

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popTargetWindow','true','popTargetDetailWindow') ", true);
    }


    protected void BT_CreateTarget_Click(object sender, EventArgs e)
    {
        LB_TargetID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popTargetWindow','true','popTargetDetailWindow') ", true);
    }

    protected void BT_NewTarget_Click(object sender, EventArgs e)
    {
        string strTargetID;

        strTargetID = LB_TargetID.Text.Trim();

        if (strTargetID == "")
        {
            AddTarget();
        }
        else
        {
            UpdateTarget();
        }
    }


    protected void AddTarget()
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


            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popTargetWindow','true') ", true);

    }

    protected void UpdateTarget()
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

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popTargetWindow','true') ", true);
    }

    protected void LBT_CloseTargetDetailWindow_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popTargetWindow','true') ", true);
    }


    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strPlanID, strLeaderID;
        int intCount;

        ShareClass.ColorDataGridSelectRow(DataGrid2, e);

        strPlanID = LB_PlanID.Text.Trim();

        strLeaderID = e.Item.Cells[2].Text.Trim();

        if (e.CommandName == "Update")
        {
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

                }
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true','popLeaderDetailWindow') ", true);
        }

        if (e.CommandName == "Delete")
        {
            strPlanID = LB_PlanID.Text.Trim();

            strHQL = "From PlanRelatedLeader as planRelatedLeader Where planRelatedLeader.ID = " + strLeaderID;
            PlanRelatedLeaderBLL planRelatedLeaderBLL = new PlanRelatedLeaderBLL();
            lst = planRelatedLeaderBLL.GetAllPlanRelatedLeaders(strHQL);

            PlanRelatedLeader planRelatedLeader = new PlanRelatedLeader();

            planRelatedLeader = (PlanRelatedLeader)lst[0];

            try
            {
                planRelatedLeaderBLL.DeletePlanRelatedLeader(planRelatedLeader);
                LoadPlanLeader(strPlanID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true') ", true);
        }
    }

    protected void BT_CreateLeader_Click(object sender, EventArgs e)
    {
        LB_LeaderID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true','popLeaderDetailWindow') ", true);
    }

    protected void BT_NewLeader_Click(object sender, EventArgs e)
    {
        string strLeaderID;

        strLeaderID = LB_LeaderID.Text.Trim();

        if (strLeaderID == "")
        {
            AddLeader();
        }
        else
        {
            UpdateLeader();
        }
    }

    protected void AddLeader()
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

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true') ", true);

    }

    protected void UpdateLeader()
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

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true') ", true);

    }

    protected void LBT_CloseLeaderDetailWindow_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true') ", true);
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true','popLeaderDetailWindow') ", true);

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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popLeaderWindow','true','popLeaderDetailWindow') ", true);

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

    protected void LoadMyPlan(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Plan as plan where plan.UserCode = " + "'" + strUserCode + "'";
        strHQL += " and plan.Status not in ('Deleted','Archived') ";
        strHQL += " Order By plan.StartTime DESC,plan.EndTime ASC";
        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql4.Text = strHQL;
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid3.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql4.Text;
        IList lst;

        PlanBLL planBLL = new PlanBLL();
        lst = planBLL.GetAllPlans(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
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
                return strUserName + LanguageHandle.GetWord("DeJiHua");
            }
        }
        else
        {
            return strUserName + LanguageHandle.GetWord("DeJiHua");
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

}
