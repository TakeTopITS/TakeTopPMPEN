<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAccountAssetPayment.aspx.cs" Inherits="TTAccountAssetPayment" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>收支明细汇总表</title>
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
                                                            <asp:Label ID="LB_ShouZhiMingXiHuiZongBiao" runat="server" Text="<%$ Resources:lang,ShouZhiMingXiHuiZongBiao%>"></asp:Label></td>
                                                        <td width="5">
                                                           <%--<img src="ImagesSkin/main_top_r.jpg" width="5" height="31" alt="" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
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
                                                                                                <asp:Label ID="LB_CaiWuZhangTao" runat="server" Text="<%$ Resources:lang,CaiWuZhangTao%>"></asp:Label>：</td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:DropDownList ID="DL_Financial" runat="server" DataTextField="FinancialName" DataValueField="FinancialCode" AutoPostBack="True" OnSelectedIndexChanged="DL_Financial_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="LB_CaiWuQuJian" runat="server" Text="<%$ Resources:lang,CaiWuQuJian%>"></asp:Label>：</td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:DropDownList ID="DL_Interval" runat="server" DataTextField="IntervalName" DataValueField="IntervalCode">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="LB_LeiXing" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：<asp:DropDownList ID="ddl_Type" runat="server">
                                                                                                <asp:ListItem Value="0" Text="<%$ Resources:lang,ShouZhiMingXiHuiZongBiao%>"/>
                                                                                                <asp:ListItem Value="1" Text="<%$ Resources:lang,ShouKuanMingXiHuiZongBiao%>"/>
                                                                                                <asp:ListItem Value="2" Text="<%$ Resources:lang,FuKuanMingXiHuiZongBiao%>"/>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="LB_KeMu" runat="server" Text="<%$ Resources:lang,KeMu%>"></asp:Label>：<asp:DropDownList ID="ddl_Account" runat="server" DataTextField="AccountName" DataValueField="AccountCode">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="lbl_sql" runat="server" Visible="False"></asp:Label>
                                                                                                <asp:Button ID="BT_Query" runat="server" CssClass="inpu" OnClick="BT_Query_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                                                                &nbsp;
                                                                                                <asp:Button ID="Button1" runat="server" CssClass="inpu" Text="<%$ Resources:lang,DaoChu%>" OnClick="Button1_Click" />
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
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_KeMuLeiXing" runat="server" Text="<%$ Resources:lang,KeMuLeiXing%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_KuaiJiKeMu" runat="server" Text="<%$ Resources:lang,KuaiJiKeMu%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_xingzhi" runat="server" Text="<%$ Resources:lang,xingzhi%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_CaiWuZhangTao2" runat="server" Text="<%$ Resources:lang,CaiWuZhangTao%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_CaiWuQuJian1" runat="server" Text="<%$ Resources:lang,CaiWuQuJian%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_FaShengJinE" runat="server" Text="<%$ Resources:lang,FaShengJinE%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_BiZhong" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_CaoZuoRen" runat="server" Text="<%$ Resources:lang,CaoZuoRen%>"></asp:Label></strong></td>
                                                                                                        <td width="20%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_CaoZuoShiJian" runat="server" Text="<%$ Resources:lang,CaoZuoShiJian%>"></asp:Label></strong></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                                                        OnPageIndexChanged="DataGrid1_PageIndexChanged" ShowHeader="False"
                                                                                        Width="100%">
                                                                                        <Columns>
                                                                                            <asp:BoundColumn DataField="AccountType" HeaderText="SubjectType">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="AccountName" HeaderText="AccountingSubjects">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:TemplateColumn HeaderText="Nature">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetAccountType(Eval("ReceivablesRecordID").ToString(),Eval("PayableRecordID").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="FinancialAccountingSet">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetFinancialName(Eval("FinancialCode").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="FinancialPeriod">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetIntervalName(Eval("IntervalCode").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="TotalMoney" HeaderText="AmountOccurred">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:TemplateColumn HeaderText="Currency">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetFinancialCurrency(Eval("FinancialCode").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="Operator">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetUserName(Eval("Creater").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="CreateTime" HeaderText="OperationTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                        </Columns>
                                                                                        
                                                                                        <ItemStyle CssClass="itemStyle" />
                                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                        <EditItemStyle BackColor="#2461BF" />
                                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                                    </asp:DataGrid>
                                                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,HuiZongQingkuang%>"></asp:Label>：<asp:Label ID="LB_ShouRu" runat="server" Text="<%$ Resources:lang,ShouRu%>" Visible="false"></asp:Label>:
                                                                                    <asp:Label ID="lbl_RecMoney" runat="server" Visible="false"></asp:Label>
                                                                                    &nbsp;<asp:Label ID="LB_ZhiChu" runat="server" Text="<%$ Resources:lang,ZhiChu%>" Visible="false"></asp:Label>：
                                                                                    <asp:Label ID="lbl_PayMoney" runat="server" Visible="false"></asp:Label>
                                                                                    &nbsp;<asp:Label ID="LB_ChaE" runat="server" Text="<%$ Resources:lang,ChaE%>" Visible="false"></asp:Label>：
                                                                                    <asp:Label ID="lbl_Money" runat="server" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="Label2" runat="server" Visible="False"></asp:Label>
                                                                                    <asp:Label ID="Label3" runat="server" Visible="False"></asp:Label>
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
