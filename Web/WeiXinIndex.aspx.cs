using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;


public partial class WeiXinIndex : System.Web.UI.Page
{
    ////��΢�Ź����˺ź�̨��Token���ñ���һ�£����ִ�Сд��
    //private readonly string Token = "TakeTopMISToken";

    protected void Page_Load(object sender, EventArgs e)
    {
        string strToken;

        WeiXinStand weiXinStand = TakeTopCore.WXHelper.GetWeiXinStand();
        if (weiXinStand != null)
        {
            //strToken = weiXinStand.TokenValue.Trim();

            strToken = TakeTopCore.WXHelper.GetAccessToken();

            Auth(strToken);
        }
    }

    /// <summary>
    /// ����΢�ŷ�������֤��Ϣ
    /// </summary>
    private void Auth(string strToken)
    {
        string signature = Request["signature"];
        string timestamp = Request["timestamp"];
        string nonce = Request["nonce"];
        string echostr = Request["echostr"];

        if (Request.HttpMethod == "GET")
        {
            //get method - ����΢�ź�̨��дURL��֤ʱ����
            if (CheckSignature.Check(signature, timestamp, nonce, strToken))
            {
                WriteContent(echostr); //��������ַ������ʾ��֤ͨ��
            }
            else
            {
                WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, strToken) + "��" +
                            LanguageHandle.GetWord("RuGuoNiZaiLiuLanQiZhongKanDaoZ").ToString().Trim());
            }
            Response.End();
        }
    }

    private void WriteContent(string str)
    {
        Response.Output.Write(str);
    }
}
