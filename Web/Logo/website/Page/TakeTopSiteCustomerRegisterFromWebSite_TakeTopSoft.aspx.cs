using Stimulsoft.Report.Dictionary;

using System;
using System.Data;
using System.Web;
using System.Web.UI;

using static com.sun.tools.javac.tree.DCTree;

public partial class TakeTopSiteCustomerRegisterFromWebSite_TakeTopSoft: System.Web.UI.Page
{
    string strSystemType, strWebSite, strLangCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        // 确定语言代码的优先级：Cookie > Session > 默认配置
        strLangCode = Request.Cookies["LangCode"]?.Value ??
                     Session["LangCode"]?.ToString() ??
                     System.Configuration.ConfigurationManager.AppSettings["DefaultLang"];


        // 确保Session和Cookie同步
        Session["LangCode"] = strLangCode;
        Response.SetCookie(new HttpCookie("LangCode", strLangCode)
        {
            Expires = DateTime.Now.AddYears(1)
        });

        strSystemType = Request.QueryString["SystemType"];
        strWebSite = Request.QueryString["WebSite"];
        if (strWebSite == null)
        {
            strWebSite = "";
        }

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "clickA", "aHandler();", true);
        if (Page.IsPostBack == false)
        {
            LoadTryProductResonType(strLangCode);

            LB_Product.Text = GetProductTypeNameByENType(strSystemType,strLangCode);
            LB_ProductHomeName.Text = GetProductTypeHomeNameByENType(strSystemType, strLangCode);
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
            LB_Message.Text = Resources.lang.TiJiaoShiBaiDaiHaoXiangBuNengW;
        }
        else
        {
            if (String.Compare(Request.Cookies["CheckCode"].Value, TB_CheckCode.Text, true) != 0)
            {
                TB_CheckCode.Text = "";

                LB_Message.Text = Resources.lang.TiJiaoShiBaiYanZhengMaCuoWuQin;
                return;
            }

            //推送信息给客服主管
            try
            {
                string strCSOperatorCode = ShareClass.GetWebSiteCustomerServiceOperatorCode(strWebSite);
                string strNofiInfo = Resources.lang.TiShiGongSi + strCompany + Resources.lang.DeYuanGong + strContactPerson + "( " + strPhoneNumber + " )" + Resources.lang.TiJiaoLe + strType + Resources.lang.DeShiYongXinXiQingGuanZhu;
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
            strSQL += " Values(" + "'" + strCompany + "'" + "," + "'" + strUserIP + "'" + "," + "'" + strUserPosition + "'" + "," + "'" + strContactPerson + "'" + "," + "'" + strPhoneNumber + "'" + "," + "'" + strEMail + "'" + "," + "'" + strAddress + "'" + "," + "'" + strPostCode + "'" + "," + "'" + strType + "'" + "," + "'" + strQuestion + "'" + "," + "now(),now()+'1 day'::interval," + "'" + Resources.lang.XinJian + "'"  + ",'','','',''," + "'" + strWebSite + "'" + ")";

            try
            {
                ShareClass.RunSqlCommandForNOOperateLog(strSQL);

                strTargetPage = GetProductTypeDemoURLByENType(strSystemType,strLangCode);

                string strScript = "openMDIFrom('" + strTargetPage + "');";
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", strScript, true);
            }
            catch (Exception err)
            {
                LogClass.WriteLogFile("Error page: " + err.Message.ToString() + "\n" + err.StackTrace);

                LB_Message.Text = Resources.lang.TiJiaoShiBaiQingJianCha;
            }
        }
    }

    protected string GetProductTypeNameByENType(string strENType,string strLangCode)
    {
        string strHQL;

        strHQL = string.Format(@"Select Type From T_RentProducttype Where ENType ='{0}' and LangCode ='{1}'", strENType,strLangCode);
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

    protected string GetProductTypeHomeNameByENType(string strENType, string strLangCode)
    {
        string strHQL;

        strHQL = string.Format(@"Select HomeTypeName From T_RentProducttype Where ENType ='{0}' and LangCode ='{1}'", strENType, strLangCode);
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


    protected string GetProductTypeDemoURLByENType(string strENType,string strLangCode)
    {
        string strHQL;

        strHQL = string.Format(@"Select DemoURL From T_RentProducttype Where ENType ='{0}' and LangCode ='{1}'", strENType,strLangCode);
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

    protected void LoadTryProductResonType(string strLangCode)
    {
        string strHQL;

        strHQL = "Select * From T_TryProductResontype Where LangCode ='" + strLangCode + "' Order By SortNumber ASC";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_TryProductResontype");

        DL_TryProductResonType.DataSource = ds;
        DL_TryProductResonType.DataBind();
    }
}
