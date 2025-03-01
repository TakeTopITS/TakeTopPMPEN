<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTConstractManageAccount.aspx.cs" Inherits="TTConstractManageAccount" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <title></title>
    <script src="js/My97DatePicker/WdatePicker.js"></script>
</head>
<body>
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
                                            <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="29">
                                                        <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                    </td>
                                                    <td align="center" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                        <asp:Label ID="Label1" runat="server" Text="��̨ͬ��"></asp:Label>
                                                    </td>
                                                    <td width="5">
                                                        <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="float: left;  margin: 2px; padding: 5px; width: 100%;">
                                    <div style="margin: 5px;">
                                        <div style="float: left;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="LB_Contractid" runat="server" Text="��ͬ���:"> </asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="TB_ConstractID" runat="server"></asp:TextBox>
                                                        </td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" Text="��ͬ����:"> </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TB_ConstractName" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text="��ͬ����:"> </asp:Label>
                                                    </td>
                                                    <td>
                                                          <asp:DropDownList ID="DDL_Constractype" runat="server" DataTextField="type" DataValueField="type" Width="175px" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                         <asp:Button ID="BTN_Query" runat="server" Text="��ѯ" OnClick="BTN_Query_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="ѡ���ͬ:"> </asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="TB_ConstractCode" runat="server"></asp:TextBox>
                                                            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                                                                Enabled="True" TargetControlID="TB_ConstractCode" PopupControlID="Panel2"
                                                                CancelControlID="IMBT_CloseTree" BackgroundCssClass="modalBackground" Y="150">
                                                            </cc1:ModalPopupExtender>
                                                        </td>
                                                    <td>
                                                        <asp:Label ID="Label6" runat="server" Text="��ͬ״̬:"> </asp:Label>
                                                    </td>
                                                    <td>
                                                          <asp:DropDownList ID="DDL_ConstractStatus" runat="server" DataTextField="status" DataValueField="status" Width="175px" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label7" runat="server" Text="ǩ������:"> </asp:Label>
                                                    </td>
                                                    <td>
                                                         <%-- <asp:TextBox ID="TB_signdate" runat="server" TextMode="Date" ></asp:TextBox>--%>
                                                        <asp:TextBox ID="DLC_signdate" runat="server" Width="99%" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" onFocus="WdatePicker({lang:'auto'})"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                     
                                    </div>
                                  
                </div>
                <div style="float: left; margin: 2px; padding: 1px; width: 300px;">
                </div>
                <div style="clear: both; padding: 2px;">
                    <hr />
                </div>

                <div>
                    <asp:GridView ID="gvContractData" runat="server" AutoGenerateColumns="False" EmptyDataText="��ʱû�в�ѯ�������Ϣ" AllowPaging="True" PageSize="15" CellPadding="2">
                        <Columns>
                            <asp:BoundField DataField="constractid" HeaderText="SerialNumber" />
                            <asp:BoundField DataField="status" HeaderText="��ͬ״̬" />
                            <asp:BoundField DataField="constractclass" HeaderText="��ͬ����" />
                            <asp:BoundField DataField="type" HeaderText="ContractType" />
                            <asp:BoundField DataField="constractcode" HeaderText="��˾��ͬ���" />
                            <asp:BoundField DataField="partyconstractcode" HeaderText="��Լ����ͬ���" />
                            <asp:BoundField DataField="constractname" HeaderText="ContractName" />
                            <asp:BoundField DataField="ProjectBasis" HeaderText="��ͬ��������" />
                            <asp:BoundField DataField="Amount" HeaderText="ContractAmount" />
                            <asp:BoundField DataField="taxrate" HeaderText="˰��" />
                            <asp:BoundField DataField="aftertaxtotalamount" HeaderText="��ͬ����˰���" />
                            <asp:BoundField DataField="provisionalamount" HeaderText="���н�" />
                            <asp:BoundField DataField="part1" HeaderText="��Լ��" />
                            <asp:BoundField DataField="signdate" HeaderText="ǩ������" />
                            <asp:BoundField DataField="signname" HeaderText="ǩ����" />
                            <asp:BoundField DataField="startdate" HeaderText="Լ������ʱ��" />
                            <asp:BoundField DataField="enddate" HeaderText="Լ������ʱ��" />
                            <asp:BoundField DataField="astartdate" HeaderText="ʵ�ʿ���ʱ��" />
                            <asp:BoundField DataField="aenddate" HeaderText="ʵ�ʿ���ʱ��" />
                            <asp:BoundField DataField="duration" HeaderText="Լ������" />
                            <asp:BoundField DataField="warranty" HeaderText="��ͬ�ʱ���" />
                            <asp:BoundField DataField="prepaypercent" HeaderText="ContractAgreedAdvancePaymentRatio" />
                            <asp:BoundField DataField="monthpaypercent" HeaderText="ContractAgreedMonthlyProgressPaymentRatio" />
                            <asp:BoundField DataField="finishpaypercent" HeaderText="ContractAgreedCompletionPaymentRatio" />
                            <asp:BoundField DataField="settlepaypercent" HeaderText="ContractAgreedSettlementPaymentRatio" />
                            <asp:BoundField DataField="realprogress" HeaderText="ʵʱ������ȣ�%��" />
                            <asp:BoundField DataField="realpvalue" HeaderText="ʵʱ��ֵ��Ԫ��" />
                            <asp:BoundField DataField="sumrecieve" HeaderText="�ۼ��տԪ��" />
                            <asp:BoundField DataField="sumpayment" HeaderText="�ۼƸ��Ԫ��" />
                            <asp:BoundField DataField="settleaccount" HeaderText="�����Ԫ��" />
                            <asp:BoundField DataField="departname" HeaderText="���ܲ���" />
                            <asp:BoundField DataField="operator" HeaderText="������" />
                            <asp:BoundField DataField="pmname" HeaderText="ProjectManager" />
                            <asp:BoundField DataField="memo" HeaderText="Remark" />
                        </Columns>
                    </asp:GridView>
                </div>

                </td>
                        </tr>
                    </table>

                    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 273px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView4" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView4_SelectedNodeChanged"
                                            ShowLines="True" Width="220px">
                                            <RootNodeStyle CssClass="rootNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="center">
                                        <asp:ImageButton ID="IMBT_CloseTree" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
