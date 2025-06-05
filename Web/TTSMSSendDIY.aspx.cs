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


public partial class TTSMSSendDIY : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL, strUserCode;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);

            string strDepartCode = GetDepartCode(strUserCode);
            strHQL = "from ProjectMember as projectMember where projectMember.DepartCode = " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            LoadSMSSendDIYList(strUserCode);
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

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
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
            AddSMS(true);
        }
        else
        {
            UpdateSMS(true);
        }
    }

    protected void AddSMS(bool isPop)
    {
        string strID;
        string strMessage;
        string strUserCode;

        strMessage = TB_Message.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        SMSSendDIYBLL smsSendDIYBLL = new SMSSendDIYBLL();
        SMSSendDIY smsSendDIY = new SMSSendDIY();

        smsSendDIY.Message = strMessage;
        smsSendDIY.UserCode = strUserCode;
        smsSendDIY.UserName = ShareClass.GetUserName(strUserCode);
        smsSendDIY.SendTime = DateTime.Now;
        smsSendDIY.Status = "New";

        try
        {
            smsSendDIYBLL.AddSMSSendDIY(smsSendDIY);

            strID = ShareClass.GetMyCreatedMaxSMSID(strUserCode);
            LB_ID.Text = strID;

            #region 增加参与人员 By LiuJianping 2013-09-11

            if (RP_Attendant.Items.Count > 0)
            {
                for (int i = 0; i < RP_Attendant.Items.Count; i++)
                {
                    if (((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim() == "")
                    {
                    }
                    else
                    {
                        AddSMSRelatedUser(strID, ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim(), ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim());
                    }
                }
            }
            #endregion

            BT_Send.Enabled = true;

            LoadSMSSendDIYList(strUserCode);
            LoadSMSRelatedUserList(strID);

            if (isPop)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }

        }
        catch
        {
            if (isPop)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateSMS(bool isPop)
    {
        string strID, strMessage;
        string strUserCode;
        string strHQL;
        IList lst;

        strID = LB_ID.Text.Trim();
        strMessage = TB_Message.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();


        strHQL = "From SMSSendDIY as smsSendDIY Where smsSendDIY.ID = " + strID;
        SMSSendDIYBLL smsSendDIYBLL = new SMSSendDIYBLL();
        lst = smsSendDIYBLL.GetAllSMSSendDIYs(strHQL);

        SMSSendDIY smsSendDIY = (SMSSendDIY)lst[0];
        smsSendDIY.Message = strMessage;


        try
        {
            smsSendDIYBLL.UpdateSMSSendDIY(smsSendDIY, int.Parse(strID));

            #region 增加参与人员 By LiuJianping 2013-09-11
            string strMemID = "0";
            if (RP_Attendant.Items.Count > 0)
            {
                for (int i = 0; i < RP_Attendant.Items.Count; i++)
                {
                    if (((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim() == "")
                    {
                    }
                    else
                    {
                        string memid = UpdateSMSRelatedUser(strID, ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim(), ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim());
                        strMemID += "," + memid;
                    }
                }
                DeleteSMSRelatedUser(strID, strMemID);
            }
            #endregion

            LoadSMSSendDIYList(strUserCode);

            if (isPop)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCCG") + "')", true);
            }
        }
        catch
        {
            if (isPop)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZBCSB") + "')", true);
            }

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        #region 新增 By LiuJianping 2013-09-11

        string struserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string struserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

        DataSet ds = GetSMSRelatedUserModule(RP_Attendant);
        DataTable dt = ds.Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["UserCode"].ToString().Trim() == struserCode.Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYCZBNZFJRJC") + "')", true);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

                return;
            }
        }

        DataRow dr = dt.NewRow();
        dr["UserName"] = struserName.Trim();
        dr["UserCode"] = struserCode.Trim();
        dt.Rows.Add(dr);

        RP_Attendant.DataSource = ds;
        RP_Attendant.DataBind();

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

        #endregion
    }

    /// <summary>
    /// 增加参与人员 By LiuJianping  2013-09-11
    /// </summary>
    /// <param name="strCoID"></param>
    /// <param name="struserCode"></param>
    /// <param name="struserName"></param>
    protected void AddSMSRelatedUser(string strCoID, string struserCode, string struserName)
    {
        SMSRelatedUserBLL sMSRelatedUserBLL = new SMSRelatedUserBLL();
        SMSRelatedUser sMSRelatedUser = new SMSRelatedUser();
        sMSRelatedUser.SMSID = int.Parse(strCoID);
        sMSRelatedUser.UserCode = struserCode.Trim();
        sMSRelatedUser.UserName = struserName.Trim();

        sMSRelatedUserBLL.AddSMSRelatedUser(sMSRelatedUser);
    }

    /// <summary>
    /// 更新参与人员 By LiuJianping  2013-09-11
    /// </summary>
    /// <param name="strCoID"></param>
    /// <param name="struserCode"></param>
    /// <param name="struserName"></param>
    protected string UpdateSMSRelatedUser(string strCoID, string struserCode, string struserName)
    {
        string MemID = "0";
        SMSRelatedUserBLL sMSRelatedUserBLL = new SMSRelatedUserBLL();
        string strHQL = "from SMSRelatedUser as sMSRelatedUser where sMSRelatedUser.SMSID = '" + strCoID + "' and sMSRelatedUser.UserName = '" + struserName + "' and sMSRelatedUser.UserCode='" + struserCode + "' ";
        IList lst = sMSRelatedUserBLL.GetAllSMSRelatedUsers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            SMSRelatedUser sMSRelatedUser = (SMSRelatedUser)lst[0];
            MemID = sMSRelatedUser.ID.ToString();
        }
        else
        {
            AddSMSRelatedUser(strCoID, struserCode, struserName);
            MemID = GetSMSRelatedUserID(strCoID, struserCode, struserName);
        }
        return MemID;
    }

    /// <summary>
    /// 删除参与人员 By LiuJianping  2013-09-11
    /// </summary>
    /// <param name="strCoID"></param>
    /// <param name="strMemIDList">如1,2,3,4</param>
    protected void DeleteSMSRelatedUser(string strCoID, string strMemIDList)
    {
        SMSRelatedUserBLL sMSRelatedUserBLL = new SMSRelatedUserBLL();
        string strHQL = "from SMSRelatedUser as sMSRelatedUser where sMSRelatedUser.SMSID = '" + strCoID + "' and sMSRelatedUser.ID not in (" + strMemIDList + ") ";
        IList lst = sMSRelatedUserBLL.GetAllSMSRelatedUsers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                SMSRelatedUser sMSRelatedUser = (SMSRelatedUser)lst[i];
                sMSRelatedUserBLL.DeleteSMSRelatedUser(sMSRelatedUser);
            }
        }
    }

    /// <summary>
    /// 获取创建参与人员的即时ID By LiuJianping  2013-09-11
    /// </summary>
    /// <param name="strUserCode"></param>
    /// <returns></returns>
    protected string GetSMSRelatedUserID(string strCoID, string struserCode, string struserName)
    {
        string strHQL = "Select Max(ID) From T_SMSRelatedUser Where SMSID = '" + strCoID + "' and UserName = '" + struserName + "' and UserCode='" + struserCode + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_SMSRelatedUser");

        return ds.Tables[0].Rows[0][0].ToString().Trim();
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strDepartCode = ((Button)e.Item.FindControl("BT_DepartCode")).Text.Trim();
            string strDepartName = ((Button)e.Item.FindControl("BT_DepartName")).Text.Trim();

            string strHQL = "from ProjectMember as projectMember where projectMember.DepartCode= " + "'" + strDepartCode + "'";
            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid4.Items.Count; i++)
                {
                    DataGrid4.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;

                strHQL = "From SMSSendDIY as smsSendDIY Where smsSendDIY.ID = " + strID;
                SMSSendDIYBLL smsSendDIYBLL = new SMSSendDIYBLL();
                lst = smsSendDIYBLL.GetAllSMSSendDIYs(strHQL);

                SMSSendDIY smsSendDIY = (SMSSendDIY)lst[0];

                LB_ID.Text = smsSendDIY.ID.ToString();
                TB_Message.Text = smsSendDIY.Message.Trim();
                LB_Status.Text = smsSendDIY.Status;

                BT_Send.Enabled = true;

                LoadSMSRelatedUserList(strID);

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strUserCode;

                strUserCode = LB_UserCode.Text.Trim();

                strHQL = "Delete From T_SMSSendDIY Where ID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    strHQL = "Delete From T_SMSRelatedUser Where SMSID = " + strID;
                    ShareClass.RunSqlCommand(strHQL);

                    BT_Send.Enabled = false;

                    LoadSMSSendDIYList(strUserCode);

                    LoadSMSRelatedUserList(strID);

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);
                }
            }
        }
    }


    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        IList lst;

        SMSSendDIYBLL smsSendDIYBLL = new SMSSendDIYBLL();
        lst = smsSendDIYBLL.GetAllSMSSendDIYs(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text.Trim();

            #region 新增  By LiuJianping  2013-09-11
            DataTable dt = GetSMSRelatedUserModule(RP_Attendant).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["UserName"].ToString().Trim() == strUserName.Trim())
                {
                    dt.Rows.Remove(dt.Rows[i]);
                }
            }
            RP_Attendant.DataSource = dt;
            RP_Attendant.DataBind();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            #endregion
        }
    }

    /// <summary>
    /// 获取当前参与人员列表 By LiuJianping  2013-09-11
    /// </summary>
    /// <param name="RP"></param>
    /// <returns></returns>
    protected DataSet GetSMSRelatedUserModule(Repeater RP)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("SMSRelatedUserModule");
        DataColumn dc = new DataColumn();
        dt.Columns.Add("UserName", typeof(string));
        dt.Columns.Add("UserCode", typeof(string));
        if (RP.Items.Count > 0)
        {
            for (int i = 0; i < RP.Items.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["UserName"] = ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim();
                dr["UserCode"] = ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim();
                dt.Rows.Add(dr);
            }
        }
        ds.Tables.Add(dt);
        return ds;
    }

    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strHQL, strUserCode, strReceiverCode;
        string strMsg, strID;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        strID = LB_ID.Text.Trim();

        if (strID == "")
        {
            AddSMS(false);
        }
        else
        {
            UpdateSMS(false);
        }

        strID = LB_ID.Text.Trim();

        strHQL = "From SMSRelatedUser as smsRelatedUser Where smsRelatedUser.SMSID = " + strID;
        SMSRelatedUserBLL smsRelatedUserBLL = new SMSRelatedUserBLL();
        lst = smsRelatedUserBLL.GetAllSMSRelatedUsers(strHQL);

        SMSRelatedUser smsRelatedUser = new SMSRelatedUser();

        Msg msg = new Msg();

        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                smsRelatedUser = (SMSRelatedUser)lst[i];

                strReceiverCode = smsRelatedUser.UserCode.Trim();
                strMsg = TB_Message.Text.Trim();

                //发送信息
                msg.SendMSM("Message",strReceiverCode, strMsg, strUserCode);
            }
        }

        LB_Status.Text = "Sent";

        UpdateSMSSendDIYStatus(strID);

        LoadSMSSendDIYList(strUserCode);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXFSWB") + "')", true);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

    }

    protected void UpdateSMSSendDIYStatus(string strID)
    {
        string strUserCode;
        string strHQL;
        IList lst;

        strID = LB_ID.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();


        strHQL = "From SMSSendDIY as smsSendDIY Where smsSendDIY.ID = " + strID;
        SMSSendDIYBLL smsSendDIYBLL = new SMSSendDIYBLL();
        lst = smsSendDIYBLL.GetAllSMSSendDIYs(strHQL);

        SMSSendDIY smsSendDIY = (SMSSendDIY)lst[0];
        smsSendDIY.Status = "Sent";
        smsSendDIY.SendTime = DateTime.Now;

        try
        {
            smsSendDIYBLL.UpdateSMSSendDIY(smsSendDIY, int.Parse(strID));

            LoadSMSSendDIYList(strUserCode);

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);

        }
        catch
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSBJC")+"')", true);

        }
    }

    protected void LoadSMSSendDIYList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From SMSSendDIY as smsSendDIY Where smsSendDIY.UserCode = " + "'" + strUserCode + "'" + " Order By smsSendDIY.ID DESC";
        SMSSendDIYBLL smsSendDIYBLL = new SMSSendDIYBLL();
        lst = smsSendDIYBLL.GetAllSMSSendDIYs(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadSMSRelatedUserList(string strID)
    {
        string strHQL;
        IList lst;


        strHQL = "From SMSRelatedUser as smsRelatedUser Where smsRelatedUser.SMSID = " + strID;
        SMSRelatedUserBLL smsRelatedUserBLL = new SMSRelatedUserBLL();
        lst = smsRelatedUserBLL.GetAllSMSRelatedUsers(strHQL);

        RP_Attendant.DataSource = lst;
        RP_Attendant.DataBind();
    }

    protected string GetDepartCode(string strUserCode)
    {
        string strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        ProjectMember projectMember = (ProjectMember)lst[0];

        string strDepartCode = projectMember.DepartCode;

        return strDepartCode;
    }

    protected string GetUserName(string strUserCode)
    {
        string strUserName, strHQL;

        strHQL = " from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        IList lst = projectMemberBLL.GetAllProjectMembers(strHQL);
        ProjectMember projectMember = (ProjectMember)lst[0];
        strUserName = projectMember.UserName;
        return strUserName.Trim();
    }
}
