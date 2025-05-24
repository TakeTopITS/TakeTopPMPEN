<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTRCJProjectCostPerformanceList.aspx.cs" Inherits="TTRCJProjectCostPerformanceBenchmar" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Brettle.Web.NeatUpload" Namespace="Brettle.Web.NeatUpload"
    TagPrefix="Upload" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>�ɱ���Ч��׼��Ϣ����</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <link href="css/inputitemsheet.css" rel="stylesheet" type="text/css" />
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
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�ɱ���Ч��׼��Ϣ�б�
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
                                                        <td class="titleIditem">רҵ���ࣺ</td>
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
                                                <table width="280" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="Label3" runat="server" Text="��������ţ�" Width="80"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="tb_QueryID" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="bt_QueryByID" runat="server" Text="<%$ Resources:lang,ChaXun %>" OnClick="bt_QueryByID_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="True" OnRowDataBound="GridView1_RowDataBound" PageSize="15">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:CommandField>
                                                        <asp:BoundField DataField="ItemNo" HeaderText="���" />
                                                        <asp:BoundField DataField="ItemName" HeaderText="�����">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TypeName" HeaderText="רҵ����">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ItemUnit" HeaderText="��λ" />
                                                        <asp:BoundField DataField="ItemNum" HeaderText="����" DataFormatString="{0:N3}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemContent" HeaderText="��������">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SubItem" HeaderText="�ֲ�����">
                                                            <HeaderStyle Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ItemPriceDeviceAdjust" HeaderText="�豸ԭ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMainMaterialAdjust" HeaderText="����ԭ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceWageAdjust" HeaderText="�˹�ԭ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMaterialAdjust" HeaderText="����ԭ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMachineAdjust" HeaderText="��еԭ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceDeviceBudget" HeaderText="Ԥ���豸�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMainMaterialBudget" HeaderText="Ԥ�����ĺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceWageBudget" HeaderText="Ԥ���˹��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMaterialBudget" HeaderText="Ԥ����Ϻϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPriceMachineBudget" HeaderText="Ԥ���е�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPricePurchaseFee" HeaderText="����Ѽ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemPricePurchaseFeeBudget" HeaderText="���ʴ�ʩ��" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemComprehensiveFeeBudget" HeaderText="���" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ItemTaxesBudget" HeaderText="Ԥ��˰��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="ProjectBCWS" HeaderText="Ԥ��ϼƺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                        <asp:BoundField DataField="Name" HeaderText="�а���">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="BeginTime" HeaderText="��ʼʱ��" DataFormatString="{0:yyyy/MM/dd}" />
                                                        <asp:BoundField DataField="EndTime" HeaderText="����ʱ��" DataFormatString="{0:yyyy/MM/dd}" />
                                                        <asp:TemplateField HeaderText="�Զ�����">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("IfSplit").ToString() == "1" ? "��":"��" %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="�۸��ѵ���">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemPriceChanged").ToString() == "1" ? "��":"��" %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
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
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="220" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�ֹ�¼��ɱ���Ч��׼����
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
                                <td style="padding: 5px 5px 5px 5px" valign="top" align="left">
                                    <table width="96%" border="1" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">��ţ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemNo" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">����ţ�</td>
                                                        <td align="left" width="220">
                                                            <asp:TextBox ID="TB_ItemName" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">��λ��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemUnit" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">������</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemCount" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">�������ƣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemContent" class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">�ֲ����</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_SubItem" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">�豸ԭ�����ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceDevice" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">����ԭ�����ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceMainMaterial" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">�˹�ԭ�����ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemWage" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">����ԭ�����ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceMaterial" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">��еԭ�����ۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceMachine" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right" class="titlezi" colspan="2">
                                                <table border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:CheckBox ID="CB_IfEveryMonth" runat="server" Text="�Զ������¼ƻ���" Width="263px" AutoPostBack="True" OnCheckedChanged="CB_IfEveryMonth_CheckedChanged" />
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">Ԥ���豸�ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceDeviceBudget" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">Ԥ�����ĺϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceMainMaterialBudget" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">Ԥ���˹��ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceWageBudget" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">Ԥ����Ϻϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ProjectMaterialBudget" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">Ԥ���е�ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ProjectMachineBudget" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">����Ѽ�����ѣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ItemPricePurchaseFee" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">���ʴ�ʩ�ѣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tb_ItemPricePurchaseFeeBudget" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">��ѣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemComprehensiveFeeBudget" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">Ԥ��˰��ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemTaxesBudget" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">�ϼ�Ԥ��ϼۣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemPriceTotalBudge" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">�а��̣�</td>
                                                        <td align="left" width="120">
                                                            <asp:DropDownList ID="DDL_ProjectSupplierID" class="inputitem" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">��ʼʱ�䣺</td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TB_BeginTime" Class="calendaritem" runat="server"></asp:TextBox>
                                                            <asp:Calendar ID="Calendar1" runat="server" Visible="False" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
                                                        </td>
                                                        <td align="left" width="20">
                                                            <asp:Button ID="BT_Calendar1" runat="server" Text="T" OnClick="BT_Calendar1_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left" colspan="2">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">����ʱ�䣺</td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TB_EndTime" Class="calendaritem" runat="server"></asp:TextBox>
                                                            <asp:Calendar ID="Calendar2" runat="server" Visible="False" OnSelectionChanged="Calendar2_SelectionChanged"></asp:Calendar>
                                                        </td>
                                                        <td align="left" width="20">
                                                            <asp:Button ID="BT_Calendar2" runat="server" Text="T" OnClick="BT_Calendar2_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lb_ShowMessage" runat="server" Text="��Ϣ��ʾ����" Font-Size="Small" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table width="240" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:Button ID="btnSaveBenchmarData" runat="server" Text="������Ч��׼����" Width="146px" OnClick="btnSaveBenchmarData_Click" />
                                                </table>
                                            </td>
                                            <td>
                                                <table width="240" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:Button ID="BT_EditBenchmarData" runat="server" Text="�޸ļ�Ч��׼����" Width="127px" OnClick="BT_EditBenchmarData_Click" />
                                                </table>
                                            </td>
                                            <td>
                                                <table width="240" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:Button ID="btnDelBenchmarData" runat="server" OnClick="btnDelBenchmarData_Click" OnClientClick="return confirm('��ȷ��ɾ���ü�¼��?')" Text="ɾ����Ч��׼����" Width="127px" />
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table id="Table1" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="220" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">����ɱ���Ч��׼����
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
                                <td style="padding: 5px 5px 5px 5px" valign="top" align="left">
                                    <table width="96%" border="1" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div>
                                                            <upload:inputfile id="FileUpload_Training" runat="server" width="400px" />
                                                            &nbsp;<asp:Button ID="btn_ExcelToDataTraining" runat="server" CssClass="inpu" OnClick="btn_ExcelToDataTraining_Click" Text="��������" />
                                                            <br />


                                                            <div id="ProgressBar">
                                                                <upload:progressbar id="ProgressBar1" runat="server" height="100px" width="500px">
                                                                </upload:progressbar>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btn_ExcelToDataTraining" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="3">

                                                <asp:Button ID="BT_ExportToExcel" runat="server" OnClick="BT_ExportToExcel_Click" Text="������Excel�ļ�" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left" colspan="3">
                                                <table width="240" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:Label ID="LB_ShowMessageImport" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="3">
                                                <table width="240" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:TextBox ID="TB_AnalysMsg" runat="server" Height="388px" TextMode="MultiLine" Width="1292px"></asp:TextBox>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BT_ExportToExcel" />

                </Triggers>

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

