using Stimulsoft.Report.Dictionary;

using System;
using System.Data;
using System.Web.UI;

using static com.sun.tools.javac.tree.DCTree;

public partial class TakeTopSiteCustomerRegisterFromWebSite_TakeTopSoft: System.Web.UI.Page
{
    string strSystemType,strWebSite;

    protected void Page_Load(object sender, EventArgs e)
    {
        strSystemType = Request.QueryString["SystemType"];

        strWebSite = Request.QueryString["WebSite"];
        if (strWebSite == null)
        {
            strWebSite = "";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadTryProductResonType();

            LB_Product.Text = GetProductTypeNameByENType(strSystemType);
        }
    }

    protected void BT_Summit_Click(object sender, EventArgs e)
    {
        string strCompany, strUserIP, strUserPosition, strContactPerson, strPhoneNumber, strEMail, strAddress, strPostCode, strType, strQuestion;
        string strSQL;
        DateTime dtAnswerTime;
        string strTargetPage = "";

        strUserIP = Request.UserHostAddress.Trim();
        strUserPosition = ShareClass.GetIPinArea(strUserIP);
        strCompany = TB_Company.Text.Trim();
        strContactPerson = TB_ContactPerson.Text.Trim();
        strPhoneNumber = TB_PhoneNumber.Text.Trim();
        strEMail = TB_EMail.Text.Trim();
        strAddress = TB_Address.Text.Trim();
        strPostCode = TB_PostCode.Text.Trim();
        strType = LB_Product.Text.Trim();
        strQuestion = DL_TryProductResonType.SelectedValue;
        dtAnswerTime = DateTime.Now.AddHours(24);

        if (strCompany == "" | strContactPerson == "" | strPhoneNumber == "" | strQuestion == "")
        {
            LB_Message.Text = "提交失败，带*号项不能为空，请检查！";
        }
        else
        {
            if (String.Compare(Request.Cookies["CheckCode"].Value, TB_CheckCode.Text, true) != 0)
            {
                TB_CheckCode.Text = "";

                LB_Message.Text = "提交失败，验证码错误，请检查！";
                return;
            }

            //推送信息给客服主管
            try
            {
                string strCSOperatorCode = ShareClass.GetWebSiteCustomerServiceOperatorCode(strWebSite);
                string strNofiInfo = "提示：公司: " + strCompany + " 的员工: " + strContactPerson + "( " + strPhoneNumber + " )" + " 提交了：" + strType + " 的试用信息，请关注！！！";
                Action action = new Action(delegate ()
                {
                    try
                    {
                        Msg msg = new Msg();
                        msg.SendMSM("Message", strCSOperatorCode, strNofiInfo, "ADMIN");
                    }
                    catch (Exception ex)
                    {
                    }
                });
                System.Threading.Tasks.Task.Factory.StartNew(action);
            }
            catch (Exception ex)
            {
                LB_Message.Text = ex.Message.ToString();
                return;
            }

            strSQL = " Insert into T_CustomerQuestion(Company,UserIP,UserPosition,ContactPerson,PhoneNumber,EMail,Address,PostCode,Type,Question,SummitTime,AnswerTime,Status,RecorderCode,OperatorCode,OperatorName,OperatorStatus,FromWebSite)";
            strSQL += " Values(" + "'" + strCompany + "'" + "," + "'" + strUserIP + "'" + "," + "'" + strUserPosition + "'" + "," + "'" + strContactPerson + "'" + "," + "'" + strPhoneNumber + "'" + "," + "'" + strEMail + "'" + "," + "'" + strAddress + "'" + "," + "'" + strPostCode + "'" + "," + "'" + strType + "'" + "," + "'" + strQuestion + "'" + "," + "now(),now()+'1 day'::interval," + "'新建'" + ",'','','',''," + "'" + strWebSite + "'" + ")";

            try
            {
                ShareClass.RunSqlCommandForNOOperateLog(strSQL);

                strTargetPage = GetProductTypeDemoURLByENType(strSystemType);

                string strScript = "openMDIFrom('" + strTargetPage + "');";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", strScript, true);
            }
            catch
            {
                LB_Message.Text = "提交失败，请检查！";
            }
        }
    }

    protected string GetProductTypeNameByENType(string strENType)
    {
        string strHQL;

        strHQL = string.Format(@"Select Type From T_RentProducttype Where ENType ='{0}'", strENType);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentProducttype");
        if(ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    protected string GetProductTypeDemoURLByENType(string strENType)
    {
        string strHQL;

        strHQL = string.Format(@"Select DemoURL From T_RentProducttype Where ENType ='{0}'", strENType);
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentProducttype");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            return "";
        }
    }

    protected void LoadTryProductResonType()
    {
        string strHQL;

        strHQL = "Select * From T_RentProductVerType Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_RentProductType");

        DL_TryProductResonType.DataSource = ds;
        DL_TryProductResonType.DataBind();
    }
}