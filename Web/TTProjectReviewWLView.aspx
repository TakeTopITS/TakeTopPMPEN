<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectReviewWLView.aspx.cs"
    Inherits="TTProjectReviewWLView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">$(function () { if (top.location != self.location) { } else { CloseWebPage(); } });</script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
            <table width="99%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
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
                                <td width="40%" align="left">
                                    <strong>
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong>
                                </td>
                                <td width="20%" align="left">
                                    <strong>
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong>
                                </td>
                                <td width="20%" align="left">
                                    <strong>
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                </td>
                                <td width="10%" align="left">
                                    <strong></strong>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="6" align="right">
                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                </tr>
            </table>
            <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" Height="1px" ShowHeader="false"
                PageSize="5" Width="99%">
                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                <ItemStyle CssClass="itemStyle" />

                <Columns>
                    <asp:BoundColumn DataField="WLID" HeaderText="Number">
                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="WLName" HeaderText="Workflow">
                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="40%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="CreateTime" HeaderText="ÉêÇëÊ±¼ä">
                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Status">
                        <ItemTemplate>
                            <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                        </ItemTemplate>
                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=Review&WLID={0}") %>'
                                Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder"/></asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                    </asp:TemplateColumn>
                </Columns>
                <HeaderStyle Horizontalalign="left" />
            </asp:DataGrid>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
