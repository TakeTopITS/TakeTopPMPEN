<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectBudgetChangeLog.aspx.cs" Inherits="TTProjectBudgetChangeLog" %>


<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>

    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else {
                CloseWebPage();
            }
        });

    </script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                        <tr>
                            <td height="31">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="95%" height="108" align="left" style="font-size: 20pt;">
                                            <br />

                                            <br />
                                            <br />
                                            <asp:Label ID="Label1" runat="server" Text="Ԥ������¼"></asp:Label>
                                        </td>
                                        <td align="left" valign="bottom">
                                            <a href="javascript:window.print()">
                                                <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" /></a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding: 5px 5px 5px 5px; vertical-align: top; border-right: solid 1px #D8D8D8;">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                <tr>
                                                    <td width="7">
                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                    </td>
                                                    <td>
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="10%" align="left">
                                                                    <strong>ID</strong>
                                                                </td>
                                                                <td width="15%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_dgAccount" runat="server" Text="ԭ��Ŀ"></asp:Label></strong>
                                                                </td>
                                                                <td width="15%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="LB_dgAmount" runat="server" Text="ԭ���"></asp:Label></strong>
                                                                </td>
                                                                <td width="20%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label2" runat="server" Text="ԭ����"></asp:Label></strong>
                                                                </td>
                                                                <td width="15%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label3" runat="server" Text="�Ǽ���"></asp:Label></strong>
                                                                </td>
                                                               
                                                                <td width="15%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label8" runat="server" Text="�����"></asp:Label></strong>
                                                                </td>
                                                                <td align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label6" runat="server" Text="���ʱ��"></asp:Label></strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="6" align="right">
                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" Width="100%"
                                                ShowHeader="false" CellPadding="4" GridLines="None">
                                                <Columns>
                                                    <asp:BoundColumn DataField="ID" HeaderText="">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Account" HeaderText="Subject">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Description" HeaderText="����">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                    </asp:BoundColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="CreatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                        DataTextField="CreatorName" HeaderText="Name" Target="_blank">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                    </asp:HyperLinkColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="UpdaterCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                        DataTextField="UpdaterName" HeaderText="Name" Target="_blank">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                    </asp:HyperLinkColumn>
                                                    <asp:BoundColumn DataField="UpdateTime" HeaderText="���ʱ��">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <ItemStyle CssClass="itemStyle" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditItemStyle BackColor="#2461BF" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                    </table>


                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>
                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
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
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
