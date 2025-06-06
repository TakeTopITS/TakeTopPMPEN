<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTUserAttendanceReportSAAS.aspx.cs" Inherits="TTUserAttendanceReportSAAS" %>


<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }



        });

        function preview1() {
            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint1-->";
            eprnstr = "<!--endprint1-->";
            prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 18);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
            document.body.innerHTML = bdhtml;
            return false;
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
                        <table cellpadding="0" cellspacing="0" width="1680px">
                            <tr>
                                <td align="left">
                                    <table style="width: 1080px;">
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>
                                            </td>
                                            <td>


                                                <asp:TextBox ID="DLC_StartTime" ReadOnly="false" runat="server" Height="22px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_StartTime">
                                                </ajaxToolkit:CalendarExtender>

                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,JieSuShiJian%>"></asp:Label>
                                            </td>
                                            <td>

                                                <asp:TextBox ID="DLC_EndTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_EndTime">
                                                </ajaxToolkit:CalendarExtender>

                                            </td>
                                            <td align="right">&nbsp;<asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XianShiFangShi%>"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DL_DisplayType" runat="server">
                                                    <asp:ListItem Value="DisplayAbnormal" Text="<%$ Resources:lang,XianShiYiChang%>" />
                                                    <asp:ListItem Value="DisplayAll" Text="<%$ Resources:lang,XianShiQuanBu%>" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YuanGongMingChen%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TB_UserName" runat="server"></asp:TextBox>
                                            </td>
                                            <td style="text-align: right;">&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td align="right">
                                                <asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                            </td>
                                            <td align="left">
                                                <a href="#" onclick="preview1()">
                                                    <img src="ImagesSkin/print.gif" alt="打印" border="0" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: Left;" colspan="4">
                                                <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
                                                <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                            </td>
                                            <td style="text-align: center;"></td>
                                            <td align="left"></td>
                                        </tr>
                                    </table>

                                    <!--startprint1-->

                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">
                                                <br />

                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,YuanGongKaoQinBaoBiao%>"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td style="width: 100%;">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="4%" align="left">
                                                                        <strong>ID</strong>
                                                                    </td>

                                                                    <td width="4%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XingMing%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="4%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZhuGuan%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ChuQingRiQi%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZaoBanShangBanShiJian%>"></asp:Label></strong>
                                                                    </td>

                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ZaoBanXiaBanShiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ZhongBanShangBanShiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ZhongBanXiaBanShiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,WanBanShangBanShiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,WanBanXiaBanShiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,JiaBanShangBanShiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,JiaBanXiaBanShiJian%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="6%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,YunXiZuiDaJuLi%>"></asp:Label></strong>
                                                                        (<asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,Mi%>"></asp:Label>)
                                                                    </td>
                                                                    <td width="4%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,ChiDao%>"></asp:Label></strong>
                                                                        (<asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,Minute%>"></asp:Label>)
                                                                    </td>

                                                                    <td width="4%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZaoTui%>"></asp:Label></strong>
                                                                        (<asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,Minute%>"></asp:Label>)
                                                                    </td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,WeiZhi%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    ShowHeader="false" ForeColor="#333333" GridLines="None" AllowPaging="False" 
                                                    Width="100%">
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                    <ItemStyle CssClass="itemStyle" />
                                                    <Columns>
                                                        <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="LeaderName" HeaderText="主管">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AttendanceDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="考勤日期">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                        </asp:BoundColumn>

                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <%# Eval("MCheckIn").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("MCheckInAddress").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("MCheckInDistance").ToString().Trim () %> (m)
                                                              
                                                                 <br />
                                                                IsMust:<%# Eval("MCheckInIsMust").ToString().Trim () %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <%# Eval("MCheckOut").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("MCheckOutAddress").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("MCheckOutDistance").ToString().Trim () %> (m)
                                                                
                                                                <br />
                                                                IsMust:<%# Eval("MCheckOutIsMust").ToString().Trim () %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <%# Eval("ACheckIn").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("ACheckInAddress").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("ACheckInDistance").ToString().Trim () %> (m)
                                                                
                                                                  <br />
                                                                IsMust:<%# Eval("ACheckInIsMust").ToString().Trim () %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <%# Eval("ACheckOut").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("ACheckOutAddress").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("ACheckOutDistance").ToString().Trim () %> (m)
                                                                 
                                                                  <br />
                                                                IsMust:<%# Eval("ACheckOutIsMust").ToString().Trim () %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <%# Eval("NCheckIn").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("NCheckInAddress").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("NCheckInDistance").ToString().Trim () %> (m)
                                                                
                                                                  <br />
                                                                IsMust:<%# Eval("NCheckInIsMust").ToString().Trim () %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <%# Eval("NCheckOut").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("NCheckOutAddress").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("NCheckOutDistance").ToString().Trim () %> (m)
                                                               
                                                                  <br />
                                                                IsMust: <%# Eval("NCheckOutIsMust").ToString().Trim () %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <%# Eval("OCheckIn").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("OCheckInAddress").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("OCheckInDistance").ToString().Trim () %> (m)
                                                                 
                                                                    <br />
                                                                IsMust: <%# Eval("OCheckInIsMust").ToString().Trim () %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <%# Eval("OCheckOut").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("OCheckOutAddress").ToString().Trim () %>
                                                                <br />
                                                                <%# Eval("OCheckOutDistance").ToString().Trim () %> (m)
                                                                  
                                                                  <br />
                                                                IsMust: <%# Eval("OCheckOutIsMust").ToString().Trim () %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="LargestDistance" HeaderText="MaximumAllowedDistance">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="LateMinute" HeaderText="迟到分钟">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EarlyMinute" HeaderText="早退分钟">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>

                                                                <a href='TTUserAttendancePosition.aspx?MemberUserCode= <%# Eval("UserCode").ToString().Trim () %> &AttendanceTime=<%# Eval("AttendanceDate").ToString().Trim () %>' target="_blank">
                                                                    <img id="IMG_GPS" src="ImagesSkin/GPS.jpg" width="20" height="20" /></a>

                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                      <%--  <tr>
                                            <td style="width: 100%; text-align: Center;">
                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,Di%>"></asp:Label>：<asp:Label ID="LB_PageIndex" runat="server"></asp:Label>
                                                &nbsp;<asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,YeGong%>"></asp:Label>
                                                <asp:Label ID="LB_TotalPageNumber" runat="server"></asp:Label>
                                                &nbsp;<asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,Ye%>"></asp:Label>
                                            </td>
                                        </tr>--%>
                                    </table>

                                    <!--endprint1-->
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
