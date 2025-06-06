<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAllUsers.aspx.cs" Inherits="TTAllUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1800px;
            width: expression (document.body.clientWidth <= 1800? "1800px" : "auto" ));
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
                                            <td align="left" width="175px">
                                                <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChaKanSuoYouChengYuan%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>：</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_UserCode" runat="server" Width="80px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_UserName" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：</td>
                                                        <td>
                                                            <asp:DropDownList ID="DL_Status" runat="server">
                                                                <asp:ListItem Value="Employed" Text="<%$ Resources:lang,ZaiZhi%>" />
                                                                <asp:ListItem Value="Resign" Text="<%$ Resources:lang,LiZhi%>" />
                                                                <asp:ListItem Value="Stop" Text="<%$ Resources:lang,ZhongZhi%>" />
                                                                <asp:ListItem />
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                        </td>
                                                        <td>&nbsp;<asp:Button ID="BT_ExportToExcel" runat="server" CssClass="inpu" Text="<%$ Resources:lang,DaoChuDaoExcel%>" OnClick="BT_ExportToExcel_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0" align="left">
                                        <tr>
                                            <td valign="top">
                                                <table style="font-size: 10pt; width: 100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                          <td style="width: 220px; padding: 5px 0px 0px 5px; border-left: solid 1px #D8D8D8"
                                                            valign="top" align="left">
                                                            <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                                                ShowLines="True" Width="220px">
                                                                <RootNodeStyle CssClass="rootNode" />
                                                                <NodeStyle CssClass="treeNode" />
                                                                <LeafNodeStyle CssClass="leafNode" />
                                                                <SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                                            </asp:TreeView>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="width: 100%; padding: 5px 5px 5px 10px; text-align: left;" valign="top">
                                                                        <asp:Label ID="LB_ProjectMemberOwner" runat="server"></asp:Label>
                                                                        &nbsp;
                                                                    <asp:Label ID="LB_UserNumber" runat="server"></asp:Label>
                                                                        &nbsp;&nbsp;<b><asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,Nan%>"></asp:Label>：</b><asp:Label ID="LB_MaleUserNumber" runat="server"></asp:Label>
                                                                        &nbsp; &nbsp;<b>
                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,Nv%>"></asp:Label>：</b><asp:Label ID="LB_FemaleUserNumber" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center; padding: 5px 5px 5px 5px;" valign="top">
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" alt="" />
                                                                                </td>
                                                                                <td>
                                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                        <tr>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="3%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,XingBie%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="3%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,NianLing%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BuMenDaiMa%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="7%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,BuMenMingCheng%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhiZe%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,BanGongDianHua%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="6%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,YiDongDianHua%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="10%" align="left">
                                                                                                <strong>EMail</strong>
                                                                                            </td>
                                                                                            <td width="10%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,GongZuoFanWei%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="7%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,JiaRuRiQi%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="5%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,QuanXian%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="5%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ShengRi%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="7%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ShenFenZhengHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td width="4%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ShunXuHao%>"></asp:Label></strong>
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
                                                                            ShowHeader="false" Height="1px" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                                            PageSize="20" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="UserCode" HeaderText="Code">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="UserCode" DataNavigateUrlFormatString="TTUserInforView.aspx?UserCode={0}"
                                                                                    DataTextField="UserName" HeaderText="Name" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:HyperLinkColumn>
                                                                                <asp:BoundColumn DataField="Gender" HeaderText="Gender">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="3%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Age" HeaderText="Age">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="3%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="DepartCode" HeaderText="DepartmentCode">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="DepartName" HeaderText="DepartmentName">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Duty" HeaderText="Responsibility">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="OfficePhone" HeaderText="OfficePhone">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="MobilePhone" HeaderText="MobilePhone">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="EMail" HeaderText="EMail">
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="WorkScope" HeaderText="ScopeOfWork">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="JoinDate" HeaderText="JoinDate" DataFormatString="{0:yyyy/MM/dd}">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="4%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:TemplateColumn HeaderText="Permission">
                                                                                    <ItemTemplate>
                                                                                        <%# GetUserStatus(Eval("UserCode").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="BirthDay" HeaderText="生日" DataFormatString="{0:yyyy/MM/dd}">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="IDCard" HeaderText="IDNumber">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="SortNumber" HeaderText="SerialNumber">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>

                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        </asp:DataGrid>
                                                                        <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_UserCode" runat="server" Font-Bold="False" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_UserName" runat="server" Font-Bold="False" Visible="False"></asp:Label>
                                                                        <asp:Label ID="LB_DepartCode" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center; padding: 5px 5px 5px 5px;" valign="top">
                                                                        <br />
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                     <iframe runat="server" id="IFrame_Chart_MemberGender" src="TTTakeTopAnalystChartSet.aspx" style="width: 300px; height: 295px; border: 1px solid white; overflow: hidden;"></iframe>

                                                                                    <%--<asp:Chart ID="Chart_MemberGender" runat="server" Width="400px">
                                                                                        <Series>
                                                                                            <asp:Series ChartArea="ChartArea1" ChartType="Pie" Legend="Legend1" Name="Series1">
                                                                                            </asp:Series>
                                                                                        </Series>
                                                                                        <ChartAreas>
                                                                                            <asp:ChartArea AlignmentOrientation="Horizontal" Name="ChartArea1">
                                                                                            </asp:ChartArea>
                                                                                        </ChartAreas>
                                                                                        <Legends>
                                                                                            <asp:Legend Name="Legend1">
                                                                                                <CellColumns>
                                                                                                    <asp:LegendCellColumn Name="Column2">
                                                                                                        <Margins Left="15" Right="15" />
                                                                                                    </asp:LegendCellColumn>
                                                                                                    <asp:LegendCellColumn ColumnType="SeriesSymbol" Name="Column3">
                                                                                                        <Margins Left="15" Right="15" />
                                                                                                    </asp:LegendCellColumn>
                                                                                                </CellColumns>
                                                                                            </asp:Legend>
                                                                                        </Legends>
                                                                                        <Titles>
                                                                                            <asp:Title Alignment="TopCenter" DockedToChartArea="ChartArea1" IsDockedInsideChartArea="False" Name="标题">
                                                                                            </asp:Title>
                                                                                        </Titles>
                                                                                    </asp:Chart>--%>

                                                                                </td>
                                                                                <td>

                                                                                     <iframe runat="server" id="IFrame_Chart_MemberStatus" src="TTTakeTopAnalystChartSet.aspx" style="width: 300px; height: 295px; border: 1px solid white; overflow: hidden;"></iframe>

                                                                                  <%--  <asp:Chart ID="Chart_MemberStatus" runat="server" Width="400px">
                                                                                        <Series>
                                                                                            <asp:Series ChartArea="ChartArea1" ChartType="Pie" Legend="Legend1" Name="Series1">
                                                                                            </asp:Series>
                                                                                        </Series>
                                                                                        <ChartAreas>
                                                                                            <asp:ChartArea AlignmentOrientation="Horizontal" Name="ChartArea1">
                                                                                            </asp:ChartArea>
                                                                                        </ChartAreas>
                                                                                        <Legends>
                                                                                            <asp:Legend Name="Legend1">
                                                                                                <CellColumns>
                                                                                                    <asp:LegendCellColumn Name="Column2">
                                                                                                        <Margins Left="15" Right="15" />
                                                                                                    </asp:LegendCellColumn>
                                                                                                    <asp:LegendCellColumn ColumnType="SeriesSymbol" Name="Column3">
                                                                                                        <Margins Left="15" Right="15" />
                                                                                                    </asp:LegendCellColumn>
                                                                                                </CellColumns>
                                                                                            </asp:Legend>
                                                                                        </Legends>
                                                                                        <Titles>
                                                                                            <asp:Title Alignment="TopCenter" DockedToChartArea="ChartArea1" IsDockedInsideChartArea="False" Name="标题">
                                                                                            </asp:Title>
                                                                                        </Titles>
                                                                                    </asp:Chart>--%>
                                                                                </td>
                                                                                <td>
                                                                                        <iframe runat="server" id="IFrame_Chart_MemberNumberTendency" src="TTTakeTopAnalystChartSet.aspx" style="width: 800px; height: 295px; border: 1px solid white; overflow: hidden;"></iframe>

                                                                                    <%--<asp:Chart ID="Chart_MemberNumberTendency" runat="server" ImageType="Png" Width="800px">
                                                                                        <Legends>
                                                                                            <asp:Legend Enabled="False" Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" Name="Default">
                                                                                            </asp:Legend>
                                                                                        </Legends>
                                                                                        <BorderSkin SkinStyle="Emboss" />
                                                                                        <Series>
                                                                                            <asp:Series BorderColor="180, 26, 59, 105" BorderWidth="3" ChartType="Line" Color="220, 65, 140, 240" MarkerSize="8" MarkerStyle="Circle" Name="Series1" ShadowColor="Black" ShadowOffset="2" XValueType="Double" YValueType="Double">
                                                                                            </asp:Series>
                                                                                        </Series>
                                                                                        <ChartAreas>
                                                                                            <asp:ChartArea Name="ChartArea1">
                                                                                                <AxisY LineColor="64, 64, 64, 64">
                                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                                </AxisY>
                                                                                                <AxisX LineColor="64, 64, 64, 64">
                                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                                </AxisX>
                                                                                            </asp:ChartArea>
                                                                                        </ChartAreas>
                                                                                    </asp:Chart>--%>

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
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BT_ExportToExcel" />
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
