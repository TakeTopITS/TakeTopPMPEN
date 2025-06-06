<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsSupplyOrderViewQualityCheck.aspx.cs" Inherits="TTGoodsSupplyOrderViewQualityCheck" %>

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
            font-family: 微软雅黑,宋体;
            font-size: 1em;
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
                                <img src="ImagesSkin/print.gif" alt="打印" border="0" />
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

                                            <asp:Label ID="LB_ReportName" runat="server" Text="<%$ Resources:lang,GongHuoZiJianDan%>"></asp:Label>
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
                            <asp:DataList ID="DataList1" runat="server" Width="100%" CellPadding="0" CellSpacing="0">
                                <ItemTemplate>
                                    <table class="bian" style="border-collapse: collapse; margin: 0px auto;" cellpadding="4" width="100%"
                                        cellspacing="0">
                                        <tr>

                                            <td style="text-align: left">
                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>：
                                            <%#DataBinder.Eval(Container.DataItem, "SUName")%>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,GongHuoShiJian%>"></asp:Label>：
                                            <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "SupplyTime")).ToString("yyyy/MM/dd")%>
                                            </td>

                                        </tr>
                                        <tr>

                                            <td style="text-align: left">
                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,GongYingShang%>"></asp:Label>：
                                            <%#DataBinder.Eval(Container.DataItem, "Supplier")%>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,DianHua%>"></asp:Label>：<%#DataBinder.Eval(Container.DataItem, "SupplierPhone")%></td>
                                        </tr>

                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 70%;">

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
                                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="10%">
                                                    <strong>
                                                        <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="8%">
                                                    <strong>
                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="15%">
                                                    <strong>
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="10%">
                                                    <strong>
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="8%">
                                                    <strong>
                                                        <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong>
                                                </td>
                                                <td align="left" width="8%">
                                                    <strong>
                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong>
                                                </td>

                                                <td align="left" width="10%">
                                                    <strong>
                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,BuLiangLiang %>"></asp:Label></strong>
                                                </td>
                                                <td align="left">
                                                    <strong>
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,JianChaJieGuo %>"></asp:Label></strong>
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
                                PageSize="5" ShowHeader="False"
                                Width="100%">
                                <Columns>


                                    <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
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

                                    <asp:BoundColumn DataField="DefectiveNumber" HeaderText="不良量">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                    </asp:BoundColumn>

                                    <asp:BoundColumn DataField="QCResult" HeaderText="质检结论">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                    </asp:BoundColumn>
                                </Columns>

                                <ItemStyle CssClass="itemStyle" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditItemStyle BackColor="#2461BF" />
                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle HorizontalAlign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                            </asp:DataGrid>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <table width="80%">
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ZhiBiao%>"></asp:Label>：</td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhiXing%>"></asp:Label>：</td>
                                                <td class="formItemBgStyleForAlignLeft"></td>

                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,PiZhun%>"></asp:Label>：</td>
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
