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

public partial class TTMakeColaborationCS : System.Web.UI.Page
{
    string strRelatedType, strRelatedID, strIdentifyString;
    string strIsMobileDevice;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strUserCode, strDepartCode;

        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/"; Session["PageName"] = "TakeTopSiteContentEdit";
        _FileBrowser.SetupCKEditor(CKEditor1);
CKEditor1.Language = Session["LangCode"].ToString();

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
        
        strIdentifyString = Request.QueryString["IdentifyString"];
        if (strIdentifyString == null)
        {
            strIdentifyString = "";
        }    

        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;
        strDepartCode = ShareClass.GetDepartCodeFromUserCode(strUserCode);
       

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack != true)
        {
            if (strIsMobileDevice == "YES")
            {
                HTEditor1.Visible = true;
            }
            else
            {
                CKEditor1.Visible = true;
            }

            strHQL = "from ActorGroup as actorGroup where actorGroup.GroupName ";
            strHQL += " in ( Select actorGroupDetail.GroupName from ActorGroupDetail as actorGroupDetail where actorGroupDetail.UserCode = " + "'" + strUserCode + "'" + ")";
            ActorGroupBLL actorGroupBLL = new ActorGroupBLL();
            lst = actorGroupBLL.GetAllActorGroups(strHQL);
            DataGrid2.DataSource = lst;
            DataGrid2.DataBind();

            LoadCollaborationList(strUserCode);
        }
    }

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strGroupName = ((Button)e.Item.FindControl("BT_ActorGroup")).Text.Trim();
            string strHQL = "from ActorGroupDetail as actorGroupDetail where actorGroupDetail.GroupName = " + "'" + strGroupName + "'";

            ActorGroupDetailBLL actroGroupDetailBLL = new ActorGroupDetailBLL();
            IList lst = actroGroupDetailBLL.GetAllActorGroupDetails(strHQL);

            DataGrid1.DataSource = lst;
            DataGrid1.DataBind();

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void BT_Create_Click(object sender, EventArgs e)
    {
        LB_CoID.Text = "";

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','false') ", true);
    }

    protected void BT_New_Click(object sender, EventArgs e)
    {
        string strID;

        strID = LB_CoID.Text.Trim();

        if (strID == "")
        {
            AddCollaboration();
        }
        else
        {
            UpdateCollaboration();
        }
    }

    protected void AddCollaboration()
    {
        string strCOID;
        string strCollaborationName, strContent;
        string strUserCode;

        strUserCode = LB_UserCode.Text.Trim();
        strCollaborationName = TB_CollaborationName.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HTEditor1.Text.Trim();       
        }
        else
        {
            strContent = CKEditor1.Text.Trim();       
        }
     
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        Collaboration collaboration = new Collaboration();

        collaboration.CollaborationName = strCollaborationName;
        collaboration.Comment = strContent;
        collaboration.CreateTime = DateTime.Now;
        collaboration.CreatorCode = strUserCode;
        collaboration.CreatorName = ShareClass.GetUserName(strUserCode);

        collaboration.IdentifyString = strIdentifyString;

        collaboration.RelatedType = strRelatedType;
        collaboration.RelatedID = int.Parse(strRelatedID);
        collaboration.Status = "New";

        try
        {
            collaborationBLL.AddCollaboration(collaboration);

            strCOID = ShareClass.GetMyCreatedMaxColloaborationID(strUserCode);
            LB_CoID.Text = strCOID;

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
                        AddCollaborationMember(strCOID, ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim(), ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim());
                    }
                }
            }
            #endregion


            BT_Close.Enabled = true;
            BT_Active.Enabled = true;
            BT_Send.Enabled = true;

            HL_RelatedDoc.Enabled = true;
            HL_RelatedDoc.NavigateUrl = "TTCollaborationRelatedDoc.aspx?RelatedID=" + strCOID;
            TB_Message.Text = LanguageHandle.GetWord("NiYiBeiYaoQingCanJiaXieZuo") + strCOID + " " + strCollaborationName + LanguageHandle.GetWord("YaoQingZhe") + ShareClass . GetUserName(strUserCode) + " " + LanguageHandle.GetWord("QingZhunShiCanJia");

            LoadCollaborationMember(strCOID);
            LoadCollaborationList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB")+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }

    protected void UpdateCollaboration()
    {
        string strCoID, strCollaborationName, strContent;
        string strUserCode;
        string strHQL;
        IList lst;

        strCoID = LB_CoID.Text.Trim();
        strCollaborationName = TB_CollaborationName.Text.Trim();

        if (strIsMobileDevice == "YES")
        {
            strContent = HTEditor1.Text.Trim();
        }
        else
        {
            strContent = CKEditor1.Text.Trim();
        }
     
        strUserCode = LB_UserCode.Text.Trim();

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = (Collaboration)lst[0];

        collaboration.CollaborationName = strCollaborationName;
        collaboration.Comment = strContent;


        try
        {
            collaborationBLL.UpdateCollaboration(collaboration, int.Parse(strCoID));

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
                        string memid = UpdateCollaborationMember(strCoID, ((Button)RP_Attendant.Items[i].FindControl("BT_UserCode")).Text.Trim(), ((Button)RP_Attendant.Items[i].FindControl("BT_UserName")).Text.Trim());
                        strMemID += "," + memid;
                    }
                }
                DeleteCollaborationMember(strCoID, strMemID);
            }
            #endregion

            LoadCollaborationList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG")+"')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB")+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }


    protected void BT_Close_Click(object sender, EventArgs e)
    {
        string strCoID;
        string strUserCode;
        string strHQL;
        IList lst;

        strCoID = LB_CoID.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = (Collaboration)lst[0];

        collaboration.Status = "Closed";

        try
        {
            collaborationBLL.UpdateCollaboration(collaboration, int.Parse(strCoID));

            LB_Status.Text = "Closed";

            LoadCollaborationList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGBCG")+"')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZGBSBJC")+"')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void BT_Active_Click(object sender, EventArgs e)
    {
        string strCoID;
        string strUserCode;
        string strHQL;
        IList lst;

        strCoID = LB_CoID.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        Collaboration collaboration = (Collaboration)lst[0];

        collaboration.Status = "InProgress";

        try
        {
            collaborationBLL.UpdateCollaboration(collaboration, int.Parse(strCoID));

            LB_Status.Text = "InProgress";

            LoadCollaborationList(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHCG")+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJHSBJC")+"')", true);
        }

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        #region 新增 By LiuJianping 2013-09-11

        string struserCode = ((Button)e.Item.FindControl("BT_UserCode")).Text;
        string struserName = ((Button)e.Item.FindControl("BT_UserName")).Text;
        string usercodeGold = LB_UserCode.Text.Trim();//操作者

        if (struserCode.Trim() == usercodeGold)
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCJZBYJRXTHZDJRJC")+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            return;
        }

        DataSet ds = GetCollaborationMemberModule(RP_Attendant);
        DataTable dt = ds.Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["UserCode"].ToString().Trim() == struserCode.Trim())
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZCCYYCZBNZFJRJC")+"')", true);

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
    protected void AddCollaborationMember(string strCoID, string struserCode, string struserName)
    {
        CollaborationMemberBLL collaborationMemberBLL = new CollaborationMemberBLL();
        CollaborationMember collaborationMember = new CollaborationMember();
        collaborationMember.CoID = int.Parse(strCoID);
        collaborationMember.UserCode = struserCode.Trim();
        collaborationMember.UserName = struserName.Trim();
        collaborationMember.CreateTime = DateTime.Now;
        collaborationMember.LastLoginTime = DateTime.Now;

        collaborationMemberBLL.AddCollaborationMember(collaborationMember);
    }

    /// <summary>
    /// 更新参与人员 By LiuJianping  2013-09-11
    /// </summary>
    /// <param name="strCoID"></param>
    /// <param name="struserCode"></param>
    /// <param name="struserName"></param>
    protected string UpdateCollaborationMember(string strCoID, string struserCode, string struserName)
    {
        string MemID = "0";
        CollaborationMemberBLL collaborationMemberBLL = new CollaborationMemberBLL();
        string strHQL = "from CollaborationMember as collaborationMember where collaborationMember.CoID = '" + strCoID + "' and collaborationMember.UserName = '" + struserName + "' and collaborationMember.UserCode='" + struserCode + "' ";
        IList lst = collaborationMemberBLL.GetAllCollaborationMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            CollaborationMember collaborationMember = (CollaborationMember)lst[0];
            MemID = collaborationMember.MemID.ToString();
        }
        else
        {
            AddCollaborationMember(strCoID, struserCode, struserName);
            MemID = GetCollaborationMemberID(strCoID, struserCode, struserName);
        }
        return MemID;
    }

    /// <summary>
    /// 删除参与人员 By LiuJianping  2013-09-11
    /// </summary>
    /// <param name="strCoID"></param>
    /// <param name="strMemIDList">如1,2,3,4</param>
    protected void DeleteCollaborationMember(string strCoID, string strMemIDList)
    {
        CollaborationMemberBLL collaborationMemberBLL = new CollaborationMemberBLL();
        string strHQL = "from CollaborationMember as collaborationMember where collaborationMember.CoID = '" + strCoID + "' and collaborationMember.MemID not in (" + strMemIDList + ") ";
        IList lst = collaborationMemberBLL.GetAllCollaborationMembers(strHQL);
        if (lst.Count > 0 && lst != null)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                CollaborationMember collaborationMember = (CollaborationMember)lst[i];
                collaborationMemberBLL.DeleteCollaborationMember(collaborationMember);
            }
        }
    }

    /// <summary>
    /// 获取创建参与人员的即时ID By LiuJianping  2013-09-11
    /// </summary>
    /// <param name="strUserCode"></param>
    /// <returns></returns>
    protected string GetCollaborationMemberID(string strCoID, string struserCode, string struserName)
    {
        string strHQL = "Select Max(MemID) From T_CollaborationMember Where CoID = '" + strCoID + "' and UserName = '" + struserName + "' and UserCode='" + struserCode + "' ";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_CollaborationMember");

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

                strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strID;
                CollaborationBLL collaborationBLL = new CollaborationBLL();
                lst = collaborationBLL.GetAllCollaborations(strHQL);

                Collaboration collaboration = (Collaboration)lst[0];

                LB_CoID.Text = collaboration.CoID.ToString();
                TB_CollaborationName.Text = collaboration.CollaborationName.Trim();

                if (strIsMobileDevice == "YES")
                {
                    HTEditor1.Text = collaboration.Comment.Trim();
                }
                else
                {
                    CKEditor1.Text = collaboration.Comment.Trim();
                }

                LB_Status.Text = collaboration.Status.Trim();

                LoadCollaborationMember(strID);

                HL_RelatedDoc.NavigateUrl = "TTCollaborationRelatedDoc.aspx?RelatedID=" + strID;
                TB_Message.Text = LanguageHandle.GetWord("NiYiBeiYaoQingCanJiaXieZuo") + strID + " " + collaboration.CollaborationName.Trim() + LanguageHandle.GetWord("YaoQingZhe") + collaboration.CreatorName.Trim() + " " + LanguageHandle.GetWord("QingZhunShiCanJia");


                HL_RelatedDoc.Enabled = true;
                BT_Close.Enabled = true;
                BT_Active.Enabled = true;
                BT_Send.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }


            if (e.CommandName == "Delete")
            {
                string strUserCode;

                strUserCode = LB_UserCode.Text.Trim();

                strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strID;
                CollaborationBLL collaborationBLL = new CollaborationBLL();
                lst = collaborationBLL.GetAllCollaborations(strHQL);

                Collaboration collaboration = (Collaboration)lst[0];

                try
                {
                    collaborationBLL.DeleteCollaboration(collaboration);

                    BT_Close.Enabled = false;
                    BT_Active.Enabled = false;
                    BT_Send.Enabled = false;
                    HL_RelatedDoc.Enabled = false;

                    #region 删除参与人员 By LiuJianping 2013-09-11
                    DeleteCollaborationMember(strID, "0");
                    #endregion

                    LoadCollaborationMember(strID);
                    LoadCollaborationList(strUserCode);

                    HL_RelatedDoc.Enabled = false;

                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCCG") + "')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZSCSBJC") + "')", true);

                    ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
                }
            }
        }
    }

    protected void DataGrid4_PageIndexChanged(object sender, DataGridPageChangedEventArgs e)
    {
        DataGrid4.CurrentPageIndex = e.NewPageIndex;
        string strHQL = LB_Sql.Text.Trim();
        IList lst;

        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();
    }
    
    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

            #region 新增  By LiuJianping  2013-09-11
            DataTable dt = GetCollaborationMemberModule(RP_Attendant).Tables[0];
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
    protected DataSet GetCollaborationMemberModule(Repeater RP)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("CollaborationMemberModule");
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
        string strSubject, strMsg;
        IList lst;

        strUserCode = Session["UserCode"].ToString();

        string strCoID = LB_CoID.Text.Trim();

        strHQL = "from Collaboration as collaboration where collaboration.CoID = " + strCoID;
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);
        Collaboration collaboration = (Collaboration)lst[0];

        strHQL = "from CollaborationMember as collaborationMember where collaborationMember.CoID = " + strCoID;
        CollaborationMemberBLL collaborationMemberBLL = new CollaborationMemberBLL();
        lst = collaborationMemberBLL.GetAllCollaborationMembers(strHQL);

        CollaborationMember collaborationMember = new CollaborationMember();

        Msg msg = new Msg();

        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                collaborationMember = (CollaborationMember)lst[i];

                strReceiverCode = collaborationMember.UserCode.Trim();
                strMsg = TB_Message.Text.Trim();

                if (CB_MSM.Checked == true | CB_Mail.Checked == true)
                {
                    strSubject = LanguageHandle.GetWord("XieZuoTongZhi");

                    if (CB_MSM.Checked == true)
                    {
                        msg.SendMSM("Message",strReceiverCode, strMsg, strUserCode);
                    }

                    if (CB_Mail.Checked == true)
                    {
                        msg.SendMail(strReceiverCode, strSubject, strMsg, strUserCode);
                    }
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZXZTZFSWB")+"')", true);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }

    protected void LoadCollaborationList(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "from Collaboration as collaboration where collaboration.CreatorCode = " + "'" + strUserCode + "'";
        strHQL += " Order by collaboration.CoID DESC";
        CollaborationBLL collaborationBLL = new CollaborationBLL();
        lst = collaborationBLL.GetAllCollaborations(strHQL);

        DataGrid4.DataSource = lst;
        DataGrid4.DataBind();

        LB_Sql.Text = strHQL;
    }

    protected void LoadCollaborationMember(string strCOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from CollaborationMember as collaborationMember where collaborationMember.CoID = " + strCOID;
        CollaborationMemberBLL collaborationMemberBLL = new CollaborationMemberBLL();
        lst = collaborationMemberBLL.GetAllCollaborationMembers(strHQL);

        RP_Attendant.DataSource = lst;
        RP_Attendant.DataBind();
    }

}
