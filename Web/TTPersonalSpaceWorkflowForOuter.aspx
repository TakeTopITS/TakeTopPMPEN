<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTPersonalSpaceWorkflowForOuter.aspx.cs" Inherits="TTPersonalSpaceWorkflowForOuter" %>

<%@ OutputCache Duration="2678400" VaryByParam="*" %>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Import Namespace="System.Globalization" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <link id="tabCss" href="css/tab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="css/tab.js"></script>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }



        });
    </script>
    <style type="text/css">
        .auto-style1 {
            margin-top: 0px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="1000" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" />
            </Triggers>
            <ContentTemplate>
                <div class="renyList" style="width: 100%; height: 300px; overflow: auto;">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top" style="padding: 1px 1px 1px 10px;">
                                <div class="nTabr">
                                    <!-- ������a?a��? -->
                                    <div class="TabTitle">
                                        <ul id="myTab3r">
                                            <li class="active" onmouseover="nTabs(this,0);" style="display: block; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                                <asp:Label ID="LB_ToBeApproved" runat="server" Text="<%$ Resources:lang,ToBeApproved%>"></asp:Label>
                                            </li>
                                            <li class="normal" onmouseover="nTabs(this,1);" style="display: block; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                                <asp:Label ID="LB_MyWorkflow" runat="server" Text="<%$ Resources:lang,MyWorkflow%>"></asp:Label>
                                            </li>

                                        </ul>
                                    </div>
                                    <!-- ?����Y?a��? -->
                                    <div class="TabContent">
                                        <div id="myTab3r_Content0">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:DataGrid ID="DataGrid6" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            ShowHeader="False" CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                            OnPageIndexChanged="DataGrid6_PageIndexChanged" PageSize="8" Width="100%">

                                                            <Columns>
                                                                <asp:BoundColumn DataField="ID" HeaderText="����o?" Visible="false">
                                                                    <ItemStyle CssClass="dibian" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>

                                                                <asp:TemplateColumn HeaderText="1����¨���">
                                                                    <ItemTemplate>
                                                                        <a href="TTWorkFlowDetailMain.aspx?ID=<%#DataBinder .Eval (Container .DataItem ,"ID") %>">
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <%#DataBinder .Eval (Container .DataItem ,"WLName") %>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="font-size: small;">
                                                                                        <%#DataBinder .Eval (Container .DataItem ,"WorkDetail") %>
                                                                                    </td>
                                                                                </tr>

                                                                            </table>
                                                                        </a>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="dibian" HorizontalAlign="Left" Width="60%" />
                                                                </asp:TemplateColumn>

                                                                <asp:HyperLinkColumn DataNavigateUrlField="CreatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                    DataTextField="CreatorName" HeaderText="����??��?" Target="_blank">
                                                                    <ItemStyle CssClass="dibian" Horizontalalign="left" Width="20%" />
                                                                </asp:HyperLinkColumn>
                                                                <asp:TemplateColumn HeaderText="���䨬?">
                                                                    <ItemTemplate>
                                                                        <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="dibian" HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <EditItemStyle BackColor="#2461BF" />
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <ItemStyle CssClass="itemStyle" />
                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        </asp:DataGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="myTab3r_Content1" class="none">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <asp:DataGrid ID="DataGrid3" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            ShowHeader="false" Height="1px" OnPageIndexChanged="DataGrid3_PageIndexChanged"
                                                            PageSize="8" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                            <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="WLID" HeaderText="����o?" Visible="false">
                                                                    <ItemStyle CssClass="dibian" Horizontalalign="left" />
                                                                </asp:BoundColumn>
                                                                <asp:HyperLinkColumn DataNavigateUrlField="WLID" DataNavigateUrlFormatString="TTMyWorkDetailMain.aspx?WLID={0}&RelatedType=Null"
                                                                    DataTextField="WLName" HeaderText="Workflow" Target="_blank">
                                                                    <ItemStyle CssClass="didibian" HorizontalAlign="Left" Width="55%" />
                                                                </asp:HyperLinkColumn>
                                                                <asp:TemplateColumn HeaderText="���䨬?">
                                                                    <ItemTemplate>
                                                                        <%# ShareClass.GetLastestStepLastestOperator(Eval("WLID").ToString()) %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="dibian" HorizontalAlign="Left" Width="30%" />
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="���䨬?">
                                                                    <ItemTemplate>
                                                                        <%# ShareClass. GetWorkflowStatusByAuto(Eval("WLID").ToString()) %><%# ShareClass.GetStatusHomeNameByWorkflowStatus(Eval("Status").ToString()) %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="dibian" HorizontalAlign="right" Width="15%" />
                                                                </asp:TemplateColumn>
                                                            </Columns>

                                                            <ItemStyle CssClass="itemStyle" />
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <EditItemStyle BackColor="#2461BF" />
                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                        </asp:DataGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="clear"></div>
                                    </div>
                                </div>
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

</body>
<script type="text/javascript" language="javascript">
    var cssDirectory = '<%=Session["CssDirectory"] %>';

    var oLink = document.getElementById('mainCss');
    oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';

    var oTabLink = document.getElementById('tabCss');
    oTabLink.href = 'css/' + cssDirectory + '/' + 'tab.css';

</script>
</html>
