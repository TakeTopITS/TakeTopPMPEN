<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAllCustomers.aspx.cs" Inherits="TTAllCustomers" %>


<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
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
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ChaKanSuoYouKeHu%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td align="left">（<asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>：
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_CustCode" runat="server" Width="120px"></asp:TextBox>
                                                                    </td>

                                                                    <td align="left">
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_CustName" runat="server" Width="120px"></asp:TextBox>
                                                                    </td>

                                                                    <td align="left">
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_IndustryTypeFind" runat="server" Width="120px"></asp:TextBox>
                                                                        <asp:DropDownList ID="DL_IndustryTypeFind" runat="server" AutoPostBack="True" DataTextField="Type"
                                                                            DataValueField="Type" OnSelectedIndexChanged="DL_IndustryTypeFind_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label>：
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_ContactPerson" runat="server" Width="120px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,FenGuanDaiLiShang%>"></asp:Label>
                                                                        ： </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_AgencyName" runat="server" Width="120px"></asp:TextBox>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,QuYu%>"></asp:Label>
                                                                        ： </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_AreaAddress" runat="server" Width="120px"></asp:TextBox>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ShengFen%>"></asp:Label>
                                                                        ： </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_State" runat="server" Width="120px"></asp:TextBox>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ChengShi%>"></asp:Label>
                                                                        ： </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TB_City" runat="server" Width="120px"></asp:TextBox>
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    <asp:Button ID="BT_FindType" runat="server" CssClass="inpu" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                                    </td>
                                                            </table>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px 5px 5px 5px;">
                                    <table cellpadding="0" cellspacing="0" width="98%">
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="15%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">&nbsp;
                                                                    </td>
                                                                    <td width="7%" align="left"><strong>&nbsp;</strong> </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left"><strong>
                                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,FenGuanDaiLiShang%>"></asp:Label>

                                                                    </strong></td>
                                                                    <td width="8%" align="left"><strong>
                                                                        <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,QuYu%>"></asp:Label>

                                                                    </strong></td>
                                                                    <td width="8%" align="left"><strong>
                                                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ShengFen%>"></asp:Label>

                                                                    </strong></td>
                                                                    <td width="8%" align="left"><strong>
                                                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,ChengShi%>"></asp:Label>

                                                                    </strong></td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,DianHua%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>EMail</strong>
                                                                    </td>
                                                                    <%-- <td width="10%" align="left">
                                                                        <strong>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="Label140" runat="server" Text="<%$ Resources:lang,QuYu%>"></asp:Label></td>
                                                                                    <td>
                                                                                        <asp:Button ID="BT_SortByAreaAddress" CssClass="inpuUpDown" runat="server" OnClick="BT_SortByAreaAddress_Click" />
                                                                                        <asp:Label ID="LB_UpDown" runat="server" Text="UP" Visible="false"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </strong>
                                                                    </td>--%>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,DiZhi%>"></asp:Label></strong>
                                                                    </td>
                                                                    <%-- <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,JianLiRiQi%>"></asp:Label></strong>
                                                                    </td>--%>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    ShowHeader="False" Height="1px" PageSize="25"
                                                    OnPageIndexChanged="DataGrid2_PageIndexChanged" Width="100%" CellPadding="4"
                                                    ForeColor="#333333" GridLines="None">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="CustomerCode" HeaderText="Code">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="CustomerCode" DataNavigateUrlFormatString="TTCustomerInfoView.aspx?CustomerCode={0}"
                                                            DataTextField="CustomerName" HeaderText="CustomerName" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:TemplateColumn HeaderText="补充信息">
                                                            <ItemTemplate>
                                                                <a href='TTRelatedDIYBusinessForm.aspx?RelatedType=Customer&RelatedID=<%# getCustomerID(Eval("CustomerCode").ToString()) %>&IdentifyString=<%# ShareClass.GetWLTemplateIdentifyString(ShareClass.getBusinessFormTemName("Customer", getCustomerID(Eval("CustomerCode").ToString()))) %>'>
                                                                    <asp:Label ID="LB_BCXX" runat="server" Text="<%$ Resources:lang,XiangGuanYeWuDan%>"></asp:Label>
                                                                </a>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <a href='TTCustomerQuestionHandleDetailFromCustomer.aspx?CustomerCode=<%# Eval("CustomerCode").ToString() %>'>
                                                                    <asp:Label ID="LB_KHFW" runat="server" Text="<%$ Resources:lang,ZZKeHuFuWu%>"></asp:Label>
                                                                </a>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="ContactName" HeaderText="联系人">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BelongAgencyName" HeaderText="分管代理商">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AreaAddress" HeaderText="区域">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="State" HeaderText="省份">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="City" HeaderText="城市">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Tel1" HeaderText="Telephone">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmailAddress" HeaderText="EMail">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                        </asp:BoundColumn>

                                                        <asp:BoundColumn DataField="RegistrationAddressCN" HeaderText="地址">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <%-- <asp:BoundColumn DataField="CreateDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="建立日期">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>--%>
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
                                    <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="LB_Sql2" runat="server" Visible="False"></asp:Label>
                                    <br />
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
