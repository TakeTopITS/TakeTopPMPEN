<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTRCJProjectTotalCostFee.aspx.cs" Inherits="TTRCJProjectTargetCostFee" %>


<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>�ɱ�����Ŀ�����</title>
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
                                                <table width="180" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">��Ŀ�ɱ����ù���
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table border="0" align="right" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" width="120">
                                                            <asp:RadioButton ID="RB_ALL" runat="server" Text="ȫ����ʾ" AutoPostBack="True" GroupName="TypeForQuery" Checked="true" />
                                                        </td>
                                                        <td align="left" width="120">
                                                            <asp:RadioButton ID="RB_MainType" runat="server" Text="������" AutoPostBack="True" GroupName="TypeForQuery" />
                                                        </td>
                                                        <td align="left" width="120">
                                                            <asp:RadioButton ID="RB_SubType" runat="server" Text="��С��" AutoPostBack="True" GroupName="TypeForQuery" />
                                                        </td>
                                                        <td align="left" width="120">

                                                            <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="right" width="200">
                                                                        <asp:RadioButton ID="RB_Time" runat="server" Text="��ʱ���ѯ" AutoPostBack="True" GroupName="TypeForQuery" />
                                                                    </td>
                                                                    <td align="left" width="120">
                                                                        <asp:DropDownList ID="DDL_YearList" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                                    </td>
                                                                    <td align="left" width="120">
                                                                        <asp:DropDownList ID="DDL_MonthList" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" width="120">
                                                            <asp:Button ID="BT_QueryRecord" runat="server" Text="<%$ Resources:lang,ChaXun %>" Style="height: 27px" OnClick="BT_QueryRecord_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td colspan="2">
                                                <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" width="120">
                                                            <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click">��Ŀ�ʽ�ƻ���д</asp:LinkButton>
                                                        </td>
                                                        <%--<td align="left" width="120">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" >��Ŀ�ʽ�ƻ����</asp:LinkButton>
                                                        </td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="31">
                                    <table align="center" border="0" cellpadding="0" cellspacing="0" width="96%">
                                        <tr>
                                            <td align="left">
                                                <table align="left" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="center" class="titlezi">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderText="���" />
                                                                                <asp:BoundField DataField="CostFeeID" HeaderText="������" />
                                                                                <asp:BoundField DataField="Title" HeaderText="�ɱ�����������" />
                                                                                <asp:BoundField DataField="CostFeeSubID" HeaderText="������" />
                                                                                <asp:BoundField DataField="SubTitle" HeaderText="�ɱ������������" />
                                                                                <asp:TemplateField HeaderText="�ɱ�����">
                                                                                    <EditItemTemplate>
                                                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CostType") %>'></asp:TextBox>
                                                                                    </EditItemTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="Label1" runat="server" Text='<%# GetCostString(Eval("CostType").ToString()) %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="OriginalCost" HeaderText="�ɿسɱ�" />
                                                                                <asp:BoundField DataField="ActualCost" HeaderText="ʵ�ʳɱ�" />
                                                                                <asp:BoundField DataField="TargetCost" HeaderText="Ŀ��ɱ�" />
                                                                                <asp:BoundField DataField="LastTime" HeaderText="����޸�ʱ��" />
                                                                                <asp:CommandField ShowSelectButton="True" HeaderText="����" ButtonType="Button" />
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
                            <tr>
                                <td height="31">
                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center" class="titlezi">
                                                            <table width="100%" border="1" align="center" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" colspan="4">
                                                                        <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td align="right" width="220">���:</td>
                                                                                <td align="left" width="120">
                                                                                    <asp:TextBox ID="tbID" runat="server" ReadOnly="True"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td align="right" width="200">�ɱ��������:</td>
                                                                                <td align="left" width="120">
                                                                                    <asp:DropDownList ID="DDL_CostFee" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDL_CostFee_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table width="420" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td align="right" width="120">�ɱ��������ࣺ</td>
                                                                                <td align="left" width="120">
                                                                                    <asp:DropDownList ID="DDL_CostSubFee" runat="server" AutoPostBack="True">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td colspan="2"></td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td align="right" width="220">�ɱ�����:</td>
                                                                                <td align="left" width="120">
                                                                                    <asp:DropDownList ID="DDL_CostType" runat="server" AutoPostBack="True">
                                                                                        <asp:ListItem Value="1">����Ԥ��</asp:ListItem>
                                                                                        <asp:ListItem Value="2">ʵ��Ŀ��ɱ�</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td align="right" width="200">�ɿسɱ���</td>
                                                                                <td align="left" width="120">
                                                                                    <asp:TextBox ID="TB_OriginalCost" runat="server" ReadOnly="True">0</asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <table width="320" border="0" align="left" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td align="right" width="200">Ŀ��ɱ���</td>
                                                                                <td align="left" width="120">
                                                                                    <asp:TextBox ID="TB_Costtarget" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td></td>
                                                                </tr>

                                                                <tr>
                                                                    <td align="left" colspan="4">
                                                                        <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                                            <asp:Label ID="lb_ShowMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                                            <asp:Button ID="btnAddNewItem" runat="server" Text="��������" Width="146px" OnClick="btnAddNewItem_Click" />
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table align="left" border="0" cellpadding="0" cellspacing="0" width="240">
                                                                            <asp:Button ID="btnEditItem" runat="server" Text="�޸�����" Width="146px" OnClick="btnEditItem_Click" />
                                                                        </table>
                                                                    </td>
                                                                    <td align="left">
                                                                        <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 2px">
                                                                            <asp:Button ID="btnDeleteItem" runat="server" Text="ɾ������" Width="146px" OnClientClick="return confirm('��ȷ��ɾ���ü�¼��?')" OnClick="btnDeleteItem_Click" />
                                                                        </table>
                                                                    </td>
                                                                    <td align="left">
                                                                        <table align="left" border="0" cellpadding="0" cellspacing="0" style="width: 2px">
                                                                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click" Width="189px">ʵ�ʳɱ����ݹ���</asp:LinkButton>
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
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">����������ϼƲ�ѯ
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="280" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="RB_FeeType" runat="server" Text="������" Checked="True" GroupName="GN_TotalType" AutoPostBack="True" OnCheckedChanged="RB_FeeType_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="RB_FeeSubType" runat="server" Text="������" GroupName="GN_TotalType" AutoPostBack="True" OnCheckedChanged="RB_FeeSubType_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="580" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LB_ShowMessageTotalByType" runat="server" Text="��Ϣ��ʾ:"></asp:Label>
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
                                                        <td align="center" class="titlezi">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="GV_TotalByType" runat="server" AutoGenerateColumns="False" PageSize="12" ShowHeaderWhenEmpty="True" OnRowDataBound="GV_TotalByType_RowDataBound" OnPageIndexChanging="GV_TotalByType_PageIndexChanging">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="CostFeeID" HeaderText="������" />
                                                                                <asp:BoundField DataField="Title" HeaderText="����������" />
                                                                                <asp:BoundField DataField="CostFeeSubId" HeaderText="������" />
                                                                                <asp:BoundField DataField="SubTitle" HeaderText="�����������" />
                                                                                <asp:BoundField DataField="originalcost" HeaderText="�ɿسɱ�" />
                                                                                <asp:BoundField DataField="ActualCost" HeaderText="ʵ�ʳɱ�" />
                                                                                <asp:BoundField DataField="TargetCost" HeaderText="Ŀ��ɱ�" />
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
                                                        <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">���·ݺϼ�ʵ�ʳɱ�
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left">
                                                <table width="1080" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LB_ShowMessageTotalByMonth" runat="server" Text="��Ϣ��ʾ:"></asp:Label>
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
                                                        <td align="center" class="titlezi">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" PageSize="12" ShowHeaderWhenEmpty="True" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowDataBound="GridView2_RowDataBound">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="WorkYear" HeaderText="�������" />
                                                                                <asp:BoundField DataField="WorkMonth" HeaderText="�����·�" />
                                                                                <asp:BoundField DataField="tsum" HeaderText="ʵ�ʳɱ��ϼ�" />
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
