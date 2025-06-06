<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsTransferReport.aspx.cs" Inherits="TTGoodsTransferReport" %>


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
                    <div id="AboveDiv">
                        <table style="width: 980px;">
                            <table style="width: 99%;">
                                <tr>
                                    <td width="12%" style="text-align: right;">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ShangPinDaiMa%>"></asp:Label>：</td>
                                    <td align="left" width="25%">
                                        <asp:TextBox ID="TB_GoodsCode" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;" width="15%">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ShangPinMingCheng%>"></asp:Label>：</td>
                                    <td align="left" width="20%">
                                        <asp:TextBox ID="TB_GoodsName" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                    <td width="5%" align="right">&nbsp;</td>
                                    <td width="10%" style="text-align: left;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;" width="12%">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>：</td>
                                    <td align="left" width="25%">
                                        <asp:TextBox ID="TB_ModelNumber" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;" width="15%">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label>：</td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="TB_Spec" runat="server" Width="99%"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td width="12%" style="text-align: right;">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>：
                                    </td>
                                    <td align="left" width="25%">


                                        <asp:TextBox ID="DLC_StartTime" ReadOnly="false" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1"
                                            runat="server" TargetControlID="DLC_StartTime">
                                        </ajaxToolkit:CalendarExtender>

                                    </td>
                                    <td style="text-align: right;" width="15%">
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>：
                                    </td>
                                    <td align="left" width="20%">
                                        <asp:TextBox ID="DLC_EndTime" ReadOnly="false" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender4"
                                            runat="server" TargetControlID="DLC_EndTime">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,LaiYuan%>"></asp:Label>：
                                    </td>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:DropDownList ID="DL_RelatedType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_RelatedType_SelectedIndexChanged">
                                                        <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                                        <asp:ListItem Value="Project" Text="<%$ Resources:lang,XiangMu%>" />
                                                        <asp:ListItem Value="Constract" Text="<%$ Resources:lang,HeTong%>" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="LB_RelatedID" runat="server" Text="ID:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <NickLee:NumberBox ID="NB_RelatedID" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Precision="0" Width="30px">0</NickLee:NumberBox>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>： </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TB_RelatedCode" runat="server" Width="80px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,CaoZuoRen%>"></asp:Label>：
                                 
                                        <asp:TextBox ID="TB_Applicant" runat="server" Width="127px"></asp:TextBox>

                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="BT_FindGoodsName" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />

                                        &nbsp;<asp:Button ID="BT_Export" runat="server" CssClass="inpu" OnClick="BT_Export_Click" Text="<%$ Resources:lang,DaoChu%>" />
                                    </td>
                                    <td>
                                        <a href="#" onclick="preview1()">
                                            <img src="ImagesSkin/print.gif" alt="打印" border="0" />
                                        </a>
                                    </td>

                                </tr>

                            </table>
                            <hr />
                            <!--startprint1-->
                            <table style="width: 99%;">
                                <tr>
                                    <td style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">
                                        <br />
                                        <asp:Label ID="LB_ReportName" runat="server" Text="<%$ Resources:lang,DiaoBoBaoBiao%>"></asp:Label>
                                        <br />
                                    </td>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table align="left" background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="7">
                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                    </td>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="left" width="5%"><strong>
                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="6%"><strong>
                                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="8%"><strong>
                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="8%"><strong>
                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="10%"><strong>
                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="6%"><strong>
                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="6%"><strong>
                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="6%"><strong>
                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ChangJia%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="7%"><strong>
                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="7%"><strong>
                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ChuKuRen%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="8%"><strong>
                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ChuKuYuanYin%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="6%"><strong>
                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ChuKuShiJian%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="5%"><strong>
                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,LaiYuan%>"></asp:Label>
                                                                </strong></td>
                                                                <td align="left" width="5%"><strong>ID</strong> </td>
                                                                <td align="left" width="5%"><strong>CODE</strong> </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right" width="6">
                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" ShowHeader="false" Width="100%">
                                                <Columns>
                                                    <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="GoodsCode" HeaderText="物料代码">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="GoodsName" HeaderText="MaterialName">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="UnitName" HeaderText="Unit">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ShipmentNO" HeaderText="出库单号">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                    </asp:BoundColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="OperatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}" DataTextField="OperatorName" HeaderText="出库人" Target="_blank">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                    </asp:HyperLinkColumn>
                                                    <asp:BoundColumn DataField="ApplicationReason" HeaderText="出库原因">
                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ShipTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="出库时间">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RelatedType" HeaderText="来源">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RelatedID" HeaderText="相关ID">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RelatedCode" HeaderText="相关CODE">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                    </asp:BoundColumn>
                                                </Columns>

                                                <ItemStyle CssClass="itemStyle" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditItemStyle BackColor="#2461BF" />
                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle CssClass="notTab" Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" />
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                            </table>
                            <!--endprint1-->
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BT_Export" />
                </Triggers>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
