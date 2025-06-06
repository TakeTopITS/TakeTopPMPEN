<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSiteCustomerRegisterFromWebSite_TakeTopSoft.aspx.cs" Inherits="TakeTopSiteCustomerRegisterFromWebSite_TakeTopSoft" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />

<%@ Import Namespace="System.Globalization" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>‘⁄œﬂ ‘”√</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <link id="flxappCss" href="css/flxapp.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="Logo/website/js/jquery-1.3.1.js"></script>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>

    <style type="text/css">
        body {
            /*margin-top: 5px;*/
            /*background-image: url(Images/login_bj.jpg);*/
            background-repeat: repeat-x;
            font: normal 130% Helvetica, Arial, sans-serif;
        }
    </style>


    <script type="text/javascript" language="javascript">

        $(function () {
        });


        function openMDIFrom(strMDIPageName) {

            window.open(strMDIPageName, '_blank');

            CloseLayer();

        }

    </script>

</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div style="font-size: 36pt; width: 100%; text-align: center;">
                        <img align="absmiddle" src="images/onlineTry.jpg" alt="onlineTry" />
                    </div>
                    <table style="width: 100%;">
                        <tr>
                            <td style="height: 40px; padding-top: 20px; text-align: center;">&nbsp; &nbsp; &nbsp; &nbsp;
                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,QXTXYXXXD%>"></asp:Label><font color="#FF0000">*</font>
                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,HMWBTXRHDJZCANXTJZJDNDSYYM%>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="padding-top: 15px;">
                                <table border="0" cellpadding="0" cellspacing="3" width="500px">
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ChanPing%>"></asp:Label>
                                        </td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="LB_Product" runat="server"></asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></td>
                                        <td class="formItemBgStyleForAlignLeft" w>
                                            <asp:TextBox ID="TB_Company" runat="server" Style="width: 90%; height: 25px; font-size: 16px;"></asp:TextBox>
                                            <font color="#FF0000">&nbsp;*</font>

                                        </td>

                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label></td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_ContactPerson" runat="server" Style="width: 90%; height: 25px; font-size: 16px;"></asp:TextBox>
                                                &nbsp;<font color="#FF0000">*</font> </td>

                                        </tr>
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,LianXiDianHua%>"></asp:Label></td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_PhoneNumber" runat="server" Style="width: 90%; height: 25px; font-size: 16px;"></asp:TextBox>
                                            <font color="#FF0000">&nbsp;*</font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft" valign="top">
                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ShiYongYuanYin%>"></asp:Label></td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:DropDownList ID="DL_TryProductResonType" Height="35px" DataValueField="Type" DataTextField="Type" Style="font-size: 16px;" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft" valign="top">
                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label></td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="TB_CheckCode" runat="server" Style="width: 100px; height: 35px; font-size: 16px;"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="IM_CheckCode" runat="server" src="TTCheckCode.aspx" Width="100px" Height="35px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formItemBgStyleForAlignLeft"></td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:Button ID="BT_Summit" runat="server" Width="100px" Height="35px" OnClick="BT_Summit_Click" Text="<%$ Resources:lang,TiJiao%>" />
                                            <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,Email%>"></asp:Label>
                                        </td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_EMail" runat="server" Style="width: 90%;"></asp:TextBox>
                                            &nbsp;<font color="#FF0000">*</font> </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,LianXiDiZhi%>"></asp:Label>
                                        </td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_Address" runat="server" Style="width: 90%;"></asp:TextBox>
                                            <font color="#FF0000">&nbsp;*</font> </td>
                                    </tr>

                                    <tr style="display: none;">
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,YouBian%>"></asp:Label>
                                        </td>
                                        <td class="formItemBgStyleForAlignLeft">
                                            <asp:TextBox ID="TB_PostCode" runat="server" Style="width: 90%; height: 25px;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center" class="formItemBgStyleForAlignCenter">
                                            <br />
                                            <a href="tencent://message/?uin=3166455252&amp;Site=&amp;Menu=yes">
                                                <a href="tencent://message/uin=3166455252&amp;Site=&amp;Menu=yes">
                                                    <img align="absmiddle" src="images/qq.png" />QQ </a>£¨
                                            </a>
                                            Tel£∫<a href="tel:02151085119" class="call">021-51085119"></a>

                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                    </table>
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
