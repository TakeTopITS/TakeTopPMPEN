<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsSaleOrderView.aspx.cs" Inherits="TTGoodsSaleOrderView" %>

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
            font-family: ΢���ź�,����;
            font-size: 1em;
        }

        .auto-style4 {
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
                                <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" />
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

                                       <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ShangPinXiaoShouDan%>"></asp:Label>
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
                        <td style="text-align: center; width: 100%;">
                            <asp:DataList ID="DataList20" runat="server" Width="100%" CellPadding="0" CellSpacing="0">
                                <ItemTemplate>
                                    <table class="bian" style="width: 100%; border-collapse: collapse; margin: 0px auto;" cellpadding="4"
                                        cellspacing="0">
                                        <tr>
                                            <td style="text-align: left;">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>��
                                                 <%#DataBinder.Eval(Container.DataItem, "SOName")%>
                                            </td>
                                            <td style="text-align: left" colspan="2">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,KeHu%>"></asp:Label>��
                                                <a href='TTCustomerInfoView.aspx?CustomerCode=<%#DataBinder .Eval (Container .DataItem ,"CustomerCode") %>' target="DetailArea"><%#DataBinder.Eval(Container.DataItem, "CustomerName")%></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left" class="auto-style4">
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,YeWuYuan%>"></asp:Label>��
                                            <a href='TTUserInforSimple.aspx?UserCode=<%#DataBinder .Eval (Container .DataItem ,"SalesCode") %>' target="DetailArea"><%#DataBinder.Eval(Container.DataItem, "SalesName")%></a>
                                            </td>
                                            <td style="text-align: left" class="auto-style4">
                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label>��<%#DataBinder.Eval(Container.DataItem, "Amount")%><%#DataBinder.Eval(Container.DataItem, "CurrencyType")%></td>
                                            <td style="text-align: left" class="auto-style4">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,XiaoShouShiJian%>"></asp:Label>��
                                            <%#DataBinder.Eval(Container.DataItem, "SaleTime", "{0:yyyy/MM/dd}")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left">
                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,YuJiDaoHuoShiJian%>"></asp:Label>��
                                            <%#DataBinder.Eval(Container.DataItem, "ArrivalTime","{0:yyyy/MM/dd}")%>
                                            </td>
                                            <td style="text-align: left" colspan="2">
                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>��
                                            <%#DataBinder.Eval(Container.DataItem, "Comment")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left">
                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,KaiPiaoShiJian%>"></asp:Label>��
                                            <%#DataBinder.Eval(Container.DataItem, "OpenInvoiceTime","{0:yyyy/MM/dd}")%>
                                            </td>
                                            <td style="text-align: left" colspan="2">
                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,FaPiaoHaoMa%>"></asp:Label>��
                                            <%#DataBinder.Eval(Container.DataItem, "InvoiceCode")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                width="100%">
                                <tr>
                                    <td width="7">
                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>



                                                <td align="left" width="8%">

                                                    <strong>
                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>
                                                    </strong>
                                                </td>

                                                <td align="left" width="12%">

                                                    <strong>
                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>
                                                    </strong>
                                                </td>

                                                <td align="left" width="8%">

                                                    <strong>
                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>
                                                    </strong>
                                                </td>

                                                <td align="left" width="12%">

                                                    <strong>
                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>
                                                    </strong>
                                                </td>

                                                <td align="left" width="8%">

                                                    <strong>
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                    </strong>
                                                </td>

                                                <td align="left" width="8%">

                                                    <strong>
                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>
                                                    </strong>
                                                </td>

                                                <td align="left" width="8%">

                                                    <strong>
                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>
                                                    </strong>
                                                </td>

                                                <td align="left" width="8%">

                                                    <strong>
                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>
                                                    </strong>
                                                </td>
                                                <td align="left" width="8%">

                                                    <strong>
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label>
                                                    </strong>
                                                </td>
                                                <td align="left">

                                                    <strong>
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>
                                                    </strong>
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
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="SaleReason" HeaderText="Remark">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                    </asp:BoundColumn>
                                </Columns>

                                <EditItemStyle BackColor="#2461BF" />

                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                <ItemStyle CssClass="itemStyle" />

                                <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            </asp:DataGrid>

                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <table width="80%">
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ZhiBiao%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhiXing%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft"></td>

                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,PiZhun%>"></asp:Label>��</td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                            </tr>
                                        </table>
                                    </td>
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
