<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTSystemAnalystChartRelatedUserSet.aspx.cs" Inherits="TTSystemAnalystChartRelatedUserSet" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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

        //ˢ�¸�ҳ��
        function reloadPrentPage() {

            parent.reloadPage();
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,WeDeYeWuFenXiTu%>"></asp:Label>
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
                                <td style="padding: 5px 5px 5px 5px;">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="180px" class="tdMain" align="left" valign="top">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,FenXiTu%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid1_ItemCommand"
                                                                Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="false"
                                                                Font-Bold="True">
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="left" />

                                                                <ItemStyle CssClass="itemStyle" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="">
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="BT_ChartName" runat="server" CssClass="inpu" Width="200px" Text='<%# DataBinder.Eval(Container.DataItem,"ChartName") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>

                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" HorizontalAlign="Left" ForeColor="White" />
                                                            </asp:DataGrid>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td class="tdMain" align="left" valign="top" style="border-right: solid 1px #D8D8D8; padding-top: 35px;">

                                                <table width="100%" border="0" class="formBgStyle" cellpadding="2" cellspacing="0">
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft" style="padding-top: 0px;">
                                                            <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid4_ItemCommand" Width="90%">
                                                                <Columns>
                                                                   
                                                                    <asp:TemplateColumn HeaderText="<%$ Resources:lang,BianHao%>">
                                                                        <ItemTemplate>
                                                                          <asp:Label ID="LB_ModuleID" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem,"ID") %> '></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Width="10%" />
                                                                        <ItemStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="left" />
                                                                    </asp:TemplateColumn>


                                                                    <asp:TemplateColumn HeaderText="<%$ Resources:lang,MingChengZhongWen%>">
                                                                        <ItemTemplate>
                                                                           <asp:Label ID="LB_ModuleName" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem,"ChartName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Width="45%" />
                                                                        <ItemStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="left" />
                                                                    </asp:TemplateColumn>

                                                                    <asp:TemplateColumn HeaderText="<%$ Resources:lang,ShongXuHao%>">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="TB_SortNumber" runat="server" Text="0" Width="40px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" Width="15%" />
                                                                        <ItemStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="left" />
                                                                    </asp:TemplateColumn>

                                                                    <asp:TemplateColumn HeaderText="">
                                                                        <ItemStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="left" />
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="BT_DeleteChart" runat="server" CommandName="DELETE" CssClass="inpu" Text="<%$ Resources:lang,ShanChu%>" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle BorderColor="#394F66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" />
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="left" />

                                                                <ItemStyle BackColor="#EFF3FB" CssClass="dg_item" />
                                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="left" />
                                                            </asp:DataGrid>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="2">

                                                            <table width="58%" border="0" class="formBgStyle" cellpadding="2" cellspacing="0">
                                                                <tr>
                                                                    <td class="formItemBgStyleForAlignRight">
                                                                        <asp:Button ID="BT_Save" runat="server" CssClass="inpu" OnClick="BT_Save_Click" Text="<%$ Resources:lang,BaoCun%>" />
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
