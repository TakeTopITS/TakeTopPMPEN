<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSoftBuy_TakeTopSoftCloud.aspx.cs" Inherits="TakeTopSoftBuy_TakeTopSoftCloud" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />

<meta content="��ҵ�ơ���ҵ�����������������" name="keywords">
<meta content="��ҵ�ƣ��ṩ��ҵ��������������÷���" name="description">
<meta charset="utf-8" />
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>���߹���---̩���ض�</title>
    <link href="Logo/website/css/zuyong.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/website.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/goumai.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Logo/website/js/jquery-1.3.1.js"></script>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            if (top.location != self.location)
            {

            }
            else {

                window.location.href = 'https://www.taketopits.com';
            }


            aHandler();

            $('.bigbox').hover(function () {
                $(".pointer", this).stop().animate({ top: '190px' }, { queue: false, duration: 160 });
            }, function () {
                $(".pointer", this).stop().animate({ top: '278px' }, { queue: false, duration: 160 });
            });

        });
    </script>
    <style type="text/css">
        a {
            color: #333;
            text-decoration: none;
        }
    </style>
</head>
<body style="background-color: white;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <center>
                    <p>
                        &nbsp;
                    </p>
                    <div class="title">
                        ��ҵ��8�����չ���
                    </div>
                    <div class="title1">
                        ��ľ���ṹ���ɰ���ѡ�䣬�������
                    </div>
                    <div class="main">
                        <p style="text-align: center; padding-bottom: 30px; font-size: 28px;">
                            <br />
                            <span style="text-align: center; font-size: small;"><a href="TakeTopSoftModuleChart_TakeTopSoftCloud.html"
                                target="_blank">����ģ����۸�</a> </span>
                        </p>
                    </div>
                    <div class="box">
                        <div id="list">
                            <ul id="smalllist">
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="Logo/website/Images/201312231521547781.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">��Ŀ��</a></h3>
                                    <h3>
                                        <br />
                                        3000Ԫ��50�û�
                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">�ṩ��Ŀ�ƻ������ȡ����񡢷��á��ĵ������ܣ������ã�10����ѧ��</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="Logo/website/Images/201411328407512.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">ϵͳ������Ŀ����ƽ̨</a></h3>
                                    <h3>
                                        <br />
                                        10000Ԫ��
                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">ϵͳ������Ŀ����ƽ̨</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="Logo/website/Images/2013122395112630.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">�з���Ŀ����ƽ̨</a></h3>
                                    <h3>
                                        <br />
                                        10000Ԫ��
                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">��Ʒ�뼼���з�����</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="Logo/website/Images/201312181423536202.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">������Ŀ����ƽ̨</a></h3>
                                    <h3>
                                        <br />
                                        10000Ԫ��
                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">�������&ʩ����Ŀ����</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="Logo/website/Images/20131218142463453.png" /></a>
                                    </div>

                                    <h3>
                                        <a href="#">���ʵʩ��Ŀ����ƽ̨</a></h3>
                                    <h3>
                                        <br />
                                        10000Ԫ��
                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">���ϵͳʵʩ��Ŀ����</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="Logo/website/Images/201312231521462477.png" /></a>
                                    </div>
                                    <h3>
                                        <a href="#">��Ŀ��ERPƽ̨</a></h3>
                                    <h3>
                                        <br />
                                        10000Ԫ��
                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">��Ŀ���������</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="Logo/website/Images/201312181423378443.png" /></a>
                                    </div>

                                    <h3>
                                        <a href="#">������Ŀ����ƽ̨</a></h3>
                                    <h3>
                                        <br />
                                        10000Ԫ��
                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">������Ŀ����</a>
                                        </p>
                                    </div>
                                </li>
                                <li class="bigbox">
                                    <div class="img">
                                        <a href="#">
                                            <img src="Logo/website/Images/201312181423459253.png" /></a>
                                    </div>


                                    <h3>
                                        <a href="#">��Ŀ������ѯ</a></h3>
                                    <h3>
                                        <br />
                                        30000Ԫ��
                                    </h3>
                                    <p>
                                        &nbsp;
                                    </p>
                                    <div class="pointer">
                                        <p class="pointer_one">
                                            <a href="#">��Ŀ������ѯ����ѵ����֤</a>
                                        </p>
                                    </div>
                                </li>
                                <%--  <li class="bigbox">
                                    <div class="img"><a href="#"><img src="Logo/website/Images/201312181423378443.png" /></a></div>

                                    <h3><a href="#">CRM��OA</a></h3>

                                    <h3>
                                        <br />
                                        15000Ԫ��
                                    </h3>

                                    <p>&nbsp;</p>

                                    <div class="pointer">
                                        <p class="pointer_one"><a href="#">CRM:�ͻ�&middot;��ǰ�ۺ����&middot;�������</a></p>
                                        <p class="pointer_one"><a href="#">OA:����&middot;�ʲ�&middot;֪ʶ&middot;������</a></p>
                                    </div>
                                </li>
                               

                                 <li class="bigbox">
                                    <div class="img"><a href="#"><img src="Logo/website/Images/20131218142463453.png" /></a></div>

                                    <h3><a href="#">�ۺϹ���ƽ̨</a></h3>

                                    <h3>
                                        <br />
                                        100000Ԫ��
                                    </h3>

                                    <p>&nbsp;</p>

                                    <div class="pointer">
                                        <p class="pointer_one"><a href="#">������ҵ&middot;������ҵ&middot;ʯ����ҵ</a></p>
                                    </div>
                                </li>--%>
                            </ul>
                            <p>
                                &nbsp;
                            </p>
                            <table style="text-align: left; width: 880px;">
                                <tr>
                                    <td class="formItemBgStyle">
                                        <div style="top: 29px; width: 99%;">
                                            <table width="100%">
                                                <tr>
                                                    <td width="50%">
                                                        <p class="zuyong">
                                                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;<strong>����̩���ض��������</strong>��ʹ��ʱ�䲻�ޣ������װ���û��Ա��ķ������ϣ�����������д�������룬���ǽ����ṩ�ֳ�ʵʩָ���Ͷ��ƿ�������
                                                        </p>
                                                        <div id="id5">
                                                            <table class="ziti5" border="0" cellpadding="0" cellspacing="3" width="100%">
                                                                <tr>
                                                                    <td class="formItemBgStyle" width="100px">��Ʒ
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:DropDownList ID="DL_Type" runat="server" Style="height: 50px;">
                                                                            <asp:ListItem>��Ŀ��</asp:ListItem>
                                                                            <asp:ListItem>ϵͳ������Ŀ����ƽ̨</asp:ListItem>
                                                                            <asp:ListItem>�з���Ŀ����ƽ̨</asp:ListItem>
                                                                            <asp:ListItem>������Ŀ����ƽ̨</asp:ListItem>
                                                                            <asp:ListItem>������Ŀ����ƽ̨</asp:ListItem>
                                                                            <asp:ListItem>���ʵʩ��Ŀ����ƽ̨</asp:ListItem>
                                                                            <asp:ListItem>��Ŀ��ERPƽ̨</asp:ListItem>
                                                                            <asp:ListItem>��Ŀ�ۺϹ���ƽ̨</asp:ListItem>
                                                                            <asp:ListItem>--------------</asp:ListItem>
                                                                            <asp:ListItem>ЭͬOAƽ̨</asp:ListItem>
                                                                            <asp:ListItem>�ͻ���ϵ����ƽ̨</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyle">�汾
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList ID="DL_Version" runat="server" Style="height: 50px;">
                                                                                        <asp:ListItem>��׼��</asp:ListItem>
                                                                                        <asp:ListItem>��ҵ��</asp:ListItem>
                                                                                        <asp:ListItem>���Ű�</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>&nbsp;
                                                                                </td>
                                                                                <td style="vertical-align: middle;">
                                                                                    <a href="TakeTopSoftModuleChart_TakeTopSoftCloud.html" target="_blank">ģ����۸�</a>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyle">��˾
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:TextBox ID="TB_Company" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyle">��ϵ��
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:TextBox ID="TB_ContactPerson" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyle" style="padding-bottom: 25px;">�ֻ�
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:TextBox ID="TB_PhoneNumber" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                        <br />
                                                                        <span style="font-size: xx-small;">ע��Ҫ������Ҫ��Ϣ������ȷ��д��</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyle" style="padding-bottom: 25px;">Email
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:TextBox ID="TB_EMail" runat="server" Style="width: 350px; height: 30px;" onclick="checkEmailFormat('TB_EMail')"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                        <br />
                                                                        <span style="font-size: xx-small;">ע��Ҫ������Ҫ��Ϣ������ȷ��д��</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyle">�û���
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:TextBox ID="TB_UserNumber" runat="server" ForeColor="#000000" Style="width: 150px; height: 30px;"></asp:TextBox>
                                                                        <strong>��</strong> <font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="formItemBgStyle">��֤��
                                                                    </td>
                                                                    <td align="left" class="formItemBgStyle">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="TB_CheckCode" runat="server" ForeColor="#000000" Style="width: 150px; height: 40px;"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Image ID="IM_CheckCode" runat="server" src="TTCheckCode.aspx" Style="width: 150px; height: 40px;" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyle">&nbsp;
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:Button ID="BT_Summit" runat="server" OnClick="BT_Summit_Click" Style="width: 130px; height: 30px;"
                                                                            Text="�� ��" />
                                                                        <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyle">��ϵ��ַ
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:TextBox ID="TB_Address" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyle">�洢����
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <asp:DropDownList ID="DL_ServerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_ServerType_SelectedIndexChanged">
                                                                            <asp:ListItem>����</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="TB_StorageCapacity" runat="server" Style="width: 50px;" Text="10"></asp:TextBox><strong
                                                                            style="font-size: medium;">GB</strong> <font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                            <li>�ύ�ɹ��󣬿ͷ�����������ϵ�㣬��Ҳ����ֱ����ϵ���ܣ�<br />
                                                                &nbsp;&nbsp;<a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes"><img
                                                                    align="absmiddle" src="images/qq.png" />�ͷ�QQ</a>���绰��<a href="tel:02151085119"
                                                                        class="call">021-51085119</a></li>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <p>
                                &nbsp;
                            </p>
                            <p>
                                &nbsp;
                            </p>
                            <p>
                                &nbsp;
                            </p>
                        </div>
                    </div>
                    <div class="layui-layer layui-layer-iframe" id="popwindow" name="fixedDiv" style="z-index: 9999; width: 680px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label205" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">
                            <iframe id="IFrame_BuildSite" src="TakeTopSoftRent_BuildSite.aspx" style="width: 520px; height: 540px; border: none;"
                                runat="server"></iframe>
                        </div>
                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label206" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab"
                            href="javascript:;"></a></span>
                    </div>
                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;">
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="position: absolute; left: 50%; top: 50%;">
            <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <img src="Images/Processing.gif" alt="Loading,please wait..." />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>
</body>
<script type="text/javascript" language="javascript">    var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
