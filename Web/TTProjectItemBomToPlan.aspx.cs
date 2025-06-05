using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using TakeTopCore;

using ProjectMgt.BLL;
using ProjectMgt.Model;

public partial class TTProjectItemBomToPlan : System.Web.UI.Page
{
    string strUserCode, strUserName, strDepartCode, strDepartName, strProjectID, strProjectName;

    protected void Page_Load(object sender, EventArgs e)
    {
        strProjectID = Request.QueryString["ProjectID"];
        strProjectName = ShareClass.GetProjectName(strProjectID);

        strUserCode = Session["UserCode"].ToString();
        strUserName = Session["UserName"].ToString();

        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
        strDepartName = ShareClass.GetDepartName(strDepartCode);

        string strProjectBomVerID;
        string strItemCode;
        string strPlanVerID;
        string strID;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "AutoHeight", "autoheight();", true);
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            try
            {
                LoadProjectItemBomVersion(strProjectID);

                strID = GetProjectItemBomVersionIDByType(strProjectID, "InUse");
                if (int.Parse(strID) > 0)
                {
                    DL_ChangeProjectItemBomVersionID.SelectedValue = strID;
                    strProjectBomVerID = DL_ChangeProjectItemBomVersionID.SelectedItem.Text.Trim();
                    TakeTopBOM.InitialProjectItemBomTree(strProjectID, strProjectBomVerID, TreeView2);
                }
                else
                {
                    strProjectBomVerID = DL_ChangeProjectItemBomVersionID.SelectedItem.Text.Trim();
                    TakeTopBOM.InitialProjectItemBomTree(strProjectID, strProjectBomVerID, TreeView2);
                }

                LoadProjectPlanVersion(strProjectID);
                strPlanVerID = DL_PlanVersionID.SelectedItem.Text.Trim();
                TakeTopPlan.InitialProjectPlanTree(TreeView1, strProjectID, strPlanVerID);

                HL_CopyFromOtherProjectItemBom.NavigateUrl = "TTProjectRelatedItemBomcopy.aspx?ProjectID=" + strProjectID;
                HL_ProjectImplePlan.NavigateUrl = "TTWorkPlanMain.aspx?ProjectID=" + strProjectID;

                HL_ProPlanGanttNew.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strProjectID + "&VerID=" + strPlanVerID;
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCXZXMCPMKZJCPBOMBBHSJ") + "')", true);
            }
        }
    }


    private void SolidifyProjectItemBomForAll(string strProjectID, string strProjectBomVerID, string strItemCode, string strItemBomVersionID, string strParentGuid, string strBelongItemCode, string strBelongVerID)
    {
        string strHQL;
        IList lst1, lst2;

        string strChildGuid;

        string strUnit;
        decimal deNumber;


        string strChildItemVerID, strChildItemCode, strChildItemName;

        Item item = new Item();
        ProjectRelatedItem projectRelatedItem = new ProjectRelatedItem();
        ProjectRelatedItemBom projectRelatedItemBom = new ProjectRelatedItemBom();
        ProjectRelatedItemBomBLL projectRelatedItemBomBLL = new ProjectRelatedItemBomBLL();

        TreeNode node = new TreeNode();
        ItemBom itemBom = new ItemBom();

        strHQL = "From ItemBom as itemBom Where  itemBom.ParentItemCode = " + "'" + strItemCode + "'";
        strHQL += " And itemBom.KeyWord <> itemBom.ParentKeyWord";
        strHQL += " And itemBom.BelongItemCode = " + "'" + strBelongItemCode + "'" + " And itemBom.BelongVerID = " + strBelongVerID;
        strHQL += " Order By itemBom.ID ASC";
        ItemBomBLL itemBomBLL = new ItemBomBLL();
        lst1 = itemBomBLL.GetAllItemBoms(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            itemBom = (ItemBom)lst1[i];

            strChildGuid = Guid.NewGuid().ToString();
            strChildItemCode = itemBom.ChildItemCode.Trim();
            strChildItemName = GetItemName(strChildItemCode);
            strChildItemVerID = itemBom.ChildItemVerID.ToString();
            deNumber = itemBom.Number;
            strUnit = itemBom.Unit.Trim();

            projectRelatedItemBom.ProjectID = int.Parse(strProjectID);
            projectRelatedItemBom.ItemCode = strChildItemCode;
            projectRelatedItemBom.ItemName = strChildItemName;

            projectRelatedItemBom.ParentGuid = strParentGuid;
            projectRelatedItemBom.ChildGuid = strChildGuid;

            projectRelatedItemBom.Number = deNumber;
            projectRelatedItemBom.Unit = strUnit;
            projectRelatedItemBom.VerID = int.Parse(strProjectBomVerID);

            item = new Item();
            item = GetItem(strItemCode);

            projectRelatedItemBom.ItemType = item.Type.Trim();
            projectRelatedItemBom.Specification = item.Specification.Trim();
            projectRelatedItemBom.PULeadTime = item.PULeadTime;
            projectRelatedItemBom.MFLeadTime = item.MFLeadTime;
            projectRelatedItemBom.MFCost = item.MFCost;
            projectRelatedItemBom.HRCost = item.HRCost;
            projectRelatedItemBom.MTCost = item.MTCost;
            projectRelatedItemBom.Comment = LanguageHandle.GetWord("WuLiaoZiLiao");

            projectRelatedItemBomBLL.AddProjectRelatedItemBom(projectRelatedItemBom);

            strHQL = "From ItemBom as itemBom Where itemBom.ItemCode = " + "'" + strChildItemCode + "'" + " and itemBom.ParentItemCode = " + "'" + strChildItemCode + "'" + " and itemBom.VerID = " + strChildItemVerID;
            strHQL += " And itemBom.KeyWord <> itemBom.ParentKeyWord";
            strHQL += " And itemBom.BelongItemCode = " + "'" + strBelongItemCode + "'" + " And itemBom.BelongVerID = " + strBelongVerID;
            strHQL += " Order By itemBom.ID ASC";
            lst2 = itemBomBLL.GetAllItemBoms(strHQL);
            if (lst2.Count > 0)
            {
                SolidifyProjectItemBomForAll(strProjectID, strProjectBomVerID, strChildItemCode, strChildItemVerID, strChildGuid, strBelongItemCode, strBelongVerID);
            }
        }
    }


    protected void BT_NewVersion_Click(object sender, EventArgs e)
    {

        int intVerID;
        string strID;

        intVerID = int.Parse(NB_NewProjectItemBomVerID.Amount.ToString());

        if (GetProjectItemBomVersionCount(strProjectID, intVerID.ToString()) == 0)
        {
            ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
            ProjectItemBomVersion projectItemBomVersion = new ProjectItemBomVersion();

            projectItemBomVersion.ProjectID = int.Parse(strProjectID);
            projectItemBomVersion.VerID = intVerID;
            projectItemBomVersion.Type = "Backup";

            try
            {
                projectItemBomVersionBLL.AddProjectItemBomVersion(projectItemBomVersion);

                LoadProjectItemBomVersion(strProjectID);

                strID = GetProjectItemBomVersionID(strProjectID, intVerID.ToString());

                DL_ChangeProjectItemBomVersionID.SelectedValue = strID;

                TakeTopBOM.InitialProjectItemBomTree(strProjectID, intVerID.ToString(), TreeView2);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCXMZKNCZCBBHJC") + "')", true);
        }
    }

    protected void BT_DeleteVersion_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strVerID;

        if (DL_PlanVersionID.Items.Count == 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBMXMBXBLYGJHBB") + "')", true);
            return;
        }

        strVerID = NB_NewProjectItemBomVerID.Amount.ToString();


        try
        {
            strHQL = "Delete From T_ProjectItemBomVersion Where ProjectID = " + strProjectID + " and VerID = " + strVerID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_ProjectRelatedItemBom Where ProjectID = " + strProjectID + " and VerID = " + strVerID;
            ShareClass.RunSqlCommand(strHQL);

            LoadProjectItemBomVersion(strProjectID);

            if (DL_ChangeProjectItemBomVersionID.Items.Count > 0)
            {
                strVerID = DL_ChangeProjectItemBomVersionID.SelectedItem.Text.Trim();

                TakeTopBOM.InitialProjectItemBomTree(strProjectID, strVerID, TreeView2);
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected void BT_CopyVersion_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID, strOldVerID, strNewVerID;

        strOldVerID = DL_OldVersionID.SelectedItem.Text.Trim();
        strNewVerID = DL_NewVersionID.SelectedItem.Text.Trim();

        if (strOldVerID == strNewVerID)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFZHBFZDBOMBBHBNXTJC") + "')", true);
            return;
        }

        try
        {
            strHQL = "Delete From T_ProjectRelatedItemBom Where ProjectID = " + strProjectID + " and VerID = " + strNewVerID;
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Insert Into T_ProjectRelatedItemBom(ProjectID,ParentGuid,ChildGuid,ItemCode,ItemName,Number,ReservedNumber,Unit,VerID,ItemType,Specification,PULeadTime,MFLeadTime,HRCost,MTCost,MFCost,DefaultProcess,Comment)";
            strHQL += " Select ProjectID,ParentGuid,ChildGuid,ItemCode,ItemName,Number,ReservedNumber,Unit," + strNewVerID + ",ItemType,Specification,PULeadTime,MFLeadTime,HRCost,MTCost,MFCost,DefaultProcess,Comment From T_ProjectRelatedItemBom";
            strHQL += " Where ProjectID = " + strProjectID + " and VerID = " + strOldVerID;
            strHQL += " Order By ID ASC";
            ShareClass.RunSqlCommand(strHQL);

            strID = GetProjectItemBomVersionID(strProjectID, strNewVerID);
            DL_ChangeProjectItemBomVersionID.SelectedValue = strID;

            TakeTopBOM.InitialProjectItemBomTree(strProjectID, strNewVerID, TreeView2);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGFZSBJC") + "')", true);
        }
    }

    protected void DL_ChangeProjecrItemBomVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strVerID;
        string strVerType;


        strID = DL_ChangeProjectItemBomVersionID.SelectedValue.Trim();
        strVerID = DL_ChangeProjectItemBomVersionID.SelectedItem.Text.Trim();

        strVerType = GetProjectItemBomVersionType(strProjectID, strVerID);
        DL_ChangeProjectItemBomVersionType.SelectedValue = strVerType;

        TakeTopBOM.InitialProjectItemBomTree(strProjectID, strVerID, TreeView2);
    }

    protected void DL_ChangeProjectItemBomVersionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strID, strType;
        string strHQL;


        if (DL_PlanVersionID.Items.Count > 0)
        {
            strID = DL_ChangeProjectItemBomVersionID.SelectedValue.Trim();
            strType = DL_ChangeProjectItemBomVersionType.SelectedValue.Trim();

            if (strType == "InUse")
            {
                strHQL = "update T_ProjectItemBomVersion Set Type = 'Backup' where Type = 'InUse' and ProjectID = " + strProjectID;
                ShareClass.RunSqlCommand(strHQL);
            }

            if (strType == "Baseline")
            {
                strHQL = "update T_ProjectItemBomVersion Set Type = 'Backup' where Type = 'Baseline' and ProjectID = " + strProjectID;
                ShareClass.RunSqlCommand(strHQL);
            }


            strHQL = "Update T_ProjectItemBomVersion Set Type = " + "'" + strType + "'" + " Where ID = " + strID;

            try
            {
                ShareClass.RunSqlCommand(strHQL);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBBLXYGG") + "')", true);
            }
            catch
            {
            }
        }
    }

    protected void BT_TransferToPlan_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strPlanVerID;
        string strProjectItemBomVerID;
        string strTransferType;

        DataSet ds = new DataSet();

        if (DL_ChangeProjectItemBomVersionID.Items.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWBOMBBHBNWKJC") + "')", true);
            return;
        }

        if (DL_PlanVersionID.Items.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWYZRDJHBBHBNWKJC") + "')", true);
            return;
        }

        strPlanVerID = DL_PlanVersionID.SelectedItem.Text.Trim();
        strHQL = "delete from T_PlanMember where PlanID in (Select ID From T_ImplePlan where ProjectID = " + strProjectID + " and VerID = " + strPlanVerID + ")";
        ShareClass.RunSqlCommand(strHQL);
        strHQL = "delete from T_ImplePlan where ProjectID = " + strProjectID + " and VerID = " + strPlanVerID;
        ShareClass.RunSqlCommand(strHQL);

        strProjectItemBomVerID = DL_ChangeProjectItemBomVersionID.SelectedItem.Text.Trim();

        ProjectRelatedItemBom projectRelatedItemBom = new ProjectRelatedItemBom();
        Project project = ShareClass.GetProject(strProjectID);

        strTransferType = DL_TransferPlanType.SelectedValue.Trim();

        //把WBS转成项目计划
        if (ProjectWBSTransferToPlan(strProjectID, strPlanVerID, strProjectItemBomVerID, strTransferType, strUserCode) == true)
        {
            TakeTopPlan.InitialProjectPlanTree(TreeView1, strProjectID, strPlanVerID);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZZJHCG") + "')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWZJHSBJC") + "')", true);
        }
    }

    //项目WBS转项目计划
    public bool ProjectWBSTransferToPlan(string strProjectID, string strPlanVerID, string strProjectItemBomVerID, string strTransferType, string strOperatorCode)
    {
        string strHQL, strHQL2, strHQL3;
        IList lst, lst3;
        DataSet ds;

        ItemBLL itemBLL = new ItemBLL();
        Item item = new Item();

        string strID, strItemCode, strItemName, strItemType, strUnit, strSpecification, strBrand, strDefaultProcess;
        string strParentGuid, strChildGuid;
        decimal deNumber, deReservedNumber, dePrice;
        DateTime dtBeginDate, dtEndDate, dtPlanMinDate, dtParentBeginDate;
        int intDayNumbers = 0;

        string strPlanID;
        string strBelongDepartCode, strBelongDepartName;
        string strPMCode, strPMName;

        ProjectRelatedItemBom projectRelatedItemBom = new ProjectRelatedItemBom();
        Project project = ShareClass.GetProject(strProjectID);

        strBelongDepartCode = project.BelongDepartCode.Trim();
        strBelongDepartName = project.BelongDepartName.Trim();
        strPMCode = project.PMCode.Trim();
        strPMName = project.PMName.Trim();

        strHQL = "From ProjectRelatedItemBom as projectRelatedItemBom Where projectRelatedItemBom.ProjectID = " + strProjectID + " and projectRelatedItemBom.VerID = " + strProjectItemBomVerID;
        strHQL += " and projectRelatedItemBom.ParentGuid = projectRelatedItemBom.ChildGuid ";
        ProjectRelatedItemBomBLL projectRelatedItemBomBLL = new ProjectRelatedItemBomBLL();
        lst = projectRelatedItemBomBLL.GetAllProjectRelatedItemBoms(strHQL);

        if (lst.Count == 0)
        {
            return false;
        }


        projectRelatedItemBom = (ProjectRelatedItemBom)lst[0];

        strID = projectRelatedItemBom.ID.ToString();
        strItemCode = projectRelatedItemBom.ItemCode.Trim();
        strItemName = projectRelatedItemBom.ItemName.Trim();
        strSpecification = projectRelatedItemBom.Specification.Trim();

        if (projectRelatedItemBom.Brand != null)
        {
            strBrand = projectRelatedItemBom.Brand.Trim();
        }
        else
        {
            strBrand = "";
        }

        deNumber = projectRelatedItemBom.Number;
        deReservedNumber = projectRelatedItemBom.ReservedNumber;

        dePrice = projectRelatedItemBom.PurchasePrice;

        strUnit = projectRelatedItemBom.Unit.Trim();
        strDefaultProcess = projectRelatedItemBom.DefaultProcess.Trim();

        strParentGuid = projectRelatedItemBom.ParentGuid.Trim();
        strChildGuid = projectRelatedItemBom.ChildGuid.Trim();

        dtBeginDate = project.BeginDate;
        dtEndDate = project.EndDate;

        if (dtBeginDate == dtEndDate)
        {
            dtBeginDate.AddDays(1);
        }

        strHQL2 = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
        itemBLL = new ItemBLL();
        lst3 = itemBLL.GetAllItems(strHQL2);
        if (lst3.Count > 0)
        {
            item = (Item)lst3[0];
            strItemType = item.Type.Trim();

            if (strItemType == "PurchParts")
            {
                dtParentBeginDate = dtEndDate.AddDays(double.Parse((0 - item.PULeadTime).ToString()));
            }
            else
            {
                dtParentBeginDate = dtEndDate.AddDays(double.Parse((0 - item.MFLeadTime).ToString()));
            }
        }
        else
        {
            dtParentBeginDate = dtEndDate.AddDays(double.Parse((0 - 0).ToString()));
        }

        int intGanttPID = GetPIDForGantt(int.Parse(strProjectID), int.Parse(strPlanVerID));

        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        WorkPlan workPlan = new WorkPlan();
        workPlan.Type = "Plan";
        workPlan.ProjectID = int.Parse(strProjectID);
        workPlan.WorkID = 0;
        workPlan.PriorID = 0;
        workPlan.VerID = int.Parse(strPlanVerID);
        workPlan.Name = strItemCode + " " + strItemName + LanguageHandle.GetWord("GuiGe") + projectRelatedItemBom.Specification.Trim() +  LanguageHandle.GetWord("GongYi") + strDefaultProcess + ")";
        workPlan.Start_Date = dtBeginDate;
        workPlan.End_Date = dtEndDate;
        workPlan.Resource = "";

        workPlan.Budget = 0;
        workPlan.WorkHour = 0;

        workPlan.MakeDate = DateTime.Now;
        workPlan.Status = "Plan";

        workPlan.Percent_Done = 0;
        workPlan.DefaultCost = 0;
        workPlan.DefaultSchedule = 0;
        workPlan.LockStatus = "NO";
        workPlan.UpdateManCode = strOperatorCode;
        workPlan.CreatorCode = strOperatorCode;
        workPlan.UpdateTime = DateTime.Now;

        workPlan.PID = intGanttPID;
        workPlan.Parent_ID = 0;

        workPlan.SortNumber = 1;

        workPlanBLL.AddWorkPlan(workPlan);

        strPlanID = ShareClass.GetMyCreatedMaxProPlanID(strProjectID, strPlanVerID);

        //更改计划其它属性
        strHQL3 = "Update T_ImplePlan Set ParentID = 0,BackupID = " + strID + ",LeaderCode = '" + strPMCode + "',Leader = '" + strPMName + "'";
        strHQL3 += " ,BaseLine_Start_Date = Start_Date,BaseLine_End_Date = End_Date ";
        strHQL3 += " where ProjectID = " + strProjectID + " and VerID = " + strPlanVerID + " and ID = " + strPlanID;
        ShareClass.RunSqlCommand(strHQL3);
        strHQL = "Update T_Impleplan Set RequireNumber = " + (deNumber + deReservedNumber).ToString() + ",FinishedNumber = 0,Price =" + dePrice.ToString() + ",UnitName ='" + strUnit + "' Where ID = " + strPlanID;
        ShareClass.RunSqlCommand(strHQL);
        strHQL = "Update T_ImplePlan Set BelongDepartCode = '" + strBelongDepartCode + "',BelongDepartName = '" + strBelongDepartName + "' Where ID = " + strPlanID;
        ShareClass.RunSqlCommand(strHQL);

        try
        {
            TransferProjectItemBomToPlan(intGanttPID, strProjectID, strProjectItemBomVerID, strChildGuid, dtParentBeginDate, strPlanVerID, strPlanID, strOperatorCode, strBelongDepartCode, strBelongDepartName, strPMCode, strPMName);


            if (strTransferType == "ForwardScheduling")
            {
                strHQL = "Select Min(Start_Date) From T_ImplePlan Where ProjectID = " + strProjectID + " And VerID = " + strPlanVerID + " And Parent_ID > 0";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");

                try
                {
                    dtPlanMinDate = DateTime.Parse(ds.Tables[0].Rows[0][0].ToString());

                    intDayNumbers = DateDiff(dtBeginDate, dtPlanMinDate);

                    if (dtBeginDate < dtPlanMinDate)
                    {
                        intDayNumbers = 0 - intDayNumbers;
                    }

                    strHQL = "Update T_ImplePlan Set Start_Date = Start_Date +" + intDayNumbers.ToString() + "*'1 day'::interval Where ProjectID = " + strProjectID + " and VerID = " + strPlanVerID + " and Parent_ID > 0";
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Update T_ImplePlan Set End_Date = End_Date +" + intDayNumbers.ToString() + "*'1 day'::interval Where ProjectId = " + strProjectID + " and VerID = " + strPlanVerID + " and Parent_ID > 0";
                    ShareClass.RunSqlCommand(strHQL);
                }
                catch (Exception err)
                {
                    LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

                }

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception err)
        {
            LogClass.WriteLogFile("Error page: " + Request.Url.ToString() + "\n" + err.Message.ToString() + "\n" + err.StackTrace);

            return false;
        }
    }

    //项目WBS转项目计划
    public void TransferProjectItemBomToPlan(int intGanttPID, string strProjectID, string strProjectItemBomVerID, string strParentGuid, DateTime dtParentBeginDate, string strPlanVerID, string strParentPlanID, string strOperatorCode, string strBelongDepartCode, string strBelongDepartName, string strPMCode, string strPMName)
    {
        string strHQL, strHQL2, strHQL3;
        IList lst1, lst2, lst3;

        ItemBLL itemBLL = new ItemBLL();
        Item item = new Item();

        string strID, strChildGuid;
        string strItemCode, strItemName, strItemType;
        string strUnit, strDefaultProcess;
        decimal deNumber, deReservedNumber, deTotalNumber, dePrice;
        DateTime dtBeginDate;
        string strPlanID;

        ProjectRelatedItemBom projectRelatedItemBom = new ProjectRelatedItemBom();
        WorkPlanBLL workPlanBLL = new WorkPlanBLL();
        WorkPlan workPlan = new WorkPlan();

        strHQL = "From ProjectRelatedItemBom as projectRelatedItemBom Where projectRelatedItemBom.ProjectID = " + strProjectID + " and projectRelatedItemBom.ParentGuid = " + "'" + strParentGuid + "'" + " and projectRelatedItemBom.VerID = " + strProjectItemBomVerID;
        strHQL += " And projectRelatedItemBom.ParentGuid <> projectRelatedItemBom.ChildGuid";
        strHQL += " Order By projectRelatedItemBom.ID ASC";
        ProjectRelatedItemBomBLL projectRelatedItemBomBLL = new ProjectRelatedItemBomBLL();
        lst1 = projectRelatedItemBomBLL.GetAllProjectRelatedItemBoms(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            projectRelatedItemBom = (ProjectRelatedItemBom)lst1[i];

            strID = projectRelatedItemBom.ID.ToString();
            strChildGuid = projectRelatedItemBom.ChildGuid.Trim();
            strItemCode = projectRelatedItemBom.ItemCode.Trim();
            strItemName = projectRelatedItemBom.ItemName.Trim();

            deNumber = projectRelatedItemBom.Number;
            deReservedNumber = projectRelatedItemBom.ReservedNumber;
            deTotalNumber = deNumber + deReservedNumber;

            dePrice = projectRelatedItemBom.PurchasePrice;
            strUnit = projectRelatedItemBom.Unit.Trim();
            strDefaultProcess = projectRelatedItemBom.DefaultProcess.Trim();

            strHQL2 = "From Item as item Where item.ItemCode = " + "'" + strItemCode + "'";
            itemBLL = new ItemBLL();
            lst3 = itemBLL.GetAllItems(strHQL2);
            if (lst3.Count > 0)
            {
                item = (Item)lst3[0];
                strItemType = item.Type.Trim();

                if (strItemType == "PurchParts")
                {
                    dtBeginDate = dtParentBeginDate.AddDays(double.Parse((0 - item.PULeadTime).ToString()));
                }
                else
                {
                    dtBeginDate = dtParentBeginDate.AddDays(double.Parse((0 - item.MFLeadTime).ToString()));
                }
            }
            else
            {
                dtBeginDate = dtParentBeginDate.AddDays(double.Parse((0 - 0).ToString()));
            }

            workPlan = new WorkPlan();

            workPlan.Type = "Plan";
            workPlan.ProjectID = int.Parse(strProjectID);
            workPlan.WorkID = 0;
            workPlan.PriorID = 0;
            workPlan.VerID = int.Parse(strPlanVerID);
            workPlan.Name = strItemCode + " " + strItemName + LanguageHandle.GetWord("GuiGe") + projectRelatedItemBom.Specification.Trim() + LanguageHandle.GetWord("GongYi") + strDefaultProcess + ")";
            workPlan.Start_Date = dtBeginDate;
            workPlan.End_Date = dtParentBeginDate;
            workPlan.Resource = "";

            workPlan.Budget = 0;
            workPlan.WorkHour = 0;

            workPlan.MakeDate = DateTime.Now;
            workPlan.Status = "Plan";

            workPlan.Percent_Done = 0;
            workPlan.DefaultCost = 0;
            workPlan.DefaultSchedule = 0;
            workPlan.LockStatus = "NO";
            workPlan.UpdateManCode = strOperatorCode;
            workPlan.CreatorCode = strOperatorCode;
            workPlan.UpdateTime = DateTime.Now;
            workPlan.PID = intGanttPID;
            workPlan.Parent_ID = int.Parse(strParentPlanID);

            workPlan.SortNumber = 1;

            workPlanBLL.AddWorkPlan(workPlan);

            strPlanID = ShareClass.GetMyCreatedMaxProPlanID(strProjectID, strPlanVerID);

            //更改计划其它属性
            strHQL3 = "Update T_ImplePlan Set ParentID = " + strParentPlanID + ",BackupID = " + strID + ",LeaderCode = '" + strPMCode + "',Leader = '" + strPMName + "'";
            strHQL3 += " ,BaseLine_Start_Date = Start_Date,BaseLine_End_Date = End_Date ";
            strHQL3 += " where ProjectID = " + strProjectID + " and VerID = " + strPlanVerID + " and ID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL3);
            strHQL = "Update T_Impleplan Set RequireNumber = " + deTotalNumber.ToString() + ",FinishedNumber = 0,Price =" + dePrice.ToString() + ",UnitName ='" + strUnit + "' Where ID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);
            strHQL = "Update T_ImplePlan Set BelongDepartCode = '" + strBelongDepartCode + "',BelongDepartName = '" + strBelongDepartName + "' Where ID = " + strPlanID;
            ShareClass.RunSqlCommand(strHQL);


            strHQL = "From ProjectRelatedItemBom as projectRelatedItemBom Where projectRelatedItemBom.ProjectID = " + strProjectID + " and projectRelatedItemBom.ParentGuid = " + "'" + strChildGuid + "'" + " and projectRelatedItemBom.VerID = " + strProjectItemBomVerID;
            strHQL += " and projectRelatedItemBom.ParentGuid <> projectRelatedItemBom.ChildGuid";
            strHQL += " Order By projectRelatedItemBom.ID ASC";
            lst2 = projectRelatedItemBomBLL.GetAllProjectRelatedItemBoms(strHQL);
            if (lst2.Count > 0)
            {
                TransferProjectItemBomToPlan(intGanttPID, strProjectID, strProjectItemBomVerID, strChildGuid, dtBeginDate, strPlanVerID, strPlanID, strOperatorCode, strBelongDepartCode, strBelongDepartName, strPMCode, strPMName);
            }
        }
    }

    public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();

        return ts.Days;
    }

    public static string GetParentPlanIDString(string strParentPlanID)
    {
        string strHQL;
        string strParentPlanIDGantt, strParentPlanIDString = "";

        strHQL = "Select Parent_ID From T_ImplePlan Where ID = " + strParentPlanID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ImplePlan");
        strParentPlanIDGantt = ds.Tables[0].Rows[0][0].ToString();

        if (strParentPlanIDGantt != "0")
        {
            strParentPlanIDString += strParentPlanIDGantt + ",";

            strParentPlanIDString += GetParentPlanIDString(strParentPlanIDGantt);
        }

        return strParentPlanIDString;
    }

    //取得GANTT图控件用的项目和计划版本号
    public static int GetPIDForGantt(int intProjectID, int intVerID)
    {
        string strVerID, strPID;

        if (intVerID < 10)
        {
            strVerID = "0" + intVerID.ToString();
        }
        else
        {
            strVerID = intVerID.ToString();
        }

        strPID = intProjectID.ToString() + strVerID;

        return int.Parse(strPID);
    }

    protected void BT_AddPlanVersion_Click(object sender, EventArgs e)
    {
        string strID, strType, strHQL;
        int intVerID;

        intVerID = int.Parse(NB_PlanVerID.Amount.ToString());
        strType = DL_ChangePlanVersionType.SelectedValue.Trim();

        if (intVerID > 100 | intVerID < 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGBBHZNS1100ZJDSZ") + "')", true);
            return;
        }

        if (GetProjectPlanVersionCount(strProjectID, intVerID.ToString()) == 0)
        {
            ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
            ProjectPlanVersion projectPlanVersion = new ProjectPlanVersion();
            projectPlanVersion.VerID = intVerID;
            projectPlanVersion.ProjectID = int.Parse(strProjectID);
            projectPlanVersion.Type = strType;
            projectPlanVersion.CreatorCode = strUserCode;
            projectPlanVersion.CreateTime = DateTime.Now;
            projectPlanVersion.FromProjectID = int.Parse(strProjectID);
            projectPlanVersion.FromProjectPlanVerID = intVerID;

            try
            {
                projectPlanVersionBLL.AddProjectPlanVersion(projectPlanVersion);

                LoadProjectPlanVersion(strProjectID);
                TakeTopPlan.InitialProjectPlanTree(TreeView1, strProjectID, intVerID.ToString());

                strID = DL_PlanVersionID.SelectedValue.Trim();
                strType = DL_ChangePlanVersionType.SelectedValue.Trim();

                if (strType == "InUse")
                {
                    strHQL = "update T_ProjectPlanVersion Set Type = 'Backup' where Type = 'InUse' and ProjectID = " + strProjectID;
                    ShareClass.RunSqlCommand(strHQL);
                }

                if (strType == "Baseline")
                {
                    strHQL = "update T_ProjectPlanVersion Set Type = 'Backup' where Type = 'Baseline' and ProjectID = " + strProjectID;
                    ShareClass.RunSqlCommand(strHQL);
                }


                HL_ProPlanGanttNew.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strProjectID + "&VerID=" + intVerID.ToString();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBCXMZKNCZCBBHJC") + "')", true);
        }
    }

    protected void BT_DeletePlanVersion_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strType, strVerID;

        if (DL_PlanVersionID.Items.Count == 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBMXMBXBLYGJHBB") + "')", true);
            return;
        }

        strVerID = NB_PlanVerID.Amount.ToString();
        strType = DL_ChangePlanVersionType.SelectedValue.Trim();

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.VerID = " + strVerID + " and projectPlanVersion.ProjectID = " + strProjectID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        if (lst.Count > 0)
        {
            ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];

            try
            {
                projectPlanVersionBLL.DeleteProjectPlanVersion(projectPlanVersion);

                strHQL = "delete from T_PlanMember where PlanID in (Select ID From T_ImplePlan where ProjectID = " + strProjectID + " and VerID = " + strVerID + ")";
                ShareClass.RunSqlCommand(strHQL);
                strHQL = "delete from T_ImplePlan where ProjectID = " + strProjectID + " and VerID = " + strVerID;
                ShareClass.RunSqlCommand(strHQL);

                LoadProjectPlanVersion(strProjectID);

                if (DL_PlanVersionID.Items.Count > 0)
                {
                    strVerID = DL_PlanVersionID.SelectedItem.Text.Trim();
                }

                TakeTopPlan.InitialProjectPlanTree(TreeView1, strProjectID, strVerID);

                HL_ProPlanGanttNew.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strProjectID + "&VerID=" + strVerID;


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBKNBCZCBBHJC") + "')", true);
            }
        }
    }

    protected void DL_PlanVersionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strPlanVerID;

        strID = DL_PlanVersionID.SelectedValue.Trim();

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ID = " + strID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);
        ProjectPlanVersion projectPlanVersion = (ProjectPlanVersion)lst[0];

        strPlanVerID = projectPlanVersion.VerID.ToString();

        DL_ChangePlanVersionType.SelectedValue = projectPlanVersion.Type.Trim();

        strPlanVerID = DL_PlanVersionID.SelectedItem.Text.Trim();
        TakeTopPlan.InitialProjectPlanTree(TreeView1, strProjectID, strPlanVerID);

        HL_ProPlanGanttNew.NavigateUrl = "TTWorkPlanGanttForProject.aspx?pid=" + strProjectID + "&VerID=" + strPlanVerID;
    }


    protected int GetProjectPlanVersionCount(string strProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " and projectPlanVersion.VerID =" + strVerID;
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        return lst.Count;
    }

    protected void InitialItemBomTree(string strItemCode, string strVerID, TreeView treeView, string strBelongItemCode, string strBelongVerID)
    {
        string strHQL; ;
        IList lst;

        string strItemName, strUnit;
        decimal deNumber, deReservedNumber;

        //添加根节点
        treeView.Nodes.Clear();

        strHQL = "From ItemBom as itemBom where itemBom.ItemCode = " + "'" + strItemCode + "'" + " and itemBom.ParentItemCode = " + "'" + strItemCode + "'" + " and itemBom.ChildItemCode = " + "'" + strItemCode + "'";
        strHQL += " And itemBom.BelongItemCode = " + "'" + strBelongItemCode + "'" + " And itemBom.BelongVerID = " + strBelongVerID;

        ItemBomBLL itemBomBLL = new ItemBomBLL();
        lst = itemBomBLL.GetAllItemBoms(strHQL);

        if (lst.Count > 0)
        {
            ItemBom itemBom = (ItemBom)lst[0];

            strItemName = GetItemName(strItemCode);
            deNumber = itemBom.Number;
            deReservedNumber = itemBom.ReservedNumber;
            strUnit = itemBom.Unit.Trim();

            TreeNode node3 = new TreeNode();

            node3.Text = "<Strong>" + strItemCode + "." + strItemName + LanguageHandle.GetWord("StrongShuLiang") + deNumber.ToString() + LanguageHandle.GetWord("YuLiuLiang") + deReservedNumber + " " + strUnit + ")";
            node3.Target = Guid.NewGuid().ToString();

            node3.Expanded = true;
            treeView.Nodes.Add(node3);

            ItemBomTreeShowForAll(strVerID, strItemCode, node3, strBelongItemCode, strBelongVerID);

            treeView.DataBind();
        }
    }

    private void ItemBomTreeShowForAll(string strVerID, string strParentItemCode, TreeNode treeNode, string strBelongItemCode, string strBelongVerID)
    {
        string strHQL;
        IList lst1, lst2;

        string strChildItemVerID, strChildItemCode, strChildItemName;
        string strUnit;
        decimal deNumber, deReservedNumber;

        TreeNode node = new TreeNode();
        ItemBom itemBom = new ItemBom();

        strHQL = "From ItemBom as itemBom Where  itemBom.ParentItemCode = " + "'" + strParentItemCode + "'";
        strHQL += " And itemBom.KeyWord <> itemBom.ParentKeyWord";
        strHQL += " And itemBom.BelongItemCode = " + "'" + strBelongItemCode + "'" + " And itemBom.BelongVerID = " + strBelongVerID;
        strHQL += " Order By itemBom.ID ASC";
        ItemBomBLL itemBomBLL = new ItemBomBLL();
        lst1 = itemBomBLL.GetAllItemBoms(strHQL);

        for (int i = 0; i < lst1.Count; i++)
        {
            node = new TreeNode();

            itemBom = (ItemBom)lst1[i];

            strChildItemCode = itemBom.ChildItemCode.Trim();
            strChildItemName = GetItemName(strChildItemCode);
            strChildItemVerID = itemBom.ChildItemVerID.ToString();

            deNumber = itemBom.Number;
            deReservedNumber = itemBom.ReservedNumber;
            strUnit = itemBom.Unit.Trim();

            node.Text = strChildItemCode + "." + strChildItemName + LanguageHandle.GetWord("ShuLiang") + deNumber.ToString() + LanguageHandle.GetWord("YuLiuLiang") + deReservedNumber.ToString() + " " + strUnit + ")";
            node.Target = Guid.NewGuid().ToString();

            node.Expanded = true;
            treeNode.ChildNodes.Add(node);

            strHQL = "From ItemBom as itemBom Where itemBom.ItemCode = " + "'" + strChildItemCode + "'" + " and itemBom.ParentItemCode = " + "'" + strChildItemCode + "'";
            strHQL += " And itemBom.KeyWord <> itemBom.ParentKeyWord";
            strHQL += " And itemBom.BelongItemCode = " + "'" + strBelongItemCode + "'" + " And itemBom.BelongVerID = " + strBelongVerID;
            strHQL += " Order By itemBom.ID ASC";
            lst2 = itemBomBLL.GetAllItemBoms(strHQL);
            if (lst2.Count > 0)
            {
                ItemBomTreeShowForAll(strChildItemVerID, strChildItemCode, node, strBelongItemCode, strBelongVerID);
            }
        }
    }

    protected void LoadProjectItemBomVersion(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectItemBomVersion as projectItemBomVersion Where projectItemBomVersion.ProjectID = " + strProjectID + " Order by projectItemBomVersion.VerID DESC";
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);

        DL_ChangeProjectItemBomVersionID.DataSource = lst;
        DL_ChangeProjectItemBomVersionID.DataBind();

        DL_OldVersionID.DataSource = lst;
        DL_OldVersionID.DataBind();

        DL_NewVersionID.DataSource = lst;
        DL_NewVersionID.DataBind();
    }

    protected int GetProjectItemBomVersionCount(string strProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ProjectID = " + strProjectID + " and projectItemBomVersion.VerID =" + strVerID;
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);

        return lst.Count;
    }


    protected string GetItemBomVersionType(string strItemCode, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion as itemBomVersion where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " and itemBomVersion.VerID = " + strVerID;
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);
        ItemBomVersion itemBomVersion = (ItemBomVersion)lst[0];

        return itemBomVersion.Type.Trim();
    }

    protected string GetProjectItemBomVersionType(string strProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ProjectID = " + strProjectID + " and projectItemBomVersion.VerID = " + strVerID;
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);
        ProjectItemBomVersion projectItemBomVersion = (ProjectItemBomVersion)lst[0];

        return projectItemBomVersion.Type.Trim();
    }

    protected string GetProjectItemBomVersionID(string strProjectID, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ProjectID = " + strProjectID + " and projectItemBomVersion.VerID = " + strVerID;
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);
        ProjectItemBomVersion projectItemBomVersion = (ProjectItemBomVersion)lst[0];

        return projectItemBomVersion.ID.ToString();
    }

    protected string GetProjectItemBomVersionIDByType(string strProjectID, string strType)
    {
        string strHQL;
        IList lst;

        strHQL = "From ProjectItemBomVersion as projectItemBomVersion where projectItemBomVersion.ProjectID = " + strProjectID + " and projectItemBomVersion.Type = " + "'" + strType + "'";
        ProjectItemBomVersionBLL projectItemBomVersionBLL = new ProjectItemBomVersionBLL();
        lst = projectItemBomVersionBLL.GetAllProjectItemBomVersions(strHQL);

        if (lst.Count > 0)
        {
            ProjectItemBomVersion projectItemBomVersion = (ProjectItemBomVersion)lst[0];
            return projectItemBomVersion.ID.ToString();
        }
        else
        {
            return "0";
        }
    }

    protected string GetItemBomID(string strItemCode, string strVerID)
    {
        string strHQL;
        IList lst;

        strHQL = "From ItemBomVersion as itemBomVersion where itemBomVersion.ItemCode = " + "'" + strItemCode + "'" + " and itemBomVersion.VerID = " + strVerID;
        ItemBomVersionBLL itemBomVersionBLL = new ItemBomVersionBLL();
        lst = itemBomVersionBLL.GetAllItemBomVersions(strHQL);
        ItemBomVersion itemBomVersion = (ItemBomVersion)lst[0];

        return itemBomVersion.ID.ToString();
    }

    protected void LoadProjectPlanVersion(string strProjectID)
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectPlanVersion as projectPlanVersion where projectPlanVersion.ProjectID = " + strProjectID + " Order by projectPlanVersion.VerID DESC";
        ProjectPlanVersionBLL projectPlanVersionBLL = new ProjectPlanVersionBLL();
        lst = projectPlanVersionBLL.GetAllProjectPlanVersions(strHQL);

        DL_PlanVersionID.DataSource = lst;
        DL_PlanVersionID.DataBind();
    }

    protected string GetProjectSpecification(string strProjectID)
    {
        string strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        return project.ProjectDetail.Trim();
    }

    protected Item GetItem(string strItemCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Item as item where item.ItemCode = " + "'" + strItemCode + "'";
        ItemBLL itemBLL = new ItemBLL();
        lst = itemBLL.GetAllItems(strHQL);

        Item item = (Item)lst[0];

        return item;
    }

    protected string GetItemName(string strItemCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From Item as item where item.ItemCode = " + "'" + strItemCode + "'";
        ItemBLL itemBLL = new ItemBLL();
        lst = itemBLL.GetAllItems(strHQL);

        Item item = (Item)lst[0];

        return item.ItemName.Trim();
    }

}
