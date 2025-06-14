<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TakeTopSiteCustomerQuestionSubmitForWebSite_TakeTopSoft.aspx.cs" Inherits="TakeTopSiteCustomerQuestionSubmitForWebSite_TakeTopSoft" %>

<!DOCTYPE html>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../../Logo/website/css/yijianfankui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../../js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../../../js/allAHandlerForWebSite.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }



        });

    </script>

</head>
<body>
    <center>
        <form id="form1" runat="server">
            <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="#AboveDiv" style="position: relative; top: 30px; text-align: center;">
                        <img src="../../../Images/khfw.gif" alt="" style="width: 80%;" />
                        <table style="width: 100%;">
                            <tr>
                                <td align="center" style="padding-top: 15px;">
                                    <table border="0" cellpadding="0" cellspacing="3" class="zi" width="780px">
                                        <tr>
                                            <td colspan="2" align="center" class="formItemBgStyleForAlignLeft"></td>

                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,Company%>"></asp:Label>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_Company" runat="server" Style="width: 250px; height: 30px;"></asp:TextBox>
                                                &nbsp;<font color="#FF0000">*</font> </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label>
                                            </td>
                                            <td width="82%" class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_ContactPerson" runat="server" Style="width: 250px; height: 30px;"></asp:TextBox>
                                                &nbsp;<font color="#FF0000">*</font> &nbsp;&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LianXiDianHua%>"></asp:Label>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_PhoneNumber" runat="server" Style="width: 250px; height: 30px;"></asp:TextBox>
                                                &nbsp;<font color="#FF0000">*</font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">Email
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_EMail" runat="server" Style="width: 250px; height: 30px;"></asp:TextBox>
                                                &nbsp;<font color="#FF0000">*</font>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,WenTiLeiBie%>"></asp:Label>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:DropDownList ID="DL_CustomerQuestionType" runat="server" Height="35px" CssClass="DDList">

                                                    <asp:ListItem Text="<%$ Resources:lang,GouMaiZhiXunWenTi%>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:lang,CaoZuoShiYongWenTi%>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:lang,ShiShiFuWuWenTi%>"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ Resources:lang,QiTaiWenTi%>"></asp:ListItem>

                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">
                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,WenTiMiaoShu%>"></asp:Label>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_Question" runat="server" Rows="5" Style="width: 250px;" TextMode="MultiLine"></asp:TextBox>
                                                &nbsp;<font color="#FF0000">*</font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,YanZhengMa%>"></asp:Label></td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="TB_CheckCode" runat="server" Style="width: 100px; height: 30px;"></asp:TextBox>

                                                        </td>
                                                        <td>
                                                            <asp:Image ID="IM_CheckCode" runat="server" src="../../../TTCheckCode.aspx" Style="width: 100px;" Height="35px" />
                                                            &nbsp;<font color="#FF0000">*</font>
                                                        </td>
                                                    </tr>
                                                </table>


                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft"></td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Button ID="BT_Summit" runat="server" Style="width: 100px; height: 35px;" OnClick="BT_Summit_Click" Text="<%$ Resources:lang,TiJiao%>" />
                                                <asp:Label ID="LB_Message" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr style="display: none;">
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,LianXiDiZhi%>"></asp:Label>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_Address" runat="server"></asp:TextBox>
                                                &nbsp;<font color="#FF0000">*</font>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,YouZhengBianMa%>"></asp:Label>
                                            </td>
                                            <td class="formItemBgStyleForAlignLeft">
                                                <asp:TextBox ID="TB_PostCode" runat="server" Style="width: 100px;"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute; left: 50%; top: 50%;">
                <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <img src="../../../Images/Processing.gif" alt="Loading,please wait..." />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '../../../' + '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = '../../../' + 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
