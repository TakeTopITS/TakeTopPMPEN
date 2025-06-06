<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTMakeVendor.aspx.cs" Inherits="TTMakeVendor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="NickLee.Web.UI" Namespace="NickLee.Web.UI" TagPrefix="NickLee" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="mainCss" href="css/bluelightmain.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #AboveDiv {
            min-width: 1100px;
            width: expression (document.body.clientWidth <= 1100? "1100px" : "auto" ));
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,GongYingShangDangAn%>"></asp:Label>
                                                        </td>
                                                        <td width="5">
                                                            <%-- <img src="ImagesSkin/main_top_r.jpg" width="5" height="31" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="padding-top: 5px;">
                                                <asp:HyperLink ID="HL_VendorInfoImport" NavigateUrl="TTVendorInfoImport.aspx" runat="server" Text="<%$ Resources:lang,GongYingShangXinXiDaoRu%>"></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" cellpadding="0" cellspacing="0" style="padding-bottom: 5px;">
                                        <tr>
                                            <td width="100%" style="padding: 10px 5px 5px 5px; border-right: solid 1px #D8D8D8"
                                                valign="top">

                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="padding-bottom: 5px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_VenCode" runat="server" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_VenName" runat="server" Width="120px"></asp:TextBox>
                                                        </td>

                                                        <td>
                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>：
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TB_IndustryTypeFind" runat="server" Width="120px"></asp:TextBox>
                                                            <asp:DropDownList ID="DL_IndustryTypeFind" runat="server" AutoPostBack="True" DataTextField="Type"
                                                                DataValueField="Type" OnSelectedIndexChanged="DL_IndustryTypeFind_SelectedIndexChanged"
                                                                Width="120px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: right;">

                                                            <asp:Label ID="Label84" runat="server" Text="<%$ Resources:lang,SheBeiMingCheng%>"></asp:Label>：
                                                        </td>

                                                        <td style="text-align: left;">

                                                            <asp:TextBox ID="TB_DeviceNameFind" runat="server" Width="200px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="BT_Find" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_Find_Click" />
                                                        </td>

                                                        <td align="right" width="30%">
                                                            <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                                        </td>
                                                    </tr>
                                                </table>

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
                                                                            <asp:Label ID="Label87" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label88" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                   
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="15%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label77" runat="server" Text="<%$ Resources:lang,LianXiRen%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label78" runat="server" Text="<%$ Resources:lang,DianHua%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="Label81" runat="server" Text="<%$ Resources:lang,QuYu%>"></asp:Label></td>
                                                                                    <td>
                                                                                        <asp:Button ID="BT_SortByAreaAddress" CssClass="inpuUpDown" Width="20px" runat="server" OnClick="BT_SortByAreaAddress_Click" />
                                                                                        <asp:Label ID="LB_UpDown" runat="server" Text="UP" Visible="false"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </strong>
                                                                    </td>

                                                                    <td width="15%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label79" runat="server" Text="<%$ Resources:lang,DiZhi%>"></asp:Label></strong>
                                                                    </td>

                                                                    <%--   <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label80" runat="server" Text="<%$ Resources:lang,JianLiRiQi%>"></asp:Label></strong>
                                                                    </td>--%>
                                                                    <td width="10%" align="left">
                                                                        <strong></strong>
                                                                    </td>

                                                                    <td width="15%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label85" runat="server" Text="<%$ Resources:lang,SheBeiMingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="6" align="right">
                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                    ShowHeader="false" Height="1px" OnItemCommand="DataGrid2_ItemCommand" PageSize="25"
                                                    OnPageIndexChanged="DataGrid2_PageIndexChanged" Width="100%" CellPadding="4"
                                                    ForeColor="#333333" GridLines="None">
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
                                                        <asp:BoundColumn DataField="VendorCode" HeaderText="Code">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:HyperLinkColumn DataNavigateUrlField="VendorCode" DataNavigateUrlFormatString="TTVendorInfoView.aspx?VendorCode={0}"
                                                            DataTextField="VendorName" HeaderText="供应商名称" Target="_blank">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                        </asp:HyperLinkColumn>
                                                        <asp:BoundColumn DataField="ContactName" HeaderText="联系人">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Tel1" HeaderText="Telephone">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AreaAddress" HeaderText="区域">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="RegistrationAddressCN" HeaderText="地址">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                        </asp:BoundColumn>



                                                        <%-- <asp:BoundColumn DataField="CreateDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="建立日期">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>--%>

                                                        <asp:HyperLinkColumn DataNavigateUrlField="VendorCode" DataNavigateUrlFormatString="TTContactList.aspx?RelatedType=Vendor&RelatedID={0}"
                                                            HeaderText="相关联系人" Target="_blank" Text="<%$ Resources:lang,XiangGuanLianXiRen %>">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:HyperLinkColumn>

                                                        <asp:BoundColumn DataField="DeviceName" HeaderText="<%$ Resources:lang,SheBeiMingCheng%>">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
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
                        <asp:Label ID="LB_DepartString" runat="server" Visible="false"></asp:Label>

                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label126" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="6" align="left" style="padding: 5px 5px 5px 5px;">
                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                            Width="100%">
                                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="供应商信息" TabIndex="0">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label75" runat="server" Text="<%$ Resources:lang,GongYingShangXinXi%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <table width="100%" cellpadding="3" cellspacing="0" class="formBgStyle">

                                                        <tr>

                                                            <td style="width: 10%; " class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>：
                                                            </td>

                                                            <td style="width: 30%; "  class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_VendorCode" runat="server" Width="95%"></asp:TextBox><span style="color: #ff0000">*</span>

                                                                <asp:Label ID="LB_VendorCode" runat="server" Visible="false"></asp:Label>
                                                            </td>

                                                            <td style="width: 20%; " class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>：
                                                            </td>

                                                            <td width="35%"  class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_VendorName" runat="server" Style="margin-left: 0px" Width="95%"></asp:TextBox>

                                                                <span style="color: #ff0000">*</span></td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,YingWenMing %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_VendorEnglishName" runat="server" Height="20px" Style="margin-left: 0px"
                                                                    Width="95%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,HangYeLeiXing %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_Type" runat="server" Width="60%"></asp:TextBox>

                                                                <asp:DropDownList ID="DL_IndustryType" runat="server" AutoPostBack="True" DataTextField="Type"
                                                                    DataValueField="Type" OnSelectedIndexChanged="DL_IndustryType_SelectedIndexChanged">
                                                                </asp:DropDownList>

                                                                <span style="color: #ff0000">*</span></td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,KaiHuYinHang %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_Bank" runat="server" Width="95%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,YinHangZhangHao %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_BankAccount" runat="server" Width="99%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,FaPiaoDiZhi %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_InvoiceAddress" TextMode="MultiLine" runat="server" Height="60px"
                                                                    Width="95%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,ZhuYaoLianXiRen %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_ContactName" runat="server" Width="99%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,DianHuaYi %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_Tel1" runat="server" Width="99%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,DianHuaEr %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_Tel2" runat="server" Width="99%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">EMail：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_EMailAddress" runat="server" Width="95%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,YouZhengBianMa %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_ZP" runat="server" Width="99%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,WangZhi %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_WebSite" runat="server" Width="99%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,ChuanZhen %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_Fax" runat="server" Width="99%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,YeWuYuan %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_SalePerson" runat="server" Width="99%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,BiBie%>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:DropDownList ID="DL_CurrencyType" runat="server" DataTextField="Type" DataValueField="Type"
                                                                    Height="25px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ZheKouLv %>"></asp:Label>：
                                                            </td>

                                                            <td style="width: 100px; "  class="formItemBgStyleForAlignLeft">

                                                                <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Discount" runat="server" Height="23px" Width="80px" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,XinYongDengJi %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_CreditRate" runat="server" Height="22px" Width="44px" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,ShuiLV %>"></asp:Label>：</td>

                                                            <td class="formItemBgStyleForAlignLeft" style="width: 100px;">

                                                                <NickLee:NumberBox ID="NB_TaxRate" runat="server" Height="23px" MaxAmount="1000000000000" MinAmount="-1000000000000" Width="80px" OnBlur="" OnFocus="" OnKeyPress="" PositiveColor="" Precision="3">0.000</NickLee:NumberBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,JieSuanFangShi %>"></asp:Label>

                                                                ：</td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:DropDownList ID="DL_ClearingForm" runat="server" DataTextField="Type" DataValueField="Type">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,GuoJia %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_Country" runat="server" Width="99%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,ShengFen %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_State" runat="server" Width="99%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,ChengShi %>"></asp:Label>：
                                                            </td>

                                                            <td  class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_City" runat="server" Width="99%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,QuYu %>"></asp:Label>：
                                                            </td>

                                                            <td  class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_AreaAddress" runat="server" Width="99%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,XiangXiDiZhi %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_AddressCN" runat="server" TextMode="MultiLine" Height="60px" Width="95%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,YingWenDiZhi %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_AddressEN" TextMode="MultiLine" Height="60px" runat="server" Width="95%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label76" runat="server" Text="<%$ Resources:lang,GuiShuBuMen %>"></asp:Label>：</td>

                                                            <td class="formItemBgStyleForAlignLeft">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="LB_BelongDepartCode" runat="server"></asp:Label>
                                                                            <asp:Label ID="LB_BelongDepartName" runat="server"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="BT_BelongDepartment" runat="server" CssClass="inpu" Text="<%$ Resources:lang,XuanZhe%>" />
                                                                            <cc1:ModalPopupExtender ID="BT_BelongDepartment_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" CancelControlID="IMBT_CloseTree3" DynamicServicePath="" Enabled="True" PopupControlID="Panel3" TargetControlID="BT_BelongDepartment" Y="150">
                                                                            </cc1:ModalPopupExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,JianLiShiJian %>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="LB_CreateDate" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,SheBeiMingCheng%>"></asp:Label>：
                                                            </td>

                                                            <td colspan="3"  class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_DeviceName" runat="server" Width="95%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,BeiZhu %>"></asp:Label>：
                                                            </td>

                                                            <td colspan="3"  class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_Comment" runat="server" Height="60px" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr style="display: none;">

                                                            <td class="formItemBgStyleForAlignLeft"></td>

                                                            <td colspan="3"  class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>

                                                                <asp:HyperLink ID="HL_RelatedContactInfor"
                                                                    runat="server" Enabled="False" Target="_blank">
                                                                    ---><asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,XiangGuanLianXiRen %>"></asp:Label>
                                                                </asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="相关项目" TabIndex="0">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,XiangGuanXiangMu%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <table>

                                                        <tr>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Label ID="Label74" runat="server" Text="<%$ Resources:lang,XiangMuHao%>"></asp:Label>：
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:TextBox ID="TB_ProjectID" runat="server" Width="99%"></asp:TextBox>
                                                            </td>

                                                            <td class="formItemBgStyleForAlignLeft">

                                                                <asp:Button ID="BT_RelatedProject" runat="server" CssClass="inpu" OnClick="BT_RelatedProject_Click"
                                                                    Text="<%$ Resources:lang,GuanLian%>" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                        <tr>

                                                            <td width="7">

                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                            </td>

                                                            <td>

                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                    <tr>

                                                                        <td width="8%" align="left">

                                                                            <strong>

                                                                                <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                        </td>

                                                                        <td width="20%" align="left">

                                                                            <strong>

                                                                                <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,XiangMuMingCheng%>"></asp:Label></strong>
                                                                        </td>

                                                                        <td width="15%" align="left">

                                                                            <strong>

                                                                                <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,KaiShiRiQi%>"></asp:Label></strong>
                                                                        </td>

                                                                        <td width="15%" align="left">

                                                                            <strong>

                                                                                <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,JieShuRiQi%>"></asp:Label></strong>
                                                                        </td>

                                                                        <td width="15%" align="left">

                                                                            <strong>

                                                                                <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,LiXiangRiQi%>"></asp:Label></strong>
                                                                        </td>

                                                                        <td width="9%" align="left">

                                                                            <strong>

                                                                                <asp:Label ID="Label44" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
                                                                        </td>

                                                                        <td width="10%" align="left">

                                                                            <strong>

                                                                                <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,WanChengChengDu%>"></asp:Label></strong>
                                                                        </td>

                                                                        <td width="8%" align="left">

                                                                            <strong>

                                                                                <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,ShanChu%>"></asp:Label></strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                            <td width="6" align="right">

                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <asp:DataGrid ID="DataGrid4" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        ShowHeader="False" Height="1px" OnItemCommand="DataGrid4_ItemCommand" Width="100%"
                                                        CellPadding="4" ForeColor="#333333" GridLines="None">

                                                        <Columns>

                                                            <asp:BoundColumn DataField="ProjectID" HeaderText="Number">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="ProjectName" HeaderText="项目名称">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="BeginDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="开始日期">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="EndDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="结束日期">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                            </asp:BoundColumn>

                                                            <asp:BoundColumn DataField="MakeDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="立项日期">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                            </asp:BoundColumn>

                                                            <asp:TemplateColumn HeaderText="Status">

                                                                <ItemTemplate>

                                                                    <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                </ItemTemplate>

                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="9%" />
                                                            </asp:TemplateColumn>

                                                            <asp:TemplateColumn HeaderText="完成程度">

                                                                <ItemTemplate>

                                                                    <asp:Label ID="LB_FinishPercent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FinishPercent")%> '></asp:Label>

                                                                    %
                                                                </ItemTemplate>

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:TemplateColumn>

                                                            <asp:ButtonColumn CommandName="Delete" Text="&lt;div&gt;&lt;img src=ImagesSkin/icon_del.gif border=0 alt='Deleted' /&gt;&lt;/div&gt;">

                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
                                                            </asp:ButtonColumn>
                                                        </Columns>

                                                        <ItemStyle CssClass="itemStyle" />

                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                        <EditItemStyle BackColor="#2461BF" />

                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    </asp:DataGrid>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="VisiblePersonnel" TabIndex="0">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,KeShiRenYuan%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <table width="100%" cellpadding="0" cellspacing="0">

                                                        <tr>

                                                            <td style="width: 220px; border-right: solid 1px #D8D8D8; padding: 5px 0px 0px 5px"
                                                                valign="top" align="left">

                                                                <asp:TreeView ID="TreeView2" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged"
                                                                    ShowLines="True" Width="220px">

                                                                    <RootNodeStyle CssClass="rootNode" />

                                                                    <NodeStyle CssClass="treeNode" />

                                                                    <LeafNodeStyle CssClass="leafNode" />

                                                                    <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                                                </asp:TreeView>
                                                            </td>

                                                            <td width="165px" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; vertical-align: top;">

                                                                <table style="width: 165px; height: 53px">

                                                                    <tr>

                                                                        <td style="width: 165; text-align: center;" valign="top">

                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                                                <tr>

                                                                                    <td width="7">

                                                                                        <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                                                    </td>

                                                                                    <td>

                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                                            <tr>

                                                                                                <td align="left">

                                                                                                    <strong>

                                                                                                        <asp:Label ID="Label48" runat="server" Text="<%$ Resources:lang,BuMenRenYuan%>"></asp:Label></strong>
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
                                                                                Width="100%" Height="1px" CellPadding="4" ForeColor="#333333" GridLines="None"
                                                                                ShowHeader="False">

                                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                                <EditItemStyle BackColor="#2461BF" />

                                                                                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                                                                <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                                <ItemStyle CssClass="itemStyle" />

                                                                                <Columns>

                                                                                    <asp:TemplateColumn HeaderText="部门人员：">

                                                                                        <ItemTemplate>

                                                                                            <asp:Button ID="BT_UserCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserCode") %>'
                                                                                                CssClass="inpu" />

                                                                                            <asp:Button ID="BT_UserName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                                                                                CssClass="inpu" />
                                                                                        </ItemTemplate>

                                                                                        <ItemStyle CssClass="itemBorder" Horizontalalign="left" />

                                                                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Small"
                                                                                            Font-Strikeout="False" Font-Underline="False" />
                                                                                    </asp:TemplateColumn>
                                                                                </Columns>

                                                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                            </asp:DataGrid>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                                            <td width="500px" align="left" style="padding: 5px 5px 0px 5px; border-right: solid 1px #D8D8D8; vertical-align: top;">

                                                                <asp:Repeater ID="RP_VendorMember" runat="server" OnItemCommand="RP_VendorMember_ItemCommand">

                                                                    <ItemTemplate>

                                                                        <asp:Button ID="BT_UserName" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"UserName") %>'
                                                                            Width="70px" />
                                                                    </ItemTemplate>
                                                                </asp:Repeater>

                                                                <br />

                                                                <br />

                                                                <table cellpadding="0" cellspacing="0" width="100%">

                                                                    <tr>

                                                                        <td align="left" style="padding-left: 20px; font-weight: bold; height: 24px; color: #394f66; background-image: url('http://localhost:4783/Web/ImagesSkin/titleBG.jpg')">

                                                                            <table>

                                                                                <tr>

                                                                                    <td>

                                                                                        <asp:Label ID="LB_tbActorGroupList" runat="server" Text="<%$ Resources:lang,ActorGroupList %>" />: </td>

                                                                                    <td>（</td>

                                                                                    <td>

                                                                                        <asp:Label ID="Label49" runat="server" Text="<%$ Resources:lang,JueSeZuMingCheng%>"></asp:Label>：</td>

                                                                                    <td>

                                                                                        <asp:TextBox ID="TB_ActorGroupName" runat="server" Width="99%"></asp:TextBox></td>

                                                                                    <td>

                                                                                        <asp:Button ID="BT_FindGroup" runat="server" CssClass="inpu" Text="<%$ Resources:lang,ChaXun%>" OnClick="BT_FindGroup_Click" />
                                                                                    </td>

                                                                                    <td>)
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>

                                                                        <td align="left">

                                                                            <table>

                                                                                <tr>

                                                                                    <td align="left">

                                                                                        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">

                                                                                            <ItemTemplate>

                                                                                                <asp:Button ID="BT_GroupName" runat="server" CssClass="inpuLongRepeat" Text='<%# DataBinder.Eval(Container.DataItem,"GroupName") %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:Repeater>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr>

                                                                                    <td style="height: 10px; text-align: left"></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel7" runat="server" HeaderText="关联合同">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label50" runat="server" Text="<%$ Resources:lang,GuanLianHeTong%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <table align="left" cellpadding="0" cellspacing="0" width="100%">

                                                        <tr>

                                                            <td align="left">

                                                                <asp:Label ID="Label123425" runat="server" Font-Bold="True" Width="100%"></asp:Label></td>
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

                                                                                    <td width="7%" align="left"><strong>

                                                                                        <asp:Label ID="Label51" runat="server" Text="<%$ Resources:lang,HeTongDaiMa%>"></asp:Label></strong> </td>

                                                                                    <td width="17%" align="left"><strong>

                                                                                        <asp:Label ID="Label52" runat="server" Text="<%$ Resources:lang,HeTongMingCheng%>"></asp:Label></strong> </td>

                                                                                    <td width="5%" align="left"><strong>

                                                                                        <asp:Label ID="Label53" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong> </td>

                                                                                    <td width="5%" align="left"><strong>

                                                                                        <asp:Label ID="Label54" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>

                                                                                    <td width="8%" align="left"><strong>

                                                                                        <asp:Label ID="Label55" runat="server" Text="<%$ Resources:lang,QianDingRiQi%>"></asp:Label></strong> </td>

                                                                                    <td width="7%" align="left"><strong>

                                                                                        <asp:Label ID="Label56" runat="server" Text="<%$ Resources:lang,JinE%>"></asp:Label></strong> </td>

                                                                                    <td width="5%" align="left"><strong>

                                                                                        <asp:Label ID="Label57" runat="server" Text="<%$ Resources:lang,BiZhong%>"></asp:Label></strong> </td>

                                                                                    <td width="10%" align="left"><strong>

                                                                                        <asp:Label ID="Label58" runat="server" Text="<%$ Resources:lang,JiaFangDanWei%>"></asp:Label></strong> </td>

                                                                                    <td width="10%" align="left"><strong>

                                                                                        <asp:Label ID="Label59" runat="server" Text="<%$ Resources:lang,YiFangDanWei%>"></asp:Label></strong> </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>

                                                                        <td width="6" align="right">

                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                    </tr>
                                                                </table>

                                                                <asp:DataGrid ID="DataGrid6" runat="server" AutoGenerateColumns="False" ShowHeader="False"
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

                                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
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
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel5" runat="server" HeaderText=" 关联物料采购订单" TabIndex="0">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label60" runat="server" Text="<%$ Resources:lang,ShangPinCaiGouDingDan%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <table width="100%" cellpadding="0" cellspacing="0">

                                                        <tr>

                                                            <td style="border-right: solid 1px #D8D8D8; padding: 10px 5px 5px 5px"
                                                                valign="top" align="left">

                                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                    width="100%">

                                                                    <tr>

                                                                        <td width="7">

                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                                        <td>

                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                                <tr>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label61" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="25%"><strong>

                                                                                        <asp:Label ID="Label62" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="15%"><strong>

                                                                                        <asp:Label ID="Label63" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="20%"><strong>

                                                                                        <asp:Label ID="Label64" runat="server" Text="<%$ Resources:lang,CaiGouShiJian%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label65" runat="server" Text="<%$ Resources:lang,CaiGouYuan%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label66" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>

                                                                        <td align="right" width="6">

                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                    </tr>
                                                                </table>

                                                                <asp:DataGrid ID="DataGrid8" runat="server" AutoGenerateColumns="False"
                                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                                    ShowHeader="False"
                                                                    Width="100%">

                                                                    <Columns>

                                                                        <asp:BoundColumn DataField="POID" HeaderText="Number">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:HyperLinkColumn DataNavigateUrlField="POID" DataNavigateUrlFormatString="TTGoodsPurchaseOrderView.aspx?POID={0}"
                                                                            DataTextField="GPOName" HeaderText="Name" Target="_blank">

                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                        </asp:HyperLinkColumn>

                                                                        <asp:BoundColumn DataField="Amount" HeaderText="总金额">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="PurTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="采购时间">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:HyperLinkColumn DataNavigateUrlField="PurManCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                            DataTextField="PurManName" HeaderText="采购员" Target="_blank">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                        </asp:HyperLinkColumn>

                                                                        <asp:TemplateColumn HeaderText="Status">

                                                                            <ItemTemplate>

                                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                            </ItemTemplate>

                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                        </asp:TemplateColumn>
                                                                    </Columns>

                                                                    <EditItemStyle BackColor="#2461BF" />

                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                                                                    <ItemStyle CssClass="itemStyle" />

                                                                    <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />

                                                                    <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                </asp:DataGrid></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                            <cc1:TabPanel ID="TabPanel6" runat="server" HeaderText=" 关联订单" TabIndex="0">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label67" runat="server" Text="<%$ Resources:lang,ZiChanCaiGouDingDan%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <table width="100%" cellpadding="0" cellspacing="0">

                                                        <tr>

                                                            <td valign="top" align="left">

                                                                <table background="ImagesSkin/main_n_bj.jpg" border="0" cellpadding="0" cellspacing="0"
                                                                    width="100%">

                                                                    <tr>

                                                                        <td width="7">

                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                                        <td>

                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">

                                                                                <tr>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label68" runat="server" Text="<%$ Resources:lang,DanHao%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="25%"><strong>

                                                                                        <asp:Label ID="Label69" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="15%"><strong>

                                                                                        <asp:Label ID="Label70" runat="server" Text="<%$ Resources:lang,ZongJinE%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="20%"><strong>

                                                                                        <asp:Label ID="Label71" runat="server" Text="<%$ Resources:lang,CaiGouShiJian%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label72" runat="server" Text="<%$ Resources:lang,CaiGouYuan%>"></asp:Label></strong> </td>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label73" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>

                                                                        <td align="right" width="6">

                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                    </tr>
                                                                </table>

                                                                <asp:DataGrid ID="DataGrid10" runat="server" AutoGenerateColumns="False"
                                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px"
                                                                    ShowHeader="False"
                                                                    Width="100%">

                                                                    <Columns>

                                                                        <asp:BoundColumn DataField="POID" HeaderText="Number">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:HyperLinkColumn DataNavigateUrlField="POID" DataNavigateUrlFormatString="TTAssetPurchaseOrderView.aspx?POID={0}"
                                                                            DataTextField="POName" HeaderText="Name" Target="_blank">

                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="25%" />
                                                                        </asp:HyperLinkColumn>

                                                                        <asp:BoundColumn DataField="Amount" HeaderText="总金额">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="PurTime" DataFormatString="{0:yyyy/MM/dd}" HeaderText="采购时间">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:HyperLinkColumn DataNavigateUrlField="PurManCode" DataNavigateUrlFormatString="TTUserInforSimple.aspx?UserCode={0}"
                                                                            DataTextField="PurManName" HeaderText="采购员" Target="_blank">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                        </asp:HyperLinkColumn>

                                                                        <asp:TemplateColumn HeaderText="Status">

                                                                            <ItemTemplate>

                                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                                            </ItemTemplate>

                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                        </asp:TemplateColumn>
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
                                                </ContentTemplate>
                                            </cc1:TabPanel>

                                            <cc1:TabPanel ID="TabPanel10" runat="server" HeaderText="关联物料">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,GuanLianLiaoPin%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <table align="left" cellpadding="0" cellspacing="0" width="100%">

                                                        <tr>

                                                            <td align="left">

                                                                <asp:Label ID="LB_GoodsOwner" runat="server" Font-Bold="True" Width="100%"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="padding-right: 5px;">
                                                                <asp:Button ID="BT_CreateGoodsList" runat="server" Text="<%$ Resources:lang,New %>" CssClass="inpuYello" OnClick="BT_CreateGoodsList_Click" />
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
                                                                                            <asp:Label ID="Label80" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                                    </td>
                                                                                    <td width="5%" align="left">
                                                                                        <strong>
                                                                                            <asp:Label ID="Label86" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                                    </td>

                                                                                    <td align="left" width="5%"><strong>
                                                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,BianHao %>"></asp:Label></strong></td>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label134" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong></td>

                                                                                    <td align="left" width="15%"><strong>

                                                                                        <asp:Label ID="Label135" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>

                                                                                    <td align="left" width="15%"><strong>

                                                                                        <asp:Label ID="Label139" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong></td>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label82" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong></td>

                                                                                    <td align="left" width="13%"><strong>

                                                                                        <asp:Label ID="Label136" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>

                                                                                    <td align="left" width="8%"><strong>

                                                                                        <asp:Label ID="Label137" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label></strong></td>

                                                                                    <td align="left" width="10%"><strong>

                                                                                        <asp:Label ID="Label138" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>

                                                                        <td align="right" width="6">

                                                                            <img alt="" height="26" src="ImagesSkin/main_n_r.jpg" width="6" /></td>
                                                                    </tr>
                                                                </table>

                                                                <asp:DataGrid ID="DataGrid12" runat="server" AutoGenerateColumns="False"
                                                                    CellPadding="4" ForeColor="#333333" GridLines="None" Height="1px" OnItemCommand="DataGrid12_ItemCommand"
                                                                    ShowHeader="False"
                                                                    Width="100%">

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

                                                                        <asp:BoundColumn DataField="GoodsCode" HeaderText="Code">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="GoodsName" HeaderText="Name">

                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="Spec" HeaderText="Specification">

                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="15%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="Brand" HeaderText="Brand">

                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="13%" />
                                                                        </asp:BoundColumn>

                                                                        <asp:BoundColumn DataField="Unit" HeaderText="Unit">

                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="8%" />
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
                                                                </asp:DataGrid></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </cc1:TabPanel>
                                        </cc1:TabContainer>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_New" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label127" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer layui-layer-iframe" id="popGoodsListWindow"
                        style="z-index: 9999; width: 980px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius: 10px;">
                        <div class="layui-layer-title" style="background: #e7e7e8;" id="popwindow_title2223">
                            <asp:Label ID="Label141" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content100" class="layui-layer-content" style="overflow: auto; padding: 0px 5px 0px 5px;">

                            <table align="left" cellpadding="0" cellspacing="0" width="100%">

                                <tr>

                                    <td width="60%">

                                        <table align="left" cellpadding="3" cellspacing="0" class="formBgStyle" width="100%">

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">

                                                    <asp:Label ID="Label143" runat="server" Text="<%$ Resources:lang,LeiXing %>"></asp:Label>： </td>

                                                <td colspan="5" class="formItemBgStyleForAlignLeft" >
                                                    <asp:DropDownList ID="DL_GoodsType" runat="server" DataTextField="Type"
                                                        DataValueField="Type">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="LB_ID" runat="server" Visible="false"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">

                                                    <asp:Label ID="Label144" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label>： </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:TextBox ID="TB_GoodsCode" runat="server" Height="20px" Width="95%"></asp:TextBox></td>

                                                <td class="formItemBgStyleForAlignLeft" style="width: 15%; ">

                                                    <asp:Label ID="Label145" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label>： </td>

                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >

                                                    <asp:TextBox ID="TB_GoodsName" runat="server" Height="20px" Width="99%"></asp:TextBox></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label146" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label>：</td>

                                                <td class="formItemBgStyleForAlignLeft" colspan="5" >

                                                    <asp:TextBox ID="TB_ModelNumber" runat="server" Height="20px" Width="99%"></asp:TextBox></td>
                                            </tr>

                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label147" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label>： </td>

                                                <td  colspan="5" class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Spec" runat="server" Height="40px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                                    <asp:Button ID="BT_FindGoods" runat="server" CssClass="inpu" OnClick="BT_FindGoods_Click" Text="<%$ Resources:lang,ChaXun %>" />
                                                    <asp:Button ID="BT_Clear" runat="server" CssClass="inpu" Text="<%$ Resources:lang,QingKong %>" OnClick="BT_Clear_Click" /></td>
                                            </tr>
                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label83" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>： </td>

                                                <td  colspan="5" class="formItemBgStyleForAlignLeft">
                                                    <asp:TextBox ID="TB_Brand" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label149" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label>： </td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Price" runat="server" OnBlur="" OnFocus="" OnKeyPress=""
                                                        PositiveColor="" Width="85px">0.00</NickLee:NumberBox></td>

                                                <td class="formItemBgStyleForAlignLeft">

                                                    <asp:Label ID="Label150" runat="server" Text="<%$ Resources:lang,DanWei %>"></asp:Label>： </td>

                                                <td colspan="3" class="formItemBgStyleForAlignLeft" >

                                                    <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName"
                                                        Width="64px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                        </table>
                                    </td>

                                    <td>

                                        <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer2" runat="server" ActiveTabIndex="0"
                                            Width="100%">

                                            <cc1:TabPanel ID="TabPanel11" runat="server">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label512" runat="server" Text="<%$ Resources:lang,LPKCLB%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <asp:Label ID="Label155" runat="server" Text="<%$ Resources:lang,QingXuanQuYaoCaiGouDeShangPin %>"></asp:Label>：

                                                                            <div id="Div4" style="width: 100%; height: 300px; overflow: auto;">

                                                                                <table width="300%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                                                    <tr>

                                                                                        <td width="7">

                                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                                                        <td>

                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                                                <tr>

                                                                                                    <td width="12%" align="left"><strong>

                                                                                                        <asp:Label ID="Label156" runat="server" Text="<%$ Resources:lang,DaiMa %>"></asp:Label></strong></td>

                                                                                                    <td width="12%" align="left"><strong>

                                                                                                        <asp:Label ID="Label157" runat="server" Text="<%$ Resources:lang,MingCheng %>"></asp:Label></strong></td>

                                                                                                    <td width="10%" align="left"><strong>

                                                                                                        <asp:Label ID="Label158" runat="server" Text="<%$ Resources:lang,XingHao %>"></asp:Label></strong></td>

                                                                                                    <td width="19%" align="left"><strong>

                                                                                                        <asp:Label ID="Label159" runat="server" Text="<%$ Resources:lang,GuiGe %>"></asp:Label></strong></td>

                                                                                                    <td width="10%" align="left"><strong>

                                                                                                        <asp:Label ID="Label160" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong></td>

                                                                                                    <td align="left"><strong>

                                                                                                        <asp:Label ID="Label161" runat="server" Text="<%$ Resources:lang,ShuLiang %>"></asp:Label></strong></td>

                                                                                                    <td align="left"><strong>

                                                                                                        <asp:Label ID="Label162" runat="server" Text="<%$ Resources:lang,DanJia %>"></asp:Label></strong></td>

                                                                                                    <td align="left"><strong>

                                                                                                        <asp:Label ID="Label164" runat="server" Text="<%$ Resources:lang,DiZhi %>"></asp:Label></strong></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>

                                                                                        <td width="6" align="right">

                                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                    </tr>
                                                                                </table>

                                                                                <asp:DataGrid ID="DataGrid13" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                                    Height="1px" Width="300%" OnItemCommand="DataGrid13_ItemCommand" CellPadding="4"
                                                                                    ForeColor="#333333" GridLines="None">

                                                                                    <Columns>

                                                                                        <asp:BoundColumn DataField="ID" HeaderText="Number" Visible="False">

                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                                        </asp:BoundColumn>

                                                                                        <asp:TemplateColumn HeaderText="Code">

                                                                                            <ItemTemplate>

                                                                                                <asp:Button ID="BT_GoodsCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"GoodsCode").ToString().Trim() %>' />
                                                                                            </ItemTemplate>

                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                                        </asp:TemplateColumn>

                                                                                        <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="TTGoodsInforView.aspx?GoodsID={0}"
                                                                                            DataTextField="GoodsName" HeaderText="Name" Target="_blank">

                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                                                        </asp:HyperLinkColumn>

                                                                                        <asp:BoundColumn DataField="ModelNumber" HeaderText="Model">

                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                        </asp:BoundColumn>

                                                                                        <asp:BoundColumn DataField="Spec" HeaderText="Specification">

                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="19%" />
                                                                                        </asp:BoundColumn>

                                                                                        <asp:BoundColumn DataField="Manufacturer" HeaderText="厂家">

                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                        </asp:BoundColumn>

                                                                                        <asp:BoundColumn DataField="Number" HeaderText="Quantity">

                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                                        </asp:BoundColumn>

                                                                                        <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">

                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                                                        </asp:BoundColumn>

                                                                                        <asp:BoundColumn DataField="Position" HeaderText="地址">

                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
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

                                            <cc1:TabPanel ID="TabPanel12" runat="server">

                                                <HeaderTemplate>

                                                    <asp:Label ID="Label513" runat="server" Text="<%$ Resources:lang,LPCXLB%>"></asp:Label>
                                                </HeaderTemplate>

                                                <ContentTemplate>

                                                    <asp:Label ID="Label165" runat="server" Text="<%$ Resources:lang,QingXuanQuShangPin%>"></asp:Label>：

                                                                            <div id="Div5" style="width: 100%; height: 300px; overflow: auto;">

                                                                                <table width="200%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">

                                                                                    <tr>

                                                                                        <td width="7">

                                                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>

                                                                                        <td>

                                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                                                                <tr>

                                                                                                    <td width="15%" align="left"><strong>

                                                                                                        <asp:Label ID="Label166" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong></td>

                                                                                                    <td width="20%" align="left"><strong>

                                                                                                        <asp:Label ID="Label167" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong></td>

                                                                                                    <td width="10%" align="left"><strong>

                                                                                                        <asp:Label ID="Label168" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label></strong></td>

                                                                                                    <td width="35%" align="left"><strong>

                                                                                                        <asp:Label ID="Label169" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong></td>

                                                                                                    <td width="20%" align="left"><strong>

                                                                                                        <asp:Label ID="Label170" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>

                                                                                        <td width="6" align="right">

                                                                                            <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                    </tr>
                                                                                </table>

                                                                                <asp:DataGrid ID="DataGrid14" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                                                    Height="1px" Width="200%" OnItemCommand="DataGrid14_ItemCommand" CellPadding="4"
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
                                        </cc1:TabContainer></td>
                                </tr>
                            </table>
                        </div>

                        <div id="popwindow_footer0001" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="LinkButton7" runat="server" class="layui-layer-btn notTab" OnClick="BT_SaveGoods_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label148" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>

                    <div class="layui-layer-shade" id="popwindow_shade" style="z-index: 9998; background-color: #000; opacity: 0.3; filter: alpha(opacity=30); display: none;"></div>


                    <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" Style="display: none;">
                        <div class="modalPopup-text" style="width: 420px; height: 400px; overflow: auto;">
                            <table>
                                <tr>
                                    <td style="width: 360px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:TreeView ID="TreeView3" runat="server" NodeWrap="True" OnSelectedNodeChanged="TreeView3_SelectedNodeChanged"
                                            ShowLines="True" Width="220px">
                                            <RootNodeStyle CssClass="rootNode" />
                                            <NodeStyle CssClass="treeNode" />
                                            <LeafNodeStyle CssClass="leafNode" />
                                            <SelectedNodeStyle CssClass="selectNode" ForeColor="Red" />
                                        </asp:TreeView>
                                    </td>
                                    <td style="width: 60px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        <asp:ImageButton ID="IMBT_CloseTree3" ImageUrl="ImagesSkin/Close4.jpg" runat="server" />
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
