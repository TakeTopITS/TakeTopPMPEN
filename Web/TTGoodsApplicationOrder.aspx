<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsApplicationOrder.aspx.cs" Inherits="TTGoodsApplicationOrder" %>


<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1180px;
            width: expression (document.body.clientWidth <= 1180? "1180px" : "auto" ));
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZuoYeLingLiaoShengQing%>"></asp:Label>
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
                                <td style="padding: 5px 5px 5px 5px" valign="top" align="left">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                        <tr>
                                            <td width="7">
                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                            </td>
                                            <td>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                        </td>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                        </td>
                                                        <td width="10%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,FaQiShengQing%>" /></strong>
                                                        </td>
                                                        <td align="left" width="5%">
                                                            <strong>
                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>
                                                            </strong>
                                                        </td>
                                                        <td width="35%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>
                                                            </strong>
                                                        </td>
                                                        <td align="left" width="20%">
                                                            <strong>
                                                                <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,ShiJian%>"></asp:Label>
                                                            </strong>
                                                        </td>
                                                        <td width="20%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                            </strong>
                                                        </td>
                                                        <td align="left">&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="6" align="right">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid1_ItemCommand"
                                        Width="100%" AllowPaging="True" PageSize="25" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                        ShowHeader="false" CellPadding="4" ForeColor="#333333" GridLines="None">
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
                                            <asp:BoundColumn DataField="AAID" HeaderText="AAID">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="GAAName" HeaderText="申请名称">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="35%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ApplyTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="申请时间">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Status">
                                                <ItemTemplate>
                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="打印">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                <ItemTemplate>
                                                    <a href='TTGoodsApplicationOrderView.aspx?AAID=<%# DataBinder.Eval(Container.DataItem,"AAID") %>' target="_blank">
                                                        <img src="ImagesSkin/print.gif" alt="打印" border="0" /></a>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <EditItemStyle BackColor="#2461BF" />
                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                        <ItemStyle CssClass="itemStyle" />
                                    </asp:DataGrid>
                                    <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label111" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" style="width: 60%;">
                                        <table class="formBgStyle" cellpadding="3" cellspacing="0" style="width: 98%;"
                                            align="left">
                                            <tr>
                                                <td  width="15%" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label>
                                                    ：
                                                </td>
                                                <td style="width: 35%" class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_GAAName" runat="server" Width="95%"></asp:TextBox>
                                                </td>
                                                <td  width="20%" class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Type" runat="server" Visible="false"
                                                        Height="20px" Width="124px">
                                                        <asp:ListItem Value="COMMON" />
                                                        <asp:ListItem Value="PRODUCTION" Text="<%$ Resources:lang,ShengChanLingYong%>" />
                                                        <asp:ListItem Value="PDFILLING" Text="<%$ Resources:lang,ShengChanBuLiao%>" />
                                                        <asp:ListItem Value="SALE" Text="<%$ Resources:lang,XiaoShouLingYong%>" />
                                                        <asp:ListItem Value="AfterSale" />
                                                    </asp:DropDownList>
                                                    <asp:Label ID="LB_AAID" runat="server" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShenQingYuanYin%>"></asp:Label>
                                                    ：
                                                </td>
                                                <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_ApplyReason" runat="server"
                                                        Width="95%">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label>
                                                    ： </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_ApplyTime" runat="server" ReadOnly="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_ApplyTime">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,YaoQiuWanChengShiJian%>"></asp:Label>
                                                    ： </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="DLC_FinishTime" runat="server" ReadOnly="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_FinishTime">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,GuanLian%>"></asp:Label>
                                                    ：
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="DL_RelatedType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_RelatedType_SelectedIndexChanged">
                                                                    <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                                                    <asp:ListItem Value="Project" Text="<%$ Resources:lang,XiangMu%>" />
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LB_RelatedID" runat="server" Text="ID:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_RelatedID" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                                    PositiveColor="" Precision="0" Width="40px">
                                                                    0</NickLee:NumberBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="BT_Select" runat="server" Text="<%$ Resources:lang,XuanZe%>" Visible="False" />
                                                                <cc1:ModalPopupExtender ID="TB_Select_ModalPopupExtender" runat="server" Enabled="True"
                                                                    TargetControlID="BT_Select" PopupControlID="Panel3" CancelControlID="IMBT_Close"
                                                                    BackgroundCssClass="modalBackground" Y="150">
                                                                </cc1:ModalPopupExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label>
                                                    ： </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_ApplicantCode" runat="server" Width="96px"></asp:TextBox>
                                                    <asp:Label ID="LB_ApplicantName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                    ： </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Status" runat="server" OnSelectedIndexChanged="DL_Status_SelectedIndexChanged">
                                                        <asp:ListItem Value="New" Text="<%$ Resources:lang,XinJian%>" />
                                                        <asp:ListItem Value="InProgress" Text="<%$ Resources:lang,ShenPiZhong%>" />
                                                        <asp:ListItem Value="Completed" Text="<%$ Resources:lang,WanCheng%>" />
                                                        <asp:ListItem Value="Cancel" Text="<%$ Resources:lang,QuXiao%>" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft"></td>

                                            </tr>
                                        </table>
                                        <br />
                                    </td>

                                </tr>
                                <tr>
                                    <td style="padding: 5px 5px 5px 5px;">
                                        <table width="100%" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="right" style="padding-bottom: 5px;">
                                                    <asp:Button ID="BT_CreateDetail" runat="server" Text="<%$ Resources:lang,New %>" CssClass="inpuYello" OnClick="BT_CreateDetail_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2" style="height: 6px">
                                                    <table width="110%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>

                                                                        <td width="5%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                        </td>
                                                                        <td width="5%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                        </td>

                                                                        <td align="left" width="5%"><strong>
                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="8%" align="left"><strong>
                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,LiaoPinDaiMa %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="15%" align="left"><strong>
                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,LiaoPinMingCheng %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="8%" align="left"><strong>
                                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="20%" align="left"><strong>
                                                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="8%" align="left"><strong>
                                                                            <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,YiChuKu %>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>
                                                                        </strong></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="30px"
                                                        OnItemCommand="DataGrid2_ItemCommand" Width="110%" CellPadding="4" ShowHeader="False"
                                                        ForeColor="#333333" GridLines="None">

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

                                                            <asp:BoundColumn DataField="GoodsCode" HeaderText="物料代码">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="GoodsName" HeaderText="物料名">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CheckOutNumber" HeaderText="出库">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Unit" HeaderText="Unit">
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
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_NewMain" runat="server" class="layui-layer-btn notTab" OnClick="BT_NewMain_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton>
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label112" runat="server" Text="<%$ Resources:lang,GuanBi%>" />
                            </a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popDetailWindow" name="fixedDiv"
                        style="z-index: 9999; width: 99%; height: 580px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title2">
                            <asp:Label ID="Label113" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content2" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">


                            <table width="100%">
                                <tr>
                                    <td width="45%">
                                        <table class="formBgStyle" style="width: 100%;" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td style="width: 13%; " class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,LaiYuan %>"></asp:Label>
                                                    ：
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_RecordSourceType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_RecordSourceType_SelectedIndexChanged">
                                                        <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>"/>
                                                        <asp:ListItem Value="GoodsPDRecord" />
                                                        <asp:ListItem Value="GoodsPJRecord" />
                                                        <asp:ListItem Value="ProjectBOMRecord" />
                                                    </asp:DropDownList>
                                                    <asp:Label ID="LB_DetailID" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">ID:
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox ID="NB_RecordSourceID" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Precision="0" Width="30px">0</NickLee:NumberBox>
                                                    <asp:Label ID="LB_SourceRelatedID" runat="server" Visible="False" Text="0"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>
                                                    ：
                                                </td>
                                                <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_GoodsType" runat="server" DataTextField="Type" DataValueField="Type"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>
                                                    ：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft" colspan="3">
                                                    <asp:TextBox ID="TB_GoodsCode" runat="server" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,LiaoPinMingCheng %>"></asp:Label>
                                                    ： </td>
                                                <td colspan="3" style="height: 19px;" class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_GoodsName" runat="server" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>
                                                    ： </td>
                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >
                                                    <asp:TextBox ID="TB_ModelNumber" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>
                                                    ： </td>
                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >
                                                    <asp:TextBox ID="TB_Spec" runat="server" Height="50px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                                    <asp:Button ID="BT_FindGoods" runat="server" CssClass="inpu" OnClick="BT_FindGoods_Click" Text="<%$ Resources:lang,ChaXun %>" />
                                                    &#160;<asp:Button ID="BT_Clear" runat="server" CssClass="inpu" OnClick="BT_Clear_Click" Text="<%$ Resources:lang,QingKong %>" /></td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                    ： </td>
                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >
                                                    <asp:TextBox ID="TB_Brand" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>
                                                    ： </td>
                                                <td colspan="3" style="width: 150px; " class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Number" runat="server" Width="85px" OnBlur="" OnFocus=""
                                                        OnKeyPress="" PositiveColor="" Precision="3">
                                                        0.000</NickLee:NumberBox></td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>
                                                    ： </td>
                                                <td colspan="3" style="width: 150px; " class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName"
                                                        Height="25px" Width="80px" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                    <td>

                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer2" runat="server" ActiveTabIndex="0"
                                            Width="100%">
                                            <cc1:TabPanel ID="TabPanel8" runat="server">
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="<%$ Resources:lang,LiaoPinKuCunLieBiao%>"></asp:Label>


                                                </HeaderTemplate>



                                                <ContentTemplate>
                                                    <asp:Label runat="server" Text="<%$ Resources:lang,QingXuanQuYaoXiaoShouDeLiaoPin %>"></asp:Label>
                                                    ：<asp:Label ID="Label2" runat="server" Visible="False"></asp:Label>
                                                    <div id="Div1" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table width="150%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                            <tr>
                                                                <td width="7">
                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                <td>
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="12%" align="left"><strong>
                                                                                <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="16%" align="left"><strong>
                                                                                <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="10%" align="left"><strong>
                                                                                <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="24%" align="left"><strong>
                                                                                <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="10%" align="left"><strong>
                                                                                <asp:Label ID="Label215" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                                            </strong></td>

                                                                            <td align="left"><strong>
                                                                                <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left"><strong>
                                                                                <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td align="left"><strong>
                                                                                <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>
                                                                            </strong></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td width="6" align="right">
                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                            </tr>
                                                        </table>
                                                        <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                            Height="1px" Width="150%" OnItemCommand="DataGrid3_ItemCommand" CellPadding="4"
                                                            ForeColor="#333333" GridLines="None">
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Code">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="BT_GoodsCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"GoodsCode") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="GoodsName" HeaderText="Name">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="16%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="24%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="UnitName" HeaderText="Unit">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
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
                                                    <asp:Label runat="server" Text="<%$ Resources:lang,QingXuanQuYaoXRuKuDeLiaoPin %>"></asp:Label>
                                                    ：
                                                    <div id="Div2" style="width: 100%; height: 300px; overflow: auto;">
                                                        <table width="150%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                            <tr>
                                                                <td width="7">
                                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                <td>
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="15%" align="left"><strong>
                                                                                <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="20%" align="left"><strong>
                                                                                <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>
                                                                            </strong></td>

                                                                            <td width="15%" align="left"><strong>
                                                                                <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="35%" align="left"><strong>
                                                                                <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="10%" align="left"><strong>
                                                                                <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>
                                                                            </strong></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td width="6" align="right">
                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
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
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                </asp:TemplateColumn>
                                                                <asp:HyperLinkColumn DataNavigateUrlField="ItemCode" DataNavigateUrlFormatString="TTItemInforView.aspx?ItemCode={0}"
                                                                    DataTextField="ItemName" HeaderText="Name" Target="_blank">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                </asp:HyperLinkColumn>

                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Specification" HeaderText="Specification">
                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="35%" />
                                                                </asp:BoundColumn>

                                                                <asp:BoundColumn DataField="Brand" HeaderText="Brand">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
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

                                            <cc1:TabPanel ID="TabPanel2" runat="server" TabIndex="21">
                                                <HeaderTemplate>
                                                    <asp:Label runat="server" Text="<%$ Resources:lang,XiangMu%>"></asp:Label>
                                                    :
                                            <asp:Label ID="LB_ProjectID" runat="server"></asp:Label>
                                                    &#160;<asp:Label runat="server" Text="<%$ Resources:lang,MingXi%>"></asp:Label>：
                                                
                                                
                                                </HeaderTemplate>



                                                <ContentTemplate>
                                                    <br />
                                                    <table style="width: 100%;" cellpadding="2" cellspacing="0" class="formBgStyle">
                                                        <tr>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,Code %>"></asp:Label>
                                                                :
                                                            </td>
                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:TextBox ID="TB_FindItemCode" runat="server" Width="80px"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <asp:Label ID="Label120" runat="server" Text="<%$ Resources:lang,Name %>"></asp:Label>
                                                                : </td>
                                                            <td class="formItemBgStyleForAlignLeft" colspan="3">
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
                                                                                <asp:Label ID="Label143" runat="server" Text="<%$ Resources:lang,YuCaiGouLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="Label144" runat="server" Text="<%$ Resources:lang,YuRuKuLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="5%" align="left"><strong>
                                                                                <asp:Label ID="Label67" runat="server" Text="MaterialIssuedQuantity"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="Label168" runat="server" Text="<%$ Resources:lang,YuChuKuLiang %>"></asp:Label>
                                                                            </strong></td>
                                                                            <td width="6%" align="left"><strong>
                                                                                <asp:Label ID="Label173" runat="server" Text="<%$ Resources:lang,YuShengChanLiang %>"></asp:Label>
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
                                                        <asp:DataGrid ID="DataGrid5" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid5_ItemCommand"
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
                                                                <asp:TemplateColumn HeaderText="物料类型">
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
                                                                <asp:BoundColumn DataField="AleadyPick" HeaderText="MaterialIssuedQuantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="AleadyCheckOut" HeaderText="OutboundQuantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="AleadyProduction" HeaderText="ProducedQuantity">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:TemplateColumn HeaderText="库存量">
                                                                    <ItemTemplate>
                                                                        <%# ShareClass.GetMaterialsStockNumber(Eval("ItemCode").ToString()) %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="PurchasePrice" HeaderText="采购单价">
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

                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="ProjectBOM" TabIndex="22">
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
                                                        <asp:TreeView ID="TreeView4" runat="server" Font-Bold="False" Font-Names="宋体" Font-Size="10pt" OnSelectedNodeChanged="TreeView4_SelectedNodeChanged"
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
                            <asp:LinkButton ID="BT_NewDetail" runat="server" class="layui-layer-btn notTab" OnClick="BT_NewDetail_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton>
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label114" runat="server" Text="<%$ Resources:lang,GuanBi%>" />
                            </a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popAssignWindow" name="noConfirm"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title1">
                            <asp:Label ID="Label103" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content1" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 100px; height: 6px; text-align: right;">
                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,LiuChengMuBan%>"></asp:Label>
                                        ： </td>
                                    <td align="left" style="width: 550px; height: 6px">
                                        <asp:DropDownList ID="DL_TemName" runat="server" DataTextField="TemName" DataValueField="TemName"
                                            Height="25px" Width="194px">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>
                                        ：
                                        <asp:DropDownList ID="DL_WFType" runat="server">
                                            <asp:ListItem Value="ProductionPicking" Text="<%$ Resources:lang,ShengChanLingLiao%>" />
                                        </asp:DropDownList>
                                        <asp:HyperLink ID="HL_WLTem" runat="server" NavigateUrl="~/TTWorkFlowTemplate.aspx"
                                            Target="_blank">
                                            <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,MoBanWeiHu%>"></asp:Label>
                                        </asp:HyperLink>
                                        <asp:Button ID="BT_Reflash" runat="server" OnClick="BT_Reflash_Click" Text="<%$ Resources:lang,ShuaXin%>" CssClass="inpu" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="height: 6px; text-align: right;">
                                        <table width="100%">
                                            <tr>
                                                <td align="left" style="width: 550px; height: 27px"><span style="font-size: 10pt">（<asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,YaoQiuShouDaoXingXi%>"></asp:Label>：</span><asp:CheckBox ID="CB_SMS" runat="server"
                                                    Font-Size="10pt" Text="<%$ Resources:lang,DuanXin%>" /><asp:CheckBox ID="CB_Mail" runat="server" Text="<%$ Resources:lang,YouJian%>" /><span style="font-size: 10pt">) </span>
                                                    <asp:Button ID="BT_SubmitApply" runat="server" Enabled="False" Text="<%$ Resources:lang,TiJiaoShenQing%>" CssClass="inpu" />
                                                    <cc1:ModalPopupExtender ID="BT_SubmitApply_ModalPopupExtender" runat="server" Enabled="True"
                                                        TargetControlID="BT_SubmitApply" PopupControlID="Panel1" BackgroundCssClass="modalBackground" Y="150"
                                                        DynamicServicePath="">
                                                    </cc1:ModalPopupExtender>
                                                    <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="height: 22px; text-align: left;">
                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,DuiYingGongZuoLiuLieBiao%>"></asp:Label>
                                                    ： </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <table width="98%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="50%" align="left"><strong>
                                                                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="20%" align="left"><strong>
                                                                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>
                                                                        </strong></td>
                                                                        <td width="10%" align="left"><strong></strong></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" Height="1px"
                                                        PageSize="5" Width="98%" CellPadding="4" ShowHeader="False" ForeColor="#333333"
                                                        GridLines="None">
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
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="50%" />
                                                            </asp:HyperLinkColumn>
                                                            <asp:BoundColumn DataField="CreateTime" HeaderText="申请时间">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=WorkFlow&WLID={0}") %>'
                                                                        Target="_blank">
                                                                        <img class="noBorder" src="ImagesSkin/Doc.gif" /></asp:HyperLink>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>

                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div id="popwindow_footer11" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label115" runat="server" Text="<%$ Resources:lang,GuanBi%>" />
                            </a>
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
                                        <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,LCSQSCHYLJDLCJHYMQJHM%>"></asp:Label>
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

                    <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" Style="display: none;">
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
                    </asp:Panel>


                    <asp:Label ID="LB_GoodsOwner" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LB_Sql3" runat="server" Visible="False"></asp:Label>

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
