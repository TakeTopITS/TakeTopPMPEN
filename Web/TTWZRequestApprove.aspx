<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZRequestApprove.aspx.cs" Inherits="TTWZRequestApprove" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>��</title>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,QingKuanDan%>"></asp:Label>
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
                                                <div style="width: 2000px;">
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
                                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,QingKuanDanHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,HeTongBianHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XiangMuBianMa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,GongFangMingCheng%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,YongTu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="5%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ShiGouJinE%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="5%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShuiJin%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YunFei%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,QiTa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="5%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,JieKuanJinE%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,LiaoDanZhangShu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,JieKuanRen%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,QingKuanRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,CaiWuShenHe%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,BaoXiaoRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,YuFuKuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,YiFuKuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="right">
                                                                            <strong>
                                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,QianKuan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="3%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,JinDu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,FuKuanBiaoZhi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,WanChengBiaoJi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="4%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ShiYongBiaoJi%>"></asp:Label></strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DG_Request" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="false"
                                                        Width="100%" OnItemCommand="DG_Request_ItemCommand">
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>

                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%#Eval("RequestCode") %>' CommandName="approve" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="Claim for Payment" ? true : false %>'>���</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%#Eval("RequestCode") %>' CommandName="notApprove" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="Review" ? true : false %>'>�˻����</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%#Eval("RequestCode") %>' CommandName="account" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="Review" ? true : false %>'>����</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument='<%#Eval("RequestCode") %>' CommandName="notAccount" CssClass="notTab" Visible='<%# Eval("Progress").ToString()=="Reimbursement" ? true : false %>'>ȡ������</asp:LinkButton>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="RequestCode" HeaderText="����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CompactCode" HeaderText="Contract Number">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ProjectCode" HeaderText="��Ŀ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="SupplierName" HeaderText="��������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,GongFangMingCheng%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("SupplierName").ToString(), 8) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="UseWay" HeaderText="��;">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ActualMoney" HeaderText="ʵ�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="5%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="RatioMoney" HeaderText="˰��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="5%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Freight" HeaderText="�˷�">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="OtherObject" HeaderText="Other">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="BorrowMoney" HeaderText="�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="5%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="RowNumber" HeaderText="�ϵ�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="BorrowerName" HeaderText="�����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="RequestTime" HeaderText="�������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,QingKuanRiQi%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%#DataBinder.Eval(Container.DataItem, "RequestTime", "{0:yyyy/MM/dd}")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="ApproverName" HeaderText="�������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="CancelTime" HeaderText="��������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                            </asp:BoundColumn>--%>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,BaoXiaoRiQi%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# ShareClass.StringCutByRequire(Eval("CancelTime").ToString(), 190) %>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="BeforePayMoney" HeaderText="Ԥ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PayMoney" HeaderText="�Ѹ���">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Arrearage" HeaderText="Ƿ��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Progress" HeaderText="Progress">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="3%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="IsPay" HeaderText="�����־">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="IsFinisth" HeaderText="��ɱ��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="IsMark" HeaderText="ʹ�ñ��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="4%" />
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
