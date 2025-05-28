using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;

public partial class TTProLeadReview : System.Web.UI.Page
{
    string strIsMobileDevice;
    string strProjectID, strPMCode;
    string strUserCode, strUserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;


        strIsMobileDevice = Session["IsMobileDevice"].ToString();

        //CKEditor³õÊ¼»¯
        CKFinder.FileBrowser _FileBrowser = new CKFinder.FileBrowser();
        _FileBrowser.BasePath = "ckfinder/";
        _FileBrowser.SetupCKEditor(HE_LeaderReview);
HE_LeaderReview.Language = Session["LangCode"].ToString();

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        strProjectID = Request.QueryString["ProjectID"];
        LB_ProjectID.Text = strProjectID;

        LB_UserCode.Text = strUserCode;
        LB_UserName.Text = strUserName;


        strHQL = "from Project as project where project.ProjectID = " + strProjectID;
        ProjectBLL projectBLL = new ProjectBLL();
        IList lst = projectBLL.GetAllProjects(strHQL);
        Project project = (Project)lst[0];

        strPMCode = project.PMCode.Trim();
        LB_ProjectName.Text = project.ProjectName;

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            if (strIsMobileDevice == "YES")
            {
                HT_LeaderReview.Visible = true;
            }
            else
            {
                HE_LeaderReview.Visible = true;
            }


            strHQL = "from ProLeaderReview as proLeaderReview where proLeaderReview.ProjectID = " + strProjectID + " and proLeaderReview.UserCode = " + "'" + strUserCode + "'" + " and to_char(proLeaderReview.ReviewTime,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + " Order by proLeaderReview.ID DESC";
            ProLeaderReviewBLL proLeaderReviewBLL = new ProLeaderReviewBLL();
            lst = proLeaderReviewBLL.GetAllProLeaderReviews(strHQL);

            if (lst.Count > 0)
            {
                ProLeaderReview proLeaderReveiew = (ProLeaderReview)lst[0];

                if (strIsMobileDevice == "YES")
                {
                    HT_LeaderReview.Text = proLeaderReveiew.Review.Trim();
                }
                else
                {
                    HE_LeaderReview.Text = proLeaderReveiew.Review.Trim();
                }
                LB_ID.Text = proLeaderReveiew.ID.ToString();
            }
            else
            {
                LB_ID.Text = "-1";
            }

            LoadReviewList();

            HL_DailyWorkSum.NavigateUrl = "TTProjectSummaryAnalystChart.aspx?ProjectID=" + strProjectID;
        }
    }

    protected void BT_Summit_Click(object sender, EventArgs e)
    {
        string strProjectID, strUserCode, strUserName, strLeadReview;
        string strID, strHQL;
        IList lst;

        strID = LB_ID.Text.Trim();

        strProjectID = LB_ProjectID.Text.Trim();
        strUserCode = LB_UserCode.Text.Trim();
        strUserName = ShareClass.GetUserName(strUserCode);

        if (strIsMobileDevice == "YES")
        {
            strLeadReview = HT_LeaderReview.Text.Trim();
        }
        else
        {
            strLeadReview = HE_LeaderReview.Text.Trim();
        }

        ProLeaderReviewBLL proLeaderReviewBLL = new ProLeaderReviewBLL();
        ProLeaderReview proLeaderReview = new ProLeaderReview();

        proLeaderReview.ProjectID = int.Parse(strProjectID);
        proLeaderReview.UserCode = strUserCode;
        proLeaderReview.UserName = strUserName;
        proLeaderReview.Review = strLeadReview;
        proLeaderReview.ReviewTime = DateTime.Now;

        try
        {
            if (strID == "-1")
            {
                proLeaderReviewBLL.AddProLeaderReview(proLeaderReview);

                strHQL = "from ProLeaderReview as proLeaderReview where proLeaderReview.ProjectID = " + strProjectID + " and proLeaderReview.UserCode = " + "'" + strUserCode + "'" + " and to_char(proLeaderReview.ReviewTime,'yyyymmdd') = " + "'" + DateTime.Now.ToString("yyyyMMdd") + "'" + " Order by proLeaderReview.ID DESC";
                proLeaderReviewBLL = new ProLeaderReviewBLL();
                lst = proLeaderReviewBLL.GetAllProLeaderReviews(strHQL);
                proLeaderReview = (ProLeaderReview)lst[0];

                LB_ID.Text = proLeaderReview.ID.ToString();
            }
            else
            {
                proLeaderReview.ID = int.Parse(strID);
                proLeaderReviewBLL.UpdateProLeaderReview(proLeaderReview, int.Parse(strID));
            }

            strHQL = "from ProLeaderReview as proLeaderReview where proLeaderReview.ProjectID = " + strProjectID + " Order by proLeaderReview DESC";
            lst = proLeaderReviewBLL.GetAllProLeaderReviews(strHQL);

            DataList1.DataSource = lst;
            DataList1.DataBind();

            try
            {
                Msg msg = new Msg();
                msg.SendMSM("Message", strPMCode, strLeadReview, strUserCode);
            }
            catch
            {
            }

            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSCG").ToString().Trim() + "')", true);

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZFSSBJC").ToString().Trim() + "')", true);
        }
    }


    protected void LoadReviewList()
    {
        string strProjectID = LB_ProjectID.Text;
        string strHQL = "from ProLeaderReview as proLeaderReview where proLeaderReview.ProjectID = " + strProjectID + " Order by proLeaderReview.ID DESC";
        ProLeaderReviewBLL proLeaderReviewBLL = new ProLeaderReviewBLL();
        IList lst = proLeaderReviewBLL.GetAllProLeaderReviews(strHQL);


        DataList1.DataSource = lst;
        DataList1.DataBind();
    }


}
