<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZTurnDetailBrowse.aspx.cs" Inherits="TTWZTurnDetailBrowse" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body {
            font-family: 微软雅黑,宋体;
            font-size: 1.5em;
        }
    </style>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script type="text/javascript">

        $(function () {

        });

        function printpage() {
            document.getElementById("print").style.display = "none";
            document.getElementById("print0").style.display = "none";
            window.print()
            CloseLayer();
        }

    </script>

</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div id="AboveDiv">
                        <table id="AboveTable" cellpadding="0" width="1180" cellspacing="0" class="bian">
                            <%--  <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server"  Text="<%$ Resources:lang,QiCaiPingZhengYiJiaoChan%>"></asp:Label>
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
                            </tr>--%>
                            <tr>
                                <td style="padding: 0px 5px 5px 5px;" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="top" style="padding-top: 5px;">
                                                <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td  style="width: 100%; padding: 5px 5px 5px 5px;" class="formItemBgStyleForAlignLeft" valign="top">
                                                            <table class="formBgStyle" width="100%">
                                                                <tr>
                                                                    <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <h1>
                                                                            <asp:Label ID="Label2" runat="server"  Text="<%$ Resources:lang,ZhongDanYouDiErJianSheGongSiQiCaiPingZhengYiJiaoChan%>"></asp:Label></h1>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="4">

                                                                        <asp:Label ID="Label5" runat="server"  Text="<%$ Resources:lang,YiJiaoChanBianHao%>"></asp:Label>
                                                                        ： 
                                                                         <asp:Label ID="LB_TurnCode" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="2" >
                                                                        <asp:Label ID="Label26" runat="server"  Text="<%$ Resources:lang,GongChengXiangMu%>"></asp:Label>
                                                                        ： 
                                                                         <asp:Label ID="LB_ProjectName" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft" >
                                                                        <asp:Label ID="Label3" runat="server"  Text="<%$ Resources:lang,YiJiaoRiJi%>"></asp:Label>
                                                                        ： 
                                                                         <asp:Label ID="LB_TurnDate" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td  class="formItemBgStyleForAlignLeft" colspan="2">
                                                                        <asp:Label ID="Label4" runat="server"  Text="<%$ Resources:lang,ShiGongChanWei%>"></asp:Label>：
                                                                        <asp:Label ID="LB_PickingUnit" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label27" runat="server"  Text="<%$ Resources:lang,ZhiBiaoShiJian%>"></asp:Label>：
                                                                        <asp:Label ID="LB_CurrentDate" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <div style="width: 100%;">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                <tr>
                                                                                    <td width="7">
                                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="5%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label17" runat="server"  Text="<%$ Resources:lang,LiaoChanHao%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="7%" align="left">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,WuZiMingCheng%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="7%" align="left">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label8" runat="server"  Text="<%$ Resources:lang,GuiGeXingHao%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="5%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label7" runat="server"  Text="<%$ Resources:lang,JiLiangChanWei%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="5%" align="right">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label19" runat="server"  Text="<%$ Resources:lang,JiHuaShuLiang%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="5%" align="right">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label6" runat="server"  Text="<%$ Resources:lang,ShiFaShuLiang%>"></asp:Label></strong>
                                                                                                </td>

                                                                                                <td width="5%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label28" runat="server" Text="ID计划"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="5%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label29" runat="server"  Text="<%$ Resources:lang,TuHao%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="5%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,LingLiaoRiQi%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="4%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label31" runat="server"  Text="<%$ Resources:lang,ShouLiao%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="4%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label32" runat="server"  Text="<%$ Resources:lang,ZhiZheng%>"></asp:Label></strong>
                                                                                                </td>
                                                                                                <td width="4%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label33" runat="server"  Text="<%$ Resources:lang,FanChan%>"></asp:Label></strong>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td width="6" align="right">
                                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <asp:DataGrid ID="DG_TurnDetail" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                                                CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="20" ShowHeader="false"
                                                                                Width="100%" OnPageIndexChanged="DG_TurnDetail_PageIndexChanged">
                                                                                <Columns>
                                                                                    <asp:TemplateColumn>
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="center" Width="5%" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,NoBianHao%>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <%# ShareClass.StringCutByRequire(Eval("NoCode").ToString(), 192) %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:TemplateColumn>
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Left" Width="7%" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,WuZiMingCheng%>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <%# ShareClass.StringCutByRequire(Eval("ObjectName").ToString(), 192) %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:BoundColumn DataField="Model" HeaderText="规格型号">
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Left" Width="7%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="UnitName" HeaderText="计量单位">
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Center" Width="5%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="TicketNumber" HeaderText="计划数量">
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Right" Width="5%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="ActualNumber" HeaderText="实领数量">
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Right" Width="5%" />
                                                                                    </asp:BoundColumn>

                                                                                    <asp:BoundColumn DataField="PickingPlanCode" HeaderText="ID计划">
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="center" Width="5%" />
                                                                                    </asp:BoundColumn>

                                                                                    <asp:TemplateColumn>
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Center" Width="5%" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,KaiPiaoRiQi%>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <%--<%#DataBinder.Eval(Container.DataItem, "TicketTime", "{0:yyyy/MM/dd}")%>--%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>

                                                                                    <asp:TemplateColumn>
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Center" Width="5%" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,LingLiaoRiQi%>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <%--  <%#DataBinder.Eval(Container.DataItem, "PickingTime", "{0:yyyy/MM/dd}")%--%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:BoundColumn HeaderText="收料员">
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Center"  Width="4%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn HeaderText="质证">
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Center" Width="4%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn HeaderText="返单">
                                                                                        <ItemStyle CssClass="itemBorderBlack" HorizontalAlign="Center" Width="4%" />
                                                                                    </asp:BoundColumn>
                                                                                </Columns>
                                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                <EditItemStyle BackColor="#2461BF" />
                                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                                <ItemStyle CssClass="itemStyle" BorderWidth ="1" BorderColor ="Black" />
                                                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                                            </asp:DataGrid>
                                                                        </div>

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center" class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label20" runat="server"  Text="<%$ Resources:lang,JiHuaYuan%>"></asp:Label>：<asp:Label ID="LB_PurchaseManager" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td style="text-align: center" class="formItemBgStyleForAlignLeft">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label21" runat="server"  Text="<%$ Resources:lang,LingLiaoRen%>"></asp:Label>：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td style="text-align: center" class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label22" runat="server"  Text="<%$ Resources:lang,CaiLiaoYuan%>"></asp:Label>：
                                                                        <asp:Label ID="LB_MaterialPersonName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center" class="formItemBgStyleForAlignLeft" colspan="3">
                                                                        <p class="noprint">
                                                                            <input id="print" class="inpu" onclick="window.returnValue = false; CloseLayer();"
                                                                                type="button" value="关闭" />
                                                                            <input id="print0" class="inpu" onclick="printpage();" type="button" value="打印" />
                                                                        </p>
                                                                        <asp:Label ID="LB_TurnDetail" runat="server" Visible="false"></asp:Label>
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

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
