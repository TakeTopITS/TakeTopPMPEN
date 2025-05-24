<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTRCJProjectWorkConfirm.aspx.cs" Inherits="TTRCJProjectWorkConfirm" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ȷ��ʵ�ʹ���</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }
    </style>


    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

            aHandler();

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
                        <table id="Table4" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="180" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">ȷ�Ϲ������Ǽ���־
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="180" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">����</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="padding: 5px 5px 5px 5px" valign="top" width="100%">
                                    <table align="left" border="1" cellpadding="0" cellspacing="0" width="96%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" AllowPaging="True" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="�������" />
                                                        <asp:BoundField DataField="ItemNo" HeaderText="���" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="�����" />
                                                        <asp:BoundField DataField="WorkYear" HeaderText="�������" />
                                                        <asp:BoundField DataField="WorkMonth" HeaderText="�����·�" />
                                                        <asp:BoundField DataField="WorkNum" HeaderText="����ȷ����" DataFormatString="{0:N3}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ComprehensivePriceAdjust" HeaderText="�ۺ�ȡ�ѵ�����" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="TaxesPriceAdjust" HeaderText="˰�ѵ�����" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceDeviceActual" HeaderText="�豸ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMainMaterialActual" HeaderText="����ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceWageActual" HeaderText="�˹�ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMaterialActual" HeaderText="����ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMachineActual" HeaderText="��еʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemComprehensiveFeeActual" HeaderText="�ۺ�ȡ��ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemTaxesActual" HeaderText="˰��ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ChangedByWho" HeaderText="�Ǽ���Ա" />
                                                        <asp:BoundField DataField="ChangedTime" HeaderText="�Ǽ�ʱ��" />
                                                        <asp:BoundField DataField="ChangedMemo" HeaderText="˵��" />
                                                    </Columns>
                                                    <EditRowStyle BackColor="#999999" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" Horizontalalign="left" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                </asp:GridView>
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="position: absolute; left: 40%; top: 40%;">
                <asp:UpdateProgress ID="TakeTopUp" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <img src="Images/Processing.gif" alt="���Ժ򣬴�����..." />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </form>
    </center>
</body>
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>

