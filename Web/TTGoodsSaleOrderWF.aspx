<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsSaleOrderWF.aspx.cs" Inherits="TTGoodsSaleOrderWF" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /*#AboveDiv {
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
        }*/
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
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,XiaoShouDingDan%>"></asp:Label>
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
                                <td align="right" style="padding: 5px 5px 0px 5px;">
                                    <asp:Button ID="BT_CreateMain" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_CreateMain_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px 5px 5px 5px;">
                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                        width="100%">
                                        <tr>
                                            <td width="7">
                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label166" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                        </td>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label169" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                        </td>
                                                        <td width="10%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label170" runat="server" Text="<%$ Resources:lang,FaQiShengQing%>" /></strong>
                                                        </td>
                                                        <td align="left" width="5%">
                                                            <strong>
                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                        </td>

                                                        <td align="left" width="12%">
                                                            <strong>
                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong>
                                                        </td>
                                                        <td align="left" width="12%">
                                                            <strong>
                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label></strong>
                                                        </td>
                                                        <td align="left" width="10%">
                                                            <strong>
                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label></strong>
                                                        </td>
                                                        <td align="left" width="11%">
                                                            <strong>
                                                                <asp:Label ID="Label167" runat="server" Text="<%$ Resources:lang,ChuangJianZhe%>"></asp:Label></strong>
                                                        </td>
                                                        <td align="left" width="13%">
                                                            <strong>
                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ShiJian%>"></asp:Label></strong>
                                                        </td>
                                                        <%--  <td align="left" width="10%">
                                                                        <strong></strong>
                                                                    </td>--%>
                                                        <td align="left" width="8%">
                                                            <strong>
                                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                        </td>
                                                        <td align="left" width="5%">
                                                            <strong></strong>
                                                        </td>
                                                        <td align="left" width="5%">
                                                            <strong>&nbsp;</strong>
                                                        </td>
                                                        <td align="left" width="5%">
                                                            <strong></strong>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right" width="6">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="DataGrid5" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid5_ItemCommand"
                                        OnPageIndexChanged="DataGrid5_PageIndexChanged" PageSize="25" ShowHeader="false"
                                        Width="100%">
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
                                            <asp:ButtonColumn ButtonType="LinkButton" CommandName="Assign" Text="&lt;div&gt;&lt;img src=ImagesSkin/Assign.png border=0 alt='Deleted' /&gt;&lt;/div&gt;">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                            </asp:ButtonColumn>
                                            <asp:BoundColumn DataField="SOID" HeaderText="SOID">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="SOName" HeaderText="Name">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="12%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Amount" HeaderText="�ܽ��">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CurrencyType" HeaderText="Currency">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                            </asp:BoundColumn>
                                            <asp:HyperLinkColumn DataNavigateUrlField="OperatorCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                DataTextField="OperatorName" HeaderText="������" Target="_blank">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="11%" />
                                            </asp:HyperLinkColumn>
                                            <asp:BoundColumn DataField="SaleTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="����ʱ��">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Status">
                                                <ItemTemplate>
                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                            </asp:TemplateColumn>

                                            <asp:ButtonColumn ButtonType="LinkButton" CommandName="INVOICE" Text="<%$ Resources:lang,FaPiao%>">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:ButtonColumn>
                                            <asp:HyperLinkColumn DataNavigateUrlField="SOID" DataNavigateUrlFormatString="TTMakeCollaboration.aspx?RelatedType=GoodsSO&RelatedID={0}"
                                                HeaderText="Collaboration" Text="<%$ Resources:lang,XieZuo%>" Target="_blank">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:HyperLinkColumn>
                                            <asp:TemplateColumn HeaderText="��ӡ">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                <ItemTemplate>
                                                    <a href='TTGoodsSaleOrderView.aspx?SOID=<%# DataBinder.Eval(Container.DataItem,"SOID") %>' target="_blank">
                                                        <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" /></a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
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
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label22111" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft"  width="15%">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>��
                                                </td>

                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_SOName" runat="server" Width="200px"></asp:TextBox>
                                                </td>
                                                <td width="25%" class="formItemBgStyleForAlignLeft" >
                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,XiaoShouYuan%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_GoodsSales" runat="server" DataTextField="UserName" DataValueField="UserCode">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="LB_SOID" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td width="25%" class="formItemBgStyleForAlignLeft" >
                                                    <asp:Label ID="Label152" runat="server" Text="<%$ Resources:lang,XiaoShouLeiXing %>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_SaleType" runat="server" DataTextField="Type" DataValueField="Type">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,KeHu%>"></asp:Label>��
                                                </td>

                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Customer" runat="server" DataTextField="CustomerName" DataValueField="CustomerCode" AutoPostBack="true" OnSelectedIndexChanged="DL_Customer_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label112222" runat="server" Text="<%$ Resources:lang,ChaXun %>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_CustomerName" runat="server" Width="80px"></asp:TextBox>
                                                </td>
                                                <td colspan="2" class="formItemBgStyleForAlignLeft" >
                                                    <asp:Button ID="BT_FindCustomer" CssClass="inpu" runat="server" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_FindCustomer_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label102" runat="server" Text="<%$ Resources:lang,LianJiRen %>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_ContactPerson" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label111" runat="server" Text="<%$ Resources:lang,DianHua %>"></asp:Label>��
                                                </td>
                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >
                                                    <asp:TextBox ID="TB_ContactPhoneNumber" runat="server" Width="150px"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label146" runat="server" Text="<%$ Resources:lang,ShouHuoDiZi %>"></asp:Label>��
                                                </td>
                                                <td colspan="5" class="formItemBgStyleForAlignLeft" >
                                                    <asp:TextBox ID="TB_ReceiverAddress" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,XiaoShouShiJian%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft" style="width: 106px;">

                                                    <asp:TextBox ID="DLC_SaleTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_SaleTime">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,YuJiDaoHuoShiJian%>"></asp:Label>��
                                                </td>
                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >
                                                    <asp:TextBox ID="DLC_ArrivalTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_ArrivalTime">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft" style="width: 106px;">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Amount" runat="server" Enabled="False" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="" Width="100px" Precision="3">
                                                         0.000
                                                    </NickLee:NumberBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label>��
                                                </td>
                                                <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_CurrencyType" runat="server" ataTextField="Type" DataValueField="Type"
                                                        Height="25px" Width="100px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,KaiPiaoShiJian%>"></asp:Label>��
                                                </td>
                                                <td colspan="5" class="formItemBgStyleForAlignLeft" >
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="DLC_OpenInvoiceTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender3" runat="server" TargetControlID="DLC_OpenInvoiceTime">
                                                                </ajaxToolkit:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label118" runat="server" Text="<%$ Resources:lang,FaPiaoLiXing%>"></asp:Label>��
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DL_InvoiceType" DataValueField="Type" DataTextField="Type" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <asp:Label ID="Label157" runat="server" Text="<%$ Resources:lang,FaPiaoHaoMa%>"></asp:Label>��
                                                            </td>
                                                            <td style="text-align: left">
                                                                <asp:TextBox ID="TB_InvoiceCodeString" runat="server" Width="150px"></asp:TextBox>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>��
                                                </td>
                                                <td colspan="5" class="formItemBgStyleForAlignLeft" >
                                                    <asp:TextBox ID="TB_Comment" runat="server" Width="98%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft" style="width: 106px;">
                                                    <asp:DropDownList ID="DL_SOStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_SOStatus_SelectedIndexChanged">
                                                        <asp:ListItem Value="New" Text="<%$ Resources:lang,XinJian%>" />
                                                        <asp:ListItem Value="InProgress" Text="<%$ Resources:lang,ShenPiZhong%>" />
                                                        <asp:ListItem Value="Completed" Text="<%$ Resources:lang,WanCheng%>" />
                                                        <asp:ListItem Value="Cancel" Text="<%$ Resources:lang,QuXiao%>" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,GuanLian%>"></asp:Label>��
                                                </td>

                                                <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="DL_RelatedType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_RelatedType_SelectedIndexChanged">
                                                                    <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                                                    <asp:ListItem Value="Project" Text="<%$ Resources:lang,XiangMu%>" />
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LB_RelatedID" runat="server" Text="<%$ Resources:lang,GuanLianDuiXiangID%>"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_RelatedID" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                                    PositiveColor="" Precision="0" Width="40px">0</NickLee:NumberBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="BT_Select" runat="server" Text="<%$ Resources:lang,XuanZe%>" Visible="False" />
                                                                <cc1:ModalPopupExtender ID="TB_Select_ModalPopupExtender" runat="server" Enabled="True"
                                                                    TargetControlID="BT_Select" PopupControlID="Panel2" CancelControlID="IMBT_Close"
                                                                    BackgroundCssClass="modalBackground" Y="150">
                                                                </cc1:ModalPopupExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="padding-left: 5px;"></td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <br />
                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="���۵���ϸ">
                                                <HeaderTemplate>

                                                    <asp:Label ID="Label101" runat="server" Text="<%$ Resources:lang,XiaoShouDanMingXi%>"></asp:Label>

                                                </HeaderTemplate>
                                                <ContentTemplate>

                                                    <table align="left" cellpadding="0" cellspacing="0" width="100%">

                                                        <tr>

                                                            <td align="right" style="padding-bottom: 5px;">

                                                                <asp:Button ID="BT_CreateDetail" runat="server" Text="<%$ Resources:lang,New %>" CssClass="inpuYello" OnClick="BT_CreateDetail_Click" />

                                                            </td>

                                                        </tr>

                                                        <tr>

                                                            <td>

                                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0" width="130%">



                                                                    <tr>

                                                                        <td width="7">

                                                                            <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" />

                                                                        </td>

                                                                        <td>

                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                                <tr>

                                                                                    <td width="5%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label162" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                                    </td>
                                                                                    <td width="5%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label163" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                                    </td>


                                                                                    <td align="left" width="5%"><strong>

                                                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label>

                                                                                    </strong></td>



                                                                                    <td align="left" width="8%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="10%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="8%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="10%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="8%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="8%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="5%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="5%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="5%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label108" runat="server" Text="<%$ Resources:lang,JinE %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="6%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label107" runat="server" Text="<%$ Resources:lang,YiTongZhiChuKu %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="6%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,YiChuKu %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>



                                                                                    <td align="left" width="6%">



                                                                                        <strong>



                                                                                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,YiFaHuo %>"></asp:Label>

                                                                                        </strong>

                                                                                    </td>

                                                                                    <td align="left" width="8%">

                                                                                        <strong>

                                                                                            <asp:Label ID="Label1222212" runat="server" Text="<%$ Resources:lang,ShiShouShuLiang %>"></asp:Label>

                                                                                        </strong></td>

                                                                                </tr>

                                                                            </table>

                                                                        </td>



                                                                        <td align="right" width="6">



                                                                            <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" />

                                                                        </td>

                                                                    </tr>

                                                                </table>

                                                                <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid1_ItemCommand"
                                                                    OnPageIndexChanged="DataGrid1_PageIndexChanged" PageSize="5" ShowHeader="False"
                                                                    Width="130%">
                                                                    <Columns>
                                                                        <asp:ButtonColumn CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="3%" />
                                                                        </asp:ButtonColumn>
                                                                        <asp:TemplateColumn HeaderText="Delete">
                                                                            <ItemTemplate>

                                                                                <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>

                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="3%" />
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="ID" HeaderText="ID">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="NoticeOutNumber" HeaderText="��֪ͨ����">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="CheckOutNumber" HeaderText="�ѳ���">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="DeliveryNumber" HeaderText="���ͻ�">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="RealReceiveNumber" HeaderText="ActualReceivedQuantity">
                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                        </asp:BoundColumn>
                                                                    </Columns>
                                                                    <EditItemStyle BackColor="#2461BF" />
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <ItemStyle CssClass="itemStyle" />
                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                </asp:DataGrid>

                                                                <br />

                                                            </td>

                                                        </tr>

                                                    </table>

                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_NewMain" runat="server" class="layui-layer-btn notTab" OnClick="BT_NewMain_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label112" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popDetailWindow" name="fixedDiv"
                        style="z-index: 9999; width: 99%; height: 580px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title2">
                            <asp:Label ID="Label11113" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content2" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table width="100%">
                                <tr>
                                    <td width="50%">
                                        <table align="left" cellpadding="3" cellspacing="0" class="formBgStyle" width="100%">
                                            <tr>
                                                <td style="width: 13%; " class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label161" runat="server" Text="<%$ Resources:lang,LaiYuan %>"></asp:Label>
                                                    ��
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_RecordSourceType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_RecordSourceType_SelectedIndexChanged">
                                                        <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                                        <asp:ListItem Value="GoodsPJRecord" />
                                                        <asp:ListItem Value="ProjectBOMRecord" />
                                                    </asp:DropDownList>
                                                    <asp:Label ID="LB_DetailID" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">ID:
                                                </td>
                                                <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox ID="NB_RecordSourceID" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Precision="0" Width="30px">0</NickLee:NumberBox>
                                                    <asp:Label ID="LB_SourceRelatedID" runat="server" Visible="False" Text="0"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft" style="width: 100px;">
                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>�� 
                                                </td>

                                                <td class="formItemBgStyleForAlignLeft" style="width: 100px;">

                                                    <asp:TextBox ID="TB_GoodsCode" runat="server" Width="95%"></asp:TextBox></td>

                                                <td class="formItemBgStyleForAlignLeft" style="width: 80px; ">

                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>�� </td>

                                                <td colspan="2" class="formItemBgStyleForAlignLeft" >

                                                    <asp:TextBox ID="TB_GoodsName" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:Label ID="LB_ID" runat="server" Visible="false"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:DropDownList ID="DL_Type" runat="server" DataTextField="Type"
                                                        DataValueField="Type">
                                                    </asp:DropDownList></td>

                                                <td class="formItemBgStyleForAlignLeft"></td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="3" ></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>��</td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="5" >

                                                    <asp:TextBox ID="TB_ModelNumber" runat="server" Height="20px" Width="99%"></asp:TextBox></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>�� </td>

                                                <td colspan="5" class="formItemBgStyleForAlignLeft" >
                                                    <asp:TextBox ID="TB_Spec" runat="server" Height="50px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                                    <asp:Button ID="BT_FindGoods" runat="server" CssClass="inpu" OnClick="BT_FindGoods_Click" Text="<%$ Resources:lang,ChaXun %>" />
                                                    <asp:Button ID="BT_Clear" runat="server" CssClass="inpu" OnClick="BT_Clear_Click" Text="<%$ Resources:lang,QingKong %>" /></td>
                                            </tr>
                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>��</td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="5" >

                                                    <asp:TextBox ID="TB_Brand" runat="server" Height="20px" Width="99%"></asp:TextBox></td>
                                            </tr>
                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="2"  >

                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Number" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Width="100px" Precision="3">0.000</NickLee:NumberBox>
                                                </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft"  colspan="2">

                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Price" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Width="100px" Precision="3">0.000</NickLee:NumberBox>
                                                </td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="2"  >

                                                    <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName"
                                                        Width="100px">
                                                    </asp:DropDownList></td>

                                                <td class="formItemBgStyleForAlignLeft"></td>

                                                <td colspan="2" class="formItemBgStyleForAlignLeft" ></td>
                                            </tr>
                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,BeiZhu %>"></asp:Label>�� </td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="5" style="height: 1px; ">

                                                    <asp:TextBox ID="TB_SaleReason" runat="server" Height="42px" TextMode="MultiLine"
                                                        Width="99%"></asp:TextBox></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>

                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer2" runat="server" ActiveTabIndex="2"
                                            Width="100%">

                                            <cc1:TabPanel ID="TabPanel8" runat="server">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,LiaoPinKuCunLieBiao%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <br />

                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,QingXuanZeYaoXiaoShouDeShangPin %>"></asp:Label>��<asp:Label ID="Label75" runat="server" Visible="False"></asp:Label>

                                                    <div id="Div1" style="width: 100%; height: 300px; overflow: auto;">

                                                        <table width="150%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                            <tr>

                                                                <td width="7">

                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                </td>

                                                                <td>

                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                        <tr>

                                                                            <td width="10%" align="left">

                                                                                <strong>

                                                                                    <asp:Label ID="Label76" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong>
                                                                            </td>

                                                                            <td width="13%" align="left">

                                                                                <strong>

                                                                                    <asp:Label ID="Label77" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong>
                                                                            </td>

                                                                            <td width="10%" align="left">

                                                                                <strong>

                                                                                    <asp:Label ID="Label78" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong>
                                                                            </td>

                                                                            <td width="20%" align="left">

                                                                                <strong>

                                                                                    <asp:Label ID="Label79" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong>
                                                                            </td>

                                                                            <td width="10%" align="left"><strong>

                                                                                <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong>

                                                                            </td>



                                                                            <td width="6%" align="left">

                                                                                <strong>

                                                                                    <asp:Label ID="Label83" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong>
                                                                            </td>

                                                                            <td width="6%" align="left">

                                                                                <strong>

                                                                                    <asp:Label ID="Label84" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong>
                                                                            </td>

                                                                            <td width="6%" align="left">

                                                                                <strong>

                                                                                    <asp:Label ID="Label85" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong>
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
                                                            Height="1px" Width="150%" OnItemCommand="DataGrid2_ItemCommand" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None">

                                                            <Columns>

                                                                <asp:TemplateColumn HeaderText="Code">

                                                                    <ItemTemplate>

                                                                        <asp:Button ID="BT_GoodsCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"GoodsCode") %>' />
                                                                    </ItemTemplate>

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:TemplateColumn>

                                                                <asp:BoundColumn DataField="GoodsName" HeaderText="Name">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">

                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Manufacturer" HeaderText="Brand">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>


                                                                <asp:BoundColumn DataField="TotalNumber" HeaderText="Quantity">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="UnitName" HeaderText="Unit">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">

                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                            </Columns>

                                                            <ItemStyle CssClass="itemStyle" />

                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                            <EditItemStyle BackColor="#2461BF" />

                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                        </asp:DataGrid>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>

                                            <cc1:TabPanel ID="TabPanel6" runat="server" TabIndex="1">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label513" runat="server" Text="<%$ Resources:lang,LPCXLB%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <br />

                                                    <asp:Label ID="Label86" runat="server" Text="<%$ Resources:lang,QingXuanQuYaoRuKuDeShangPin %>"></asp:Label>��

                                                        <div id="Div2" style="width: 100%; height: 300px; overflow: auto;">

                                                            <table width="150%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                                <tr>

                                                                    <td width="7">

                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                    </td>

                                                                    <td>

                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                            <tr>

                                                                                <td width="12%" align="left">

                                                                                    <strong>

                                                                                        <asp:Label ID="Label87" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong>
                                                                                </td>

                                                                                <td width="18%" align="left">

                                                                                    <strong>

                                                                                        <asp:Label ID="Label88" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong>
                                                                                </td>



                                                                                <td width="10%" align="left"><strong>

                                                                                    <asp:Label ID="Label80" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>

                                                                                <td width="28%" align="left">

                                                                                    <strong>

                                                                                        <asp:Label ID="Label90" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong>
                                                                                </td>
                                                                                <td width="10%" align="left"><strong>

                                                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong>

                                                                                </td>

                                                                                <td width="20%" align="left">

                                                                                    <strong>

                                                                                        <asp:Label ID="Label106" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>

                                                                    <td width="6" align="right">

                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <asp:DataGrid ID="DataGrid9" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                Height="1px" Width="150%" OnItemCommand="DataGrid9_ItemCommand" CellPadding="4"
                                                                ForeColor="#333333" GridLines="None">

                                                                <Columns>

                                                                    <asp:TemplateColumn HeaderText="Code">

                                                                        <ItemTemplate>

                                                                            <asp:Button ID="BT_ItemCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ItemCode").ToString().Trim() %>' />
                                                                        </ItemTemplate>

                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                    </asp:TemplateColumn>

                                                                    <asp:HyperLinkColumn DataNavigateUrlField="ItemCode" DataNavigateUrlFormatString="TTItemInforView.aspx?ItemCode={0}"
                                                                        DataTextField="ItemName" HeaderText="Name" Target="_blank">

                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="18%" />
                                                                    </asp:HyperLinkColumn>


                                                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                    </asp:BoundColumn>

                                                                    <asp:BoundColumn DataField="Specification" HeaderText="Specification">

                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="28%" />
                                                                    </asp:BoundColumn>

                                                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                    </asp:BoundColumn>

                                                                    <asp:BoundColumn DataField="SalePrice" HeaderText="UnitPrice">

                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                    </asp:BoundColumn>
                                                                </Columns>

                                                                <ItemStyle CssClass="itemStyle" />

                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                <EditItemStyle BackColor="#2461BF" />

                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                            </asp:DataGrid>
                                                        </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>

                                            <cc1:TabPanel ID="TabPanel9" runat="server" HeaderText="���۵���ϸ" TabIndex="2">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label92" runat="server" Text="<%$ Resources:lang,BaoJiaDan%>"></asp:Label>��

                                                                        <asp:Label ID="LB_QOID" runat="server" Font-Bold="True"></asp:Label><asp:Label ID="Label91" runat="server" Text="<%$ Resources:lang,MingXi%>"></asp:Label>��
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <br />
                                                    <asp:Label ID="Label93" runat="server" Text="<%$ Resources:lang,QingXuanQuYaoXiaoShouDeLiaoPin%>"></asp:Label>��
                                                    <div id="Div1" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table align="left" cellpadding="0" cellspacing="0" width="200%">

                                                            <tr>

                                                                <td align="left"></td>
                                                            </tr>

                                                            <tr>

                                                                <td>

                                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                        width="100%">

                                                                        <tr>

                                                                            <td width="7">

                                                                                <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>

                                                                            <td>

                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                                    <tr>

                                                                                        <td align="left" width="10%"><strong>

                                                                                            <asp:Label ID="Label94" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong></td>

                                                                                        <td align="left" width="10%"><strong>

                                                                                            <asp:Label ID="Label95" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong></td>

                                                                                        <td align="left" width="34%"><strong>

                                                                                            <asp:Label ID="Label96" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong></td>
                                                                                        <td width="10%" align="left"><strong>
                                                                                            <asp:Label ID="Label215" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong> </td>
                                                                                        <td align="left" width="13%"><strong>

                                                                                            <asp:Label ID="Label97" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong></td>

                                                                                        <td align="left" width="13%"><strong>

                                                                                            <asp:Label ID="Label98" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong></td>

                                                                                        <td align="left" width="10%"><strong>

                                                                                            <asp:Label ID="Label99" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong></td>

                                                                                        <td align="left" width="10%"><strong></strong></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>

                                                                            <td align="right" width="6">

                                                                                <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" /></td>
                                                                        </tr>
                                                                    </table>

                                                                    <asp:DataGrid ID="DataGrid8" runat="server" AutoGenerateColumns="False"
                                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid8_ItemCommand"
                                                                        ShowHeader="False"
                                                                        Width="100%">

                                                                        <Columns>

                                                                            <asp:TemplateColumn HeaderText="Number">

                                                                                <ItemTemplate>

                                                                                    <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                </ItemTemplate>

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:TemplateColumn>

                                                                            <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="GoodsName" HeaderText="Name">

                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="34%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Number" HeaderText="Quantity">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="Unit" HeaderText="Unit">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                        </Columns>

                                                                        <EditItemStyle BackColor="#2461BF" />

                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                        <ItemStyle CssClass="itemStyle" />

                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>

                                            <cc1:TabPanel ID="TabPanel11" runat="server" HeaderText="<%$ Resources:lang,HeTongLiaoPingQingDan %>" TabIndex="3">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label178" runat="server" Text="<%$ Resources:lang,HeTongLiaoPingQingDan %>"></asp:Label>
                                                </HeaderTemplate>
                                                <ContentTemplate>

                                                    <br />
                                                    <div id="Div1" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table align="left" width="200%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label188" Text="<%$ Resources:lang,HeTong %>" runat="server" Font-Bold="True"></asp:Label>
                                                                    <asp:Label ID="LB_ConstractCode" runat="server" Font-Bold="True"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                        width="100%">
                                                                        <tr>
                                                                            <td width="7">
                                                                                <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>
                                                                            <td>
                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td align="left" width="7%"><strong>
                                                                                            <asp:Label ID="Label179" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="7%"><strong>
                                                                                            <asp:Label ID="Label180" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="10%"><strong>
                                                                                            <asp:Label ID="Label181" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>
                                                                                        <td width="4%" align="left"><strong>
                                                                                            <asp:Label ID="Label82" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label></strong></td>
                                                                                        <td width="10%" align="left"><strong>
                                                                                            <asp:Label ID="Label270" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>
                                                                                        <td width="15%" align="left"><strong>
                                                                                            <asp:Label ID="Label271" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong></td>
                                                                                        <td width="10%" align="left"><strong>
                                                                                            <asp:Label ID="Label81" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong> </td>
                                                                                        <td align="left" width="8%"><strong>
                                                                                            <asp:Label ID="Label182" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="8%"><strong>
                                                                                            <asp:Label ID="Label183" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="7%"><strong>
                                                                                            <asp:Label ID="Label184" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="7%"><strong>
                                                                                            <asp:Label ID="Label185" runat="server" Text="<%$ Resources:lang,JinE %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="7%"><strong>
                                                                                            <asp:Label ID="Label186" runat="server" Text="<%$ Resources:lang,ZaiDanLiangXiaoShouDan %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="7%"><strong>
                                                                                            <asp:Label ID="Label187" runat="server" Text="<%$ Resources:lang,ZaiDanLiangCaiGouDan %>"></asp:Label></strong></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="right" width="6">
                                                                                <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" /></td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:DataGrid ID="DataGrid24" runat="server" AutoGenerateColumns="False"
                                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid24_ItemCommand"
                                                                        ShowHeader="False"
                                                                        Width="100%">
                                                                        <Columns>
                                                                            <asp:TemplateColumn HeaderText="Number">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:TemplateColumn HeaderText="��������">
                                                                                <ItemTemplate>
                                                                                    <%# ShareClass.GetItemType(Eval("GoodsCode").ToString()) %>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="SaleOrderNumber" HeaderText="�ڵ��������۵���">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="PurchaseOrderNumber" HeaderText="�ڶ��� ">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                            </asp:BoundColumn>
                                                                        </Columns>
                                                                        <EditItemStyle BackColor="#2461BF" />
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <ItemStyle CssClass="itemStyle" />
                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    </asp:DataGrid></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>

                                            <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="���۵�">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label105" runat="server" Text="<%$ Resources:lang,BaoJiaDan%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <br />
                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                        width="100%">

                                                        <tr>

                                                            <td width="7">

                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                            <td>

                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                    <tr>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label69" runat="server" Text="<%$ Resources:lang,ID %>"></asp:Label></strong></td>

                                                                        <td align="left" width="30%"><strong>

                                                                            <asp:Label ID="Label70" runat="server" Text="<%$ Resources:lang,DanHao %>"></asp:Label></strong></td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label71" runat="server" Text="<%$ Resources:lang,ZongJinE %>"></asp:Label></strong></td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label72" runat="server" Text="<%$ Resources:lang,BiBie %>"></asp:Label></strong></td>

                                                                        <td align="left" width="20%"><strong>

                                                                            <asp:Label ID="Label73" runat="server" Text="<%$ Resources:lang,BaoJiaShiJian %>"></asp:Label></strong></td>

                                                                        <td align="left" width="10%"><strong>

                                                                            <asp:Label ID="Label74" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong></td>

                                                                        <td align="left" width="10%"><strong></strong></td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                            <td align="right" width="6">

                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <asp:DataGrid ID="DataGrid7" runat="server" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid7_ItemCommand"
                                                        ShowHeader="False"
                                                        Width="100%">

                                                        <Columns>

                                                            <asp:TemplateColumn HeaderText="����">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />

                                                                <ItemTemplate>

                                                                    <asp:Button ID="BT_QOID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"QOID") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>

                                                            <asp:BoundColumn DataField="QOName" HeaderText="Name">

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="30%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="Amount" HeaderText="�ܽ��">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="CurrencyType" HeaderText="Currency">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="QuotationTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="����ʱ��">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="��ӡ">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />

                                                                <ItemTemplate>

                                                                    <a href='TTGoodsSaleQuotationOrderView.aspx?QOID=<%# DataBinder.Eval(Container.DataItem,"QOID") %>' target="_blank">

                                                                        <img src="ImagesSkin/print.gif" alt="��ӡ" border="0" /></a>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                        </Columns>

                                                        <ItemStyle CssClass="itemStyle" />

                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                        <EditItemStyle BackColor="#2461BF" />

                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    </asp:DataGrid>
                                                </ContentTemplate>
                                            </cc1:TabPanel>

                                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="������ͬ">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label104" runat="server" Text="<%$ Resources:lang,GuanLianHeTong%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>
                                                    <br />
                                                    <div id="Div1122" style="width: 100%; height: 428px; overflow: auto;">
                                                        <table align="left" cellpadding="0" cellspacing="0" width="200%">

                                                            <tr>

                                                                <td align="left">

                                                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Width="100%"></asp:Label></td>
                                                            </tr>

                                                            <tr>

                                                                <td>

                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                                        <tr>

                                                                            <td width="7">

                                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                                            <td>

                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                                    <tr>

                                                                                        <td width="6%" align="left"><strong>

                                                                                            <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,HeTongDaiMa%>"></asp:Label></strong></td>

                                                                                        <td width="16%" align="left"><strong>

                                                                                            <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,HeTongMingCheng%>"></asp:Label></strong></td>

                                                                                        <td width="5%" align="left"><strong>

                                                                                            <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong></td>

                                                                                        <td width="5%" align="left"><strong>

                                                                                            <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong></td>

                                                                                        <td width="8%" align="left"><strong>

                                                                                            <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,QianDingRiQi%>"></asp:Label></strong></td>

                                                                                        <td width="6%" align="left"><strong>

                                                                                            <asp:Label ID="Label65" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong></td>

                                                                                        <td width="5%" align="left"><strong>

                                                                                            <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong></td>

                                                                                        <td width="10%" align="left"><strong>

                                                                                            <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,JiaFangDanWei%>"></asp:Label></strong></td>

                                                                                        <td width="10%" align="left"><strong>

                                                                                            <asp:Label ID="Label68" runat="server" Text="<%$ Resources:lang,YiFangDaiWei%>"></asp:Label></strong></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>

                                                                            <td width="6" align="right">

                                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                        </tr>
                                                                    </table>

                                                                    <asp:DataGrid ID="DataGrid6" runat="server" AutoGenerateColumns="False" ShowHeader="False" OnItemCommand="DataGrid6_ItemCommand"
                                                                        Height="1px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                        <Columns>

                                                                            <asp:TemplateColumn HeaderText="ContractCode">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="BT_ConstractCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ConstractCode") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:TemplateColumn>

                                                                            <asp:HyperLinkColumn DataNavigateUrlField="ConstractCode" DataNavigateUrlFormatString="TTConstractView.aspx?ConstractCode={0}"
                                                                                DataTextField="ConstractName" HeaderText="ContractName" Target="_blank">

                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="16%" />
                                                                            </asp:HyperLinkColumn>

                                                                            <asp:BoundColumn DataField="Type" HeaderText="Type">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:TemplateColumn HeaderText="Status">
                                                                                <ItemTemplate>
                                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                                            </asp:TemplateColumn>

                                                                            <asp:BoundColumn DataField="SignDate" HeaderText="SigningDate" DataFormatString="{0:yyyy/MM/dd}">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="Amount" HeaderText="Amount">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="Currency" HeaderText="Currency">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="PartA" HeaderText="PartyAUnit">

                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="PartB" HeaderText="�ҷ���λ">

                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                        </Columns>

                                                                        <ItemStyle CssClass="itemStyle" />

                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                        <EditItemStyle BackColor="#2461BF" />

                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                    </asp:DataGrid><asp:Label ID="Label4" runat="server" Visible="False"></asp:Label><asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Size="9pt"
                                                                        Visible="False" Width="57px"></asp:Label><asp:Label ID="Label6" runat="server" Font-Bold="False" Font-Size="9pt"
                                                                            Visible="False" Width="57px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                        <asp:Label ID="Label145" runat="server" Text="<%$ Resources:lang,KeShiGeTong%>"></asp:Label><br />
                                                        <table align="left" cellpadding="0" cellspacing="0" width="200%">

                                                            <tr>

                                                                <td align="left">

                                                                    <asp:Label ID="Label109" runat="server" Font-Bold="True" Width="100%"></asp:Label>
                                                                </td>
                                                            </tr>

                                                            <tr>

                                                                <td>

                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                                        <tr>

                                                                            <td width="7">

                                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                                            <td>

                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                                    <tr>

                                                                                        <td width="6%" align="left"><strong>

                                                                                            <asp:Label ID="Label110" runat="server" Text="<%$ Resources:lang,HeTongDaiMa%>"></asp:Label></strong></td>

                                                                                        <td width="16%" align="left"><strong>

                                                                                            <asp:Label ID="Label116" runat="server" Text="<%$ Resources:lang,HeTongMingCheng%>"></asp:Label></strong></td>

                                                                                        <td width="5%" align="left"><strong>

                                                                                            <asp:Label ID="Label117" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong></td>

                                                                                        <td width="5%" align="left"><strong>

                                                                                            <asp:Label ID="Label136" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong></td>

                                                                                        <td width="8%" align="left"><strong>

                                                                                            <asp:Label ID="Label137" runat="server" Text="<%$ Resources:lang,QianDingRiQi%>"></asp:Label></strong></td>

                                                                                        <td width="6%" align="left"><strong>

                                                                                            <asp:Label ID="Label138" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong></td>

                                                                                        <td width="5%" align="left"><strong>

                                                                                            <asp:Label ID="Label139" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong></td>

                                                                                        <td width="10%" align="left"><strong>

                                                                                            <asp:Label ID="Label140" runat="server" Text="<%$ Resources:lang,JiaFangDanWei%>"></asp:Label></strong></td>

                                                                                        <td width="10%" align="left"><strong>

                                                                                            <asp:Label ID="Label141" runat="server" Text="<%$ Resources:lang,YiFangDaiWei%>"></asp:Label></strong></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>

                                                                            <td width="6" align="right">

                                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                        </tr>
                                                                    </table>

                                                                    <asp:DataGrid ID="DataGrid10" runat="server" AutoGenerateColumns="False" ShowHeader="False" OnItemCommand="DataGrid10_ItemCommand"
                                                                        Height="1px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                        <Columns>

                                                                            <asp:TemplateColumn HeaderText="ContractCode">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="BT_ConstractCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ConstractCode") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:TemplateColumn>

                                                                            <asp:HyperLinkColumn DataNavigateUrlField="ConstractCode" DataNavigateUrlFormatString="TTConstractView.aspx?ConstractCode={0}"
                                                                                DataTextField="ConstractName" HeaderText="ContractName" Target="_blank">

                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="16%" />
                                                                            </asp:HyperLinkColumn>

                                                                            <asp:BoundColumn DataField="Type" HeaderText="Type">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:TemplateColumn HeaderText="Status">
                                                                                <ItemTemplate>
                                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                                            </asp:TemplateColumn>

                                                                            <asp:BoundColumn DataField="SignDate" HeaderText="SigningDate" DataFormatString="{0:yyyy/MM/dd}">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="Amount" HeaderText="Amount">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="Currency" HeaderText="Currency">

                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="PartA" HeaderText="PartyAUnit">

                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                            </asp:BoundColumn>

                                                                            <asp:BoundColumn DataField="PartB" HeaderText="�ҷ���λ">

                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                        </Columns>

                                                                        <ItemStyle CssClass="itemStyle" />

                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                        <EditItemStyle BackColor="#2461BF" />

                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                    </asp:DataGrid><asp:Label ID="Label142" runat="server" Visible="False"></asp:Label><asp:Label ID="Label143" runat="server" Font-Bold="False" Font-Size="9pt"
                                                                        Visible="False" Width="57px"></asp:Label><asp:Label ID="Label144" runat="server" Font-Bold="False" Font-Size="9pt"
                                                                            Visible="False" Width="57px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="�ͻ�����">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label113" runat="server" Text="<%$ Resources:lang,KeHuLiaoPin %>"></asp:Label>
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <br />
                                                    <br />
                                                    <div id="Div11" style="width: 100%; height: 428px; overflow: auto;">
                                                        <table align="left" width="200%">
                                                            <tr>
                                                                <td>
                                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                        width="100%">
                                                                        <tr>
                                                                            <td width="7">
                                                                                <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>
                                                                            <td>
                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td align="left" width="15%"><strong>
                                                                                            <asp:Label ID="Label147" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="10%"><strong>
                                                                                            <asp:Label ID="Label148" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong></td>
                                                                                        <td align="left" width="15%"><strong>
                                                                                            <asp:Label ID="Label149" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>
                                                                                        <td width="10%" align="left"><strong>
                                                                                            <asp:Label ID="Label150" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>
                                                                                        <td width="15%" align="left"><strong>
                                                                                            <asp:Label ID="Label151" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong></td>
                                                                                        <td width="10%" align="left"><strong>
                                                                                            <asp:Label ID="Label89" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong> </td>
                                                                                        <td align="left" width="8%"><strong>
                                                                                            <asp:Label ID="Label153" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong></td>
                                                                                        <td align="left"><strong>
                                                                                            <asp:Label ID="Label154" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="right" width="6">
                                                                                <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:DataGrid ID="DataGrid11" runat="server" AutoGenerateColumns="False"
                                                                        CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid11_ItemCommand"
                                                                        ShowHeader="False"
                                                                        Width="100%">
                                                                        <Columns>
                                                                            <asp:TemplateColumn HeaderText="Number">
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                            </asp:TemplateColumn>
                                                                            <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                            </asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                            </asp:BoundColumn>
                                                                        </Columns>
                                                                        <EditItemStyle BackColor="#2461BF" />
                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                        <ItemStyle CssClass="itemStyle" />
                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>


                                            <cc1:TabPanel ID="TabPanel5" runat="server" TabIndex="21">
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label>
                                                    :
                                            <asp:Label ID="LB_ProjectID" runat="server"></asp:Label>
                                                    &#160;<asp:Label runat="server" Text="<%$ Resources:lang,MingXi%>"></asp:Label>��
                                                
                                                
                                                </HeaderTemplate>



                                                <ContentTemplate>
                                                    <br />
                                                    <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label155" runat="server" Text="<%$ Resources:lang,Code %>"></asp:Label>
                                                                :
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:TextBox ID="TB_FindItemCode" runat="server" Width="80px"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label156" runat="server" Text="<%$ Resources:lang,Name %>"></asp:Label>
                                                                : </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:TextBox ID="TB_FindItemName" runat="server" Width="80px"></asp:TextBox>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label211" runat="server" Text="<%$ Resources:lang,Specification %>"></asp:Label>
                                                                : </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:TextBox ID="TB_FindItemSpec" runat="server" Width="80px"></asp:TextBox>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label212" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>
                                                                : </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:TextBox ID="TB_FindModelNumber" runat="server" Width="80px"></asp:TextBox>
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Button ID="BT_MaterialBudgetFind" CssClass="inpu" runat="server" Text="<%$ Resources:lang,ChaXun %>" OnClick="BT_MaterialBudgetFind_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <div id="Div1" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table width="200%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                            <tr>
                                                                <td width="7">
                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                <td>
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="10%" align="left"><strong>
                                                                                <asp:Label ID="LB_dgChildItemID" runat="server" Text="<%$ Resources:lang,ID %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="LB_dgChildItemCode" runat="server" Text="<%$ Resources:lang,Code %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="10%" align="left"><strong>
                                                                                <asp:Label ID="LB_dgChildItemName" runat="server" Text="<%$ Resources:lang,Name %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="4%" align="left"><strong>
                                                                                <asp:Label ID="Label333" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="LB_dgChildBomVersion" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="LB_dgChildItemNumber" runat="server" Text="<%$ Resources:lang,Number %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="Label158" runat="server" Text="<%$ Resources:lang,YuCaiGouLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="Label159" runat="server" Text="<%$ Resources:lang,YuRuKuLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="Label168" runat="server" Text="<%$ Resources:lang,YuChuKuLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="Label173" runat="server" Text="<%$ Resources:lang,YuShengChanLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="5%" align="left"><strong>
                                                                                <asp:Label ID="Label160" runat="server" Text="SoldQuantity"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="LB_dgChildItemProcess" runat="server" Text="<%$ Resources:lang,KuChengLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="LB_dgChildItemUnit" runat="server" Text="<%$ Resources:lang,Unit %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left">
                                                                                <strong>
                                                                                    <asp:Label ID="Label209" runat="server" Text="<%$ Resources:lang,CaiGouDanJia %>"></asp:Label>
                                                                                </strong>
                                                                            </td>
                                                                            <td align="left" width="6%">
                                                                                <strong>
                                                                                    <asp:Label ID="Label210" runat="server" Text="<%$ Resources:lang,JinE %>"></asp:Label>
                                                                                </strong>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td width="6" align="right">
                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                            </tr>
                                                        </table>
                                                        <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid3_ItemCommand"
                                                            Width="200%" ShowHeader="False" BorderColor="#7FC4ED" BorderStyle="Solid" BorderWidth="1px">
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <EditItemStyle BackColor="#2461BF" />
                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                            <ItemStyle CssClass="itemStyle" Horizontalalign="left" />
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Number">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="ItemCode" HeaderText="Code">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ItemName" HeaderText="Name">
                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                </asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="��������">
                                                                    <ItemTemplate>
                                                                        <%# ShareClass.GetItemType(Eval("ItemCode").ToString()) %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="4%" />
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="AleadyPurchased" HeaderText="PurchasedQuantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="AleadyCheckIn" HeaderText="StockedQuantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="AleadyCheckOut" HeaderText="OutboundQuantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="AleadyProduction" HeaderText="ProducedQuantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="AleadySale" HeaderText="MaterialIssuedQuantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                </asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="�����">
                                                                    <ItemTemplate>
                                                                        <%# ShareClass.GetMaterialsStockNumber(Eval("ItemCode").ToString()) %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="PurchasePrice" HeaderText="�ɹ�����">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="Amount">
                                                                    <ItemTemplate>
                                                                        <%# decimal.Parse( ShareClass.GetMaterialsStockNumber(Eval("PurchasePrice").ToString())) * decimal.Parse( ShareClass.GetMaterialsStockNumber(Eval("Number").ToString())) %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <a href='TTDocumentTreeView.aspx?RelatedType=BOM&RelatedItemCode=<%#DataBinder.Eval (Container .DataItem ,"ItemCode") %>&RelatedID=<%#DataBinder.Eval (Container .DataItem ,"BomVersionID") %>'>
                                                                            <img src="ImagesSkin/Doc.gif" class="noBorder" />
                                                                        </a>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>


                                                </ContentTemplate>



                                            </cc1:TabPanel>

                                            <cc1:TabPanel ID="TabPanel7" runat="server" HeaderText="ProjectBOM" TabIndex="22">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label172" runat="server" Text="ProjectBOM"></asp:Label>


                                                </HeaderTemplate>



                                                <ContentTemplate>
                                                    <br />
                                                    <div id="GoodsListDivID12" style="width: 140%; height: 428px; overflow: auto; text-align: left;">
                                                        <table cellpadding="2" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LB_tbItemBomVersion" runat="server" Text="<%$ Resources:lang,Version%>"></asp:Label>
                                                                    : </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="DL_ChangeProjectItemBomVersionID" runat="server" AutoPostBack="True"
                                                                        DataTextField="VerID" DataValueField="ID" OnSelectedIndexChanged="DL_ChangeProjecrItemBomVersionID_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <hr />
                                                        <asp:TreeView ID="TreeView4" runat="server" Font-Bold="False" Font-Names="����" Font-Size="10pt" OnSelectedNodeChanged="TreeView4_SelectedNodeChanged"
                                                            NodeWrap="True" ShowLines="True">
                                                            <RootNodeStyle CssClass="rootNode" />
                                                            <NodeStyle CssClass="treeNode" />
                                                            <LeafNodeStyle CssClass="leafNode" />
                                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                                        </asp:TreeView>
                                                    </div>


                                                </ContentTemplate>



                                            </cc1:TabPanel>

                                        </cc1:TabContainer>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer1" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_NewDetail" runat="server" class="layui-layer-btn notTab" OnClick="BT_NewDetail_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label114" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popAssignWindow" name="noConfirm"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title1">
                            <asp:Label ID="Label103" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content1" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table cellpadding="3" cellspacing="0" class="formBgStyle" width="100%">

                                <tr style="font-size: 10pt">

                                    <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 10px; "><strong>

                                        <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,GouMaiShenQingGongZuoLiu%>"></asp:Label>��</strong> </td>
                                </tr>

                                <tr style="font-size: 10pt">

                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>�� </td>

                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:DropDownList ID="DL_WFType" runat="server">

                                            <asp:ListItem Value="MaterialSales" Text="<%$ Resources:lang,LiaoPingXiaoShou%>" />
                                        </asp:DropDownList><asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,GongZuoLiuMuBan%>"></asp:Label>��<asp:DropDownList ID="DL_TemName" runat="server" DataTextField="TemName" DataValueField="TemName"
                                            Width="144px">
                                        </asp:DropDownList><asp:HyperLink ID="HL_WLTem" runat="server" NavigateUrl="~/TTWorkFlowTemplate.aspx"
                                            Target="_blank">
                                            <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,MuBanWeiHu%>"></asp:Label>
                                        </asp:HyperLink><asp:Button ID="BT_Reflash" runat="server" CssClass="inpu" OnClick="BT_Reflash_Click"
                                            Text="<%$ Resources:lang,ShuaXin%>" /></td>
                                </tr>

                                <tr style="font-size: 10pt">

                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>�� </td>

                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:TextBox ID="TB_WLName" runat="server" Width="387px"></asp:TextBox></td>
                                </tr>

                                <tr style="font-size: 10pt">

                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,ShuoMing%>"></asp:Label>�� </td>

                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:TextBox ID="TB_Description" runat="server" Height="48px" TextMode="MultiLine"
                                            Width="441px"></asp:TextBox></td>
                                </tr>

                                <tr style="font-size: 10pt">

                                    <td class="formItemBgStyleForAlignLeft"></td>

                                    <td class="formItemBgStyleForAlignLeft" style="height: 19px;">��<asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,YaoQiuShouDaoChuLiXinXi%>"></asp:Label>��<asp:CheckBox ID="CB_SMS" runat="server" Text="<%$ Resources:lang,DuanXin%>" /><asp:CheckBox ID="CB_Mail" runat="server" Text="<%$ Resources:lang,YouJian%>" /><span style="font-size: 10pt">) </span>

                                        <asp:Button ID="BT_SubmitApply" runat="server" CssClass="inpu" Enabled="False" Text="<%$ Resources:lang,TiJiaoShenQing%>" /><cc1:ModalPopupExtender ID="BT_SubmitApply_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" Y="150"
                                            DynamicServicePath="" Enabled="True" PopupControlID="Panel1" TargetControlID="BT_SubmitApply">
                                        </cc1:ModalPopupExtender>
                                    </td>
                                </tr>
                            </table>

                            <table cellpadding="0" cellspacing="0" width="100%">

                                <tr style="font-size: 10pt;">

                                    <td style="height: 14px; text-align: left">

                                        <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,DuiYingShenPiJiLu%>"></asp:Label>�� </td>
                                </tr>

                                <tr style="font-size: 10pt">

                                    <td style="height: 11px; text-align: left">

                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                            width="100%">

                                            <tr>

                                                <td width="7">

                                                    <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>

                                                <td>

                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                        <tr>

                                                            <td align="left" width="10%"><strong>

                                                                <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong></td>

                                                            <td align="left" width="55%"><strong>

                                                                <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong></td>

                                                            <td align="left" width="15%"><strong>

                                                                <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong></td>

                                                            <td align="left" width="10%"><strong>

                                                                <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong></td>

                                                            <td align="left" width="10%"><strong></strong></td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td align="right" width="6">

                                                    <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" /></td>
                                            </tr>
                                        </table>

                                        <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" Height="1px" PageSize="5" ShowHeader="False"
                                            Width="100%">

                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                            <EditItemStyle BackColor="#2461BF" />

                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                            <ItemStyle CssClass="itemStyle" />

                                            <Columns>

                                                <asp:BoundColumn DataField="WLID" HeaderText="Number">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:BoundColumn>

                                                <asp:HyperLinkColumn DataNavigateUrlField="WLID" DataNavigateUrlFormatString="TTMyWorkDetailMain.aspx?WLID={0}"
                                                    DataTextField="WLName" HeaderText="Workflow" Target="_blank">

                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="55%" />
                                                </asp:HyperLinkColumn>

                                                <asp:BoundColumn DataField="CreateTime" HeaderText="����ʱ��">

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                </asp:BoundColumn>

                                                <asp:TemplateColumn HeaderText="Status">
                                                    <ItemTemplate>
                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateColumn>

                                                <asp:TemplateColumn>

                                                    <ItemTemplate>

                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=Review&WLID={0}") %>'
                                                            Target="_blank"><img class="noBorder" src="ImagesSkin/Doc.gif" /></asp:HyperLink>
                                                    </ItemTemplate>

                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid></td>
                                </tr>

                                <tr style="font-size: 10pt">

                                    <td style="text-align: right">

                                        <asp:Label ID="LB_Sql5" runat="server" Visible="False"></asp:Label><asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label></td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer11" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label115" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popInvoiceWindow" name="noConfirm"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title23">
                            <asp:Label ID="Label134" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content23" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table width="100%">
                                <tr>
                                    <td align="right" style="padding-right: 5px;">
                                        <asp:Button ID="BT_OpenInvoice" runat="server" Text="<%$ Resources:lang,New %>" CssClass="inpuYello" OnClick="BT_OpenInvoice_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                            width="100%">
                                            <tr>
                                                <td width="7">
                                                    <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" /></td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label164" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                            </td>
                                                            <td width="5%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label165" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                            </td>

                                                            <td align="left" width="5%"><strong>
                                                                <asp:Label ID="Label125" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>
                                                            <td align="left" width="6%"><strong>
                                                                <asp:Label ID="Label126" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label></strong></td>
                                                            <td align="left" width="6%"><strong>
                                                                <asp:Label ID="Label127" runat="server" Text="<%$ Resources:lang,ShuiPiaoLeiXing %>"></asp:Label></strong></td>
                                                            <td align="left" width="12%"><strong>
                                                                <asp:Label ID="Label128" runat="server" Text="<%$ Resources:lang,FaPiaoHaoMa %>"></asp:Label></strong></td>
                                                            <td align="left" width="6%"><strong>
                                                                <asp:Label ID="Label129" runat="server" Text="<%$ Resources:lang,JinE %>"></asp:Label></strong></td>
                                                            <td align="left" width="7%"><strong>
                                                                <asp:Label ID="Label130" runat="server" Text="<%$ Resources:lang,ShuiLv %>"></asp:Label></strong></td>
                                                            <td align="left" width="6%"><strong>
                                                                <asp:Label ID="Label131" runat="server" Text="<%$ Resources:lang,KaiPiaoRiQi %>"></asp:Label></strong></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" width="6">
                                                    <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" /></td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid12" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid12_ItemCommand"
                                            ShowHeader="False" Width="100%">
                                            <Columns>
                                                <asp:ButtonColumn CommandName="Update" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 alt='Modify' /&gt;&lt;/div&gt;">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:ButtonColumn>
                                                <asp:TemplateColumn HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBT_Delete" CommandName="Delete" runat="server" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="&lt;div&gt;&lt;img src=ImagesSkin/Delete.png border=0 alt='Deleted' /&gt;&lt;/div&gt;"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="ID" HeaderText="ID">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ReceiveOpen" HeaderText="Type">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="TaxType" HeaderText="˰Ʊ">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="InvoiceCode" HeaderText="��Ʊ����">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Amount" HeaderText="Amount">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="TaxRate" HeaderText="˰��">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="OpenDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="��Ʊ����">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                </asp:BoundColumn>
                                            </Columns>
                                            <EditItemStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Horizontalalign="left" />
                                            <ItemStyle CssClass="itemStyle" />
                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        </asp:DataGrid></td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding-right: 280px;">
                                        <asp:Label ID="Label132" runat="server" Text="<%$ Resources:lang,KaiPiaoZongE %>"></asp:Label>��<asp:Label ID="LB_TotalOpenInvoiceAmount" runat="server" /><asp:Label ID="Label133" runat="server" Text="<%$ Resources:lang,ShouPiaoZongE %>"></asp:Label>��<asp:Label ID="LB_TotalReceiveInvoiceAmount" runat="server" /></td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer23" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label135" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popInvoiceDetailWindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title22">
                            <asp:Label ID="Label275" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content22" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft"  style="width: 10%;">
                                        <asp:Label ID="LB_InvoiceID" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="Label119" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>��</td>
                                    <td colspan="3" class="formItemBgStyleForAlignLeft" >
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="DL_InvoiceReceiveOPen" runat="server">
                                                        <asp:ListItem Value="OPEN" Text="<%$ Resources:lang,KaiPiao%>" />
                                                        <asp:ListItem Value="RECEIVE" Text="<%$ Resources:lang,ShouPiao%>" />
                                                    </asp:DropDownList></td>
                                                <td>
                                                    <asp:Label ID="Label120" runat="server" Text="<%$ Resources:lang,ShuiPiaoZhongLei%>"></asp:Label>��</td>
                                                <td>
                                                    <asp:DropDownList ID="DL_TaxType" runat="server">
                                                        <asp:ListItem Value="NationalTax" Text="<%$ Resources:lang,GuoShui%>" />
                                                        <asp:ListItem Value="LocalTax" Text="<%$ Resources:lang,DiShui%>" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label121" runat="server" Text="<%$ Resources:lang,FaPiaoHaoMa%>"></asp:Label>�� </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_InvoiceCode" runat="server" Width="99%"></asp:TextBox></td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label122" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label>�� </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_InvoiceAmount" runat="server" OnBlur="" OnFocus=""
                                            OnKeyPress="" PositiveColor="" Width="100px" Precision="3">0.000</NickLee:NumberBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label123" runat="server" Text="<%$ Resources:lang,ShuiLv%>"></asp:Label>�� </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_InvoiceTaxRate" runat="server" OnBlur="" OnFocus=""
                                            OnKeyPress="" PositiveColor="" Width="80px" Precision="3">0.000</NickLee:NumberBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label124" runat="server" Text="<%$ Resources:lang,KaiPiaoRiQi%>"></asp:Label>�� </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="DLC_OpenDate" runat="server"></asp:TextBox><ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender8"
                                            runat="server" TargetControlID="DLC_OpenDate" Enabled="True">
                                        </ajaxToolkit:CalendarExtender>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="2"></td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_InvoiceSql" runat="server" Visible="False"></asp:Label></td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer2" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton3" runat="server" class="layui-layer-btn notTab" OnClick="BT_Invoice_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label276" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>

                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;"
                        Width="500px">
                        <div>
                            <table>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:Label ID="Label100" runat="server" Text="<%$ Resources:lang,LCSQSCHYLJDLCJHYMQJHM%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="BT_ActiveYes" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Shi%>" OnClick="BT_ActiveYes_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                            ID="BT_ActiveNo" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Fou%>" OnClick="BT_ActiveNo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 273px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView2" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged"
                                            ShowLines="True" Width="220px">
                                            <RootNodeStyle CssClass="rootNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_Close" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="LB_UserName" runat="server"
                            Visible="False"></asp:Label>
                        <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                    </asp:Panel>
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
