<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TTCarApplyForm.aspx.cs" Inherits="TTCarApplyForm" %>

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
            min-width: 1150px;
            width: expression (document.body.clientWidth <= 1150? "1150px" : "auto" ));
        }
    </style>
    <script type="text/javascript" src="js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js/allAHandler.js"></script>
    <script type="text/javascript" src="js/layer/layer/layer.js"></script>
    <script type="text/javascript" src="js/popwindow.js"></script>
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
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang,CheLiangShenQing%>"></asp:Label>
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
                                    <table style="width: 98%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right" style="padding: 5px 5px 0px 5px;">
                                                <asp:Button ID="BT_Create" runat="server" Text="<%$ Resources:lang,New%>" CssClass="inpuYello" OnClick="BT_Create_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="padding: 5px 5px 5px 5px;">

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
                                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lang,BianJi %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label38" runat="server" Text="<%$ Resources:lang,ShanChu %>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                       <strong>
                                                                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:lang,FaQiShengQing%>" /></strong>
                                                                    </td>
                                                                    <td width="5%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="LB_DGProjectID" runat="server" Text="<%$ Resources:lang,BianHao %>" /></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:lang,YongCheBuMen %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="10%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:lang,ShenQingRen %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources:lang,YongCheShiJian %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="12%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:lang,HuanCheShiJian %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="16%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources:lang,SuiCheRen %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td width="16%" align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label25" runat="server" Text="<%$ Resources:lang,ShenQingYuanYin %>"></asp:Label></strong>
                                                                    </td>
                                                                    <td align="left">
                                                                        <strong>
                                                                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:lang,ZhuangTai %>"></asp:Label></strong>
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
                                                    Height="1px" OnItemCommand="DataGrid2_ItemCommand" ShowHeader="False" PageSize="25"
                                                    OnPageIndexChanged="DataGrid2_PageIndexChanged" Width="100%" CellPadding="4"
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
                                                        <asp:ButtonColumn ButtonType="LinkButton" CommandName="Assign" Text="&lt;div&gt;&lt;img src=ImagesSkin/Assign.png border=0 alt='Deleted' /&gt;&lt;/div&gt;">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:ButtonColumn>
                                                        <asp:BoundColumn DataField="ID" HeaderText="ID">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="5%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DepartName" HeaderText="用车部门">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ApplicantName" HeaderText="Applicant">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="DepartTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="用车时间">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="BackTime" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="还车时间">
                                                            <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="12%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Attendant" HeaderText="随车人">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="16%" />
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ApplyReason" HeaderText="申请原因">
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="16%" />
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Status">
                                                            <ItemTemplate>
                                                                <%# ShareClass. GetStatusHomeNameByOtherStatus(Eval("Status").ToString()) %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" />
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
                    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none;"
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


                    <div class="layui-layer layui-layer-iframe" id="popwindow"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius:10px;">
                        <div class="layui-layer-title"  style="background:#e7e7e8;" id="popwindow_title">
                            <asp:Label ID="Label36" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content" class="layui-layer-content" style="overflow: auto; padding :0px 5px 0px 5px;">

                            <table class="formBgStyle" style="width: 100%;" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 15%">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lang,CheXing%>"></asp:Label>：
                                    </td>
                                    <td colspan ="3"  class="formItemBgStyleForAlignLeft" >
                                        <asp:DropDownList ID="DL_CarType" runat="server" DataTextField="Type" DataValueField="Type">
                                        </asp:DropDownList>
                                        <asp:Label ID="LB_ID" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lang,YongCheBuMen%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_DepartCode" Width="80px" runat="server"></asp:TextBox>&nbsp;
                                                        <asp:Label ID="LB_DepartName" runat="server"></asp:Label>
                                        <cc1:ModalPopupExtender ID="TB_DepartCode_ModalPopupExtender" runat="server" Enabled="True"
                                            TargetControlID="TB_DepartCode" PopupControlID="Panel1" CancelControlID="IMBT_Close"
                                            BackgroundCssClass="modalBackground" Y="150">
                                        </cc1:ModalPopupExtender>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:lang,ShenQingRen%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="LB_UserCode" runat="server"></asp:Label>
                                        &nbsp;
                                                        <asp:Label ID="LB_UserName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="width: 10%" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:lang,YongCheShiJian%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="DLC_DepartTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender3" runat="server" TargetControlID="DLC_DepartTime">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DL_DepartBeginHour" runat="server" CssClass="shuru">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:lang,Shi%>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DL_DepartBeginMinute" runat="server" CssClass="shuru">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:lang,Fen%>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft" style="width: 10%;">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:lang,HuanCheShiJian%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                        <table>
                                            <tr>
                                                <td>

                                                    <asp:TextBox ID="DLC_BackTime" ReadOnly="false" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender Format="yyyy-MM-dd" ID="CalendarExtender1" runat="server" TargetControlID="DLC_BackTime">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DL_BackBeginHour" runat="server" CssClass="shuru">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:lang,Shi%>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DL_BackBeginMinute" runat="server" CssClass="shuru">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:lang,Fen%>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:lang,ShangCheDiDian%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_BoardingSite" runat="server" Width="90%"></asp:TextBox>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:lang,MuDiDi%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:TextBox ID="TB_Destination" runat="server" Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:lang,SuiCheRen%>"></asp:Label>：
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" colspan="3">
                                        <asp:TextBox ID="TB_Attendant" runat="server" Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 30px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:lang,ShenQingYuanYin%>"></asp:Label>：
                                    </td>
                                    <td  style="height: 30px" class="formItemBgStyleForAlignLeft" colspan="3">
                                        <asp:TextBox ID="TB_ApplyReason" runat="server" Height="69px" TextMode="MultiLine"
                                            Width="90%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="height: 6px" class="formItemBgStyleForAlignLeft">
                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label>：
                                    </td>
                                    <td  style="height: 6px" class="formItemBgStyleForAlignLeft">
                                        <asp:DropDownList ID="DL_Status" runat="server" OnSelectedIndexChanged="DL_Status_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem Value="New" Text="<%$ Resources:lang,XinJian%>" />
                                            <asp:ListItem Value="InProgress" Text="<%$ Resources:lang,ShenPiZhong%>" />
                                            <asp:ListItem Value="Passed" Text="<%$ Resources:lang,TongGuo%>" />
                                            <asp:ListItem Value="Completed" Text="<%$ Resources:lang,WanCheng%>" />
                                            <asp:ListItem Value="Cancel" Text="<%$ Resources:lang,QuXiao%>" />
                                        </asp:DropDownList>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 6px">&nbsp;
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft" style="height: 6px">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formItemBgStyleForAlignLeft"></td>
                                    <td class="formItemBgStyleForAlignLeft">

                                        <asp:Label ID="LB_Sql" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">&nbsp;
                                    </td>
                                    <td class="formItemBgStyleForAlignLeft">&nbsp;
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div id="popwindow_footer" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <asp:LinkButton ID="BT_New" runat="server" class="layui-layer-btn notTab" OnClick="BT_New_Click" Text="<%$ Resources:lang,BaoCun%>"></asp:LinkButton><a class="layui-layer-btn notTab" onclick="return popClose();"><asp:Label ID="Label37" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
                        </div>
                        <span class="layui-layer-setwin"><a onclick="return popClose();" class="layui-layer-ico layui-layer-close layui-layer-close1 notTab" href="javascript:;"></a></span>
                    </div>


                    <div class="layui-layer layui-layer-iframe" id="popWFApplyWindow" name="noConfirm"
                        style="z-index: 9999; width: 900px; height: 530px; position: absolute; overflow: hidden; display: none; border-radius:10px;">
                        <div class="layui-layer-title"  style="background:#e7e7e8;" id="popwindow_title1">
                            <asp:Label ID="Label18" runat="server" Text="&lt;div&gt;&lt;img src=ImagesSkin/Update.png border=0 width=30px height=30px alt='BusinessForm' /&gt;&lt;/div&gt;"></asp:Label>
                        </div>
                        <div id="popwindow_content1" class="layui-layer-content" style="overflow: auto; padding :0px 5px 0px 5px;">

                            <table width="98%" cellpadding="4" cellspacing="0">
                                <tr>
                                    <td align="right" style="width: 100px; height: 27px">
                                        <asp:Label ID="Label26" runat="server" Text="<%$ Resources:lang,LiuChengMuBan%>"></asp:Label>：
                                    </td>
                                    <td align="left" style="width: 650px; height: 27px">
                                        <asp:DropDownList ID="DL_TemName" runat="server" DataTextField="TemName" DataValueField="TemName"
                                            Width="228px">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label27" runat="server" Text="<%$ Resources:lang,LeiXing%>"></asp:Label>： 
                                        <asp:DropDownList ID="DL_WFType" runat="server">
                                            <asp:ListItem Value="VehicleRequest" Text="<%$ Resources:lang,CheLiangShenQing%>" />
                                        </asp:DropDownList><asp:HyperLink
                                            ID="HL_WLTem" runat="server" NavigateUrl="~/TTWorkFlowTemplate.aspx" Target="_blank">
                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:lang,MuBanWeiHu%>"></asp:Label>
                                        </asp:HyperLink><asp:Button
                                            ID="BT_Reflash" runat="server" OnClick="BT_Reflash_Click" Text="<%$ Resources:lang,ShuaXin%>" CssClass="inpu" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 100px; height: 27px"></td>
                                    <td align="left" style="width: 650px; height: 27px">
                                        <span style="font-size: 10pt"></span>
                                        <asp:CheckBox ID="CB_SMS" runat="server" Text="<%$ Resources:lang,DuanXin%>" /><asp:CheckBox
                                            ID="CB_Mail" runat="server" Font-Size="10pt" Text="<%$ Resources:lang,YouJian%>" /><span style="font-size: 10pt"><asp:Label ID="Label29" runat="server" Text="<%$ Resources:lang,YaoQiuShouDaoXinXi%>"></asp:Label>
                                            </span>
                                        <asp:Button ID="BT_SubmitApply" runat="server" Enabled="False" Text="<%$ Resources:lang,TiJiaoShenQing%>" CssClass="inpu" />
                                        <cc1:ModalPopupExtender ID="BT_SubmitApply_ModalPopupExtender" runat="server" Enabled="True"
                                            TargetControlID="BT_SubmitApply" PopupControlID="Panel2" CancelControlID="IMBT_Close"
                                            BackgroundCssClass="modalBackground" Y="150" DynamicServicePath="">
                                        </cc1:ModalPopupExtender>
                                        <asp:Label ID="LB_RelatedType" runat="server" Visible="False"></asp:Label><asp:Label
                                            ID="LB_RelatedID" runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="height: 22px; text-align: left;">
                                        <asp:Label ID="Label30" runat="server" Text="<%$ Resources:lang,DuiYingGongZuoLiuLieBiao%>"></asp:Label>：
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" background="ImagesSkin/main_n_bj.jpg">
                                            <tr>
                                                <td width="7">
                                                    <img src="ImagesSkin/main_n_l.jpg" width="7" height="26" />
                                                </td>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:lang,BianHao%>"></asp:Label></strong> &nbsp;
                                                            </td>
                                                            <td width="50%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:lang,GongZuoLiu%>"></asp:Label></strong> &nbsp;
                                                            </td>
                                                            <td width="20%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:lang,ShenQingShiJian%>"></asp:Label></strong> &nbsp;
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong>
                                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:lang,ZhuangTai%>"></asp:Label></strong> &nbsp;
                                                            </td>
                                                            <td width="10%" align="left">
                                                                <strong></strong>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td width="6" align="right">
                                                    <img src="ImagesSkin/main_n_r.jpg" width="6" alt="" height="26" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:DataGrid ID="DataGrid4" runat="server" AutoGenerateColumns="False" Height="1px"
                                            ShowHeader="False" PageSize="5" Width="100%" CellPadding="4" ForeColor="#333333"
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
                                                        <%# ShareClass.GetStatusHomeNameByRequirementStatus(Eval("Status").ToString()) %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.WLID", "TTWLRelatedDoc.aspx?DocType=WorkFlow&WLID={0}") %>'
                                                            Target="_blank"><img src="ImagesSkin/Doc.gif" class="noBorder"/></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="itemBorder" Horizontalalign="left" Width="10%" />
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>

                        </div>

                        <div id="popwindow_footer1" class="layui-layer-btn" style="border-top: 1px solid #ccc;">
                            <a class="layui-layer-btn notTab" onclick="return popClose();">
                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:lang,GuanBi%>" /></a>
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
<script type="text/javascript" language="javascript">var cssDirectory = '<%=Session["CssDirectory"] %>'; var oLink = document.getElementById('mainCss'); oLink.href = 'css/' + cssDirectory + '/' + 'bluelightmain.css';</script></html>
