<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAssetPurchaseOrderDetailView.aspx.cs" Inherits="TTAssetPurchaseOrderDetailView" %>


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
        <form id="form2" runat="server">
            <div style="position: relative; top: 10px;">
                
                <!--startprint1-->
                <table style="width: 100%;">
                  
                    <tr>
                        <td style="text-align: center; width: 100%;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                <tr>
                                    <td width="7">
                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                    </td>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                </td>
                                                <td width="10%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                </td>
                                                <td width="8%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                </td>
                                                <td width="28%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,GouMaiLiYou%>"></asp:Label></strong>
                                                </td>
                                                <%--  <td width="6%" align="left">
                                                    <strong>
                                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="6" align="right">
                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid runat="server" AutoGenerateColumns="False" GridLines="None"
                                ShowHeader="false" Height="1px" Width="100%" ID="DataGrid1">
                                <Columns>
                                    <asp:BoundColumn DataField="ID" HeaderText="Number">
                                        <ItemStyle Horizontalalign="left" CssClass="itemBorder" Width="8%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Type" HeaderText="Type">
                                        <ItemStyle Horizontalalign="left" CssClass="itemBorder" Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="AssetName" HeaderText="Name">
                                        <ItemStyle Horizontalalign="left" CssClass="itemBorder" Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                        <ItemStyle Horizontalalign="left" CssClass="itemBorder" Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                        <ItemStyle Horizontalalign="left" CssClass="itemBorder" Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                        <ItemStyle Horizontalalign="left" CssClass="itemBorder" Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                        DataTextField="ApplicantName" Target="_blank" HeaderText="Applicant">
                                        <ItemStyle Horizontalalign="left" CssClass="itemBorder" Width="8%"></ItemStyle>
                                    </asp:HyperLinkColumn>
                                    <asp:BoundColumn DataField="PurReason" HeaderText="��������">
                                        <ItemStyle HorizontalAlign="Left" CssClass="itemBorder" Width="28%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <%--  <asp:TemplateColumn HeaderText="Status">
    <ItemTemplate>
        <%# ShareClass.GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
    </ItemTemplate>
    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="6%" />
</asp:TemplateColumn>--%>
                                </Columns>
                                <ItemStyle CssClass="itemStyle"></ItemStyle>
                                <PagerStyle Horizontalalign="center"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <!--endprint1-->
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>

