<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAccountReceiptEvidence.aspx.cs" Inherits="TTAccountReceiptEvidence" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>财务收款入账</title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }
        </style>
    <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="js/allAHandler.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }
           
        });
    </script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
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
                                                <table width="345" border="0" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="29">
                                                            <%--<img src="Logo/main_top_l.jpg" alt="" width="29" height="31" />--%>
                                                        </td>
                                                        <td align="left" background="ImagesSkin/main_top_bj.jpg" class="titlezi">
                                                            <asp:Label ID="LB_CaiWuShouKuanRuZhang" runat="server" Text="<%$ Resources:lang,CaiWuShouKuanRuZhang%>"></asp:Label>
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
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td rowspan="2" style="width: 200px; padding: 5px 0px 0px 5px; border-left: solid 1px #D8D8D8" valign="top" align="left">
                                                <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" ShowLines="True" Width="200px">
                                                    <RootNodeStyle CssClass="rootNode" /><NodeStyle CssClass="treeNode" /><LeafNodeStyle CssClass="leafNode" /><SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                                </asp:TreeView>
                                            </td>
                                            <td>
                                                                        <table align="left" cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ShouKuanXinXi%>"></asp:Label>：</td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:TextBox ID="txt_ReceivInfo" runat="server" CssClass="shuru"></asp:TextBox>
                                                                                                <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                                                                                            </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>
                                                                                                <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="font-size: 10pt">
                                                                                <td>
                                                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                                        width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td width="4%" align="left">
                                                                                                            <strong></strong></td>
                                                                                                        <td width="8%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,HeTongBianMa%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ShouKuanJinE%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShouKuanRiQi%>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,HuiLv%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,CaoZuoZhe%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,FuKuanFang%>"></asp:Label></strong></td>
                                                                                                        <td width="8%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong></td>
                                                                                                        <td width="14%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label></strong></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid1_ItemCommand"
                                                                                        OnPageIndexChanged="DataGrid1_PageIndexChanged" PageSize="8" ShowHeader="False"
                                                                                        Width="100%">
                                                                                        <Columns>
                                                                                            <asp:TemplateColumn HeaderText="选择">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="CbSelect" runat="server" />
                                                                                                    <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID")%>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="Number">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="ConstractCode" HeaderText="合同编码">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="ReceiverAccount" HeaderText="收款金额">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="ReceiverTime" HeaderText="收款日期" DataFormatString="{0:yyyy-MM-dd}">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:TemplateColumn HeaderText="Currency">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetReceiverCurrency(Eval("ID").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="ExchangeRate">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetReceiverCurrencyExchangeRate(Eval("ID").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="OperatorName" HeaderText="操作者">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="Payer" HeaderText="Payer">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:TemplateColumn HeaderText="Status">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetAccountGeneralLedgerStatus(Eval("ID").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="Comment" HeaderText="Remark">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="14%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
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
                                        <tr>
                                            <td>
                                                <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShouKuanBianMa%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="1" >
                                                            <asp:Label ID="lbl_ReceivablesRecordID" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,KuaiJiZhangTao%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DL_FinancialID" runat="server" DataTextField="FinancialName" DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="DL_FinancialID_SelectedIndexChanged" Visible="False">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lbl_FinancialID" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="lbl_FinancialName" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,KuaiJiQuJian%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DL_IntervalID" runat="server" DataTextField="IntervalName" DataValueField="ID" Visible="False">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lbl_IntervalID" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="lbl_IntervalName" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,BenWeiBi%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="lbl_CurrencyType" runat="server"></asp:Label>
                                                            <asp:Label ID="lbl_ExchangeRate" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,KuaiJiKeMu%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DL_AccountID" runat="server" DataTextField="AccountName" DataValueField="AccountCode">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,RuZhangJinE%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <NickLee:NumberBox ID="NB_TotalMoney" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" Width="85px" Enabled="False">0.00</NickLee:NumberBox>
                                                            <asp:Label ID="lbl_TotalMoney" runat="server" Visible="False">0.00</asp:Label>
                                                            <asp:Label ID="lbl_ReceiverAccount" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">&nbsp;</td>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="3" style="text-align: left; vertical-align: middle;">
                                                            <asp:Button ID="BT_Add" runat="server" OnClick="BT_Add_Click" CssClass="inpu" Text="<%$ Resources:lang,RuZhang%>" />
                                                            &nbsp;<asp:Button ID="Btn_AllAdd" runat="server" CssClass="inpu" Text="<%$ Resources:lang,PiLiangRuZhang%>" OnClick="Btn_AllAdd_Click" /></td>
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
