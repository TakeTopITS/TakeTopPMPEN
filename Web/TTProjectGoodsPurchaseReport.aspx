<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectGoodsPurchaseReport.aspx.cs" Inherits="TTProjectGoodsPurchaseReport" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                     <div id="AboveDiv">
                    <table style="width: 980px;">
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,KaiShiShiJian%>"></asp:Label>��
                            </td>
                            <td>
                                <asp:TextBox ID="DLC_StartTime" ReadOnly="false" runat="server"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender3" runat="server" TargetControlID="DLC_StartTime">
                                </ajaxToolkit:CalendarExtender>

                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,JieShuShiJian%>"></asp:Label>��
                            </td>
                            <td>
                                <asp:TextBox ID="DLC_EndTime" ReadOnly="false" runat="server"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_EndTime">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="BT_Find" runat="server" OnClick="BT_Find_Click" Text="<%$ Resources:lang,ChaXun%>" CssClass="inpu" />
                                <a href="#" onclick="preview1()">
                                    &nbsp;</a></td>
                            <td><a href="#" onclick="preview1()">
                                <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" />
                                </a></td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td style="text-align: right;">&nbsp;
                            </td>
                            <td>
                                &nbsp;</td>
                            <td align="left">&nbsp;</td>
                            <td align="left">&nbsp;</td>
                        </tr>
                    </table>

                    <hr />
                    <!--startprint1-->
                    <table cellpadding="0" cellspacing="0" width="98%">
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 100%; height: 80px; font-size: xx-large; text-align: center;">
                                            <br />
                                            <asp:Label ID="LB_ReportName" runat="server" ></asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: left;">&nbsp;  &nbsp;
                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ZongShu%>"></asp:Label>��<asp:Label ID="LB_TotalNumber" runat="server"></asp:Label>
                                            &nbsp;
                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label>��<asp:Label ID="LB_TotalAmount" runat="server"></asp:Label>
                                            <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                            <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
                                            <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; text-align: center;">
                                            <table width="98%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                <tr>
                                                    <td width="7">
                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                    <td>
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="8%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="8%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="12%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="12%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="6%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="6%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="6%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                                </td>
                                                                <td width="8%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                                </td>

                                                                <td width="16%" align="left">
                                                                    <strong>
                                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ShiJian%>"></asp:Label></strong>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="6" align="right">
                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                </tr>
                                            </table>
                                            <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                                                Height="1px" Width="98%"
                                                CellPadding="4" ForeColor="#333333" GridLines="None">
                                                <Columns>
                                                    <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                    </asp:BoundColumn>
                                                    <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                        DataTextField="ApplicantName" HeaderText="Applicant" Target="_blank">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                    </asp:HyperLinkColumn>

                                                    <asp:BoundColumn DataField="PurTime" HeaderText="����ʱ��" DataFormatString="{0:yyyy/MM/dd}">
                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="16%" />
                                                    </asp:BoundColumn>
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
                            </td>
                        </tr>
                    </table>
                    <!--endprint1-->
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
