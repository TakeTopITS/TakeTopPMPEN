<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAPPPersonalSpaceAnalysisChart.aspx.cs" Inherits="TTAPPPersonalSpaceAnalysisChart" %>

<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; minimum-scale=0.1; user-scalable=1" />
<%--<%@ OutputCache Duration="2678400" VaryByParam="*" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style>
        html {
            overflow-x: hidden;
            overflow-y: hidden;
        }
    </style>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {

            /*  if (top.location != self.location) { } else { CloseWebPage(); }*/

        });

        function displayScroll() {

            document.getElementById("divRenyList").style.overflow = "auto";
        }

        function hideScroll() {

            document.getElementById("divRenyList").style.overflow = "hidden";
        }


    </script>

</head>
<body>
    <center>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <%--   <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="1000" />--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <%-- <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" />
                </Triggers>--%>
                <ContentTemplate>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top" align="center" style="padding: 5px 1px 1px 1px;" onmouseenter="javascript:displayScroll();" onmousemove="javascript:displayScroll();" onmouseover="javascript:displayScroll();" onmouseout="javascript:hideScroll();">
                                <div id="divRenyList" class="renyList" style="width: 100%; height: 175px; text-align: center; overflow: hidden;">
                                    <asp:Repeater ID="RP_ChartList" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="LB_ChartName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ChartName") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="LB_ChartType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ChartType") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="LB_SqlCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SqlCode") %>' Visible="false"></asp:Label>

                                            <iframe id="iframeChart" src="TTTakeTopAnalystChartSet.aspx?FormType=<%# DataBinder.Eval(Container.DataItem,"FormType") %>&ChartType=<%# DataBinder.Eval(Container.DataItem,"ChartType") %>&ChartName=<%# DataBinder.Eval(Container.DataItem,"ChartName") %>" style="width: 300px; height: 295px; border: 1px solid white; overflow: hidden;"></iframe>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>
                        </tr>
                    </table>
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
