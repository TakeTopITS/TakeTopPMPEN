<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAPPQRCodeForLocalSAAS.aspx.cs" Inherits="TTAPPQRCodeForLocalSAAS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" style="padding-top: 40px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" Text="<%$ Resources:lang,APPURLMiaoShu%>"></asp:Label>
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Font-Size="X-Large" Text="<%$ Resources:lang,APPURLShouChang%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-top: 10px;">
                        <asp:Image ID="IMG_APPQRCode" runat="server" />

                        <br />

                        <asp:Label ID="LB_APPURL" runat="server" Font-Size="X-Large"></asp:Label>
                        <br />
                        <br />

                    </td>
                </tr>
                <tr id="TD_SAAS" runat="server">
                    <td align="left" style="padding-top: 10px;">

                        <p style="font-size: larger; font-weight: 400;"><b>̩���ض����SAAS�汾΢�Ŷ�APPʹ�÷��� </b></p>

                        <br />
                        1��	���������̩���ض���վ�����õ�̩���ض��������ô���������΢��ɨ������Ķ�ά���ע��̩���ض�  ΢�Ź��ں�
                         <br />
                        <br />
                        <img src="ImagesSkin/TakeTopWXGZHQRCode.png" style="width: 150px; height: 150px;" />
                        <br />
                        <br />
                        2��	������ں��ϵģ�����ƽ̨�������APP��¼����
                        <br />
                        <br />

                        3��	�ڵ�¼�����һ��������Ĺ�˾��ƣ���ϵͳ����Ա�����ڶ���������ĵ�¼�ʺţ��������������룬����ͼ��
                        <br />
                       
                        <br />
                        <img src="ImagesSkin/TakeTopAPPLoginWXGZHForSAAS.jpg" />
                        <br />


                        ע��������Ϣ�ᱣ���ڻ�����ڶ��ε�¼ʱ���Զ���䣬�����ظ����룡
                        <br />
                        <br />

                        4��	���  ��¼ ��ť��ϵͳ���Զ���¼���APP�������Ժ���ƽ̨���͸������Ϣ������ʾ��������ں��
                        <br />
                        <br />
                        <img src="ImagesSkin/TakeTopWXGZHForSAAS.jpg" />
                        <br />

                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
