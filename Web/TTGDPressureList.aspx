<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGDPressureList.aspx.cs" Inherits="TTGDPressureList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>��ѹ���б�</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.7.2.min.js"></script>
    <script src="js/allAHandler.js"></script>
    <script type="text/javascript">

        //�༭
        function AlertAddEditPage(url) {

            popShowByURLForFixedSize(url, '', 1200, 500);

        }

        function LoadProjectList() {
            //alert("���óɹ�");
            document.getElementById("BT_RelaceLoad").click();
        }


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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ShiYaBaoHaoLieBiao%>"></asp:Label>
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
                                            <td align="left" class="formItemBgStyle">
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label>��<asp:DropDownList ID="DDL_GDProject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_GDProject_SelectedIndexChanged"></asp:DropDownList>&nbsp;
                                                <asp:Button ID="BT_Search" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_Search_Click" />&nbsp;
                                                <input type="button" class="inpuLong" value="������ѹ��" onclick="AlertAddEditPage('TTGDPressureEdit.aspx?id=');" />
                                                <%--<input type="button" class="inpuLong" value="������ѹ��" onclick="window.location.href = 'TTGDPressureEdit.aspx'" />--%>
                                                <asp:Button ID="BT_RelaceLoad" runat="server" Text="<%$ Resources:lang,ChongXinJiaZaiLieBiao%>" OnClick="BT_RelaceLoad_Click" CssClass="inpu" Style="display: none;" />
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
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,ShiYaBaoHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XuLieHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,FaBuRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShiYaJieZhi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,YaLiShiYan%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhuQuYu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,FenQuYu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YongTu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,XiTongHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,JieZhi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,GuanXianJianCha%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,LiShiJiLu%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="center">
                                                                            <strong>
                                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShiYaRiQi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="6%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong>
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
                                                        Width="100%" OnItemCommand="DG_List_ItemCommand" OnPageIndexChanged="DG_List_PageIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,CaoZuo%>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"PressureCode") %>' CommandName="edit" CssClass="notTab">
                                                                        <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,BianJi%>"></asp:Label>
                                                                    </asp:LinkButton>
                                                                    <%--<a href='TTGDPressureEdit.aspx?id=<%# DataBinder.Eval(Container.DataItem,"PressureCode") %>'>�༭</a>--%>
                                                                    <asp:LinkButton ID="LB_Del" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"PressureCode") %>' CommandName="del" CssClass="notTab">
                                                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ShanChu%>"></asp:Label>
                                                                    </asp:LinkButton>
                                                                    <%--<asp:Button ID="Button1" CssClass="inpu" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"PressureCode") %>' CommandName="del" Text="Deleted" />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="PressureCode" HeaderText="��ѹ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="OrderNumber" HeaderText="���к�">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PublicTime" HeaderText="��������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PressureMedium" HeaderText="��ѹ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PressureTest" HeaderText="ѹ������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="MainArea" HeaderText="������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PointArea" HeaderText="������">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PressureUser" HeaderText="��;">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="SystemCode" HeaderText="ϵͳ��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Medium" HeaderText="����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PipelineCheck" HeaderText="���߼��">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="HistoryRecord" HeaderText="��ʷ��¼">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="PressureTime" HeaderText="��ѹ����">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Remarks" HeaderText="Remark">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
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
