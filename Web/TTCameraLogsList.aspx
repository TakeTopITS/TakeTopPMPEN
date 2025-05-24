<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTCameraLogsList.aspx.cs" Inherits="TTCameraLogsList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>����ͷ��־</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div>
        <div id="AboveDiv">
            <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                <tr>
                    <td height="31" class="page_topbj">
                        <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="29">
                                                <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                            </td>
                                            <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,SheXiangTouRiZhi%>"></asp:Label>
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
                                    <asp:DataGrid ID="DG_CameraLogList" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="True"
                                        Width="100%">
                                        <Columns>
                                            <asp:BoundColumn DataField="Area" HeaderText="����">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ProjectDepartment" HeaderText="ProjectDepartment">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ServerIP" HeaderText="����ͷIP">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CreatorName" HeaderText="�鿴��">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CreateTime" HeaderText="�鿴ʱ��">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                            </asp:BoundColumn>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <EditItemStyle BackColor="#2461BF" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                        
                                        <ItemStyle CssClass="itemStyle" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Horizontalalign="left" />
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
