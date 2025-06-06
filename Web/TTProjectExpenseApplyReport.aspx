<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectExpenseApplyReport.aspx.cs"
    Inherits="TTProjectExpenseApplyReport" %>

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

                    <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                        <tr>
                            <td height="31" class="page_topbj">
                                <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="29">
                                                        <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%></td>
                                                    <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,JingFeiShenQingHuiZong%>"></asp:Label>
                                                        <asp:Label ID="LB_ProjectID" runat="server" Visible="False"></asp:Label>
                                                        <asp:Label ID="LB_Sql" runat="server" Font-Bold="False" Font-Italic="False" Height="16px"
                                                            Visible="False" Width="46px"></asp:Label>
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
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="200" valign="top" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; text-align: left;">
                                            <asp:TreeView ID="TreeView1" runat="server"
                                                NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" ShowLines="True"
                                                Width="220px">
                                                <RootNodeStyle CssClass="rootNode" />
                                                <NodeStyle CssClass="treeNode" />
                                                <LeafNodeStyle CssClass="leafNode" />
                                                <SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                            </asp:TreeView>
                                            <br />
                                        </td>
                                        <td style="vertical-align: top">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td colspan="3">
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="left" style="padding: 0px 5px 0px 5px; font-weight: bold; height: 24px; color: #394f66; background-image: url('ImagesSkin/titleBG.jpg')">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="background: #f0f0f0; text-align: left; width: 60
                                                                            %; height: 25px;"
                                                                                colspan="2">
                                                                                <asp:Label ID="LB_MyQueryScope" runat="server" Text="<%$ Resources:lang,MyQueryScope%>"></asp:Label>:<asp:Label ID="LB_QueryScope" runat="server"
                                                                                    Font-Names="Arial,宋体" Font-Size="9pt"></asp:Label>
                                                                            </td>
                                                                            <td style="background: #f0f0f0; text-align: right; width: 35%; height: 25px;" colspan="2">
                                                                                <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                                                &nbsp;
                                                                            <asp:Label ID="LB_UserName" runat="server" Font-Size="9pt"
                                                                                Visible="false"></asp:Label>
                                                                                <asp:Button ID="BT_AllMember" runat="server" CssClass="inpuLong" OnClick="BT_AllMember_Click"
                                                                                    Text="<%$ Resources:lang,SuoYouJingFeiShenQing%>" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" style="margin-top: 5px">
                                                                        <tr>
                                                                            <td>
                                                                                <table width="98%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                    <tr>
                                                                                        <td width="7">
                                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                        <td>
                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                <tr>

                                                                                                    <td width="8%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="10%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,XiangGuanLeiXing%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="8%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,XiangGuanID%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="26%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="15%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="10%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="15%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="8%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td width="6" align="right">
                                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                                <asp:DataGrid ID="DataGrid2" runat="server" AllowCustomPaging="True" AllowPaging="True"
                                                                                    AutoGenerateColumns="False" Height="1px" OnItemCommand="DataGrid2_ItemCommand" ShowHeader="false"
                                                                                    Width="98%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                                    <ItemStyle CssClass="itemStyle" />
                                                                                    <Columns>
                                                                                        <asp:TemplateColumn HeaderText="Number">
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:BoundColumn DataField="RelatedType" HeaderText="相关类型">
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="RelatedID" HeaderText="相关ID">
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="ExpenseName" HeaderText="Name">
                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="26%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                            DataTextField="ApplicantName" HeaderText="Applicant" Target="_blank">
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                        </asp:HyperLinkColumn>
                                                                                        <asp:BoundColumn DataField="PayBackTime" HeaderText="还款时间" DataFormatString="{0:yyyy/MM/dd}">
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:TemplateColumn HeaderText="Status">
                                                                                            <ItemTemplate>
                                                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                        </asp:TemplateColumn>
                                                                                    </Columns>
                                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                                </asp:DataGrid>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="height: 15px; text-align: left">
                                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,DuiYingGongZuoLiuLieBiao%>"></asp:Label>：
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="height: 15px;">
                                                                                <table width="98%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                    <tr>
                                                                                        <td width="7">
                                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                        <td>
                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                <tr>

                                                                                                    <td width="10%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="55%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="20%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="10%" align="left">
                                                                                                        <strong>
                                                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                                    </td>
                                                                                                    <td width="5%" align="left">
                                                                                                        <strong></strong>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td width="6" align="right">
                                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                                <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" Height="1px" ShowHeader="false"
                                                                                    PageSize="5" Width="98%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                                    <ItemStyle CssClass="itemStyle" />
                                                                                    <Columns>
                                                                                        <asp:BoundColumn DataField="WLID" HeaderText="Number">
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="WLName" HeaderText="Workflow">
                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="55%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:BoundColumn DataField="CreateTime" HeaderText="申请时间">
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                                        </asp:BoundColumn>
                                                                                        <asp:TemplateColumn HeaderText="Status">
                                                                                            <ItemTemplate>
                                                                                                <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                        </asp:TemplateColumn>
                                                                                        <asp:TemplateColumn>
                                                                                            <ItemTemplate>
                                                                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=WorkFlow&WLID={0}") %>'
                                                                                                    Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder"/></asp:HyperLink>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                        </asp:TemplateColumn>
                                                                                    </Columns>
                                                                                </asp:DataGrid>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="text-align: left">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <strong>
                                                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,FeiYongHuiZong%>"></asp:Label>: </strong>
                                                                                            <asp:Label ID="LB_Member" runat="server" Width="100px"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ShenQingFeiYongZongE%>"></asp:Label>:<asp:Label ID="LB_Amount" runat="server" Font-Bold="True" Width="93px"></asp:Label>
                                                                                        </td>
                                                                                        <td>(<asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,XiangMuZongYuSuan%>"></asp:Label>:
                                                                                        <asp:Label ID="LB_ProBudget" runat="server" Font-Bold="True" Width="115px"></asp:Label>)
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
                                            <br />
                                        </td>
                                    </tr>
                                </table>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
