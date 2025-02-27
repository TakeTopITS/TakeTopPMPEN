using System; using System.Resources;
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

public partial class TTScheduleEventLesderReview : System.Web.UI.Page
{
    IList lst;

    string strIsMobileDevice;
    string strScheduleUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        string strUserCode, strUserName;
        string strScheduleID, strScheduleName;

        
        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor初始化
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_LeaderReview);

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);
        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;

        strScheduleID = Request.QueryString["ScheduleID"];
       
        strHQL = "Select Name,UserCode From T_ScheduleEvent Where ID = " + strScheduleID;
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_ScheduleEvent");
        strScheduleName = ds.Tables[0].Rows[0][0].ToString();
        LB_ScheduleID.Text = strScheduleID;
        LB_ScheduleName.Text = strScheduleName;
        strScheduleUserCode = ds.Tables[0].Rows[0][1].ToString().Trim();

        //this.Title = "日程：" + strScheduleID + " " + strScheduleName + " 的评审意见";

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_LeaderReview.Visible = true;
            }
            else
            {
                HE_LeaderReview.Visible = true;
            }
         

            strHQL = "from ScheduleEventLeaderReview as scheduleEventLeaderReview where scheduleEventLeaderReview.ScheduleID = " + strScheduleID + " and scheduleEventLeaderReview.LeaderCode = " + "'" + strUserCode + "'" + " and to_char(scheduleEventLeaderReview.ReviewTime,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + " Order by scheduleEventLeaderReview.ReviewID DESC";
            ScheduleEventLeaderReviewBLL scheduleEventLeaderReviewBLL = new ScheduleEventLeaderReviewBLL();
            lst = scheduleEventLeaderReviewBLL.GetAllScheduleEventLeaderReviews(strHQL);

            if (lst.Count > 0)
            {
                BT_Summit.Text = LanguageHandle.GetWord("XiuGai").ToString().Trim();
                ScheduleEventLeaderReview scheduleEventLeaderReview = (ScheduleEventLeaderReview)lst[0];

                if (strIsMobileDevice == "YES")
                {
                    HT_LeaderReview.Text = scheduleEventLeaderReview.Review.Trim();
                }
                else
                {
                    HE_LeaderReview.Text = scheduleEventLeaderReview.Review.Trim();
                }
                LB_ID.Text = scheduleEventLeaderReview.ReviewID.ToString();

                NB_Scoring.Amount = scheduleEventLeaderReview.Scoring;
            }
            else
            {
                LB_ID.Text = "-1";
            }

            LoadReviewList();

            HL_ReviewReport.NavigateUrl = "TTScheduleEventLeaderReviewReport.aspx?ScheduleID=" + strScheduleID;
        }
    }

    protected void LoadReviewList()
    {
        string strScheduleID = LB_ScheduleID.Text;
        string strHQL = "from ScheduleEventLeaderReview as scheduleEventLeaderReview where scheduleEventLeaderReview.ScheduleID = " + strScheduleID + " Order by scheduleEventLeaderReview.ReviewID DESC";
        ScheduleEventLeaderReviewBLL scheduleEventLeaderReviewBLL = new ScheduleEventLeaderReviewBLL();
        IList lst = scheduleEventLeaderReviewBLL.GetAllScheduleEventLeaderReviews(strHQL);


        DataList1.DataSource = lst;
        DataList1.DataBind();
    }

    protected void BT_Summit_Click(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;

        string strID, strScheduleID, strUserCode, strUserName, strLeadReview, strType;
        decimal deScoring;
    

        strID = LB_ID.Text.Trim();
        strScheduleID = LB_ScheduleID.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        strUserName = ShareClass.GetUserName(strUserCode);
        deScoring = NB_Scoring.Amount;

        strType = DL_Type.SelectedValue.Trim();

        if (strIsMobileDevice == "YES")
        {
            strLeadReview = HT_LeaderReview.Text.Trim();
        }
        else
        {
            strLeadReview = HE_LeaderReview.Text.Trim();
        }

        ScheduleEventLeaderReviewBLL scheduleEventLeaderReviewBLL = new ScheduleEventLeaderReviewBLL();
        ScheduleEventLeaderReview scheduleEventLeaderReview = new ScheduleEventLeaderReview();
        
        scheduleEventLeaderReview.ScheduleID = int.Parse(strScheduleID);
        scheduleEventLeaderReview.LeaderCode = strUserCode;
        scheduleEventLeaderReview.LeaderName = strUserName;
        scheduleEventLeaderReview.Review = strLeadReview;
        scheduleEventLeaderReview.ReviewTime = DateTime.Now;
        scheduleEventLeaderReview.Scoring = deScoring;

        try
        {
            if (strID == "-1")
            {
                scheduleEventLeaderReviewBLL.AddScheduleEventLeaderReview(scheduleEventLeaderReview);

                LB_ID.Text = ShareClass.GetMyCreatedMaxScheduleDailyWorkID();
            }
            else
            {
                strHQL = "Update T_ScheduleEvent_LeaderReview Set Review = " + "'" +strLeadReview+ "'" + ",Scoring = " + deScoring.ToString();
                strHQL += " Where ReviewID = " + strID;
                ShareClass.RunSqlCommand(strHQL);
            }


            if (DL_Type.SelectedValue.Trim() == "WEEK")
            {
                DateTime dt = DateTime.Now;  //当前时间
                int intWeekDay = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
                if(intWeekDay == 0)
                {
                    intWeekDay = 7;
                }

                DateTime startWeek = dt.AddDays(1 - intWeekDay);  //本周周一
                DateTime endWeek = startWeek.AddDays(6);  //本周周日

                strHQL = "Insert Into T_ScheduleEvent_LeaderReview(ScheduleID,LeaderCode,LeaderName,Review,ReviewTime,Scoring)";
                strHQL += " Select ID," + "'" + strUserCode + "'" + "," + "'" + strUserName + "'" + "," + "'" + strLeadReview + "'" + ",now()," + deScoring.ToString();
                strHQL += " From T_ScheduleEvent Where UserCode = " + "'" + strScheduleUserCode + "'" + " and  to_char(eventstart,'yyyymmdd')  >= " + "'" + startWeek.ToString("yyyyMMdd") + "'";
                strHQL += " And to_char(eventend,'yyyymmdd')  <= " + "'" + endWeek.ToString("yyyyMMdd") + "'";
                strHQL += " And ID <> + " + strScheduleID;
                ShareClass.RunSqlCommand(strHQL);
            }

            strHQL = "from ScheduleEventLeaderReview as scheduleEventLeaderReview where scheduleEventLeaderReview.ScheduleID = " + strScheduleID + " Order by scheduleEventLeaderReview.ReviewID DESC";
            lst = scheduleEventLeaderReviewBLL.GetAllScheduleEventLeaderReviews(strHQL);

            DataList1.DataSource = lst;
            DataList1.DataBind();
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJSBJC").ToString().Trim()+"')", true);
        }
    }

}
