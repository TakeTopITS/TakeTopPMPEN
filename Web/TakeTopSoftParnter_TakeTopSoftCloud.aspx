<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSoftParnter_TakeTopSoftCloud.aspx.cs" Inherits="TakeTopSoftParnter_TakeTopSoftCloud" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />
<meta content="��ҵ�ơ���ҵ�����������������" name="keywords">
<meta content="��ҵ�ƣ��ṩ��ҵ��������������÷���" name="description">
<meta charset="utf-8" />


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>���߼���</title>
    <link href="Logo/website/css/media.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/qudaohezuo.css" rel="stylesheet" type="text/css" />
    <link href="Logo/website/css/zuyong.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        a {
            color: #333;
            text-decoration: none;
        }

        .auto-style1 {
            height: 28px;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {

            if (top.location != self.location) {

            }
            else {

                window.location.href = 'https://www.taketopits.com';
            }
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <center>
                    <table style="text-align: left;">
                        <tr>
                            <td class="formItemBgStyleForAlignLeft">
                                <div style="top: 29px; width: 980px;">
                                    <table width="100%">
                                        <tr>
                                            <td width="50%">

                                                <div class="qudaohezuo">
                                                    <div class="wenzi">
                                                        <p class="zuyong">
                                                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;��������ڵĵ�����������˾Ŀǰ��ӵ�еĿͻ���Դ�ǳ��ʺ��ƹ�̩���ض�����������������Ϊ���ǵĺ�����飬��Ӯ�����ǵľ����ں����������������ȫ����֧�֣�����ȫ�����ѵ������֧�֣��������ǻ��ṩ�����ڵ��������ۻ��ᣬ��֧������ҵ������ķ�չ��
                                                        </p>

                                                        <%-- <p style="text-indent: 20px;"><strong>�����ֳ����ߣ�</strong></p>

                                                        <p style="text-indent: 20px;">1����չ�Ĵ���ÿ��һ�������˵�������û���Ȩ�Ѵ��������ҷ��ɹ����10%��ɣ���Ȩ��һ��ռ��ͬ���70%���ң�</p>

                                                        <p style="text-indent: 20px;">2����������ȥ��չ�ĵ�һ�����ÿ��һ�������԰��˵�������û���Ȩ�Ѵ��������ҷ��ɹ����5%��ɣ�</p>

                                                        <p style="text-indent: 20px;"><strong>���۷ֳ����ߣ�</strong></p>

                                                        <p style="text-indent: 20px;">1���ṩ�̻������ǣ���Э���������ۣ������������ɵ������԰��˵�������û���Ȩ�ѵ�20%��ɣ�</p>

                                                        <p style="text-indent: 20px;">
                                                            2��ֱ�����ۣ�����Э�����ֳɰ������۴���ֳ�Э�顿ִ�У�<br />                                                          
                                                        </p>--%>


                                                        <div id="id5" style="padding-left: 20px;">
                                                            <h3>&nbsp;&nbsp;&nbsp;
                                                                
                                                                  <asp:Label ID="Label1" runat="server"  Text="<%$ Resources:lang,RuYouYiXiangQingZaiXiaMianTianXieNiDeLianJiXinXi%>"></asp:Label>
                                                            </h3>
                                                            <table class="ziti5" border="0" cellpadding="0" cellspacing="3" width="100%">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label2" runat="server"  Text="<%$ Resources:lang,GongSi%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Company" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label3" runat="server"  Text="<%$ Resources:lang,LianJiRen%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_ContactPerson" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> &nbsp;&nbsp;</td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label4" runat="server"  Text="<%$ Resources:lang,DianHua%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_PhoneNumber" runat="server" ForeColor="#000000" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>



                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label5" runat="server"  Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
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
                                                                    <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Button ID="BT_Summit" runat="server" OnClick="BT_Summit_Click" Style="width: 130px; height: 30px;"  Text="<%$ Resources:lang,DiJiao%>" />
                                                                        <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                                                        <br />
                                                                    </td>
                                                                </tr>

                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyleForAlignLeft" width="100px">��Ʒ</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DL_Type" runat="server" Style="height: 50px;">
                                                                            <asp:ListItem>---</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="auto-style1">�汾</td>
                                                                    <td class="auto-style1">
                                                                        <asp:DropDownList ID="DL_Version" runat="server" Style="height: 50px;">
                                                                            <asp:ListItem>--------------</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td class="formItemBgStyleForAlignLeft">�û���</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_UserNumber" runat="server" ForeColor="#000000" Style="width: 150px; height: 30px;" Text="0"></asp:TextBox>
                                                                        <strong>��</strong> <font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft">Email </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_EMail" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font> </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft">��ϵ��ַ</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Address" runat="server" Style="width: 350px; height: 30px;"></asp:TextBox>
                                                                        &nbsp;<font color="#FF0000">*</font>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">

                                                                    <td class="formItemBgStyleForAlignLeft">�洢����</td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:DropDownList ID="DL_ServerType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_ServerType_SelectedIndexChanged">
                                                                            <asp:ListItem>����</asp:ListItem>

                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="TB_StorageCapacity" runat="server" Style="width: 50px;" Text="10"></asp:TextBox><strong style="font-size: medium;">GB</strong>
                                                                        <font color="#FF0000">*</font> </td>
                                                                </tr>
                                                            </table>

                                                            <br />

                                                            <li>
                                                                <asp:Label ID="Label6" runat="server"  Text="<%$ Resources:lang,DiJiaoChengGongHouKeFuJiangHuiLiKeLianJiNiNiYeKeYiZhiJieLianJiKeFu%>"></asp:Label>��
                                                              <br />
                                                                &nbsp;&nbsp;<a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes"><img align="absmiddle" src="images/qq.png" />QQ</a>��Tel��<a href="tel:02151085119" class="call">021-51085119</a><br />
                                                            </li>

                                                        </div>

                                                        <p>&nbsp</p>

                                                        <p style="margin: 0px; padding: 0px; color: rgb(0, 0, 0); text-transform: none; text-indent: 20px; letter-spacing: normal; word-spacing: 0px; white-space: normal; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px;"><strong>��������������ϵ</strong></p>

                                                        <div class="news-title" style="color: rgb(0, 0, 0); text-transform: none; text-indent: 0px; letter-spacing: normal; word-spacing: 0px; white-space: normal; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px;">&nbsp;&nbsp;</div>

                                                        <div class="news-title" style="color: rgb(0, 0, 0); text-transform: none; text-indent: 20px; letter-spacing: normal; word-spacing: 0px; white-space: normal; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px;">
                                                            <p style="margin: 0px; padding: 0px;">��Ʒ��Ԫ����������Ի��Ĵ���С�Ͳ�����ҵ��������������ͼ�ֵ������ϵ,����������������ϵ�������ص���Ϣ��������̬����&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong>&nbsp;&nbsp;��Ʒ�������</strong></p>

                                                            <p style="margin: 0px; padding: 0px;">��ָ��ǩ���Ʒ����Э�飬ͨ��������Ȩ��������ϵ�в�Ʒʵ��ӯ���Ļ�顣</p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong>&nbsp;&nbsp;��ֵ�������</strong></p>

                                                            <p style="margin: 0px; padding: 0px;">��ָ��ǩ�������ֵ����Э�鲢ͨ�����з�������������������ϵ�в�Ʒ��ʵ��ӯ���Ļ�顣</p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong>&nbsp;&nbsp;�̻��������</strong></p>

                                                            <p style="margin: 0px; padding: 0px;">��ָ��ǩ���̻�����Э�飬ƾ������ͻ���Դ���ƣ�Ϊ�ṩ��Ʒ�����̻����߰���������ɹ�ϵӪ�����̵ķ��˵�λ���߸��ˡ�</p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong>&nbsp;&nbsp;��Ȩ������</strong></p>

                                                            <p style="margin: 0px; padding: 0px;">��ָ���ʵʩ/������ϵ���ʸ���֤�����߱���ǿ�Ĺ����������ʵʩ�����������Ļ�顣</p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong>&nbsp;&nbsp;��Ȩ��ѵ���</strong></p>

                                                            <p style="margin: 0px; padding: 0px;">��ָ�ܽ��ϵ�в�Ʒ����ѵ������к���������Լ��������������������֯�ͻ�����</p>

                                                            <p style="margin: 0px; padding: 0px;">&nbsp;</p>

                                                            <p style="margin: 0px; padding: 0px;"><strong>&nbsp;&nbsp;��ҵ�������</strong></p>

                                                            <p style="margin: 0px; padding: 0px;">��ָ�ܹ�ͨ��ƽ̨��ɼ���ƽ̨�������γ��뻥������ҵģ���Ʒ�Ļ�顣����ҵ��Ʒ��Լ���ĺ���ģʽ�������������ۣ�����������ȡ����</p>
                                                        </div>
                                                    </div>
                                                </div>


                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
