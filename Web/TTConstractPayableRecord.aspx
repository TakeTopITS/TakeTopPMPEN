<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTConstractPayableRecord.aspx.cs" Inherits="TTConstractPayableRecord" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 980px;
            width: expression (document.body.clientWidth <= 980? "980px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            if (top.location != self.location) { } else { CloseWebPage(); }

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
                                <td>
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
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
                                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,HeTongFuKuanJiLu%>"></asp:Label>
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
                                            <td style="padding-top: 5px">
                                                <table width="99%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="5" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td colspan="11" class="tdTopLine">
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="6%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="10%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,YuanShiDanHao%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,FuKuanKeMu%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="7%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,YingFuJinE%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YingFuRiQi%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="7%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShiFuJinE%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <%--  <td align="left" width="7%">
                                                                                                <strong><asp:Label runat="server" Text="<%$ Resources:lang,KaiPiaoJinE%>"></asp:Label></strong>
                                                                                            </td>--%>
                                                                                            <td align="left" width="7%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,WeiFuJinE%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="21%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ShouKuanFang%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,TiQian%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="5%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="9%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,DengJi%>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="6" align="right">
                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                            CellPadding="4" ForeColor="#333333" GridLines="None"
                                                                            Height="1px" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="Number">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="BillCode" HeaderText="OriginalDocumentNumber">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Account" HeaderText="付款科目">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="PayableAccount" HeaderText="AmountPayable">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="PayableTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="应付日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="OutOfPocketAccount" HeaderText="ActualPaymentAmount">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>

                                                                                <%--  <asp:BoundColumn DataField="InvoiceAccount" HeaderText="开票金额">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>--%>
                                                                                <asp:BoundColumn DataField="UNPayAmount" HeaderText="UnpaidAmount">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Receiver" HeaderText="Payee">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="21%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="PreDays" HeaderText="提前">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="OperatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="OperatorName" HeaderText="Registration" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="9%" />
                                                                                </asp:HyperLinkColumn>
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
                                                            <asp:Label ID="LB_Status" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="LB_ConstractCode" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="LB_ConstractID" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px;" align="left">
                                                            <table width="90%">
                                                                <tr>
                                                                    <td align="right" style="padding-right: 5px;">
                                                                        <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New %>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                            width="100%">
                                                                            <tr>
                                                                                <td width="7">
                                                                                    <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" />
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td width="5%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                                            </td>
                                                                                            <td width="5%" align="left">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                                            </td>

                                                                                            <td align="left" width="6%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="7%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ShiFuJinE%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="7%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ShouXuFei%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="7%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,BenBiJinE%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ShiFuRiQi%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,FuKuanFangShi%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,HuiLv%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,YinHang%>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="15%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ShouKuanRen%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="8%">
                                                                                                <strong>
                                                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,DengJi%>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td align="right" width="6">
                                                                                    <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                            ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid1_ItemCommand"
                                                                            ShowHeader="False" Width="100%">
                                                                            <Columns>
                                                                                <asp:ButtonColumn ButtonType="LinkButton" CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:ButtonColumn>
                                                                                <asp:TemplateColumn HeaderText="Delete">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:TemplateColumn>
                                                                                <asp:BoundColumn DataField="ID" HeaderText="ID">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="OutOfPocketAccount" HeaderText="ActualAmountReceived">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="HandlingCharge" HeaderText="HandlingFee">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="HomeCurrencyAmount" HeaderText="本币金额">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="OutOfPocketTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="实收日期">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ReAndPayType" HeaderText="收款方式">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Currency" HeaderText="Currency">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="ExchangeRate" HeaderText="ExchangeRate">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Bank" HeaderText="Bank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:BoundColumn DataField="Receiver" HeaderText="Payee">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                                </asp:BoundColumn>
                                                                                <asp:HyperLinkColumn DataNavigateUrlField="OperatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="OperatorName" HeaderText="Registration" Target="_blank">
                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:HyperLinkColumn>
                                                                            </Columns>
                                                                            <EditItemStyle BackColor="#2461BF" />
                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Horizontalalign="left" />
                                                                            <ItemStyle CssClass="itemStyle" />
                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,YingFuZongE%>"></asp:Label>：<asp:Label ID="LB_PayableAmount" runat="server"></asp:Label>&nbsp;<asp:Label ID="Label24" runat="server"></asp:Label><asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ShiFuZongE%>"></asp:Label>：<asp:Label
                                                                            ID="LB_OutOfPocketAmount" runat="server"></asp:Label>
                                                                        &nbsp;<asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,DaiFuZongE%>"></asp:Label>：<asp:Label ID="LB_UNOutOfPocketAmount" runat="server"></asp:Label>
                                                                        &nbsp; &nbsp;<asp:Label ID="LB_PayableCurrency" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="LB_PopWindowTitle" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft"  style="width: 10%;">
                                        <asp:Label ID="LB_PayableRecordID" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,FuKuanFangShi%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft"  style="width: 10%;">
                                        <asp:DropDownList ID="DL_ReAndPayType" runat="server" DataTextField="Type" DataValueField="Type">
                                        </asp:DropDownList></td>
                                    <td class="formItemBgStyleForAlignLeft"  style="width: 10%;">
                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft"  style="width: 10%;">
                                        <asp:DropDownList ID="DL_Currency" runat="server" DataTextField="Type" DataValueField="Type" AutoPostBack="true"
                                            Height="25px" Width="100px" OnSelectedIndexChanged="DL_Currency_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                    <td class="formItemBgStyleForAlignLeft"  style="width: 10%;">
                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,HuiLv%>"></asp:Label>：</td>
                                    <td class="formItemBgStyleForAlignLeft" >
                                        <NickLee:NumberBox ID="NB_ExchangeRate" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Precision="5" Width="80px" Amount="1">1.0001.000</NickLee:NumberBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,ShiFuJinE%>"></asp:Label>：
                                    </td>
                                    <td colspan="5" class="formItemBgStyleForAlignLeft">
                                        <table>
                                            <tr>
                                                <td>
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_OutOfPocketAccount" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Width="100px" Precision="2">0.000</NickLee:NumberBox>
                                                    <asp:Label ID="LB_ReceivablesAccountCurre" runat="server" Text="<%$ Resources:lang,Yuan %>"></asp:Label>

                                                    <asp:Label ID="Label345" runat="server" Text="<%$ Resources:lang,ShuiLv %>"></asp:Label>
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_OutOfPocketTaxRate" runat="server" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="" Width="80px" Precision="2">0.000</NickLee:NumberBox>
                                                    <asp:DropDownList ID="DL_OutOfPocketTaxRate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DL_OutOfPocketTaxRate_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                        <asp:ListItem Value="0.13" Text="13%"></asp:ListItem>
                                                        <asp:ListItem Value="0.09" Text="9%"></asp:ListItem>
                                                        <asp:ListItem Value="0.06" Text="6%"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ShouXuFei%>"></asp:Label>：
                                                </td>
                                                <td>
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_HandlingCharge" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Width="60px" Precision="2">0.000</NickLee:NumberBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="BT_Count" runat="server" Text="<%$ Resources:lang,JiSuan%>" OnClick="BT_Count_Click" />

                                                </td>
                                                <td>
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,BenBiJinE%>"></asp:Label>：
                                                </td>
                                                <td>
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_HomeCurrencyAmount" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Width="120px" Precision="2">0.000</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,ShiFuRiQi%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="DLC_OutOfPocketTime" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender4"
                                            runat="server" TargetControlID="DLC_OutOfPocketTime" Enabled="True">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,YinHang%>"></asp:Label>：</td>
                                    <td colspan="3" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Bank" runat="server" Width="50%"></asp:TextBox>
                                        <asp:DropDownList ID="DL_Bank" runat="server" DataTextField="BankName" DataValueField="BankName" AutoPostBack="True" OnSelectedIndexChanged="DL_Bank_SelectedIndexChanged">
                                        </asp:DropDownList>

                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_PayableInvoiceAccount" runat="server" OnBlur="" OnFocus="" Visible="false"
                                            OnKeyPress="" PositiveColor="" Width="80px" Precision="2">0.000</NickLee:NumberBox>
                                        <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,Yuan %>"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,ShouKuanRen%>"></asp:Label>： </td>
                                    <td colspan="5" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Receiver" runat="server" Width="50%"></asp:TextBox>
                                        <asp:DropDownList ID="DL_Receiver" runat="server" AutoPostBack="True" DataTextField="Receiver" DataValueField="Receiver" Height="25px" OnSelectedIndexChanged="DL_Receiver_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>：
                                    </td>
                                    <td colspan="5" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_ReceiveComment" runat="server" Width="80%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,DengJiRenYuan%>"></asp:Label>：
                                    </td>
                                    <td colspan="5" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_PayableOperatorCode" runat="server"></asp:Label>
                                        <asp:Label ID="LB_PayableOperatorName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton1" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label189" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <%--                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>--%>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script>
</html>
