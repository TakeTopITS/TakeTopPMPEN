<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWZPayDetailList.aspx.cs" Inherits="TTWZPayDetailList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>������ϸ</title>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,FuKuanMingXi%>"></asp:Label> 
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
                                                <table style="width: 80%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td align="left" style="width: 45%; padding: 5px 5px 5px 5px;" class="formItemBgStyle" valign="top">
                                                            <table class="formBgStyle" width="40%">
                                                                <tr>
                                                                    <td style="text-align: left" class="formItemBgStyle">
                                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,JiHuaFuKuan%>"></asp:Label>  </td>
                                                                    <td style="text-align: left" class="formItemBgStyle">
                                                                        <asp:TextBox ID="TXT_PlanMoney" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td style="text-align: left; display: none;" class="formItemBgStyle">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,YongTu%>"></asp:Label> </td>
                                                                    <td style="text-align: left; display: none;" class="formItemBgStyle">
                                                                        <asp:DropDownList ID="DDL_UseWay" runat="server">
                                                                            <asp:ListItem Text="" Value=""/>
                                                                            <asp:ListItem Text="<%$ Resources:lang,FuCaiLiaoKuan%>" Value="�����Ͽ�"/>
                                                                            <asp:ListItem Text="<%$ Resources:lang,FuSheBeiKuan%>" Value="���豸��"/>
                                                                            <asp:ListItem Text="<%$ Resources:lang,FuQiTaKuan%>" Value="��������"/>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center" class="formItemBgStyle" colspan="4">
                                                                        <asp:Button ID="BT_Save" runat="server" Text="<%$ Resources:lang,BaoCun%>" CssClass="inpu" OnClick="BT_Save_Click" />&nbsp;
                                                                        <asp:Button ID="BT_Reset" runat="server" Text="<%$ Resources:lang,QuXiao%>" CssClass="inpu" OnClick="BT_Reset_Click" />&nbsp;
                                                                        <asp:Button ID="BT_MoreAdd" runat="server" Text="<%$ Resources:lang,HeTongBiaoZhu%>" CssClass="inpu" OnClick="BT_MoreAdd_Click" />&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 45%; padding: 5px 5px 5px 5px;" class="formItemBgStyle" valign="top">
                                                            <table class="formBgStyle">
                                                                <tr>
                                                                    <td class="formItemBgStyle" style="width: 180px">
                                                                        <asp:ListBox ID="LB_Pay" name="LB_Pay" runat="server" Width="180px" Height="300px"
                                                                            DataTextField="PayID" DataValueField="PayID" AutoPostBack="true" OnSelectedIndexChanged="LB_Pay_SelectedIndexChanged"></asp:ListBox>
                                                                    </td>
                                                                    <td class="formItemBgStyle">
                                                                        <div style="width: 1500px;">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                <tr>
                                                                                    <td width="7">
                                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,YingFuKuanID%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,QingKuanDanHao%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,BaoXiaoRiQi%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,HeTongBianHao%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,GongFanBianHao%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="left">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,GongYingShang%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="right">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,FuKuanJiHua%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,JieKuanRen%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                                <td width="8%" align="center">
                                                                                                    <strong>
                                                                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,FuKuanJingDu%>"></asp:Label> </strong>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td width="6" align="right">
                                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <asp:DataGrid ID="DG_PayDetail" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                                                CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="false"
                                                                                Width="100%" OnItemCommand="DG_PayDetail_ItemCommand">
                                                                                <Columns>
                                                                                    <asp:TemplateColumn>
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label> 
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="del">
                                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ShanChu%>"></asp:Label> </asp:LinkButton>&nbsp;
                                                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="edit">
                                                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label> </asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:BoundColumn DataField="PayID" HeaderText="Ӧ����ID">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="RequestCode" HeaderText="����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="CancelTime" HeaderText="��������">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="CompactCode" HeaderText="Contract Number">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="SupplierCode" HeaderText="�������">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                                                    </asp:BoundColumn>
                                                                                    <%--<asp:BoundColumn DataField="Supplier" HeaderText="Supplier">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                    </asp:BoundColumn>--%>
                                                                                    <asp:TemplateColumn>
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,GongYingShang%>"></asp:Label> 
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <%# ShareClass.StringCutByRequire(Eval("Supplier").ToString(), 190) %>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:BoundColumn DataField="PlanMoney" HeaderText="�ƻ�����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Right" Width="8%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Borrower" HeaderText="�����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="UseWay" HeaderText="��;">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="PayProcess" HeaderText="�������">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
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
                                                    <tr>
                                                        <td align="left" style="width: 45%; padding: 5px 5px 5px 5px;" class="formItemBgStyle" valign="top">
                                                            <table class="formBgStyle">
                                                                <tr>
                                                                    <td class="formItemBgStyle" colspan="2">
                                                                        <div style="width: 1500px;">
                                                                            <asp:DataGrid ID="DG_Request" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                                                                                CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="True"
                                                                                Width="100%" OnItemCommand="DG_Request_ItemCommand">
                                                                                <Columns>
                                                                                    <asp:TemplateColumn>
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label> 
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <input type="checkbox" name="cb_Request_Code" value='<%#Eval("RequestCode") %>' />&nbsp;
                                                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%#Eval("RequestCode") %>' CommandName="add">
                                                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,XinZeng%>"></asp:Label> </asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:BoundColumn DataField="RequestCode" HeaderText="����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="CompactCode" HeaderText="Contract Number">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="ProjectCode" HeaderText="��Ŀ����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="SupplierCode" HeaderText="�������">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="UseWay" HeaderText="��;">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="ActualMoney" HeaderText="ʵ�����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="RatioMoney" HeaderText="˰��">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Freight" HeaderText="�˷�">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="OtherObject" HeaderText="Other">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="BorrowMoney" HeaderText="�����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="RowNumber" HeaderText="�ϵ�����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Borrower" HeaderText="�����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="RequestTime" HeaderText="�������">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Approver" HeaderText="�������">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="CancelTime" HeaderText="��������">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="BeforePayMoney" HeaderText="Ԥ����">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="PayMoney" HeaderText="�Ѹ���">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Arrearage" HeaderText="Ƿ��">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Progress" HeaderText="Progress">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="IsPay" HeaderText="�����־">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="IsFinisth" HeaderText="��ɱ��">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="IsMark" HeaderText="ʹ�ñ��">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
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
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:HiddenField ID="HF_PayDetailID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
