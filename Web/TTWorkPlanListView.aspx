<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTWorkPlanListView.aspx.cs" Inherits="TTWorkPlanListView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1240px;
            width: expression (document.body.clientWidth <= 1240? "1240px" : "auto" ));
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
                        <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%></td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ShiShiJiHua%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px 5px 5px 5px;">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:HyperLink ID="HL_ProPlanGantt" runat="server" Enabled="False" Height="17px"
                                                    NavigateUrl="TTWorkPlanGanttForProject.aspx" Target="_blank" Width="55px">
                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,GanTeTu%>"></asp:Label>
                                                </asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="14%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,JiHuaNeiRong%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="6%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,FuZeRen%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="8%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ZhiDingShiJian%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="15%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ZhiYuan%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,YuSuan%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,BiaoZhunChengBen%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BiaoZhunJingDu%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShiJiJingDu%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,BuZhou%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="5%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,QianBuZhou%>"></asp:Label>
                                                                        </strong>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                    </tr>
                                                </table>

                                                <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                                    ShowHeader="false"
                                                    Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="PlanID" HeaderText="���">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PlanDetail" HeaderText="�ƻ�����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="14%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="LeaderCode" DataNavigateUrlFormatString="TTUserInforView.aspx?UserCode={0}"
                                                            DataTextField="Leader" HeaderText="����" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="6%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:BoundColumn DataField="BeginTime" HeaderText="��ʼʱ��" DataFormatString="{0:yyyy/MM/dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EndTime" HeaderText="����ʱ��" DataFormatString="{0:yyyy/MM/dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="MakeDate" HeaderText="�ƶ�ʱ��" DataFormatString="{0:yyyy/MM/dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Resource" HeaderText="��Դ">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Budget" HeaderText="Budget">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DefaultCost" HeaderText="��׼�ɱ�">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DefaultSchedule" HeaderText="��׼����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="ʵ�ʽ���">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LB_FinishPercent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FinishPercent")%> '></asp:Label>%
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="״̬">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="PlanID" HeaderText="����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PriorID" HeaderText="ǰ����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="5%" />
                                                        </asp:BoundColumn>

                                                    </Columns>

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; text-align: left;">
                                                <span style="font-size: 10pt; color: #ff9900"><span style="color: #ff0033">
                                                    </span><asp:Label
                                                        ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_PlanID" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td style="width: 100%; text-align: left;">
                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,DXZANDJLBSWCYLDJHHSBSJHTQ%>"></asp:Label><asp:Label ID="LB_Plan" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td style="width: 100%; height: 18px">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,JiHuaHao%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="10%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,XuHao%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="15%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="35%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZhuYaoGongZuo%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="15%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,KaiShiGongZuoShiJian%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                    <td width="15%" align="center">
                                                                        <strong>
                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,JieShuGongZuoShiJian%>"></asp:Label>
                                                                        </strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                    </tr>
                                                </table>

                                                <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False"
                                                    Width="100%" CellPadding="4" ShowHeader="false"
                                                    ForeColor="#333333" GridLines="None">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="PlanID" HeaderText="�ƻ���">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ID" HeaderText="���">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="UserCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                            DataTextField="UserName" HeaderText="����" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="15%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:BoundColumn DataField="MainWork" HeaderText="��Ҫ����">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="35%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="StartWorkDate" HeaderText="��ʼ����ʱ��" DataFormatString="{0:yyyy/MM/dd}">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="15%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EndWorkDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��������ʱ��">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Center" Width="15%" />
                                                        </asp:BoundColumn>
                                                    </Columns>

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle HorizontalAlign="Center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
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
