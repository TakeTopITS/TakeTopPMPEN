<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTRCJProjectAdjustPrice.aspx.cs" Inherits="TTRCJProjectAdjustPrice" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>�г��۸����</title>
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
                        <table id="Table1" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="145" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�۸�����б�
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
                                                            <asp:DropDownList ID="DDL_PerformanceType" runat="server" Style="margin-left: 0px" AutoPostBack="True" OnSelectedIndexChanged="DDL_PerformanceType_SelectedIndexChanged" Enabled="False"></asp:DropDownList>
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
                                    <table align="center" border="1" cellpadding="0" cellspacing="0" width="96%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" />
                                                        <asp:BoundField DataField="ID" HeaderText="��¼��" />
                                                        <asp:BoundField DataField="AdjustID" HeaderText="�������" />
                                                        <asp:BoundField DataField="ItemNo" HeaderText="��Ч��¼���" />
                                                        <asp:BoundField DataField="ItemNum" HeaderText="����" DataFormatString="{0:N3}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceDeviceAdjust" HeaderText="�豸��������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMainMaterialAdjust" HeaderText="���ĵ�������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceWageAdjust" HeaderText="�˹���������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMaterialAdjust" HeaderText="���ϵ�������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMachineAdjust" HeaderText="��е��������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ProjectBCWS" HeaderText="Ԥ��ϼƺϼ�(BCWS)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemTaxesBudget" HeaderText="Ԥ��˰��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceDeviceBudget" HeaderText="Ԥ���豸�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMainMaterialBudget" HeaderText="Ԥ�����ĺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceWageBudget" HeaderText="Ԥ���˹��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMaterialBudget" HeaderText="Ԥ����Ϻϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMachineBudget" HeaderText="Ԥ���е�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPricePurchaseFee" HeaderText="����Ѽ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPricePurchaseFeeBudget" HeaderText="���ʴ�ʩ��" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemComprehensiveFeeBudget" HeaderText="���" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="Memo" HeaderText="��ע" />
                                                    </Columns>
                                                    <EditRowStyle BackColor="#999999" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
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
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="145" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�۸��������
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
                                    <table align="center" border="1" cellpadding="0" cellspacing="0" width="96%">
                                        <tr>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">������ţ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemNo" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">����ţ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemName" runat="server" ReadOnly="True" Enabled="False" AutoPostBack="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td align="right" width="200">������ţ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_AdjustID" runat="server" ReadOnly="True" Enabled="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">������</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemNum" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">�豸�������ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_DeviceAdjust" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td align="right" width="200">���ĵ������ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_MainAdjust" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">�˹��������ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_WageAdjust" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">���ϵ������ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_MartAdjust" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td align="right" width="200">��е�������ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_MachineAdjust" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">Ԥ���豸�ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceDeviceBudget" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">����Ԥ��ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceMainMaterialBudget" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td align="right" width="200">�˹�Ԥ��ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceWageBudget" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">����Ԥ��ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ProjectMaterialBudget" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">��еԤ��ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ProjectMachineBudget" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td align="right" width="200">Ԥ��ϼƺϼ�(BCWS)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ProjectBCWS" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">���ʴ�ʩ�ѣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ItemPricePurchaseFeeBudget" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">��ѣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemComprehensiveFeeBudget" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td align="right" width="200">����Ѽ�����ѣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ItemPricePurchaseFee" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="280">
                                                    <tr>
                                                        <td align="right" width="120">Ԥ��˰��ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemTaxesBudget" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left" colspan="2">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="600">
                                                    <tr>
                                                        <td align="right" width="120">��ע��</td>
                                                        <td align="left" width="480">
                                                            <asp:TextBox ID="TB_Memo" runat="server" Width="468px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="left">
                                                <asp:Label ID="lb_ShowMessage" runat="server" Font-Size="Small" ForeColor="Red" Text="��Ϣ��ʾ����"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table align="center" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="btnSavePriceAdjust" runat="server" OnClick="btnSavePriceAdjust_Click" Text="�����۸��������" Width="146px" />
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="btnEditPriceAdjust" runat="server" OnClick="btnEditPriceAdjust_Click" OnClientClick="return confirm('�޸ĸü�¼����������ؼ�Ч���¼��㣬ȷ���޸ĸü�¼��?')" Text="�޸ļ۸��������" Width="146px" />
                                                </table>
                                            </td>
                                            <td>
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                    <asp:Button ID="btnDelPriceAdjust" runat="server" OnClick="btnDelPriceAdjust_Click" OnClientClick="return confirm('��ȷ��ɾ���õ�����¼��?')" Text="ɾ�������������" Width="146px" />
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
                        <table id="Table2" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="145" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�۸������־
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
                                <td align="left" style="padding: 5px 5px 5px 5px" valign="top" width="100%">
                                    <table align="center" border="1" cellpadding="0" cellspacing="0" width="96%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" AllowPaging="True" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="��¼��" />
                                                        <asp:BoundField DataField="ItemNo" HeaderText="��Ч��¼���" />
                                                        <asp:BoundField DataField="ItemNum" HeaderText="����" />
                                                        <asp:BoundField DataField="ItemPriceDeviceAdjust" HeaderText="�豸��������" />
                                                        <asp:BoundField DataField="ItemPriceMainMaterialAdjust" HeaderText="���ĵ�������" />
                                                        <asp:BoundField DataField="ItemPriceWageAdjust" HeaderText="�˹���������" />
                                                        <asp:BoundField DataField="ItemPriceMaterialAdjust" HeaderText="���ϵ�������" />
                                                        <asp:BoundField DataField="ItemPriceMachineAdjust" HeaderText="��е��������" />
                                                        <asp:BoundField DataField="ProjectBCWS" HeaderText="Ԥ��ϼƺϼ�(BCWS)" />
                                                        <asp:BoundField DataField="Memo" HeaderText="��ע" />
                                                        <asp:BoundField DataField="AdjustByWho" HeaderText="������" />
                                                        <asp:BoundField DataField="AdjustTime" HeaderText="��������" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" />
                                                        <asp:BoundField DataField="AdjustMemo" HeaderText="����˵��" />
                                                    </Columns>
                                                    <EditRowStyle BackColor="#999999" />
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
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
