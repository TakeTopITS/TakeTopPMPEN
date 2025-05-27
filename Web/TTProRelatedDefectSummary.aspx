<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProRelatedDefectSummary.aspx.cs" Inherits="TTProRelatedDefectSummary" %>


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

            <table cellpadding="0" cellspacing="0" width="100%" class="bian">
                <tr>
                    <td height="31" class="page_topbj">
                        <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="29">
                                                <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                            </td>
                                            <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,XiangMuXiangGuanQueXian%>"></asp:Label>
                                            </td>
                                            <td width="5">
                                                <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table cellpadding="5" cellspacing="0" width="95%">
                            <tr>
                                <td style="height: 16px; padding: 5px 5px 5px 5px;" align="right">
                                    <a href="#" onclick="preview1()">
                                        <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" />
                                    </a></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <!--startprint1-->
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">
                                                <br />
                                                <strong>
                                                    <asp:Label ID="LB_ReportName" runat="server"></asp:Label>
                                                </strong>
                                                <br />
                                                   <br />
                                            </td>
                                        </tr>
                                    <tr>
                                            <td style="height: 16px; padding: 5px 5px 5px 5px;" align="center">
                                                <iframe runat="server" id="IFrame_Chart1" src="TTTakeTopAnalystChartSet.aspx" style="width: 800px; height: 295px; border: 1px solid white; overflow: hidden;"></iframe>


                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 16px; padding: 5px 5px 5px 5px;" align="center">
                                                <table width="980px" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="36%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,QueXianMingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,YaoQiuWanChengRiQi%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,LiXiangRiQi%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="8%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left">
                                                                        <strong>&nbsp;</strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid1" runat="server" AllowCustomPaging="True" AllowPaging="True"
                                                    AutoGenerateColumns="False" Height="1px" PageSize="30" Width="980px" CellPadding="4"
                                                    ShowHeader="false" ForeColor="#333333" GridLines="None">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="DefectID" HeaderText="Number">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DefectType" HeaderText="Type">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="DefectID" DataNavigateUrlFormatString="TTProRelatedDefectView.aspx?DefectID={0}"
                                                            DataTextField="DefectName" HeaderText="��������" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="36%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:BoundColumn DataField="DefectFinishedDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��������">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="MakeDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��������">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                            DataTextField="ApplicantName" HeaderText="Applicant" Target="_blank" Text="<%$ Resources:lang,ShenQingRen%>">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.DefectID", "TTDocumentTreeView.aspx?DocType=Defect&RelatedID={0}") %>'
                                                                    Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder"/></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
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

                                    <!--endprint1-->
                                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="LB_UserName" runat="server" Font-Size="9pt" Height="20px"
                                        Width="70px" Visible="False"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
