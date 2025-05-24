<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTRCJProjectWorkDetails.aspx.cs" Inherits="T_RCJProjectWorkDetails" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>¼��ʵ�ʹ���</title>
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

        function OpenWorkDetailsLog(url, w, h) {
            window.open(url, "newwindow", "height=h, width=w,toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
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
                                            <td align="left">
                                                <table width="145" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">ʵ�ʹ���¼��
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
                                                            <asp:LinkButton ID="LB_WorkDetailsLog" runat="server" OnClick="LB_WorkDetailsLog_Click">ʵ�ʹ������Ǽ���־</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="padding: 5px 5px 5px 5px" valign="top">
                                    <table align="left" border="1" cellpadding="0" cellspacing="0" width="96%">
                                        <tr>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 319px">
                                                    <tr>
                                                        <td align="right" width="100">������ţ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ID" runat="server" Width="207px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td align="right" width="100">��ţ�</td>
                                                        <td align="left" width="220">
                                                            <asp:TextBox ID="TB_ItemNo" runat="server" ReadOnly="True" Style="margin-left: 0px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td colspan="2">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="420px">
                                                    <tr>
                                                        <td align="right" width="100">�����·ݣ�</td>
                                                        <td align="left" width="320">
                                                            <asp:DropDownList ID="DDL_YearList" runat="server" AutoPostBack="True" DataValueField="ProjectYear" OnSelectedIndexChanged="DDL_YearList_SelectedIndexChanged"></asp:DropDownList>
                                                            <asp:DropDownList ID="DDL_MonthList" runat="server" DataValueField="ProjectMonth"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 319px">
                                                    <tr>
                                                        <td align="right" width="100">����ţ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemName" runat="server" Width="207px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td align="right" width="100">�������ƣ�</td>
                                                        <td align="left" width="220">
                                                            <asp:TextBox ID="TB_ItemContent" runat="server" Style="margin-left: 0px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left" colspan="2">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="420">
                                                    <tr>
                                                        <td align="right" width="100">��������</td>
                                                        <td align="left" width="320">
                                                            <asp:TextBox ID="TB_WorkNumDetails" runat="server" Style="margin-left: 0px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lb_ShowMessage" runat="server" Font-Size="Small" ForeColor="Red" Text="��Ϣ��ʾ����"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_SaveWorkDetails" runat="server" Text="����������¼" Width="146px" OnClick="BT_SaveWorkDetails_Click" />
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_EditWorkDetails" runat="server" Text="�޸Ĺ�����¼" Width="127px" OnClick="BT_EditWorkDetails_Click" />
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_DelWorkDetails" runat="server" Text="ɾ��������¼" Width="127px" OnClientClick="return confirm('ɾ���ü�¼���Ӧ�Ĺ���ȷ�ϼ�¼���տ��¼һ��ɾ������ȷ��ɾ���ü�¼��?')" OnClick="BT_DelWorkDetails_Click" />
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table id="Table1" cellpadding="0" width="100%" cellspacing="0" class="bian">
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
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">ʵ�ʹ�������Ϣ�б�
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="340" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">רҵ���ࣺ</td>
                                                        <td align="left" width="220">
                                                            <asp:DropDownList ID="DDL_PerformanceType" runat="server" Style="margin-left: 0px" AutoPostBack="True" OnSelectedIndexChanged="DDL_PerformanceType_SelectedIndexChanged"></asp:DropDownList>
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
                                                <asp:GridView ID="gvWorkDetails" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="gvWorkDetails_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvWorkDetails_PageIndexChanging" OnRowDataBound="gvWorkDetails_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:CommandField HeaderText="ѡ��" ShowSelectButton="True" />
                                                        <asp:BoundField DataField="ID" HeaderText="�������" />
                                                        <asp:BoundField DataField="WorkConfirmID" HeaderText="ȷ�����" />
                                                        <asp:BoundField DataField="ItemNo" HeaderText="���" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="�����" />
                                                        <asp:BoundField DataField="WorkYear" HeaderText="�������" />
                                                        <asp:BoundField DataField="WorkMonth" HeaderText="�����·�" />
                                                        <asp:BoundField DataField="WorkNum" HeaderText="������" DataFormatString="{0:N3}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="BCWP" HeaderText="ʵ�ʺϼƺϼ�(BCWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="BCRWP" HeaderText="Ӧ�տ��(BCRWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="PBCWP" HeaderText="ʵ�տ�(PBCWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ComfirmPercent" HeaderText="ȷ�ϱ���" DataFormatString="{0:p2}" />
                                                        <asp:BoundField DataField="RecievePercent" HeaderText="�տ����" DataFormatString="{0:p2}" />
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
                        <br />
                        <table id="Table3" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="145" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">ʵ�ʹ���ȷ��
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
                                <td align="left" style="padding: 5px 5px 5px 5px" valign="top">
                                    <table align="left" border="1" cellpadding="0" cellspacing="0" width="96%">
                                        <tr>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 319px">
                                                    <tr>
                                                        <td align="right" width="100">�����·ݣ�</td>
                                                        <td align="left" width="320">
                                                            <asp:DropDownList ID="DDL_YearListConfirm" runat="server" DataValueField="ProjectYear"></asp:DropDownList>
                                                            <asp:DropDownList ID="DDL_MonthListConfirm" runat="server" DataValueField="ProjectMonth"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td colspan="2">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="420px">
                                                    <tr>
                                                        <td align="right" width="100">ȷ�Ϲ�������</td>
                                                        <td align="left" width="320">
                                                            <asp:TextBox ID="TB_WorkNumConfirm" runat="server" Style="margin-left: 0px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lb_ShowConfirmMessage" runat="server" Font-Size="Small" ForeColor="Red" Text="��Ϣ��ʾ����"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_SaveWorkConfirm" runat="server" Text="��������ȷ��" Width="146px" OnClick="BT_SaveWorkConfirm_Click" />
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_EditWorkConfirm" runat="server" Text="�޸Ĺ���ȷ��" Width="127px" OnClick="BT_EditWorkConfirm_Click" />
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_DelWorkConfirm" runat="server" Text="ɾ������ȷ��" Width="127px" OnClientClick="return confirm('��ȷ��ɾ���ü�¼��?')" OnClick="BT_DelWorkConfirm_Click" />
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table id="Table2" cellpadding="0" width="100%" cellspacing="0" class="bian">
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
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">ȷ�Ϲ�������Ϣ�б�
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
                                                            <asp:LinkButton ID="LB_WorkConfirmLog" runat="server" OnClick="LB_WorkConfirmLog_Click">ȷ�Ϲ������Ǽ���־</asp:LinkButton>
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
                                                <asp:GridView ID="gvWorkConfirm" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="gvWorkConfirm_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvWorkConfirm_PageIndexChanging" OnRowDataBound="gvWorkConfirm_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:CommandField HeaderText="ѡ��" ShowSelectButton="True" />
                                                        <asp:BoundField DataField="ID" HeaderText="�������" />
                                                        <asp:BoundField DataField="ItemNo" HeaderText="���" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="�����" />
                                                        <asp:BoundField DataField="WorkYear" HeaderText="�������" />
                                                        <asp:BoundField DataField="WorkMonth" HeaderText="�����·�" />
                                                        <asp:BoundField DataField="WorkNum" HeaderText="������" DataFormatString="{0:N3}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceDeviceActual" HeaderText="�豸ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMainMaterialActual" HeaderText="����ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceWageActual" HeaderText="�˹�ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMaterialActual" HeaderText="����ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMachineActual" HeaderText="��еʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemComprehensiveFeeActual" HeaderText="ʵ�ʷ��ʴ�ʩ�Ѻϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemTaxesActual" HeaderText="˰��ʵ�ʺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ProjectBCRWP" HeaderText="Ӧ�տ��(BCRWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ProjectPBCWP" HeaderText="ʵ�տ�(PBCWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
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
                        <br />
                        <table id="Table5" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="145" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�տ����¼
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
                                <td align="left" style="padding: 5px 5px 5px 5px" valign="top">
                                    <table align="left" border="1" cellpadding="0" cellspacing="0" width="96%">
                                        <tr>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 319px">
                                                    <tr>
                                                        <td align="right" width="100">�����·ݣ�</td>
                                                        <td align="left" width="320">
                                                            <asp:DropDownList ID="DDL_YearListMoney" runat="server" DataValueField="ProjectYear"></asp:DropDownList>
                                                            <asp:DropDownList ID="DDL_MonthListMoney" runat="server" DataValueField="ProjectMonth"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td colspan="2">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="420px">
                                                    <tr>
                                                        <td align="right" width="100">�տ����</td>
                                                        <td align="left" width="320">
                                                            <asp:TextBox ID="TB_WorkNumMoney" runat="server" Style="margin-left: 0px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lb_ShowMoneyMessage" runat="server" Font-Size="Small" ForeColor="Red" Text="��Ϣ��ʾ����"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_SaveWorkMoney" runat="server" Text="�����տ��¼" Width="146px" OnClick="BT_SaveWorkMoney_Click" />
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_EditWorkMoney" runat="server" Text="�޸��տ��¼" Width="127px" OnClick="BT_EditWorkMoney_Click" />
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="BT_DelWorkMoney" runat="server" Text="ɾ���տ��¼" Width="127px" OnClientClick="return confirm('��ȷ��ɾ���ü�¼��?')" OnClick="BT_DelWorkMoney_Click" />
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
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
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�տ����Ϣ�б�
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
                                                            <asp:LinkButton ID="LB_QueryWorkMoneyLog" runat="server" OnClick="LB_QueryWorkMoneyLog_Click">�տ�Ǽ���־</asp:LinkButton>
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
                                                <asp:GridView ID="gvMoneyConfirm" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="gvMoneyConfirm_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvMoneyConfirm_PageIndexChanging" OnRowDataBound="gvMoneyConfirm_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:CommandField HeaderText="ѡ��" ShowSelectButton="True" />
                                                        <asp:BoundField DataField="ID" HeaderText="�������" />
                                                        <asp:BoundField DataField="ItemNo" HeaderText="���" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="�����" />
                                                        <asp:BoundField DataField="WorkYear" HeaderText="�������" />
                                                        <asp:BoundField DataField="WorkMonth" HeaderText="�����·�" />
                                                        <asp:BoundField DataField="MoneyNum" HeaderText="ʵ�տ�(PBCWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
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
