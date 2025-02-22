<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZCollectPrintPag.aspx.cs" Inherits="TTWZCollectPrintPag" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script language="javascript">
        function printpage() {
            document.getElementById("print").style.display = "none";
            document.getElementById("print0").style.display = "none";
            window.print();
            CloseLayer();
        }

        $(function () {

        });
    </script>


    <style type="text/css" media="print">
        .noprint {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <center>
            <div style="padding: 0 10px 0 10px">
                <asp:Repeater ID="RT_Collect" runat="server" OnItemDataBound="RT_Collect_ItemDataBound">
                    <HeaderTemplate>
                        <table width="1080px" cellpadding="0" cellspacing="0" class="bian">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr style="height: 300px;">
                            <td style="padding-top: 100px;">
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: center; font-size: 36px;">��ʯ�͵ڶ����蹫˾���ϵ�
                                            <br />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr style="height: 300px;">
                                        <td>
                                            <table width="100%" cellpadding="0" cellspacing="0" border="1">
                                                <tr>
                                                    <td align="left">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 30%;">���ϲֿ⣺<%# Eval("StoreRoom") %></td>
                                                                <td style="width: 20%;">���Ϸ�ʽ��<%# Eval("CollectMethod") %></td>
                                                                <td style="width: 30%;">�������ڣ�<%#  DateTime .Parse ( Eval("CollectTime").ToString ()).ToString ("yyyy-MM-dd") %></td>
                                                                <td style="width: 20%;">��λ�ţ�<%# Eval("GoodsCode") %>
                                                                    <br />
                                                                    No��<%# Eval("CollectCode") %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 30%;">�ļ�ţ�<%# Eval("CheckCode") %></td>
                                                                <td style="width: 20%;">��ͬ�ţ�<%# Eval("CompactCode") %></td>
                                                                <td style="width: 50%;">��ע��<%# Eval("StoreRoom") %>��&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 30%;">�������ƣ�<%# Eval("ObjectName") %></td>
                                                                <td style="width: 20%;">������λ��<%# Eval("UnitName") %></td>
                                                                <td style="width: 20%;">��׼��<%# Eval("Criterion") %></td>
                                                                <td style="width: 30%;">����<%# Eval("Grade") %></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 30%;">����ͺţ�<%# Eval("Model") %></td>
                                                                <td style="width: 20%;">��Ʊ���룺<%# Eval("TicketNumber") %></td>
                                                                <td style="width: 20%;">�˷ѣ�<%# Eval("Freight") %></td>
                                                                <td style="width: 30%;">������<%# Eval("OtherObject") %></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 30%;">������λ��<%# Eval("SupplierName") %></td>
                                                                <td style="width: 20%;">&nbsp;</td>
                                                                <td style="width: 20%;">&nbsp;</td>
                                                                <td style="width: 30%;">&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 15%;">Ӧ��������<%# Eval("CollectNumber") %></td>
                                                                <td style="width: 15%;">ʵ��������<%# Eval("ActualNumber") %></td>
                                                                <td style="width: 15%;">�ƻ�����<%# Eval("ActualNumber") %>��&nbsp;</td>
                                                                <td style="width: 15%;">�ƻ����<%# Eval("ActualNumber") %>��&nbsp;</td>
                                                                <td style="width: 15%;">ʵ�����ۣ�<%# Eval("ActualPrice") %></td>
                                                                <td style="width: 15%;">ʵ����<%# Eval("ActualMoney") %></td>
                                                                <td style="width: 10%;">˰��<%# Eval("RatioMoney") %></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 25%;">��Ӧ���ܣ�<%# Eval("DelegateAgentName") %></td>
                                                                <td style="width: 25%;">�ļ�Ա��<%# Eval("CheckerName") %></td>
                                                                <td style="width: 25%;">����Ա��<%# Eval("SafekeeperName") %></td>
                                                                <td style="width: 25%;">�Ƶ���<%# Eval("ContacterName") %></td>
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
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

            </div>
            <div id="divOver" style="text-align: center">
                <br />
                <p class="noprint">
                    <input id="print" type="button" class="btn" value="ȷ�ϴ�ӡ" onclick="printpage();" />
                    <input id="print0" type="button" value="Closed" onclick="CloseLayer();" class="btn" />
                </p>
            </div>
        </center>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
