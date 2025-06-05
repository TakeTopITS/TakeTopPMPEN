using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTKPICheckForUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strUserCode = Session["UserCode"].ToString();
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "员工KPI设置", strUserCode);

        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "员工KPI设置---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_EndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_StartTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

            DLC_EndTimeNew.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DLC_StartTimeNew.Text = DateTime.Now.ToString("yyyy-MM-dd");

            LoadKPIType();

            TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);

            TakeTopCore.CoreShareClass.InitialDepartmentPositionTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"), TreeView3, strUserCode);
            ShareClass.InitialKPITree(TreeView4);
        }
    }

    protected void BT_CloseAllUserPastKPI_Click(object sender, EventArgs e)
    {
        string strHQL;

        strHQL = "Update T_UserKPICheck Set Status = 'Closed' where EndTime < now()";

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZGBSBJC") + "')", true);
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strPosition = "";
        string strUserCode = Session["UserCode"].ToString();

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            LB_DepartCode.Text = strDepartCode;
            LB_Position.Text = strPosition;

            //生成本部门的职称树
            TreeView3.Nodes.Clear();
            TreeNode treeNode3 = new TreeNode();
            treeNode3.Text = "<B>" + LanguageHandle.GetWord("ZZJGT") + "</B>";
            treeNode3.Target = "0";
            treeNode3.Expanded = true;
            TreeView3.Nodes.Add(treeNode3);
            AllDepartmentPositionTreeShow(strDepartCode, treeNode3);

            ShareClass.InitialKPICheckTreeByDepartPosition(TreeView2, strDepartCode, strPosition);
        }
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strKPICheckID, strKPICheckName, strPriorKPICheckID;
        string strUserCode, strUserName;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            if (treeNode.Depth == 1)
            {
                strUserCode = treeNode.Target.Trim();
                strUserName = treeNode.Text.Trim();

                LB_UserCode.Text = strUserCode;
                LB_UserName.Text = strUserName;

                LB_Position.Text = ShareClass.GetUserJobTitle(strUserCode);

            }
            else
            {
                strUserCode = treeNode.Parent.Target.Trim();
                strUserName = treeNode.Parent.Text.Trim();

                LB_Position.Text = ShareClass.GetUserJobTitle(strUserCode);

                strKPICheckID = treeNode.Target.Trim();
                strKPICheckName = treeNode.Text.Trim();

                LB_KPICheckID.Text = strKPICheckID;
                TB_KPICheckName.Text = strKPICheckName;

                strHQL = "From UserKPICheck as userKPICheck Where userKPICheck.KPICheckID = " + strKPICheckID;
                UserKPICheckBLL userKPICheckBLL = new UserKPICheckBLL();
                lst = userKPICheckBLL.GetAllUserKPIChecks(strHQL);

                UserKPICheck userKPICheck = (UserKPICheck)lst[0];

                NB_TotalSelfPoint.Amount = userKPICheck.TotalSelfPoint;
                NB_TotalLeaderPoint.Amount = userKPICheck.TotalLeaderPoint;
                NB_TotalThirdPartPoint.Amount = userKPICheck.TotalThirdPartPoint;
                NB_TotalSqlPoint.Amount = userKPICheck.TotalSqlPoint;
                NB_TotalHRPoint.Amount = userKPICheck.TotalHRPoint;
                NB_TotalPoint.Amount = userKPICheck.TotalPoint;

                DLC_StartTime.Text = userKPICheck.StartTime.ToString("yyyy-MM-dd");
                DLC_EndTime.Text = userKPICheck.EndTime.ToString("yyyy-MM-dd");
                DL_Status.SelectedValue = userKPICheck.Status.Trim();

                LoadKPI(strKPICheckID);

                LB_UserCode.Text = strUserCode;
                LB_UserName.Text = strUserName;

                BT_UpdateKPICheck.Enabled = true;
                BT_DeleteKPICheck.Enabled = true;

                strHQL = "From UserKPICheckDetail as userKPICheckDetail Where userKPICheckDetail.KPICheckID = " + strKPICheckID;
                UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
                lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);
                if (lst.Count == 0)
                {
                    strHQL = "Select Max(KPICheckID) From T_UserKPICheck Where KPICheckID < " + strKPICheckID + " and UserCode = " + "'" + strUserCode + "'";
                    DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheck");
                    strPriorKPICheckID = ds.Tables[0].Rows[0][0].ToString();

                    if (strPriorKPICheckID != "")
                    {
                        BT_CopyFromPriorKPI.Enabled = true;
                    }
                    else
                    {
                        BT_CopyFromPriorKPI.Enabled = false;
                    }
                }
            }
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;

        string strKPICheckID;
        string strOperatorCode, strOperatorName, strUserCode, strUserName;
        string strDepartCode, strPosition, strTemDepartCode, strTemDepartName, strTemPosition;

        strOperatorCode = Session["UserCode"].ToString().Trim();
        strOperatorName = ShareClass.GetUserName(strOperatorCode);

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        strKPICheckID = LB_KPICheckID.Text.Trim();

        if (strUserCode != "" & strUserName != "")
        {
            TreeNode treeNode = new TreeNode();
            treeNode = TreeView3.SelectedNode;

            if (treeNode.ChildNodes.Count == 0)
            {
                try
                {
                    strTemDepartCode = treeNode.Target.Trim();
                    strTemDepartName = ShareClass.GetDepartName(strTemDepartCode);
                }
                catch
                {
                    strTemDepartCode = treeNode.Parent.Target.Trim();
                    strTemPosition = treeNode.Text.Trim();

                    if (strDepartCode == strTemDepartCode & strPosition == strTemPosition)
                    {
                        strHQL = "Insert Into T_UserKPICheckDetail(KPICheckID,SortNumber,KPI,KPIType,Definition,KPIFunction,Formula,SqlCode,UnitSqlPoint,Source,Weight,Target,SelfPoint,LeaderPoint,ThirdPartPoint,SqlPoint,HRPoint,Point,Comment,OperatorCode,OperatorName,SelfComment,LeaderComment,TotalComment)";
                        strHQL += " Select " + strKPICheckID + ",SortNumber,KPI,KPIType,Definition,KPIFunction,Formula,SqlCode,UnitSqlPoint,Source,Weight,'',100,100,100,100,100,100,''," + "'" + strOperatorCode + "'" + "," + "'" + strOperatorName + "'" + ",'','',''" + " From T_KPITemplateForDepartPosition Where DepartCode = " + "'" + strDepartCode + "'" + " and Position = " + "'" + strPosition + "'";
                        strHQL += " and KPI not in (Select KPI From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID + ")";

                        ShareClass.RunSqlCommand(strHQL);

                        LoadKPI(strKPICheckID);
                    }
                }
            }
        }
    }

    protected void TreeView4_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;

        string strKPICheckID, strKPIID, strKPI;
        string strOperatorCode, strOperatorName, strUserCode, strUserName;
        string strDepartCode, strPosition;

        strOperatorCode = Session["UserCode"].ToString().Trim();
        strOperatorName = ShareClass.GetUserName(strOperatorCode);

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        strKPICheckID = LB_KPICheckID.Text.Trim();

        if (strDepartCode != "" & strPosition != "")
        {
            TreeNode treeNode = new TreeNode();
            treeNode = TreeView4.SelectedNode;

            if (treeNode.Depth == 2)
            {
                strKPIID = treeNode.Target.Trim();
                strKPI = treeNode.Text.Trim();

                strHQL = "Insert Into T_UserKPICheckDetail(KPICheckID,SortNumber,KPI,KPIType,Definition,KPIFunction,Formula,SqlCode,UnitSqlPoint,Source,Target,Weight,SelfPoint,LeaderPoint,ThirdPartPoint,SqlPoint,HRPoint,Point,Comment,OperatorCode,OperatorName,SelfComment,LeaderComment,TotalComment)";
                strHQL += " Select " + strKPICheckID + ",SortNumber,KPI,KPIType,Definition,KPIFunction,Formula,SqlCode,UnitSqlPoint,Source,'',0,100,100,100,100,100,100,'','" + strOperatorCode + "'" + "," + "'" + strOperatorName + "'" + ",'','',''" + " From  T_KPILibrary Where ID = " + strKPIID;
                strHQL += " and KPI not in (Select KPI From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID + ")";
                ShareClass.RunSqlCommand(strHQL);

                LoadKPI(strKPICheckID);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBZYGMYSZZCQJC") + "')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            string strDepartCode = LB_DepartCode.Text.Trim();
            string strPosition = LB_Position.Text.Trim();

            string strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }
                e.Item.ForeColor = Color.Red;

                strHQL = "From UserKPICheckDetail as userKPICheckDetail Where userKPICheckDetail.ID = " + strID;
                UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
                lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);

                UserKPICheckDetail userKPICheckDetail = (UserKPICheckDetail)lst[0];

                LB_KPIID.Text = strID;
                TB_KPI.Text = userKPICheckDetail.KPI.Trim();
                DL_KPIType.SelectedValue = userKPICheckDetail.KPIType;

                NB_Weight.Amount = userKPICheckDetail.Weight;
                TB_Formula.Text = userKPICheckDetail.Formula.Trim();
                TB_SqlCode.Text = userKPICheckDetail.SqlCode.Trim();
                NB_UnitSqlPoint.Amount = userKPICheckDetail.UnitSqlPoint;
                TB_Source.Text = userKPICheckDetail.Source.Trim();
                NB_SortNubmer.Amount = userKPICheckDetail.SortNumber;

                TB_Target.Text = userKPICheckDetail.Target.Trim();

                HL_RelatedUser.NavigateUrl = "TTKPIThirdPartReviewUser.aspx?UserKPIID=" + LB_KPIID.Text;
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strKPICheckID = LB_KPICheckID.Text.Trim();

                strDepartCode = LB_DepartCode.Text.Trim();
                strPosition = LB_Position.Text.Trim();

                strHQL = "Delete From T_UserKPICheckDetail Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    LoadKPI(strKPICheckID);


                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }

    protected void BT_RelatedUser_Click(object sender, EventArgs e)
    {
        string strKPIID, strIMTitle;
        string strJavaScriptFuntion;
        string strMessage;

        strKPIID = LB_KPIID.Text.Trim();

        strIMTitle = "KPI：" + strKPIID + LanguageHandle.GetWord("PingHeRen");

        
        strMessage = "TTKPIThirdPartReviewUser.aspx?UserKPIID=" + strKPIID;
        strJavaScriptFuntion = "opim(" + "'" + strKPIID + "'" + "," + "'" + strIMTitle + "'" + "," + "'" + strMessage + "'" + ");";
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", strJavaScriptFuntion, true);
    }

    protected void DataGrid2_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid2.CurrentPageIndex = e.NewPageIndex;

        string strHQL = LB_Sql.Text;
        IList lst;

        UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
        lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();
    }

    protected void BT_CreateKPICheck_Click(object sender, EventArgs e)
    {
        LB_KPICheckID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popKPICheckWindow','false') ", true);
    }


    protected void BT_NewKPICheck_Click(object sender, EventArgs e)
    {
        string strKPICheckID;

        strKPICheckID = LB_KPICheckID.Text;

        if (strKPICheckID == "")
        {
            AddKPICheck();
        }
    }

    protected void AddKPICheck()
    {
        string strHQL;

        string strKPICheckID, strKPICheckName, strPriorKPICheckID, strUserName, strUserCode, strCreatorCode, strCreatorName, strDepartCode, strPosition;
        DateTime dtStartTime, dtEndTime;

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        strCreatorCode = Session["UserCode"].ToString().Trim();
        strCreatorName = ShareClass.GetUserName(strCreatorCode);

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        strKPICheckName = TB_KPICheckNameNew.Text.Trim();
        dtStartTime = DateTime.Parse(DLC_StartTimeNew.Text);
        dtEndTime = DateTime.Parse(DLC_EndTimeNew.Text);

        UserKPICheckBLL userKPICheckBLL = new UserKPICheckBLL();
        UserKPICheck userKPICheck = new UserKPICheck();

        userKPICheck.KPICheckName = strKPICheckName;
        userKPICheck.StartTime = dtStartTime;
        userKPICheck.EndTime = dtEndTime;
        userKPICheck.TotalSelfPoint = 100;
        userKPICheck.TotalLeaderPoint = 100;
        userKPICheck.TotalThirdPartPoint = 100;
        userKPICheck.TotalSqlPoint = 100;
        userKPICheck.TotalHRPoint = 100;
        userKPICheck.TotalPoint = 100;
        userKPICheck.UserCode = strUserCode;
        userKPICheck.UserName = strUserName;
        userKPICheck.CreatorCode = strCreatorCode;
        userKPICheck.CreatorName = strCreatorName;
        userKPICheck.Status = "InProgress";

        try
        {
            userKPICheckBLL.AddUserKPICheck(userKPICheck);


            TB_KPICheckName.Text = TB_KPICheckNameNew.Text.Trim();
            DLC_StartTime.Text = DLC_StartTimeNew.Text;
            DLC_EndTime.Text = DLC_EndTimeNew.Text;



            strKPICheckID = ShareClass.GetMyCreatedMaxUserKPICheckID();

            LB_KPICheckID.Text = strKPICheckID;

            ShareClass.InitialKPICheckTreeByDepartPosition(TreeView2, strDepartCode, strPosition);

            DL_Status.SelectedValue = "InProgress";

            LoadKPI(strKPICheckID);

            BT_UpdateKPICheck.Enabled = true;
            BT_DeleteKPICheck.Enabled = true;

            strHQL = "Select Max(KPICheckID) From T_UserKPICheck Where KPICheckID < " + strKPICheckID + " and UserCode = " + "'" + strUserCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheck");
            strPriorKPICheckID = ds.Tables[0].Rows[0][0].ToString();

            if (strPriorKPICheckID != "")
            {
                BT_CopyFromPriorKPI.Enabled = true;
            }
            else
            {
                BT_CopyFromPriorKPI.Enabled = false;
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popKPICheckWindow','true') ", true);
        }
    }

    protected void BT_UpdateKPICheck_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strKPICheckID, strKPICheckName, strUserName, strUserCode, strCreatorCode, strCreatorName, strDepartCode, strPosition, strStatus;
        DateTime dtStartTime, dtEndTime;

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        strCreatorCode = Session["UserCode"].ToString().Trim();
        strCreatorName = ShareClass.GetUserName(strCreatorCode);

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        strKPICheckID = LB_KPICheckID.Text.Trim();
        strKPICheckName = TB_KPICheckName.Text.Trim();
        dtStartTime = DateTime.Parse(DLC_StartTime.Text);
        dtEndTime = DateTime.Parse(DLC_EndTime.Text);

        strStatus = DL_Status.SelectedValue;

        strHQL = "From UserKPICheck as userKPICheck Where userKPICheck.KPICheckID = " + strKPICheckID;
        UserKPICheckBLL userKPICheckBLL = new UserKPICheckBLL();
        lst = userKPICheckBLL.GetAllUserKPIChecks(strHQL);

        UserKPICheck userKPICheck = (UserKPICheck)lst[0];

        userKPICheck.KPICheckName = strKPICheckName;
        userKPICheck.StartTime = dtStartTime;
        userKPICheck.EndTime = dtEndTime;
        userKPICheck.UserCode = strUserCode;
        userKPICheck.UserName = strUserName;
        userKPICheck.CreatorCode = strCreatorCode;
        userKPICheck.CreatorName = strCreatorName;
        userKPICheck.Status = strStatus;

        try
        {
            userKPICheckBLL.UpdateUserKPICheck(userKPICheck, int.Parse(strKPICheckID));

            ShareClass.InitialKPICheckTreeByDepartPosition(TreeView2, strDepartCode, strPosition);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJC") + "')", true);
        }
    }

    protected void BT_DeleteKPICheck_Click(object sender, EventArgs e)
    {
        string strHQL;

        string strKPICheckID, strDepartCode, strPosition;

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        strKPICheckID = LB_KPICheckID.Text.Trim();

        strHQL = "Delete From T_UserKPICheck Where KPICheckID = " + strKPICheckID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            strHQL = "Delete From T_UserKPICheckDetail Where KPICheckID = " + strKPICheckID;
            ShareClass.RunSqlCommand(strHQL);

            ShareClass.InitialKPICheckTreeByDepartPosition(TreeView2, strDepartCode, strPosition);

            BT_UpdateKPICheck.Enabled = false;
            BT_DeleteKPICheck.Enabled = false;

            BT_CopyFromPriorKPI.Enabled = false;

            LoadKPI("0");

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
        }
    }

    protected void BT_CopyFromPriorKPI_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode;
        string strKPICheckID, strPriorKPICheckID;

        strUserCode = LB_UserCode.Text.Trim();

        strKPICheckID = LB_KPICheckID.Text.Trim();

        strHQL = "From UserKPICheckDetail as userKPICheckDetail Where userKPICheckDetail.KPICheckID = " + strKPICheckID;
        UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
        lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);
        if (lst.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZSBCKHYCZKHZBBNFZJC") + "')", true);
            return;
        }

        try
        {
            strHQL = "Select Max(KPICheckID) From T_UserKPICheck Where KPICheckID < " + strKPICheckID + " and UserCode = " + "'" + strUserCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheck");

            strPriorKPICheckID = ds.Tables[0].Rows[0][0].ToString();

            if (strPriorKPICheckID != "")
            {
                strHQL = "Insert Into T_UserKPICheckDetail(KPICheckID,KPI,KPIType,Definition,KPIFunction,Formula,Source,Weight ,Target,";
                strHQL += " SelfPoint,LeaderPoint,ThirdPartPoint,SqlPoint,HRPoint,Point,SqlCode,UnitSqlPoint,SortNumber,OperatorCode,OperatorName,SelfComment,LeaderComment,ThirdPartComment,HRComment,TotalComment)";
                strHQL += " Select " + strKPICheckID + ",KPI,KPIType,Definition,KPIFunction,Formula,Source,Weight ,Target,";
                strHQL += " SelfPoint,LeaderPoint,ThirdPartPoint,SqlPoint,HRPoint,Point,SqlCode,UnitSqlPoint,SortNumber,OperatorCode,OperatorName,SelfComment,LeaderComment,ThirdPartComment,HRComment,TotalComment";
                strHQL += " From T_UserKPICheckDetail Where KPICheckID = " + strPriorKPICheckID;
                ShareClass.RunSqlCommand(strHQL);

                strHQL = "Insert Into T_KPIThirdPartReview(UserKPIID,UserCode,UserName,Comment,Point,ReviewTime) ";
                strHQL += " Select C.ID,A.UserCode,A.UserName,A.Comment,A.Point,A.ReviewTime From T_KPIThirdPartReview A,T_UserKPICheckDetail B,T_UserKPICheckDetail C ";
                strHQL += " Where A.UserKPIID = B.ID and B.KPICheckID = " + strPriorKPICheckID;
                strHQL += " and B.KPI = C.KPI and C.KPICheckID = " + strKPICheckID;
                ShareClass.RunSqlCommand(strHQL);

                LoadKPI(strKPICheckID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFZCG") + "')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCYGBCZCJXKHWFFZJC") + "')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWFZSBJC") + "')", true);
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_KPIID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strKPIID;

        strKPIID = LB_KPIID.Text;

        if (strKPIID == "")
        {
            AddKPI();
        }
        else
        {
            UpdateKPI();
        }
    }

    protected void AddKPI()
    {
        string strKPICheckID;
        string strDepartCode, strPosition;
        string strKPIID, strKPI, strKPIType, strFormula, strSqlCode, strSource, strTarget;
        int intSortNumber;
        decimal deWeight;

        string strUserCode = Session["UserCode"].ToString();
        string strUserName = ShareClass.GetUserName(strUserCode);

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        strKPICheckID = LB_KPICheckID.Text.Trim();
        strKPI = TB_KPI.Text.Trim();
        strKPIType = DL_KPIType.SelectedValue.Trim();
        strFormula = TB_Formula.Text.Trim();
        strSqlCode = TB_SqlCode.Text.Trim();
        strSource = TB_Source.Text.Trim();
        strTarget = TB_Target.Text.Trim();


        deWeight = NB_Weight.Amount;
        if (deWeight > 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBNDY1JC") + "')", true);
            return;
        }

        if (strSqlCode.ToUpper().IndexOf("DELETE") > -1 | strSqlCode.ToUpper().IndexOf("UPDATE") > -1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLDMBNHDELETEHUPDATEYJJC") + "')", true);
            return;
        }

        intSortNumber = int.Parse(NB_SortNubmer.Amount.ToString());

        if (strKPI != "" | strKPIType != "")
        {
            UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
            UserKPICheckDetail userKPICheckDetail = new UserKPICheckDetail();

            try
            {
                userKPICheckDetail.KPICheckID = int.Parse(strKPICheckID);
                userKPICheckDetail.KPI = strKPI;
                userKPICheckDetail.KPIType = strKPIType;

                userKPICheckDetail.Formula = strFormula;
                userKPICheckDetail.SqlCode = strSqlCode;
                userKPICheckDetail.UnitSqlPoint = NB_UnitSqlPoint.Amount;

                userKPICheckDetail.Source = strSource;
                userKPICheckDetail.Target = strTarget;
                userKPICheckDetail.SortNumber = intSortNumber;
                userKPICheckDetail.Weight = deWeight;


                userKPICheckDetail.SelfPoint = 100;
                userKPICheckDetail.LeaderPoint = 100;
                userKPICheckDetail.ThirdPartPoint = 100;
                userKPICheckDetail.SqlPoint = 100;
                userKPICheckDetail.HRPoint = 100;
                userKPICheckDetail.Point = 100;

                userKPICheckDetail.OperatorCode = strUserCode;
                userKPICheckDetail.OperatorName = strUserName;

                userKPICheckDetail.HRComment = "";
                userKPICheckDetail.SelfComment = "";
                userKPICheckDetail.LeaderComment = "";
                userKPICheckDetail.HRComment = "";
                userKPICheckDetail.TotalComment = "";


                userKPICheckDetailBLL.AddUserKPICheckDetail(userKPICheckDetail);

                strKPIID = ShareClass.GetMyCreatedMaxUserKPICheckDetailID();
                LB_KPIID.Text = strKPIID;

                LoadKPI(strKPICheckID);


                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXBNWKJC") + "')", true);
        }
    }

    protected void UpdateKPI()
    {
        string strHQL;
        IList lst;

        string strKPICheckID;
        string strDepartCode, strPosition;
        string strKPIID, strKPI, strKPIType, strFormula, strSqlCode, strSource, strTarget;
        int intSortNumber;
        decimal deWeight;

        strDepartCode = LB_DepartCode.Text.Trim();
        strPosition = LB_Position.Text.Trim();

        strKPICheckID = LB_KPICheckID.Text.Trim();
        strKPIID = LB_KPIID.Text.Trim();
        strKPI = TB_KPI.Text.Trim();
        strKPIType = DL_KPIType.SelectedValue.Trim();

        strFormula = TB_Formula.Text.Trim();
        strSqlCode = TB_SqlCode.Text.Trim();

        strSource = TB_Source.Text.Trim();
        strTarget = TB_Target.Text.Trim();

        deWeight = NB_Weight.Amount;
        if (deWeight > 1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGZBNDY1JC") + "')", true);
            return;
        }

        if (strSqlCode.ToUpper().IndexOf("DELETE") > -1 | strSqlCode.ToUpper().IndexOf("UPDATE") > -1)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGSQLDMBNHDELETEHUPDATEYJJC") + "')", true);
            return;
        }

        intSortNumber = int.Parse(NB_SortNubmer.Amount.ToString());

        if (strKPI != "" | strKPIType != "")
        {
            strHQL = "From UserKPICheckDetail as userKPICheckDetail Where userKPICheckDetail.ID = " + strKPIID;
            UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
            lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);
            UserKPICheckDetail userKPICheckDetail = (UserKPICheckDetail)lst[0];

            try
            {
                userKPICheckDetail.KPICheckID = int.Parse(strKPICheckID);
                userKPICheckDetail.KPI = strKPI;
                userKPICheckDetail.KPIType = strKPIType;

                userKPICheckDetail.Formula = strFormula;
                userKPICheckDetail.SqlCode = strSqlCode;
                userKPICheckDetail.UnitSqlPoint = NB_UnitSqlPoint.Amount;

                userKPICheckDetail.Source = strSource;
                userKPICheckDetail.Target = strTarget;
                userKPICheckDetail.SortNumber = intSortNumber;
                userKPICheckDetail.Weight = deWeight;

                userKPICheckDetailBLL.UpdateUserKPICheckDetail(userKPICheckDetail, int.Parse(strKPIID));

                LoadKPI(strKPICheckID);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXBNWKJC") + "')", true);
        }
    }

    protected string SumKPIWeight(string strKPICheckID)
    {
        string strHQL;
        strHQL = " Select Sum(Weight) From T_UserKPICheckDetail where KPICheckID = " + strKPICheckID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_UserKPICheckDetail");

        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }
    }

    protected void LoadKPI(string strKPICheckID)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserKPICheckDetail as userKPICheckDetail where userKPICheckDetail.KPICheckID = " + strKPICheckID;
        strHQL += " Order by userKPICheckDetail.SortNumber ASC";

        UserKPICheckDetailBLL userKPICheckDetailBLL = new UserKPICheckDetailBLL();
        lst = userKPICheckDetailBLL.GetAllUserKPICheckDetails(strHQL);

        DataGrid2.DataSource = lst;
        DataGrid2.DataBind();

        LB_Sql.Text = strHQL;

        LB_Weight.Text = SumKPIWeight(strKPICheckID);
    }

    protected void LoadKPIType()
    {
        string strHQL;
        IList lst;

        strHQL = "From KPIType as kpiType Order By kpiType.SortNumber ASC";
        KPITypeBLL kpiTypeBLL = new KPITypeBLL();
        lst = kpiTypeBLL.GetAllKPITypes(strHQL);

        DL_KPIType.DataSource = lst;
        DL_KPIType.DataBind();
    }

    public void AllDepartmentPositionTreeShow(string strParentCode, TreeNode treeNode)
    {
        string strHQL1, strHQL2;
        IList lst1, lst2;

        TreeNode node1, node2;

        string strDepartCode, strDepartName, strPosition;

        strHQL1 = "from Department as department where department.DepartCode = '" + strParentCode + "'";

        DepartPositionBLL departPositionBLL = new DepartPositionBLL();
        DepartPosition departPosition = new DepartPosition();

        DepartmentBLL departmentBLL = new DepartmentBLL();
        Department department = new Department();
        lst1 = departmentBLL.GetAllDepartments(strHQL1);

        department = (Department)lst1[0];

        strDepartCode = department.DepartCode.Trim();
        strDepartName = department.DepartName.Trim();

        node1 = new TreeNode();
        node1.Target = strDepartCode;
        node1.Text = strDepartCode + " " + strDepartName;
        treeNode.ChildNodes.Add(node1);
        node1.Expanded = true;

        strHQL2 = "From DepartPosition as departPosition Order By SortNumber ASC";
        lst2 = departPositionBLL.GetAllDepartPositions(strHQL2);
        for (int j = 0; j < lst2.Count; j++)
        {
            departPosition = (DepartPosition)lst2[j];

            strPosition = departPosition.Position.Trim();

            node2 = new TreeNode();
            node2.Target = departPosition.ID.ToString();
            node2.Text = strPosition;
            node1.ChildNodes.Add(node2);
            node2.Expanded = false;
        }
    }

}
