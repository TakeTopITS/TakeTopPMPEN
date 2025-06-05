using System;
using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTCustomerQuestion : System.Web.UI.Page
{
    string strWebSite;

    protected void Page_Load(object sender, EventArgs e)
    {
        strWebSite = Request.QueryString["WebSite"];
        if (strWebSite == null)
        {
            strWebSite = "";
        }

        string strUserCode = Session["UserCode"].ToString();
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true); 
        if (Page.IsPostBack == false)
        {
            LoadCustomerQuestionType();
        }
    }

    protected void BT_Summit_Click(object sender, EventArgs e)
    {
        string strCompany, strUserIP, strUserPosition, strContactPerson, strPhoneNumber, strEMail, strAddress, strPostCode, strType, strQuestion;
        string strSQL;
        DateTime dtAnswerTime;


        strUserIP = Request.UserHostAddress.Trim();
        strUserPosition = ShareClass.GetIPinArea(strUserIP);

        strCompany = TB_Company.Text.Trim();

        strContactPerson = TB_ContactPerson.Text.Trim();
        strPhoneNumber = TB_PhoneNumber.Text.Trim();
        strEMail = TB_EMail.Text.Trim();
        strAddress = TB_Address.Text.Trim();
        strPostCode = TB_PostCode.Text.Trim();
        strType = DL_CustomerQuestionType.SelectedValue.Trim();
        strQuestion = TB_Question.Text.Trim();
        dtAnswerTime = DateTime.Now.AddHours(24);


        if (strCompany == "" | strContactPerson == "" | strPhoneNumber == "" | strEMail == "" | strQuestion == "" | strAddress == "")
        {
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZJGDHXBNWKJC") + "')", true);

            LB_Message.Text = "" + LanguageHandle.GetWord("ZZTJSBJC") + "";
        }
        else
        {
            if (String.Compare(Request.Cookies["CheckCode"].Value, TB_CheckCode.Text, true) != 0)
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('" + LanguageHandle.GetWord("ZZYZMCWSRZDYZM") + "')", true);
                TB_CheckCode.Text = "";

                LB_Message.Text = "" + LanguageHandle.GetWord("ZZTJSBJC") + "";
                return;
            }

            //推送信息给客服主管
            try
            {
                string strCSOperatorCode = ShareClass.GetWebSiteCustomerServiceOperatorCode(strWebSite);
                string strNofiInfo = LanguageHandle.GetWord("DiShiGongSi") + strCompany + LanguageHandle.GetWord("DeZhi") + strAddress + LanguageHandle.GetWord("DeYuanGong") + strContactPerson + LanguageHandle.GetWord("ZhuCeLe") + strType + LanguageHandle.GetWord("DeXinXiQingGuanZhu");
                Action action = new Action(delegate ()
                {
                    try
                    {
                        ////信息推送接口
                        //NotificationHelper.ApplePush(strCSOperatorCode, strNofiInfo, 1);

                        Msg msg = new Msg();
                        msg.SendMSM("Message",strCSOperatorCode, strNofiInfo, "ADMIN");
                    }
                    catch (Exception ex)
                    {

                    }
                });
                System.Threading.Tasks.Task.Factory.StartNew(action);
            }
            catch (Exception ex)
            {
                //LB_Message.Text = ex.Message.ToString();
                //return;
            }

            strSQL = " Insert into T_CustomerQuestion(Company,UserIP,UserPosition,ContactPerson,PhoneNumber,EMail,Address,PostCode,Type,Question,SummitTime,AnswerTime,Status,RecorderCode,OperatorCode,OperatorName,OperatorStatus,FromWebSite)";
            strSQL += " Values(" + "'" + strCompany + "'" + "," + "'" + strUserIP + "'" + "," + "'" + strUserPosition + "'" + "," + "'" + strContactPerson + "'" + "," + "'" + strPhoneNumber + "'" + "," + "'" + strEMail + "'" + "," + "'" + strAddress + "'" + "," + "'" + strPostCode + "'" + "," + "'" + strType + "'" + "," + "'" + strQuestion + "'" + "," + "now(),now()+'1 day'::interval," + "'New'" + ",'','','',''," + "'" + strWebSite + "'" + ")";


            try
            {
                ShareClass.RunSqlCommandForNOOperateLog(strSQL);

                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJCGTDKFHZYTZNBNJJWTXX")+"')", true);

                LB_Message.Text = "" + LanguageHandle.GetWord("ZZTJCGTDKFHZYTZNBNJJWTXX") + "";
            }
            catch
            {
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", "alert('"+LanguageHandle.GetWord("ZZTJSBJC")+"')", true);

                LB_Message.Text = "" + LanguageHandle.GetWord("ZZTJSBJC") + "";
            }
        }
    }

    protected void LoadCustomerQuestionType()
    {
        string strHQL;
        IList lst;

        strHQL = "from CustomerQuestionType as customerQuestionType Order By customerQuestionType.SortNumber ASC";
        CustomerQuestionTypeBLL customerQuestionTypeBLL = new CustomerQuestionTypeBLL();
        lst = customerQuestionTypeBLL.GetAllCustomerQuestionTypes(strHQL);

        DL_CustomerQuestionType.DataSource = lst;
        DL_CustomerQuestionType.DataBind();
    }
}