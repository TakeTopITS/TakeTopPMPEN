<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTTransferProjectRecord.aspx.cs"
    Inherits="TTTransferProjectRecord" %>

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

            <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                <tr>
                    <td height="31" class="page_topbj">
                        <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <table width="300" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="29">
                                                <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                            </td>
                                            <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XiangMuJingLiBianGengJiLu%>"></asp:Label>
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
                    <td>
                        <br />
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="height: 16px; padding: 5px 5px 5px 5px;" align="right">
                                    <a href="#" onclick="preview1()">
                                        <img src="ImagesSkin/print.gif" alt="打印" border="0" />
                                    </a></td>
                            </tr>
                        </table>
                        <!--startprint1-->
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">
                                    <br />
                                    <strong>
                                        <asp:Label ID="LB_ReportName" runat="server" Text="<%$ Resources:lang,XiangMuJingLiBianGengJiLu%>"></asp:Label>
                                    </strong>
                                    <br />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="90%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                        <tr>
                                            <td width="7">
                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="10%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label></strong>
                                                        </td>
                                                        <td width="10%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XiangMuHao%>"></asp:Label></strong>
                                                        </td>
                                                        <td width="15%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,JueSe%>"></asp:Label></strong>
                                                        </td>
                                                        <td width="20%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,YuanXiangMuJingLi%>"></asp:Label></strong>
                                                        </td>
                                                        <td width="20%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,XianXiangMuJingLi%>"></asp:Label></strong>
                                                        </td>
                                                        <td width="25%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,BianGengShiJian%>"></asp:Label></strong>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="6" align="right">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" Height="1px"
                                        PageSize="5" Width="90%" ShowHeader="false" CellPadding="4" ForeColor="#333333"
                                        GridLines="None">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <EditItemStyle BackColor="#2461BF" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                        <ItemStyle CssClass="itemStyle" />
                                        <Columns>
                                            <asp:BoundColumn DataField="ID" HeaderText="SerialNumber">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ProjectID" HeaderText="ProjectNumber">
                                                <ItemStyle CssClass="itemBorder" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Actor" HeaderText="角色">
                                                <ItemStyle CssClass="itemBorder" Width="15%" />
                                            </asp:BoundColumn>
                                            <asp:HyperLinkColumn DataNavigateUrlField="OldPMCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                DataTextField="OldPMName" HeaderText="原项目经理" Target="_blank">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                            </asp:HyperLinkColumn>
                                            <asp:HyperLinkColumn DataNavigateUrlField="NewPMCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                DataTextField="NewPMName" HeaderText="现项目经理" Target="_blank">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                            </asp:HyperLinkColumn>
                                            <asp:BoundColumn DataField="ChangeTime" HeaderText="变更时间">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="25%" />
                                            </asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                        <!--endprint1-->
                        <br />
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
