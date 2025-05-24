<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTProjectGoodsApplicationWL.aspx.cs"
    Inherits="TTProjectGoodsApplicationWL" %>

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
    <script type="text/javascript">

        var disPostion = 0;

        function SaveScroll() {
            disPostion = GoodsListDivID.scrollTop;
        }

        function RestoreScroll() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function EndRequestHandler(sender, args) {
            GoodsListDivID.scrollTop = disPostion;
        }
    </script>

    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,ZiChanLingYongShenQing%>"></asp:Label>
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
                                <td>
                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="middle" style="width: 60%; border-right: solid 1px #D8D8D8; vertical-align: middle; padding: 5px 5px 5px 5px;">
                                                <table class="formBgStyle" cellpadding="3" cellspacing="0" style="width: 98%; margin-top: 5px"
                                                    align="left">
                                                    <tr>
                                                        <td  width="15%" class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>��
                                                        </td>
                                                        <td  style="width: 35%" class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="LB_AAID" runat="server"></asp:Label>
                                                        </td>
                                                        <td  width="20%" class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>��
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DL_Type" runat="server" DataTextField="Type" DataValueField="Type"
                                                                Height="20px" Width="124px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,ShenQingMingCheng%>"></asp:Label>��
                                                        </td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_AAName" runat="server" Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShenQingYuanYin%>"></asp:Label>��
                                                        </td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_ApplyReason" runat="server" Height="70px" TextMode="MultiLine"
                                                                Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignRight">
                                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label>��
                                                        </td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:TextBox ID="TB_ApplicantCode" runat="server" Width="96px"></asp:TextBox>
                                                            <asp:Label ID="LB_ApplicantName" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft">
                                                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label>��
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">

                                                            <asp:TextBox ID="DLC_ApplyTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender4" runat="server" TargetControlID="DLC_ApplyTime">
                                                            </ajaxToolkit:CalendarExtender>

                                                        </td>
                                                        <td class="formItemBgStyleForAlignRight">
                                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,YaoQiuWanChengShiJian%>"></asp:Label>��
                                                        </td>
                                                        <td class="formItemBgStyleForAlignLeft">

                                                            <asp:TextBox ID="DLC_FinishTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_FinishTime">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignRight">
                                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>��
                                                        </td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:DropDownList ID="DL_Status" runat="server" OnSelectedIndexChanged="DL_Status_SelectedIndexChanged">
                                                                <asp:ListItem Value="New" Text="<%$ Resources:lang,XinJian%>" />
                                                                <asp:ListItem Value="InProgress" Text="<%$ Resources:lang,ShenPiZhong%>" />
                                                                <asp:ListItem Value="Completed" Text="<%$ Resources:lang,WanCheng%>" />
                                                                <asp:ListItem Value="Cancel" Text="<%$ Resources:lang,QuXiao%>" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formItemBgStyleForAlignLeft"></td>
                                                        <td colspan="3"  class="formItemBgStyleForAlignLeft">
                                                            <asp:Button ID="BT_NewAA" runat="server" OnClick="BT_NewClaim_Click" Text="<%$ Resources:lang,XinJian%>" CssClass="inpu" />&nbsp;
                                                        <asp:Button ID="BT_UpdateAA" runat="server" OnClick="BT_UpdateClaim_Click" Text="<%$ Resources:lang,BaoCun%>"
                                                            CssClass="inpu" />&nbsp;
                                                        <asp:Button ID="BT_DeleteAA" runat="server" OnClick="BT_DeleteClaim_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())" Text="<%$ Resources:lang,ShanChu%>"
                                                            CssClass="inpu" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                            <td style="width: 40%; border-right: solid 1px #D8D8D8; padding: 5px 5px 5px 5px;"
                                                valign="top">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                    <tr>
                                                        <td width="7">
                                                            <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                        </td>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td width="20%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="40%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,ShenQingMingCheng%>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="20%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong>
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
                                                    Width="100%" AllowPaging="True" PageSize="6" OnPageIndexChanged="DataGrid1_PageIndexChanged"
                                                    ShowHeader="false" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Number">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                            <ItemTemplate>
                                                                <asp:Button ID="BT_AAID" runat="server" CssClass="inpu" Style="text-align: center"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"AAID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="AAName" HeaderText="��������">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="40%" />
                                                        </asp:BoundColumn>
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
                                                <asp:Label ID="LB_Sql" runat="server" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; border-right: solid 1px #D8D8D8;" valign="top">
                                                <table width="98%" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" colspan="2" style="height: 6px">
                                                            <cc1:TabContainer CssClass="ajax_tab_menu" ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                                                Width="100%">
                                                                <cc1:TabPanel ID="TabPanel1" runat="server">

                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="Label47" runat="server" Text="<%$ Resources:lang,ShenQingDanMingXi%>"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                                                                        <tr>
                                                                                            <td width="7">
                                                                                                <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" /></td>
                                                                                            <td>
                                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td width="13%" align="left"><strong>
                                                                                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> </td>
                                                                                                        <td width="15%" align="left"><strong>
                                                                                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,ZiChanDaiMa%>"></asp:Label></strong> </td>
                                                                                                        <td width="20%" align="left"><strong>
                                                                                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ZiChanMing%>"></asp:Label></strong> </td>
                                                                                                        <td width="32%" align="left"><strong>
                                                                                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong> </td>
                                                                                                        <td width="10%" align="left"><strong>
                                                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong> </td>
                                                                                                        <td width="10%" align="left"><strong>
                                                                                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label></strong> </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                            <td width="6" align="right">
                                                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:DataGrid ID="DataGrid2" runat="server" AutoGenerateColumns="False" Height="30px"
                                                                                        OnItemCommand="DataGrid2_ItemCommand" Width="100%" CellPadding="4" ShowHeader="False"
                                                                                        ForeColor="#333333" GridLines="None">

                                                                                        <Columns>
                                                                                            <asp:TemplateColumn>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Button ID="BT_ID" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>' />
                                                                                                </ItemTemplate>

                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="13%" />
                                                                                            </asp:TemplateColumn>
                                                                                            <asp:BoundColumn DataField="GoodsCode" HeaderText="�ʲ�����">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="15%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="GoodsName" HeaderText="�ʲ���">
                                                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="Spec" HeaderText="Specification">
                                                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="32%" />
                                                                                            </asp:BoundColumn>
                                                                                            <asp:BoundColumn DataField="Number" HeaderText="Quantity">
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
                                                                                    <asp:Label ID="LB_ID" runat="server" Visible="False"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table class="formBgStyle" style="width: 100%;" cellpadding="3" cellspacing="0">
                                                                                        <tr>
                                                                                            <td style="width: 14%; " class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label>�� </td>
                                                                                            <td style="height: 19px;" class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="LB_DetailID" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,ZiChanDaiMa%>"></asp:Label>�� </td>
                                                                                            <td class="formItemBgStyleForAlignLeft" style="height: 19px;">
                                                                                                <asp:TextBox ID="TB_GoodsCode" runat="server" Width="80px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ZiChanMingCheng%>"></asp:Label>�� </td>
                                                                                            <td style="height: 19px;" class="formItemBgStyleForAlignLeft">
                                                                                                <asp:TextBox ID="TB_GoodsName" runat="server" Width="80%"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label>�� </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:TextBox ID="TB_ModelNumber" runat="server" Width="90%"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label>�� </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:TextBox ID="TB_Spec" runat="server" Width="99%" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                                                                                <asp:Button ID="BT_FindGoods" runat="server" CssClass="inpu" OnClick="BT_FindGoods_Click" Text="<%$ Resources:lang,ChaXun%>" />
                                                                                                &#160;<asp:Button ID="BT_Clear" runat="server" CssClass="inpu" OnClick="BT_Clear_Click" Text="<%$ Resources:lang,QingKong%>" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>

                                                                                            <td class="formItemBgStyleForAlignLeft">

                                                                                                <asp:Label ID="Label42" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label>��
                                                                                            </td>

                                                                                            <td class="formItemBgStyleForAlignLeft">

                                                                                                <asp:TextBox ID="TB_Brand" runat="server" Width="90%"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label>�� </td>
                                                                                            <td style="width: 150px; " class="formItemBgStyleForAlignLeft">
                                                                                                <NickLee:NumberBox MaxAmount="1000000000000" MinAmount="-1000000000000" ID="NB_Number" runat="server" Width="85px" OnBlur="" OnFocus=""
                                                                                                    OnKeyPress="" PositiveColor="">0.00</NickLee:NumberBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,DanWei%>"></asp:Label>�� </td>
                                                                                            <td style="width: 150px; " class="formItemBgStyleForAlignLeft">
                                                                                                <asp:DropDownList ID="DL_Unit" runat="server" DataTextField="UnitName" DataValueField="UnitName"
                                                                                                    Height="25px" Width="80px" AutoPostBack="True">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,ShengChanChangJia%>"></asp:Label>�� </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:TextBox ID="TB_Manufacturer" runat="server" Width="90%"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft">IP�� </td>
                                                                                            <td class="formItemBgStyleForAlignLeft">
                                                                                                <asp:TextBox ID="TB_IP" runat="server" Width="98%"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="formItemBgStyleForAlignLeft"></td>
                                                                                            <td style="height: 1px; "  class="formItemBgStyleForAlignLeft"
                                                                                                <asp:Button ID="BT_New" runat="server" Enabled="False" OnClick="BT_New_Click" Text="<%$ Resources:lang,XinZeng%>"
                                                                                                    CssClass="inpu" />
                                                                                                &#160;&#160;<asp:Button ID="BT_Update" runat="server" Enabled="False" OnClick="BT_Update_Click"
                                                                                                    CssClass="inpu" Text="<%$ Resources:lang,BaoCun%>" />
                                                                                                &#160;&#160;<asp:Button ID="BT_Delete" runat="server" Enabled="False" OnClick="BT_Delete_Click" OnClientClick="return confirm(getDeleteMsgByLangCode())"
                                                                                                    CssClass="inpu" Text="<%$ Resources:lang,ShanChu%>" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <br />
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </cc1:TabPanel>
                                                                <cc1:TabPanel ID="TabPanel2" runat="server">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="Label88" runat="server" Text="<%$ Resources:lang,GongZuoLiuDingYi%>"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left" style="width: 100px; height: 6px; text-align: right;">
                                                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,LiuChengMuBan%>"></asp:Label>�� </td>
                                                                                <td align="left" style="width: 550px; height: 6px">
                                                                                    <asp:DropDownList ID="DL_TemName" runat="server" DataTextField="TemName" DataValueField="TemName"
                                                                                        Height="25px" Width="194px">
                                                                                    </asp:DropDownList><asp:Label ID="Label36" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>��
                                                                                    <asp:DropDownList ID="DL_WFType" runat="server">
                                                                                        <asp:ListItem Value="MaterialWithdrawal" Text="<%$ Resources:lang,LiaoPingLingYong%>" />
                                                                                    </asp:DropDownList><asp:HyperLink
                                                                                        ID="HL_WLTem" runat="server" NavigateUrl="~/TTWorkFlowTemplate.aspx" Target="_blank">
                                                                                        <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,MuBanWeiHu%>"></asp:Label>
                                                                                    </asp:HyperLink><asp:Button
                                                                                        ID="BT_Reflash" runat="server" OnClick="BT_Reflash_Click" Text="<%$ Resources:lang,ShuaXin%>" CssClass="inpu" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" colspan="2" style="height: 6px; text-align: right;">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="left" style="width: 550px; height: 27px"><span style="font-size: 10pt">��<asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,YaoQiuShouDaoXinXi%>"></asp:Label>��</span><asp:CheckBox ID="CB_SMS" runat="server"
                                                                                                Font-Size="10pt" Text="<%$ Resources:lang,DuanXin%>" /><asp:CheckBox ID="CB_Mail" runat="server"
                                                                                                    Text="<%$ Resources:lang,YouJian%>" /><span style="font-size: 10pt">) </span>
                                                                                                <asp:Button ID="BT_SubmitApply" runat="server" Enabled="False" Text="<%$ Resources:lang,TiJiaoShenQing%>" CssClass="inpu" /><cc1:ModalPopupExtender
                                                                                                    ID="BT_SubmitApply_ModalPopupExtender" runat="server" Enabled="True" TargetControlID="BT_SubmitApply"
                                                                                                    PopupControlID="Panel1" BackgroundCssClass="modalBackground" Y="150" DynamicServicePath="">
                                                                                                </cc1:ModalPopupExtender>
                                                                                                <asp:Label ID="LB_UserCode" runat="server" Visible="False"></asp:Label><asp:Label
                                                                                                    ID="LB_UserName" runat="server" Visible="False"></asp:Label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" style="height: 22px; text-align: left;">
                                                                                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,DuiYingGongZuoLiuLieBiao%>"></asp:Label>�� </td>
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
                                                                                                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> </td>
                                                                                                                    <td width="50%" align="left"><strong>
                                                                                                                        <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong> </td>
                                                                                                                    <td width="20%" align="left"><strong>
                                                                                                                        <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong> </td>
                                                                                                                    <td width="10%" align="left"><strong>
                                                                                                                        <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> </td>
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
                                                                                                        <asp:BoundColumn DataField="CreateTime" HeaderText="����ʱ��">
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="20%" />
                                                                                                        </asp:BoundColumn>
                                                                                                        <asp:TemplateColumn HeaderText="Status">
                                                                                                            <ItemTemplate>
                                                                                                                <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                                                                        </asp:TemplateColumn>
                                                                                                        <asp:TemplateColumn>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=WorkFlow&WLID={0}") %>'
                                                                                                                    Target="_blank"><img  class="noBorder" src="ImagesSkin/Doc.gif"/></asp:HyperLink>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                                                                        </asp:TemplateColumn>
                                                                                                    </Columns>
                                                                                                </asp:DataGrid>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </cc1:TabPanel>
                                                            </cc1:TabContainer>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 40%; border-right: solid 1px #D8D8D8; padding: 5px 5px 5px 5px;"
                                                valign="top" align="left">
                                                <asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,QXQYLYDZC%>"></asp:Label>��<asp:Label ID="LB_Sql3" runat="server" Visible="False"></asp:Label>
                                                <div id="GoodsListDivID" style="width: 100%; height: 300px; overflow: auto;">
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
                                                                                <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,DaiMa%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="12%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,MingCheng%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="10%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label40" runat="server" Text="<%$ Resources:lang,XingHao%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="24%" align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label41" runat="server" Text="<%$ Resources:lang,GuiGe%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td width="10%" align="left"><strong>
                                                                            <asp:Label ID="Label215" runat="server" Text="<%$ Resources:lang,PinPai %>"></asp:Label></strong> </td>
                                                                        <td align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label43" runat="server" Text="<%$ Resources:lang,ShuLiang%>"></asp:Label></strong>
                                                                        </td>
                                                                        <td  align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label45" runat="server" Text="<%$ Resources:lang,DanJia%>"></asp:Label></strong>
                                                                        </td>

                                                                        <td align="left">
                                                                            <strong>
                                                                                <asp:Label ID="Label46" runat="server" Text="<%$ Resources:lang,DiZhi%>"></asp:Label></strong>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="6" align="right">
                                                                <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DataGrid ID="DataGrid3" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                                        Height="1px" Width="150%" OnItemCommand="DataGrid3_ItemCommand" CellPadding="4"
                                                        ForeColor="#333333" GridLines="None">
                                                        <Columns>
                                                            <asp:BoundColumn DataField="ID" HeaderText="Number" Visible="False">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                            </asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Code">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BT_GoodsCode" runat="server" CssClass="inpu" Text='<%# DataBinder.Eval(Container.DataItem,"GoodsCode") %>' />
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
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="24%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Manufacturer" HeaderText="����">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Number" HeaderText="Quantity">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Price" HeaderText="UnitPrice">
                                                                <ItemStyle CssClass="itemBorder" Horizontalalign="left"/>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Position" HeaderText="��ַ">
                                                                <ItemStyle CssClass="itemBorder" HorizontalAlign="Left"/>
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <ItemStyle CssClass="itemStyle" />
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditItemStyle BackColor="#2461BF" />
                                                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle Horizontalalign="center" Mode="NumericPages" NextPageText="" PrevPageText="" CssClass="notTab" />
                                                    </asp:DataGrid>
                                                </div>
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
                                        <asp:Label ID="Label35" runat="server" Text="<%$ Resources:lang,LCSQSCHYLJDLCJHYMQJHM%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 420px; padding: 5px 5px 5px 5px;" valign="top" align="left">
                                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="BT_ActiveYes" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Shi%>" OnClick="BT_ActiveYes_Click" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                                            ID="BT_ActiveNo" runat="server" CssClass="inpu" Text="<%$ Resources:lang,Fou%>" OnClick="BT_ActiveNo_Click" />
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
