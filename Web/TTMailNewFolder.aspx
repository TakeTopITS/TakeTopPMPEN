<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTMailNewFolder.aspx.cs" Inherits="NewFolder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>新建文件夹</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="js/jquery-1.7.2.min.js"></script><script type="text/javascript" src="js/allAHandler.js"></script><script type="text/javascript" language="javascript">$(function () {if (top.location != self.location) { } else { CloseWebPage(); }});</script></head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" class="bian">
        <tr>
            <td height="31" class="page_topbj">
                            <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="29"><%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%></td>
                                                <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XinJianWenJianJia%>"></asp:Label>
                                                </td>
                                                <td width="5"><%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
        </tr>
        <tr>
            <td valign="top" style="padding:5px 5px 5px 5px;">
                <table align="left" cellspacing="0" cellpadding="3" width="98%" class="formBgStyle">
                
                    <tr >
                        <td style="width: 5%"  class="formItemBgStyleForAlignLeft">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>:
                        </td>
                        <td class="formItemBgStyleForAlignLeft">
                            <asp:TextBox ID="Name" runat="server" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfN" runat="server" ControlToValidate="Name" CssClass="GbText"
                                Display="Dynamic" ErrorMessage="名称不能为空！"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr >
                        <td   class="formItemBgStyleForAlignLeft">
                        </td>
                        <td class="formItemBgStyleForAlignLeft">
                            <font face="宋体">&nbsp;</font><asp:Button ID="NewBtn" runat="server" Text="<%$ Resources:lang,QueDing%>" 
                                CssClass="inpu" OnClick="NewBtn_Click" /><font face="宋体">&nbsp;</font><asp:Button
                                    ID="ReturnBtn" runat="server" Text="<%$ Resources:lang,FanHui%>"  CssClass="inpu" OnClick="ReturnBtn_Click"
                                    CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
