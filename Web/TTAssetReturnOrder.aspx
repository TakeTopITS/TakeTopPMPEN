<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTAssetReturnOrder.aspx.cs" Inherits="TTAssetReturnOrder" %>


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
            min-width: 1200px;
            width: expression (document.body.clientWidth <= 1200? "1200px" : "auto" ));
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


    <script type="text/javascript">

        var disPostion = 0;

        function SaveScroll() {
            disPostion = AssetListDivID.scrollTop;
        }

        function RestoreScroll() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function EndRequestHandler(sender, args) {
            AssetListDivID.scrollTop = disPostion;
        }
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZiChanTuiKu%>"></asp:Label>
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
                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                        </td>
                                                        <td width="5%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label68" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                        </td>

                                                        <td align="left" width="5%">
                                                            <strong>
                                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                        </td>
                                                        <td width="50%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                        </td>
                                                        <td width="20%" align="left">
                                                            <strong>
                                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="6" align="right">
                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataGrid ID="DataGrid5" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        ShowHeader="false" Height="1px" OnItemCommand="DataGrid5_ItemCommand" OnPageIndexChanged="DataGrid5_PageIndexChanged"
                                        PageSize="5" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
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

                                            <asp:BoundColumn DataField="ROID" HeaderText="ROID">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ReturnName" HeaderText="Name">
                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="50%" />
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Applicant" HeaderText="Applicant">
                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
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
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label111" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">
                            <table width="100%" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：</td>
                                    <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_ReturnName" runat="server" Width="99%"></asp:TextBox>
                                        <div style="display: none;">
                                            <asp:Label ID="LB_ROID" runat="server"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 25%;">
                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Amount" runat="server" Enabled="False" OnBlur="" OnFocus=""
                                            OnKeyPress="" PositiveColor="" Width="85px">   
                                                                0.00
                                        </NickLee:NumberBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_CurrencyType" runat="server" ataTextField="Type" DataValueField="Type" Height="25px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShiJian%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="DLC_ReturnTime" runat="server" ReadOnly="false"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" TargetControlID="DLC_ReturnTime">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label>： </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Applicant" runat="server" Width="96%"></asp:TextBox>
                                    </td>

                                </tr>

                                <tr style="display: none;">
                                    <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                        <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_UserName" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_Sql1" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_Sql2" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_Sql3" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_Sql5" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="LB_AssetOwner" runat="server" Width="100%" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="6" align="left">
                                        <table width="100%" cellpadding="0" cellspacing="0" align="left">
                                            <tr>
                                                <td align="right" style="padding: 5px 5px 0px 5px;">

                                                    <asp:Button ID="BT_CreateDetail" runat="server" CssClass="inpuYello" OnClick="BT_CreateDetail_Click" Text="<%$ Resources:lang,New %>" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <table width="130%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                            </td>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="5%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                        </td>
                                                                        <td width="5%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                        </td>


                                                                        <td align="left" width="5%"><strong>
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong> </td>

                                                                        <td width="8%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZiChanDaiMa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="10%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ZiChanMingCheng%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="8%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="8%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="10%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="8%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="8%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="8%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ShengChanChangJia%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="8%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,CunFangWeiZhi%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,TuiHuoYuanYin%>"></asp:Label></strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                        Height="1px" OnItemCommand="DataGrid1_ItemCommand" Width="130%" CellPadding="4"
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
                                                            <asp:BoundColumn DataField="AssetCode" HeaderText="资产代码">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AssetName" HeaderText="资产名称">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="UnitName" HeaderText="Unit">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Manufacturer" HeaderText="生产厂家">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ToPosition" HeaderText="存放位置">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="8%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ReturnReason" HeaderText="退货原因">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
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
                        </div>
                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_NewMain" runat="server" class="layui-layer-btn notTab" OnClick="BT_NewMain_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label112" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>
                    <div class="layui-layer layui-layer-iframe" id="popDetailWindow" name="fixedDiv"
                        style="z-index: 9999; width: 99%; height: 580px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title2">
                            <asp:Label ID="Label113" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content2" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">
                            <table align="left" width="100%">
                                <tr>
                                    <td width="50%" class="formItemBgStyleForAlignLeft">
                                        <table align="left" width="100%" cellpadding="1" cellspacing="0" class="formBgStyle">
                                            <tr>
                                                <td width="15%"  class="formItemBgStyleForAlignLeft">
                                                    <div style="display: none;">
                                                        <asp:Label ID="LB_ID" runat="server"></asp:Label>
                                                    </div>
                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,LaiYuan%>"></asp:Label>：
                                                </td>
                                                <td width="15%"  class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_SourceType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_SourceType_SelectedIndexChanged">
                                                        <asp:ListItem Value="Other" Text="<%$ Resources:lang,QiTa%>" />
                                                        <asp:ListItem Value="AssetAO" Text="<%$ Resources:lang,ShenQingDan%>" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="60px"  class="formItemBgStyleForAlignLeft">ID:</td>
                                                <td width="15%"  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_SourceID" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Precision="0" Width="35px">0</NickLee:NumberBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Button ID="BT_SelectAO" runat="server" Text="<%$ Resources:lang,ShenQingDan%>" Visible="false" />
                                                    <cc1:ModalPopupExtender ID="BT_SelectAO_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" Y="150"
                                                        CancelControlID="IMBT_CloseAO" Enabled="True" PopupControlID="Panel2" TargetControlID="BT_SelectAO">
                                                    </cc1:ModalPopupExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,CunFangWeiZhi%>"></asp:Label>：</td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="4"  >
                                                    <asp:TextBox ID="TB_ToPosition" runat="server" Width="60%"></asp:TextBox>
                                                    <asp:DropDownList ID="DL_WareHouse" runat="server" AutoPostBack="True" DataTextField="WHName" DataValueField="WHName" Height="25px" OnSelectedIndexChanged="DL_WareHouse_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZiChanDaiMa%>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_AssetCode" runat="server" Width="80px"></asp:TextBox>
                                                </td>
                                                <td style="width: 10%;" class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                                </td>
                                                <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                    <asp:DropDownList ID="DL_Type" runat="server" CssClass="DDList" DataTextField="Type"
                                                        DataValueField="Type">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft" colspan="4">
                                                    <asp:TextBox ID="TB_AssetName" runat="server" Width="70%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>：
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="4" >
                                                    <asp:TextBox ID="TB_ModelNumber" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft" colspan="4">
                                                    <asp:TextBox ID="TB_Spec" runat="server" Height="50px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                                    <asp:Button ID="BT_FindAsset" runat="server" CssClass="inpu" OnClick="BT_FindAsset_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                    <asp:Button ID="BT_Clear" runat="server" CssClass="inpu" OnClick="BT_Clear_Click" Text="<%$ Resources:lang,QingKong%>" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Number" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Width="53px">0.00</NickLee:NumberBox>
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label>：
                                                </td>
                                                <td colspan="2"  class="formItemBgStyleForAlignLeft">
                                                    <NickLee:NumberBox ID="NB_Price" runat="server" MaxAmount="1000000000000" MinAmount="-1000000000000" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Width="53px">0.00</NickLee:NumberBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft">

                                                    <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName">
                                                    </asp:DropDownList>

                                                </td>
                                                <td class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                                <td colspan="2"  class="formItemBgStyleForAlignLeft">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ShengChanChangJia%>"></asp:Label>：
                                                </td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="4"  >
                                                    <asp:TextBox ID="TB_Manufacturer" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="formItemBgStyleForAlignLeft">
                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,TuiHuoYuanYin%>"></asp:Label>：
                                                </td>
                                                <td  class="formItemBgStyleForAlignLeft" colspan="4">
                                                    <asp:TextBox ID="TB_ReturnReason" runat="server" Width="90%"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                <td class="formItemBgStyleForAlignLeft" colspan="4"  >

                                                    <asp:Label ID="LB_DepartString" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="资产列表">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,ZiChanKuCunLieBiao%>"></asp:Label>
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,QXQYTKDZC %>"></asp:Label>：
                                                                        <div id="AssetListDivID" style="width: 100%; height: 300px; overflow: auto;">
                                                                            <table width="150%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                <tr>
                                                                                    <td width="7">
                                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="10%" align="left"><strong>
                                                                                                    <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong> </td>
                                                                                                <td width="14%" align="left"><strong>
                                                                                                    <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong> </td>
                                                                                                <td width="10%" align="left"><strong>
                                                                                                    <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong> </td>
                                                                                                <td width="14%" align="left"><strong>
                                                                                                    <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong> </td>
                                                                                                <td width="10%" align="left"><strong>
                                                                                                    <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label></strong> </td>
                                                                                                <td width="6%" align="left"><strong>
                                                                                                    <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong> </td>
                                                                                                <td width="6%" align="left"><strong>
                                                                                                    <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong> </td>
                                                                                                <td width="10%" align="left"><strong>
                                                                                                    <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,RuKuShiJian %>"></asp:Label></strong> </td>
                                                                                                <td width="10%" align="left"><strong>
                                                                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,ChangJia %>"></asp:Label></strong> </td>
                                                                                                <td width="10%" align="left"><strong>
                                                                                                    <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,DiZhi %>"></asp:Label></strong> </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td width="6" align="right">
                                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                </tr>
                                                                            </table>
                                                                            <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                                Height="1px" Width="150%" OnItemCommand="DataGrid2_ItemCommand" CellPadding="4"
                                                                                ForeColor="#333333" GridLines="None">

                                                                                <Columns>
                                                                                    <asp:BoundColumn DataField="ID" HeaderText="Number" Visible="False">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:TemplateColumn HeaderText="Code">
                                                                                        <ItemTemplate>
                                                                                            <asp:Button ID="BT_AssetCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"AssetCode") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                    </asp:TemplateColumn>
                                                                                    <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTAssetInforView.aspx?AssetID={0}"
                                                                                        DataTextField="AssetName" HeaderText="Name" Target="_blank">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="14%" />
                                                                                    </asp:HyperLinkColumn>
                                                                                    <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="14%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Type" HeaderText="Type">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="6%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="BuyTime" HeaderText="入库时间">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Position" HeaderText="地址">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                    </asp:BoundColumn>
                                                                                </Columns>
                                                                                <EditItemStyle BackColor="#2461BF" />
                                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                <ItemStyle CssClass="itemStyle" />
                                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                            </asp:DataGrid>
                                                                        </div>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel6" runat="server" HeaderText="资产查询列表">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label513" runat="server" Text="<%$ Resources:lang,ZCCXLB%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>
                                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,QXQYTKDZC%>"></asp:Label>：
                                                                        <div id="Div1" style="width: 100%; height: 300px; overflow: auto;">
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                <tr>
                                                                                    <td width="7">
                                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="15%" align="left"><strong>
                                                                                                    <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong> </td>
                                                                                                <td width="20%" align="left"><strong>
                                                                                                    <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>
                                                                                                <td width="10%" align="left"><strong>
                                                                                                    <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong> </td>
                                                                                                <td width="35%" align="left"><strong>
                                                                                                    <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong> </td>
                                                                                                <td width="20%" align="left"><strong>
                                                                                                    <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong> </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td width="6" align="right">
                                                                                        <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                </tr>
                                                                            </table>
                                                                            <asp:DataGrid ID="DataGrid7" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                                Height="1px" Width="100%" OnItemCommand="DataGrid7_ItemCommand" CellPadding="4"
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
                                                                                    <asp:BoundColumn DataField="SmallType" HeaderText="Model">
                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="Specification" HeaderText="Specification">
                                                                                        <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="35%" />
                                                                                    </asp:BoundColumn>
                                                                                    <asp:BoundColumn DataField="PurchasePrice" HeaderText="采购单价">
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
                                            <cc1:TabPanel ID="TabPanel1" runat="server" TabIndex="1">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,ShenQingDan%>"></asp:Label>:
                                                                        <asp:Label ID="LB_A0ID" runat="server"></asp:Label>&#160;<asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,MingXi%>"></asp:Label>：
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <br />
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                        <tr>
                                                            <td width="7">
                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> </td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,ZiChanDaiMa%>"></asp:Label></strong> </td>
                                                                        <td width="15%" align="left"><strong>
                                                                            <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,ZiChanMingCheng%>"></asp:Label></strong> </td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong> </td>
                                                                        <td width="15%" align="left"><strong>
                                                                            <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong> </td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong> </td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label65" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong> </td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,ChangJia%>"></asp:Label></strong> </td>
                                                                        <td width="10%" align="left"><strong>IP</strong> </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" Height="30px"
                                                        Width="100%" CellPadding="4" ShowHeader="False" ForeColor="#333333" OnItemCommand="DataGrid4_ItemCommand"
                                                        GridLines="None">
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Number">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="AssetCode" HeaderText="Code">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="AssetName" HeaderText="资产名">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Unit" HeaderText="Unit">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="IP" HeaderText="IP">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                        </Columns>

                                                        <ItemStyle CssClass="itemStyle" />
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    </asp:DataGrid><asp:Label ID="LB_Sql4" runat="server" Visible="False"></asp:Label>
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

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>



                    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 550px; height: 350px; overflow: auto;">
                            <table>
                                <tr>
                                    <td>
                                        <table width="500px" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                            <tr>
                                                <td width="7">
                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                </td>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="20%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="40%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,ShenQingMingCheng%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="20%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label></strong>
                                                            </td>
                                                            <td width="20%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="6" align="right">
                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" OnItemCommand="DataGrid3_ItemCommand"
                                            Width="500px" AllowPaging="True" PageSize="10" OnPageIndexChanged="DataGrid3_PageIndexChanged"
                                            ShowHeader="false" CellPadding="4" ForeColor="#333333" GridLines="None">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Number">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                    <ItemTemplate>
                                                        <asp:Button ID="BT_AAID" runat="server" CssClass="inpu" Style="text-align: center"
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"AAID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="AAName" HeaderText="申请名称">
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="40%" />
                                                </asp:BoundColumn>
                                                <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInfoSimple.aspx?UserCode={0}"
                                                    DataTextField="ApplicantName" HeaderText="Applicant" Target="_blank">
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                </asp:HyperLinkColumn>
                                                <asp:TemplateColumn HeaderText="Status">
                                                    <ItemTemplate>
                                                        <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditItemStyle BackColor="#2461BF" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                            <ItemStyle CssClass="itemStyle" />
                                        </asp:DataGrid>
                                    </td>
                                    <td style="width: 50px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_CloseAO" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
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
