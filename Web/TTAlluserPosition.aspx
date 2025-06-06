<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAlluserPosition.aspx.cs" Inherits="TTAlluserPosition" %>


<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1500px;
            width: expression (document.body.clientWidth <= 1500? "1500px" : "auto" ));
        }
    </style>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }
           
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChaKanSuoYouChengYuanWeiZhi%>"></asp:Label>
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
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_UserCode" runat="server" Width="80px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>��</td>
                                                        <td>
                                                            <asp:TextBox ID="TB_UserName" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td>�ӣ�
                                                        </td>
                                                        <td style="width: 220px;" align="left">

                                                            <asp:TextBox ID="DLC_StartTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_StartTime">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,Dao%>"></asp:Label>��
                                                        </td>
                                                        <td style="width: 220px;" align="left">


                                                            <asp:TextBox ID="DLC_EndTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_EndTime">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="BT_Find" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                        </td>
                                                        <td>&nbsp;</td>
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
                                            <td width="950px" align="left">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 220px; padding: 5px 0px 0px 5px; border-left: solid 1px #D8D8D8"
                                                            valign="top" align="left">
                                                            <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                                                ShowLines="True" Width="220px">
                                                                <RootNodeStyle CssClass="rootNode" /><NodeStyle CssClass="treeNode" /><LeafNodeStyle CssClass="leafNode" /><SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                                            </asp:TreeView>
                                                        </td>
                                                        <td style="width: 180px; text-align: center" valign="top">
                                                            <table width="180px" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td width="5%" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="LB_DepartmentMember" runat="server" Text="<%$ Resources:lang,DepartmentMember%>"></asp:Label></strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid1_ItemCommand"
                                                                Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="false">
                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                <EditItemStyle BackColor="#2461BF" />
                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                <PagerStyle BackColor="#2461BF" ForeColor="White" Horizontalalign="left" />

                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Resources.lang.ZZXMZY">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="BT_UserCode" runat="server" CssClass="inpu" Style="text-align: center"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>' />
                                                                            <asp:Button ID="BT_UserName" runat="server" CssClass="inpu" Style="text-align: center"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </td>
                                                        <td style="width: 750px; text-align: center" valign="top">
                                                            <table width="750px" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                <tr>
                                                                    <td width="7">
                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td align="left" width="15%"><strong>
                                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,UserName%>"></asp:Label>
                                                                                </strong></td>
                                                                                <td align="left" width="150px"><strong>
                                                                                    <asp:Label ID="LB_CreateTime" runat="server" Text="<%$ Resources:lang,CreateTime%>"></asp:Label>
                                                                                </strong></td>
                                                                                <td align="left" width="250px"><strong>
                                                                                    <asp:Label ID="LB_UserPosition" runat="server" Text="<%$ Resources:lang,Position%>"></asp:Label>
                                                                                </strong></td>
                                                                                <td width="100px" align="left">
                                                                                    <strong>
                                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,MacAddress%>"></asp:Label></strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="6" align="right">
                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                ShowHeader="false" Height="1px" OnPageIndexChanged="DataGrid2_PageIndexChanged"
                                                                PageSize="30" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="UserName" HeaderText="Name">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="CreateTime" HeaderText="Time">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="150px" />
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="λ�ã�">
                                                                        <ItemTemplate>
                                                                            <a href='TTUserPositionView.aspx?lng=<%# DataBinder.Eval(Container.DataItem,"Longitude").ToString().Trim() %>&lat=<%# DataBinder.Eval(Container.DataItem,"Latitude").ToString().Trim() %>'
                                                                                target="IFR_UserPositionMap">
                                                                                <%# DataBinder.Eval(Container.DataItem,"Address")%></a>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="left" Width="240px" />
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="MacAddress" HeaderText="MacAddress">
                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="100px" />
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
                                                </table>
                                            </td>
                                            <%-- <td style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8;" valign="top">
                                                <iframe id="IFR_UserPositionMap" name="IFR_UserPositionMap" style="width: 100%; height:800px"></iframe>
                                            </td>--%>
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
