<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTKPIManagement.aspx.cs"
    Inherits="TTKPIManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1100px;
            width: expression (document.body.clientWidth <= 1100? "1100px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }



        });

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
                        <table cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,WoDeJiXiao%>"></asp:Label>
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
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="tdFullBorder" style="padding-right: 5px; padding-top: 5px; font-weight: bold; color: #394f66; background-image: url('ImagesSkin/titleBG.jpg');"
                                                align="right">
                                                <asp:Button ID="BT_AllKPI" runat="server" CssClass="inpuLong" OnClick="BT_AllKPI_Click"
                                                    Text="<%$ Resources:lang,SuoYouJiXiao%>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; padding: 5px 5px 5px 5px;" align="left">
                                                <cc2:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                                    Width="100%">
                                                    <cc2:TabPanel ID="TabPanel1" runat="server" HeaderText="">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ZhiJieChengYuanJiXiao%>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <table style="width: 100%; height: 1px">
                                                                <tr>
                                                                    <td style="text-align: left;">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,DaiWoShenHeDeJiXiao %>"></asp:Label>：
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;">
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                            width="100%">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="25%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JiXiaoMingCheng %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,GuiShu %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,KaiShiShiJian %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,JieShuShiJian %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZiPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShangJiPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,DiSanFangPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ZongPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="right" width="6">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnPageIndexChanged="DataGrid2_PageIndexChanged"
                                                                            PageSize="6" ShowHeader="False" Width="100%">

                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="KPICheckID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="KPICheckID" DataNavigateUrlFormatString="TTKPILeaderReview.aspx?KPICheckID={0}"
                                                                                    DataTextField="KPICheckName" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="UserCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="UserName" HeaderText="受考人" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="StartTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="EndTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalSelfPoint" HeaderText="SelfAssessmentScore">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalLeaderPoint" HeaderText="上级评分">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalThirdPartPoint" HeaderText="ThirdPartyAssessmentScore">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalPoint" HeaderText="总评分">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid><asp:Label ID="LB_Sql2" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left;">
                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,WoShenHeGuoDeJiXiao %>"></asp:Label>：
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;">
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                            width="100%">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="25%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,JiXiaoMingCheng %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,GuiShu %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,KaiShiShiJian %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,JieShuShiJian %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ZiPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ShangJiPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,DiSanFangPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,ZongPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="right" width="6">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid5" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnPageIndexChanged="DataGrid5_PageIndexChanged"
                                                                            PageSize="6" ShowHeader="False" Width="100%">

                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="KPICheckID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="KPICheckID" DataNavigateUrlFormatString="TTKPILeaderReview.aspx?KPICheckID={0}"
                                                                                    DataTextField="KPICheckName" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="UserCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="UserName" HeaderText="受考人" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="StartTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="EndTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalSelfPoint" HeaderText="SelfAssessmentScore">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalLeaderPoint" HeaderText="上级评分">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalThirdPartPoint" HeaderText="ThirdPartyAssessmentScore">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalPoint" HeaderText="总评分">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid><asp:Label ID="LB_Sql5" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                    <cc2:TabPanel ID="TabPanel2" runat="server" HeaderText="">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,QiTaChengYuanJiXiao %>"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ContentTemplate>
                                                            <table style="width: 100%; height: 1px">
                                                                <tr>
                                                                    <td style="text-align: left;">
                                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,DaiWoShenHeDeJiXiao %>"></asp:Label>：
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;">
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                            width="100%">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="25%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,JiXiaoMingCheng %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,GuiShu %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,KaiShiShiJian %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,JieShuShiJian %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,ZiPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ShangJiPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,DiSanFangPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,ZongPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="right" width="6">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid4" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnPageIndexChanged="DataGrid4_PageIndexChanged"
                                                                            PageSize="6" ShowHeader="False" Width="100%">

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="KPICheckID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="KPICheckID" DataNavigateUrlFormatString="TTKPIThirdPartReview.aspx?KPICheckID={0}"
                                                                                    DataTextField="KPICheckName" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="UserCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="UserName" HeaderText="受考人" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="StartTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="EndTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalSelfPoint" HeaderText="SelfAssessmentScore">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalLeaderPoint" HeaderText="上级评分">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalThirdPartPoint" HeaderText="ThirdPartyAssessmentScore">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalPoint" HeaderText="总评分">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        </asp:DataGrid><asp:Label ID="LB_Sql4" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: left;">
                                                                        <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,WoShenHeGuoDeJiXiao %>"></asp:Label>：
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;">
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                            width="100%">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="25%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,JiXiaoMingCheng %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,GuiShu %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,KaiShiShiJian %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,JieShuShiJian %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,ZiPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,ShangJiPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,DiSanFangPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,ZongPingFen %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="right" width="6">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                                            PageSize="6" ShowHeader="False" Width="100%">

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="KPICheckID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="KPICheckID" DataNavigateUrlFormatString="TTKPIThirdPartReview.aspx?KPICheckID={0}"
                                                                                    DataTextField="KPICheckName" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="UserCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="UserName" HeaderText="受考人" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="StartTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="EndTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalSelfPoint" HeaderText="SelfAssessmentScore">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalLeaderPoint" HeaderText="上级评分">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalThirdPartPoint" HeaderText="ThirdPartyAssessmentScore">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="TotalPoint" HeaderText="总评分">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        </asp:DataGrid><asp:Label ID="LB_Sql1" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc2:TabPanel>
                                                </cc2:TabContainer>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; padding-left: 10px;">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; padding-left: 10px;">
                                                <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,WoDeJiXiao %>"></asp:Label>：
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; padding: 5px 5px 5px 8px;">
                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                    width="96%">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td align="left" width="5%">
                                                                        <strong>
                                                                            <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="25%">
                                                                        <strong>
                                                                            <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,JiXiaoMingCheng %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="9%">
                                                                        <strong>
                                                                            <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,KaiShiShiJian %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="9%">
                                                                        <strong>
                                                                            <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,JieShuShiJian %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="8%">
                                                                        <strong>
                                                                            <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,ZiPingFen %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="8%">
                                                                        <strong>
                                                                            <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,ShangJiPingFen %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="8%">
                                                                        <strong>
                                                                            <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,DiSanFangPingFen %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="8%">
                                                                        <strong>
                                                                            <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,ZongPingFen %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="8%">
                                                                        <strong>
                                                                            <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left" width="9%">
                                                                        <strong>
                                                                            <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,ZhiDingZhe %>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right" width="6">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid3" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnPageIndexChanged="DataGrid3_PageIndexChanged"
                                                    PageSize="6" ShowHeader="False" Width="96%">

                                                    <Columns>
                                                        <asp:BoundColumn DataField="KPICheckID" HeaderText="Number">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="KPICheckID" DataNavigateUrlFormatString="TTMyKPICheckSet.aspx?KPICheckID={0}"
                                                            DataTextField="KPICheckName" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:BoundColumn DataField="StartTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="StartTime">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EndTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="EndTime">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TotalSelfPoint" HeaderText="SelfAssessmentScore">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TotalLeaderPoint" HeaderText="上级评分">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TotalThirdPartPoint" HeaderText="ThirdPartyAssessmentScore">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="TotalPoint" HeaderText="总评分">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                        </asp:TemplateColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="CreatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                            DataTextField="CreatorName" HeaderText="制定者" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                        </asp:HyperLinkColumn>
                                                    </Columns>
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                </asp:DataGrid><asp:Label ID="LB_Sql3" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
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
