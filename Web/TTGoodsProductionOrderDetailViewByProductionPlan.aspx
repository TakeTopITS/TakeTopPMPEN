<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsProductionOrderDetailViewByProductionPlan.aspx.cs" Inherits="TTGoodsProductionOrderDetailViewByProductionPlan" %>


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
            <div>
                <table width="100%">
                    <tr>
                        <td align="left">
                            <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                width="100%">
                                <tr>
                                    <td width="7">
                                        <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" />
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="8%">
                                                    <strong>
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="8%">
                                                    <strong>
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="18%">
                                                    <strong>
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="13%">
                                                    <strong>
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="25%">
                                                    <strong>
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="8%">
                                                    <strong>
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="10%">
                                                    <strong>
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                </td>

                                                <td align="left" width="10%">
                                                    <strong>
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" width="6">
                                        <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" />
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                ShowHeader="False"
                                Width="100%">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Éú²úµ¥">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                        <ItemTemplate>
                                            <a href='TTGoodsProductionOrderView.aspx?PDID=<%# DataBinder.Eval(Container.DataItem,"PDID") %>' target="_blank">
                                                <%# DataBinder.Eval(Container.DataItem,"PDName") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="18%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="25%" />
                                    </asp:BoundColumn>
                                     <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                    </asp:BoundColumn>

                                    <asp:BoundColumn DataField="UnitName" HeaderText="Unit">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
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
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
