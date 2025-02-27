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

public partial class TTMailProfileSet : System.Web.UI.Page
{
    string strUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strHQL;
        IList lst;
 
        string strPasswordSet;
        strUserCode = Request.QueryString["UserCode"].Trim();
        

        if (!Page.IsPostBack)
        {
            try
            {
                strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strUserCode + "'";
                MailProfileBLL mailProfileBLL = new MailProfileBLL();
                lst = mailProfileBLL.GetAllMailProfiles(strHQL);

                if (lst.Count > 0)
                {
                    MailProfile mailProfile = (MailProfile)lst[0];

                    LB_ID.Text = mailProfile.WebMailID.ToString();
                    TB_UserName.Text = mailProfile.UserName.Trim();
                    TB_AliasName.Text = mailProfile.AliasName.Trim();
                    TB_Email.Text = mailProfile.Email.Trim();
                    TB_Password.Text = mailProfile.Password.Trim();
                    TB_SmtpIP.Text = mailProfile.SmtpServerIP.Trim();
                    TB_SmtpPort.Text = mailProfile.SmtpServerPort.ToString();
                    TB_Pop3ServerIP.Text = mailProfile.Pop3ServerIP.Trim();
                    TB_Pop3ServerPort.Text = mailProfile.Pop3ServerPort.ToString();

                    if (mailProfile.EnablePOP3SSL.Trim() == "YES")
                    {
                        CB_EnablePOPSSL.Checked = true;
                    }
                    else
                    {
                        CB_EnablePOPSSL.Checked = false;
                    }

                    if (mailProfile.EnableSMTPSSL.Trim() == "YES")
                    {
                        CB_EnableSMTPSSL.Checked = true;
                    }
                    else
                    {
                        CB_EnableSMTPSSL.Checked = false;
                    }
                }

                strPasswordSet = GetMailBoxAuthorityPassword(strUserCode);
                if (strPasswordSet == "NO")
                {
                    TB_Password.Enabled = false;
                }
            }
            catch
            {

            }
        }
    }


    protected void BT_Add_Click(object sender, EventArgs e)
    {
        int intMailID;
        string strUserName = TB_UserName.Text.Trim();
        string strAliasName = TB_AliasName.Text.Trim();
        string strPassword = TB_Password.Text.Trim();
        string strEMail = TB_Email.Text.Trim();
        string strSmtpServerIP = TB_SmtpIP.Text.Trim();
        string strSmtpServerPort = TB_SmtpPort.Text.Trim();
        string strPop3ServerIP = TB_Pop3ServerIP.Text.Trim();
        string strPop3ServerPort = TB_Pop3ServerPort.Text.Trim();

        string strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strUserCode + "'";
        MailProfileBLL mailProfileBLL = new MailProfileBLL();
        IList lst = mailProfileBLL.GetAllMailProfiles(strHQL);

        try
        {
            if (lst.Count == 0)
            {
                MailProfile mailProfile = new MailProfile();

                mailProfile.UserCode = strUserCode;
                mailProfile.UserName = strUserName;
                mailProfile.AliasName = strAliasName;
                mailProfile.Email = strEMail;
                mailProfile.Password = strPassword;
                mailProfile.SmtpServerIP = strSmtpServerIP;
                if (CB_EnableSMTPSSL.Checked == true)
                {
                    mailProfile.EnableSMTPSSL = "YES";
                }
                else
                {
                    mailProfile.EnableSMTPSSL = "NO";
                }

                mailProfile.SmtpServerPort = int.Parse(strSmtpServerPort);
                mailProfile.Pop3ServerIP = strPop3ServerIP;

                if (CB_EnablePOPSSL.Checked == true)
                {
                    mailProfile.EnablePOP3SSL = "YES";
                }
                else
                {
                    mailProfile.EnablePOP3SSL = "NO";
                }

                mailProfile.Pop3ServerPort = int.Parse(strPop3ServerPort);

                mailProfileBLL.AddMailProfile(mailProfile);
            }
            else
            {
                MailProfile mailProfile = (MailProfile)lst[0];

                intMailID = int.Parse(LB_ID.Text.Trim());

                mailProfile.UserCode = strUserCode;
                mailProfile.UserName = strUserName;
                mailProfile.AliasName = strAliasName;
                mailProfile.Email = strEMail;
                mailProfile.Password = strPassword;
                mailProfile.SmtpServerIP = strSmtpServerIP;
                if (CB_EnableSMTPSSL.Checked == true)
                {
                    mailProfile.EnableSMTPSSL = "YES";
                }
                else
                {
                    mailProfile.EnableSMTPSSL = "NO";
                }

                mailProfile.SmtpServerPort = int.Parse(strSmtpServerPort);
                mailProfile.Pop3ServerIP = strPop3ServerIP;

                if (CB_EnablePOPSSL.Checked == true)
                {
                    mailProfile.EnablePOP3SSL = "YES";
                }
                else
                {
                    mailProfile.EnablePOP3SSL = "NO";
                }

                mailProfile.Pop3ServerPort = int.Parse(strPop3ServerPort);

                mailProfileBLL.UpdateMailProfile(mailProfile, intMailID);
            }

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCCG").ToString().Trim() + "');</script>");
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('" + LanguageHandle.GetWord("ZZBCSBJC").ToString().Trim() + "');</script>");
        }
    }

    protected string GetMailBoxAuthorityPassword(string strUserCode)
    {
        string strHQL;
        IList lst;

        strHQL = "From MailBoxAuthority as mailBoxAuthority where mailBoxAuthority.UserCode = " + "'" + strUserCode + "'";
        MailBoxAuthorityBLL mailBoxAuthorityBLL = new MailBoxAuthorityBLL();
        lst = mailBoxAuthorityBLL.GetAllMailBoxAuthoritys(strHQL);

        MailBoxAuthority mailBoxAuthority = (MailBoxAuthority)lst[0];

        return mailBoxAuthority.PasswordSet.Trim();
    }
}
