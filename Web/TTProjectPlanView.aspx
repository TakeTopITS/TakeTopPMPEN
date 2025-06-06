<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectPlanView.aspx.cs"
    Inherits="TTProjectPlanView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="js/jquery-1.7.2.min.js"></script><script type="text/javascript" src="js/allAHandler.js"></script><script type="text/javascript" language="javascript">$(function () {if (top.location != self.location) { } else { CloseWebPage(); }});</script></head>
<body>
    <center>
        <form id="form1" runat="server">

            <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                <tr>
                    <td height="31" class="page_topbj">
                        <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="29">
                                                <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%></td>
                                            <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XMJHXXXX%>"></asp:Label>
                                            </td>
                                            <td width="5">
                                                <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="98%">
                            <tr>
                                <td colspan="6" style="text-align: center; height: 18px;">
                                    <br />
                                    <asp:DataList ID="DataList1" runat="server" Width="98%" CellPadding="0" ForeColor="#333333">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <ItemTemplate>
                                            <table cellpadding="0" cellspacing="5" style="width: 100%" class="bian">
                                                <tr>
                                                    <td style="width: 10%; text-align: right;">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label>:
                                                    </td>
                                                    <td style="width: 20%; text-align: left;">
                                                        <%#DataBinder .Eval (Container .DataItem ,"ID") %>
                                                    </td>
                                                    <td style="width: 10%; text-align: right;">
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>:
                                                    </td>
                                                    <td style="text-align: left; width: 20%;">
                                                        <%#DataBinder .Eval (Container .DataItem ,"Type") %>
                                                    </td>
                                                    <td style="text-align: right; width: 10%;">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,YuSuan%>"></asp:Label>:
                                                    </td>
                                                    <td style="text-align: left; width: 20%;">
                                                        <%#DataBinder .Eval (Container .DataItem ,"Budget") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%; text-align: right;">
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JiHuaNeiRong%>"></asp:Label>:
                                                    </td>
                                                    <td colspan="5" style="height: 21px; text-align: left;">
                                                        <%#DataBinder .Eval (Container .DataItem ,"Name") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%; text-align: right;">
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ZiYuan%>"></asp:Label>:
                                                    </td>
                                                    <td colspan="5" style="text-align: left">
                                                        <%#DataBinder .Eval (Container .DataItem ,"Resource") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%; text-align: right;">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>:
                                                    </td>
                                                    <td style="height: 20px; text-align: left;">
                                                        <%#DataBinder .Eval (Container .DataItem ,"Start_Date","{0:yyyy/MM/dd}") %>
                                                    </td>
                                                    <td style="height: 20px; text-align: right;">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>:
                                                    </td>
                                                    <td style="height: 20px; text-align: left;" colspan="3">
                                                        <%#DataBinder .Eval (Container .DataItem ,"End_Date","{0:yyyy/MM/dd}") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%; text-align: right;">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>:
                                                    </td>
                                                    <td colspan="3" style="text-align: left">
                                                        <%#DataBinder .Eval (Container .DataItem ,"Status") %>
                                                    </td>
                                                    <td colspan="2" style="text-align: center">
                                                        <span style="color: #0000ff;"></td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                        
                                        <ItemStyle CssClass="itemStyle" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    </asp:DataList>
                                    <asp:Label ID="LB_UserCode" runat="server" Font-Bold="False" Font-Size="9pt" Font-Underline="False"
                                         Visible="False"></asp:Label>&nbsp;
                                <asp:Label ID="LB_UserName" runat="server" Font-Bold="False" Font-Italic="False"
                                    Font-Size="9pt" Font-Underline="False"  Width="81px" Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
