<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZObjectBigDetail.aspx.cs" Inherits="TTWZObjectBigDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <div id="AboveDiv">
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,DaLeiDaiMa%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 0px 5px 5px 5px;" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="top" style="padding-top: 5px;">
                                                <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td  style="width: 100%; padding: 5px 5px 5px 5px;" class="formItemBgStyleForAlignLeft" valign="top">

                                                            <table class="formBgStyle" width="80%">
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,DaLeiDaiMa%>"></asp:Label>��</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_DLCode" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,DaLeiMingCheng%>"></asp:Label>��</td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TXT_DLName" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,DaLeiShuoMing%>"></asp:Label>��</td>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <asp:TextBox ID="TXT_DLDesc" runat="server" TextMode="MultiLine" Height="50px" Width="555px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="4">
                                                                        <asp:Button ID="BT_Save" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="BT_Save_Click" />&nbsp;
                                                                        <input type="button" value="����" id="BT_Return" class="inpu" onclick="window.location.href = 'TTWZObjectBigList.aspx'" />
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:HiddenField ID="HF_DLCode" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="BT_Save" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
