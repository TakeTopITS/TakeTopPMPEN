using System;
using System.Resources;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Security.Permissions;
using System.Data.SqlClient;

using System.ComponentModel;
using System.Web.SessionState;
using System.Drawing.Imaging;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMemberLevelSet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;
        string strDepartString;

        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();

        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "直接成员层次设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        //this.Title = "直接成员层次设置---" + System.Configuration.ConfigurationManager.AppSettings["SystemName"];

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "AdjustDivHeight();", true);
        //ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll2", "SaveScroll(Div_TreeView2);");
        ScriptManager.RegisterOnSubmitStatement(this.Page, this.Page.GetType(), "SavePanelScroll3", "SaveScroll(Div_TreeView3);");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"),TreeView3, strUserCode);

            strDepartString = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"),TreeView2, strUserCode);
            LB_DepartString.Text = strDepartString;

            InitialMemberTree("", strDepartString);

            LB_DepartCode.Text = "";
        }
    }

    protected void TreeView3_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode, strDepartString;

        strDepartString = LB_DepartString.Text.Trim();


        TreeNode treeNode = new TreeNode();
        treeNode = TreeView3.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();
            InitialMemberTree(strDepartCode, strDepartString);
            LB_DepartCode.Text = strDepartCode;
        }
        else
        {
            InitialMemberTree("", strDepartString);
            LB_DepartCode.Text = "";
        }

        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll(EndRequestHandler3);", true);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strDepartCode;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView2.SelectedNode;

        if (treeNode.Target != "0")
        {
            strDepartCode = treeNode.Target.Trim();

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid3);
        }


       //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetPanelScroll", "RestoreScroll(EndRequestHandler2);", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strUnderCode = TB_UnderCode.Text.Trim();
        string strUnderName = TB_UnderName.Text.Trim();
        string strUserCode = TB_UserCode.Text.Trim();
        string strID = LB_ID.Text.Trim();
        string strProjectVisible = DL_ProjectVisible.SelectedValue.Trim();
        string strPlanVisible = DL_PlanVisible.SelectedValue.Trim();
        string strKPIVisible = DL_KPIVisible.SelectedValue.Trim();
        string strWorkloadVisible = DL_WorkloadVisible.SelectedValue.Trim();
        string strScheduleVisible = DL_ScheduleVisible.SelectedValue.Trim();
        string strWorkflowVisible = DL_WorkflowVisible.SelectedValue.Trim();
        string strCustomerServiceVisible = DL_CustomerServiceVisible.SelectedValue.Trim();
        string strConstractVisible = DL_ConstractVisible.SelectedValue.Trim();
        string strPositionVisible = DL_PositionVisible.SelectedValue.Trim();

        string strDepartCode = LB_DepartCode.Text.Trim();
        string strDepartString = LB_DepartString.Text.Trim();

        if (GetUnderUserCount(strUserCode, strUnderCode) > 0)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGCZJCYYCZBNZFZJ") + "')", true);
            return;
        }

        MemberLevelBLL memberLevelBLL = new MemberLevelBLL();
        MemberLevel memberLevel = new MemberLevel();

        if (strUserCode != "" & strUnderCode != "")
        {
            if (strID == "0")
            {
                memberLevel.UserCode = strUnderCode;
            }
            else
            {
                memberLevel.UserCode = strUserCode;
            }

            memberLevel.UnderCode = strUnderCode;
            memberLevel.AgencyStatus = 0;
            memberLevel.ProjectVisible = strProjectVisible;
            memberLevel.PlanVisible = strPlanVisible;
            memberLevel.KPIVisible = strKPIVisible;
            memberLevel.WorkloadVisible = strWorkloadVisible;
            memberLevel.ScheduleVisible = strScheduleVisible;
            memberLevel.WorkflowVisible = strWorkflowVisible;
            memberLevel.CustomerServiceVisible = strCustomerServiceVisible;
            memberLevel.ConstractVisible = strConstractVisible;
            memberLevel.PositionVisible = strPositionVisible;


            try
            {
                memberLevelBLL.AddMemberLevel(memberLevel);

                if (strUserCode != "0")
                {
                    LB_ID.Text = ShareClass.GetMyCreatedMaxMemberLevelID();
                }
                else
                {
                    LB_ID.Text = "0";
                }

                LB_ChildCount.Text = "0";


                BT_Update.Enabled = true;
                BT_Delete.Enabled = true;

                InitialMemberTree(strDepartCode, strDepartString);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBJCDMZFHMXWK") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZSBDMHFDMBNWK") + "')", true);
        }
    }

    protected void BT_Update_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = LB_ID.Text.Trim();

        string strUnderCode = TB_UnderCode.Text.Trim();
        string strUnderName = TB_UnderName.Text.Trim();
        string strUserCode = TB_UserCode.Text.Trim();

        string strProjectVisible = DL_ProjectVisible.SelectedValue.Trim();
        string strPlanVisible = DL_PlanVisible.SelectedValue.Trim();
        string strKPIVisible = DL_KPIVisible.SelectedValue.Trim();
        string strWorkloadVisible = DL_WorkloadVisible.SelectedValue.Trim();
        string strScheduleVisible = DL_ScheduleVisible.SelectedValue.Trim();
        string strWorkflowVisible = DL_WorkflowVisible.SelectedValue.Trim();
        string strCustomerServiceVisible = DL_CustomerServiceVisible.SelectedValue.Trim();
        string strConstractVisible = DL_ConstractVisible.SelectedValue.Trim();
        string strPositionVisible = DL_PositionVisible.SelectedValue.Trim();

        string strDepartCode = LB_DepartCode.Text.Trim();
        string strDepartString = LB_DepartString.Text.Trim();

        if (strID == "0")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSBCZBCCSZYXGDJD") + "')", true);
            return;
        }

        if (strUserCode != "" & strUnderCode != "")
        {
            MemberLevelBLL memberLevelBLL = new MemberLevelBLL();
            strHQL = "from MemberLevel as memberLevel where memberLevel.ID = " + strID;
            lst = memberLevelBLL.GetAllMemberLevels(strHQL);
            MemberLevel memberLevel = (MemberLevel)lst[0];

            memberLevel.UserCode = strUserCode;
            memberLevel.UnderCode = strUnderCode;
            memberLevel.AgencyStatus = 0;
            memberLevel.ProjectVisible = strProjectVisible;
            memberLevel.PlanVisible = strPlanVisible;
            memberLevel.KPIVisible = strKPIVisible;
            memberLevel.WorkloadVisible = strWorkloadVisible;
            memberLevel.ScheduleVisible = strScheduleVisible;
            memberLevel.WorkflowVisible = strWorkflowVisible;
            memberLevel.CustomerServiceVisible = strCustomerServiceVisible;
            memberLevel.ConstractVisible = strConstractVisible;
            memberLevel.PositionVisible = strPositionVisible;

            try
            {
                memberLevelBLL.UpdateMemberLevel(memberLevel, int.Parse(strID));

                InitialMemberTree(strDepartCode, strDepartString);

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSBJCDMZFHMXWK") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXGSBDMHFDMBNWK") + "')", true);
        }
    }

    protected void BT_Delete_Click(object sender, EventArgs e)
    {
        string strHQL;

        int intChildCount;

        string strID, strUserCode, strUnderCode;

        string strDepartCode = LB_DepartCode.Text.Trim();
        string strDepartString = LB_DepartString.Text.Trim();

        strUserCode = TB_UserCode.Text.Trim();
        strUnderCode = TB_UnderCode.Text.Trim();

        strID = LB_ID.Text.Trim();
        intChildCount = int.Parse(LB_ChildCount.Text.Trim());

        if (strUserCode == strUnderCode)
        {
            strHQL = "Select UserCode From T_MemberLevel Where UserCode = " + "'" + strUserCode + "'" + " and UnderCode = " + "'" + strUserCode + "'";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MemberLevel");
            if (ds.Tables[0].Rows.Count == 1)
            {
                strHQL = "Select UserCode From T_MemberLevel Where UserCode = " + "'" + strUserCode + "'" + " and UnderCode <> " + "'" + strUserCode + "'";
                ds = ShareClass.GetDataSetFromSql(strHQL, "T_MemberLevel");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGHYTBDZJCYBNSCCYSZJDJDJC") + "')", true);
                    return;
                }
            }
        }

        if (intChildCount == 0)
        {
            strHQL = "Delete From T_MemberLevel Where ID = " + strID;

            try
            {
                ShareClass.RunSqlCommand(strHQL);

                BT_Update.Enabled = false;
                BT_Delete.Enabled = false;

                InitialMemberTree(strDepartCode, strDepartString);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCCJC") + "')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCYGCZXJYGBNSC") + "')", true);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strDepartCode = ((Button)e.Item.FindControl("BT_DepartCode")).Text.Trim();
            string strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";

            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();

            IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

            DataGrid3.DataSource = lst;
            DataGrid3.DataBind();
        }
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strOldUserCode, strOldUnderCode;
        string strUnderCode, strUnderName;

        strOldUserCode = TB_UserCode.Text.Trim();
        strOldUnderCode = TB_UnderCode.Text.Trim();

        strUnderCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        strUnderName = ShareClass.GetUserName(strUnderCode);

        TB_UnderCode.Text = strUnderCode;
        TB_UnderName.Text = strUnderName;

        if ((strOldUserCode != strOldUnderCode) || (strOldUnderCode == ""))
        {
            BT_Update.Enabled = true;
        }
        else
        {
            BT_Update.Enabled = false;
        }

        BT_Delete.Enabled = false;
    }

    protected void InitialMemberTree(string strDepartCode, string strDepartString)
    {
        string strHQL;
        IList lst;

        string strUserCode, strUserName, strUnderCode, strUnderName, strID, strUnderID;
        string strCmdText2;
        int intRow1, intRow2, i, j;
        DataSet ds2;


        //添加根节点
        TreeView1.Nodes.Clear();

        TreeNode node1 = new TreeNode();
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();


        if (strDepartCode == "")
        {
            node1.Text = LanguageHandle.GetWord("BSuoYouBuMenZhiJieChengYuanZuZ");
        }
        else
        {
            node1.Text = "<B>" + strDepartCode + " " + ShareClass.GetDepartName(strDepartCode) + LanguageHandle.GetWord("ZhiJieChengYuanZuZhiCengCiTuB");
        }

        node1.Target = "0";
        node1.Expanded = true;
        TreeView1.Nodes.Add(node1);

        if (strDepartCode == "")
        {
            strHQL = "from MemberLevel as memberLevel where memberLevel.UserCode = memberLevel.UnderCode";
            strHQL += " and memberLevel.UserCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode in " + strDepartString + ")";
        }
        else
        {
            strHQL = "from MemberLevel as memberLevel where memberLevel.UserCode = memberLevel.UnderCode";
            strHQL += " and memberLevel.UserCode in (Select projectMember.UserCode From ProjectMember as projectMember Where projectMember.DepartCode = " + "'" + strDepartCode + "'" + ")";
        }
        strHQL += " Order By memberLevel.SortNumber ASC";

        MemberLevelBLL memberLevelBLL = new MemberLevelBLL();
        lst = memberLevelBLL.GetAllMemberLevels(strHQL);
        MemberLevel memberLevel = new MemberLevel();

        intRow1 = lst.Count;

        for (i = 0; i < intRow1; i++)
        {
            memberLevel = (MemberLevel)lst[i];
            strID = memberLevel.ID.ToString();
            strUserCode = memberLevel.UserCode.Trim();

            try
            {
                strUserName = ShareClass.GetUserName(strUserCode);
                node3 = new TreeNode();

                node3.Text = strUserCode + " " + strUserName;
                node3.Target = strID;
                node3.Expanded = false;

                node1.ChildNodes.Add(node3);

                strCmdText2 = "select ID,UnderCode from T_MemberLevel where UserCode = " + "'" + strUserCode + "'";
                strCmdText2 += " Order by SortNumber ASC";

                ds2 = ShareClass.GetDataSetFromSql(strCmdText2, "T_MemberLevel");
                intRow2 = ds2.Tables[0].Rows.Count;

                for (j = 0; j < intRow2; j++)
                {
                    strUnderID = ds2.Tables[0].Rows[j][0].ToString();
                    strUnderCode = ds2.Tables[0].Rows[j][1].ToString();
                    try
                    {
                        strUnderName = ShareClass.GetUserName(strUnderCode);
                        node4 = new TreeNode();

                        node4.Text = strUnderCode + " " + strUnderName;
                        node4.Target = strUnderID;

                        node3.ChildNodes.Add(node4);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            TreeView1.DataBind();
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode, strUnderCode, strID;

        TreeNode treeNode = new TreeNode();
        treeNode = TreeView1.SelectedNode;

        try
        {
            strID = treeNode.Target;

            strHQL = "from MemberLevel as memberLevel where memberLevel.ID = " + strID;
            MemberLevelBLL memberLevelBLL = new MemberLevelBLL();
            lst = memberLevelBLL.GetAllMemberLevels(strHQL);
            MemberLevel memberLevel = (MemberLevel)lst[0];

            strUserCode = memberLevel.UserCode.Trim();
            strUnderCode = memberLevel.UnderCode.Trim();

            TB_UserCode.Text = strUserCode;
            TB_UnderCode.Text = strUnderCode;
            TB_UnderName.Text = ShareClass.GetUserName(strUnderCode);
            LB_ID.Text = strID;

            DL_ProjectVisible.SelectedValue = memberLevel.ProjectVisible.Trim();
            DL_PlanVisible.SelectedValue = memberLevel.PlanVisible.Trim();
            DL_KPIVisible.SelectedValue = memberLevel.KPIVisible.Trim();
            DL_WorkloadVisible.SelectedValue = memberLevel.WorkloadVisible.Trim();
            DL_ScheduleVisible.SelectedValue = memberLevel.ScheduleVisible.Trim();
            DL_WorkflowVisible.SelectedValue = memberLevel.WorkflowVisible.Trim();
            DL_CustomerServiceVisible.SelectedValue = memberLevel.CustomerServiceVisible.Trim();
            DL_ConstractVisible.SelectedValue = memberLevel.ConstractVisible.Trim();
            DL_PositionVisible.SelectedValue = memberLevel.PositionVisible.Trim();

            LB_ChildCount.Text = treeNode.ChildNodes.Count.ToString();

            BT_Update.Enabled = true;
            BT_Delete.Enabled = true;
        }
        catch
        {
            LB_ID.Text = "0";
            TB_UnderCode.Text = "";
            TB_UnderName.Text = "";
            TB_UserCode.Text = "0";
        }
    }

    protected int GetUnderUserCount(string strUserCode, string strUnderCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + " and memberLevel.UnderCode = " + "'" + strUnderCode + "'";
        MemberLevelBLL memberLevelBLL = new MemberLevelBLL();
        lst = memberLevelBLL.GetAllMemberLevels(strHQL);

        return lst.Count;
    }

    protected int GetChildNodeCount(int intID, string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from MemberLevel as memberLevel where memberLevel.ID = " + intID.ToString() + " and memberLevel.UserCode = " + "'" + strUserCode + "'";
        MemberLevelBLL memberLevelBLL = new MemberLevelBLL();
        lst = memberLevelBLL.GetAllMemberLevels(strHQL);

        return lst.Count;
    }

    protected int GetLevelID(string strUserCode, string strUnderCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from MemberLevel as memberLevel where memberLevel.UserCode = " + "'" + strUserCode + "'" + " and memberLevel.UnderCode = " + "'" + strUnderCode + "'";
        MemberLevelBLL memberLevelBLL = new MemberLevelBLL();
        lst = memberLevelBLL.GetAllMemberLevels(strHQL);

        MemberLevel memberLevel = (MemberLevel)lst[0];

        return memberLevel.ID;
    }
}
