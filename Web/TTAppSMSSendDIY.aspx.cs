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

public partial class TTAppSMSSendDIY : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL, strUserCode;
        IList lst;

        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;

        //this.Title = "发送短信";

        if (Page.IsPostBack != true)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);

            string strDepartCode = GetDepartCode(strUserCode);

            LoadSMSSendDIYList(strUserCode);

            string strSystemVersionType = HttpContext.Current.Session["SystemVersionType"].ToString();
            string strProductType = System.Configuration.ConfigurationManager.AppSettings["ProductType"];
            if (strSystemVersionType == "SAAS" || strProductType.IndexOf("SAAS") > -1)
            {
                TD_TreeView.Visible = false;

                ShareClass.LoadMemberByUserCodeForDataGrid(strUserCode, "ALL", DataGrid1);
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

            ShareClass.LoadUserByDepartCodeForDataGrid(strDepartCode, DataGrid1);
        }
    }

    protected void AddSMS()
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

            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZCG")+"')", true);

        }
        catch
        {
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZSBJC")+"')", true);

        }
    }


    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        #region 注销 By LiuJianping 2013-09-11
        //string strHQL;
        //IList lst;

        //string strID = LB_ID.Text.Trim();

        //if (strID != "")
        //{
        //    string strUserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        //    string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

        //    SMSRelatedUserBLL smsRelatedUserBLL = new SMSRelatedUserBLL();
        //    SMSRelatedUser smsRelatedUser = new SMSRelatedUser();

        //    strHQL = "from SMSRelatedUser as  smsRelatedUser where smsRelatedUser.ID = " + strID + " and smsRelatedUser.UserName = " + "'" + strUserName + "'";
        //    lst = smsRelatedUserBLL.GetAllSMSRelatedUsers(strHQL);

        //    if (lst.Count == 0)
        //    {
        //        try
        //        {
        //            smsRelatedUser.SMSID = int.Parse(strID);
        //            smsRelatedUser.UserCode = strUserCode;
        //            smsRelatedUser.UserName = strUserName;

        //            smsRelatedUserBLL.AddSMSRelatedUser(smsRelatedUser);

        //            LoadSMSRelatedUserList(strID);
        //        }
        //        catch
        //        {
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCJSRYYCZBNZFJRJC")+"')", true);
        //    }
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCWKJLBNZJJSRY")+"')", true);
        //}
        #endregion

        #region 新增 By LiuJianping 2013-09-11

        string struserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string struserName = ((Button)e.Item.FindControl("BT_UserName")).Text;
        //string usercodeGold = LB_UserCode.Text.Trim();//操作者

        //if (struserCode.Trim() == usercodeGold)
        //{
        //    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCJZBYJRXTHZDJRJC")+"')", true);
        //    return;
        //}

        DataSet ds = GetSMSRelatedUserModule(RP_Attendant);
        DataTable dt = ds.Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["UserCode"].ToString().Trim() == struserCode.Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZCCYYCZBNZFJRJC") + "')", true);
                return;
            }
        }

        DataRow dr = dt.NewRow();
        dr["UserName"] = struserName.Trim();
        dr["UserCode"] = struserCode.Trim();
        dt.Rows.Add(dr);

        RP_Attendant.DataSource = ds;
        RP_Attendant.DataBind();

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
        }
    }


    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = ((Button)e.Item.FindControl("BT_ID")).Text;

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
            #endregion

            #region 注销  By LiuJianping  2013-09-11
            //string strHQL, strID;
            //IList lst;

            //strID = LB_ID.Text.Trim();

            //strHQL = "From SMSRelatedUser as smsRelatedUser Where smsRelatedUser.SMSID = " + strID + " and smsRelatedUser.UserName = " + "'" + strUserName + "'";
            //SMSRelatedUserBLL smsRelatedUserBLL = new SMSRelatedUserBLL();
            //lst = smsRelatedUserBLL.GetAllSMSRelatedUsers(strHQL);

            //if (lst.Count > 0)
            //{
            //    strHQL = "Delete From T_SMSRelatedUser Where SMSID = " + strID + " and UserName = " + "'" + strUserName + "'";
            //    ShareClass.RunSqlCommand(strHQL);
            //}
            //else
            //{
            //}

            //LoadSMSRelatedUserList(strID);
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

        //存储短信
        AddSMS();

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
