<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTHumanResourcesTotalTable.aspx.cs" Inherits="TTHumanResourcesTotalTable" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>项目人力资源表</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }
    </style>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }
            
        });
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XiangMuRenLiZiYuanBiao%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 5px;">
                                    <table align="left" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left">
                                                <table width="70%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="ddl_ProjectID" runat="server" DataTextField="ProjectName" DataValueField="ProjectID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DropDownList1" runat="server">
                                                                <asp:ListItem Value="ByTrade" Text="<%$ Resources:lang,AnGongZhong%>" />
                                                                <asp:ListItem Value="BySubgroup" Text="<%$ Resources:lang,AnZiZu%>" />
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" >
                                                            <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                            &nbsp;
                                                                                                <asp:Button ID="Button1" runat="server" CssClass="inpu" OnClick="Button1_Click" Text="<%$ Resources:lang,DaoChu%>" />
                                                            <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="font-size: 10pt">
                                            <td>
                                                <asp:Panel ID="Panel_1" runat="server">
                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                        width="100%">
                                                        <tr>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td width="40%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ZhiChengGongZhong%>"></asp:Label></strong></td>
                                                                        <td width="30%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,JiHuaRenShu%>"></asp:Label></strong></td>
                                                                        <td width="30%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ChuBeiRenShu%>"></asp:Label></strong></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                        OnPageIndexChanged="DataGrid1_PageIndexChanged" PageSize="5" Width="100%" ShowHeader="false">
                                                        <Columns>
                                                            <asp:BoundColumn DataField="WorkType" HeaderText="职称/工种">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="40%" />
                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="NumberUsedTotal" HeaderText="PlannedLaborHours">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="30%" />
                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="NumberAllTotal" HeaderText="ReservedLaborHours">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="30%" />
                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        
                                                        <ItemStyle CssClass="itemStyle" />
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    </asp:DataGrid>
                                                </asp:Panel>
                                                <asp:Panel ID="Panel_2" runat="server">
                                                    <table align="left" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td align="left" width="40%"><strong>
                                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,RenLiZiZu%>"></asp:Label></strong></td>
                                                                                    <td align="left" width="30%"><strong>
                                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,JiHuaRenShu%>"></asp:Label></strong></td>
                                                                                    <td align="left" width="30%"><strong>
                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ChuBeiRenShu%>"></asp:Label></strong></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnPageIndexChanged="DataGrid2_PageIndexChanged" PageSize="5" ShowHeader="False" Width="100%">
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="HumanSubgroups" HeaderText="HumanResourceSubgroup">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="40%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="NumberUsedTotal" HeaderText="PlannedLaborHours">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="30%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="NumberAllTotal" HeaderText="ReservedLaborHours">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="30%" />
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
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="Button1" />
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
