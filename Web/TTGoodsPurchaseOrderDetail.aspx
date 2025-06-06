<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTGoodsPurchaseOrderDetail.aspx.cs" Inherits="TTGoodsPurchaseOrderDetail" %>

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
    <script type="text/javascript" language="javascript">
        $(function () { if (top.location != self.location) { } else { CloseWebPage(); }



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
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,ShangPinCaiGouShenQing%>"></asp:Label>
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
                                <td style="padding: 5px 5px 5px 5px;">

                                    <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                        <tr>
                                            <td class="formItemBgStyleForAlignLeft"  colspan="2">
                                                <table width="98%" cellpadding="3" cellspacing="0" class="formBgStyle">
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft"  width="15%">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,CaiGouDanHao%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" style="width: 20%;">
                                                            <asp:Label ID="LB_POID" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_POName" runat="server" Width="98%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,CaiGouYuanDaiMa%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                            <asp:TextBox ID="TB_PurManCode" runat="server" Width="80px"></asp:TextBox>
                                                            <asp:Label ID="LB_PurManName" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,CaiGouShiJian%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" style="width: 106px;">

                                                            <asp:TextBox ID="DLC_PurTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender2" runat="server" TargetControlID="DLC_PurTime">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,YuJiDaoHuoShiJian%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="DLC_ArrivalTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_ArrivalTime">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="3" style="width: 106px;">
                                                            <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Amount" runat="server" Enabled="False" OnBlur="" OnFocus=""
                                                                OnKeyPress="" PositiveColor="" Width="85px" Precision="3">
                                                                0.00
                                                            </NickLee:NumberBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,BeiZhu%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                            <asp:TextBox ID="TB_Comment" runat="server" Width="99%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft" style="width: 106px;">
                                                            <asp:DropDownList ID="DL_POStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DL_POStatus_SelectedIndexChanged">
                                                                <asp:ListItem Value="New" Text="<%$ Resources:lang,XinJian%>" />
                                                                <asp:ListItem Value="InProgress" Text="<%$ Resources:lang,ShenPiZhong%>" />
                                                                <asp:ListItem Value="Procuring" Text="<%$ Resources:lang,CaiGouZhong%>" />
                                                                <asp:ListItem Value="Cancel" Text="<%$ Resources:lang,QuXiao%>" />
                                                                <asp:ListItem Value="Completed" Text="<%$ Resources:lang,WanCheng%>" />
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,GuanLian%>"></asp:Label>：
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="DL_RelatedType" runat="server">
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
                                                                        <asp:Button ID="BT_Select" runat="server" Text="<%$ Resources:lang,XuanZe%>" />
                                                                        <cc1:ModalPopupExtender ID="TB_Select_ModalPopupExtender" runat="server" Enabled="True"
                                                                            TargetControlID="BT_Select" PopupControlID="Panel2" CancelControlID="IMBT_Close"
                                                                            BackgroundCssClass="modalBackground" Y="150">
                                                                        </cc1:ModalPopupExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft"></td>
                                                        <td class="formItemBgStyleForAlignLeft" colspan="3" >
                                                            <asp:Button ID="BT_UpdatePO" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_UpdatePO_Click"
                                                                Text="<%$ Resources:lang,BaoCun%>" />

                                                            <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label>
                                                            <asp:Label ID="LB_UserName" runat="server"
                                                                Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; width: 70%;">
                                                <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                                                    <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="采购单明细">

                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,CaiGouDanMingXii%>"></asp:Label>
                                                        </HeaderTemplate>

                                                        <ContentTemplate>

                                                            <table align="left" cellpadding="0" cellspacing="0" width="98%">

                                                                <tr>

                                                                    <td align="left">

                                                                        <asp:Label ID="LB_GoodsOwner" runat="server" Font-Bold="True" Width="100%"></asp:Label>
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

                                                                                            <td align="left" width="10%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong>
                                                                                            </td>



                                                                                            <td align="left" width="15%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="10%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong>
                                                                                            </td>
                                                                                            <td align="left" width="20%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="8%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="8%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="10%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="10%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,ShenQingRen %>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="10%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,YiRuKu %>"></asp:Label></strong>
                                                                                            </td>
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
                                                                            Width="100%">

                                                                            <Columns>

                                                                                <asp:TemplateColumn HeaderText="Number">

                                                                                    <ItemTemplate>

                                                                                        <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                    </ItemTemplate>

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:TemplateColumn>


                                                                                <asp:BoundColumn DataField="GoodsName" HeaderText="Name">

                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Spec" HeaderText="Specification">

                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="20%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Number" HeaderText="Quantity">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Unit" HeaderText="Unit">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:HyperLinkColumn DataNavigateUrlField="ApplicantCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                                    DataTextField="ApplicantName" HeaderText="Applicant" Target="_blank">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                </asp:HyperLinkColumn>

                                                                                <asp:BoundColumn DataField="CheckInNumber" HeaderText="已入库">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
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

                                                            <table width="100%">
                                                                <tr>
                                                                    <td width="50%">
                                                                        <table align="left" cellpadding="3" cellspacing="0" class="formBgStyle" width="98%">

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <asp:Label ID="LB_ID" runat="server"></asp:Label>
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ShenGouZheDaiMa %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="2"  >

                                                                                    <asp:TextBox ID="TB_ApplicantCode" runat="server" Width="80px"></asp:TextBox>

                                                                                    <asp:Label ID="LB_ApplicantName" runat="server"></asp:Label>

                                                                                    (---&gt;<asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,KeYiCongYouBianXuanQuRenYuan %>"></asp:Label>)
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <asp:DropDownList ID="DL_Type" runat="server" AutoPostBack="True" DataTextField="Type"
                                                                                        DataValueField="Type">
                                                                                    </asp:DropDownList>
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="2"  >

                                                                                    <asp:TextBox ID="TB_GoodsName" runat="server" Height="20px" Width="350px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>：</td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="4"  >

                                                                                    <asp:TextBox ID="TB_ModelNumber" runat="server" Height="20px" Width="99%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="4" style="height: 15px;">

                                                                                    <asp:TextBox ID="TB_Spec" runat="server" Height="48px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,GouMaiLiYou %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="4" style="height: 1px; ">

                                                                                    <asp:TextBox ID="TB_PurReason" runat="server" Height="42px" TextMode="MultiLine"
                                                                                        Width="500px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="2"  >

                                                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Number" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                                                        PositiveColor="" Width="80px" Precision="3">0.000</NickLee:NumberBox>
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft">

                                                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Price" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                                                        PositiveColor="" Width="85px" Precision="3">0.000</NickLee:NumberBox>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="2"  >

                                                                                    <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName"
                                                                                        Width="64px">
                                                                                    </asp:DropDownList>
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft"></td>

                                                                                <td class="formItemBgStyleForAlignLeft"></td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft">
                                                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,GongYingShang %>"></asp:Label>：
                                                                                </td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="4" style="height: 18px; ">

                                                                                    <asp:TextBox ID="TB_Supplier" runat="server" Width="280px"></asp:TextBox>

                                                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,GongYingShangDianHua %>"></asp:Label>：

                                                                                <asp:TextBox ID="TB_SupplierPhone" runat="server" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>

                                                                                <td class="formItemBgStyleForAlignLeft"></td>

                                                                                <td class="formItemBgStyleForAlignLeft" colspan="4"  >

                                                                                    <asp:Button ID="BT_New" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_New_Click"
                                                                                        Text="<%$ Resources:lang,XinZeng %>" />

                                                                                    <asp:Button ID="BT_Update" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Update_Click"
                                                                                        Text="<%$ Resources:lang,BaoCun %>" />

                                                                                    <asp:Button ID="BT_Delete" runat="server" CssClass="inpu" Enabled="False" OnClick="BT_Delete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())"
                                                                                        Text="<%$ Resources:lang,ShanChu %>" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                    </td>

                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td style="border-right: solid 1px #D8D8D8;" valign="top" width="170">
                                                                                    <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                                        width="170px">
                                                                                        <tr>
                                                                                            <td width="7">
                                                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="left">
                                                                                                            <strong>
                                                                                                                <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,ZhiJieChengYuan %>"></asp:Label></strong>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td align="right" width="6">
                                                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                                        ForeColor="#333333" GridLines="None" OnItemCommand="DataGrid3_ItemCommand" ShowHeader="False"
                                                                                        Width="170px">
                                                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                        <EditItemStyle BackColor="#2461BF" />
                                                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                                        <ItemStyle CssClass="itemStyle" />
                                                                                        <Columns>
                                                                                            <asp:TemplateColumn HeaderText="部门人员：">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Button ID="BT_UserCode" runat="server" CssClass="inpu"  Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>' />
                                                                                                    <asp:Button ID="BT_UserName" runat="server" CssClass="inpu"  Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                                            </asp:TemplateColumn>
                                                                                        </Columns>
                                                                                    </asp:DataGrid>
                                                                                </td>
                                                                                <td align="left" style="width: 220px;" valign="top"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>


                                                        </ContentTemplate>
                                                    </cc1:TabPanel>
                                                    <cc1:TabPanel ID="TabPanel2" runat="server">

                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,GongZuoLiuDingYi%>"></asp:Label>
                                                        </HeaderTemplate>

                                                        <ContentTemplate>

                                                            <table cellpadding="3" cellspacing="0" class="formBgStyle" width="100%">

                                                                <tr style="font-size: 10pt">

                                                                    <td class="formItemBgStyleForAlignLeft" colspan="2" style="height: 10px; ">

                                                                        <strong>
                                                                            <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,GouMaiShenQingGongZuoLiu%>"></asp:Label>：</strong>
                                                                    </td>
                                                                </tr>

                                                                <tr style="font-size: 10pt">

                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                                                    </td>

                                                                    <td class="formItemBgStyleForAlignLeft">

                                                                        <asp:DropDownList ID="DL_WFType" runat="server">
                                                                            <asp:ListItem Value="MaterialProcurement" Text="<%$ Resources:lang,LiaoPingCaiGou%>" />
                                                                        </asp:DropDownList>

                                                                        <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,GongZuoLiuMuBan%>"></asp:Label>：<asp:DropDownList ID="DL_TemName" runat="server" DataTextField="TemName" DataValueField="TemName"
                                                                            Width="144px">
                                                                        </asp:DropDownList>

                                                                        <asp:HyperLink ID="HL_WLTem" runat="server" NavigateUrl="~/TTWorkFlowTemplate.aspx"
                                                                            Target="_blank">
                                                                            <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,MuBanWeiHu%>"></asp:Label>
                                                                        </asp:HyperLink>

                                                                        <asp:Button ID="BT_Reflash" runat="server" CssClass="inpu" OnClick="BT_Reflash_Click"
                                                                            Text="<%$ Resources:lang,ShuaXin%>" />
                                                                    </td>
                                                                </tr>

                                                                <tr style="font-size: 10pt">

                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                                                    </td>

                                                                    <td class="formItemBgStyleForAlignLeft">

                                                                        <asp:TextBox ID="TB_WLName" runat="server" Width="387px"></asp:TextBox>
                                                                    </td>
                                                                </tr>

                                                                <tr style="font-size: 10pt">

                                                                    <td class="formItemBgStyleForAlignLeft">
                                                                        <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,ShuoMing%>"></asp:Label>：
                                                                    </td>

                                                                    <td class="formItemBgStyleForAlignLeft">

                                                                        <asp:TextBox ID="TB_Description" runat="server" Height="48px" TextMode="MultiLine"
                                                                            Width="441px"></asp:TextBox>
                                                                    </td>
                                                                </tr>

                                                                <tr style="font-size: 10pt">

                                                                    <td class="formItemBgStyleForAlignLeft"></td>

                                                                    <td class="formItemBgStyleForAlignLeft" style="height: 19px;">（<asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,YaoQiuShouDaoChuLiXinXi%>"></asp:Label>：<asp:CheckBox ID="CB_SMS" runat="server" Text="<%$ Resources:lang,DuanXin%>" />

                                                                        <asp:CheckBox ID="CB_Mail" runat="server" Text="<%$ Resources:lang,YouJian%>" />

                                                                        <span style="font-size: 10pt">) </span>

                                                                        <asp:Button ID="BT_SubmitApply" runat="server" CssClass="inpu" Enabled="False" Text="<%$ Resources:lang,TiJiaoShenQing%>" />

                                                                        <cc1:ModalPopupExtender ID="BT_SubmitApply_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" Y="150"
                                                                            DynamicServicePath="" Enabled="True" PopupControlID="Panel1" TargetControlID="BT_SubmitApply">
                                                                        </cc1:ModalPopupExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <table cellpadding="0" cellspacing="0" width="100%">

                                                                <tr style="font-size: 10pt;">

                                                                    <td style="height: 14px; text-align: left">
                                                                        <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,DuiYingShenPiJiLu%>"></asp:Label>：
                                                                    </td>
                                                                </tr>

                                                                <tr style="font-size: 10pt">

                                                                    <td style="height: 11px; text-align: left">

                                                                        <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                            width="100%">

                                                                            <tr>

                                                                                <td width="7">

                                                                                    <img height="26" src="ImagesSkin/main_n_l.jpg" width="7" />
                                                                                </td>

                                                                                <td>

                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                                        <tr>

                                                                                            <td align="left" width="10%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="55%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="15%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="10%">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td align="left" width="10%">

                                                                                                <strong></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td align="right" width="6">

                                                                                    <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" />
                                                                                </td>
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

                                                                                <asp:BoundColumn DataField="CreateTime" HeaderText="申请时间">

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
                                                                        </asp:DataGrid>
                                                                    </td>
                                                                </tr>

                                                                <tr style="font-size: 10pt">

                                                                    <td style="text-align: right">

                                                                        <asp:Label ID="LB_Sql5" runat="server" Visible="False"></asp:Label>

                                                                        <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc1:TabPanel>
                                                    <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="关联合同">

                                                        <HeaderTemplate>
                                                            关联合同
                                                        </HeaderTemplate>

                                                        <ContentTemplate>

                                                            <table align="left" cellpadding="0" cellspacing="0" width="98%">

                                                                <tr>

                                                                    <td align="left">

                                                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Width="100%"></asp:Label>
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

                                                                                            <td width="7%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,HeTongDaiMa%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="17%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,HeTongMingCheng%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="5%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="5%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="8%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,QianDingRiQi%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="7%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="5%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="10%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,JiaFangDanWei%>"></asp:Label></strong>
                                                                                            </td>

                                                                                            <td width="10%" align="left">

                                                                                                <strong>
                                                                                                    <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,YiFangDaiWei%>"></asp:Label></strong>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>

                                                                                <td width="6" align="right">

                                                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                            </tr>
                                                                        </table>

                                                                        <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                            Height="1px" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                                                            <Columns>

                                                                                <asp:BoundColumn DataField="ConstractCode" HeaderText="ContractCode">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:HyperLinkColumn DataNavigateUrlField="ConstractCode" DataNavigateUrlFormatString="TTConstractView.aspx?ConstractCode={0}"
                                                                                    DataTextField="ConstractName" HeaderText="ContractName" Target="_blank">

                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="17%" />
                                                                                </asp:HyperLinkColumn>

                                                                                <asp:BoundColumn DataField="Type" HeaderText="Type">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:TemplateColumn HeaderText="Status">
                                                                                    <ItemTemplate>
                                                                                        <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="5%" />
                                                                                </asp:TemplateColumn>

                                                                                <asp:BoundColumn DataField="SignDate" HeaderText="SigningDate" DataFormatString="{0:yyyy/MM/dd}">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Amount" HeaderText="Amount">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="7%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="Currency" HeaderText="Currency">

                                                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="PartA" HeaderText="PartyAUnit">

                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                </asp:BoundColumn>

                                                                                <asp:BoundColumn DataField="PartB" HeaderText="乙方单位">

                                                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                </asp:BoundColumn>
                                                                            </Columns>



                                                                            <ItemStyle CssClass="itemStyle" />

                                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                            <EditItemStyle BackColor="#2461BF" />

                                                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                                            <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                                        </asp:DataGrid>

                                                                        <asp:Label ID="Label4" runat="server" Visible="False"></asp:Label>

                                                                        <asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Size="9pt"
                                                                            Visible="False" Width="57px"></asp:Label>

                                                                        <asp:Label ID="Label6" runat="server" Font-Bold="False" Font-Size="9pt"
                                                                            Visible="False" Width="57px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </cc1:TabPanel>
                                                </cc1:TabContainer>
                                            </td>

                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;"
                        Width="500px">
                        <div>
                            <table>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,LCSQSCHYLJDLCJHYMQJHM%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="BT_ActiveYes" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Shi%>" OnClick="BT_ActiveYes_Click" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
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
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor ="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_Close" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
