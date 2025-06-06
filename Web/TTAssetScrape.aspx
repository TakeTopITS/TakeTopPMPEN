<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAssetScrape.aspx.cs" Inherits="TTAssetScrape" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
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
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZiChanBaoFei%>"></asp:Label>
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
                                            <td style="width: 220px; border-right: solid 1px #D8D8D8; padding: 5px 0px 0px 5px"
                                                valign="top" align="left">
                                                <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                                    ShowLines="True" Width="220px">
                                                    <RootNodeStyle CssClass="rootNode" />
                                                    <NodeStyle CssClass="treeNode" />
                                                    <LeafNodeStyle CssClass="leafNode" />
                                                    <SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                                </asp:TreeView>
                                            </td>
                                            <td valign="top" width="170" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; vertical-align: top;">
                                                <table width="100%" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,BuMenRenYuan%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    ForeColor="#333333" ShowHeader="false" GridLines="None" OnItemCommand="DataGrid3_ItemCommand"
                                                    Width="170px">
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="部门人员：">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BT_UserCode" runat="server" CssClass="inpu"  Text='<%# DataBinder.Eval(Container.DataItem,"UserCode").ToString().Trim() %>' />
                                                                <asp:Button ID="BT_UserName" runat="server" CssClass="inpu"  Text='<%# DataBinder.Eval(Container.DataItem,"UserName").ToString().Trim() %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </td>
                                            <td>
                                                <table style="width: 98%; margin-top: 5px">
                                                    <tr>
                                                        <td>
                                                            <table class="formBgStyle" cellpadding="3" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 15%;" class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>：
                                                                    </td>
                                                                    <td style="width: 20%; " class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_AssetCode" runat="server" Width="90px" Enabled="False"></asp:TextBox>
                                                                        <asp:Label ID="LB_AssetID" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                                                    </td>
                                                                    <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_AssetName" runat="server" Width="382px" Enabled="False" Height="20px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_Type" runat="server" Enabled="False" Width="90px"></asp:TextBox>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,YuanYongHuDaiMa%>"></asp:Label>：
                                                                    </td>
                                                                    <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_OldUserCode" runat="server" Width="89px" Enabled="False"></asp:TextBox>
                                                                        <asp:Label ID="LB_OldUserName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,BaoFeiShuLiang%>"></asp:Label>：
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_ScrapeNumber" runat="server" Width="79px">0.00</NickLee:NumberBox>
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">&nbsp;
                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft" colspan="2"  >&nbsp;
                                                                    <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_DepartString" runat="server" Visible="false"></asp:Label>
                                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Number" runat="server" Visible="false">0.00</NickLee:NumberBox>
                                                                        <asp:Label ID="LB_DepartCode" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_OwnerCode" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td rowspan="1"  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BaoFeiYuanYin%>"></asp:Label>：
                                                                    </td>
                                                                    <td colspan="4" rowspan="1"  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_ScrapeReason" runat="server" Height="56px" TextMode="MultiLine"
                                                                            Width="595px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td rowspan="1"  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,BaoFeiHouYongTu%>"></asp:Label>：
                                                                    </td>
                                                                    <td colspan="4" rowspan="1"  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_AfterUse" runat="server" Height="56px" TextMode="MultiLine" Width="595px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BaoFeiShiJian%>"></asp:Label>：
                                                                    </td>
                                                                    <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="DLC_ScrapeTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_ScrapeTime">
                                                                        </ajaxToolkit:CalendarExtender>

                                                                    </td>
                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,HuiShouJinE%>"></asp:Label>：
                                                                    </td>
                                                                    <td  class="formItemBgStyleForAlignLeft">
                                                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="TB_GetAmount" runat="server" Width="79px">0.00</NickLee:NumberBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 18px;" class="formItemBgStyleForAlignLeft">
                                                                        <span style="font-size: 9pt">
                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,JingBanRenDaiMa%>"></asp:Label>：</span>
                                                                    </td>
                                                                    <td colspan="4" style="height: 18px; " class="formItemBgStyleForAlignLeft">
                                                                        <asp:TextBox ID="TB_OperatorCode" runat="server" Width="100px"></asp:TextBox>
                                                                        <asp:Label ID="LB_OperatorName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 15px;" class="formItemBgStyleForAlignLeft"></td>
                                                                    <td colspan="2" style="height: 15px; " class="formItemBgStyleForAlignLeft">
                                                                        <asp:Button ID="BT_Scrape" runat="server" OnClick="BT_Scrape_Click" Text="<%$ Resources:lang,BaoFei%>" CssClass="inpu" />
                                                                        &nbsp;
                                                                    </td>
                                                                    <td style="height: 15px" class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_Sql4" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                    <td style="height: 15px; "  class="formItemBgStyleForAlignLeft">
                                                                        <asp:Button ID="BT_Reduce" runat="server" OnClick="BT_Reduce_Click" Text="<%$ Resources:lang,HuanYuanCiBaoFeiZiChan%>"
                                                                            CssClass="inpuLong" Enabled="False" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="height: 15px; text-align: left">
                                                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                                                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="在用资产">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,UsingAsset%>"></asp:Label>
                                                                                </HeaderTemplate>

                                                                                <ContentTemplate>
                                                                                    <table width="99%">
                                                                                        <tr>
                                                                                            <td style="text-align: left; height: 15px;">
                                                                                                <asp:Label ID="LB_AssetOwner" runat="server" Font-Bold="True" Visible="False" Width="450px"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left">
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                                    <tr>
                                                                                                        <td width="7">
                                                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                <tr>
                                                                                                                    <td width="8%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="5%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="12%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="5%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="5%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="10%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="5%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="5%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="5%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="10%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,YongHu%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="10%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ChangJia%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="10%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,DiZhi%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="10%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ShiJian%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                        <td width="6" align="right">
                                                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                                    ShowHeader="False" Height="1px" OnItemCommand="DataGrid1_ItemCommand" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                                                                    PageSize="14" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateColumn HeaderText="Number">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:BoundColumn DataField="AssetCode" HeaderText="Code">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:HyperLinkColumn DataNavigateUrlField="AssetCode" DataNavigateUrlFormatString="TTAssetInforView.aspx?AssetCode={0}"
                                                                                                            DataTextField="AssetName" HeaderText="Name" Target="_blank">
                                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                                                                                        </asp:HyperLinkColumn>
                                                                                                        <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                                        </asp:BoundColumn>

                                                                                                        <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="UnitName" HeaderText="Unit">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:HyperLinkColumn DataNavigateUrlField="OwnerCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                                            DataTextField="OwnerName" HeaderText="用户" Target="_blank">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                                        </asp:HyperLinkColumn>
                                                                                                        <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="Position" HeaderText="地址">
                                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="BuyTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="购买时间">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                                        </asp:BoundColumn>
                                                                                                    </Columns>

                                                                                                    <ItemStyle CssClass="itemStyle" />
                                                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                                                </asp:DataGrid>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </cc1:TabPanel>
                                                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="报废资产">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label513" runat="server" Text="<%$ Resources:lang,ScrapeAsset%>"></asp:Label>
                                                                                </HeaderTemplate>

                                                                                <ContentTemplate>
                                                                                    <table width="100%" align="left">
                                                                                        <tr>
                                                                                            <td style="text-align: left; height: 15px;">
                                                                                                <asp:Label ID="Label134345" runat="server" Font-Bold="True" Text="已报废资产："></asp:Label>
                                                                                                <asp:Label ID="LB_ID" runat="server" Visible="False"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                                    <tr>
                                                                                                        <td width="7">
                                                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                                <tr>
                                                                                                                    <td width="10%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="10%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="8%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="20%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="8%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="12%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,HuiShouJinE%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="12%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,CaoZuoZhe%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                    <td width="15%" align="left">
                                                                                                                        <strong>
                                                                                                                            <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,BaoFeiShiJian%>"></asp:Label></strong>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                        <td width="6" align="right">
                                                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <asp:DataGrid ID="DataGrid4" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                                    ShowHeader="False" Height="1px" OnItemCommand="DataGrid4_ItemCommand" OnPageIndexChanged="DataGrid4_PageIndexChanged"
                                                                                                    PageSize="14" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateColumn HeaderText="Number">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:BoundColumn DataField="AssetCode" HeaderText="Code">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:HyperLinkColumn DataNavigateUrlField="AssetCode" DataNavigateUrlFormatString="TTAssetInforView.aspx?AssetCode={0}"
                                                                                                            DataTextField="AssetName" HeaderText="Name" Target="_blank">
                                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                                        </asp:HyperLinkColumn>
                                                                                                        <asp:BoundColumn DataField="ScrapeNumber" HeaderText="Quantity">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:BoundColumn DataField="GetAmount" HeaderText="回收金额">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:HyperLinkColumn DataNavigateUrlField="OperatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                                            DataTextField="OperatorName" HeaderText="用户" Target="_blank">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                                                        </asp:HyperLinkColumn>
                                                                                                        <asp:BoundColumn DataField="ScrapeTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="报废时间">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                                                        </asp:BoundColumn>
                                                                                                    </Columns>

                                                                                                    <ItemStyle CssClass="itemStyle" />
                                                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                                                </asp:DataGrid>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </cc1:TabPanel>
                                                                        </cc1:TabContainer>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
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
