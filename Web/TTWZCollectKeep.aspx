<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZCollectKeep.aspx.cs" Inherits="TTWZCollectKeep" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>���ϵ��б�</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script language="javascript">
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ShouLiaoDanBaoGuanYuan%>"></asp:Label>
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
                                <td style="padding: 0px 5px 5px 5px;" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="top" style="padding-top: 5px;">
                                                <div style="width: 3300px;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label75" runat="server" Text="<%$ Resources:lang,JinDu%>"></asp:Label>��
                                               <asp:DropDownList ID="DDL_Progress" runat="server">
                                                   <%--    <asp:ListItem Text="<%$ Resources:lang,CaiJian%>" Value="�ļ�" />--%>
                                                   <asp:ListItem Text="" Value="" />

                                                   <asp:ListItem Text="<%$ Resources:lang,KaiPiao%>" Value="��Ʊ" />
                                                   <asp:ListItem Text="<%$ Resources:lang,ShangZhang%>" Value="�ļ�" />


                                               </asp:DropDownList>&nbsp;
                                                          
                                                            
                                                            <asp:Button ID="BT_Search" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_Search_Click" />


                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                            </td>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="2%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ShouLiaoDanHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,HeTongBianHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XiangMuBianMa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,WuZiDaiMa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,WuZiMingCheng%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,KuBie%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,KaiPiaoRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShouLiaoFangShi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,YingShouShuLiang%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShiShouShuLiang%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ShiGouDanJia%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ShiGouJinE%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShuiLv%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ShuiJin%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,YunFei%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,QiTa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,HuanSuanShuLiang%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,GongFangBianHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,FaPiaoHaoMa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,JianHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,CaiJianYuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,CaiJianRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,BaoGuanYuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ShouLiaoRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="2%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,HeTongYuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,QingKuanDanHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,CaiWuShenHe%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,BaoXiaoJinDu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="2%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,JinDu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,ShiYongBiaoJi%>"></asp:Label></strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DG_Collect" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="false"
                                                        Width="100%" OnItemCommand="DG_Collect_ItemCommand">
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="2%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CollectCode") %>' CommandName="account" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="��Ʊ" ? true : false %>'>����</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CollectCode") %>' CommandName="notAccount" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="����" ? true : false %>'>��Ʊ</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CollectCode") %>' CommandName="print" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="����" ? true : false %>'>��ӡ</asp:LinkButton>

                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="CollectCode" HeaderText="���ϵ���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CompactCode" HeaderText="��ͬ���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ProjectCode" HeaderText="��Ŀ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="PlanDetailID" HeaderText="�ƻ����" Visible="false">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:BoundColumn DataField="ObjectCode" HeaderText="���ʴ���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="ObjectName" HeaderText="��������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,WuZiMingCheng%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("ObjectName").ToString(), 8) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="StoreRoom" HeaderText="���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="TicketTime" HeaderText="��Ʊ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,KaiPiaoRiQi%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%#DataBinder.Eval(Container.DataItem, "TicketTime", "{0:yyyy/MM/dd}")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="CollectMethod" HeaderText="���Ϸ�ʽ">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CollectNumber" HeaderText="Ӧ������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ActualNumber" HeaderText="ʵ������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ActualPrice" HeaderText="ʵ������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ActualMoney" HeaderText="ʵ�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Ratio" HeaderText="˰��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="RatioMoney" HeaderText="˰��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Freight" HeaderText="�˷�">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="OtherObject" HeaderText="Other">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ConvertNumber" HeaderText="��������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SupplierName" HeaderText="�������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="TicketNumber" HeaderText="��Ʊ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="CheckCode" HeaderText="���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="3%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="3%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,JianHao%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("CheckCode").ToString(), 5) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="CheckerName" HeaderText="�ļ�Ա">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="CheckTime" HeaderText="�ļ�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,CaiJianRiQi%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("CheckTime").ToString(), 190) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="SafekeeperName" HeaderText="����Ա">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="CollectTime" HeaderText="��������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,ShouLiaoRiQi%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("CollectTime").ToString(), 190) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="ContacterName" HeaderText="��ͬԱ">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="2%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="RequestCode" HeaderText="����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="FinanceApproveName" HeaderText="�������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PayProcess" HeaderText="��������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Progress" HeaderText="����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="2%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="IsMark" HeaderText="ʹ�ñ��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                        <ItemStyle CssClass="itemStyle" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                    </asp:DataGrid>
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
