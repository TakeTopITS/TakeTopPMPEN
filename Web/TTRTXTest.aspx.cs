using System; using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using RTXSAPILib;
using RTXClient;

public partial class TTRTXTest : System.Web.UI.Page
{
    RTXSAPILib.RTXSAPIRootObj RootObj;  //����һ��������
    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        try
        {
            RootObj = new RTXSAPIRootObj();     //����������
            RootObj.ServerIP = txtSvrIP.Text; //���÷�����IP
            RootObj.ServerPort = Convert.ToInt16(txtSvrPort.Text); //���÷������˿�
        }
        catch
        {
            Response.Write(LanguageHandle.GetWord("ChuangJianRTXFuWuShiBaiQingJia").ToString().Trim());
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {//������Ϣ
        try
        {
            RootObj.SendNotify(txtReceivers.Text, txtMsgTitle.Text, Convert.ToInt32(txtTime.Text), txtMsgContent.Text); //��ȡ�汾��Ϣ

            txtResult.Text = LanguageHandle.GetWord("FaSongChengGong").ToString().Trim();
        }
        catch
        {
            txtResult.Text = LanguageHandle.GetWord("FaSongShiBai").ToString().Trim();
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //��ѯ״̬
        try
        {
            txtResult.Text = RootObj.QueryUserState(txtUserName.Text); //��ȡ�û�״̬
        }
        catch 
        {
            txtResult.Text = LanguageHandle.GetWord("HuoQuYongHuZhuangTaiShiBai").ToString().Trim();
        }
    }
   
    protected void Button4_Click(object sender, EventArgs e)
    {
        try
        {
            txtResult.Text = RootObj.GetVersion(); //��ȡ�汾��Ϣ
            
        }
        catch 
        {
            txtResult.Text = LanguageHandle.GetWord("HuoQuBanBenXinXiShiBai").ToString().Trim();
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        try
        {
            txtResult.Text = RootObj.GetUserRightList(txtUserName.Text); //��ȡ�û�Ȩ���б�
        }
        catch 
        {
            txtResult.Text = LanguageHandle.GetWord("HuoQuYongHuQuanXianXinXiShiBai").ToString().Trim();
        }
    }
}
