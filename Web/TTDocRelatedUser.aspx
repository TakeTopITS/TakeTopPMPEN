<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="TTDocRelatedUser.aspx.cs"
    Inherits="TTDocRelatedUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload"
    TagPrefix="Upload" %>
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

       function AdjustDivHeight() {

            document.getElementById("Div_TreeView1").style.height = document.documentElement.clientHeight + "px";
        }

    </script>

     <script type="text/javascript">

         var disPostion = 0;

         function SaveScroll() {
             disPostion = Div_TreeView1.scrollTop;
         }

         function RestoreScroll() {
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
         }

         function EndRequestHandler(sender, args) {
             Div_TreeView1.scrollTop = disPostion;
         }
    </script>


</head>
<body>
    <center>
        <form id="form1" runat="server">
          <%--  <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True">--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server" >
            </asp:ScriptManager>    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="98%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 220px; border-right: solid 1px #D8D8D8; padding: 5px 0px 0px 5px"
                        valign="top" align="left">
                          <div id="Div_TreeView1" style="overflow: auto; height: 800px;">
                                <asp:TreeView ID="TreeView2" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged"
                                    ShowLines="True" Width="220px">
                                    <RootNodeStyle CssClass="rootNode" /><NodeStyle CssClass="treeNode" /><LeafNodeStyle CssClass="leafNode" /><SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                </asp:TreeView>
                         </div>
                    </td>
                    <td width="165px" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8;
                        vertical-align: top;">
                        <table style="width: 165px; height: 53px">
                            <tr>
                                <td style="width: 165; text-align: center;" valign="top">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                        <tr>
                                            <td width="7">
                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td align="left">
                                                            <strong>
                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,BuMenRenYuan%>"></asp:Label></strong>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="6" align="right">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid3_ItemCommand"
                                        Width="100%" Height="1px" CellPadding="4" ForeColor="#333333" GridLines="None"
                                        ShowHeader="False">
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <EditItemStyle BackColor="#2461BF" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                        
                                        <ItemStyle CssClass="itemStyle" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="������Ա��">
                                                <ItemTemplate>
                                                    <asp:Button ID="BT_UserCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>'
                                                        CssClass="inpu" />
                                                    <asp:Button ID="BT_UserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                                        CssClass="inpu" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Small"
                                                    Font-Strikeout="False" Font-Underline="False" />
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    </asp:DataGrid>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="left" width="415px" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8;
                        vertical-align: top;">
                        <asp:Repeater ID="RP_DocRelatedUser" runat="server" OnItemCommand="Repeater1_ItemCommand">
                            <ItemTemplate>
                                <asp:Button ID="BT_UserName" runat="server" CssClass="inpuRepeat" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                    Width="70px" />
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
            <asp:Label ID="LB_DocID" runat="server" Visible="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="position: absolute; left: 50%; top: 50%;">
        <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <img src="Images/Processing.gif" alt="Loading,please wait..." /></ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    </form>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
