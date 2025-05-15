using LANZ;

using LumiSoft.Net.POP3.Client;

using ProjectMgt.BLL;
using ProjectMgt.Model;

using RTXSAPILib;

using RTXServerApi;

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Xml;// �����⼸�������ռ䣬����Ҫ������������System.Web��

using TakeTopSecurity;

/// <summary>
/// Msg ��ժҪ˵��
/// </summary>
public class Msg
{
    public Msg()
    {
        //
        // TODO: �ڴ˴���ӹ��캯���߼�
        //
    }

    //----����Ϣ,��ʱ����--------------------------
    public bool SendMSM(string strSubject, string strReceivedUserCode, string strMsg, string strSendUserCode)
    {
        try
        {
            new System.Threading.Thread(delegate ()
            {
                string strSendUserName = "";
                try
                {
                    strSendUserName = ShareClass.GetUserName(strSendUserCode);
                }
                catch
                {
                    strSendUserName = "";
                }

                try
                {
                    try
                    {
                        //������Ϣ��΢���û���������ҵ��
                        string strUserID = TakeTopCore.WXHelper.GetUserWeXinUserIDByUserCode(strReceivedUserCode);
                        if (strUserID != "")
                        {
                            SendWeChatQYMsg(strUserID, strMsg + ",---" + LanguageHandle.GetWord("ZZZXinXiLaiZhi").ToString().Trim() + strSendUserCode + " " + strSendUserName);
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        //������Ϣ��΢���û������ڹ��ں�
                        string strOpenID = TakeTopCore.WXHelper.GetUserWeXinOpenIDByUserCode(strReceivedUserCode);
                        if (strOpenID != "")
                        {
                            SendWeChatGZMsg(strOpenID, strMsg + ",---" + LanguageHandle.GetWord("ZZZXinXiLaiZhi").ToString().Trim() + strSendUserCode + " " + strSendUserName);
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        //���Ͷ��ŵ��ֻ���
                        SendMsgToPhoneBySP(strReceivedUserCode, strMsg, strSendUserCode);
                    }
                    catch
                    {
                    }

                    ////��Э����ʽ����Ϣ
                    //try
                    //{
                    //    SendCollaboration(strSendUserCode, strReceivedUserCode, strSubject, strMsg);
                    //}
                    //catch
                    //{
                    //}

                    //try
                    //{
                    //    //������Ϣ��IOS APP
                    //    NotificationHelper.ApplePush(strReceivedUserCode, strMsg, 1);

                    //}
                    //catch
                    //{
                    //}

                    //try
                    //{
                    //    //�������͵�ANDROID APP
                    //    PushMsgByJPUSH.SendSMSByJPUSH(strReceivedUserCode, strMsg);
                    //}
                    //catch
                    //{
                    //}

                    //try
                    //{
                    //    //����RTX��Ϣ
                    //    SendRTXMsg(strReceivedUserCode, strMsg);
                    //}
                    //catch
                    //{
                    //}
                }
                catch
                {
                }
            }).Start();
        }
        catch
        {
        }

        return true;
    }

    //�������ؼ���������Ϣ��������Ա
    public void SendMsgToOtherMemberForWorkflow(string strUserCode, string strWFTemName, string strWFTemStepID, string strWLID, string strWFName, string strStepID, string strStepName, string strNoticeKeyWord, string strOperation)
    {
        try
        {
            string strHQL;
            string strOtherUserCode, strMustNotice, strMsgTitle, strMsg;

            strHQL = "Select * From T_WorkFlowTemplateStepBusinessMember Where TemName = " + "'" + strWFTemName + "'" + " and StepID = " + strWFTemStepID;
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WorkFlowTemplateStepBusinessMember");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                strOtherUserCode = ds.Tables[0].Rows[i]["UserCode"].ToString().Trim();
                strMustNotice = ds.Tables[0].Rows[i][strNoticeKeyWord].ToString().Trim();

                strMsgTitle = strWLID + strWFName + " " + strOperation;
                strMsg = strUserCode + ShareClass.GetUserName(strUserCode) + " " + LanguageHandle.GetWord("YiJing").ToString().Trim() + " " + strOperation + LanguageHandle.GetWord("GongZuoLiuShenQing").ToString().Trim() + " : " + strWLID + strWFName + "," + LanguageHandle.GetWord("BuZhou").ToString().Trim() + ": " + strStepID + strStepName;

                if (strMustNotice == "YES")
                {
                    SendMSM(strMsgTitle, strOtherUserCode, strMsg, strUserCode);
                }
            }
        }
        catch
        {
        }
    }

    //��Э����ʽ������Ϣ
    public static void SendCollaboration(string strUserCode, string strChatterCode, string strCollaborationName, string strContent)
    {
        string strCoID;

        strCoID = AddCollaborationForAutoSend(strUserCode, strChatterCode, strCollaborationName, strContent);

        CollaborationLogBLL collaborationLogBLL = new CollaborationLogBLL();
        CollaborationLog collaborationLog = new CollaborationLog();

        collaborationLog.CoID = int.Parse(strCoID);
        collaborationLog.LogContent = strContent;
        collaborationLog.UserCode = strUserCode;
        collaborationLog.UserName = ShareClass.GetUserName(strUserCode);
        collaborationLog.CreateTime = DateTime.Now;

        try
        {
            collaborationLogBLL.AddCollaborationLog(collaborationLog);
        }
        catch
        {
        }
    }

    //����Э������Э����ʽ������Ϣ
    public static string AddCollaborationForAutoSend(string strUserCode, string strChatterCode, string strCollaborationName, string strContent)
    {
        string strHQL;
        string strCOID;

        CollaborationBLL collaborationBLL = new CollaborationBLL();
        Collaboration collaboration = new Collaboration();

        collaboration.CollaborationName = strCollaborationName;
        collaboration.Comment = strContent;
        collaboration.CreateTime = DateTime.Now;
        collaboration.CreatorCode = strUserCode;
        collaboration.CreatorName = ShareClass.GetUserName(strUserCode);
        collaboration.RelatedType = "OTHER";
        collaboration.RelatedID = 0;
        collaboration.Status = "InProgress";

        try
        {
            collaborationBLL.AddCollaboration(collaboration);

            strCOID = ShareClass.GetMyCreatedMaxColloaborationID(strUserCode);

            strHQL = "insert T_CollaborationMember(CoID,UserCode,UserName,CreateTime,LastLoginTime) ";
            strHQL += " Values(" + strCOID + "," + "'" + strChatterCode + "'" + "," + "'" + ShareClass.GetUserName(strChatterCode) + "'" + ",now(),now())";
            ShareClass.RunSqlCommand(strHQL);

            return strCOID;
        }
        catch
        {
            return "0";
        }
    }

    //---���ʹ����͵���Ϣ��ÿ��20��������ICP----------------------------------------
    public void SendUNSentSMSBySP()
    {
        string strHQL1, strHQL2;
        string strID = "0", strMobilePhone, strUserCode, strUserRTXCode, strMsg, strSPInterface, strSPName;
        int i;

        DataSet ds;

        //������Ϣ��APP
        strHQL1 = "Select ID,UserCode,Msg,Mobile,UserRTXCode From sms_send Where ltrim(rtrim(UserCode)) <> '' and State = 0 ";
        strHQL1 += " order By ID DESC limit 20";
        ds = ShareClass.GetDataSetFromSql(strHQL1, "T_Sms_Send");
        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            strID = ds.Tables[0].Rows[i][0].ToString();

            strHQL2 = "Update Sms_Send Set State = 1,SendYorn = 1,Sendtime = now() Where ID = " + strID;
            ShareClass.RunSqlCommandForNOOperateLog(strHQL2);

            try
            {
                strUserCode = ds.Tables[0].Rows[i][1].ToString().Trim();
            }
            catch
            {
                strUserCode = "";
            }
            strMsg = ds.Tables[0].Rows[i][2].ToString().Trim();

            strMsg = strMsg.Replace("Notice:Hello, you have a new task assigned to you. The task content is: Self-Review.", LanguageHandle.GetWord("ZZTZNHNYSPGZNRZSQJSDLGLPTJXCLZXXLZXTGLY").ToString().Trim());  
            strMsg = strMsg.Replace("Notice: Hello, you have a task, content:", LanguageHandle.GetWord("ZZTZNHNYSPGZNR").ToString().Trim());  
            strMsg = strMsg.Replace("Please log in to the management platform and open the Workflow Module to process it as soon as possible. This message is from the System Administrator!", LanguageHandle.GetWord("ZZQJSDLGLPTJXCLZXXLZXTGLY").ToString().Trim());  

            strMobilePhone = ds.Tables[0].Rows[i][3].ToString().Trim();
            strUserRTXCode = ds.Tables[0].Rows[i][4].ToString().Trim();

            //��APP������Ϣ
            if (strUserCode != "")
            {
                try
                {
                    new System.Threading.Thread(delegate ()
                    {
                        try
                        {
                            try
                            {
                                //������Ϣ��΢���û���������ҵ��
                                string strUserID = TakeTopCore.WXHelper.GetUserWeXinUserIDByUserCode(strUserCode);
                                if (strUserID != "")
                                {
                                    SendWeChatQYMsg(strUserID, strMsg);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                //������Ϣ��΢���û������ڹ��ں�
                                string strOpenID = TakeTopCore.WXHelper.GetUserWeXinOpenIDByUserCode(strUserCode);
                                if (strOpenID != "")
                                {
                                    SendWeChatGZMsg(strOpenID, strMsg);
                                }
                            }
                            catch
                            {
                            }

                            //���ֻ����Ͷ���
                            strSPInterface = GetSPInterface().SPInterface.Trim();
                            strSPName = GetSPInterface().SPName;
                            if (strSPInterface != "")
                            {
                                try
                                {
                                    SendSMSBySP(strSPName, strSPInterface, strMobilePhone, strMsg);
                                }
                                catch
                                {
                                }
                            }

                            ////��Э����ʽ����Ϣ
                            //try
                            //{
                            //    SendCollaboration("ADMIN", strUserCode, "Message", strMsg);
                            //}
                            //catch
                            //{
                            //}

                            //try
                            //{
                            //    //�������͵�ANDROID APP
                            //    PushMsgByJPUSH.SendSMSByJPUSH(strUserCode, strMsg);
                            //}
                            //catch
                            //{
                            //}

                            //try
                            //{
                            //    //������Ϣ��IOS APP
                            //    NotificationHelper.ApplePush(strUserCode, strMsg, 1);
                            //}
                            //catch
                            //{
                            //}

                            ////��RTX������Ϣ
                            //if (GetRTXServerCount() > 0)
                            //{
                            //    try
                            //    {
                            //        strMsg = ds.Tables[0].Rows[i][2].ToString().Trim();
                            //        strUserRTXCode = ds.Tables[0].Rows[i][4].ToString().Trim();
                            //        //������Ϣ��RTX
                            //        if (SendRTXMsgByUserRTXCode(strUserRTXCode, strMsg))
                            //        {
                            //            strHQL2 = "Update Sms_Send Set RTXState = 1,Sendtime = now() Where RTXState = 0 and ID = " + strID;
                            //            ShareClass.RunSqlCommandForNOOperateLog(strHQL2);
                            //        }
                            //    }
                            //    catch
                            //    {
                            //    }
                            //}
                        }
                        catch
                        {
                        }
                    }).Start();
                }
                catch
                {
                }
            }
        }
    }

    //-------��ȫ��ϵͳ�û�������Ϣ----------------------------------------------------------------
    public void StartNotificationToSystemActiveUser()
    {
        string strHQL;
        string strActiveUserCode, strSuperDepartString;
        string strLangCode;
        int i = 0, k = 0, count = 0, total = 0;

        Msg msg = new Msg();

        strHQL = "from SystemActiveUser as systemActiveUser where 1=1";
        SystemActiveUserBLL systemActiveUserBLL = new SystemActiveUserBLL();
        IList lst_User = systemActiveUserBLL.GetAllSystemActiveUsers(strHQL);
        if (lst_User.Count == 0 & lst_User != null)
        {
            return;
        }

        if (HttpContext.Current.Session["SystemVersionType"] == null)
        {
            //���ע�����Ƿ�Ϸ�
            string strServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
            try
            {
                TakeTopLicense license = new TakeTopLicense();
              
                HttpContext.Current.Session["SystemVersionType"] = license.GetVerType(strServerName);
                HttpContext.Current.Session["ForbitModule"] = license.GetForbitModuleString(strServerName);
            }
            catch
            {

                HttpContext.Current.Session["SystemVersionType"] = "GROUP";
                HttpContext.Current.Session["ForbitModule"] = "NONE";
            }
            if (System.Configuration.ConfigurationManager.AppSettings["ProductType"].IndexOf("SAAS") > -1)
            {
                HttpContext.Current.Session["SystemVersionType"] = "SAAS";
            }
        }
        else
        {
            HttpContext.Current.Session["SystemVersionType"] = "GROUP";
        }

        string strSystemVersionType = HttpContext.Current.Session["SystemVersionType"].ToString();

        SystemActiveUser activeUser = new SystemActiveUser();
        FunInforDialBox funInforDialBox = new FunInforDialBox();

        for (i = 0; i < lst_User.Count; i++)
        {
            total = 0;

            activeUser = (SystemActiveUser)lst_User[i];

            strActiveUserCode = activeUser.UserCode.Trim();

            if (ShareClass.CheckUserIsExist(strActiveUserCode))
            {
                //strSuperDepartString = ShareClass.InitialDepartmentStringByAuthoritySuperUser(strActiveUserCode, strSystemVersionType);
                strLangCode = ShareClass.GetUserLangCode(strActiveUserCode);

                StringBuilder newInforNameList = new StringBuilder();//��ʾԤ������
                FunInforDialBoxBLL funInforDialBoxBLL = new FunInforDialBoxBLL();
                string strHQL_Fun = "From FunInforDialBox as funInforDialBoxBySystem Where funInforDialBoxBySystem.Status = 'Enabled'";
                //strHQL_Fun += " and len(Ltrim(Rtrim(funInforDialBoxBySystem.LinkAddress))) > 0 ";
                strHQL_Fun += " and funInforDialBoxBySystem.LangCode = " + "'" + strLangCode + "'";
                strHQL_Fun += " Order By funInforDialBoxBySystem.ID Desc ";
                IList lst_Fun = funInforDialBoxBLL.GetAllFunInforDialBoxs(strHQL_Fun);
                if (lst_Fun.Count > 0 & lst_Fun != null)
                {
                    for (k = 0; k < lst_Fun.Count; k++)
                    {
                        funInforDialBox = (FunInforDialBox)lst_Fun[k];

                        try
                        {
                            strHQL = funInforDialBox.SQLCode.Trim();
                            strHQL = strHQL.Replace("[TAKETOPUSERCODE]", strActiveUserCode);
                            //strHQL = strHQL.Replace("[TAKETOPSUPERDEPARTSTRING]", strSuperDepartString);
                            DataSet ds = ShareClass.GetDataSetFromSqlNOOperateLog(strHQL, "FunInforDialBoxList");

                            count = 0;
                            count = ds.Tables[0].Rows.Count;

                            if (count > 0)
                            {
                                total += count;
                                if (newInforNameList.Length > 0)
                                {
                                    newInforNameList.AppendFormat("\n{0}({1})", funInforDialBox.InforName, count);
                                }
                                else
                                {
                                    newInforNameList.AppendFormat("{0}({1})", funInforDialBox.InforName, count);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }

                    string strNofiInfo = LanguageHandle.GetWord("ZZDNCLDGZ").ToString().Trim() + newInforNameList.ToString() + "��" + LanguageHandle.GetWord("ZZFZXTGLY").ToString().Trim();

                    if (total > 0)
                    {
                        try
                        {
                            //������Ϣ��΢���û���������ҵ��
                            string strUserID = TakeTopCore.WXHelper.GetUserWeXinUserIDByUserCode(strActiveUserCode);
                            if (strUserID != "")
                            {
                                SendWeChatQYMsg(strUserID, strNofiInfo);
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            //������Ϣ��΢���û������ڹ��ں�
                            string strOpenID = TakeTopCore.WXHelper.GetUserWeXinOpenIDByUserCode(strActiveUserCode);
                            if (strOpenID != "")
                            {
                                SendWeChatGZMsg(strOpenID, strNofiInfo);
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            //���Ͷ���
                            msg.SendMsgToPhoneBySP(strActiveUserCode, strNofiInfo, "ADMIN");
                        }
                        catch
                        {
                        }

                        ////��Э����ʽ����Ϣ
                        //try
                        //{
                        //    SendCollaboration("ADMIN", strUserCode, "Message", strNofiInfo);
                        //}
                        //catch
                        //{
                        //}

                        //try
                        //{
                        //    //�������͵�ANDROID APP
                        //    PushMsgByJPUSH.SendSMSByJPUSH(strUserCode, strNofiInfo);
                        //}
                        //catch (Exception ex)
                        //{
                        //}

                        //try
                        //{
                        //    //��IOS APP������Ϣ
                        //    NotificationHelper.ApplePush(strUserCode, strNofiInfo, total);
                        //}
                        //catch (Exception ex)
                        //{
                        //}

                        //try
                        //{
                        //    //��Ϣ�����ʼ�
                        //    msg.SendMail(strUserCode, LanguageHandle.GetWord("ZZDNCLDGZ").ToString().Trim(), strNofiInfo, "ADMIN");
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                    }
                }
            }
        }
    }


    //΢�Ź��ںţ�������Ϣ����Ӧ΢���û�
    public void SendWeChatGZMsg(string OPENID, string text)
    {
        //string accessToken = ShareClass.GetUserWeChatAccessToken();

        string accessToken = TakeTopCore.WXHelper.GetAccessToken();

        if (accessToken != "")
        {
            var data = "{ \"touser\":\"" + OPENID + "\", \"msgtype\":\"text\", \"text\": { \"content\":\" " + text + "\" }}";
            var json = HttpHelper.Post("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + accessToken, data);
            HttpContext.Current.Response.Write(json);
            HttpContext.Current.Response.End();
        }
    }

    ///΢����ҵ�ţ�������Ϣ����Ӧ΢���û�
    public string SendWeChatQYMsg(string strUserOID, string strMessage)
    {
        string strAgentID = "1";

        string strHQL;
        strHQL = "Select AgentID From T_WeiXinQYStand";
        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_WeiXinQYStand");
        if (ds.Tables[0].Rows.Count > 0)
        {
            strAgentID = ds.Tables[0].Rows[0][0].ToString().Trim();
        }
        else
        {
            strAgentID = "1";
        }

        string responeJsonStr = "{";
        responeJsonStr += "\"touser\": \"" + strUserOID + "\",";
        responeJsonStr += "\"msgtype\": \"text\",";
        responeJsonStr += "\"agentid\": \"" + strAgentID + "\",";
        responeJsonStr += "\"text\": {";
        responeJsonStr += "  \"content\": \"" + strMessage + "\"";
        responeJsonStr += "},";
        responeJsonStr += "\"safe\":\"0\"";
        responeJsonStr += "}";

        string accessToken = TakeTopCore.WXHelper.GetAccessToken();
        string postUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", accessToken);

        return PostWebRequest(postUrl, responeJsonStr, Encoding.UTF8);
    }

    /// <summary>
    /// ΢����ҵ�� Post���ݽӿ�
    /// </summary>
    /// <param name="postUrl">�ӿڵ�ַ</param>
    /// <param name="paramData">�ύjson����</param>
    /// <param name="dataEncode">���뷽ʽ</param>
    /// <returns></returns>
    public string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
    {
        string ret = string.Empty;
        try
        {
            byte[] byteArray = dataEncode.GetBytes(paramData); //ת��
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";

            webReq.ContentLength = byteArray.Length;
            Stream newStream = webReq.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);//д�����
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            ret = sr.ReadToEnd();
            sr.Close();
            response.Close();
            newStream.Close();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return ret;
    }

    //--------BEGIN����ICP��GET��ʽ���͵ý������ͬ��ICP�в�ͬ�ķ��ͷ������ķ������ڴ���Ϳɣ�[TAKETOPNUMBER]��[TAKETOPMESSAGE]������յ��ֻ��������Ϣ���ݣ�-----------------------------------------------------
    //������Ϣ���ֻ��ϣ�����ICP
    public bool SendMsgToPhoneBySP(string strReceivedUserCode, string strMsg, string strSendUserCode)
    {
        string strHQL;
        IList lst;
        string strMobilePhone, strUserRTXCode, strSPInterface, strSPName;

        //���Ͷ���

        try
        {
            strSPName = GetSPInterface().SPName;
            strSPInterface = GetSPInterface().SPInterface;
        }
        catch
        {
            return false;
        }

        if (strSPInterface != "" & strSPInterface != null)
        {
            strMsg = strMsg + "---" + LanguageHandle.GetWord("ZZZXinXiLaiZhi").ToString().Trim() + "��" + strSendUserCode + " " + ShareClass.GetUserName(strSendUserCode);

            ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
            strHQL = "from ProjectMember as projectMemberBySystem where projectMemberBySystem.UserCode = " + "'" + strReceivedUserCode + "'";
            lst = projectMemberBLL.GetAllProjectMembers(strHQL);
            ProjectMember projectMember = (ProjectMember)lst[0];

            strMobilePhone = projectMember.MobilePhone.Trim();
            strUserRTXCode = projectMember.UserRTXCode.Trim();

            if (strMobilePhone == null)
            {
                return false;
            }
            if (strMobilePhone.Trim() == "")
            {
                return false;
            }

            try
            {
                if (SendSMSBySP(strSPName, strSPInterface, strMobilePhone, strMsg))
                {
                    strHQL = "Insert Into Sms_Send(UserCode,UserRTXCode,Mobile,Msg,State,SendYorn,SendTime) Values(" + "'" + strReceivedUserCode + "'" + "," + "'" + strUserRTXCode + "'" + "," + "'" + strMobilePhone + "'" + "," + "'" + strMsg + "'" + ",1,1,now())";
                    ShareClass.RunSqlCommandForNOOperateLog(strHQL);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

   

    /// <summary>
    /// �����ֻ����뷢�Ͷ��� By Liu Jianping 2013-11-21
    /// </summary>
    /// <param name="strReceivedUserCode"></param>
    /// <param name="strMsg"></param>
    /// <param name="strSendUserCode"></param>
    /// <returns></returns>
    public bool SendPhoneMSMBySP(string strMobilePhone, string strMsg, string strSendUserCode)
    {
        string strSPInterface, strSPName;

        if (strMobilePhone == null)
        {
            return false;
        }
        if (strMobilePhone.Trim() == "")
        {
            return false;
        }

        try
        {
            strSPName = GetSPInterface().SPName;
            strSPInterface = GetSPInterface().SPInterface;
        }
        catch
        {
            return false;
        }

        if (strSPInterface != "" & strSPInterface != null)
        {
            strMsg = strMsg + "---" + LanguageHandle.GetWord("ZZZXinXiLaiZhi").ToString().Trim() + "��" + strSendUserCode + " " + ShareClass.GetUserName(strSendUserCode);

            try
            {
                if (SendSMSBySP(strSPName, strSPInterface, strMobilePhone, strMsg))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }//end

    //����ICP�ӿڷ��Ͷ���
    private bool SendSMSBySP(string strSPName, string strSPInterfaceURL, string strMobilePhone, string strMessage)
    {
        HttpWebRequest hwRequest;
        HttpWebResponse hwResponse;

        if (strMobilePhone == null)
        {
            return false;
        }
        if (strMobilePhone.Trim() == "")
        {
            return false;
        }

        Random rd = new Random();
        int AuthCodeNumber = rd.Next(100000, 1000000);
        string strAuthCode = AuthCodeNumber.ToString();

        try
        {
            if (strSPName == "������")  
            {
                strMessage = System.Web.HttpUtility.UrlEncode(strMessage, Encoding.UTF8);
                strSPInterfaceURL += "&PhoneNumber=" + strMobilePhone + "&Message=" + strMessage;

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strSPInterfaceURL);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";

                try
                {
                    using (WebResponse wr = req.GetResponse())
                    {
                        //������Խ��յ���ҳ�����ݽ��д���
                    }
                }
                catch (System.Exception err)
                {
                    // WriteErrLog(err.ToString());
                    return false;
                }
            }
        }
        catch
        {
        }

        if (strSPName == "��������")  
        {
            strMessage = System.Web.HttpUtility.UrlEncode(strMessage, Encoding.UTF8);

            strSPInterfaceURL = strSPInterfaceURL.Replace("[TAKETOPNUMBER]", strMobilePhone);
            strSPInterfaceURL = strSPInterfaceURL.Replace("[TAKETOPMESSAGE]", strMessage);

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(strSPInterfaceURL);

                hwRequest.Timeout = 6000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
            }
            catch (System.Exception err)
            {
                // WriteErrLog(err.ToString());
                return false;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();

                ////LiuJianping 2013-09-17
                //if (strResult.Trim() != "100")
                //{
                //    return false;
                //}
                ////end
            }
            catch (System.Exception err)
            {
                //WriteErrLog(err.ToString());
                return false;
            }
        }


        try
        {
            if (strSPName == "��������")  
            {
                strMessage = System.Web.HttpUtility.UrlEncode(strMessage, Encoding.UTF8);

                strSPInterfaceURL = strSPInterfaceURL.Replace("[TAKETOPNUMBER]", strMobilePhone);
                strSPInterfaceURL = strSPInterfaceURL.Replace("[TAKETOPMESSAGE]", strMessage);

                string strResult = string.Empty;
                try
                {
                    hwRequest = (System.Net.HttpWebRequest)WebRequest.Create(strSPInterfaceURL);
                    //     hwRequest.Timeout = 5000;
                    hwRequest.Timeout = 5000;
                    hwRequest.Method = "GET";
                    hwRequest.ContentType = "application/x-www-form-urlencoded";
                }
                catch (System.Exception err)
                {
                    // WriteErrLog(err.ToString());
                    return false;
                }

                //get response
                try
                {
                    hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                    StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                    strResult = srReader.ReadToEnd();
                    srReader.Close();
                    hwResponse.Close();

                    //LiuJianping 2013-09-17
                    if (strResult.Trim() != "100")
                    {
                        return false;
                    }
                    //end
                }
                catch (System.Exception err)
                {
                    //WriteErrLog(err.ToString());
                    return false;
                }
            }

            if (strSPName == "�˳۶���")  
            {
                string url, SMSparameters, targeturl, res;
                string[] Para;

                strMessage += "//�ظ�TD�˶����Ӷ����ӡ�";
                strMessage = System.Web.HttpUtility.UrlEncode(strMessage, System.Text.Encoding.GetEncoding("gb2312"));

                //url = "http://www.lanz.net.cn/LANZGateway/DirectSendSMSs.asp";
                //string SMSparameters = "UserID=" + Userid + "&Account=" + Account + "&Password=" + PassWord + "&SMSType=1" + "&Content=" + content + "&Phones=" + TextBox1.Text + "&SendDate=&Sendtime=";

                Para = strSPInterfaceURL.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries);
                url = Para[0];
                SMSparameters = Para[1];
                //SMSparameters += "&SendDate=" + DateTime.Now.ToShortDateString() + "&Sendtime=" + DateTime.Now.ToLocalTime();

                SMSparameters = SMSparameters.Replace("[TAKETOPNUMBER]", strMobilePhone);
                SMSparameters = SMSparameters.Replace("[TAKETOPMESSAGE]", strMessage);

                targeturl = url.Trim().ToString();
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                //hr.CookieContainer = Form1.cookieContainerSMS;
                res = httpPost.HttpSMSPost(hr, url, SMSparameters);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(res);

                string errorNum = xml.SelectSingleNode("/LANZ_ROOT/ErrorNum").InnerText;  //��ȡ�Ƿ��½�ɹ�

                if (errorNum == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (strSPName == "���ȶ���")  
            {
                strMessage += "//�ظ�TD�˶������Ͷ�����";

                strSPInterfaceURL = strSPInterfaceURL.Replace("[TAKETOPNUMBER]", strMobilePhone);
                strSPInterfaceURL = strSPInterfaceURL.Replace("[TAKETOPMESSAGE]", strMessage);

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strSPInterfaceURL);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";

                try
                {
                    using (WebResponse wr = req.GetResponse())
                    {
                        //������Խ��յ���ҳ�����ݽ��д���
                    }
                }
                catch
                {
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    //---END �ֻ��������ͷ�ʽ------------------------------------------------------------------------------------------------------------

    //--RTX��������BEGIN---------------------------------------------------------------------------------------------------------
    //������Ϣ��RTX�����û����룩
    public bool SendRTXMsg(string strReceivedUserCode, string strMsg)
    {
        string strHQL;
        IList lst;

        string strServerIP;
        string strServerPort;
        string strWebSite;
        string strUserRTXCode;

        RTXSAPILib.RTXSAPIRootObj RootObj;  //����һ��������

        if (GetRTXServerCount() > 0)
        {
            strHQL = "From ProjectMember as projectMemberBySystem Where projectMemberBySystem.UserCode = " + "'" + strReceivedUserCode + "'";
            ProjectMemberBLL projectMemberBLl = new ProjectMemberBLL();
            lst = projectMemberBLl.GetAllProjectMembers(strHQL);
            ProjectMember projectMember = (ProjectMember)lst[0];

            strUserRTXCode = projectMember.UserRTXCode.Trim();

            if (strUserRTXCode == "")
            {
                return false;
            }

            strHQL = "From RTXConfig as rtxConfigBySystem";
            RTXConfigBLL rtxConfigBLL = new RTXConfigBLL();
            lst = rtxConfigBLL.GetAllRTXConfigs(strHQL);

            RTXConfig rtxConfig = new RTXConfig();

            for (int i = 0; i < lst.Count; i++)
            {
                rtxConfig = (RTXConfig)lst[i];

                strServerIP = rtxConfig.ServerIP.Trim();
                strServerPort = rtxConfig.ServerPort.ToString();
                strWebSite = rtxConfig.WebSite.Trim();

                if (strServerIP == "" | strServerPort == "")
                {
                    return false;
                }

                RootObj = new RTXSAPIRootObj();     //����������
                RootObj.ServerIP = strServerIP; //���÷�����IP
                RootObj.ServerPort = Convert.ToInt16(strServerPort); //���÷������˿�

                strMsg = strMsg + " " + strWebSite;

                //������Ϣ
                try
                {
                    RootObj.SendNotify(strUserRTXCode, "ϵͳ��Ϣ", 60000000, strMsg); //��ȡ�汾��Ϣ  
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        return true;
    }

    //������Ϣ��RTX(���û�RTX���룩
    public bool SendRTXMsgByUserRTXCode(string strUserRTXCode, string strMsg)
    {
        string strHQL;
        IList lst;

        string strServerIP;
        string strServerPort;
        string strWebSite;

        RTXSAPILib.RTXSAPIRootObj RootObj;  //����һ��������

        if (strUserRTXCode == "")
        {
            return false;
        }

        strHQL = "From RTXConfig as rtxConfigBySystem";
        RTXConfigBLL rtxConfigBLL = new RTXConfigBLL();
        lst = rtxConfigBLL.GetAllRTXConfigs(strHQL);

        RTXConfig rtxConfig = new RTXConfig();

        for (int i = 0; i < lst.Count; i++)
        {
            rtxConfig = (RTXConfig)lst[i];

            strServerIP = rtxConfig.ServerIP.Trim();
            strServerPort = rtxConfig.ServerPort.ToString();
            strWebSite = rtxConfig.WebSite.Trim();

            if (strServerIP == "" | strServerPort == "")
            {
                return false;
            }

            RootObj = new RTXSAPIRootObj();     //����������
            RootObj.ServerIP = strServerIP; //���÷�����IP
            RootObj.ServerPort = Convert.ToInt16(strServerPort); //���÷������˿�

            strMsg = strMsg + " " + strWebSite;

            //������Ϣ
            try
            {
                RootObj.SendNotify(strUserRTXCode, LanguageHandle.GetWord("SystemMessage").ToString().Trim(), 60000000, strMsg); //��ȡ�汾��Ϣ
                return true;
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    public bool RTXSendIM(string Sender, string pwd, string RECVUsers, string IMMsg) //���ͼ�ʱ��Ϣ
    {
        //����:���ͼ�ʱ��Ϣ
        //����˵��:
        //Sender:������
        //pwd:����������
        //RECVUsers:������,����м���,���
        //IMMsg:���͵���Ϣ����
        try
        {
            RTXObjectClass RTXObj = new RTXObjectClass();
            RTXCollectionClass RTXParams = new RTXCollectionClass();

            RTXObj.Name = "SYSTOOLS";
            RTXParams.Add("SENDER", Sender);
            RTXParams.Add("RECVUSERS", RECVUsers);
            RTXParams.Add("IMMsg", IMMsg);
            //string pass=Page.Session["UserPwd"].ToString();
            //string pass="123";
            RTXParams.Add("SDKPASSWORD", pwd);
            Object iStatus = new Object();
            //iStatus =RTXobj.Call2( &H2002, RTXParams);

            iStatus = RTXObj.Call2(enumCommand_.PRO_SYS_SENDIM, RTXParams);
            string result = iStatus.ToString();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RTXSendIMts(string Sender, string pwd, string RECVUsers, string IMMsg)//�㲥��ʽ������Ϣ
    {
        //����:�㲥��ʽ������Ϣ
        //����˵��:
        //Sender:������
        //pwd:����������
        //RECVUsers:������,����м���,���
        //IMMsg:���͵���Ϣ����
        try
        {
            RTXObjectClass RTXObj = new RTXObjectClass();
            RTXCollectionClass RTXParams = new RTXCollectionClass();
            RTXObj.Name = "EXTTOOLS";
            RTXParams.Add("USERNAME", RECVUsers);
            RTXParams.Add("SDKPASSWORD", pwd);
            RTXParams.Add("MSGINFO", IMMsg);
            Object iStatus = new Object();
            iStatus = RTXObj.Call2(RTXServerApi.enumCommand_.PRO_EXT_NOTIFY, RTXParams);

            return true;
        }
        catch
        {
            return false;
        }
    }

    //----RTX��������END-----------------------------------------------------------------------------------------------------

    //�����ʼ��������ʼ����ݣ������ַ��
    public bool SendMailByEmail(string strEmail, string strSubject, string strBody, string strSendUserCode)
    {
        int nContain = 0;
        string strHQL;
        IList lst;

        string strTo;
        int nMailID;

        Folder folder = new Folder();

        strTo = strEmail;

        if (strTo == "")
            return false;

        strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strSendUserCode + "'";
        MailProfileBLL mailProfileBLL = new MailProfileBLL();
        lst = mailProfileBLL.GetAllMailProfiles(strHQL);

        if (lst.Count == 0)
            return false;

        MailProfile mailProfile = (MailProfile)lst[0];

        if (mailProfile.Email == null)
            return false;

        ///��ӷ����˵�ַ
        string strFrom = mailProfile.Email.Trim();

        if (strFrom == "")
            return false;

        MailMessage mailMsg = new MailMessage();

        mailMsg.From = new MailAddress(strFrom, mailProfile.UserName.Trim());
        mailMsg.To.Add(strTo);
        nContain += strTo.Length;

        //mailMsg.CC.Add(strTo);
        //nContain += strTo.Length;

        ///����ʼ�����
        mailMsg.Subject = strSubject;
        nContain += strSubject.Length;

        ///����ʼ�����
        mailMsg.Body = strBody;
        mailMsg.BodyEncoding = Encoding.UTF8;
        mailMsg.IsBodyHtml = true;

        nContain += strBody.Length;

        //nContain += 100;

        try
        {
            IMail mail = new Mail();
            SmtpClient smtpClient = new SmtpClient(mailProfile.SmtpServerIP, mailProfile.SmtpServerPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mailProfile.AliasName.Trim(), mailProfile.Password.Trim());
            /*ָ����δ���������ʼ�*/
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                //�����ʼ�
                smtpClient.Send(mailMsg);

                return true;
            }
            catch
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    //���Ͳ���������δ�����ʼ�
    public void SendUNSentNoAttachmentMail()
    {
        string strHQL;
        IList lst;

        string strUserCode;

        Folder folder = new Folder();
        strHQL = "From Mails as mails where mails.FolderID in (Select folders.FolderID From Folders as folders Where folders.KeyWord = 'Waiting')";
        strHQL += " and mails.MailID not in (Select attachments.MailID From Attachments as attachments )";
        MailsBLL mailsBLL = new MailsBLL();
        lst = mailsBLL.GetAllMailss(strHQL);

        Mails mails = new Mails();

        if (lst.Count > 0)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                mails = (Mails)lst[i];

                strUserCode = mails.UserCode.Trim();

                SendMailByNoAttachmentMails(strUserCode, mails);
            }
        }
    }

    //���Ͳ��������ʼ������ڵ��ʼ��������ⷢ�ͣ�
    protected void SendMailByNoAttachmentMails(string strUserCode, Mails mails)
    {
        int nContain = 0;
        string strHQL;
        IList lst;

        string from;

        int intMailID;
        int intSendFoldID;
        int intWaitingFoldID;

        Folder folder = new Folder();
        intSendFoldID = folder.GetFolderID("Send", strUserCode);
        intWaitingFoldID = folder.GetFolderID("Waiting", strUserCode);

        strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strUserCode + "'";
        MailProfileBLL mailProfileBLL = new MailProfileBLL();
        lst = mailProfileBLL.GetAllMailProfiles(strHQL);
        MailProfile mailProfile = (MailProfile)lst[0];

        ///��ӷ����˵�ַ
        from = mailProfile.Email.Trim();

        intMailID = mails.MailID;

        MailMessage mailMsg = new MailMessage();

        mailMsg.From = new MailAddress(from);
        mailMsg.To.Add(mails.ToAddress.Trim());
        nContain += mails.ToAddress.Trim().Length;

        if (mails.CCAddress.Trim() != "")
        {
            mailMsg.CC.Add(mails.CCAddress.Trim());
            nContain += mails.CCAddress.Trim().Length;
        }
        else
        {
            //mailMsg.CC.Add(mails.ToAddress.Trim());
        }

        ///����ʼ�����
        mailMsg.Subject = mails.Title.Trim();
        nContain += mails.Title.Trim().Length;

        ///����ʼ�����
        mailMsg.Body = mails.Body.Trim();
        mailMsg.BodyEncoding = Encoding.UTF8;
        mailMsg.IsBodyHtml = true;

        nContain += mails.Body.Trim().Length;

        try
        {
            Mail mail = new Mail();

            SmtpClient smtpClient = new SmtpClient(mailProfile.SmtpServerIP, mailProfile.SmtpServerPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mailProfile.AliasName.Trim(), mailProfile.Password.Trim());
            /*ָ����δ���������ʼ�*/
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                smtpClient.Send(mailMsg);

                mail.MoveMail(intMailID, intSendFoldID);
            }
            catch
            {
            }
        }
        catch (Exception ex)
        {
        }
    }

    //�����ʼ��������ʼ����ݣ������ڲ���Ա���໥���ͣ�
    public bool SendMail(string strUserCode, string strSubject, string strBody, string strSendUserCode)
    {
        int nContain = 0;
        string strHQL;
        IList lst;

        string strTo;
        int nMailID;

        string strEnableSMTPSSL;

        Folder folder = new Folder();

        ProjectMemberBLL projectMemberBLL = new ProjectMemberBLL();
        strHQL = "from ProjectMember as projectMember where projectMember.UserCode = " + "'" + strUserCode + "'";
        lst = projectMemberBLL.GetAllProjectMembers(strHQL);

        if (lst.Count == 0)
            return false;

        ProjectMember projectMember = (ProjectMember)lst[0];

        if (projectMember.EMail == null)
            return false;

        strTo = projectMember.EMail.Trim();

        if (strTo == "")
            return false;

        strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strSendUserCode + "'";
        MailProfileBLL mailProfileBLL = new MailProfileBLL();
        lst = mailProfileBLL.GetAllMailProfiles(strHQL);

        if (lst.Count == 0)
            return false;

        MailProfile mailProfile = (MailProfile)lst[0];

        strEnableSMTPSSL = mailProfile.EnableSMTPSSL.Trim();

        if (mailProfile.Email == null)
            return false;

        ///��ӷ����˵�ַ
        string strFrom = mailProfile.Email.Trim();

        if (strFrom == "")
            return false;

        MailMessage mailMsg = new MailMessage();

        mailMsg.From = new MailAddress(strFrom, mailProfile.UserName.Trim());
        mailMsg.To.Add(strTo);
        nContain += strTo.Length;

        //mailMsg.CC.Add(strTo);
        //nContain += strTo.Length;

        ///����ʼ�����
        mailMsg.Subject = strSubject;
        mailMsg.Subject = mailMsg.Subject.Replace("����֪ͨ", LanguageHandle.GetWord("ZZShengPiTongZhi").ToString().Trim());  

        nContain += strSubject.Length;

    
        ///����ʼ�����
        mailMsg.Body = strBody;
        mailMsg.Body = mailMsg.Body.Replace("Notice:Hello, you have a new task assigned to you. The task content is: Self-Review.", LanguageHandle.GetWord("ZZTZNHNYSPGZNRZSQJSDLGLPTJXCLZXXLZXTGLY").ToString().Trim());  
        mailMsg.Body = mailMsg.Body.Replace("Notice: Hello, you have a task, content:", LanguageHandle.GetWord("ZZTZNHNYSPGZNR").ToString().Trim());  
        mailMsg.Body = mailMsg.Body.Replace("Please log in to the management platform and open the Workflow Module to process it as soon as possible. This message is from the System Administrator!", LanguageHandle.GetWord("ZZQJSDLGLPTJXCLZXXLZXTGLY").ToString().Trim());  

        mailMsg.BodyEncoding = Encoding.UTF8;
        mailMsg.IsBodyHtml = true;

        nContain += strBody.Length;

        //nContain += 100;

        try
        {
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            ////�û���
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mailProfile.AliasName.Trim());
            ////����
            //mailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", mailProfile.Password.Trim());

            IMail mail = new Mail();
            SmtpClient smtpClient = new SmtpClient(mailProfile.SmtpServerIP, mailProfile.SmtpServerPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(mailProfile.AliasName.Trim(), mailProfile.Password.Trim());
            /*ָ����δ���������ʼ�*/
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (strEnableSMTPSSL == "YES")
            {
                //����SSL
                smtpClient.EnableSsl = true;
            }

            try
            {
                //�����ʼ�
                smtpClient.Send(mailMsg);

                nMailID = mail.SaveAsMail(mailMsg.Subject, mailMsg.Body, strFrom,
                    strTo, strTo, 1,
                    nContain, mailMsg.Attachments.Count > 0 ? 1 : 0, 1, folder.GetFolderID("Send", strUserCode), strSendUserCode);

                return true;
            }
            catch
            {
                nMailID = mail.SaveAsMail(mailMsg.Subject, mailMsg.Body, strFrom,
                    strTo, strTo, 1,
                    nContain, mailMsg.Attachments.Count > 0 ? 1 : 0, 1, folder.GetFolderID("Waiting", strUserCode), strSendUserCode);

                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    //�Զ���ȡ�ʼ�����(�����û����룩
    public void ReceiveMailByUserCode(string strUserCode, string strDocSavePath)
    {
        string strHQL;
        IList lst;

        string strPop3Server, strAliasName, strPassword;
        int intPort, intMailCount;

        strHQL = "from MailProfile as mailProfile where mailProfile.UserCode = " + "'" + strUserCode + "'";
        MailProfileBLL mailProfileBLL = new MailProfileBLL();
        lst = mailProfileBLL.GetAllMailProfiles(strHQL);

        if (lst.Count > 0)
        {
            MailProfile mailProfile = (MailProfile)lst[0];

            strAliasName = mailProfile.AliasName.Trim();
            strPassword = mailProfile.Password.Trim();
            strPop3Server = mailProfile.Pop3ServerIP.Trim();
            intPort = mailProfile.Pop3ServerPort;

            POP3_Client _POP3Client = new POP3_Client();
            if (_POP3Client.IsConnected == false)
            {
                _POP3Client.Connect(strPop3Server, intPort);
                _POP3Client.Authenticate(strAliasName, strPassword, true);

                intMailCount = _POP3Client.Messages.Count;
                _POP3Client.Disconnect();
                _POP3Client.Dispose();

                for (int i = 1; i <= intMailCount; i++)
                {
                    //strAliasName:��¼�� strPassword:����
                    ShareClass.ReceiveMail(strPop3Server, strUserCode, strAliasName, strPassword, intPort, strDocSavePath);
                }
            }
        }
    }

    //��POP3��������ɾ���ʼ�
    public void DeleteMailFromPOP3Server(string strMailPOP3Server, int intPOP3Port, string strMailAccount, string strMailServerPassword)
    {
        using (POP3_Client c = new POP3_Client())
        {
            //����POP3������
            c.Connect(strMailPOP3Server, intPOP3Port);

            //��֤�û����
            c.Authenticate(strMailAccount, strMailServerPassword, false);

            if (c.Messages.Count > 0)
            {
                foreach (POP3_ClientMessage mail in c.Messages)
                {
                    mail.MarkForDeletion(); //ɾ���ʼ�
                }
            }

            c.Disconnect();
            c.Dispose();
        }
    }

    public int GetRTXServerCount()
    {
        string strHQL;
        IList lst;

        strHQL = "From RTXConfig as rtxConfigBySystem";
        RTXConfigBLL rtxConfigBLL = new RTXConfigBLL();
        lst = rtxConfigBLL.GetAllRTXConfigs(strHQL);

        return lst.Count;
    }

    public SMSInterface GetSPInterface()
    {
        string strHQL;
        IList lst;

        strHQL = "From SMSInterface as smsInterfaceBySystem Where smsInterfaceBySystem.Status ='InProgress'";
        SMSInterfaceBLL smsInterfaceBLL = new SMSInterfaceBLL();
        lst = smsInterfaceBLL.GetAllSMSInterfaces(strHQL);
        SMSInterface smsInterface = new SMSInterface();

        if (lst.Count > 0)
        {
            smsInterface = (SMSInterface)lst[0];
            return smsInterface;
        }
        else
        {
            return null;
        }
    }

    public string CreateRandomCode(int codeCount)
    {
        string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] allCharArray = allChar.Split(',');
        string randomCode = "";
        int temp = -1;

        Random rand = new Random();
        for (int i = 0; i < codeCount; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            }
            int t = rand.Next(36);
            if (temp != -1 && temp == t)
            {
                return CreateRandomCode(codeCount);
            }
            temp = t;
            randomCode += allCharArray[t];
        }
        return randomCode;
    }

    public string StringToUnicode(string srcText)
    {
        string dst = "";
        char[] src = srcText.ToCharArray();
        for (int i = 0; i < src.Length; i++)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
            string str = @"\u" + bytes[1].ToString("X2") + bytes[0].ToString("X2");
            dst += str;
        }
        return dst;
    }
}
