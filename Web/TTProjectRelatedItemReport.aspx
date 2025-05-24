<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectRelatedItemReport.aspx.cs" Inherits="TTProjectRelatedItemReport" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
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
        <form id="form1" runat="server">
            <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>

                        <table style="width: 1080px;">
                            <tr>
                                <td width="12%" style="text-align: right;">
                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ShangPinDaiMa%>"></asp:Label>��</td>
                                <td align="left" width="25%">
                                    <asp:TextBox ID="TB_GoodsCode" runat="server" Width="190px"></asp:TextBox>
                                </td>
                                <td style="text-align: right;" width="15%">
                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ShangPinMingCheng%>"></asp:Label>��</td>
                                <td align="left" width="20%">
                                    <asp:TextBox ID="TB_GoodsName" runat="server" Width="190px"></asp:TextBox>
                                </td>
                                <td width="5%" align="right">&nbsp;</td>
                                <td width="10%" style="text-align: left;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right;" width="12%">
                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>��</td>
                                <td align="left" width="25%">
                                    <asp:TextBox ID="TB_ModelNumber" runat="server" Width="190px"></asp:TextBox>
                                </td>
                                <td style="text-align: right;" width="15%">
                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label>��</td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="TB_Spec" runat="server" Width="99%"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td align="left"></td>
                                <td align="left">
                                    <asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                </td>
                                <td class="style2" style="text-align: center;">
                                    <a href="#" onclick="preview1()">
                                        <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" />
                                    </a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;" colspan="6">&nbsp;
                                </td>
                            </tr>
                        </table>
                        <hr />

                        <table style="width: 1080px;">

                            <tr>
                                <td>

                                    <div style="position: relative; width: 100%;">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                            <tr>
                                                <td colspan="14" style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">
                                                    <br />
                                                    <asp:Label ID="LB_tbProjectRelatedItemList" runat="server" Text="<%$ Resources:lang,XiangMuGuanLianLiaoPing%>"></asp:Label>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="14"></td>
                                            </tr>
                                            <tr>
                                                <td width="7">
                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                </td>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <%-- <td width="3%" align="left">
                                                            <strong>
                                                                <asp:Label ID="LB_dgChildItemID" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label></strong>
                                                        </td>--%>
                                                            <td width="9%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="LB_dgChildItemCode" runat="server" Text="<%$ Resources:lang,Code%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="LB_dgChildItemName" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="LB_dgChildItemNumber" runat="server" Text="<%$ Resources:lang,Number%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,YuCaiGouLiang%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YuRuKuLiang%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label9" runat="server" Text="MaterialIssuedQuantity"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,YuChuKuLiang%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,YuShengChanLiang %>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label10" runat="server" Text="SoldQuantity"></asp:Label></strong>
                                                            </td>

                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="LB_dgChildItemProcess" runat="server" Text="<%$ Resources:lang,KuChengLiang%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="7%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong>
                                                            </td>
                                                            <%-- <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label15" runat="server" Text="���۵���"></asp:Label></strong>
                                                        </td>
                                                        <td align="left">
                                                            <strong>
                                                                <asp:Label ID="Label14" runat="server" Text="�����ܶ�"></asp:Label></strong>
                                                        </td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="6" align="right">
                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div style="overflow-y: auto; height: 500px; display: block; width: 100%;">
                                        <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False"
                                            Width="100%" ShowHeader="false">
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditItemStyle BackColor="#2461BF" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                            <ItemStyle CssClass="itemStyle" Horizontalalign="left" />
                                            <Columns>
                                                <%--  <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="3%" />
                                            </asp:BoundColumn>--%>
                                                <asp:BoundColumn DataField="ItemCode" HeaderText="Code">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ItemName" HeaderText="Name">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Specification" HeaderText="Specification">
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyPurchased" HeaderText="PurchasedQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyCheckIn" HeaderText="StockedQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyPick" HeaderText="MaterialIssuedQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyCheckOut" HeaderText="OutboundQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyProduction" HeaderText="ProducedQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadySale" HeaderText="SoldQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="�����">
                                                    <ItemTemplate>
                                                        <%# ShareClass.GetMaterialsStockNumber(Eval("ItemCode").ToString()) %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="PurchasePrice" HeaderText="�ɹ�����">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="�ɹ��ܶ�">
                                                    <ItemTemplate>
                                                        <%# decimal.Parse( Eval("PurchasePrice").ToString()) * decimal.Parse(Eval("Number").ToString()) %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                </asp:TemplateColumn>
                                                <%-- <asp:BoundColumn DataField="SalePrice" HeaderText="���۵���">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="�����ܶ�">
                                                <ItemTemplate>
                                                    <%# decimal.Parse( Eval("SalePrice").ToString()) * decimal.Parse(Eval("Number").ToString()) %>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                            </asp:TemplateColumn>--%>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>

                                    <div style="position: relative; width: 100%; display: none;">

                                        <!--startprint1-->
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                            <tr>
                                                <td colspan="14" style="width: 100%; height: 80px; font-size: xx-large; text-align: center; background-color: white;">
                                                    <br />
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,XiangMuGuanLianLiaoPing%>"></asp:Label>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="14" align="left">
                                                   
                                                        <asp:Label ID="LB_ProjectName" runat="server"></asp:Label>
                                                   

                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="7">
                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                </td>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <%-- <td width="3%" align="left">
                                                            <strong>
                                                                <asp:Label ID="LB_dgChildItemID" runat="server" Text="<%$ Resources:lang,ID%>"></asp:Label></strong>
                                                        </td>--%>
                                                            <td width="9%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,Code%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,Name%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,Number%>"></asp:Label></strong>
                                                            </td>

                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,YuCaiGouLiang%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,YuRuKuLiang%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label26" runat="server" Text="MaterialIssuedQuantity"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,YuChuKuLiang%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,YuShengChanLiang %>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label29" runat="server" Text="SoldQuantity"></asp:Label></strong>
                                                            </td>

                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,KuChengLiang%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="7%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong>
                                                            </td>
                                                            <%-- <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label15" runat="server" Text="���۵���"></asp:Label></strong>
                                                        </td>
                                                        <td align="left">
                                                            <strong>
                                                                <asp:Label ID="Label14" runat="server" Text="�����ܶ�"></asp:Label></strong>
                                                        </td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="6" align="right">
                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False"
                                            Width="100%" ShowHeader="false">
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditItemStyle BackColor="#2461BF" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                            <ItemStyle CssClass="itemStyle" Horizontalalign="left" />
                                            <Columns>
                                                <%--  <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="3%" />
                                            </asp:BoundColumn>--%>
                                                <asp:BoundColumn DataField="ItemCode" HeaderText="Code">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ItemName" HeaderText="Name">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Specification" HeaderText="Specification">
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>

                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyPurchased" HeaderText="PurchasedQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyCheckIn" HeaderText="StockedQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyPick" HeaderText="MaterialIssuedQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyCheckOut" HeaderText="OutboundQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadyProduction" HeaderText="ProducedQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="AleadySale" HeaderText="SoldQuantity">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="�����">
                                                    <ItemTemplate>
                                                        <%# ShareClass.GetMaterialsStockNumber(Eval("ItemCode").ToString()) %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="PurchasePrice" HeaderText="�ɹ�����">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="�ɹ��ܶ�">
                                                    <ItemTemplate>
                                                        <%# decimal.Parse( Eval("PurchasePrice").ToString()) * decimal.Parse(Eval("Number").ToString()) %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                </asp:TemplateColumn>
                                                <%-- <asp:BoundColumn DataField="SalePrice" HeaderText="���۵���">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="�����ܶ�">
                                                <ItemTemplate>
                                                    <%# decimal.Parse( Eval("SalePrice").ToString()) * decimal.Parse(Eval("Number").ToString()) %>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                            </asp:TemplateColumn>--%>
                                            </Columns>
                                        </asp:DataGrid>
                                        <!--endprint1-->
                                    </div>

                                </td>
                            </tr>
                        </table>

                    </div>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
