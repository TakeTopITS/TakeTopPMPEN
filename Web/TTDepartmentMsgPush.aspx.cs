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

public partial class TTDepartmentMsgPush : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode;

        strUserCode = Session["UserCode"].ToString();
        LB_UserCode.Text = strUserCode;

        //this.Title = "组织级信息推送";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {
            LB_DepartString.Text = TakeTopCore.CoreShareClass.InitialDepartmentTreeByUserInfor(LanguageHandle.GetWord("ZZJGT"),TreeView1, strUserCode);

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

            LB_SelectedDepartCode.Text = strDepartCode;
            LB_SelectedDepartName.Text = ShareClass.GetDepartName(strDepartCode);
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
        string strHQL;

        string strID;
        string strMessage;
        string strUserCode;
        string strStatus;
        string strDepartString, strDepartCode;


        strMessage = TB_Message.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        strStatus = DL_Status.SelectedValue.Trim();

        strDepartCode = LB_SelectedDepartCode.Text.Trim();
        if (strDepartCode != "")
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByDepartment(strDepartCode);
        }
        else
        {
            strDepartString = LB_DepartString.Text.Trim();
        }

        strDepartString = strDepartString.Replace(",", "-");
        strDepartString = strDepartString.Replace("'", "");


        strHQL = "Insert Into T_DepartmentMsgPush(Message,DepartString,PushTime,OperatorCode,Status)";
        strHQL += " Values(" + "'" + strMessage + "'" + "," + "'" + strDepartString + "'" + ",now()," + "'" + strUserCode + "'" + "," + "'" + strStatus + "'" + ")";


        try
        {
            ShareClass.RunSqlCommand(strHQL);

            strID = ShareClass.GetMyCreatedMaxDepartmentMsgPushID(strUserCode);
            LB_ID.Text = strID;

            BT_Send.Enabled = true;

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

    protected void UpdateSMS(bool isPop)
    {
        string strHQL;

        string strID, strMessage;
        string strUserCode;
        string strDepartString, strDepartCode;

        strID = LB_ID.Text.Trim();
        strMessage = TB_Message.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();

        strDepartCode = LB_SelectedDepartCode.Text.Trim();
        if (strDepartCode != "")
        {
            strDepartString = TakeTopCore.CoreShareClass.InitialUnderDepartmentStringByDepartment(strDepartCode);
        }
        else
        {
            strDepartString = LB_DepartString.Text.Trim();
        }

        strDepartString = strDepartString.Replace(",", "-");
        strDepartString = strDepartString.Replace("'", "");

        strHQL = "Update T_DepartmentMsgPush Set Message = " + "'" + strMessage + "'" + ",Status = " + "'" + DL_Status.SelectedValue.Trim() + "'";
        strHQL += ",DepartString = " + "'" + strDepartString + "'";
        strHQL += " Where MsgID = " + strID;


        try
        {
            ShareClass.RunSqlCommand(strHQL);

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


    protected void DataGrid4_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;

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

                strHQL = "Select MsgID, Message,PushTime,Status From T_DepartmentMsgPush Where MsgID = " + strID;
                DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DepartmentMsgPush");

                LB_ID.Text = strID;

                TB_Message.Text = ds.Tables[0].Rows[0][1].ToString();
                DL_Status.SelectedValue = ds.Tables[0].Rows[0][2].ToString().Trim();

                BT_Send.Enabled = true;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                string strUserCode;

                strUserCode = LB_UserCode.Text.Trim();

                strHQL = "Delete From T_DepartmentMsgPush Where MsgID = " + strID;

                try
                {
                    ShareClass.RunSqlCommand(strHQL);

                    BT_Send.Enabled = false;

                    LoadSMSSendDIYList(strUserCode);

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


    protected void BT_Send_Click(object sender, EventArgs e)
    {
        string strHQL;
        string strMsg, strID, strDepartString, strUserCode, strReceiverCode;
        int intAllCount;

        strUserCode = Session["UserCode"].ToString();
        strDepartString = LB_DepartString.Text.Trim();

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

        strHQL = "Select UserCode From T_ProjectMember Where DepartCode in " + strDepartString;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ProjectMember");

        Msg msg = new Msg();

        intAllCount = ds.Tables[0].Rows.Count;

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strReceiverCode = ds.Tables[0].Rows[0][0].ToString();
            strMsg = TB_Message.Text.Trim();

            //发送信息
            msg.SendMSM("Message",strReceiverCode, strMsg, strUserCode);

            LB_SendedNumber.Text = (i + 1).ToString() + "/" + intAllCount.ToString();
        }

        LoadSMSSendDIYList(strUserCode);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZDXFSWB") + "')", true);

        ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
    }


    protected void LoadSMSSendDIYList(string strUserCode)
    {
        string strHQL;

        strHQL = "Select MsgID, Message,PushTime,Status From T_DepartmentMsgPush Order By MsgID DESC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_DepartmentMsgPush");

        DataGrid4.DataSource = ds;
        DataGrid4.DataBind();

        LB_Sql.Text = strHQL;
    }
}
