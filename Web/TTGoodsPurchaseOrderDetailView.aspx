<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsPurchaseOrderDetailView.aspx.cs" Inherits="TTGoodsPurchaseOrderDetailView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }



        });

    </script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
            <div style="position: relative; top: 10px;">
                <table style="width: 100%;">
                    <tr>
                        <td align="left">
                          <asp:Label ID="LB_Sql" runat="server"></asp:Label>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                <tr>
                                    <td width="7">
                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                    </td>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                </td>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                </td>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label></strong>
                                                </td>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                </td>
                                                <%--<td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,GongYi%>"></asp:Label></strong>
                                                </td>--%>
                                                <td width="15%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="8%"><strong>
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label></strong> </td>

                                                <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                </td>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                </td>
                                                <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                </td>

                                                <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,KuCun%>"></asp:Label></strong>
                                                </td>

                                                <%-- <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                </td>--%>
                                                <td align="left">
                                                    <strong>
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,GouMaiLiYou%>"></asp:Label></strong>
                                                </td>
                                                <%--  <td width="14%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,YiRuKu%>"></asp:Label></strong>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="6" align="right">
                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" GridLines="None"
                                ShowHeader="false" Width="100%">
                                <Columns>
                                    <asp:BoundColumn DataField="ID" HeaderText="Number">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Type" HeaderText="Type">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Manufacturer" HeaderText="Brand">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:BoundColumn>
                                  <%--  <asp:BoundColumn DataField="ProcessRoute" HeaderText="工艺">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:BoundColumn>--%>
                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>

                                    <asp:TemplateColumn HeaderText="库存数量">
                                        <ItemTemplate>
                                            <%#  GetGoodsStockTotalNumber(Eval("GoodsCode").ToString(),Eval("Manufacturer").ToString(),Eval("ModelNumber").ToString(),Eval("Spec").ToString()) %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                    </asp:TemplateColumn>

                                    <%-- <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                        DataTextField="ApplicantName" Target="_blank" HeaderText="Applicant">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:HyperLinkColumn>--%>
                                    <asp:HyperLinkColumn DataNavigateUrlField="PurReason" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                        DataTextField="PurReason" Target="_blank" HeaderText="购买理由">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                    </asp:HyperLinkColumn>
                                    <%-- <asp:BoundColumn DataField="CheckInNumber" HeaderText="已入库">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="14%" />
                                    </asp:BoundColumn>--%>
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
