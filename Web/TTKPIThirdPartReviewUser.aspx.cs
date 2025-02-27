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

using System.Text;
using System.IO;
using System.Web.Mail;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTKPIThirdPartReviewUser : System.Web.UI.Page
{
    string strUserCode, strUserName, strUserKPIID;

    protected void Page_Load(object sender, EventArgs e)
    {
        strUserKPIID = Request.QueryString["UserKPIID"];

        strUserCode = Session["UserCode"].ToString();
        strUserName = ShareClass.GetUserName(strUserCode);

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); if (Page.IsPostBack == false)
        {
            TakeTopCore.CoreShareClass.InitialDepartmentTreeByAuthority(LanguageHandle.GetWord("ZZJGT").ToString().Trim(), TreeView2, strUserCode);

            LoadKPIThirdPartReview(strUserKPIID);

            LB_UserKPIID.Text = strUserKPIID;
        }
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
    }

    protected void DataGrid3_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        string strUserKPIID, strOperatorCode;

        strUserKPIID = LB_UserKPIID.Text.Trim();

        if (strUserKPIID != "")
        {
            strOperatorCode = ((Button)e.Item.FindControl("BT_UserCode")).Text.Trim();

            KPIThirdPartReviewBLL kpiThirdPartReviewBLL = new KPIThirdPartReviewBLL();
            KPIThirdPartReview kpiThirdPartReview = new KPIThirdPartReview();

            kpiThirdPartReview.UserKPIID = int.Parse(strUserKPIID);
            kpiThirdPartReview.UserCode = strOperatorCode;
            kpiThirdPartReview.UserName = ShareClass.GetUserName(strOperatorCode);
            kpiThirdPartReview.Comment = "";
            kpiThirdPartReview.Point = 100;
            kpiThirdPartReview.ReviewTime = DateTime.Now;

            try
            {
                kpiThirdPartReviewBLL.AddKPIThirdPartReview(kpiThirdPartReview);

                LoadKPIThirdPartReview(strUserKPIID);
            }
            catch
            {
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZXZHTCNZJCY").ToString().Trim() + "')", true);
        }
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            string strUserName = ((Button)e.Item.FindControl("BT_UserName")).Text;

            string strHQL, strUserKPIID;
            IList lst;

            strUserKPIID = LB_UserKPIID.Text.Trim();
            strHQL = "from KPIThirdPartReview as kpiThirdPartReview where kpiThirdPartReview.UserKPIID = " + "'" + strUserKPIID + "'" + " and kpiThirdPartReview.UserName = " + "'" + strUserName + "'";
            KPIThirdPartReviewBLL kpiThirdPartReviewBLL = new KPIThirdPartReviewBLL();
            lst = kpiThirdPartReviewBLL.GetAllKPIThirdPartReviews(strHQL);

            KPIThirdPartReview kpiThirdPartReview = (KPIThirdPartReview)lst[0];

            kpiThirdPartReviewBLL.DeleteKPIThirdPartReview(kpiThirdPartReview);

            LoadKPIThirdPartReview(strUserKPIID);
        }
    }

    protected void LoadKPIThirdPartReview(string strUserKPIID)
    {
        string strHQL;
        IList lst;

        strHQL = "from KPIThirdPartReview as kpiThirdPartReview where kpiThirdPartReview.UserKPIID = " + strUserKPIID;
        KPIThirdPartReviewBLL kpiThirdPartReviewBLL = new KPIThirdPartReviewBLL();
        lst = kpiThirdPartReviewBLL.GetAllKPIThirdPartReviews(strHQL);

        RP_KPIThirdPartReview.DataSource = lst;
        RP_KPIThirdPartReview.DataBind();
    }

}