using System;
using System.Resources;
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
using System.Drawing;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTUserAttendanceRule : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName;

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);


        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "员工考勤规则设置", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        string strSystemVersionType = Session["SystemVersionType"].ToString();
        string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
        if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
        {
            Response.Redirect("TTUserAttendanceRuleSAAS.aspx");
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            DLC_CreateDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            strHQL = "Insert Into T_UserAttendanceRule(UserCode,UserName,CreateDate,MCheckInStart,MCheckInEnd,MCheckOutStart,MCheckOutEnd,";
            strHQL += "ACheckInStart,ACheckInEnd,ACheckOutStart,ACheckOutEnd,NCheckInStart,NCheckInEnd,NCheckOutStart,NCheckOutEnd,";
            strHQL += "OCheckInStart,OCheckInEnd,OCheckOutStart,OCheckOutEnd,Status,MCheckInIsMust,MCheckOutIsMust,ACheckInIsMust,ACheckOutIsMust,NCheckInIsMust,NCheckOutIsMust,OCheckInIsMust,OCheckOutIsMust,LargestDistance,LeaderCode,LeaderName,OfficeLongitude,OfficeLatitude,Address)";
            strHQL += " Select A.UserCode,A.UserName,now(),B.MCheckInStart,B.MCheckInEnd,B.MCheckOutStart,B.MCheckOutEnd,";
            strHQL += "B.ACheckInStart,B.ACheckInEnd,B.ACheckOutStart,B.ACheckOutEnd,B.NCheckInStart,B.NCheckInEnd,B.NCheckOutStart,B.NCheckOutEnd,";
            strHQL += "B.OCheckInStart,B.OCheckInEnd,B.OCheckOutStart,B.OCheckOutEnd,'InProgress',B.MCheckInIsMust,B.MCheckOutIsMust,B.ACheckInIsMust,B.ACheckOutIsMust,B.NCheckInIsMust,B.NCheckOutIsMust,B.OCheckInIsMust,B.OCheckOutIsMust,B.LargestDistance,'','',B.OfficeLongitude,B.OfficeLatitude,B.Address";
            strHQL += " From T_ProjectMember A, T_AttendanceRule B";
            strHQL += " Where A.UserCode not in (Select UserCode From T_UserAttendanceRule) and A.Status not in ('Resign','Stop') ";
            ShareClass.RunSqlCommand(strHQL);

            try
            {
                TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"), TreeView1, strUserCode);
            }
            catch(Exception err)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickDFSFF", "alert('" + err.Message.ToString() + "')", true);
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
            strDepartCode = treeNode.Target;

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid2);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();
        string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

        LoadUserAttendanceRule(strUserCode);

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        BT_AddUserAttendanceRule.Enabled = true;
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strHQL;
        IList lst;

        string strID = ((Button)e.Item.FindControl("BT_ID")).Text.Trim();

        for (int i = 0; i < DataGrid3.Items.Count; i++)
        {
            DataGrid3.Items[i].ForeColor = Color.Black;
        }

        e.Item.ForeColor = Color.Red;

        strHQL = "From UserAttendanceRule as userAttendanceRule Where userAttendanceRule.ID = " + strID;
        UserAttendanceRuleBLL userAttendanceRuleBLL = new UserAttendanceRuleBLL();
        lst = userAttendanceRuleBLL.GetAllUserAttendanceRules(strHQL);

        UserAttendanceRule userAttendanceRule = (UserAttendanceRule)lst[0];

        LB_ID.Text = userAttendanceRule.ID.ToString();
        LB_UserCode.Text = userAttendanceRule.UserCode.Trim();
        LB_UserName.Text = userAttendanceRule.UserName.Trim();
        DLC_CreateDate.Text = userAttendanceRule.CreateDate.ToString();

        TB_MCheckInStart.Text = userAttendanceRule.MCheckInStart.Trim();
        TB_MCheckInEnd.Text = userAttendanceRule.MCheckInEnd.Trim();
        DDL_MCheckInIsMust.SelectedValue = userAttendanceRule.MCheckInIsMust.Trim();

        TB_MCheckOutStart.Text = userAttendanceRule.MCheckOutStart.Trim();
        TB_MCheckOutEnd.Text = userAttendanceRule.MCheckOutEnd.Trim();
        DDL_MCheckOutIsMust.SelectedValue = userAttendanceRule.MCheckOutIsMust.Trim();

        TB_ACheckInStart.Text = userAttendanceRule.ACheckInStart.Trim();
        TB_ACheckInEnd.Text = userAttendanceRule.ACheckInEnd.Trim();
        DDL_ACheckInIsMust.SelectedValue = userAttendanceRule.ACheckInIsMust.Trim();

        TB_ACheckOutStart.Text = userAttendanceRule.ACheckOutStart.Trim();
        TB_AChectOutEnd.Text = userAttendanceRule.ACheckOutEnd.Trim();
        DDL_ACheckOutIsMust.SelectedValue = userAttendanceRule.ACheckOutIsMust.Trim();

        TB_NCheckInStart.Text = userAttendanceRule.NCheckInStart.Trim();
        TB_NCheckInEnd.Text = userAttendanceRule.NCheckInEnd.Trim();
        DDL_NCheckInIsMust.SelectedValue = userAttendanceRule.NCheckInIsMust.Trim();

        TB_NCheckOutStart.Text = userAttendanceRule.NCheckOutStart.Trim();
        TB_NCheckOutEnd.Text = userAttendanceRule.NCheckOutEnd.Trim();
        DDL_NCheckOutIsMust.SelectedValue = userAttendanceRule.NCheckOutIsMust.Trim();

        TB_OCheckInStart.Text = userAttendanceRule.OCheckInStart.Trim();
        TB_OCheckInEnd.Text = userAttendanceRule.OCheckInEnd.Trim();
        DDL_OCheckInIsMust.SelectedValue = userAttendanceRule.OCheckInIsMust.Trim();

        TB_OCheckOutStart.Text = userAttendanceRule.OCheckOutStart.Trim();
        TB_OCheckOutEnd.Text = userAttendanceRule.OCheckOutEnd.Trim();
        DDL_OCheckOutIsMust.SelectedValue = userAttendanceRule.OCheckOutIsMust.Trim();

        TB_Longitude.Text = userAttendanceRule.OfficeLongitude.Trim();
        TB_Latitude.Text = userAttendanceRule.OfficeLatitude.Trim();

        TB_Address.Text = userAttendanceRule.Address;

        NB_LargestDistance.Amount = userAttendanceRule.LargestDistance;

        DL_Status.SelectedValue = userAttendanceRule.Status.Trim();



        BT_UpdateUserAttendanceRule.Enabled = true;
        BT_DeleteUserAttendanceRule.Enabled = true;
        BT_PaiBanRule.Enabled = true;
    }

    protected void BT_AddUserAttendanceRule_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID;
        string strUserCode, strUserName;
        string strMCheckInStart, strMCheckInEnd, strMCheckOutStart, strMCheckOutEnd;
        string strACheckInStart, strACheckInEnd, strACheckOutStart, strACheckOutEnd;
        string strNCheckInStart, strNCheckInEnd, strNCheckOutStart, strNCheckOutEnd;
        string strOCheckInStart, strOCheckInEnd, strOCheckOutStart, strOCheckOutEnd;
        DateTime dtCreateDate;
        string strStatus;

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        dtCreateDate = DateTime.Parse(DLC_CreateDate.Text);

        strMCheckInStart = TB_MCheckInStart.Text.Trim();
        strMCheckInEnd = TB_MCheckInEnd.Text.Trim();
        strMCheckOutStart = TB_MCheckOutStart.Text.Trim();
        strMCheckOutEnd = TB_MCheckOutEnd.Text.Trim();

        strACheckInStart = TB_ACheckInStart.Text.Trim();
        strACheckInEnd = TB_ACheckInEnd.Text.Trim();
        strACheckOutStart = TB_ACheckOutStart.Text.Trim();
        strACheckOutEnd = TB_AChectOutEnd.Text.Trim();

        strNCheckInStart = TB_NCheckInStart.Text.Trim();
        strNCheckInEnd = TB_NCheckInEnd.Text.Trim();
        strNCheckOutStart = TB_NCheckOutStart.Text.Trim();
        strNCheckOutEnd = TB_NCheckOutEnd.Text.Trim();

        strOCheckInStart = TB_OCheckInStart.Text.Trim();
        strOCheckInEnd = TB_OCheckInEnd.Text.Trim();
        strOCheckOutStart = TB_OCheckOutStart.Text.Trim();
        strOCheckOutEnd = TB_OCheckOutEnd.Text.Trim();

        strStatus = DL_Status.SelectedValue.Trim();

        if (strMCheckInStart == "" || strMCheckInEnd == "" || strMCheckOutStart == "" || strMCheckOutEnd == ""
          || strACheckInStart == "" || strACheckInEnd == "" || strACheckOutStart == "" || strACheckOutEnd == ""
          || strNCheckInStart == "" || strNCheckInEnd == "" || strNCheckOutStart == "" || strNCheckOutEnd == ""
          || strOCheckInStart == "" || strOCheckInEnd == "" || strOCheckOutStart == "" || strOCheckOutEnd == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSGXDBNWKQJC") + "')", true);
            return;
        }


        UserAttendanceRuleBLL userAttendanceRuleBLL = new UserAttendanceRuleBLL();
        UserAttendanceRule userAttendanceRule = new UserAttendanceRule();

        userAttendanceRule.UserCode = strUserCode;
        userAttendanceRule.UserName = strUserName;

        userAttendanceRule.CreateDate = dtCreateDate;

        userAttendanceRule.MCheckInStart = strMCheckInStart;
        userAttendanceRule.MCheckInEnd = strMCheckInEnd;
        userAttendanceRule.MCheckInIsMust = DDL_MCheckInIsMust.SelectedValue.Trim();

        userAttendanceRule.MCheckOutStart = strMCheckOutStart;
        userAttendanceRule.MCheckOutEnd = strMCheckOutEnd;
        userAttendanceRule.MCheckOutIsMust = DDL_MCheckOutIsMust.SelectedValue.Trim();

        userAttendanceRule.ACheckInStart = strACheckInStart;
        userAttendanceRule.ACheckInEnd = strACheckInEnd;
        userAttendanceRule.ACheckInIsMust = DDL_ACheckInIsMust.SelectedValue.Trim();

        userAttendanceRule.ACheckOutStart = strACheckOutStart;
        userAttendanceRule.ACheckOutEnd = strACheckOutEnd;
        userAttendanceRule.ACheckOutIsMust = DDL_ACheckOutIsMust.SelectedValue.Trim();

        userAttendanceRule.NCheckInStart = strNCheckInStart;
        userAttendanceRule.NCheckInEnd = strNCheckInEnd;
        userAttendanceRule.NCheckInIsMust = DDL_NCheckInIsMust.SelectedValue.Trim();

        userAttendanceRule.NCheckOutStart = strNCheckOutStart;
        userAttendanceRule.NCheckOutEnd = strNCheckOutEnd;
        userAttendanceRule.NCheckOutIsMust = DDL_NCheckOutIsMust.SelectedValue.Trim();

        userAttendanceRule.OCheckInStart = strOCheckInStart;
        userAttendanceRule.OCheckInEnd = strOCheckInEnd;
        userAttendanceRule.OCheckInIsMust = DDL_OCheckInIsMust.SelectedValue.Trim();

        userAttendanceRule.OCheckOutStart = strOCheckOutStart;
        userAttendanceRule.OCheckOutEnd = strOCheckOutEnd;
        userAttendanceRule.OCheckOutIsMust = DDL_OCheckOutIsMust.SelectedValue.Trim();

        userAttendanceRule.OfficeLatitude = TB_Latitude.Text.Trim();
        userAttendanceRule.OfficeLongitude = TB_Longitude.Text.Trim();
        userAttendanceRule.Address = TB_Address.Text.Trim();

        userAttendanceRule.LargestDistance = NB_LargestDistance.Amount;

        userAttendanceRule.LeaderCode = "";
        userAttendanceRule.LeaderName = "";

        userAttendanceRule.Status = strStatus;

        try
        {
            userAttendanceRuleBLL.AddUserAttendanceRule(userAttendanceRule);

            strID = ShareClass.GetMyCreatedMaxUserAttendanceRule(strUserCode);
            LB_ID.Text = strID;

            if (strStatus == "InProgress")
            {
                strHQL = "Update T_UserAttendanceRule Set Status = 'Backup' Where ID <> " + strID + " and UserCode = " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);
            }

            LoadUserAttendanceRule(strUserCode);

            BT_UpdateUserAttendanceRule.Enabled = true;
            BT_DeleteUserAttendanceRule.Enabled = true;
            

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXJSBJC") + "')", true);
        }
    }

    protected void BT_UpdateUserAttendanceRule_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strID;
        string strUserCode, strUserName;
        string strMCheckInStart, strMCheckInEnd, strMCheckOutStart, strMCheckOutEnd;
        string strACheckInStart, strACheckInEnd, strACheckOutStart, strACheckOutEnd;
        string strNCheckInStart, strNCheckInEnd, strNCheckOutStart, strNCheckOutEnd;
        string strOCheckInStart, strOCheckInEnd, strOCheckOutStart, strOCheckOutEnd;
        DateTime dtCreateDate;
        string strStatus;

        strID = LB_ID.Text.Trim();

        strUserCode = LB_UserCode.Text.Trim();
        strUserName = LB_UserName.Text.Trim();

        dtCreateDate = DateTime.Parse(DLC_CreateDate.Text);

        strMCheckInStart = TB_MCheckInStart.Text.Trim();
        strMCheckInEnd = TB_MCheckInEnd.Text.Trim();
        strMCheckOutStart = TB_MCheckOutStart.Text.Trim();
        strMCheckOutEnd = TB_MCheckOutEnd.Text.Trim();

        strACheckInStart = TB_ACheckInStart.Text.Trim();
        strACheckInEnd = TB_ACheckInEnd.Text.Trim();
        strACheckOutStart = TB_ACheckOutStart.Text.Trim();
        strACheckOutEnd = TB_AChectOutEnd.Text.Trim();

        strNCheckInStart = TB_NCheckInStart.Text.Trim();
        strNCheckInEnd = TB_NCheckInEnd.Text.Trim();
        strNCheckOutStart = TB_NCheckOutStart.Text.Trim();
        strNCheckOutEnd = TB_NCheckOutEnd.Text.Trim();

        strOCheckInStart = TB_OCheckInStart.Text.Trim();
        strOCheckInEnd = TB_OCheckInEnd.Text.Trim();
        strOCheckOutStart = TB_OCheckOutStart.Text.Trim();
        strOCheckOutEnd = TB_OCheckOutEnd.Text.Trim();

        strStatus = DL_Status.SelectedValue.Trim();

        if (strMCheckInStart == "" || strMCheckInEnd == "" || strMCheckOutStart == "" || strMCheckOutEnd == ""
          || strACheckInStart == "" || strACheckInEnd == "" || strACheckOutStart == "" || strACheckOutEnd == ""
          || strNCheckInStart == "" || strNCheckInEnd == "" || strNCheckOutStart == "" || strNCheckOutEnd == ""
          || strOCheckInStart == "" || strOCheckInEnd == "" || strOCheckOutStart == "" || strOCheckOutEnd == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYSGXDBNWKQJC") + "')", true);
            return;
        }

        UserAttendanceRuleBLL userAttendanceRuleBLL = new UserAttendanceRuleBLL();
        UserAttendanceRule userAttendanceRule = new UserAttendanceRule();

        userAttendanceRule.ID = int.Parse(strID);
        userAttendanceRule.UserCode = strUserCode;
        userAttendanceRule.UserName = strUserName;
        userAttendanceRule.CreateDate = dtCreateDate;

        userAttendanceRule.MCheckInStart = strMCheckInStart;
        userAttendanceRule.MCheckInEnd = strMCheckInEnd;
        userAttendanceRule.MCheckInIsMust = DDL_MCheckInIsMust.SelectedValue.Trim();

        userAttendanceRule.MCheckOutStart = strMCheckOutStart;
        userAttendanceRule.MCheckOutEnd = strMCheckOutEnd;
        userAttendanceRule.MCheckOutIsMust = DDL_MCheckOutIsMust.SelectedValue.Trim();

        userAttendanceRule.ACheckInStart = strACheckInStart;
        userAttendanceRule.ACheckInEnd = strACheckInEnd;
        userAttendanceRule.ACheckInIsMust = DDL_ACheckInIsMust.SelectedValue.Trim();

        userAttendanceRule.ACheckOutStart = strACheckOutStart;
        userAttendanceRule.ACheckOutEnd = strACheckOutEnd;
        userAttendanceRule.ACheckOutIsMust = DDL_ACheckOutIsMust.SelectedValue.Trim();

        userAttendanceRule.NCheckInStart = strNCheckInStart;
        userAttendanceRule.NCheckInEnd = strNCheckInEnd;
        userAttendanceRule.NCheckInIsMust = DDL_NCheckInIsMust.SelectedValue.Trim();

        userAttendanceRule.NCheckOutStart = strNCheckOutStart;
        userAttendanceRule.NCheckOutEnd = strNCheckOutEnd;
        userAttendanceRule.NCheckOutIsMust = DDL_NCheckOutIsMust.SelectedValue.Trim();

        userAttendanceRule.OCheckInStart = strOCheckInStart;
        userAttendanceRule.OCheckInEnd = strOCheckInEnd;
        userAttendanceRule.OCheckInIsMust = DDL_OCheckInIsMust.SelectedValue.Trim();

        userAttendanceRule.OCheckOutStart = strOCheckOutStart;
        userAttendanceRule.OCheckOutEnd = strOCheckOutEnd;
        userAttendanceRule.OCheckOutIsMust = DDL_OCheckOutIsMust.SelectedValue.Trim();

        userAttendanceRule.OfficeLatitude = TB_Latitude.Text.Trim();
        userAttendanceRule.OfficeLongitude = TB_Longitude.Text.Trim();
        userAttendanceRule.Address = TB_Address.Text.Trim();

        userAttendanceRule.LargestDistance = NB_LargestDistance.Amount;

        userAttendanceRule.Status = strStatus;

        try
        {
            userAttendanceRuleBLL.UpdateUserAttendanceRule(userAttendanceRule, int.Parse(strID));

            if (strStatus == "InProgress")
            {
                strHQL = "Update T_UserAttendanceRule Set Status = 'Backup' Where ID <> " + strID + " and UserCode = " + "'" + strUserCode + "'";
                ShareClass.RunSqlCommand(strHQL);
            }

            LoadUserAttendanceRule(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWXGSBJC") + "')", true);
        }
    }

    protected void BT_DeleteUserAttendanceRule_Click(object sender, EventArgs e)
    {
        string strID;
        string strHQL;

        string strUserCode = LB_UserCode.Text.Trim();

        strID = LB_ID.Text.Trim();
        strHQL = "Delete From T_UserAttendanceRule Where ID = " + strID;

        try
        {
            ShareClass.RunSqlCommand(strHQL);

            LoadUserAttendanceRule(strUserCode);

            BT_UpdateUserAttendanceRule.Enabled = false;
            BT_DeleteUserAttendanceRule.Enabled = false;

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCWSCSBJC") + "')", true);
        }
    }

    protected void LoadUserAttendanceRule(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From UserAttendanceRule as userAttendanceRule Where userAttendanceRule.UserCode = " + "'" + strUserCode + "'";
        //strHQL += " and Len(rtrim(userAttendanceRule.LeaderCode)) = 0";
        strHQL += " Order by userAttendanceRule.ID DESC";
        UserAttendanceRuleBLL userAttendanceRuleBLL = new UserAttendanceRuleBLL();
        lst = userAttendanceRuleBLL.GetAllUserAttendanceRules(strHQL);

        DataGrid3.DataSource = lst;
        DataGrid3.DataBind();
    }

    protected void BT_PaiBanRule_Click(object sender, EventArgs e)
    {
        Response.Redirect("TTUserScheduleRule.aspx");
    }

    protected int GetUserNumber()
    {
        string strHQL;
        IList lst;

        strHQL = "from ProjectMember as projectMember";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        return lst.Count;
    }
}
