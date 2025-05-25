<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTMakeCar.aspx.cs" Inherits="TTMakeCar" %>

<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />

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
                        <table cellpadding="0" cellspacing="0" width="100%" align="left" class="bian">
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,CheLiangDangAnGuanLi%>"></asp:Label>
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
                                <td align="left">
                                    <table cellpadding="2" cellspacing="0" class="formBgStyle" width="100%">
                                        <tr>
                                            <td class="formItemBgStyleForAlignRight" style="padding: 5px 5px 5px 5px;">
                                                <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft" style="padding: 5px 5px 5px 5px;">
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
                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                  
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="LB_DGProjectID" runat="server" Text="<%$ Resources:lang,BianHao%>" /></strong>
                                                                    </td>
                                                                    <td width="20%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,ZuoWeiShu%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ZaiZhong%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="15%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,GuiShu%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="1px"
                                                    OnItemCommand="DataGrid2_ItemCommand" Width="100%" CellPadding="4" ForeColor="#333333"
                                                    GridLines="None" ShowHeader="false">
                                                    <ItemStyle CssClass="itemStyle" />
                                                    <HeaderStyle Horizontalalign="left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
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
                                                        <asp:BoundColumn DataField="CarCode" HeaderText="CarCode">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CarName" HeaderText="Name">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CarBrand" HeaderText="Brand">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CarType" HeaderText="Type">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="SeatNumber" HeaderText="×ùÎ»Êý">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DWT" HeaderText="ÔØÖØ">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BelongDepartName" HeaderText="¹éÊô">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                            <HeaderStyle BorderColor="#394f66" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true"
                                                                Horizontalalign="left" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditItemStyle BackColor="#2461BF" />
                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:TextBox ID="TB_DepartString" runat="server" Style="visibility: hidden"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 273px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 220px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView1" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
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


                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label27" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table cellpadding="2" cellspacing="0" class="formBgStyle" width="733px">
                                <tr>
                                    <td style="width: 150px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ChePaiHaoMa%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_CarCode" runat="server" CssClass="shuru" Width="150px"></asp:TextBox>
                                        <asp:Label ID="LB_CarCode" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_CarName" runat="server" CssClass="shuru" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td  style="width: 150px; height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,PinPai%>"></asp:Label>£º
                                    </td>
                                    <td  style="height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_CarBrand" runat="server" Width="150px" CssClass="shuru"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:DropDownList ID="DL_CarType" runat="server" DataTextField="Type" DataValueField="Type">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YanSe%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_CarColor" runat="server" CssClass="shuru" Width="150px"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ZaiZhong%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_DWT" runat="server" Width="79px">0.00</NickLee:NumberBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td  style="width: 150px; height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,ZuoWeiShu%>"></asp:Label>£º
                                    </td>
                                    <td  style="height: 30px" class="formItemBgStyleForAlignLeft">
                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_SeatNumber" runat="server" Width="79px" Precision="0">0</NickLee:NumberBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,YouHao%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_FuelConsumption" runat="server" Width="79px">0.00</NickLee:NumberBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,ChuShiLiCheng%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_InitialMileage" runat="server" Width="79px">0.00</NickLee:NumberBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">&nbsp;
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">&nbsp;
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,FaDongJiDaiMa%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_EngineCode" runat="server" CssClass="shuru" Width="150px"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,CheShenDaiMa%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_FrameCode" runat="server" CssClass="shuru" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,GongYingShang%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_Vendor" runat="server" CssClass="shuru" Width="150px"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,JiaGe%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Price" runat="server" Width="79px">0.00</NickLee:NumberBox>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td class="formItemBgStyleForAlignLeft" style="width: 150px; height: 30px">
                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,CaiGouShiJian%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">

                                        <asp:TextBox ID="DLC_BuyTime" ReadOnly="false" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_BuyTime">
                                        </ajaxToolkit:CalendarExtender>

                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,GuiShuBuMen%>"></asp:Label>£º
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 30px">
                                        <asp:TextBox ID="TB_DepartCode" runat="server" Width="80px"></asp:TextBox>
                                        <cc1:ModalPopupExtender ID="TB_DepartCode_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" Y="150"
                                            CancelControlID="IMBT_Close" Enabled="True" PopupControlID="Panel1" TargetControlID="TB_DepartCode">
                                        </cc1:ModalPopupExtender>
                                        <asp:Label ID="LB_DepartName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="width: 150px; height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>£º
                                    </td>
                                    <td colspan="3"  style="height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_Status" runat="server">
                                            <asp:ListItem Value="InUse" Text="<%$ Resources:lang,ZaiYong%>" />
                                            <asp:ListItem Value="Maintenance" Text="<%$ Resources:lang,WeiXiu%>" />
                                            <asp:ListItem Value="Scrapped" Text="<%$ Resources:lang,BaoFei%>" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                            </table>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_New" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>


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
