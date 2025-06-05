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

public partial class TTMyWorkDetailMain : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strWLID = Request.QueryString["WLID"];

        string strWLName;
        string strHQL;
        IList lst;

        //�����Ƿ��Զ��幤����ģ��ģʽ
        Session["DIYWFModule"] = "NO";

        string strUserCode, strCreatorCode;
        strUserCode = Session["UserCode"].ToString();

        strHQL = "from WorkFlow as workFlow where workFlow.WLID = " + strWLID;
        WorkFlowBLL workFlowBLL = new WorkFlowBLL();
        lst = workFlowBLL.GetAllWorkFlows(strHQL);
        WorkFlow workFlow = (WorkFlow)lst[0];

        strWLName = workFlow.WLName.Trim();

        this.Title = LanguageHandle.GetWord("WoDeGongZuoLiu") + ": " + strWLID + " " + strWLName;

        strCreatorCode = workFlow.CreatorCode.Trim();

        if (strUserCode != strCreatorCode )
        {
            string strErrorMsg = LanguageHandle.GetWord("ZZJGNBSZLCDFQRBNCKCLCQJC");
            Response.Redirect("TTDisplayCustomErrorMessage.aspx?ErrorMsg=" + strErrorMsg);
        }
    }
}
