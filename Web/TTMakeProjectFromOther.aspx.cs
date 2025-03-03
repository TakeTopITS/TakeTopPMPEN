using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTMakeProjectFromOther : System.Web.UI.Page
{
    string strRelatedType, strRelatedID;
    string strIsMobileDevice, strProjectUserCode;
    string strLangCode;


    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode, strUserName;
        string strProjectType;

        strLangCode = Session["LangCode"].ToString();
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_AcceptStandard);
        _FileBrowser.SetupCKEditor(HE_ProjectDetail);

        strRelatedType = Request.QueryString["RelatedType"];
        strRelatedID = Request.QueryString["RelatedID"];

        strProjectUserCode = Session["UserCode"].ToString().Trim();
        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        if (strRelatedType == "Customer")
        {
            //this.Title = "Customer" + ": " + strRelatedID + " " + GetCustomerName(strRelatedID) + " 转项目";
        }

        if (strRelatedType == "Constract")
        {
            //this.Title = LanguageHandle.GetWord("GeTong").ToString().Trim() + ": " + GetConstractCode(strRelatedID) + " " + GetConstractName(strRelatedID) + " 转项目";
        }

        if (strRelatedType == "tender")
        {
            //this.Title = LanguageHandle.GetWord("QiaoBiao").ToString().Trim() + ": " + strRelatedID + " " + GetBMAnnInvitationName(strRelatedID) + " 转项目";
        }

        if (strRelatedType == "Other")
        {
            strRelatedType = "Other";
            //this.Title = "建立和分派项目";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_ProjectDetail.Visible = true;
                HT_AcceptStandard.Visible = true;

                if (strRelatedType == "tender")
                {
                    HT_ProjectDetail.Text = LanguageHandle.GetWord("YaoQingHanNeiRong").ToString().Trim() + GetBMAnnInvitationRemark(strRelatedID) + LanguageHandle.GetWord("ZhongBiaoXinXi").ToString().Trim() + GetSupplierBidName(strRelatedID);
                }
            }
            else
            {
                HE_ProjectDetail.Visible = true;
                HE_AcceptStandard.Visible = true;

                if (strRelatedType == "tender")
                {
                    HE_ProjectDetail.Text = LanguageHandle.GetWord("YaoQingHanNeiRong").ToString().Trim() + GetBMAnnInvitationRemark(strRelatedID) + LanguageHandle.GetWord("ZhongBiaoXinXi").ToString().Trim() + GetSupplierBidName(strRelatedID);
                }
            }

            DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ShareClass.InitialPrjectTreeByAuthority(TreeView1, strUserCode);

            string strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityProjectLeader(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);
            ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_PM);
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityProjectLeader(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView3, strUserCode);

            ShareClass.LoadCurrencyType(DL_CurrencyType);

            ShareClass.LoadProjectType(DL_ProjectType);

            if (strRelatedType == "Customer")
            {
                TB_ProjectName.Text = "Customer" + ": " + strRelatedID + " " + GetCustomerName(strRelatedID) + LanguageHandle.GetWord("XiangMu").ToString().Trim();
                LoadCustomerRelatedProject(strRelatedID);
            }
            if (strRelatedType == "Constract")
            {
                TB_ProjectName.Text = LanguageHandle.GetWord("GeTong").ToString().Trim() + ": " + GetConstractCode(strRelatedID) + " " + GetConstractName(strRelatedID) + LanguageHandle.GetWord("XiangMu").ToString().Trim();
                LoadConstractRelatedProject(GetConstractCode(strRelatedID));
            }
            if (strRelatedType == "tender")
            {
                TB_ProjectName.Text = LanguageHandle.GetWord("QiaoBiao").ToString().Trim() + ": " + strRelatedID + " " + GetBMAnnInvitationName(strRelatedID) + LanguageHandle.GetWord("XiangMu").ToString().Trim();
                LoadBMAnnInvitRelatedProject(strRelatedID);
            }

            strProjectType = DL_ProjectType.SelectedItem.Text.Trim();
            ShareClass.LoadProjectForPMStatus(strProjectType, strLangCode,DL_Status);

            LB_BelongDepartCode.Text = ShareClass.GetDepartCodeFromUserCode(strUserCode);
            LB_BelongDepartName.Text = ShareClass.GetDepartName(LB_BelongDepartCode.Text.Trim());

            if (ShareClass.GetCodeRuleStatusByType("ProjectCode") == "YES")
            {
                TB_ProjectCode.Text = ShareClass.GetCodeByRule("ProjectCode", "", "0");
            }

            //BusinessForm,列出业务表单类型 
            ShareClass.LoadWorkflowType(DL_WLType, strLangCode);

            //列出可用的工作流模板
            ShareClass.LoadProjectPlanStartupRelatedWorkflowTemplate(strUserCode, DL_PlanStartupRelatedWorkflowTemplate);
        }
    }

    protected string GetBMAnnInvitationName(string strBMAnnInvitID)
    {
        string strHQL = "from BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.ID = " + strBMAnnInvitID;
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        if (lst != null && lst.Count > 0)
        {
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
            return bMAnnInvitation.Name.Trim();
        }
        else
            return "";
    }

    /// <summary>
    /// 招标邀请函内容
    /// </summary>
    /// <param name="strBMAnnInvitID"></param>
    /// <returns></returns>
    protected string GetBMAnnInvitationRemark(string strBMAnnInvitID)
    {
        string strHQL = "from BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.ID = " + strBMAnnInvitID;
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        if (lst != null && lst.Count > 0)
        {
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
            return bMAnnInvitation.Remark.Trim();
        }
        else
            return "";
    }

    /// <summary>
    /// 招标招标结果
    /// </summary>
    /// <param name="strBMAnnInvitID"></param>
    /// <returns></returns>
    protected string GetSupplierBidName(string strBMAnnInvitID)
    {
        string result = "";
        string strHQL = "From BMSupplierBid as bMSupplierBid Where bMSupplierBid.AnnInvitationID='" + strBMAnnInvitID + "' and bMSupplierBid.BidStatus='Y' ";
        BMSupplierBidBLL bMSupplierBidBLL = new BMSupplierBidBLL();
        IList lst = bMSupplierBidBLL.GetAllBMSupplierBids(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                BMSupplierBid bMSupplierBid = (BMSupplierBid)lst[i];
                strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + bMSupplierBid.SupplierCode.ToString() + "' ";
                BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                if (lst.Count > 0 && lst != null)
                {
                    BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                    result += "(" + bMSupplierInfo.Code.Trim() + ")" + bMSupplierInfo.Name.Trim() + ";";
                }
            }
            return result;
        }
        else
            return ""+LanguageHandle.GetWord("WeiKaiBiao")+"！";   
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strProjectID, strProjectName, strHQL;
        string strUserName;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        strProjectID = treeNode.Target.Trim();

        ProjectBLL projectBLL = new ProjectBLL();
        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strUserName = LB_UserName.Text.Trim();
        strProjectName = project.ProjectName.Trim();

        LB_ParentProjectID.Text = project.ProjectID.ToString();
        TB_ParentProject.Text = project.ProjectName.Trim();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strHQL;
        IList lst;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
            strHQL += " Order By projectMember.SortNumber DESC";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DL_PM.DataSource = lst;
            DL_PM.DataBind();
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            LB_BelongDepartCode.Text = strDepartCode;
            LB_BelongDepartName.Text = ShareClass.GetDepartName(strDepartCode);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_MyMember_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();

        ShareClass.LoadMemberByUserCodeForDropDownList(strUserCode, DL_PM);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Find_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strProjectName, strPMName;

        strProjectName = TB_FindProjectName.Text.Trim();
        strProjectName = "%" + strProjectName + "%";

        strPMName = TB_FindPMName.Text.Trim();
        strPMName = "%" + strPMName + "%";

        strHQL = "From Project as project Where project.ProjectName Like  " + "'" + strProjectName + "'" + " and project.PMName Like " + "'" + strPMName + "'";
        strHQL += " And project.UserCode = " + "'" + LB_UserCode.Text + "'";
        strHQL += " Order By project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        //BusinessForm，隐藏业务表单元素
        Panel_RelatedBusiness.Visible = false;

        LB_ProjectID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    //BusinessForm，判断后续是否可以改表单内容
    protected void DL_AllowUpdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        string strProjectID, strAllowUpdate;

        strAllowUpdate = DL_AllowUpdate.SelectedValue;
        strProjectID = LB_ProjectID.Text.Trim();

        try
        {
            strHQL = "Update T_RelatedBusinessForm Set AllowUpdate = '" + strAllowUpdate + "'  Where RelatedType = 'Project' and RelatedID = " + strProjectID;
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGGSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strProjectID;

        strProjectID = LB_ProjectID.Text.Trim();

        if (strProjectID == "")
        {
            //如果自动产生客户编码，禁用客户代码输入框 
            if (ShareClass.GetCodeRuleStatusByType("ProjectCode") == "YES")
            {
                TB_ProjectCode.Enabled = false;
                TB_ProjectCode.Text = "PJ" + DateTime.Now.ToString("yyyyMMddHHMMss");
            }

            AddProject();
        }
        else
        {
            UpdateProject();
        }
    }

    protected void AddProject()
    {
        string strHQL;

        string strProjectID, strProjectCode;
        string strPMCode, strCustomerPMName, strUserCode, strUserName, strStatus, strStatusValue, strBeginDate;
        string strEndDate, strProject, strDetail, strAcceptStandard, strParentID, strPriority;
        string strBudget = NB_Budget.Amount.ToString();
        string strProjectType;
        decimal deProjectAmount, deManHour, deManNumber;

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();
        strPMCode = DL_PM.SelectedValue.Trim();
        strCustomerPMName = TB_CustomerPMName.Text.Trim();

        strBeginDate = DLC_BeginDate.Text.ToString();
        strEndDate = DLC_EndDate.Text;
        strProjectCode = TB_ProjectCode.Text.Trim();
        strProject = TB_ProjectName.Text.Trim();
        strProjectType = DL_ProjectType.SelectedValue.Trim();
        strBudget = NB_Budget.Amount.ToString();

        if (strIsMobileDevice == "YES")
        {
            strDetail = HT_ProjectDetail.Text.Trim();
            strAcceptStandard = HT_AcceptStandard.Text.Trim();
        }
        else
        {
            strDetail = HE_ProjectDetail.Text.Trim();
            strAcceptStandard = HE_AcceptStandard.Text.Trim();
        }

        strParentID = LB_ParentProjectID.Text.Trim();
        deProjectAmount = NB_ProjectAmount.Amount;
        deManHour = NB_ManHour.Amount;
        deManNumber = NB_ManNubmer.Amount;
        strStatus = DL_Status.SelectedValue;
        strStatusValue = DL_StatusValue.SelectedValue.Trim();

        strPriority = DL_Priority.SelectedValue.Trim();

        if (strBudget == "")
        {
            strBudget = "0";
        }

        if (strProjectCode != "")
        {
            if (ShareClass.GetProjecCountByProjectCodeAndID(strProjectCode, "0") > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZXTDXMDMDXMJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                return;
            }
        }

        if (strParentID == "")
        {
            strParentID = "1";
        }

        if (strPMCode == "" | strStatus == "" | strBeginDate == "" | strEndDate == "" | strProject == "" | strDetail == "" | strAcceptStandard == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
        else
        {
            ProjectBLL projectBLL = new ProjectBLL();
            projectBLL = new ProjectBLL();
            Project project = new Project();

            project.ProjectCode = strProjectCode;
            project.UserCode = strUserCode;
            project.UserName = LB_UserName.Text.Trim();
            project.PMCode = strPMCode;
            project.PMName = ShareClass.GetUserName(strPMCode);
            project.CustomerPMName = strCustomerPMName;
            project.ProjectName = strProject;
            project.ProjectType = strProjectType;
            project.ProjectClass = "NormalProject";
            project.ProjectAmount = NB_ProjectAmount.Amount;
            project.Budget = decimal.Parse(strBudget);
            project.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            project.ManHour = deManHour;
            project.ManNumber = deManNumber;
            project.ProjectDetail = strDetail;
            project.AcceptStandard = strAcceptStandard;
            project.BeginDate = DateTime.Parse(strBeginDate);
            project.EndDate = DateTime.Parse(strEndDate);
            project.MakeDate = DateTime.Now;
            project.Status = strStatus;
            project.StatusValue = "InProgress";
            project.ParentID = int.Parse(strParentID);

            project.Priority = strPriority;

            project.BelongDepartCode = LB_BelongDepartCode.Text.Trim();
            project.BelongDepartName = LB_BelongDepartName.Text.Trim();

            try
            {
                projectBLL.AddProject(project);

                strProjectID = ShareClass.GetMyCreatedMaxProjectID(strUserCode);
                LB_ProjectID.Text = strProjectID;
               
                //更改项目其它字段值 
                UpdateProjectOtherFieldValue(strProjectID);

                string strNewProjectCode = ShareClass.GetCodeByRule("ProjectCode", strProjectType, strProjectID);
                if (strNewProjectCode != "")
                {
                    TB_ProjectCode.Text = strNewProjectCode;
                    strHQL = "Update T_Project Set ProjectCode = " + "'" + strNewProjectCode + "'" + " Where ProjectID = " + strProjectID;
                    ShareClass.RunSqlCommand(strHQL);
                }


                CB_SMS.Enabled = true;
                CB_Mail.Enabled = true;
                BT_Send.Enabled = true;

                //2013-08-26 LiuJianping
                HL_ProjectTask.Enabled = true;
                HL_ProjectTask.NavigateUrl = "TTProjectPrimaveraTask.aspx?ProjectID=" + strProjectID;//end

                HL_ProjectCostManageEdit.Enabled = true;
                HL_ProjectCostManageEdit.NavigateUrl = "TTProjectCostManageEdit.aspx?ProjectID=" + strProjectID;

                TB_Message.Text = strUserName + LanguageHandle.GetWord("GeiNiJianLiLeXiangMu").ToString().Trim() + strProjectID + " " + strProject + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();

                //插入项目关联表
                InsertIntoRelatedProject(strProjectID);

                if (strRelatedType == "Customer")
                {
                    LoadCustomerRelatedProject(strRelatedID);

                    try
                    {
                        strHQL = "Update T_ProExpense Set ProjectID = " + strProjectID + " Where QuestionID in (Select QuestionID  From T_CustomerRelatedQuestion Where CustomerCode = " + "'" + strRelatedID + "'" + ")";
                        ShareClass.RunSqlCommand(strHQL);
                    }
                    catch
                    {
                    }
                }

                if (strRelatedType == "Constract")
                {
                    LoadConstractRelatedProject(GetConstractCode(strRelatedID));

                    //把合同物料清单转入项目关联物资
                    TransfertConstractGoodsToProjectRelatedItem(strProjectID, GetConstractCode(strRelatedID));
                }

                if (strRelatedType == "Other")
                {
                    LoadProject(strUserCode);
                }

                if (strRelatedType == "tender")
                {
                    ModifyRelatedUser(strRelatedID, strProjectID);
                    LoadBMAnnInvitRelatedProject(strRelatedID);
                }

                //依项目类型添加关联的工作流模板和文档模板
                ShareClass.AddRelatedWorkFlowTemplateByProjectType(strProjectType, strProjectID, "Project", "Project", "ProjectType");
                //ShareClass.AddRelatedDocumentTemplateByProjectType(strProjectType, strProjectID, "Project", "ProjectType");

                ShareClass.InitialPrjectTreeByAuthority(TreeView1, strUserCode);

                //判断立项后是不是自动发起流程
                if (GetProjectTypeAutoRunWFAfterMakeProject(strProjectType) == "YES")
                {
                    string strURL = "popShowByURL(" + "'TTRelatedDIYWorkflowForm.aspx?RelatedType=Project&RelatedID=" + strProjectID + "','" + LanguageHandle.GetWord("RunByWF").ToString().Trim() + "', 800, 600,window.location);";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop12", strURL, true);

                    //Response.Redirect("TTRelatedDIYWorkflowForm.aspx?RelatedType=Project&RelatedID=" + strProjectID);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
        }
    }

    protected string GetProjectTypeAutoRunWFAfterMakeProject(string strProjectType)
    {
        string strHQL;

        strHQL = "Select AutoRunWFAfterMakeProject From T_ProjectType Where Type = '" + strProjectType + "'";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectType");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "OtherProject";
        }
    }

    //把合同物料清单转入项目关联物资
    protected void TransfertConstractGoodsToProjectRelatedItem(string strProjectID, string strConstractCode)
    {
        string strHQL;

        strHQL = "Insert Into T_ProjectRelatedItem(ProjectID,ItemCode,ItemName,BomVersionID,Number,Unit,Status,ReservedNumber,DefaultProcess)";
        strHQL += " Select " + strProjectID + ",GoodsCode,GoodsName,1,Number,Unit,'New',Number,'' From T_ConstractRelatedGoods";
        strHQL += " Where ConstractCode = '" + strConstractCode + "' And GoodsCode Not in (Select ItemCode From T_ProjectRelatedItem Where ProjectID = " + strProjectID + ")";

        try
        {
            ShareClass.RunSqlCommand(strHQL);
        }
        catch
        {
        }
    }

    protected void UpdateProject()
    {
        string strUserCode, strCustomerPMName, strUserName, strStatus, strStatusValue, strBeginDate;
        string strEndDate, strProject, strDetail, strAcceptStandard, strParentID, strPriority;
        string strBudget;
        string strHQL;
        string strProjectID, strProjectCode;
        string strProjectType;
        IList lst;
        string strPMCode;
        decimal deProjectAmount, deManHour, deManNumber;

        string strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue;

        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        RelatedUser relatedUser = new RelatedUser();

        strUserCode = LB_UserCode.Text;
        strUserName = LB_UserName.Text;
        strPMCode = DL_PM.SelectedValue.Trim();
        strCustomerPMName = TB_CustomerPMName.Text.Trim();

        strBeginDate = DLC_BeginDate.Text;
        strEndDate = DLC_EndDate.Text;
        strProjectCode = TB_ProjectCode.Text.Trim();
        strProject = TB_ProjectName.Text.Trim();
        strProjectType = DL_ProjectType.SelectedValue.Trim();


        if (strIsMobileDevice == "YES")
        {
            strDetail = HT_ProjectDetail.Text.Trim();
            strAcceptStandard = HT_AcceptStandard.Text.Trim();
        }
        else
        {
            strDetail = HE_ProjectDetail.Text.Trim();
            strAcceptStandard = HE_AcceptStandard.Text.Trim();
        }

        deProjectAmount = NB_ProjectAmount.Amount;
        deManHour = NB_ManHour.Amount;
        deManNumber = NB_ManNubmer.Amount;
        strStatus = DL_Status.SelectedValue.Trim();
        strStatusValue = DL_StatusValue.SelectedValue.Trim();

        strParentID = LB_ParentProjectID.Text.Trim();
        strProjectID = LB_ProjectID.Text;
        strBudget = NB_Budget.Amount.ToString();

        strNewStatus = DL_Status.SelectedValue.Trim();
        strNewStatusValue = DL_StatusValue.SelectedValue.Trim();

        strPriority = DL_Priority.SelectedValue.Trim();

        if (strProjectCode != "")
        {
            if (ShareClass.GetProjecCountByProjectCodeAndID(strProjectCode, strProjectID) > 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZXTDXMDMDXMJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                return;
            }
        }

        if (strPMCode == "" | strStatus == "" | strBeginDate == "" | strEndDate == "" | strProject == "" | strDetail == "" | strAcceptStandard == "" | strParentID == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSRHYXDBNWKJC").ToString().Trim() + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
        else
        {
            strHQL = "from Project as project where project.ProjectID = " + strProjectID;
            ProjectBLL projectBLL = new ProjectBLL();
            lst = projectBLL.GetAllProjects(strHQL);
            Project project = (Project)lst[0];

            strOldStatus = project.Status.Trim();
            strOldStatusValue = project.StatusValue.Trim();

            project.ProjectCode = TB_ProjectCode.Text.Trim();
            project.UserCode = strUserCode;
            project.UserName = LB_UserName.Text.Trim();
            project.PMCode = strPMCode;
            project.PMName = ShareClass.GetUserName(strPMCode);
            project.CustomerPMName = strCustomerPMName;
            project.ProjectName = strProject;
            project.ProjectType = strProjectType;
            project.Budget = decimal.Parse(strBudget);

            try
            {
                project.CurrencyType = DL_CurrencyType.SelectedValue.Trim();
            }
            catch
            {

            }

            project.ProjectDetail = strDetail;
            project.AcceptStandard = strAcceptStandard;
            project.BeginDate = DateTime.Parse(strBeginDate);
            project.EndDate = DateTime.Parse(strEndDate);
            project.MakeDate = DateTime.Now;
            project.Status = strStatus;
            project.StatusValue = strStatusValue;
            project.ProjectAmount = deProjectAmount;
            project.ManHour = deManHour;
            project.ManNumber = deManNumber;

            project.Priority = strPriority;

            project.BelongDepartCode = LB_BelongDepartCode.Text.Trim();
            project.BelongDepartName = LB_BelongDepartName.Text.Trim();

            project.ParentID = int.Parse(strParentID);

            if (strProjectID == "")
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXXMBNXGJC").ToString().Trim() + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }
            else
            {
                try
                {
                    projectBLL.UpdateProject(project, int.Parse(strProjectID));

                    //更改项目其它字段值 
                    UpdateProjectOtherFieldValue(strProjectID);

                    //BusinessForm，关联相应的业务表单模板
                    ShareClass.SaveRelatedBusinessForm("Project", strProjectID, DL_WFTemplate.SelectedValue, DL_AllowUpdate.SelectedValue, strUserCode);

                    AddStatusChangeRecord(strProjectID, strOldStatus, strNewStatus, strOldStatusValue, strNewStatusValue);
                    LB_Status.Text = strNewStatus;

                    if (strRelatedType == "Customer")
                    {
                        LoadCustomerRelatedProject(strRelatedID);
                    }

                    if (strRelatedType == "Constract")
                    {
                        LoadConstractRelatedProject(GetConstractCode(strRelatedID));

                        //把合同物料清单转入项目关联物资
                        TransfertConstractGoodsToProjectRelatedItem(strProjectID, GetConstractCode(strRelatedID));
                    }

                    if (strRelatedType == "Other")
                    {
                        LoadProject(strUserCode);
                    }

                    if (strRelatedType == "tender")
                    {
                        LoadBMAnnInvitRelatedProject(strRelatedID);
                    }

                    //依项目类型添加关联的工作流模板和文档模板
                    ShareClass.AddRelatedWorkFlowTemplateByProjectType(strProjectType, strProjectID, "Project", "Project", "ProjectType");
                    //ShareClass.AddRelatedDocumentTemplateByProjectType(strProjectType, strProjectID, "Project", "ProjectType");

                    ShareClass.InitialPrjectTreeByAuthority(TreeView1, strUserCode);

                    TB_Message.Text = strUserName + LanguageHandle.GetWord("GengXinLeXiangMu").ToString().Trim() + strProjectID + " " + strProject + LanguageHandle.GetWord("DeNeiRongQingGuanZhuTeCiTongZh").ToString().Trim();

                    LB_Sql.Text = strHQL;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB").ToString().Trim() + "')", true);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }
            }
        }
    }

    protected void DL_ProjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strType;

        strType = DL_ProjectType.SelectedValue.Trim();

        ShareClass.LoadProjectForPMStatus(strType,strLangCode,DL_Status);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    //更改项目其它字段值 
    public void UpdateProjectOtherFieldValue(string strProjectID)
    {
        string strHQL;

        string strLockStartupedPlan;
        strLockStartupedPlan = DL_LockStartupedPlan.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set LockStartupedPlan = '{0}' Where ProjectID = {1}", strLockStartupedPlan, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strAllowPMChangeStatus;
        strAllowPMChangeStatus = DL_AllowPMChangeStatus.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set AllowPMChangeStatus = '{0}' Where ProjectID = {1}", strAllowPMChangeStatus, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strProgressByDetailImpact;
        strProgressByDetailImpact = DL_ProgressByDetailImpact.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set ProgressByDetailImpact = '{0}' Where ProjectID = {1}", strProgressByDetailImpact, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strPlanProgressNeedPlanerConfirm;
        strPlanProgressNeedPlanerConfirm = DL_PlanProgressNeedPlanerConfirm.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set PlanProgressNeedPlanerConfirm = '{0}' Where ProjectID = {1}", strPlanProgressNeedPlanerConfirm, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strAutoRunWFAfterMakeProject;
        strAutoRunWFAfterMakeProject = DL_AutoRunWFAfterMakeProject.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set AutoRunWFAfterMakeProject = '{0}' Where ProjectID = {1}", strAutoRunWFAfterMakeProject, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strProjectStartupNeedSupperConfirm;
        strProjectStartupNeedSupperConfirm = DL_ProjectStartupNeedSupperConfirm.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set ProjectStartupNeedSupperConfirm = '{0}' Where ProjectID = {1}", strProjectStartupNeedSupperConfirm, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strProjectPlanStartupStauts;
        strProjectPlanStartupStauts = DL_ProjectPlanStartupSatus.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set ProjectPlanStartupStatus = '{0}' Where ProjectID = {1}", strProjectPlanStartupStauts, strProjectID);
        ShareClass.RunSqlCommand(strHQL);

        string strProjectPlanStartupRelatedWorkflowTemplate;
        strProjectPlanStartupRelatedWorkflowTemplate = DL_PlanStartupRelatedWorkflowTemplate.SelectedValue.Trim();
        strHQL = string.Format(@"Update T_Project Set PlanStartupRelatedWorkflowTemplate = '{0}' Where ProjectID = {1}", strProjectPlanStartupRelatedWorkflowTemplate, strProjectID);
        ShareClass.RunSqlCommand(strHQL);
    }

    //设置项目其它属性的值 
    public void SetProjectOtherFieldValue(string strProjectID)
    {
        string strHQL;

        strHQL = string.Format(@"Select * From T_Project Where ProjectID={0}", strProjectID);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_Project");

        DL_LockStartupedPlan.SelectedValue = ds.Tables[0].Rows[0]["LockStartupedPlan"].ToString().Trim();
        DL_AllowPMChangeStatus.SelectedValue = ds.Tables[0].Rows[0]["AllowPMChangeStatus"].ToString().Trim();

        DL_ProgressByDetailImpact.SelectedValue = ds.Tables[0].Rows[0]["ProgressByDetailImpact"].ToString().Trim();
        DL_PlanProgressNeedPlanerConfirm.SelectedValue = ds.Tables[0].Rows[0]["PlanProgressNeedPlanerConfirm"].ToString().Trim();

        DL_AutoRunWFAfterMakeProject.SelectedValue = ds.Tables[0].Rows[0]["AutoRunWFAfterMakeProject"].ToString().Trim();
        DL_ProjectStartupNeedSupperConfirm.SelectedValue = ds.Tables[0].Rows[0]["ProjectStartupNeedSupperConfirm"].ToString().Trim();

        DL_ProjectPlanStartupSatus.SelectedValue = ds.Tables[0].Rows[0]["ProjectPlanStartupStatus"].ToString().Trim();

        DL_PlanStartupRelatedWorkflowTemplate.SelectedValue = ds.Tables[0].Rows[0]["PlanStartupRelatedWorkflowTemplate"].ToString().Trim();
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strProjectID, strHQL;
        string strUserCode, strUserName, strRelatedUserCode;
        IList lst;
        string strMsg, strSubject;

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        strProjectID = LB_ProjectID.Text.Trim();

        strHQL = "from RelatedUser as relatedUser where relatedUser.ProjectID = " + strProjectID;
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        lst = relatedUserBLL.GetAllRelatedUsers(strHQL);

        RelatedUser relatedUser = new RelatedUser();

        strMsg = TB_Message.Text.Trim();

        strSubject = LanguageHandle.GetWord("XiangMuTongZhi").ToString().Trim();

        Msg msg = new Msg();

        try
        {
            for (int i = 0; i < lst.Count; i++)
            {
                relatedUser = (RelatedUser)lst[i];
                strRelatedUserCode = relatedUser.UserCode.Trim();

                if (CB_SMS.Checked == true | CB_Mail.Checked == true)
                {
                    if (CB_SMS.Checked == true)
                    {
                        msg.SendMSM("Message",strRelatedUserCode, strMsg, strUserCode);
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

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strProjectID, strProjectName, strHQL, strStatus;
        string strUserName;

        if (e.CommandName != "Page")
        {
            strUserName = LB_UserName.Text.Trim();
            strProjectID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                try
                {
                    for (int i = 0; i < DataGrid2.Items.Count; i++)
                    {
                        DataGrid2.Items[i].ForeColor = Color.Black;
                    }

                    e.Item.ForeColor = Color.Red;

                    strHQL = "from Project as project where project.ProjectID = " + strProjectID;

                    ProjectBLL projectBLL = new ProjectBLL();
                    IList lst = projectBLL.GetAllProjects(strHQL);

                    Project project = (Project)lst[0];

                    strProjectName = project.ProjectName.Trim();

                    TB_ProjectCode.Text = project.ProjectCode.Trim();
                    LB_ProjectID.Text = project.ProjectID.ToString();

                    try
                    {
                        DL_ProjectType.SelectedValue = project.ProjectType;
                        ShareClass.LoadProjectForPMStatus(project.ProjectType.Trim(), strLangCode, DL_Status);
                    }
                    catch
                    {
                    }
                    try
                    {
                        DL_ProjectType.SelectedValue = project.ProjectType.Trim();
                        ShareClass.LoadProjectForPMStatus(project.ProjectType.Trim(), strLangCode, DL_Status);
                    }
                    catch
                    {
                    }
                    try
                    {
                        DL_Status.SelectedValue = project.Status;
                        DL_StatusValue.SelectedValue = project.StatusValue.Trim();
                    }
                    catch
                    {
                    }
                    try
                    {
                        DL_Status.SelectedValue = project.Status.Trim();
                        DL_StatusValue.SelectedValue = project.StatusValue.Trim();
                    }
                    catch
                    {
                    }
                    strStatus = project.Status.Trim();

                    try
                    {
                        DL_PM.SelectedValue = project.PMCode;
                    }
                    catch
                    {
                        DL_PM.Items.Insert(0, new ListItem(project.PMName, project.PMCode));
                    }

                    TB_CustomerPMName.Text = project.CustomerPMName;
                    LB_Status.Text = project.Status.Trim();
                    DLC_BeginDate.Text = project.BeginDate.ToString("yyyy-MM-dd");
                    DLC_EndDate.Text = project.EndDate.ToString("yyyy-MM-dd");
                    TB_ProjectName.Text = project.ProjectName;

                    if (strIsMobileDevice == "YES")
                    {
                        HT_ProjectDetail.Text = project.ProjectDetail;
                        HT_AcceptStandard.Text = project.AcceptStandard;
                    }
                    else
                    {
                        HE_ProjectDetail.Text = project.ProjectDetail;
                        HE_AcceptStandard.Text = project.AcceptStandard;
                    }

                    NB_Budget.Amount = project.Budget;
                    DLC_BeginDate.Text = project.BeginDate.ToString("yyyy-MM-dd");
                    DLC_EndDate.Text = project.EndDate.ToString("yyyy-MM-dd");
                    NB_ProjectAmount.Amount = project.ProjectAmount;
                    NB_ManHour.Amount = project.ManHour;
                    NB_ManNubmer.Amount = project.ManNumber;

                    DL_Priority.SelectedValue = project.Priority.Trim();

                    LB_BelongDepartCode.Text = project.BelongDepartCode.Trim();
                    LB_BelongDepartName.Text = project.BelongDepartName.Trim();

                    //2013-08-26 LiuJianping
                    HL_ProjectTask.Enabled = true;
                    HL_ProjectTask.NavigateUrl = "TTProjectPrimaveraTask.aspx?ProjectID=" + strProjectID;//end


                    //项目成本控制-"+LanguageHandle.GetWord("YuSuan")+"，仅仅创建人可以对预算进行管理
                    if (project.UserCode.Trim() == strProjectUserCode.Trim())
                    {
                        HL_ProjectCostManageEdit.Enabled = true;
                        HL_ProjectCostManageEdit.NavigateUrl = "TTProjectCostManageEdit.aspx?ProjectID=" + strProjectID;
                    }
                    else
                    {
                        HL_ProjectCostManageEdit.Enabled = false;
                    }

                    //设置项目其它属性的值 
                    SetProjectOtherFieldValue(strProjectID);

                    //会出错，这个错误是防止修改总项目
                    LB_ParentProjectID.Text = project.ParentID.ToString();
                    TB_ParentProject.Text = ShareClass.GetProjectName(project.ParentID.ToString());

                    CB_SMS.Enabled = true;
                    CB_Mail.Enabled = true;
                    BT_Send.Enabled = true;

                    //BusinessForm，列出关联表单模板
                    try
                    {
                        Panel_RelatedBusiness.Visible = true;

                        string strTemName;
                        strHQL = "Select * From T_RelatedBusinessForm Where RelatedType = 'Project' and RelatedID = " + strProjectID;
                        strHQL += " Order By CreateTime DESC";

                        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RelatedBusinessForm");

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            strTemName = ds.Tables[0].Rows[0]["TemName"].ToString().Trim();

                            DL_WLType.SelectedValue = ShareClass.GetWorkTemplateType(strTemName);
                            ShareClass.LoadWFTemplate(LB_UserCode.Text.Trim(), DL_WLType.SelectedValue.Trim(), DL_WFTemplate);
                            DL_WFTemplate.SelectedValue = strTemName;

                            DL_AllowUpdate.SelectedValue = ds.Tables[0].Rows[0]["AllowUpdate"].ToString().Trim();
                        }
                    }
                    catch
                    {
                    }

                    //BusinessForm,装载关联信息
                    TabContainer1.ActiveTabIndex = 0;
                    strProjectID = LB_ProjectID.Text.Trim();
                    ShareClass.LoadBusinessForm("Project", strProjectID, DL_WFTemplate.SelectedValue.Trim(), IFrame_RelatedInformation);

                    TB_Message.Text = strUserName + LanguageHandle.GetWord("GeiNiJianLiLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("QingJiShiShouLi").ToString().Trim();

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }
                catch (Exception ex)
                {
                    BT_Send.Enabled = false;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYKNS1STXGZXM2CXMFXMBCZ3XMZTLBZMYCXMDZTJC").ToString().Trim() + "')", true);
                }
            }

            if (e.CommandName == "Delete")
            {
                string strPMCode, strUserCode;
                IList lst;

                strUserCode = LB_UserCode.Text;
                strUserName = LB_UserName.Text;
                strPMCode = DL_PM.SelectedValue.Trim();

                ProjectBLL projectBLL = new ProjectBLL();
                strHQL = "from Project as project where project.ProjectID = " + strProjectID;
                lst = projectBLL.GetAllProjects(strHQL);
                Project project = (Project)lst[0];
                project.Status = "Deleted";

                strProjectName = project.ProjectName.Trim();

                try
                {
                    //会出错，这个错误是防止修改总项目
                    LB_ParentProjectID.Text = project.ParentID.ToString();
                    TB_ParentProject.Text = ShareClass.GetProjectName(project.ParentID.ToString());

                    projectBLL.UpdateProject(project, int.Parse(strProjectID));

                    if (strRelatedType == "Customer")
                    {
                        LoadCustomerRelatedProject(strRelatedID);
                    }

                    if (strRelatedType == "Constract")
                    {
                        LoadConstractRelatedProject(GetConstractCode(strRelatedID));
                    }

                    if (strRelatedType == "Other")
                    {
                        LoadProject(strUserCode);
                    }

                    if (strRelatedType == "tender")
                    {
                        LoadBMAnnInvitRelatedProject(strRelatedID);
                    }

                    ShareClass.InitialPrjectTreeByAuthority(TreeView1, strUserCode);



                    TB_Message.Text = strUserName + LanguageHandle.GetWord("ShanChuLeXiangMu").ToString().Trim() + strProjectID + " " + strProjectName + LanguageHandle.GetWord("DeNeiRongQingGuanZhuTeCiTongZh").ToString().Trim();

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "(" + LanguageHandle.GetWord("ZZCCYYKNS1STXGZXM2CXMFXMBCZ3XMZTLBZMYCXMDZTJC").ToString().Trim() + ")" + "')", true);
                }
            }
        }
    }

    //BusinessForm,工作流类型查询
    protected void DL_WLType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL, strWLType;

        strWLType = DL_WLType.SelectedValue.Trim();
        if (string.IsNullOrEmpty(strWLType))
        {
            return;
        }
        strHQL = "Select TemName From T_WorkFlowTemplate Where type = " + "'" + strWLType + "'" + " and Visible = 'YES' and Authority = 'All'";
        strHQL += " Order by SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplate");
        DL_WFTemplate.DataSource = ds;
        DL_WFTemplate.DataBind();

        DL_WFTemplate.Items.Add(new ListItem(""));

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    //BusinessForm,启动关联的业务表单
    protected void BT_StartupBusinessForm_Click(object sender, EventArgs e)
    {
        string strURL;
        string strTemName, strIdentifyString;
        strTemName = DL_WFTemplate.SelectedValue.Trim();
        strIdentifyString = ShareClass.GetWLTemplateIdentifyString(strTemName);

        string strProjectID;
        strProjectID = LB_ProjectID.Text.Trim();

        if (strProjectID == "")
        {
            strProjectID = "0";
        }

        //strURL = "popShowByURL(" + "'TTRelatedDIYBusinessForm.aspx?RelatedType=Project&RelatedID=" + strProjectID + "&IdentifyString=" + strIdentifyString + "','" + LanguageHandle.GetWord("XiangGuanYeWuDan").ToString().Trim() + "', 800, 600,window.location);";
        //ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop12", strURL, true);

        strURL = "TTRelatedDIYBusinessForm.aspx?RelatedType=Project&RelatedID=" + strProjectID + "&IdentifyString=" + strIdentifyString;
        IFrame_RelatedInformation.Attributes.Add("src", strURL);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    //BusinessForm,删除关联的业务表单
    protected void BT_DeleteBusinessForm_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strTemName;
        strTemName = DL_WFTemplate.SelectedValue.Trim();

        string strProjectID;
        strProjectID = LB_ProjectID.Text.Trim();

        strHQL = "Delete From T_RelatedBusinessForm Where RelatedType = 'Project' and RelatedID = " + strProjectID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG").ToString().Trim() + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void BT_AllProjects_Click(object sender, EventArgs e)
    {
        string strUserCode = LB_UserCode.Text.Trim();

        if (strRelatedType == "Customer")
        {
            LoadCustomerRelatedProject(strRelatedID);
        }

        if (strRelatedType == "Constract")
        {
            LoadConstractRelatedProject(GetConstractCode(strRelatedID));
        }

        if (strRelatedType == "Other")
        {
            LoadMyCreateProjectList(strUserCode);
        }
        if (strRelatedType == "tender")
            LoadBMAnnInvitRelatedProject(strRelatedID);

        DLC_BeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        DLC_EndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        TB_ProjectName.Text = "";

        if (strIsMobileDevice == "YES")
        {
            HT_ProjectDetail.Text = "";
            HT_AcceptStandard.Text = "";
        }
        else
        {
            HE_ProjectDetail.Text = "";
            HE_AcceptStandard.Text = "";
        }

        LB_ProjectID.Text = "";
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;

        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void DL_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strProjectID, strStatus;

        strProjectID = LB_ProjectID.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        if (strProjectID != "")
        {
            DL_StatusValue.SelectedValue = GetProjectStatusLatestValue(strProjectID, strStatus);
        }
        else
        {
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DL_StatusValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL, strWLID, strProjectID, strProjectType, strStatus, strStatusValues, strReviewControl;
        IList lst;

        strStatus = DL_Status.SelectedValue.Trim();
        strStatusValues = DL_StatusValue.SelectedValue.Trim();
        strProjectID = LB_ProjectID.Text.Trim();
        strProjectType = DL_ProjectType.SelectedValue.Trim();

        if (strProjectID != "")
        {
            strReviewControl = GetProjectStatusReviewControl(strProjectType, strStatus);

            if (strReviewControl == "YES")
            {
                if (strStatusValues == "Passed")
                {
                    strHQL = "from StatusRelatedWF as statusRelatedWF where statusRelatedWF.Status = " + "'" + strStatus + "'" + " and  statusRelatedWF.RelatedType = 'Project' and statusRelatedWF.RelatedID = " + strProjectID + " Order by statusRelatedWF.ID DESC";
                    StatusRelatedWFBLL statusRelatedWFBLL = new StatusRelatedWFBLL();
                    lst = statusRelatedWFBLL.GetAllStatusRelatedWFs(strHQL);
                    if (lst.Count > 0)
                    {
                        StatusRelatedWF statusRelatedWF = (StatusRelatedWF)lst[0];
                        strWLID = statusRelatedWF.WLID.ToString();

                        strHQL = "from WorkFlow as workFlow where workFlow.Status in ('Passed','CaseClosed') and workFlow.WLID = " + strWLID;
                        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
                        lst = workFlowBLL.GetAllWorkFlows(strHQL);

                        if (lst.Count == 0)
                        {
                            DL_StatusValue.SelectedValue = "InProgress";
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMYTJPSHPSMTGZTZBNGWTG").ToString().Trim() + "')", true);
                        }
                    }
                    else
                    {
                        DL_StatusValue.SelectedValue = "InProgress";
                        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZTMYTJPSHPSMTGZTZBNGWTG").ToString().Trim() + "')", true);
                    }
                }
            }
        }
        else
        {
            DL_StatusValue.SelectedValue = "InProgress";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCWKJLBNGBZTZXZXM").ToString().Trim() + "')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void ModifyRelatedUser(string strBMAnnInvitID, string strprojectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strprojectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst1 = projectBLL.GetAllProjects(strHQL);

        strHQL = "from BMAnnInvitation as bMAnnInvitation where bMAnnInvitation.ID = " + strBMAnnInvitID;
        BMAnnInvitationBLL bMAnnInvitationBLL = new BMAnnInvitationBLL();
        IList lst = bMAnnInvitationBLL.GetAllBMAnnInvitations(strHQL);
        if (lst != null && lst.Count > 0 && lst1 != null && lst1.Count > 0)
        {
            Project project = (Project)lst1[0];
            BMAnnInvitation bMAnnInvitation = (BMAnnInvitation)lst[0];
            string strBidPlanID = bMAnnInvitation.BidPlanID.ToString();
            string strSupplierIDList = string.IsNullOrEmpty(bMAnnInvitation.BidObjects) ? "" : bMAnnInvitation.BidObjects.Trim();
            #region [供应商-投标者]
            if (strSupplierIDList != "")
            {
                if (strSupplierIDList.Contains(","))
                {
                    string[] tempSupplierId = strSupplierIDList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < tempSupplierId.Length; i++)
                    {
                        strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + tempSupplierId[i] + "' ";
                        BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                        lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                        if (lst.Count > 0 && lst != null)
                        {
                            BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                            strHQL = "From ProjectMember as projectMember Where projectMember.UserCode='" + bMSupplierInfo.Code.Trim() + "' ";
                            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
                            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                            if (lst != null && lst.Count > 0)
                            {
                                ProjectMember projectMember = (ProjectMember)lst[0];
                                AddRelatedUserData(projectMember, project, LanguageHandle.GetWord("TouBiaoZhe").ToString().Trim());
                            }

                            strHQL = "From BMSupplierLink as bMSupplierLink Where bMSupplierLink.SupplierCode='" + bMSupplierInfo.Code.Trim() + "' ";
                            BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
                            lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);
                            if (lst != null && lst.Count > 0)
                            {
                                for (int j = 0; j < lst.Count; j++)
                                {
                                    BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[j];
                                    strHQL = "From ProjectMember as projectMember Where projectMember.UserCode='" + bMSupplierLink.Code.Trim() + "' ";
                                    lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                                    if (lst != null && lst.Count > 0)
                                    {
                                        ProjectMember projectMember = (ProjectMember)lst[0];
                                        AddRelatedUserData(projectMember, project, LanguageHandle.GetWord("TouBiaoZhe").ToString().Trim());
                                    }
                                    continue;
                                }
                            }
                            continue;
                        }
                    }
                }
                else
                {
                    strHQL = "From BMSupplierInfo as bMSupplierInfo Where bMSupplierInfo.ID='" + strSupplierIDList + "' ";
                    BMSupplierInfoBLL bMSupplierInfoBLL = new BMSupplierInfoBLL();
                    lst = bMSupplierInfoBLL.GetAllBMSupplierInfos(strHQL);
                    if (lst.Count > 0 && lst != null)
                    {
                        BMSupplierInfo bMSupplierInfo = (BMSupplierInfo)lst[0];
                        strHQL = "From ProjectMember as projectMember Where projectMember.UserCode='" + bMSupplierInfo.Code.Trim() + "' ";
                        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
                        lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                        if (lst != null && lst.Count > 0)
                        {
                            ProjectMember projectMember = (ProjectMember)lst[0];
                            AddRelatedUserData(projectMember, project, LanguageHandle.GetWord("TouBiaoZhe").ToString().Trim());
                        }

                        strHQL = "From BMSupplierLink as bMSupplierLink Where bMSupplierLink.SupplierCode='" + bMSupplierInfo.Code.Trim() + "' ";
                        BMSupplierLinkBLL bMSupplierLinkBLL = new BMSupplierLinkBLL();
                        lst = bMSupplierLinkBLL.GetAllBMSupplierLinks(strHQL);
                        if (lst != null && lst.Count > 0)
                        {
                            for (int j = 0; j < lst.Count; j++)
                            {
                                BMSupplierLink bMSupplierLink = (BMSupplierLink)lst[j];
                                strHQL = "From ProjectMember as projectMember Where projectMember.UserCode='" + bMSupplierLink.Code.Trim() + "' ";
                                lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                                if (lst != null && lst.Count > 0)
                                {
                                    ProjectMember projectMember = (ProjectMember)lst[0];
                                    AddRelatedUserData(projectMember, project, LanguageHandle.GetWord("TouBiaoZhe").ToString().Trim());
                                }
                                continue;
                            }
                        }
                    }
                }
            }
            #endregion

            strHQL = "from BMBidPlan as bMBidPlan where bMBidPlan.ID = " + strBidPlanID;
            BMBidPlanBLL bMBidPlanBLL = new BMBidPlanBLL();
            lst = bMBidPlanBLL.GetAllBMBidPlans(strHQL);
            if (lst != null && lst.Count > 0)
            {
                BMBidPlan bMBidPlan = (BMBidPlan)lst[0];
                string strExpertList = string.IsNullOrEmpty(bMBidPlan.UserCodeList) ? "" : bMBidPlan.UserCodeList.Trim();
                string strExpertAddList = string.IsNullOrEmpty(bMBidPlan.AddUserCodeList) ? "" : bMBidPlan.AddUserCodeList.Trim();
                #region [评标者]
                if (strExpertList != "")
                {
                    if (strExpertList.Contains(","))
                    {
                        string[] tempExpertId = strExpertList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < tempExpertId.Length; i++)
                        {
                            strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + tempExpertId[i] + "' ";
                            WZExpertBLL wZExpertBLL = new WZExpertBLL();
                            lst = wZExpertBLL.GetAllWZExperts(strHQL);
                            if (lst.Count > 0 && lst != null)
                            {
                                WZExpert wZExpert = (WZExpert)lst[0];
                                strHQL = "From ProjectMember as projectMember Where projectMember.UserCode='" + wZExpert.ExpertCode.Trim() + "' ";
                                ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
                                lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                                if (lst != null && lst.Count > 0)
                                {
                                    ProjectMember projectMember = (ProjectMember)lst[0];
                                    AddRelatedUserData(projectMember, project, LanguageHandle.GetWord("PingBiaoZhe").ToString().Trim());
                                }
                                continue;
                            }
                        }
                    }
                    else
                    {
                        strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + strExpertList + "' ";
                        WZExpertBLL wZExpertBLL = new WZExpertBLL();
                        lst = wZExpertBLL.GetAllWZExperts(strHQL);
                        if (lst.Count > 0 && lst != null)
                        {
                            WZExpert wZExpert = (WZExpert)lst[0];
                            strHQL = "From ProjectMember as projectMember Where projectMember.UserCode='" + wZExpert.ExpertCode.Trim() + "' ";
                            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
                            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                            if (lst != null && lst.Count > 0)
                            {
                                ProjectMember projectMember = (ProjectMember)lst[0];
                                AddRelatedUserData(projectMember, project, LanguageHandle.GetWord("PingBiaoZhe").ToString().Trim());
                            }
                        }
                    }
                }
                #endregion

                #region [评标者]
                if (strExpertAddList != "")
                {
                    if (strExpertAddList.Contains(","))
                    {
                        string[] tempExpertId1 = strExpertAddList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < tempExpertId1.Length; i++)
                        {
                            strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + tempExpertId1[i] + "' ";
                            WZExpertBLL wZExpertBLL = new WZExpertBLL();
                            lst = wZExpertBLL.GetAllWZExperts(strHQL);
                            if (lst.Count > 0 && lst != null)
                            {
                                WZExpert wZExpert = (WZExpert)lst[0];
                                strHQL = "From ProjectMember as projectMember Where projectMember.UserCode='" + wZExpert.ExpertCode.Trim() + "' ";
                                ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
                                lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                                if (lst != null && lst.Count > 0)
                                {
                                    ProjectMember projectMember = (ProjectMember)lst[0];
                                    AddRelatedUserData(projectMember, project, LanguageHandle.GetWord("PingBiaoZhe").ToString().Trim());
                                }
                                continue;
                            }
                        }
                    }
                    else
                    {
                        strHQL = "From WZExpert as wZExpert Where wZExpert.ID='" + strExpertAddList + "' ";
                        WZExpertBLL wZExpertBLL = new WZExpertBLL();
                        lst = wZExpertBLL.GetAllWZExperts(strHQL);
                        if (lst.Count > 0 && lst != null)
                        {
                            WZExpert wZExpert = (WZExpert)lst[0];
                            strHQL = "From ProjectMember as projectMember Where projectMember.UserCode='" + wZExpert.ExpertCode.Trim() + "' ";
                            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
                            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
                            if (lst != null && lst.Count > 0)
                            {
                                ProjectMember projectMember = (ProjectMember)lst[0];
                                AddRelatedUserData(projectMember, project, LanguageHandle.GetWord("PingBiaoZhe").ToString().Trim());
                            }
                        }
                    }
                }
                #endregion
            }
        }
    }

    protected void AddRelatedUserData(ProjectMember pm, Project pj, string strActor)
    {
        RelatedUserBLL relatedUserBLL = new RelatedUserBLL();
        RelatedUser relatedUser = new RelatedUser();

        relatedUser.Actor = strActor;
        relatedUser.ActorGroup = "";
        relatedUser.DepartCode = pm.DepartCode.Trim();
        relatedUser.DepartName = pm.DepartName.Trim();
        relatedUser.JoinDate = pm.JoinDate;
        relatedUser.ProjectID = pj.ProjectID;
        relatedUser.ProjectName = pj.ProjectName.Trim();
        relatedUser.PromissionScale = 0;
        relatedUser.SalaryMethod = LanguageHandle.GetWord("DiCheng").ToString().Trim();
        relatedUser.SMSCount = 0;
        relatedUser.Status = "Plan";
        relatedUser.UnitHourSalary = 0;
        relatedUser.UserCode = pm.UserCode.Trim();
        relatedUser.UserName = pm.UserName.Trim();
        relatedUser.WorkDetail = pm.WorkScope.Trim();

        relatedUserBLL.AddRelatedUser(relatedUser);
    }


    protected void AddStatusChangeRecord(string strProjectID, string strOldStatus, string strNewStatus, string strOldStatusValue, string strNewStatusValue)
    {
        string strUserCode, strUserName;

        if ((strOldStatus != strNewStatus) | (strOldStatusValue != strNewStatusValue))
        {
            strUserCode = LB_UserCode.Text.Trim();
            strUserName = LB_UserName.Text.Trim();
            ProStatusChangeBLL proStatusChangeBLL = new ProStatusChangeBLL();
            ProStatusChange proStatusChange = new ProStatusChange();

            proStatusChange.ProjectID = int.Parse(strProjectID);
            proStatusChange.UserCode = strUserCode;
            proStatusChange.UserName = strUserName;
            proStatusChange.OldStatus = strOldStatus;
            proStatusChange.NewStatus = strNewStatus;
            proStatusChange.OldStatusValue = strOldStatusValue;
            proStatusChange.NewStatusValue = strNewStatusValue;
            proStatusChange.ChangeTime = DateTime.Now;

            try
            {
                proStatusChangeBLL.AddProStatusChange(proStatusChange);
            }
            catch
            {
            }
        }
    }

    protected void InsertIntoRelatedProject(string strProjectID)
    {
        string strHQL;

        if (strRelatedType == "Customer")
        {
            strHQL = "Insert Into T_ProjectCustomer(ProjectID,CustomerCode) Values(" + strProjectID + "," + "'" + strRelatedID + "'" + ")";
            ShareClass.RunSqlCommand(strHQL);
        }
        if (strRelatedType == "Constract")
        {
            strHQL = "Insert Into T_ConstractRelatedProject(ProjectID,ConstractCode) Values(" + strProjectID + "," + "'" + GetConstractCode(strRelatedID) + "'" + ")";
            //
            ShareClass.RunSqlCommand(strHQL);
        }
        if (strRelatedType == "tender")
        {
            strHQL = "Insert Into T_BMAnnInvitRelatedProject(ProjectID,BMAnnInID) Values(" + strProjectID + "," + strRelatedID + ")";
            ShareClass.RunSqlCommand(strHQL);
        }
    }

    protected void LoadCustomerRelatedProject(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.ProjectID in (Select projectCustomer.ProjectID From ProjectCustomer as projectCustomer Where projectCustomer.CustomerCode = " + "'" + strCustomerCode + "'" + ")";
        strHQL += " Order By project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;

    }

    protected void LoadConstractRelatedProject(string strConstractCode)
    {
        string strHQL;
        IList lst;
        strHQL = "from Project as project where project.ProjectID in (Select constractRelatedProject.ProjectID From ConstractRelatedProject as constractRelatedProject Where constractRelatedProject.ConstractCode = " + "'" + strConstractCode + "'" + ")";
        strHQL += " Order By project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;

    }

    protected void LoadBMAnnInvitRelatedProject(string strBMAnnInvitID)
    {
        string strHQL;
        IList lst;
        strHQL = "from Project as project where project.ProjectID in (Select bMAnnInvitRelatedProject.ProjectID From BMAnnInvitRelatedProject as bMAnnInvitRelatedProject Where bMAnnInvitRelatedProject.BMAnnInID = " + "'" + strBMAnnInvitID + "'" + ")";
        strHQL += " Order By project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadMyCreateProjectList(string strUserCode)
    {
        string strHQL;
        IList lst;
        strHQL = "from Project as project where project.UserCode = " + "'" + strUserCode + "'" + " and  project.PMCode in (select memberLevel.UnderCode from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + ") and project.Status <> 'Archived' order by project.ProjectID DESC";

        ProjectBLL projectBLL = new ProjectBLL();

        lst = projectBLL.GetAllProjects(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadProject(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where project.UserCode = " + "'" + strUserCode + "'";

        if (strRelatedType == "Customer")
        {
            strHQL += " and project.ProjectID in (Select projectCustomer.ProjectID From ProjectCustomer as projectCustomer Where projectCustomer.CustomerCode = " + "'" + strRelatedID + "'" + ")";
        }

        if (strRelatedType == "Constract")
        {
            strHQL += " and project.ProjectID in (Select constractRelatedProject.ProjectID From ConstractRelatedProject as constractRelatedProject Where constractRelatedProject.ConstractCode = " + "'" + GetConstractCode(strRelatedID) + "'" + ")";
        }

        if (strRelatedType == "tender")
        {
            strHQL += " and project.ProjectID in (Select bMAnnInvitRelatedProject.ProjectID From BMAnnInvitRelatedProject as bMAnnInvitRelatedProject Where bMAnnInvitRelatedProject.BMAnnInID = " + "'" + strRelatedID + "'" + ")";
        }

        strHQL += " and project.Status <> 'Archived' order by project.ProjectID DESC";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);
        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected string GetProjectID(string strProjectName)
    {
        string strHQL;
        IList lst;

        strHQL = "from Project as project where rtrim(ltrim(project.ProjectName)) = " + "'" + strProjectName + "'";
        ProjectBLL projectBLL = new ProjectBLL();
        lst = projectBLL.GetAllProjects(strHQL);

        Project project = (Project)lst[0];

        return project.ProjectName.Trim();
    }

    protected string GetProjectStatusReviewControl(string strProjectType, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectStatus as projectStatus where projectType = " + "'" + strProjectType + "'" + " and projectStatus.Status = " + "'" + strStatus + "'";
        ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
        lst = projectStatusBLL.GetAllProjectStatuss(strHQL);

        ProjectStatus projectStatus = (ProjectStatus)lst[0];

        return projectStatus.ReviewControl.Trim();
    }

    protected string GetProjectStatusLatestValue(string strProjectID, string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = " from ProStatusChange as proStatusChange where proStatusChange.ProjectID = " + strProjectID;
        strHQL += " and proStatusChange.NewStatus = " + "'" + strStatus + "'";
        strHQL += " Order by proStatusChange.ChangeTime DESC";
        ProStatusChangeBLL proStatusChangeBLL = new ProStatusChangeBLL();
        lst = proStatusChangeBLL.GetAllProStatusChanges(strHQL);

        if (lst.Count > 0)
        {
            ProStatusChange proStatusChange = (ProStatusChange)lst[0];
            return proStatusChange.NewStatusValue.Trim();
        }
        else
        {
            return "InProgress";
        }
    }

    protected string GetProjectStatusIdentityString(string strStatus)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectStatus as projectStatus where projectStatus.Status = " + "'" + strStatus + "'";
        ProjectStatusBLL projectStatusBLL = new ProjectStatusBLL();
        lst = projectStatusBLL.GetAllProjectStatuss(strHQL);

        ProjectStatus projectStatus = (ProjectStatus)lst[0];

        return projectStatus.IdentityString.Trim();
    }

    protected string GetCustomerName(string strCustomerCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Customer as customer where customer.CustomerCode = " + "'" + strCustomerCode + "'";
        CustomerBLL customerBLL = new CustomerBLL();
        lst = customerBLL.GetAllCustomers(strHQL);

        Customer customer = (Customer)lst[0];

        return customer.CustomerName.Trim();
    }

    protected string GetConstractCode(string strConstractID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.ConstractID = " + strConstractID;

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        Constract constract = (Constract)lst[0];

        return constract.ConstractCode.Trim();

    }

    protected string GetConstractName(string strConstractID)
    {
        string strHQL;
        IList lst;

        strHQL = "from Constract as constract where constract.ConstractID = " + strConstractID;

        ConstractBLL constractBLL = new ConstractBLL();
        lst = constractBLL.GetAllConstracts(strHQL);

        Constract constract = (Constract)lst[0];

        return constract.ConstractName.Trim();
    }
}
