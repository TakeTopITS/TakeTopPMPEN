<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectIncomeExpenseSAAS.aspx.cs" Inherits="TTProjectIncomeExpenseSAAS" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }



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

            <table cellpadding="0" cellspacing="0" width="980px" class="bian">
                <tr>
                    <td height="31" class="page_topbj">
                        <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="right">
                                    <a href="#" onclick="preview1()">
                                        <img src="ImagesSkin/print.gif" alt="打印" border="0" />
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!--startprint1-->
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top">
                                    <table cellpadding="0" cellspacing="0" width="98%">
                                        <tr style ="display :none;">
                                            <td align="left" style="padding: 0px 5px 0px 5px; font-weight: bold; height: 24px; color: #394f66; ">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="background: #f0f0f0; text-align: left; height: 25px; text-align: center; font-size: 15pt;">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label>：
                                                        <asp:Label ID="LB_ProjectIncomeExpenseTitle" runat="server" Font-Size="15pt"></asp:Label>
                                                            &nbsp;<asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ShouYiBiao%>"></asp:Label><br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="5" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td class="tdTopLine" align="left">
                                                                        <table style="width: 800px;">
                                                                            <tr>
                                                                                <td align="left">
                                                                                       <iframe runat="server" id="IFrame_Chart1" src="TTTakeTopAnalystChartSet.aspx" style="width: 300px; height: 295px; border: 1px solid white; overflow: hidden;"></iframe>


                                                                                    <%--<asp:Chart ID="Chart1" Width="800px" runat="server">
                                                                                        <Series>
                                                                                            <asp:Series Name="Series1" ChartType="Pie" ChartArea="ChartArea1" Legend="Legend1"></asp:Series>
                                                                                        </Series>
                                                                                        <ChartAreas>
                                                                                            <asp:ChartArea Name="ChartArea1" AlignmentOrientation="Horizontal"></asp:ChartArea>
                                                                                        </ChartAreas>
                                                                                        <Legends>
                                                                                            <asp:Legend Name="Legend1">
                                                                                                <CellColumns>
                                                                                                    <asp:LegendCellColumn Name="Column2">
                                                                                                        <Margins Left="15" Right="15" />
                                                                                                    </asp:LegendCellColumn>
                                                                                                    <asp:LegendCellColumn ColumnType="SeriesSymbol" Name="Column3">
                                                                                                        <Margins Left="15" Right="15" />
                                                                                                    </asp:LegendCellColumn>
                                                                                                </CellColumns>
                                                                                            </asp:Legend>
                                                                                        </Legends>
                                                                                        <Titles>
                                                                                            <asp:Title Name="标题" Alignment="TopCenter" IsDockedInsideChartArea="False" DockedToChartArea="ChartArea1"></asp:Title>
                                                                                        </Titles>
                                                                                    </asp:Chart>--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr style ="display :none;">
                                                                    <td class="tdTopLine" align="left">&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,HeTongZongE%>"></asp:Label>：
                                                                    <asp:Label ID="LB_TotalContractAmount" runat="server"></asp:Label>
                                                                        &nbsp;<asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YingShouZongE%>"></asp:Label>：<asp:Label ID="LB_TotalReceivablesAmount" runat="server"></asp:Label>
                                                                        &nbsp;<asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ShiShouZongE%>"></asp:Label>：<asp:Label ID="LB_TotalContractIncome" runat="server"></asp:Label>
                                                                        &nbsp;<asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,FeiYongZongE%>"></asp:Label>：<asp:Label ID="LB_TotalContractExpense" runat="server"></asp:Label>
                                                                        &nbsp;<br />
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ShiShouYuFeiYongChaE%>"></asp:Label>：<asp:Label ID="LB_IncomeExpenseBalance" runat="server"></asp:Label>
                                                                        &nbsp;<asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,HeTongYuShiShouChaE%>"></asp:Label>：<asp:Label ID="LB_ContractReceiverBalance" runat="server"></asp:Label>
                                                                        &nbsp;<asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,YingShouYuShiShouChaE%>"></asp:Label>：<asp:Label ID="LB_ReceivableReceiverBalance" runat="server"></asp:Label>

                                                                    </td>
                                                                </tr>
                                                                <tr style ="display :none;">
                                                                    <td style="background: #f0f0f0; text-align: Left;">
                                                                        <strong>&nbsp;&nbsp;<asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,HeTongShouKuang%>"></asp:Label>：</strong>
                                                                    </td>
                                                                </tr>
                                                                 <tr style ="display :none;">
                                                                    <td>
                                                                        <asp:DataList ID="DataList1" runat="server" Width="100%" CellPadding="0" ForeColor="#333333">
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <HeaderTemplate>
                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                    <tr>
                                                                                        <td width="7">
                                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td width="5%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="15%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,HeTongE%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="15%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,YingShouE%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="15%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShiShouE%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="25%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,DSKHTESSE%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="25%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,DSKYSESSE%>"></asp:Label></strong>
                                                                                                    </td>

                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td width="6" align="right">
                                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <table style="width: 100%" cellpadding="4" cellspacing="0">
                                                                                    <tr>
                                                                                        <td style="width: 5%; text-align: center" class="tdLeft">
                                                                                            <%#DataBinder .Eval (Container .DataItem ,"ProjectID") %>
                                                                                        </td>
                                                                                        <td style="width: 15%; text-align: center" class="tdLeft">
                                                                                            <%#DataBinder .Eval (Container .DataItem ,"ConstractAmount") %>
                                                                                        </td>
                                                                                        <td style="width: 15%; text-align: center" class="tdLeft">
                                                                                            <%#DataBinder .Eval (Container .DataItem ,"TotalReceivablesAmount") %>
                                                                                        </td>
                                                                                        <td style="width: 15%; text-align: center" class="tdLeft">
                                                                                            <%#DataBinder .Eval (Container .DataItem ,"TotalIncome") %>
                                                                                        </td>
                                                                                        <td style="width: 25%; text-align: center" class="tdLeft">
                                                                                            <%#DataBinder .Eval (Container .DataItem ,"ContractReceiverBalance") %>
                                                                                        </td>
                                                                                        <td style="width: 25%; text-align: center" class="tdLeft">
                                                                                            <%#DataBinder .Eval (Container .DataItem ,"ReceivableReceiverBalance") %>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                            <ItemStyle CssClass="itemStyle" />
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
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,HeTongDaiMa%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,XiangMuHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,YuanShiDanHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="10%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,YingShouJinE%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="10%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ShiShouJinE%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="10%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,ShiShouRiQi%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="10%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,KaiPiaoJinE%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,FuKuanFang%>"></asp:Label></strong>
                                                                                            </td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="right" width="6">
                                                                                    <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                            ForeColor="#333333" GridLines="None" Height="1px" ShowHeader="False" Width="100%">

                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="ConstractCode" DataNavigateUrlFormatString="TTConstractView.aspx?ConstractCode={0}"
                                                                                    DataTextField="ConstractCode" HeaderText="ContractCode" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="RelatedID" DataNavigateUrlFormatString="TTProjectDetailView.aspx?ProjectID={0}"
                                                                                    DataTextField="RelatedID" HeaderText="ProjectNumber" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="BillCode" HeaderText="OriginalDocumentNumber">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ReceivablesAccount" HeaderText="AmountReceivable">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ReceiverAccount" HeaderText="ActualAmountReceived">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ReceiverTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="实收日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="InvoiceAccount" HeaderText="开票金额">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Payer" HeaderText="Payer">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>


                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Horizontalalign="left" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="LB_UserCode" runat="server" Visible="false"></asp:Label>
                                                                        <asp:Label ID="LB_UserName" runat="server" Visible="false"></asp:Label>
                                                                        <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <!--endprint1-->
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
