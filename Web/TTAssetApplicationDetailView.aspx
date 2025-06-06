<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAssetApplicationDetailView.aspx.cs"
    Inherits="TTAssetApplicationDetailView" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        #AboveDiv {
            min-width: 900px;
            width: expression (document.body.clientWidth <= 900? "900px" : "auto" ));
        }
    </style>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }

          

        });
    </script>

</head>
<body>
    <center>
        <form id="form1" runat="server">
            <div id="AboveDiv">
                <table width="100%" cellpadding="4" cellspacing="4">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                <tr>
                                    <td width="7">
                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                </td>
                                                <td width="20%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ZiChanMing%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                </td>
                                                <td width="20%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>IP</strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ChangJia%>"></asp:Label></strong>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="6" align="right">
                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                </tr>
                            </table>
                            <asp:DataGrid runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                Height="30px" Width="100%" ID="DataGrid2">

                                <Columns>
                                    <asp:BoundColumn DataField="ID" HeaderText="Number">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AssetName" HeaderText="资产名">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="IP" HeaderText="IP">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                </Columns>
                                <ItemStyle CssClass="itemStyle"></ItemStyle>
                                <PagerStyle Horizontalalign="center"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
