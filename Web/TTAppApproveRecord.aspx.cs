using ProjectMgt.BLL;
using ProjectMgt.Model;
using System;
using System.Collections;
using System.Web.UI;

public partial class TTAppApproveRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strType = Request.QueryString["Type"];
        string strRelatedID = Request.QueryString["RelatedID"].Trim();
        string strStepID = Request.QueryString["StepID"].Trim();

        string strWLName;
        string strHQL;
        IList lst;
        string strUserCode, strUserName;

        strUserCode = Session["UserCode"].ToString();
        strWLName = ShareClass.GetWorkFlowName(strRelatedID);

        if (strType == "WorkFlow")
        {
            //this.Title = LanguageHandle.GetWord("GongZuoLiu").ToString().Trim() + strRelatedID + " " + strWLName + "ÉóºË¼ÇÂ¼£¡";
            LB_WorkFlow.Text = LanguageHandle.GetWord("GongZuoLiu").ToString().Trim() + strRelatedID + " " + strWLName + LanguageHandle.GetWord("ShenHeJiLu").ToString().Trim();
        }
        else
        {
            //this.Title = LanguageHandle.GetWord("GongZuoLiu").ToString().Trim() + strRelatedID + " " + strWLName + LanguageHandle.GetWord("BuZhou").ToString().Trim() + strStepID + "ÉóºË¼ÇÂ¼£¡";
            LB_WorkFlow.Text = LanguageHandle.GetWord("GongZuoLiu").ToString().Trim() + strRelatedID + " " + strWLName + LanguageHandle.GetWord("BuZhou").ToString().Trim() + strStepID + LanguageHandle.GetWord("ShenHeJiLu").ToString().Trim();
        }

        if (Page.IsPostBack != true)
        {
            LB_UserCode.Text = strUserCode;
            strUserName = Session["UserName"].ToString();
            LB_UserName.Text = strUserName;

            try
            {
                if (strType == "WorkFlow")
                {
                    strHQL = "from Approve as approve where approve.Type = 'Workflow' and approve.RelatedID = " + strRelatedID + " Order by approve.ID DESC";
                }
                else
                {
                    strHQL = "from Approve as approve where approve.Type = 'Workflow' and approve.RelatedID = " + strRelatedID + " and approve.StepID = " + strStepID + " Order by approve.ID DESC";
                }

                ApproveBLL approveBLL = new ApproveBLL();
                lst = approveBLL.GetAllApproves(strHQL);

                DataList1.DataSource = lst;
                DataList1.DataBind();
            }
            catch
            {
            }
        }
    }

}
