<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTRCJProjectCost.aspx.cs" Inherits="TTRCJProjectCost" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>��Ŀ�ɱ�����</title>
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
                        <table id="AboveTable" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="220" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">��Ŀ��֧��Ч������Ϣ</td>
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
                                    <table width="96%" border="1" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��Ŀ�ţ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectNo" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�ƻ��ܶ�(BCWS)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectBCWS" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��ֵ�ܶ�(BCWP)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectBCWP" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">Ӧ���ܶ�(BCRWP)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectBCRWP" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��ͬ��(STCV)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectSTCV" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�ƻ�ƫ��(SV)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectSV" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�˵�ӯ��(BV)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectBV" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�������(EAV)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectEAV" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��ͬ���գ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectContractReceived" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�ƻ������(SPI)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectSPI" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�˵���Ч(BVI)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectBVI" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">���㼨Ч(AI)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectAI" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��ͬƫ��(CPB)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectCPB" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">ʵ���ܶ�(PBCWP)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectPBCWP" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�ؿЧ(RVI)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectRVI" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�ؿ�ƫ��(RV)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectRV" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��ͬ�����(CFI)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectCFI" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��������(P&amp;L)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectPL" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��֧����</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectTotalSpending" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">��֧�ܲ</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectIncomeDifference" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">ʵ�ʳɱ�(ACWP)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectACWP" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�ɱ���Ч(RP)��</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectRP" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="right" width="120">�����룺</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="tbProjectTotalIncome" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <table width="240" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:Label ID="lb_ShowMessage1" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="240" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:Button ID="Button1" runat="server" Text="�����ɱ���Ч����" OnClick="Button1_Click" />
                                                </table>
                                            </td>
                                            <td>
                                                <table width="240" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:LinkButton ID="btnGetAllPerformanceList" runat="server" OnClick="btnGetAllPerformanceList_Click" Width="197px">�˽������ϸ��Ϣ</asp:LinkButton>
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
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="220" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�ɱ���Ч��Ϣ�б�
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="center" class="titlezi">
                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">�ֹ�¼���׼��Ϣ</asp:LinkButton>
                                            </td>
                                            <td align="center" class="titlezi">
                                                <asp:Button ID="Button3" runat="server" Text="������в�������(����ר��)" OnClick="Button2_Click" Visible="False" />
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="DDL_PerformanceType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDL_PerformanceType_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px 5px 5px 5px" valign="top" align="left">
                                    <table width="96%" border="1" align="center" cellpadding="0" cellspacing="0">
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
                                                        <td class="titleIditem">�������ƣ�</td>
                                                        <td align="left" width="120">
                                                            <asp:TextBox ID="TB_ItemContent" Class="inputitem" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">��ʼʱ����Сֵ��</td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TB_BeginTime" Class="calendaritem" runat="server"></asp:TextBox>
                                                            <asp:Calendar ID="Calendar1" runat="server" Visible="False" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
                                                        </td>
                                                        <td align="left" width="20">
                                                            <asp:Button ID="BT_Calendar1" runat="server" Text="T" OnClick="BT_Calendar1_Click" />
                                                        </td>
                                                        <td class="titleIditem">��ʼʱ�����ֵ��</td>
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
                                            <td align="left">
                                                <table class="tableitem">
                                                    <tr>
                                                        <td class="titleIditem">�ֲ����</td>
                                                        <td align="left" width="120">
                                                            <asp:DropDownList ID="DDL_SubItem" runat="server" AutoPostBack="True" DataTextField="SubItem"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right" class="titlezi">
                                                <table border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <asp:Button ID="bt_QueryByID" runat="server" Text="<%$ Resources:lang,ChaXun %>" OnClick="bt_QueryByID_Click" />
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="31">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center" class="titlezi" colspan="3">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowPaging="True" OnRowCommand="GridView1_RowCommand1" OnRowDataBound="GridView1_RowDataBound" PageSize="15">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:CommandField ShowSelectButton="True" SelectText="ѡ��">
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:CommandField>
                                                                                <asp:ButtonField CommandName="AdjustPrice" Text="�۸����">
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:ButtonField>
                                                                                <asp:BoundField DataField="ItemNo" HeaderText="���" />
                                                                                <asp:BoundField DataField="ItemName" HeaderText="�����" HtmlEncode="False">
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="ItemContent" HeaderText="��������">
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="SubItem" HeaderText="�ֲ�����">
                                                                                    <HeaderStyle Wrap="False" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="ItemUnit" HeaderText="��λ" />
                                                                                <asp:BoundField DataField="ItemNum" HeaderText="����" DataFormatString="{0:N3}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceDeviceBudget" HeaderText="Ԥ���豸�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMainMaterialBudget" HeaderText="Ԥ�����ĺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceWageBudget" HeaderText="Ԥ���˹��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMaterialBudget" HeaderText="Ԥ����Ϻϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMachineBudget" HeaderText="Ԥ���е�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPricePurchaseFee" HeaderText="����Ѽ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPricePurchaseFeeBudget" HeaderText="���ʴ�ʩ��" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemComprehensiveFeeBudget" HeaderText="���" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemTaxesBudget" HeaderText="Ԥ��˰��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="BCWS" HeaderText="Ԥ��ϼƺϼ�(BCWS)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="TotalWork" HeaderText="ʵ�ʹ�������" DataFormatString="{0:N3}" HtmlEncode="False">
                                                                                    <HeaderStyle Width="50px" Wrap="False" />
                                                                                    <ItemStyle Width="50px" Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="TotalConfirmWork" HeaderText="ȷ�Ϲ�������" DataFormatString="{0:N3}" HtmlEncode="False">
                                                                                    <HeaderStyle Width="50px" Wrap="False" />
                                                                                    <ItemStyle Width="50px" Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="ItemPriceDeviceActual" HeaderText="ʵ���豸�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMainMaterialActual" HeaderText="ʵ�����ĺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMaterialActual" HeaderText="ʵ�ʲ��Ϻϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceWageActual" HeaderText="ʵ���˹��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMachineActual" HeaderText="��еִ�кϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemComprehensiveFeeActual" HeaderText="ʵ�ʷ��ʴ�ʩ�Ѻϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemTaxesActual" HeaderText="ʵ��˰��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceTotalActual" HeaderText="ִ�кϼƺϼۣ�BCWP��" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ProjectPlanCompleteBalance" HeaderText="�ƻ���ɲ��(SV)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ProjectBCRWP" HeaderText="Ӧ�տ��(BCRWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ProjectAI" HeaderText="���㼨Чָ��(AI)" DataFormatString="{0:p}" />
                                                                                <asp:BoundField DataField="ProjectEAV" HeaderText="�������(EAV)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ProjectPBCWP" HeaderText="ʵ�տ�(PBCWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ProjectRV" HeaderText="���ƫ��(RV)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ProjectRVI" HeaderText="��Чָ��(RVI)" DataFormatString="{0:p}" />
                                                                                <asp:BoundField DataField="Name" HeaderText="�а���">
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="BeginTime" HeaderText="��ʼʱ��" DataFormatString="{0:yyyy/MM/dd}" />
                                                                                <asp:BoundField DataField="EndTime" HeaderText="����ʱ��" DataFormatString="{0:yyyy/MM/dd}" />
                                                                                <asp:TemplateField HeaderText="�۸��ѵ���">
                                                                                    <%--<EditItemTemplate>
                                                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ItemPriceChanged") %>'></asp:TextBox>
                                                                                    </EditItemTemplate>--%>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemPriceChanged").ToString() == "1" ? "YES":"NO" %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
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
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table id="Table2" cellpadding="0" width="100%" cellspacing="0" class="bian">
                            <tr>
                                <td height="31" class="page_topbj">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="220" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">�۸������Ϣ�б�
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
                                <td height="31">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center" class="titlezi" colspan="3">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" AllowPaging="True" OnRowCommand="GridView2_RowCommand" OnRowDataBound="GridView2_RowDataBound" PageSize="15">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:ButtonField CommandName="ActualWorks" Text="ʵ�ʹ���">
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:ButtonField>
                                                                                <asp:BoundField DataField="AdjustID" HeaderText="�������" />
                                                                                <asp:BoundField DataField="ItemNo" HeaderText="��Ч��¼���" />
                                                                                <asp:BoundField DataField="ItemNum" HeaderText="����" DataFormatString="{0:N3}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceDeviceAdjust" HeaderText="�豸��������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMainMaterialAdjust" HeaderText="���ĵ�������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceWageAdjust" HeaderText="�˹���������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMaterialAdjust" HeaderText="���ϵ�������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMachineAdjust" HeaderText="��е��������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ProjectBCWS" HeaderText="Ԥ��ϼƺϼ�(BCWS)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="BCWP" HeaderText="ʵ�ʺϼƺϼ�(BCWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="BCRWP" HeaderText="Ӧ�տ�(BCRWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="PBCWP" HeaderText="ʵ�տ�(PBCWP)" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceDeviceBudget" HeaderText="Ԥ���豸�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMainMaterialBudget" HeaderText="Ԥ�����ĺϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceWageBudget" HeaderText="Ԥ���˹��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMaterialBudget" HeaderText="Ԥ����Ϻϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPriceMachineBudget" HeaderText="Ԥ���е�ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPricePurchaseFee" HeaderText="����Ѽ������" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemPricePurchaseFeeBudget" HeaderText="���ʴ�ʩ��" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemComprehensiveFeeBudget" HeaderText="���" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="ItemTaxesBudget" HeaderText="Ԥ��˰��ϼ�" DataFormatString="{0:N2}" HtmlEncode="False" />
                                                                                <asp:BoundField DataField="Memo" HeaderText="��ע">
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
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
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="Button1" />
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
