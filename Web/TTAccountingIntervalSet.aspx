<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAccountingIntervalSet.aspx.cs" Inherits="TTAccountingIntervalSet" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>财务区间设置</title>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,CaiWuQuJianSheZhi%>"></asp:Label>
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
                                            <td align="left" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; width:90%;">
                                                <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_ZhangTao" runat="server" Text="<%$ Resources:lang,ZhangTao%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="1" >
                                                            <asp:DropDownList ID="DL_FinancialID" runat="server" DataTextField="FinancialName" DataValueField="FinancialCode">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_ZhuangTai" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DL_Status" runat="server">
                                                                <asp:ListItem Value="OPEN" Text="<%$ Resources:lang,QiYong%>"/>
                                                                <asp:ListItem Value="CLOSE" Text="<%$ Resources:lang,GuanBi%>"/>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_QuJianBianMa" runat="server" Text="<%$ Resources:lang,QuJianBianMa%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_IntervalCode" runat="server" CssClass="shuru" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_QuJianMingCheng" runat="server" Text="<%$ Resources:lang,QuJianMingCheng%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_IntervalName" runat="server" CssClass="shuru" Width="120px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_KaiShiRiQi" runat="server" Text="<%$ Resources:lang,KaiShiRiQi%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="DLC_StartTime" runat="server" CssClass="shuru" ReadOnly="false" Width="120px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_StartTime">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_JieShuRiQi" runat="server" Text="<%$ Resources:lang,JieShuRiQi%>"></asp:Label>：</td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="DLC_EndTime" runat="server" Width="120px" CssClass="shuru" ReadOnly="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_EndTime">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft" style="vertical-align: middle;">&nbsp;</td>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="3" style="text-align: left; vertical-align: middle;">
                                                            <asp:Button ID="BT_Add" runat="server" OnClick="BT_Add_Click" CssClass="inpu" Text="<%$ Resources:lang,XinZeng%>" />&nbsp;
                                                            <asp:Button ID="BT_Update" runat="server" OnClick="BT_Update_Click" CssClass="inpu" Text="<%$ Resources:lang,BaoCun%>" Enabled="False" />&nbsp;
                                                            <asp:Button ID="BT_Delete" runat="server" OnClick="BT_Delete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" CssClass="inpu" Text="<%$ Resources:lang,ShanChu%>" Enabled="False" />
                                                            <asp:TextBox ID="TB_ID" runat="server" CssClass="shuru" Enabled="False" ReadOnly="false" Visible="False" Width="40px"></asp:TextBox>
                                                            <asp:Label ID="lbl_IntervalCode" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left: 5px;">
                                                                        <table align="left" cellpadding="0" cellspacing="0" width="99%">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="LB_QuJianXinXi" runat="server" Text="<%$ Resources:lang,QuJianXinXi%>"></asp:Label>：</td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:TextBox ID="txt_IntervalInfo" runat="server" CssClass="shuru"></asp:TextBox>
                                                                                            </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="LB_ZhuangTai1" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：</td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:DropDownList ID="ddlStatus" runat="server">
                                                                                                    <asp:ListItem Value="" Text="<%$ Resources:lang,QingXunZe%>"/>
                                                                                                    <asp:ListItem Value="OPEN" Text="<%$ Resources:lang,QiYong%>"/>
                                                                                                    <asp:ListItem Value="CLOSE" Text="<%$ Resources:lang,GuanBi%>"/>
                                                                                                </asp:DropDownList>
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
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_BianHao" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                                        </td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_QuJianBianMa1" runat="server" Text="<%$ Resources:lang,QuJianBianMa%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_QuJianMingCheng1" runat="server" Text="<%$ Resources:lang,QuJianMingCheng%>"></asp:Label></strong></td>
                                                                                                        <td width="20%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_ZhangTaoMingCheng" runat="server" Text="<%$ Resources:lang,ZhangTaoMingCheng%>"></asp:Label></strong></td>
                                                                                                        <td width="10%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_ZhuangTai2" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong></td>
                                                                                                        <td width="20%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_KaiShiRiQi1" runat="server" Text="<%$ Resources:lang,KaiShiRiQi%>"></asp:Label></strong></td>
                                                                                                        <td width="20%" align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="LB_JieShuRiQi1" runat="server" Text="<%$ Resources:lang,JieShuRiQi%>"></asp:Label></strong></td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid1_ItemCommand"
                                                                                        OnPageIndexChanged="DataGrid1_PageIndexChanged" PageSize="5" ShowHeader="False"
                                                                                        Width="100%">
                                                                                        <Columns>
                                                                                            <asp:TemplateColumn HeaderText="Number">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:Button ID="BT_Interval" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="IntervalCode" HeaderText="区间编码">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="IntervalName" HeaderText="区间名称">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:TemplateColumn HeaderText="帐套名称">
                                                                                                <ItemTemplate>
                                                                                                    <%# GetFinancialName(Eval("FinancialCode").ToString()) %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:TemplateColumn HeaderText="Status">
                                                                                                <ItemTemplate>
                                                                                                    <%# Eval("Status").ToString()=="OPEN"?"Enabled":"Closed" %>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="StartTime" HeaderText="开始日期" DataFormatString="{0:yyyy-MM-dd}">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                                <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Horizontalalign="left" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="EndTime" HeaderText="结束日期" DataFormatString="{0:yyyy-MM-dd}">
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
