<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTCarManagement.aspx.cs"
    Inherits="TTCarManagement" %>

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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,CheLiangGuanLi%>"></asp:Label>
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
                                            <td>
                                                <table style="width: 100%; margin-top: 5px;">
                                                    <tr>
                                                        <td style="width: 100%; text-align: left;">
                                                            <span style="font-size: 10pt">
                                                                <asp:Label ID="Label324341" runat="server" Text="<%$ Resources:lang,YaoChuLiDeCheLiangShenQing%>"></asp:Label>：</span>
                                                            <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 5px 5px 5px 5px">
                                                            <table width="100%" cellpadding="0" cellspacing="0">
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
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="20%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ShiYou%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="12%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,YongCheBuMen%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="10%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="13%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YongCheShiJian%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="13%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,HuanCheShiJian%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="18%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,SuiCheRen%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
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
                                                                            Height="1px" ShowHeader="False" PageSize="15" OnPageIndexChanged="DataGrid2_PageIndexChanged"
                                                                            Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTCarAssignFormSingle.aspx?ApplyFormID={0}"
                                                                                    DataTextField="ApplyReason" HeaderText="用车原因">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="DepartName" HeaderText="用车部门">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="ApplicantName" HeaderText="Name" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="DepartTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="用车时间">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="BackTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="还车时间">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Attendant" HeaderText="随车人">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="18%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Label ID="LB_Sql1" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span style="font-size: 10pt">
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YiChuLiDeCheLiangShenQing%>"></asp:Label>：</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 5px 5px 5px 5px">
                                                            <table width="100%" cellpadding="0" cellspacing="0">
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
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="20%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShiYou%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="12%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,YongCheBuMen%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="10%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="13%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,YongCheShiJian%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="13%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,HuanCheShiJian%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="18%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,SuiCheRen%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                            Height="1px" ShowHeader="False" PageSize="15" OnPageIndexChanged="DataGrid2_PageIndexChanged"
                                                                            Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTCarAssignFormSingle.aspx?ApplyFormID={0}"
                                                                                    DataTextField="ApplyReason" HeaderText="用车原因">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="DepartName" HeaderText="Department">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="ApplicantName" HeaderText="Name" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="DepartTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="用车时间">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="BackTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="还车时间">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Attendant" HeaderText="随车人">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="18%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                </asp:TemplateColumn>
                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Label ID="LB_Sql2" runat="server" Visible="False"></asp:Label>
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
