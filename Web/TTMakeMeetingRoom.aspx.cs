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

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTMakeMeetingRoom : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strDepartString;

        strUserCode = Session["UserCode"].ToString();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        Label1.Text = ShareClass.GetPageTitle(this.GetType().BaseType.Name + ".aspx"); bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", strUserCode);  //bool blVisible = TakeTopSecurity.TakeTopLicense.GetAuthobility(this.GetType().BaseType.Name + ".aspx", "ø‚¥Êπ‹¿Ì", strUserCode);
        if (blVisible == false)
        {
            Response.Redirect("TTDisplayErrors.aspx");
            return;
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack != true)
        {           
            strDepartString =  TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthorityAsset(LanguageHandle.GetWord("ZZJGT").ToString().Trim(),TreeView1, strUserCode);
            TB_DepartString.Text = strDepartString;

            LoadMeetingRoom(strUserCode);          
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

            TB_DepartCode.Text = strDepartCode;
            LB_DepartName.Text = ShareClass.GetDepartName(strDepartCode);
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
            AddMeetingRoom();
        }
        else
        {
            UpdateMeetingRoom();
        }
    }

    protected void AddMeetingRoom()
    {
        string strID,strRoomName,strRoomPosition,strNote;
        string strDepartCode;
        int intAccommodateCount;
    

        strRoomName = TB_RoomName.Text.Trim();
        strRoomPosition = TB_Address.Text.Trim();
        intAccommodateCount = int.Parse(TB_Count.Text.Trim());
        strNote = TB_Note.Text.Trim();
        strDepartCode = TB_DepartCode.Text.Trim();

        if (strDepartCode == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZJGGSBMBNWKCZSBJC").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);

            return;
        }

        MeetingRoomBLL meetingRoomBLL = new MeetingRoomBLL();
        MeetingRoom meetingRoom = new MeetingRoom();

        meetingRoom.RoomName = strRoomName;
        meetingRoom.RoomPosition = strRoomPosition;
        meetingRoom.AccommodateCount = intAccommodateCount;
        meetingRoom.Note = strNote;
        meetingRoom.BelongDepartCode = strDepartCode;
        meetingRoom.BelongDepartName = ShareClass.GetDepartName(strDepartCode);

        try
        {
            meetingRoomBLL.AddMeetingRoom(meetingRoom);

            strID = ShareClass.GetMyCreatedMaxMeetingRoomID();
      
            LoadMeetingRoom(strUserCode);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }
 

    protected void UpdateMeetingRoom()
    {
        string strRoomName, strRoomPosition, strNote,strID;
        string strDepartCode;
        int intAccommodateCount;
      

        strID = LB_ID .Text.Trim ();
        strRoomName = TB_RoomName.Text.Trim();
        strRoomPosition = TB_Address.Text.Trim();
        intAccommodateCount = int.Parse(TB_Count.Text.Trim());
        strNote = TB_Note.Text.Trim();
        strDepartCode = TB_DepartCode.Text.Trim();


        MeetingRoomBLL meetingRoomBLL = new MeetingRoomBLL();
        MeetingRoom meetingRoom = new MeetingRoom();

        meetingRoom.RoomName = strRoomName;
        meetingRoom.RoomPosition = strRoomPosition;
        meetingRoom.AccommodateCount = intAccommodateCount;
        meetingRoom.Note = strNote;
        meetingRoom.BelongDepartCode = strDepartCode;
        meetingRoom.BelongDepartName = ShareClass.GetDepartName(strDepartCode);

        try
        {
            meetingRoomBLL.UpdateMeetingRoom(meetingRoom, int.Parse(strID));

            LoadMeetingRoom(strUserCode);

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCCG").ToString().Trim()+"')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZBCSB").ToString().Trim()+"')", true);

            ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
        }
    }
 

    protected void DataGrid2_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strID, strHQL;
        IList lst;

        if (e.CommandName != "Page")
        {
            strID = e.Item.Cells[2].Text.Trim();

            if (e.CommandName == "Update")
            {
                for (int i = 0; i < DataGrid2.Items.Count; i++)
                {
                    DataGrid2.Items[i].ForeColor = Color.Black;
                }

                e.Item.ForeColor = Color.Red;


                strHQL = "from MeetingRoom as meetingRoom where meetingRoom.ID = " + strID;
                MeetingRoomBLL meetingRoomBLL = new MeetingRoomBLL();
                lst = meetingRoomBLL.GetAllMeetingRooms(strHQL);

                MeetingRoom meetingRoom = (MeetingRoom)lst[0];

                LB_ID.Text = meetingRoom.ID.ToString();
                TB_RoomName.Text = meetingRoom.RoomName.Trim();
                TB_Address.Text = meetingRoom.RoomPosition.Trim();
                TB_Count.Text = meetingRoom.AccommodateCount.ToString();
                TB_Note.Text = meetingRoom.Note.Trim();
                TB_DepartCode.Text = meetingRoom.BelongDepartCode;
                LB_DepartName.Text = meetingRoom.BelongDepartName;

                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "pop", "popShow('popwindow','true') ", true);
            }

            if (e.CommandName == "Delete")
            {
                strHQL = "from MeetingRoom as meetingRoom where meetingRoom.ID = " + strID;
                MeetingRoomBLL meetingRoomBLL = new MeetingRoomBLL();
                lst = meetingRoomBLL.GetAllMeetingRooms(strHQL);

                MeetingRoom meetingRoom = (MeetingRoom)lst[0];

                try
                {
                    meetingRoomBLL.DeleteMeetingRoom(meetingRoom);

            
                    LoadMeetingRoom(strUserCode);
                }
                catch
                {
                }
            }
        }
    }

    protected void LoadMeetingRoom(string strUserCode)
    {
        string strHQL;
        string strDepartString;

        strDepartString = TB_DepartString.Text;

        strHQL = " Select * From T_MeetingRoom Where ";
        strHQL += " BelongDepartCode in " + strDepartString;
        strHQL += " Order By ID ASC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_MeetingRoom");

        DataGrid2.DataSource = ds;
        DataGrid2.DataBind();
    }
  
}
