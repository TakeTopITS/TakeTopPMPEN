<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsShipmentOrderView.aspx.cs" Inherits="TTGoodsShipmentOrderView" %>

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

                                       <asp:Label ID="LB_ReportName" runat="server" Text="<%$ Resources:lang,ChuKuDan%>"></asp:Label>
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

                        <td style="text-align: center; width: 980px;">
                            <asp:DataList ID="DataList1" runat="server" Width="980px" CellPadding="0" CellSpacing="0">
                                <ItemTemplate>
                                    <table class="bian" style="width: 100%; border-collapse: collapse; margin: 0px auto;" cellpadding="4"
                                        cellspacing="0">
                                        <tr>
                                            <td style="text-align: left;">
                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,BuMen%>"></asp:Label>：
                                            </td>
                                            <td style="text-align: left" colspan="2">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ChuKuShiJian%>"></asp:Label>：
                                            <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "ShipTime")).ToString("yyyy/MM/dd")%>
                                            </td>

                                            <td style="text-align: left">
                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：<asp:CheckBox ID="CheckBox2" runat="server" />
                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,chengpin%>"></asp:Label>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,CaiLiao%>"></asp:Label>
                                                <asp:CheckBox ID="CheckBox3" runat="server" />
                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,QiTa%>"></asp:Label>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: left">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>：
                                            <%#DataBinder.Eval(Container.DataItem, "GSHOName")%>
                                            </td>
                                            <td style="text-align: left" colspan="4">
                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label>：
                                            <%#DataBinder.Eval(Container.DataItem, "Applicant")%>
                                            </td>
                                        </tr>

                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 70%;">

                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                <tr>
                                    <td width="7">
                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                    </td>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>

                                                <td width="7%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShangPinDaiMa%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ShangPinMingCheng%>"></asp:Label></strong>
                                                </td>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                </td>
                                                <td width="12%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                </td>
                                                <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                </td>
                                                <%--   <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                </td>--%>
                                                <%-- <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong>
                                                </td>--%>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                </td>
                                                <%--  <td width="8%" align="left">
                                                                                    <strong><asp:Label runat="server" Text="<%$ Resources:lang,BaoXiuQiTian%>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="8%" align="left">
                                                                                    <strong><asp:Label runat="server" Text="<%$ Resources:lang,XuLieHao%>"></asp:Label></strong>
                                                                                </td>--%>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ChuKuWeiZhi%>"></asp:Label></strong>
                                                </td>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="6" align="right">
                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                Height="1px" Width="100%" CellPadding="4"
                                ForeColor="#333333" GridLines="Both">
                                <Columns>

                                    <asp:BoundColumn DataField="GoodsCode" HeaderText="物料代码">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="7%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="GoodsName" HeaderText="MaterialName">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <%-- <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                    </asp:BoundColumn>--%>
                                    <asp:BoundColumn DataField="UnitName" HeaderText="Unit">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <%-- <asp:BoundColumn DataField="WarrantyPeriod" HeaderText="保修期（天）">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SN" HeaderText="序列号">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                    </asp:BoundColumn>--%>
                                    <asp:BoundColumn DataField="FromPosition" HeaderText="出库位置">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Comment" HeaderText="Remark">
                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="8%" />
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
                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,FaLiaoRen%>"></asp:Label>：</td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ShenHe%>"></asp:Label>：</td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft" width="150px">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,PiZhunRen%>"></asp:Label>：</td>
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
