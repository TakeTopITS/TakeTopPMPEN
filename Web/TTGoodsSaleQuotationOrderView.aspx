<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsSaleQuotationOrderView.aspx.cs" Inherits="TTGoodsSaleQuotationOrderView" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        body {
            font-family: Î¢ÈíÑÅºÚ,ËÎÌå;
            font-size: 1em;
        }
    </style>

    <style type="text/css">
        #AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }

        .auto-style1 {
            height: 18px;
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }



        });

        function preview1() {
            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint1-->";
            eprnstr = "<!--endprint1-->";
            prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 18);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
            document.body.innerHTML = bdhtml;
            return false;
        }

    </script>
</head>
<body>
    <center>
        <form id="form2" runat="server">
            <div style="position: relative; top: 50px;">
                <table width="100%">
                    <tr>
                        <td width="" align="right">
                            <a href="#" onclick="preview1()">
                                <img src="ImagesSkin/print.gif" alt="´òÓ¡" border="0" />
                            </a>
                        </td>

                    </tr>
                </table>
                <!--startprint1-->
                <table style="width: 980px;">
                    <tr>
                        <td style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">
                            <table width="100%">
                                <tr>
                                    <td align="center">

                                        <asp:Label ID="LB_ReportName" runat="server" Text="<%$ Resources:lang,ShangPinSongHuoDan%>"></asp:Label>
                                        <br />
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left">
                                        <%--<img src="Logo/FormLogo.png" />--%>
                                        <asp:Image ID="Img_BarCode" runat="server" />

                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: Left; width: 980px;">
                            <asp:DataList ID="DataList20" runat="server" Height="1px" Width="100%" CellPadding="0" CellSpacing="0">
                                <ItemTemplate>
                                    <table class="bian" width="100%" style="border-collapse: collapse; margin: 0px auto;" cellpadding="4" cellspacing="0">
                                        <tr>
                                            <td style="text-align: left" colspan="2">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>£º
                                            <%#DataBinder.Eval(Container.DataItem, "QOName")%>
                                            </td>


                                        </tr>
                                        <tr>
                                            <td style="text-align: left">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,KeHu%>"></asp:Label>£º
                                            <a href='TTCustomerInfoView.aspx?CustomerCode=<%#DataBinder .Eval (Container .DataItem ,"CustomerCode") %>' target="DetailArea"><%#DataBinder.Eval(Container.DataItem, "CustomerName")%></a>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,BaoJiaShiJian%>"></asp:Label>£º
                                            <%#DataBinder.Eval(Container.DataItem, "QuotationTime", "{0:yyyy/MM/dd}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,YeWuYuan%>"></asp:Label>£º
                                            <a href='TTUserInforSimple.aspx?UserCode=<%#DataBinder .Eval (Container .DataItem ,"SalesCode") %>' target="DetailArea"><%#DataBinder.Eval(Container.DataItem, "SalesName")%></a>
                                            </td>
                                            <td style="text-align: left" class="auto-style1">
                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label>£º<%#DataBinder.Eval(Container.DataItem, "Amount")%><%#DataBinder.Eval(Container.DataItem, "CurrencyType")%></td>

                                        </tr>

                                    </table>
                                </ItemTemplate>
                            </asp:DataList>

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
                                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="15%">
                                                    <strong>
                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="10%">
                                                    <strong>
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>
                                                    </strong>
                                                </td>
                                                <td align="left" width="15%">
                                                    <strong>
                                                        <asp:Label ID="Label70" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>
                                                    </strong>
                                                </td>
                                                <td align="left" width="9%">
                                                    <strong>
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                    </strong>
                                                </td>
                                                <td align="left" width="10%">
                                                    <strong>
                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="5%">
                                                    <strong>
                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="8%">
                                                    <strong>
                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="10%">
                                                    <strong>
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,JinE %>"></asp:Label></strong>
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
                                CellPadding="4" ForeColor="#333333" GridLines="Both" Height="1px"
                                ShowHeader="False"
                                Width="100%">

                                <Columns>

                                    <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="7%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="5%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                    </asp:BoundColumn>
                                </Columns>

                                <EditItemStyle BackColor="#2461BF" />

                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                <ItemStyle CssClass="itemStyle" />

                                <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            </asp:DataGrid>
                            <br />
                            <br />
                            <table width="80%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" width="150px">&nbsp;
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ZhiBiao%>"></asp:Label>£º</td>
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                    <td class="formItemBgStyleForAlignLeft" width="150px">
                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhiXing%>"></asp:Label>£º</td>
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                    <td class="formItemBgStyleForAlignLeft" width="150px">
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,PiZhun%>"></asp:Label>£º</td>
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!--endprint1-->
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
