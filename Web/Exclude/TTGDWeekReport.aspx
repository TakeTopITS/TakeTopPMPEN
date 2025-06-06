<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGDWeekReport.aspx.cs" Inherits="TTGDWeekReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>每周工作量报告</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { 

            

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <div id="AboveDiv">
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,MeiZhouGongZuoLiangBaoGao%>"></asp:Label>
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
                                <td style="padding: 5px 5px 5px 5px;" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td  class="formItemBgStyleForAlignLeft">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>：<asp:TextBox ID="TXT_StartTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>&nbsp;
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,Zhi%>"></asp:Label>&nbsp;
                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>：<asp:TextBox ID="TXT_EndTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>&nbsp;
                                                <asp:Button ID="BT_Search" runat="server" CssClass="inpuLong" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_Search_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="padding-top: 5px;">
                                                <div style="width: 100%;">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                            </td>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                              
                                                                        <td width="30%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,QuYu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="30%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,HanJieXingShi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="40%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,CunKouHeJi%>"></asp:Label></strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DG_List" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="20" ShowHeader="false"
                                                        Width="100%" OnPageIndexChanged="DG_List_PageIndexChanged">
                                                        <Columns>
                                                      
                                                            <asp:BoundColumn DataField="Area" HeaderText="区域">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="30%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Mold" HeaderText="焊接形式">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="30%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="TotalSize" HeaderText="寸口合计">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="40%" />
                                                            </asp:BoundColumn>

                                                        </Columns>
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                        
                                                        <ItemStyle CssClass="itemStyle" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                    </asp:DataGrid>
                                                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
